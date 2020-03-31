using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Interfaces;

namespace TwinSovet.Data.DataBase
{
    /// <summary>
    /// Репозиторий настроек приложения - объекты настроек хранятся в единственном экземпляре.
    /// </summary>
    public class DbObjectStorage : IObjectStorage 
    {
        private static readonly object Locker = new object();
        private static readonly object CacheLocker = new object();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<object, IEnumerable<string>> includedPropsCache = new Dictionary<object, IEnumerable<string>>();

        private readonly IDbContextFactory contextFactory;


        public DbObjectStorage(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }


        /// <summary>
        /// Возвращает объект настроек типа <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип объекта для сохранения.</typeparam>
        /// <returns>Результат с объектом настройки.</returns>
        public T GetSingleSimple<T>() where T : SimpleDbObject, new() 
        {
            return GetDbObjectSynchronized<T>(true);
        }

        /// <summary>
        /// Вернуть объект сложных настроек типа <see cref="TComplexObject"/>.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложных настроек.</typeparam>
        /// <returns>Результат получения объекта сложных настроек.</returns>
        public TComplexObject GetSingleComplex<TComplexObject>() where TComplexObject : ComplexDbObject, new() 
        {
            return GetDbObjectSynchronized<TComplexObject>(false);
        }

        /// <summary>
        /// Сохранить простые настройки - не содержащие пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TSimpleObject">Тип простого объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект простых настроек.</param>
        /// <returns>Результат сохранения.</returns>
        public void SaveSingleSimple<TSimpleObject>(TSimpleObject objectToSave) where TSimpleObject : SimpleDbObject, new()
        {
            try
            {
                SaveSimpleImpl(objectToSave);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    Console.WriteLine($"wtf > { ex.Message }");
                    // сообщение о последствиях тупого двойного инсерта, который EF пытался задолбить в базу.
                    // второй инсерт мы игнорили, так что в базе одна запись - всё хорошо
                    return;
                }

                logger.Error(ex, $"Не удалось сохранить объект типа '{ typeof(TSimpleObject).Name }'");

                throw;
            }
        }

        /// <summary>
        /// Сохранить настройки - не содержащие пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложного объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект сложных настроек.</param>
        /// <returns>Результат сохранения.</returns>
        public void SaveSingleComplex<TComplexObject>(TComplexObject objectToSave) where TComplexObject : ComplexDbObject, new()
        {
            try
            {
                //var efLogs = new List<string>();
                //RemoveCascade<TComplexObject>();

                DbContextBase<TComplexObject> complexContext = contextFactory.CreateContext<TComplexObject>();

                using (complexContext)
                {
                    //complexContext.Database.Log = efLogs.Add;
                    // для полного удаления сложного объекта нужно приложить к нему названия его дочерних пропертей (которым соответствуют отдельные таблицы)
                    DbQuery<TComplexObject> allObjectsQuery = complexContext.Objects.Include(objectToSave.IncludedPropertyNames);
                    foreach (string childPropName in objectToSave.IncludedChildPropNames)
                    {
                        allObjectsQuery.Include(childPropName);
                    }

                    List<TComplexObject> allObjects = allObjectsQuery.ToList();

                    complexContext.Objects.RemoveRange(allObjects);

                    DbEntityEntry<ComplexDbObject> entry = complexContext.Entry(objectToSave.PrepareMappedProps());
                    entry.State = EntityState.Added;

                    complexContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233079)
                {
                    // последствие тупого двойного инсерта, который EF пытался задолбить в базу.
                    // второй инсерт мы игнорили, так что в базе одна запись - всё хорошо
                    return;
                }

                throw;
            }
        }


        private void SaveSimpleImpl<TSimpleObject>(TSimpleObject objectToSave) where TSimpleObject : SimpleDbObject, new()
        {
            //var efLogs = new List<string>();
            using (var dbContext = new SimpleDbContext<TSimpleObject>())
            {
                //dbContext.Database.Log = efLogs.Add;

                var allObjects = dbContext.Objects.Cast<TSimpleObject>().ToArray();

                dbContext.Objects.RemoveRange(allObjects);

                var entry = dbContext.Entry(objectToSave);
                entry.State = EntityState.Added;

                dbContext.SaveChanges();
            }
        }

        private TObject GetDbObjectSynchronized<TObject>(bool isSimple) where TObject : DbObject, new() 
        {
            lock (Locker)
            {
                if (isSimple)
                {
                    return TryGetSimpleDbObject<TObject>();
                }

                return TryGetComplexDbObject<TObject>();
            }
        }

        private TObject TryGetSimpleDbObject<TObject>() where TObject : DbObject, new()
        {
            try
            {

                using (var dbContext = contextFactory.CreateContext<TObject>())
                {
                    //var logs = new List<string>();
                    //dbContext.Database.Log = logs.Add;

                    TObject foundProxy =
                        dbContext.Objects.Local.SingleOrDefault() ??
                        dbContext.Objects.SingleOrDefault();

                    return foundProxy;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить объект типа '{ typeof(TObject).Name }'");
                throw;
            }
        }

        private TObject TryGetComplexDbObject<TObject>() where TObject : DbObject, new() 
        {
            try
            {
                bool cached = false;
                ComplexDbObject complexProxy;

                using (DbContextBase<TObject> dbContext = contextFactory.CreateContext<TObject>())
                {
                    IEnumerable<string> cachedProps = GetCachedProps<TObject>();

                    TObject foundProxy;

                    if (cachedProps != null)
                    {
                        DbQuery<TObject> query = IncludeProps(dbContext, cachedProps);
                        foundProxy = query.SingleOrDefault();
                        complexProxy = foundProxy as ComplexDbObject;
                    }
                    else
                    {
                        foundProxy = dbContext.Objects.SingleOrDefault();
                        if (foundProxy == null)
                        {
                            string typeName = typeof(TObject).Name;
                            throw new InvalidOperationException("oooooo");
                        }
                        complexProxy = foundProxy as ComplexDbObject;
                        var includedProps = new List<string> { complexProxy.IncludedPropertyNames };
                        includedProps.AddRange(complexProxy.IncludedChildPropNames);
                        SaveCachedProps<TObject>(includedProps);
                        cached = true;
                    }
                }

                if (cached)
                {
                    return TryGetComplexDbObject<TObject>();
                }

                return complexProxy as TObject;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить объект типа '{ typeof(TObject).Name }'");
                throw;
            }
        }





        private IEnumerable<string> GetCachedProps<TObject>() where TObject : DbObject
        {
            lock (CacheLocker)
            {
                var cachedKey = includedPropsCache.Keys.FirstOrDefault(typeKey => typeKey is List<TObject>);
                if (cachedKey != null)
                {
                    return includedPropsCache[cachedKey];
                }

                return null;
            }
        }

        private void SaveCachedProps<TObject>(IEnumerable<string> props) where TObject : DbObject 
        {
            lock (CacheLocker)
            {
                var cachedKey = includedPropsCache.Keys.FirstOrDefault(typeKey => typeKey is List<TObject>);
                if (cachedKey != null)
                {
                    return;
                }

                includedPropsCache.Add(new List<TObject>(), props);
            }
        }

        private DbQuery<TObject> IncludeProps<TObject>(DbContextBase<TObject> context, IEnumerable<string> props) 
            where TObject : DbObject, new()
        {
            DbQuery<TObject> query = context.Objects;
            foreach (string cachedPropName in props)
            {
                query = query.Include(cachedPropName);
            }

            return query;
        }
    }
}