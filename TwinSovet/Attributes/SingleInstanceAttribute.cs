using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.Attributes 
{
    /// <summary>
    /// Атрибут для придания объектам свойства, помечающего их желанием (НЕ) использоваться в единственном экземпляре, (НЕ) кэшировать, (НЕ) создавать дубли.
    /// </summary>
    public class SingleInstanceAttribute : Attribute 
    {
        /// <summary>
        /// Конструирует <see cref="SingleInstanceAttribute"/> с заданным флагом единственного инстанса.
        /// </summary>
        /// <param name="mustBeSingle">True означает желание объекта быть использованным в единственном экземпляре.</param>
        public SingleInstanceAttribute(bool mustBeSingle)
        {
            MustBeSingle = mustBeSingle;
        }


        /// <summary>
        /// Возвращает флаг - желает ли объект быть использованным в единственном экземпляре.
        /// </summary>
        public bool MustBeSingle { get; }
    }
}