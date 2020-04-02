using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Common.Extensions;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Interfaces;

using NLog;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase 
{
    /// <summary>
    /// Реализация <see cref="IDbEndPoint"/>.
    /// </summary>
    public class DbEndPoint : IDbEndPoint 
    {
        private static readonly object Locker = new object();
        private static readonly object CacheLocker = new object();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<object, IEnumerable<string>> includedPropsCache = new Dictionary<object, IEnumerable<string>>();

        private readonly IDbContextFactory contextFactory;


        public DbEndPoint(IDbContextFactory contextFactory) 
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }


        public static void SetTimestamps(AttachmentModelBase noteModel) 
        {
            noteModel.ModificationTime = DateTime.Now;

            if (noteModel.CreationTime == null || noteModel.CreationTime == default(DateTime))
            {
                noteModel.CreationTime = noteModel.ModificationTime;
            }
        }


        public IEnumerable<T> GetSimpleObjects<T>(Func<T, bool> predicate) where T : SimpleDbObject, new() 
        {
            return GetDbObjectSynchronized<T>(predicate, true);
        }

        /// <summary>
        /// Получить сложные объекты из базы типа <see cref="TComplexObject"/>.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложных объектов.</typeparam>
        /// <returns>Результат получения объекта сложных настроек.</returns>
        public IEnumerable<TComplexObject> GetComplexObjects<TComplexObject>(Func<TComplexObject, bool> predicate) 
            where TComplexObject : ComplexDbObject, new() 
        {
            return GetDbObjectSynchronized<TComplexObject>(predicate, false);
        }

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

        public void SaveComplexObjects<TComplexObject>(IEnumerable<TComplexObject> objectsToSave) 
            where TComplexObject : ComplexDbObject, new()
        {
            objectsToSave.AssertNotNull(nameof(objectsToSave));
            if (!objectsToSave.Any()) return;

            try
            {
                //var efLogs = new List<string>();
                //RemoveCascade<TComplexObject>();

                DbContextBase<TComplexObject> context = contextFactory.CreateContext<TComplexObject>();

                using (context)
                {
                    //complexContext.Database.Log = efLogs.Add;
                    // для полного удаления сложного объекта нужно приложить к нему названия его дочерних пропертей (которым соответствуют отдельные таблицы)
                    TComplexObject firstObject = objectsToSave.First();
                    DbQuery<TComplexObject> allObjectsQuery = IncludeProps(context, new[] { firstObject.IncludedPropertyNames });
                    foreach (string childPropName in firstObject.IncludedChildPropNames)
                    {
                        allObjectsQuery.Include(childPropName);
                    }

                    //List<TComplexObject> allObjects = allObjectsQuery.ToList();
                    //complexContext.Objects.RemoveRange(allObjects);

                    foreach (TComplexObject complexObject in objectsToSave)
                    {
                        SetTimestamps(complexObject as AttachmentModelBase);

                        var existingObject = context.Objects.FirstOrDefault(obj => obj.Id == complexObject.Id);
                        if (existingObject == null)
                        {
                            DbEntityEntry<TComplexObject> entry = context.Entry(complexObject);
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            existingObject.AcceptProps(complexObject);
                            DbEntityEntry<TComplexObject> entry = context.Entry(existingObject);
                            entry.State = EntityState.Modified;
                        }
                    }

                    context.SaveChanges();
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

        private IEnumerable<TObject> GetDbObjectSynchronized<TObject>(Func<TObject, bool> predicate, bool isSimple) 
            where TObject : DbObject, new() 
        {
            lock (Locker)
            {
                if (isSimple)
                {
                    return TryGetSimpleDbObject<TObject>(predicate);
                }

                return TryGetComplexObjects<TObject>(predicate);
            }
        }

        private IEnumerable<TObject> TryGetSimpleDbObject<TObject>(Func<TObject, bool> predicate) 
            where TObject : DbObject, new() 
        {
            try
            {

                using (var dbContext = contextFactory.CreateContext<TObject>())
                {
                    return dbContext.Objects.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить объект типа '{ typeof(TObject).Name }'");
                throw;
            }
        }

        private IEnumerable<TObject> TryGetComplexObjects<TObject>(Func<TObject, bool> predicate) 
            where TObject : DbObject, new() 
        {
            try
            {
                var foundProxies = new List<TObject>();

                using (DbContextBase<TObject> dbContext = contextFactory.CreateContext<TObject>())
                {
                    IEnumerable<string> cachedProps = GetCachedProps<TObject>();
                    
                    if (cachedProps == null)
                    {
                        var complexProxy = new TObject() as ComplexDbObject;
                        var includedProps = new List<string> { complexProxy.IncludedPropertyNames };
                        includedProps.AddRange(complexProxy.IncludedChildPropNames);
                        SaveCachedProps<TObject>(includedProps);
                    }

                    cachedProps = GetCachedProps<TObject>();
                    DbQuery<TObject> query = IncludeProps(dbContext, cachedProps);
                    foundProxies.AddRange(predicate == null ? query.AsEnumerable() : query.AsEnumerable()?.Where(predicate));
                }
                
                return foundProxies;
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
            if (props == null || !props.Any() || props.Any(string.IsNullOrWhiteSpace)) return;

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

            if (props == null) return query;

            foreach (string cachedPropName in props)
            {
                if (!string.IsNullOrWhiteSpace(cachedPropName))
                {
                    query = query.Include(cachedPropName);
                }
            }

            return query;
        }
    }
}