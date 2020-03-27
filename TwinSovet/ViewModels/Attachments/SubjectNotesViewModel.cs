using System;

using DataVirtualization;

using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Helpers;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;

using Microsoft.Practices.Unity;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Вьюмоделей заметок субъекта.
    /// </summary>
    internal class SubjectNotesViewModel : AttachmentViewModelBase
    {
        private readonly int pageSize = 10;
        private readonly int pageTimeout = int.MaxValue;
        private readonly IUnityContainer container;

        private IAttachmentsProvider attachmentsProvider;
        private SubjectEntityViewModel notesOwner;


        public SubjectNotesViewModel(IUnityContainer container) 
        {
            this.container = container;
        }


        public bool HasOwner => CurrentNotesOwner != null;

        /// <summary>
        /// Возвращает текущую вьюмодель субъекта, для которого отображаем заметки.
        /// </summary>
        public SubjectEntityViewModel CurrentNotesOwner 
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
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Note;

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> NoteDecorators { get; private set; }


        public void SetNotesOwner(SubjectEntityViewModel owner) 
        {
            if(notesOwner != null) throw new InvalidOperationException($"Нельзя повторно задать субъекта");

            CurrentNotesOwner = owner;
            var config = new RootAttachmentsProviderConfig(owner.TypeOfSubject, AttachmentType.Note);
            attachmentsProvider = new AttachmentsProvider(container.Resolve<IDbContextFactory>(), config);

            NoteDecorators = new AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric>(attachmentsProvider, pageSize, pageTimeout);

            OnPropertyChanged(nameof(NoteDecorators));

            RefreshCollection();
        }


        private void RefreshCollection() 
        {
            NoteDecorators?.Refresh();
        }
    }
}