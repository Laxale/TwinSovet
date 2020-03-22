using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.ViewModels;


namespace TwinSovet.Providers
{
    internal static class FloorsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly int startMebelFlatNumber = 1;
        private static readonly int startHospitalFlatNumber = 121;
        private static readonly Dictionary<SectionType, List<FloorViewModel>> sectionFloors = new Dictionary<SectionType, List<FloorViewModel>>();


        public static FlatViewModel FindFlatById(string flatId) 
        {
            lock (Locker)
            {
                if (!sectionFloors.ContainsKey(SectionType.Furniture))
                {
                    GetFloors(SectionType.Furniture);
                }
                if (!sectionFloors.ContainsKey(SectionType.Hospital))
                {
                    GetFloors(SectionType.Hospital);
                }

                return FindFlat(flatId);
            }
        }

        private static FlatViewModel FindFlat(string flatId) 
        {
            FlatViewModel mebelFlat =
                sectionFloors[SectionType.Furniture]
                    .SelectMany(floor => floor.FlatsEnumerable)
                    .FirstOrDefault(flat => flat.GetId() == flatId);
            if (mebelFlat != null)
            {
                return mebelFlat;
            }

            FlatViewModel hospitalFlat =
                sectionFloors[SectionType.Hospital]
                    .SelectMany(floor => floor.FlatsEnumerable)
                    .FirstOrDefault(flat => flat.GetId() == flatId);

            return hospitalFlat;
        }

        public static IEnumerable<FloorViewModel> GetFloors(SectionType section) 
        {
            if (section == SectionType.None)
            {
                throw new InvalidOperationException($"Неожиданное значение секции '{ section }'");
            }

            lock (Locker)
            {
                if (sectionFloors.TryGetValue(section, out List<FloorViewModel> floors))
                {
                    return floors;
                }

                int flatsPerFloor =
                    section == SectionType.Furniture ?
                        StaticsProvider.FlatsPerMebelFloor : 
                        StaticsProvider.FlatsPerHospitalFloor ;
                int startFlatNumber =
                    section == SectionType.Furniture ?
                        startMebelFlatNumber :
                        startHospitalFlatNumber;

                List<FloorViewModel> floorModels = CreateFloorsForSection(section, flatsPerFloor, startFlatNumber);

                sectionFloors.Add(section, floorModels);

                return floorModels;
            }
        }


        private static List<FloorViewModel> CreateFloorsForSection(SectionType section, int flatsPerFloor, int startFlatNumber) 
        {
            int startNumber = startFlatNumber;
            var floorModels = new List<FloorViewModel>();
            for (int floorIndex = 1; floorIndex <= StaticsProvider.FloorsCount; floorIndex++)
            {
                var floorViewModel = new FloorViewModel { FloorNumber = floorIndex, Section = section };

                var flats = new List<FlatViewModel>();
                for (int flatIndex = 0; flatIndex < flatsPerFloor; flatIndex++)
                {
                    var flatModel = new FlatModel
                    {
                        Area = 40,
                        FloorNumber = floorViewModel.FloorNumber,
                        Number = startNumber,
                        Section = section
                    };
                    var flat = new FlatViewModel(flatModel);
                    flats.Add(flat);

                    startNumber++;
                }

                floorViewModel.SetFlats(flats);
                floorModels.Add(floorViewModel);
            }

            floorModels.Reverse();

            return floorModels;
        }
    }
}