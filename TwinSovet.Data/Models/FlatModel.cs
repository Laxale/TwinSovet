using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models 
{
    public class FlatModel : DbObject 
    {
        /// <summary>
        /// Возвращает или задаёт номер квартиры.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Возвращает или задаёт площадь квартиры в метрах.
        /// </summary>
        public float Area { get; set; }

        /// <summary>
        /// Возвращает или задаёт номер этажа квартиры.
        /// </summary>
        public int FloorNumber { get; set; }

        /// <summary>
        /// Возвращает или задаёт тип секции квартиры.
        /// </summary>
        public SectionType Section { get; set; }
    }
}