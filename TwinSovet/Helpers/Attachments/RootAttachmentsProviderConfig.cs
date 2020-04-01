using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Helpers.Attachments 
{
    internal class RootAttachmentsProviderConfig : AttachmentProviderConfigBase 
    {
        public RootAttachmentsProviderConfig(SubjectType typeOfSubject, string subjectIdentifier, AttachmentType attachmentType) 
        {
            TypeOfSubject = typeOfSubject;
            AttachmentType = attachmentType;

            Predicate = attachmentModel =>
            {
                bool isFiltered = 
                    attachmentModel.HostType == TypeOfSubject.ToAttachmentHostType() &&
                    attachmentModel.HostId == subjectIdentifier;

                return isFiltered;
            };

            if (attachmentType == AttachmentType.Note)
            {
                DecoratorTransform = NoteDecoratorTransform;
            }
            else if (attachmentType == AttachmentType.Photo)
            {
                DecoratorTransform = PhotoDecoratorTransform;
            }
            if (attachmentType == AttachmentType.PhotoAlbum)
            {
                DecoratorTransform = PhotoAlbumDecoratorTransform;
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        public AttachmentType AttachmentType { get; }

        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public override Func<AttachmentModelBase, bool> Predicate { get; }

        /// <summary>
        /// Функция преобразования модели объекта в декоратор.
        /// </summary>
        public override Func<AttachmentModelBase, AttachmentPanelDecoratorBase_NonGeneric> DecoratorTransform { get; }


        private SubjectType TypeOfSubject { get; }
    }
}