using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TwinSovet.ViewModels.Attachments;
using TwinSovet.Views;
using TwinSovet.Views.Attachments;


namespace TwinSovet.Helpers 
{
    internal static class AttachmentPanelResolver 
    {
        private const string SpecificPhotoTemplate_Detailed = "PhotoSpecificContentTemplate_Detailed";
        private const string SpecificPhotoTemplate_CreateNew = "PhotoSpecificContentTemplate_CreateNew";
        private const string SpecificPhotoAlbumTemplate_CreateNew = "PhotoAlbumSpecificContentTemplate_CreateNew";
        


        public static UserControl GetDetailedAttachmentPanelView(AttachmentPanelDecoratorBase_NonGeneric attachmentDecorator) 
        {
            if (attachmentDecorator is NotePanelDecorator)
            {
                return new AttachmentPanelView { DataContext = attachmentDecorator };
            }

            if (attachmentDecorator is PhotoPanelDecorator)
            {
                return new AttachmentPanelView
                {
                    DataContext = attachmentDecorator,
                    SpecificContentTemplate = (DataTemplate)Application.Current.Resources[SpecificPhotoTemplate_Detailed]
                };
            }

            throw new NotImplementedException();
        }

        public static DataTemplate CreateNew_GetSpecificContentTemplate(SubjectAttachmentsViewModelBase rootViewModel) 
        {
            if (rootViewModel is SubjectNotesViewModel)
            {
                return null;
            }

            if (rootViewModel is SubjectPhotosViewModel)
            {
                return (DataTemplate) Application.Current.Resources[SpecificPhotoAlbumTemplate_CreateNew];
            }

            throw new NotImplementedException();
        }
    }
}