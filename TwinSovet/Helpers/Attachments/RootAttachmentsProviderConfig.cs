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
        public RootAttachmentsProviderConfig(SubjectType typeOfSubject, AttachmentType attachmentType) 
        {
            TypeOfSubject = typeOfSubject;
            AttachmentType = attachmentType;

            Predicate = attachmentModel => attachmentModel.HostType == TypeOfSubject.ToAttachmentHostType();
        }


        public SubjectType TypeOfSubject { get; }

        public AttachmentType AttachmentType { get; }

        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public override Func<AttachmentModelBase, bool> Predicate { get; }
            
    }
}