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

using TwinSovet.Providers;
using TwinSovet.ViewModels.Attachments;

using PubSub;


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

            if (!IsTagged(border))
            {
                border.BorderBrush = (Brush)this["SteelGrayBrush"];
            }
            else
            {
                border.BorderThickness = new Thickness(border.BorderThickness.Left + 1);
            }
        }

        private void PhotoPreviewBorder_OnDragLeave(object sender, DragEventArgs e) 
        {
            var border = (Border)sender;

            if (!IsTagged(border))
            {
                border.BorderBrush = null;
            }
            else
            {
                border.BorderThickness = new Thickness(border.BorderThickness.Left - 1);
            }
        }

        private void PhotoPreviewBorder_OnDrop(object sender, DragEventArgs e) 
        {
            var border = (Border)sender;
            if (!IsTagged(border))
            {
                border.BorderBrush = null;
            }
            else
            {
                border.BorderThickness = new Thickness(border.BorderThickness.Left - 1);
            }

            using (var helper = new DragAndDropHelper(border, true))
            {
                string[] files = helper.GetDroppedFiles(e);
                IEnumerable<string> imageFiles = files.Where(PreviewProvider.IsImage);
                if (!imageFiles.Any()) return;

                // мы в режиме детализации
                if (border.DataContext is PhotoPanelDecorator decorator)
                {

                }
                else if (border.DataContext is PhotoAlbumAttachmentViewModel albumViewModel)
                {
                    albumViewModel.AddFilesToAddedBuffer(imageFiles);
                }
                else if (border.DataContext is PhotoAlbumPanelDecorator photoAlbumDecorator)
                {
                    photoAlbumDecorator.EditableAttachmentViewModel.AddFilesToAddedBuffer(imageFiles);
                }
            }
        }


        private bool IsTagged(Border border) 
        {
            return border.Tag?.ToString() == (string) this["ClearTag"];
        }
    }
}