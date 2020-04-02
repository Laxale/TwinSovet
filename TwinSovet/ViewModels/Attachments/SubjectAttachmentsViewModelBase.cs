using System;

using DataVirtualization;

using Microsoft.Practices.Unity;

using TwinSovet.Data.Enums;

using Prism.Commands;

using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс для вьюмоделей коллекций аттачей корневых объектов - субъектов <see cref="SubjectType"/>.
    /// </summary>
    internal abstract class SubjectAttachmentsViewModelBase : ViewModelBase 
    {
        protected readonly int pageSize = 10;
        protected readonly int pageTimeout = int.MaxValue;
        protected readonly IUnityContainer container;

        private SubjectEntityViewModelBase notesOwner;
        private AttachmentPanelDecoratorBase_NonGeneric detailedAttachmentDecorator;


        protected SubjectAttachmentsViewModelBase(IUnityContainer container) 
        {
            this.container = container;

            ClientCommands.EventAttachmentSaveAttempt += ClientCommands_OnAttachmentSaveAttempt;

            CommandOpenDetails = new DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric>(OpenDetailsImpl, CanOpenDetails);
        }

        ~SubjectAttachmentsViewModelBase() 
        {
            ClientCommands.EventAttachmentSaveAttempt -= ClientCommands_OnAttachmentSaveAttempt;
        }


        public DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric> CommandOpenDetails { get; }


        public bool IsDetailing => DetailedAttachmentDecorator != null;

        /// <summary>
        /// Возвращает текущий декоратор детализируемого аттача.
        /// </summary>
        public AttachmentPanelDecoratorBase_NonGeneric DetailedAttachmentDecorator 
        {
            get => detailedAttachmentDecorator;

            protected set
            {
                if (detailedAttachmentDecorator == value) return;

                detailedAttachmentDecorator = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDetailing));
            }
        }

        public bool HasOwner => CurrentNotesOwner != null;

        /// <summary>
        /// Возвращает текущую вьюмодель субъекта, для которого отображаем заметки.
        /// </summary>
        public SubjectEntityViewModelBase CurrentNotesOwner 
        {
            get => notesOwner;

            private set
            {
                if (notesOwner == value) return;

                notesOwner = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOwner));
            }
        }

        /// <summary>
        /// Возвращает тип аттачей, которые содержит данная вьюмодель.
        /// </summary>
        public abstract AttachmentType TypeOfAttachment { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> NoteDecorators { get; private set; }


        public void OnCancelledDetailing() 
        {
            DetailedAttachmentDecorator = null;
        }

        public void SetNotesOwner(SubjectEntityViewModelBase owner) 
        {
            if (notesOwner != null) throw new InvalidOperationException($"Нельзя повторно задать субъекта");

            CurrentNotesOwner = owner;
            string hostId = RootSubjectIdentifier.Identify(owner);
            var config = new RootAttachmentsProviderConfig(owner.TypeOfSubject, hostId, TypeOfAttachment);

            NoteDecorators = InitCollection(config);

            OnPropertyChanged(nameof(NoteDecorators));

            RefreshCollection();
        }

        protected abstract void RefreshProvider();

        protected abstract AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> InitCollection(RootAttachmentsProviderConfig config);

        protected abstract bool MustReactToAttachmentCreation(AttachmentViewModelBase attachmentViewModelBase);


        private void OpenDetailsImpl(AttachmentPanelDecoratorBase_NonGeneric detailedDecorator) 
        {
            DetailedAttachmentDecorator = detailedDecorator;
        }

        private bool CanOpenDetails(AttachmentPanelDecoratorBase_NonGeneric detailedDecorator) 
        {
            return !IsDetailing;
        }

        
        private void RefreshCollection() 
        {
            NoteDecorators?.Refresh();
        }


        private void ClientCommands_OnAttachmentSaveAttempt(AttachmentViewModelBase attachmentViewModelBase, bool suceeded) 
        {
            if (!suceeded) return;

            if (MustReactToAttachmentCreation(attachmentViewModelBase))
            {
                RefreshProvider();
                RefreshCollection();
            }
        }
    }
}