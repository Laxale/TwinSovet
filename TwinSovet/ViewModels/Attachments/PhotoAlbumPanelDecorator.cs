using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoAlbumPanelDecorator : AttachmentPanelDecoratorBase<PhotoAlbumAttachmentViewModel> 
    {
        public PhotoAlbumPanelDecorator(PhotoAlbumAttachmentViewModel editableViewModel) : 
            base(editableViewModel
                //, false
                ) 
        {

        }
    }
}