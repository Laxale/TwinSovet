using System;

using DataVirtualization;

using Prism.Commands;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.ViewModels.Attachments 
{
    internal abstract class AttachmentPanelDecoratorBase<TAttachmentViewModel> : AttachmentPanelDecoratorBase_NonGeneric 
        where TAttachmentViewModel : AttachmentViewModelBase
    {
        protected AttachmentPanelDecoratorBase(TAttachmentViewModel editableViewModel
            //, bool cloneToReadonly
            ) 
        {
            EditableAttachmentViewModel = editableViewModel;

            //if (cloneToReadonly)
            //{
                ReadonlyAttachmentViewModel = (TAttachmentViewModel)AttachmentViewModelFactory.CreateReadonly(editableViewModel.GetModel());
            //}

            EditableAttachmentViewModel.EventExecutedAttachmentSave += EditableModel_OnExecutedAttachmentSave;
        }


        public TAttachmentViewModel ReadonlyAttachmentViewModel { get; }

        public TAttachmentViewModel EditableAttachmentViewModel { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> Children { get; private set; }


        /// <summary>
        /// Задать субъекта-владельца данной вьюмодели. То есть ему принадлежит данный аттач.
        /// </summary>
        /// <param name="subject">Вьюмодель субъекта-владельца данного аттача.</param>
        public override void SetOwnerSubject(SubjectEntityViewModelBase subject) 
        {
            base.SetOwnerSubject(subject);

            EditableAttachmentViewModel.SetOwnerSubject(subject);
            ReadonlyAttachmentViewModel.SetOwnerSubject(subject);
        }


        protected override void OnCancelEdit() 
        {
            EditableAttachmentViewModel.ResetToSaved(ReadonlyAttachmentViewModel);
        }


        private void EditableModel_OnExecutedAttachmentSave() 
        {
            IsEditing = false;
            ReadonlyAttachmentViewModel.AcceptEditableProps(EditableAttachmentViewModel);
        }
    }
}