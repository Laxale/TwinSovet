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
    public class OwnRelationModel : DbObject 
    {
        /// <summary>
        /// Возвращает или задаёт номер квартиры.
        /// </summary>
        public int FlatNumber { get; set; }

        /// <summary>
        /// Возвращает или задаёт идентификатор владельца квартиры.
        /// </summary>
        public string OwnerId { get; set; }


        /// <summary>Возвращает строку, представляющую текущий объект.</summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString() 
        {
            return $"Flat { FlatNumber } is owned by { OwnerId }";
        }
    }
}