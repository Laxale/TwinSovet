using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class PhotosConfiguration : BasicAttachmentConfiguration<PhotoAttachmentModel> 
    {
        public PhotosConfiguration() 
        {
            HasMany(note => note.ChildAttachmentDescriptors)
                .WithRequired(descriptor => descriptor.NavigationParent)
                .HasForeignKey(descriptor => descriptor.ParentId)
                .WillCascadeOnDelete(true);
        }
    }
}