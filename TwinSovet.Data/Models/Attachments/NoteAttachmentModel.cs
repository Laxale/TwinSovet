using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.DataBase.Context;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель аттачмента-заметки.
    /// </summary>
    [Table(DbConst.TableNames.NotesTableName)]
    [RelationalContext(typeof(NoteAttachmentsContext))]
    public class NoteAttachmentModel : AttachmentModelBase 
    {
        public NoteAttachmentModel() 
        {
            TypeOfAttachment = AttachmentType.Note;
        }


        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public virtual List<OfNoteAttachmentDescriptor> ChildAttachmentDescriptors { get; set; } = new List<OfNoteAttachmentDescriptor>();


        public override AttachmentModelBase Clone() 
        {
            var clone = new NoteAttachmentModel
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
                HostId = HostId,
                HostType = HostType,
                RootSubjectId = RootSubjectId,
                RootSubjectType = RootSubjectType,
                TypeOfAttachment = TypeOfAttachment,
            };

            //clone.ChildDescriptors.AddRange(ChildDescriptors);

            return clone;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            ChildAttachmentDescriptors.Clear();
            
            ChildAttachmentDescriptors.AddRange(ChildAttachmentDescriptors.Select(descriptor =>
            {
                var clone = new OfNoteAttachmentDescriptor
                {
                    Id = descriptor.Id,
                    ChildAttachmentId = descriptor.ChildAttachmentId,
                    ChildAttachmentType = descriptor.ChildAttachmentType,
                    ParentId = this.Id,
                    NavigationParent = this
                };

                return clone;
            }));

            return this;
        }
    }
}