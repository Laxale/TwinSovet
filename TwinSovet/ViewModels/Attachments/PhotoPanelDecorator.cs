using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoPanelDecorator : AttachmentPanelDecoratorBase<PhotoAttachmentViewModel> 
    {
        public PhotoPanelDecorator(PhotoAttachmentViewModel attachmentViewModel) : 
            base(attachmentViewModel
                //, true
                ) 
        {

        }
    }
}