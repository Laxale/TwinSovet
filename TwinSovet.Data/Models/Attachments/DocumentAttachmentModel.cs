using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.DataBase.Context;


namespace TwinSovet.Data.Models.Attachments 
{
    [Table(DbConst.TableNames.DocumentsTableName)]
    [RelationalContext(typeof(DocumentAttachmentsContext))]
    public class DocumentAttachmentModel : DescriptableDataAttachmentModel 
    {
        public string DataType { get; set; }

        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public List<OfDocumentChildAttachmentsDescriptor> ChildDescriptors { get; set; } = new List<OfDocumentChildAttachmentsDescriptor>();

        /// <summary>
        /// Не использовать в коде! Коллекция для хранения свойства <see cref="ChildDescriptors"/> в базе.
        /// </summary>
        public virtual List<OfDocumentChildAttachmentsDescriptor> ChildDescriptors_Map { get; set; } = new List<OfDocumentChildAttachmentsDescriptor>();
    }
}