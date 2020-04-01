using System;

using DataVirtualization;

using Microsoft.Practices.Unity;

using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Вьюмодель фотографий субъекта.
    /// </summary>
    internal class SubjectPhotosViewModel : SubjectAttachmentsViewModelBase
    {
        private IAttachmentsProvider<PhotoAlbumAttachmentModel> provider;


        public SubjectPhotosViewModel(IUnityContainer container) : base(container) 
        {

        }


        public override AttachmentType TypeOfAttachment { get; } = AttachmentType.PhotoAlbum;


        protected override void RefreshProvider() 
        {
            provider?.Refresh();
        }

        protected override AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> InitCollection(RootAttachmentsProviderConfig config) 
        {
            var endpoint = container.Resolve<IDbEndPoint>();
            var factory = container.Resolve<IDbContextFactory>();

            provider = new AttachmentsProvider<PhotoAlbumAttachmentModel>(endpoint, factory, config);

            return new AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric>(provider, pageSize, pageTimeout);
        }

        protected override bool MustReactToAttachmentCreation(AttachmentViewModelBase attachmentViewModelBase) 
        {
            return attachmentViewModelBase is PhotoAttachmentViewModel || attachmentViewModelBase is PhotoAlbumAttachmentViewModel;
        }
    }
}