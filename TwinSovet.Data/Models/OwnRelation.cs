using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Представляет отношение владения квартирой.
    /// </summary>
    public class OwnRelation : DbObject 
    {
        public FlatModel Flat { get; }

        public AborigenModel Owner { get; set; }
    }
}