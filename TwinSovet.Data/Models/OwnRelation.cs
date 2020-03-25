using System;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Представляет отношение владения квартирой.
    /// </summary>
    [Table(DbConst.TableNames.OwnRelationsTableName)]
    public class OwnRelation : DbObject 
    {
        /// <summary>
        /// Возвращает или задаёт номер квартиры.
        /// </summary>
        public int FlatNumber { get; set; }

        /// <summary>
        /// Возвращает или задаёт идентификатор владельца квартиры.
        /// </summary>
        public string OwnerId { get; set; }
    }
}