using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class PhotoAlbumsConfiguration : BasicAttachmentConfiguration<PhotoAlbumAttachmentModel> 
    {
        public PhotoAlbumsConfiguration() 
        {
            // можно использовать атрибут [NotMapped]?
            //Ignore(album => album.ChildDescriptors);

            HasMany(album => album.AlbumCollectionDescriptors_Map)
                .WithRequired(photoDescriptor => (PhotoAlbumAttachmentModel)photoDescriptor.NavigationParent)
                .HasForeignKey(childDescriptor => childDescriptor.ParentId)
                .WillCascadeOnDelete(true);
        }
    }
}