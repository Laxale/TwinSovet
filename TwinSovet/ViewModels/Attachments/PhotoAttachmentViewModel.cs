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
        public PhotoAttachmentViewModel(PhotoAttachmentModel photoModel) 
        {
            
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Photo;
    }
}