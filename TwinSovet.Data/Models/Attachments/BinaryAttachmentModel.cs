using System;
using System.ComponentModel.DataAnnotations.Schema;
using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель хранимых в базе довичных данных.
    /// </summary>
    public abstract class BinaryAttachmentModel : AttachmentModelBase 
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор блоба данных (объекта <see cref="BinaryDataModel"/>), соответствующего данному аттачу.
        /// </summary>
        public string FullDataDescriptorId { get; set; }

        /// <summary>
        /// Дескриптор данных объекта. Не мапится при чтении аттача, читается отдельным запросом при необходимости.
        /// </summary>
        [NotMapped]
        public BinaryDataModel FullDataModel { get; set; }


        /// <summary>
        /// Принять свойства редактированной копии исходного объекта в базе для сохранения изменений.
        /// </summary>
        /// <returns></returns>
        public override void AcceptProps(ComplexDbObject other)
        {
            var binaryOther = (BinaryAttachmentModel) other;
            base.AcceptModelProperties(binaryOther);

            FullDataDescriptorId = binaryOther.FullDataDescriptorId;
        }
    }
}