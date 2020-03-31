using System.ComponentModel.DataAnnotations.Schema;
using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.DataBase.Context;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Дескриптор дочернего аттача, приложенного к родительскому аттачу.
    /// </summary>
    [RelationalContext(typeof(ChildAttachmentDescriptorsContext))]
    [Table(DbConst.TableNames.ChildAttachmentDescriptorsTableName)]
    public class ChildAttachmentDescriptor : ChildAttachmentDbObject 
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор дочернего аттача.
        /// </summary>
        public string ChildAttachmentId { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип дочернего аттача.
        /// </summary>
        public AttachmentType ChildAttachmentType { get; set; }
    }
}