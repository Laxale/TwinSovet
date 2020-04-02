using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Configuration;

using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class PhotoAlbumsConfiguration : BasicAlbumAttachmentConfiguration<PhotoAlbumAttachmentModel, PhotoAlbumInnerItemDescriptor, OfPhotoAlbumAttachmentDescriptor> 
    {
        public PhotoAlbumsConfiguration() 
        {
            //ToTable(DbConst.TableNames.PhotoAlbumsTableName);
            // вызывается в базовом классе
            //Map(config => config.MapInheritedProperties());

        }
    }
}