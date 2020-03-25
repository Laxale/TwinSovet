using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TwinSovet.Data.DataBase.Base 
{
    /// <summary>
    /// Базовый класс для хранящихся в базе объектов.
    /// </summary>
    public abstract class DbObject 
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected DbObject() 
        {
            Id = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Возвращает или задаёт уникальный идентификатор объекта в базе.
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
    }
}