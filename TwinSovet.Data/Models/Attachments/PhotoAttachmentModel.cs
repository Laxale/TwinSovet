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
    [Table(DbConst.TableNames.PhotosTableName)]
    [RelationalContext(typeof(PhotoAttachmentsContext))]
    public class PhotoAttachmentModel : BinaryAttachmentModel 
    {
        public PhotoAttachmentModel() 
        {
            TypeOfAttachment = AttachmentType.Photo;
        }


        public byte[] PreviewDataBlob { get; set; }
        
        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public List<OfPhotoAttachmentDescriptor> ChildAttachmentDescriptors = new List<OfPhotoAttachmentDescriptor>();


        public override AttachmentModelBase Clone() 
        {
            var clone = new PhotoAttachmentModel();
            clone.AcceptProps(this);

            clone.FullDataDescriptorId = this.FullDataDescriptorId;
            clone.PreviewDataBlob = this.PreviewDataBlob.ToArray();

            return clone;
        }
    }
}