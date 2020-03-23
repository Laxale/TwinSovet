using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Data.Enums;
using TwinSovet.ViewModels;


namespace TwinSovet.Providers 
{
    internal static class RelationsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly Dictionary<string, string> aborigenFlats = new Dictionary<string, string>();


        public static string GetFlatId(string aborigenId) 
        {
            lock (Locker)
            {
                if (aborigenFlats.TryGetValue(aborigenId, out string cachedFlatId))
                {
                    return cachedFlatId;
                }

                IEnumerable<FloorViewModel> floors = FloorsProvider.GetFloors(SectionType.Furniture);
                int randomFloorIndex = new Random().Next(1, 20);
                int randomFlatIndex = new Random().Next(1, 6);

                string flatId = floors.ElementAt(randomFloorIndex).FlatsEnumerable.ElementAt(randomFlatIndex).Flat?.GetId();

                aborigenFlats.Add(aborigenId, flatId);

                return flatId;
            }
        }
    }
}