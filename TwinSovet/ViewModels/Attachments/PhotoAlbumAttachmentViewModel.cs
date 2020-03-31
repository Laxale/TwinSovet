using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoAlbumAttachmentViewModel : AlbumAttachmentViewModelBase<PhotoAlbumAttachmentModel, PhotoDescriptorModel> 
    {
        


        public PhotoAlbumAttachmentViewModel(PhotoAlbumAttachmentModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            //todo поставить на превью первую фотку альбома
            //PreviewProvider.SetPreview(photoModel);
            //Preview.SetPreviewSource(PreviewProvider.GetPreview(photoModel.Id));
        }



    }
}