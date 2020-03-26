using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;


namespace TwinSovet.Data.Models 
{
    public class FloorModel 
    {
        public int FloorNumber { get; set; }

        public int MinFlatNumber { get; set; }

        public int MaxFlatNumber { get; set; }

        public SectionType Section { get; set; }
    }
}