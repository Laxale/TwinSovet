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
    internal class PhotoAlbumAttachmentViewModel : 
        AlbumAttachmentViewModelBase<PhotoAlbumAttachmentModel, OfPhotoAlbumAttachmentDescriptor, PhotoAttachmentModel> 
    {
        


        public PhotoAlbumAttachmentViewModel(PhotoAlbumAttachmentModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            //todo поставить на превью первую фотку альбома
            //PreviewProvider.SetPreview(photoModel);
            //Preview.SetPreviewSource(PreviewProvider.GetPreview(photoModel.Id));
        }


        protected override AttachmentPanelDecoratorBase_NonGeneric DecoratorFactory(PhotoAttachmentModel model) 
        {
            return new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(model));
        }

        protected override bool ItemDecoratorFilter(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            var photoDecorator = (PhotoPanelDecorator) decorator;

            //пока нет логики, поскольку провайдер поставляет только элементы заведомо данного альбома.
            // другой логики фильтрации внутри альбома ещё нет
            return true;
        }
    }
}