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
            CommandAddNew = new DelegateCommand(AddNewImpl);
        }


        public DelegateCommand CommandAddNew { get; }


        public bool IsDetailing => DetailedAttachmentDecorator != null;

        /// <summary>
        /// 
        /// </summary>
        public AttachmentPanelDecoratorBase_NonGeneric DetailedAttachmentDecorator 
        {
            get => detailedAttachmentDecorator;

            private set
            {
                if (detailedAttachmentDecorator == value) return;

                detailedAttachmentDecorator = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDetailing));
            }
        }


        private void AddNewImpl() 
        {
            throw new NotImplementedException();
        }
    }
}