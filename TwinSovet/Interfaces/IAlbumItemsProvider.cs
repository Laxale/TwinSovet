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
    internal interface IAlbumItemsProvider<TAlbum, TDescriptor> : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric>
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TDescriptor>, new() 
        where TDescriptor : ChildAttachmentDescriptor<TAlbum>, new() 
    {
        
    }
}