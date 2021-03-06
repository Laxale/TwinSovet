﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public abstract class AttachmentModelBase : ComplexDbObject 
    {
        private const string DefaultTitle = "Название не указано";


        /// <summary>
        /// Возвращает или задаёт название аттача.
        /// </summary>
        [Required]
        [DefaultValue(DefaultTitle)]
        public string Title { get; set; } = DefaultTitle;

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
        [DefaultValue(DbConst.DefaulStringValue)]
        public string HostId { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип объекта, к которому приложен данный аттач.
        /// </summary>
        public AttachmentHostType HostType { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип аттачмента.
        /// </summary>
        [Required]
        public AttachmentType TypeOfAttachment { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату создания данного аттача.
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату изменения заметки.
        /// </summary>
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// Клонировать данную модель аттача.
        /// </summary>
        /// <returns>Клон данного аттача.</returns>
        public abstract AttachmentModelBase Clone();
        

        public virtual void AcceptModelProperties(AttachmentModelBase other) 
        {
            if (TypeOfAttachment != other.TypeOfAttachment)
            {
                throw new InvalidOperationException("Нельзя принимать свойства от модели другого типа");
            }

            Id = other.Id;
            Title = other.Title;
            Description = other.Description;
            CreationTime = other.CreationTime;
            ModificationTime = other.ModificationTime;
            RootSubjectId = other.RootSubjectId;
            RootSubjectType = other.RootSubjectType;
            HostType = other.HostType;
            HostId = other.HostId;
        }

        
        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        public override List<string> GetIncludedPropNames() 
        {
            return new List<string>();
        }
    }
}