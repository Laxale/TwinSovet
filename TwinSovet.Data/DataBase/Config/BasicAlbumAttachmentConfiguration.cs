using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal abstract class BasicAlbumAttachmentConfiguration<TAlbum, TDescriptor> : BasicAttachmentConfiguration<TAlbum> 
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TDescriptor>, new()
        where TDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
    {
        protected BasicAlbumAttachmentConfiguration() 
        {
            //HasMany(album => album.ChildAttachmentDescriptors).WithRequired(childDescriptor => childDescriptor.NavigationParent).HasForeignKey(childDescriptor => childDescriptor.ParentId).WillCascadeOnDelete(true);

            //HasMany(album => album.AlbumCollectionDescriptors).WithRequired(itemDescriptor => itemDescriptor.NavigationParent).HasForeignKey(childDescriptor => childDescriptor.ParentId).WillCascadeOnDelete(true);
        }
    }
}