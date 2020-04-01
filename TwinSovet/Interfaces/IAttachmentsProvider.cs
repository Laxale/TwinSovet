using System;
using System.Collections.Generic;
using System.Linq;

using DataVirtualization;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Interfaces 
{
    /// <summary>
    /// Интерфейс для реализации провайдера аттачей.
    /// </summary>
    internal interface IAttachmentsProvider<TAttachmentModel> : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric> 
        where TAttachmentModel : AttachmentModelBase
    {
        
    }
}