using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.DataBase.Context;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель альбома фотографий.
    /// </summary>
    [RelationalContext(typeof(PhotoAlbumsContext))]
    [Table(DbConst.TableNames.PhotoAlbumsTableName)]
    public class PhotoAlbumAttachmentModel : 
        AlbumAttachmentModelBase<PhotoAlbumAttachmentModel, PhotoAlbumInnerItemDescriptor, OfPhotoAlbumAttachmentDescriptor> 
    {
        public PhotoAlbumAttachmentModel() 
        {
            TypeOfAttachment = AttachmentType.PhotoAlbum;
        }
    }
}