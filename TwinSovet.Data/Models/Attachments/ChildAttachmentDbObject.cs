using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.Models.Attachments 
{
    public class ChildAttachmentDbObject : SimpleDbObject 
    {
        /// <summary>
        /// Внешний ключ для связи с родительским объектом <see cref="NavigationParent"/>.
        /// </summary>
        [Required]
        public string ParentId { get; set; }

        /// <summary>
        /// Навигационное свойство - родительский объект <see cref="AttachmentModelBase"/>.
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public AttachmentModelBase NavigationParent { get; set; }
    }
}