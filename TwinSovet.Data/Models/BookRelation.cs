using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Представляет отношение съёма квартиры.
    /// </summary>
    public class BookRelation : DbObject 
    {
        public FlatModel Flat { get; }

        public AborigenModel Booker { get; set; }
    }
}