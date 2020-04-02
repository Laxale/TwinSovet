using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    public class DocumentAlbumAttachmentModel : 
        AlbumAttachmentModelBase<DocumentAlbumAttachmentModel, DocumentAlbumInnerItemDescriptor, OfDocumentAlbumAttachmentDescriptor> 
    {
        public DocumentAlbumAttachmentModel() 
        {
            TypeOfAttachment = AttachmentType.DocumentAlbum;
        }
    }
}