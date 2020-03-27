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
            return null;
        }
    }
}
