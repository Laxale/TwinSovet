using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель ссылки на данные - то есть дескриптора, указателя на объект в базе.
    /// </summary>
    public abstract class BinaryDescriptorModel : ChildAttachmentDbObject 
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор блоба данных (объекта <see cref="BinaryDataModel"/>), соответствующего данному аттачу.
        /// </summary>
        public string DataBlobId { get; set; }

        /// <summary>
        /// Дескриптор данных объекта. Не мапится при чтении аттача, читается отдельным запросом при необходимости.
        /// </summary>
        [NotMapped]
        public BinaryDataModel DataBlobModel { get; set; }
    }
}