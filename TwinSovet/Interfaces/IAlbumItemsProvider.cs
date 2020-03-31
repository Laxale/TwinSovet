using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Interfaces 
{
    internal interface IAlbumItemsProvider<TAttachmentDescriptor, out TAttachmentModel> : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric>
        where TAttachmentDescriptor : BinaryDescriptorModel
        where TAttachmentModel : BinaryAttachmentModel
    {
        
    }
}