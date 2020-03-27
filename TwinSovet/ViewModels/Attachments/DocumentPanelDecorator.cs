using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class DocumentPanelDecorator : AttachmentPanelDecoratorBase<DocumentAttachmentViewModel> 
    {
        public DocumentPanelDecorator(DocumentAttachmentViewModel attachmentViewModel) : base(attachmentViewModel)
        {

        }
    }
}