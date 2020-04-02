using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class OfPhotoAlbumDescriptorConfiguration : EntityTypeConfiguration<OfPhotoAlbumAttachmentDescriptor> 
    {
        public OfPhotoAlbumDescriptorConfiguration() 
        {
            Map(config =>
            {
                config
                    .MapInheritedProperties()
                    .ToTable(DbConst.TableNames.ChildAttachmentDescriptorsTableName);
            });

            HasRequired(descriptor => descriptor.NavigationParent)
                .WithMany(album => album.ChildAttachmentDescriptors)
                .HasForeignKey(descriptor => descriptor.ParentId)
                .WillCascadeOnDelete();
        }
    }
}