using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;

using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Context;
using TwinSovet.Data.DataBase.Interfaces;


namespace TwinSovet.Data.DataBase 
{
    /// <summary>
    /// Реализует логику создания контекстов БД для всех типов хранимых объектов.
    /// </summary>
    public class DbContextFactory : IDbContextFactory 
    {
        /// <summary>
        /// Создать контекст БД для данного типа объекта.
        /// </summary>
        /// <typeparam name="TObject">Тип хранимого в базе объекта.</typeparam>
        /// <returns>Инстанс контекста БД.</returns>
        public DbContextBase<TObject> CreateContext<TObject>() where TObject : DbObject, new() 
        {
            Type contextType = GetContextType<TObject>();

            DbContextBase<TObject> context = CreateContextOfType<TObject>(contextType);

            return context;
        }


        private Type GetContextType<TObject>() where TObject : DbObject, new() 
        {
            var objectType = typeof(TObject);
            string objectTypeName = objectType.Name;

            var contextTypeAttribute = objectType.GetCustomAttributes<RelationalContextAttribute>().FirstOrDefault();

            var basicType = typeof(object);
            var simpleType = typeof(SimpleDbObject);

            if (contextTypeAttribute == null)
            {
                while (objectType.BaseType != basicType)
                {
                    if (objectType.BaseType == simpleType)
                    {
                        return typeof(SimpleDbContext<TObject>);
                    }

                    objectType = objectType.BaseType;
                }

                ThrowInvalidObjectError(objectTypeName);
            }

            return contextTypeAttribute.ContextType;
        }

        private DbContextBase<TObject> CreateContextOfType<TObject>(Type contextType) where TObject : DbObject, new() 
        {
            ConstructorInfo[] ctors = contextType.GetConstructors();
            ConstructorInfo defaultCtor = ctors.FirstOrDefault(ctor => ctor.IsPublic && !ctor.GetParameters().Any());

            if(defaultCtor == null) throw new InvalidOperationException($"{ contextType } не содержит дефолтный конструктор");

            object context = defaultCtor.Invoke(null);

            return (DbContextBase<TObject>)context;
        }

        private void ThrowInvalidObjectError(string objectTypeName) 
        {
            string error =
                $"Для определения контекста тип '{ objectTypeName }' должен наследовать { nameof(SimpleDbObject) } " +
                $"или быть помеченным атрибутом { nameof(RelationalContextAttribute) }";

            throw new InvalidOperationException(error);
        }
    }
}