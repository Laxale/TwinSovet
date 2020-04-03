using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Base;
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
        public List<OfPhotoAttachmentDescriptor> ChildAttachmentDescriptors { get; set; } = new List<OfPhotoAttachmentDescriptor>();


        public override AttachmentModelBase Clone() 
        {
            var clone = new PhotoAttachmentModel();
            clone.AcceptModelProperties(this);

            clone.FullDataDescriptorId = this.FullDataDescriptorId;
            clone.PreviewDataBlob = this.PreviewDataBlob.ToArray();

            return clone;
        }

        /// <summary>
        /// Принять свойства редактированной копии исходного объекта в базе для сохранения изменений.
        /// </summary>
        /// <returns></returns>
        public override void AcceptProps(ComplexDbObject other) 
        {
            base.AcceptProps(other);

            var photoOther = (PhotoAttachmentModel) other;
            PreviewDataBlob = photoOther.PreviewDataBlob;
            ChildAttachmentDescriptors.Clear();
            ChildAttachmentDescriptors.AddRange(photoOther.ChildAttachmentDescriptors);
        }

        public override void PrepareNavigationProps()
        {
            ChildAttachmentDescriptors.ForEach(desc => desc.NavigationParent = null);
        }
    }
}