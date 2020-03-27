using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using TwinSovet.Data.DataBase.Attributes;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.DataBase.Context;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель аттачмента-заметки.
    /// </summary>
    [Table(DbConst.TableNames.NotesTableName)]
    [RelationalContext(typeof(NoteAttachmentsContext))]
    public class NoteAttachmentModel : AttachmentModelBase 
    {
        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public List<OfNoteChildAttachmentsDescriptor> ChildDescriptors { get; set; } = new List<OfNoteChildAttachmentsDescriptor>();

        /// <summary>
        /// Не использовать в коде! Коллекция для хранения свойства <see cref="ChildDescriptors"/> в базе.
        /// </summary>
        public virtual List<OfNoteChildAttachmentsDescriptor> ChildDescriptors_Map { get; set; } = new List<OfNoteChildAttachmentsDescriptor>();


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

            clone.ChildDescriptors.AddRange(ChildDescriptors);

            return clone;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            ChildDescriptors_Map.Clear();
            ChildDescriptors_Map.AddRange(ChildDescriptors.Select(descriptor =>
            {
                var clone = new OfNoteChildAttachmentsDescriptor
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


        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override List<string> GetIncludedPropNames() 
        {
            return new List<string> { nameof(ChildDescriptors_Map) };
        }
    }
}