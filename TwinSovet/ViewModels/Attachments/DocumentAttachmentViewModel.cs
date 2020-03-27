using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;

namespace TwinSovet.ViewModels.Attachments
{
    internal class DocumentAttachmentViewModel : AttachmentViewModelBase 
    {
        private DocumentAttachmentViewModel(DocumentAttachmentModel documentModel, bool isReadonly) : base(documentModel, isReadonly) 
        {
            
        }


        public static DocumentAttachmentViewModel CreateReadonly(DocumentAttachmentModel documentModel) 
        {
            return new DocumentAttachmentViewModel(documentModel, true);
        }

        public static DocumentAttachmentViewModel CreateEditable(DocumentAttachmentModel documentModel) 
        {
            return new DocumentAttachmentViewModel(documentModel, false);
        }

        
        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Document;
    }
}