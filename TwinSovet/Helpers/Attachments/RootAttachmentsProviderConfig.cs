using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;


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
        }


        public AttachmentType AttachmentType { get; }

        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public override Func<AttachmentModelBase, bool> Predicate { get; }


        private SubjectType TypeOfSubject { get; }
    }
}