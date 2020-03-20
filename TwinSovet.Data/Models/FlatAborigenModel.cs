using System.Collections.Generic;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Модель жильца или владельца площади в доме.
    /// </summary>
    public class FlatAborigenModel : DbObject 
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Otchestvo { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public GenderType Gender { get; set; }
    }
}