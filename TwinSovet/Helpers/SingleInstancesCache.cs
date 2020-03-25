using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using TwinSovet.Attributes;
using TwinSovet.Extensions;

using Prism.Unity;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Фабрика объектов для <see cref="UnityBootstrapper"/>, учитывающая наличие у типов атрибута <see cref="SingleInstanceAttribute"/> для кэширования инстансов.
    /// </summary>
    internal class SingleInstancesCache 
    {
        private readonly Func<Type, object> defaultFactory;
        private readonly List<string> notSingleInstancedTypes = new List<string>();
        private readonly Dictionary<string, object> typeToInstanceCache = new Dictionary<string, object>();


        /// <summary>
        /// Конструирует <see cref="SingleInstancesCache"/> с заданной дефолтной фабрикой объектов.
        /// </summary>
        /// <param name="defaultFactory">Дефолтная фабрика объектов.</param>
        public SingleInstancesCache(Func<Type, object> defaultFactory) 
        {
            this.defaultFactory = defaultFactory ?? throw new ArgumentNullException(nameof(defaultFactory));
        }


        /// <summary>
        /// Получить (кэшированный) или создать инстанс объекта данного типа.
        /// </summary>
        /// <param name="objType">Тип объекта для создания.</param>
        /// <returns>Кэшированный или созданный инстанс объекта.</returns>
        public object GetOrCreateInstance(Type objType) 
        {
            objType.AssertNotNull(nameof(objType));

            string typeName = objType.FullName;
            if (notSingleInstancedTypes.Contains(typeName))
            {
                return defaultFactory(objType);
            }

            if (typeToInstanceCache.ContainsKey(typeName))
            {
                return typeToInstanceCache[typeName];
            }
            else
            {
                object createdObj = defaultFactory(objType);
                bool mustBeSingle = ReadSingleInstanceAttributeValue(objType);

                if (mustBeSingle)
                {
                    typeToInstanceCache.Add(typeName, createdObj);
                }
                else
                {
                    notSingleInstancedTypes.Add(typeName);
                }

                return createdObj;
            }
        }


        private bool ReadSingleInstanceAttributeValue(Type viewType) 
        {
            var singleInstanceAttribute = viewType.GetCustomAttribute<SingleInstanceAttribute>();

            if (singleInstanceAttribute == null)
            {
                return false;
            }

            return singleInstanceAttribute.MustBeSingle;
        }
    }
}
