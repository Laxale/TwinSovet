using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Config;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Модель жильца или владельца площади в доме.
    /// </summary>
    [Table(DbConst.TableNames.AborigensTableName)]
    public class AborigenModel : DbObject 
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Otchestvo { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public GenderType Gender { get; set; }
    }
}