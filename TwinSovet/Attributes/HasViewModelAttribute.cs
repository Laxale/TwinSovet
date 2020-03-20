using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.Attributes 
{
    /// <summary>
    /// Для целей связывания вью и вьюмоделей объявляет тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HasViewModelAttribute : Attribute 
    {
        /// <summary>
        /// Конструирует <see cref="HasViewModelAttribute"/> с заданным типом вьюмодели.
        /// </summary>
        /// <param name="viewModelType">Тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.</param>
        public HasViewModelAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }


        /// <summary>
        /// Тип вьюмодели, соответствующий типу вью, который помечен данным атрибутом.
        /// </summary>
        public Type ViewModelType { get; }
    }
}