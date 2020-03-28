using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.ViewModels.Attachments 
{
    internal static class AttachmentViewModelFactory 
    {
        public static AttachmentViewModelBase CreateReadonly(AttachmentModelBase attachmentModel) 
        {
            if (attachmentModel is NoteAttachmentModel noteModel)
            {
                return NoteAttachmentViewModel.CreateReadonly(noteModel);
            }

            if (attachmentModel is PhotoAttachmentModel photoModel)
            {
                return PhotoAttachmentViewModel.CreateReadonly(photoModel);
            }

            if (attachmentModel is DocumentAttachmentModel documentModel)
            {
                return DocumentAttachmentViewModel.CreateReadonly(documentModel);
            }

            throw new NotImplementedException();
        }
    }
}