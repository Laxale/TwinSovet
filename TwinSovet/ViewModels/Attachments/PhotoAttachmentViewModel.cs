using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Вьюмодель аттача - отдельной фотографии.
    /// </summary>
    internal class PhotoAttachmentViewModel : AttachmentViewModelBase 
    {
        private PhotoAttachmentViewModel(PhotoAttachmentModel photoModel, bool isReadonly) : base(photoModel, isReadonly) 
        {
            if (photoModel.PreviewDataBlob?.Any() ?? false)
            {
                PreviewProvider.SetPreview(photoModel);
                Preview.SetPreviewSource(PreviewProvider.GetPreview(photoModel.Id));
            }
        }


        public static PhotoAttachmentViewModel CreateReadonly(PhotoAttachmentModel photoModel) 
        {
            return new PhotoAttachmentViewModel(photoModel, true);
        }

        public static PhotoAttachmentViewModel CreateEditable(PhotoAttachmentModel photoModel) 
        {
            return new PhotoAttachmentViewModel(photoModel, false);
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType TypeOfAttachment { get; } = AttachmentType.Photo;

        /// <summary>
        /// Возвращает ссылку на вьюмодель превью данной картинки.
        /// </summary>
        public PreviewViewModel Preview { get; } = new PreviewViewModel();
    }
}