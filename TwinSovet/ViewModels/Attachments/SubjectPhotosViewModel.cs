using System;

using Microsoft.Practices.Unity;

using TwinSovet.Data.Enums;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Вьюмодель фотографий субъекта.
    /// </summary>
    internal class SubjectPhotosViewModel : SubjectAttachmentsViewModelBase 
    {
        public SubjectPhotosViewModel(IUnityContainer container) : base(container) 
        {

        }


        public override AttachmentType TypeOfAttachment { get; } = AttachmentType.Photo;


        protected override bool MustReactToAttachmentCreation(AttachmentViewModelBase attachmentViewModelBase) 
        {
            return attachmentViewModelBase is PhotoAttachmentViewModel;
        }
    }
}