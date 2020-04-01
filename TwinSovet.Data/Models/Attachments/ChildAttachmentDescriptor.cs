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
    /// <typeparam name="TParentAttachmentModel">Тип аттачмента, дочерние аттачи которого описывает данный дескриптор.</typeparam>
    /// </summary>
    public abstract class ChildAttachmentDescriptor 
        <TParentAttachmentModel>
        : 
        //ChildAttachmentDbObject 
        ChildSimpleDbObject<TParentAttachmentModel>
        where TParentAttachmentModel : AttachmentModelBase, new()
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