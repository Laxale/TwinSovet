using System;
using System.ComponentModel;

using Prism.Commands;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.Helpers;
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

        private string title;
        private string description;
        private DateTime? modificationTime;

        /// <summary>
        /// Событие успешного сохранения данных аттача.
        /// </summary>
        public event Action EventExecutedAttachmentSave = () => { };


        protected AttachmentViewModelBase(AttachmentModelBase attachmentModel, bool isReadonly) 
        {
            originalModel = attachmentModel;
            IsReadonly = isReadonly;

            BeginAcceptingProps();
            Title = attachmentModel.Title;
            Description = attachmentModel.Description;
            CreationTime = attachmentModel.CreationTime;
            ModificationTime = attachmentModel.ModificationTime;
            BeginAcceptingProps();

            PropertyChanged += Self_OnPropertyChanged;
        }


        public bool IsReadonly { get; }

        public bool ForceSkipReadonlyCheck { get; private set; }

        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public abstract AttachmentType EntityType { get; }

        public bool HasTitle { get; private set; }

        public string Title 
        {
            get => title;

            set
            {
                this.VerifyIsEditable();
                if (title == value) return;

                title = value;
                HasTitle = !string.IsNullOrWhiteSpace(value);

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTitle));
            }
        }

        public bool HasDescription { get; private set; }

        public string Description 
        {
            get => description;

            set
            {
                this.VerifyIsEditable();
                if (description == value) return;

                description = value;
                HasDescription = !string.IsNullOrWhiteSpace(value);

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasDescription));
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


        public void Save() 
        {
            this.VerifyIsEditable();

            AttachmentsProvider<AttachmentModelBase>.SaveOrUpdate(GetModel());

            EventExecutedAttachmentSave();
        }

        public bool CanSave() 
        {
            return HasTitle && HasDescription;
        }

        public AttachmentModelBase GetModel() 
        {
            var clone = originalModel.Clone();
            clone.Title = Title;
            clone.Description = Description;
            
            return clone;
        }

        public virtual void ResetToSaved(AttachmentViewModelBase readonlyModel) 
        {
            this.VerifyIsEditable();
            readonlyModel.VerifyIsReadonly();

            AcceptProps(readonlyModel);
        }

        public virtual void AcceptEditableProps(AttachmentViewModelBase editableModel) 
        {
            this.VerifyIsReadonly();
            editableModel.VerifyIsEditable();

            AcceptProps(editableModel);
        }


        protected void BeginAcceptingProps() 
        {
            ForceSkipReadonlyCheck = true;
        }

        protected void EndAcceptingProps() 
        {
            ForceSkipReadonlyCheck = false;
        }


        private void AcceptProps(AttachmentViewModelBase editableModel) 
        {
            BeginAcceptingProps();

            Title = editableModel.Title;
            Description = editableModel.Description;
            ModificationTime = editableModel.ModificationTime;

            EndAcceptingProps();
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(Title) || e.PropertyName == nameof(Description))
            {
                ClientCommands.CommanSaveAttachment.RaiseCanExecuteChanged();
            }
        }
    }
}