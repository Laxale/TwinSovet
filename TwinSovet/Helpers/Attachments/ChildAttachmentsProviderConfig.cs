using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Interfaces;


namespace TwinSovet.Helpers.Attachments 
{
    /// <summary>
    /// Конфигурация для <see cref="AttachmentsProvider"/>. Содержит логику выделения дочерних аттачей при чтении из базы.
    /// </summary>
    internal class ChildAttachmentsProviderConfig : AttachmentProviderConfigBase 
    {
        public ChildAttachmentsProviderConfig(NoteAttachmentModel parentAttachment) 
        {
            Predicate = model =>
                parentAttachment.ChildDescriptors.Any(childDescriptor =>
                    childDescriptor.ChildAttachmentId == model.Id
                // не нужно проверять тип, поскольку аттачи лежат в разных таблицах.
                // предикат, соответственно, применяется к одному типу аттачей внутри одного EF-контекста.
                // конкретную таблицу выбирает провайдер по полю ChildAttachmentType
                //&& childDescriptor.ChildAttachmentType == model.TypeOfAttachment
                );
        }

        public ChildAttachmentsProviderConfig(PhotoAttachmentModel parentAttachment) 
        {
            Predicate = model =>
                parentAttachment.ChildDescriptors.Any(childDescriptor =>
                        childDescriptor.ChildAttachmentId == model.Id);
        }

        public ChildAttachmentsProviderConfig(DocumentAttachmentModel parentAttachment) 
        {
            Predicate = model =>
                parentAttachment.ChildDescriptors.Any(childDescriptor =>
                    childDescriptor.ChildAttachmentId == model.Id);
        }


        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public override Func<AttachmentModelBase, bool> Predicate { get; }
    }
}