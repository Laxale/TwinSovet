using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Модель дескриптора довичных данных в базе.
    /// </summary>
    [Table(DbConst.TableNames.BlobsTableName)]
    public class BinaryDataDescriptor : SimpleDbObject 
    {
        /// <summary>
        /// Возвращает или задаёт тип данных - имеется в виду расширение файла, которому соответствует блоб.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Возвращает или задаёт флаг - является ли данный блоб сжатым (архивированным).
        /// </summary>
        public bool IsCompressed { get; set; }

        /// <summary>
        /// Возвращает или задаёт блоб данных.
        /// </summary>
        public byte[] Blob { get; set; }
    }
}