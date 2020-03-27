using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models 
{
    [Table(DbConst.TableNames.BlobsTableName)]
    public class BinaryDataDescriptor : SimpleDbObject 
    {
        /// <summary>
        /// Возвращает или задаёт тип данных - имеется в виду расширение файла, которому соответствует блоб.
        /// </summary>
        public string DataType { get; set; }

        public bool IsCompressed { get; set; }

        public byte[] Blob { get; set; }
    }
}