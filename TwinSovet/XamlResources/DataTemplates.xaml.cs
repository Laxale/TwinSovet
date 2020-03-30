using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Common.Helpers;
using TwinSovet.Messages.Details;
using TwinSovet.ViewModels.Subjects;

using PubSub;
using TwinSovet.Providers;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.XamlResources 
{
    partial class DataTemplates 
    {
        private void SimpleFlatView_OnEventShowFlatDetails(FlatDecoratorViewModel flat) 
        {
            this.Publish(new MessageShowFlatDetails(flat));
        }

        private void SimpleFlatView_OnEventShowOwnerDetails(FlatDecoratorViewModel flat) 
        {
            this.Publish(new MessageShowAborigenDetails(flat.OwnerDecorator));
        }

        private void PhotoPreviewBorder_OnDragEnter(object sender, DragEventArgs e) 
        {
            var border = (Border) sender;
            border.BorderBrush = (Brush)this["SteelGrayBrush"];
        }

        private void PhotoPreviewBorder_OnDragLeave(object sender, DragEventArgs e) 
        {
            var border = (Border)sender;
            border.BorderBrush = null;
        }

        private void PhotoPreviewBorder_OnDrop(object sender, DragEventArgs e) 
        {
            var border = (Border)sender;
            border.BorderBrush = null;
            // мы в режиме детализации
            if (border.DataContext is PhotoPanelDecorator decorator)
            {
                using (var helper = new DragAndDropHelper(border, true))
                {
                    string[] files = helper.GetDroppedFiles(e);
                    IEnumerable<string> imageFiles = files.Where(PreviewProvider.IsImage);
                    if (imageFiles.Any())
                    {
                        throw new NotImplementedException();
                        //decorator.EditableAttachmentViewModel.
                    }
                }
            }
        }
    }
}