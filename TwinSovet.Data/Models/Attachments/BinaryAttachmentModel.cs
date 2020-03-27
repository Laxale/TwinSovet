using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace TwinSovet.Data.Models.Attachments 
{
    public class BinaryAttachmentModel : AttachmentModelBase 
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор блоба данных, соответствующих данному аттачу.
        /// </summary>
        public string FullDataDescriptorId { get; set; }

        /// <summary>
        /// Дескриптор данных объекта. Не мапится при чтении аттача, читается отдельным запросом при необходимости.
        /// </summary>
        [NotMapped]
        public BinaryDataDescriptor FullDataDescriptor { get; set; }
    }
}