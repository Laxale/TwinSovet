using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;


namespace TwinSovet.Helpers 
{
    internal static class StaticsProvider 
    {
        static StaticsProvider() 
        {
            AvailableSectionNumbers = new List<SectionType> { SectionType.Furniture, SectionType.Hospital };

            AvailableGenders = new List<GenderType>
            {
                GenderType.None,
                GenderType.Man,
                GenderType.Woman,
                GenderType.Libertarian
            };
        }


        public static IReadOnlyCollection<GenderType> AvailableGenders { get; }

        public static IReadOnlyCollection<SectionType> AvailableSectionNumbers { get; }
    }
}