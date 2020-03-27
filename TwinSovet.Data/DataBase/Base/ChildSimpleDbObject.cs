using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TwinSovet.Data.DataBase.Base 
{
    /// <summary>
    /// Базовый класс для наследжования простых БД-объектов, привязанных к родительскому объекту.
    /// </summary>
    /// <typeparam name="TComplexParent">Родительский сложный БД-объект.</typeparam>
    public abstract class ChildSimpleDbObject<TComplexParent> : SimpleDbObject where TComplexParent : ComplexDbObject, new() 
    {
        /// <summary>
        /// Внешний ключ для связи с родительским объектом <see cref="TComplexParent"/>.
        /// </summary>
        [Required]
        public string ParentId { get; set; }

        /// <summary>
        /// Навигационное свойство - родительский объект <see cref="TComplexParent"/>.
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public TComplexParent NavigationParent { get; set; }
    }
}