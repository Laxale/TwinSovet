using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization;

namespace TwinSovet.ViewModels 
{
    internal abstract class AttachmentPanelDecoratorBase<TAttachmentViewModel> where TAttachmentViewModel : AttachmentViewModelBase 
    {
        public AttachmentPanelDecoratorBase(TAttachmentViewModel attachmentViewModel) 
        {
            AttachmentViewModel = attachmentViewModel;
        }


        public TAttachmentViewModel AttachmentViewModel { get; }

        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> Children { get; private set; }
    }
}