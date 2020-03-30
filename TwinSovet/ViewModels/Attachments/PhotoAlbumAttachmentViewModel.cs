using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoAlbumAttachmentViewModel : AttachmentViewModelBase 
    {
        public PhotoAlbumAttachmentViewModel(PhotoAlbumAttachmentModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            //todo поставить на превью первую фотку альбома
            //PreviewProvider.SetPreview(photoModel);
            //Preview.SetPreviewSource(PreviewProvider.GetPreview(photoModel.Id));
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.PhotoAlbum;

        /// <summary>
        /// Возвращает ссылку на вьюмодель превью данной картинки.
        /// </summary>
        public PreviewViewModel Preview { get; } = new PreviewViewModel();
    }
}