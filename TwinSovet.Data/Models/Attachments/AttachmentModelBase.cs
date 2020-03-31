using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Базовый класс моделей аттачментов.
    /// Логически является абстрактным, но таковым не сделан для возможности хранения в одном списке всех типов дочерних аттачей.
    /// </summary>
    public class AttachmentModelBase : ComplexDbObject 
    {
        /// <summary>
        /// Возвращает или задаёт название аттача.
        /// </summary>
        [DefaultValue(DbConst.DefaulStringValue)]
        public string Title { get; set; }

        /// <summary>
        /// Возвращает или задаёт текст описания аттача.
        /// </summary>
        [DefaultValue(DbConst.DefaulStringValue)]
        public string Description { get; set; }

        /// <summary>
        /// Возвращает или задаёт идентификатор корневого объекта, к которому приложен данный аттач.
        /// Данный идентификатор описывает ТОЛЬКО субъекты типа <see cref="SubjectType"/>.
        /// </summary>
        public string RootSubjectId { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип корневого объекта, к которому приложен данный аттач.
        /// </summary>
        public SubjectType RootSubjectType { get; set; }

        /// <summary>
        /// Возвращает или задаёт идентификатор объекта, к которому приложен данный аттач.
        /// </summary>
        public string HostId { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип объекта, к которому приложен данный аттач.
        /// </summary>
        public AttachmentHostType HostType { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип аттачмента.
        /// </summary>
        public AttachmentType TypeOfAttachment { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату создания заметки.
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату изменения заметки.
        /// </summary>
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        [NotMapped]
        public List<ChildAttachmentDescriptor> ChildDescriptors { get; set; } = new List<ChildAttachmentDescriptor>();

        /// <summary>
        /// Не использовать в коде! Коллекция для хранения свойства <see cref="ChildDescriptors"/> в базе.
        /// </summary>
        public virtual List<ChildAttachmentDescriptor> ChildDescriptors_Map { get; set; } = new List<ChildAttachmentDescriptor>();


        public virtual AttachmentModelBase Clone() => throw new NotImplementedException();

        public void AcceptProps(AttachmentModelBase other) 
        {
            Title = other.Title;
            Description = other.Description;
            ModificationTime = other.ModificationTime;
        }

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public override ComplexDbObject PrepareMappedProps() 
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override List<string> GetIncludedPropNames() 
        {
            throw new NotImplementedException();
        }
    }
}