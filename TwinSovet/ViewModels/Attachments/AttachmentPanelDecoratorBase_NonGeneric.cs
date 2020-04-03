using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;

using TwinSovet.Helpers;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс для возможности хранения всех возможных декораторов в одном <see cref="List{T}"/>.
    /// </summary>
    internal abstract class AttachmentPanelDecoratorBase_NonGeneric : ViewModelBase 
    {
        private bool isEditing;

        private SubjectEntityViewModelBase subject;



        protected AttachmentPanelDecoratorBase_NonGeneric() 
        {
            CommandEdit = new DelegateCommand(EditImpl);
            CommandCancelEdit = new DelegateCommand(CancelEditImpl, CanCancelEdit);
        }

        
        public DelegateCommand CommandEdit { get; }

        public DelegateCommand CommandCancelEdit { get; }

        
        /// <summary>
        /// Возвращает флаг - находится ли сейчас декоратор в состоянии редактирования.
        /// </summary>
        public bool IsEditing 
        {
            get => isEditing;

            protected set
            {
                if (isEditing == value) return;

                isEditing = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Задать субъекта-владельца данной вьюмодели. То есть ему принадлежит данный аттач.
        /// </summary>
        /// <param name="subject">Вьюмодель субъекта-владельца данного аттача.</param>
        public virtual void SetOwnerSubject(SubjectEntityViewModelBase subject) 
        {
            this.subject = subject;
        }


        protected abstract void OnCancelEdit();

        
        private void EditImpl() 
        {
            IsEditing = true;

            ClientCommands.CommanSaveAttachment.RaiseCanExecuteChanged();
        }

        private void CancelEditImpl() 
        {
            IsEditing = false;
            OnCancelEdit();
        }

        private bool CanCancelEdit() 
        {
            return IsEditing;
        }
    }
}