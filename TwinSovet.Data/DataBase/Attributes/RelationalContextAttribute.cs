using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.Data.DataBase.Attributes 
{
    /// <summary>
    /// Атрибут, объявляющий ORM-контекст для типа объекта, хранимого в базе.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RelationalContextAttribute : Attribute 
    {
        /// <summary>
        /// Конструирует <see cref="RelationalContextAttribute"/> с заданным типом ORM-контекста для помечаемого класса.
        /// </summary>
        /// <param name="contextType">Тип контекста, используемый для хранения помечаемого атрибутом объекта в базе.</param>
        [DebuggerStepThrough]
        public RelationalContextAttribute(Type contextType)
        {
            ContextType = contextType ?? throw new ArgumentNullException(nameof(contextType));
        }


        /// <summary>
        /// Возвращает тип ORM-контекста для класса, помеченного атрибутом <see cref="RelationalContextAttribute"/>.
        /// </summary>
        public Type ContextType { get; }
    }
}