using System;

using DataVirtualization;

using Prism.Commands;


namespace TwinSovet.ViewModels.Attachments 
{
    internal abstract class AttachmentPanelDecoratorBase<TAttachmentViewModel> : AttachmentPanelDecoratorBase_NonGeneric 
        where TAttachmentViewModel : AttachmentViewModelBase
    {
        protected AttachmentPanelDecoratorBase(TAttachmentViewModel editableViewModel) 
        {
            EditableAttachmentViewModel = editableViewModel;
            ReadonlyAttachmentViewModel = (TAttachmentViewModel)AttachmentViewModelFactory.CreateReadonly(editableViewModel.GetModel());

            EditableAttachmentViewModel.EventExecutedAttachmentSave += EditableModel_OnExecutedAttachmentSave;
        }


        public TAttachmentViewModel ReadonlyAttachmentViewModel { get; }

        public TAttachmentViewModel EditableAttachmentViewModel { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> Children { get; private set; }


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