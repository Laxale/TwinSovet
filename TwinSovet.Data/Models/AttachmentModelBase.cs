using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Базовый класс моделей аттачментов.
    /// </summary>
    public abstract class AttachmentModelBase : DbObject 
    {
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
    }
}