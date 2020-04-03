using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Commands;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Subjects;


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

        protected SubjectEntityViewModelBase subject;

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
            EndAcceptingProps();

            PropertyChanged += Self_OnPropertyChanged;
        }


        /// <summary>
        /// Возвращает флаг - является ли данная вьюмодель 'readonly'.
        /// </summary>
        public bool IsReadonly { get; }

        /// <summary>
        /// Возвращает флаг - форсирует ли данная вьюмодель пропуск проверок на 'readonly'.
        /// </summary>
        public bool ForceSkipReadonlyCheck { get; private set; }

        /// <summary>
        /// Возвращает тип данного аттачмента.
        /// </summary>
        public abstract AttachmentType TypeOfAttachment { get; }

        /// <summary>
        /// Возвращает флаг - есть ли у данной модели НЕпустое название.
        /// </summary>
        public bool HasTitle { get; private set; }

        /// <summary>
        /// Возвращает или задаёт название аттача данной вьюмодели.
        /// </summary>
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

        /// <summary>
        /// Возвращает флаг - заполнено ли сейчас у вьюмодели НЕпустое поле <see cref="Description"/>.
        /// </summary>
        public bool HasDescription { get; private set; }

        /// <summary>
        /// Возвращает или задаёт описание аттача данной вьюмодели.
        /// </summary>
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

            AttachmentModelBase updatedModel = GetModel();
            PrepareOriginalForSaving(updatedModel);
            AttachmentsProvider<NoteAttachmentModel>.SaveOrUpdate(updatedModel);

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

        /// <summary>
        /// Задать субъекта-владельца данной вьюмодели. То есть ему принадлежит данный аттач.
        /// </summary>
        /// <param name="subject">Вьюмодель субъекта-владельца данного аттача.</param>
        public virtual void SetOwnerSubject(SubjectEntityViewModelBase subject) 
        {
            this.subject = subject;
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

        protected virtual void PrepareOriginalForSaving(AttachmentModelBase clonedOriginalModel) 
        {

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