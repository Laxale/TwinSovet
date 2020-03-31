using System;
using System.Collections.Generic;
using System.Linq;

using DataVirtualization;

using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Interfaces 
{
    /// <summary>
    /// Интерфейс для реализации провайдера аттачей.
    /// </summary>
    internal interface IAttachmentsProvider : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric> 
    {
        
    }
}