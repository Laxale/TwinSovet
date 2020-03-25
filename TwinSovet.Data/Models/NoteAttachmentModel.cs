using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using SQLite.CodeFirst;
using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Модель аттачмента-заметки.
    /// </summary>
    [Table(DbConst.TableNames.NotesTableName)]
    public class NoteAttachmentModel : AttachmentModelBase 
    {
        /// <summary>
        /// Возвращает или задаёт дату создания заметки.
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату изменения заметки.
        /// </summary>
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату изменения заметки.
        /// </summary>
        [DefaultValue(DbConst.DefaulStringValue)]
        public string Text { get; set; }
    }
}