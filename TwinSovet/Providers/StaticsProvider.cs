using System.Collections.Generic;
using System.Security.Principal;

using TwinSovet.Data.Enums;


namespace TwinSovet.Providers 
{
    internal static class StaticsProvider 
    {
        /// <summary>
        /// 
        /// </summary>
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

            IsAdminMode = IsAdministrator();
        }


        public static bool IsAdminMode { get; }

        public static int SearchDelay { get; } = 300;

        public static int FlatsPerMebelFloor { get; } = 6;

        public static int FlatsPerHospitalFloor { get; } = 7;

        public static int FloorsCount { get; } = 20;

        public static IReadOnlyCollection<GenderType> AvailableGenders { get; }

        public static IReadOnlyCollection<SectionType> AvailableSectionNumbers { get; }


        public static bool IsAdministrator() 
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}