using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class AlbumInnerDescriptorsConfiguration<TAlbum, TInnerDescriptor, TChildDescriptor> : EntityTypeConfiguration<TInnerDescriptor> 
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TInnerDescriptor, TChildDescriptor>, new()
        where TInnerDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
        where TChildDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
    {
        public AlbumInnerDescriptorsConfiguration() 
        {
            Map(config =>
            {
                config
                    .MapInheritedProperties()
                    .ToTable(DbConst.TableNames.AlbumInnerItemDescriptorsTableName);
            });

            HasRequired(descriptor => descriptor.NavigationParent)
                .WithMany(album => album.AlbumCollectionDescriptors)
                .HasForeignKey(descriptor => descriptor.ParentId)
                .WillCascadeOnDelete();
        }
    }
}