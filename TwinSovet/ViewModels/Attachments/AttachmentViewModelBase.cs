using System;

using Prism.Commands;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.Interfaces;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс для реализации аттачментов.
    /// В силу наследования от <see cref="AttachmentAcceptorViewModel"/> любому аттачу можно приложить дерево аттачей.
    /// </summary>
    internal abstract class AttachmentViewModelBase : AttachmentAcceptorViewModel, IReadonlyFlagged 
    {
        protected readonly AttachmentModelBase originalModel;

        protected bool isAcceptingProps;

        private string title;
        private string description;
        private DateTime? modificationTime;

        /// <summary>
        /// Событие успешного сохранения данных аттача.
        /// </summary>
        public event Action EventExecutedAttachmentEdit = () => { };


        protected AttachmentViewModelBase(AttachmentModelBase attachmentModel, bool isReadonly) 
        {
            originalModel = attachmentModel;
            IsReadonly = isReadonly;

            Title = attachmentModel.Title;
            Description = attachmentModel.Description;
            CreationTime = attachmentModel.CreationTime;
            ModificationTime = attachmentModel.ModificationTime;

            CommandSave = new DelegateCommand(SaveImpl);
        }


        public DelegateCommand CommandSave { get; }


        public bool IsReadonly { get; }

        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public abstract AttachmentType EntityType { get; }
        
        public string Title 
        {
            get => title;

            set
            {
                this.VerifyIsEditable();
                if (title == value) return;

                title = value;

                OnPropertyChanged();
            }
        }

        public string Description 
        {
            get => description;

            set
            {
                this.VerifyIsEditable();
                if (description == value) return;

                description = value;

                OnPropertyChanged();
            }
        }

        public DateTime? CreationTime { get; }

        public DateTime? ModificationTime 
        {
            get => modificationTime;

            set
            {
                this.VerifyIsEditable();
                if (modificationTime == value) return;

                modificationTime = value;

                OnPropertyChanged();
            }
        }


        public AttachmentModelBase GetModel() => originalModel.Clone();
        
        public virtual void AcceptProps(AttachmentViewModelBase editableModel) 
        {
            this.VerifyIsReadonly();
            editableModel.VerifyIsEditable();

            Title = editableModel.Title;
            Description = editableModel.Description;
            ModificationTime = editableModel.ModificationTime;
        }


        private void SaveImpl() 
        {
            throw new NotImplementedException();
        }
    }
}