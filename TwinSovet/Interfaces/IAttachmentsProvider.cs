using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization;
using TwinSovet.ViewModels;


namespace TwinSovet.Interfaces 
{
    internal interface _IAttachmentsProvider<TAttachmentDecorator, TAttachmentViewModel> : IItemsProvider<TAttachmentDecorator> 
        where TAttachmentDecorator : AttachmentPanelDecoratorBase<TAttachmentViewModel>
        where TAttachmentViewModel : AttachmentViewModelBase 
        //
    {
        void SetFilter(Func<TAttachmentDecorator, bool> predicate);
    }

    internal interface IAttachmentsProvider : IItemsProvider<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> 
    //
    {
        void SetFilter(Func<AttachmentPanelDecoratorBase<AttachmentViewModelBase>, bool> predicate);
    }
}