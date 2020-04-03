using System;

using DataVirtualization;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Interfaces 
{
    internal interface IAlbumItemsProvider<TAlbum, TInnerDescriptor, TChildDescriptor> : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric>
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TInnerDescriptor, TChildDescriptor>, new() 
        where TInnerDescriptor : ChildAttachmentDescriptor<TAlbum>, new() 
        where TChildDescriptor : ChildAttachmentDescriptor<TAlbum>, new() 
    {
        
    }
}