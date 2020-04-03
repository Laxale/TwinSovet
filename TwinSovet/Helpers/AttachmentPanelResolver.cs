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
        private const string PhotoAlbumSpecificContentTemplate_Detailed = "PhotoAlbumSpecificContentTemplate_Detailed";
        


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
                    TopSpecificContentTemplate = (DataTemplate)Application.Current.Resources[SpecificPhotoTemplate_Detailed]
                };
            }

            if (attachmentDecorator is PhotoAlbumPanelDecorator photoAlbumDecorator)
            {
                return new AttachmentPanelView
                {
                    DataContext = attachmentDecorator,
                    //DataContext = photoAlbumDecorator.EditableAttachmentViewModel,
                    BottomSpecificContentTemplate = (DataTemplate)Application.Current.Resources[PhotoAlbumSpecificContentTemplate_Detailed]
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