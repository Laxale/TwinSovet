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
    internal class OfPhotoDescriptorConfiguration : EntityTypeConfiguration<OfPhotoAttachmentDescriptor> 
    {
        public OfPhotoDescriptorConfiguration() 
        {
            Map(config =>
                {
                    config
                        .MapInheritedProperties()
                        .ToTable(DbConst.TableNames.ChildAttachmentDescriptorsTableName);
                });
            
            HasRequired(descriptor => descriptor.NavigationParent)
                .WithMany(photo => photo.ChildAttachmentDescriptors)
                .HasForeignKey(desc => desc.ParentId)
                //.HasForeignKey(desc => desc.NavigationParent)
                .WillCascadeOnDelete(true)
                ;
        }
    }
}