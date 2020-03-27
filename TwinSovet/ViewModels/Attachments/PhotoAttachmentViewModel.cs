using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;

namespace TwinSovet.ViewModels.Attachments
{
    internal class PhotoAttachmentViewModel : AttachmentViewModelBase 
    {
        private PhotoAttachmentViewModel(PhotoAttachmentModel photoModel, bool isReadonly) : base(photoModel, isReadonly) 
        {
            
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
        public override AttachmentType EntityType { get; } = AttachmentType.Photo;
    }
}