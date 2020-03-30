using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TwinSovet.ViewModels.Attachments;
using TwinSovet.Views;
using TwinSovet.Views.Attachments;


namespace TwinSovet.Helpers 
{
    internal static class AttachmentPanelResolver 
    {
        public static UserControl GetAttachmentPanelView(AttachmentPanelDecoratorBase_NonGeneric attachmentDecorator) 
        {
            if (attachmentDecorator is NotePanelDecorator)
            {
                return new AttachmentPanelView { DataContext = attachmentDecorator };
            }

            if (attachmentDecorator is PhotoPanelDecorator)
            {
                return new PhotoPanelView { DataContext = attachmentDecorator };
            }

            throw new NotImplementedException();
        }
    }
}