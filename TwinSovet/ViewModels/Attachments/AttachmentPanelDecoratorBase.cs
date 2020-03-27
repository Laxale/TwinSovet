using DataVirtualization;


namespace TwinSovet.ViewModels.Attachments 
{
    internal abstract class AttachmentPanelDecoratorBase<TAttachmentViewModel> : AttachmentPanelDecoratorBase_NonGeneric 
        where TAttachmentViewModel : AttachmentViewModelBase 
    {
        public AttachmentPanelDecoratorBase(TAttachmentViewModel attachmentViewModel) 
        {
            AttachmentViewModel = attachmentViewModel;
        }


        public TAttachmentViewModel AttachmentViewModel { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> Children { get; private set; }
    }
}