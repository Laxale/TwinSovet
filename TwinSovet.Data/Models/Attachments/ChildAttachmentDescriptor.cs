using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Дескриптор дочернего аттача, приложенного к родительскому аттачу.
    /// </summary>
    /// <typeparam name="T">Тип родительского аттача, дочерних аттачей которого описывает данный дескриптор.</typeparam>
    [Table(DbConst.TableNames.ChildAttachmentDescriptorTableName)]
    public abstract class ChildAttachmentDescriptor<T> : ChildSimpleDbObject<T> where T : AttachmentModelBase, new () 
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