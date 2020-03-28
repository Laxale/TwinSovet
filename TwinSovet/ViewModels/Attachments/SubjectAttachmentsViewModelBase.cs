using System;

using TwinSovet.Data.Enums;

using Prism.Commands;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс для вьюмоделей коллекций аттачей корневых объектов - субъектов <see cref="SubjectType"/>.
    /// </summary>
    internal abstract class SubjectAttachmentsViewModelBase : ViewModelBase 
    {
        private AttachmentPanelDecoratorBase_NonGeneric detailedAttachmentDecorator;


        protected SubjectAttachmentsViewModelBase() 
        {
            CommandOpenDetails = new DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric>(OpenDetailsImpl, CanOpenDetails);
        }


        public DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric> CommandOpenDetails { get; }


        public bool IsDetailing => DetailedAttachmentDecorator != null;

        /// <summary>
        /// 
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


        public void OnCancelledDetailing() 
        {
            DetailedAttachmentDecorator = null;
        }


        private void OpenDetailsImpl(AttachmentPanelDecoratorBase_NonGeneric detailedDecorator) 
        {
            DetailedAttachmentDecorator = detailedDecorator;
        }

        private bool CanOpenDetails(AttachmentPanelDecoratorBase_NonGeneric detailedDecorator) 
        {
            return !IsDetailing;
        }
    }
}