using DataVirtualization;


namespace TwinSovet.ViewModels.Attachments 
{
    internal abstract class AttachmentPanelDecoratorBase<TAttachmentViewModel> : AttachmentPanelDecoratorBase_NonGeneric 
        where TAttachmentViewModel : AttachmentViewModelBase 
    {
        public AttachmentPanelDecoratorBase(TAttachmentViewModel editableViewModel) 
        {
            EditableAttachmentViewModel = editableViewModel;

            ReadonlyAttachmentViewModel = (TAttachmentViewModel)AttachmentViewModelFactory.CreateReadonly(editableViewModel.GetModel());
        }


        public TAttachmentViewModel ReadonlyAttachmentViewModel { get; }

        public TAttachmentViewModel EditableAttachmentViewModel { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> Children { get; private set; }
    }
}