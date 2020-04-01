using System;

using DataVirtualization;

using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Helpers;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;

using Microsoft.Practices.Unity;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Вьюмодель заметок субъекта.
    /// </summary>
    internal class SubjectNotesViewModel : SubjectAttachmentsViewModelBase 
    {
        private IAttachmentsProvider<NoteAttachmentModel> provider;


        /// <summary>
        /// Конструирует <see cref="SubjectNotesViewModel"/> с данными зависимостями.
        /// </summary>
        /// <param name="container"><see cref="IUnityContainer"/>.</param>
        public SubjectNotesViewModel(IUnityContainer container) : base(container) 
        {
            
        }


        public override AttachmentType TypeOfAttachment { get; } = AttachmentType.Note;


        protected override void RefreshProvider() 
        {
            provider?.Refresh();
        }

        protected override AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> InitCollection(RootAttachmentsProviderConfig config) 
        {
            var endpoint = container.Resolve<IDbEndPoint>();
            var factory = container.Resolve<IDbContextFactory>();

            provider = new AttachmentsProvider<NoteAttachmentModel>(endpoint, factory, config);

            return new AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric>(provider, pageSize, pageTimeout);
        }

        protected override bool MustReactToAttachmentCreation(AttachmentViewModelBase attachmentViewModelBase) 
        {
            return attachmentViewModelBase is NoteAttachmentViewModel;
        }
    }
}