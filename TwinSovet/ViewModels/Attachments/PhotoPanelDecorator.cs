﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoPanelDecorator : AttachmentPanelDecoratorBase<PhotoAttachmentViewModel> 
    {
        public PhotoPanelDecorator(PhotoAttachmentViewModel attachmentViewModel) : base(attachmentViewModel)
        {

        }
    }
}