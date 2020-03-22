using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Providers
{
    internal static class FlatsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly Dictionary<string, FlatViewModel> aborigenFlats = new Dictionary<string, FlatViewModel>();


        public static FlatViewModel FindFlatOfAborigen(string aborigenId) 
        {
            lock (Locker)
            {
                if (aborigenFlats.TryGetValue(aborigenId, out FlatViewModel cachedFlat))
                {
                    return cachedFlat;
                }

                string flatId = RelationsProvider.GetFlatId(aborigenId);

                FlatViewModel flat = FloorsProvider.FindFlatById(flatId);

                aborigenFlats.Add(aborigenId, flat);

                return flat;
            }
        }
    }
}