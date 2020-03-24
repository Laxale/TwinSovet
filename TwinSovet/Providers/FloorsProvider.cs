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
        private static readonly Dictionary<int, float> mebelFlatAreas = new Dictionary<int, float>
        {
            { 1, 30 }, { 2, 35 }, { 3, 40 }, { 4, 45 }, { 5, 50 }, { 6, 55 }
        };
        private static readonly Dictionary<int, float> hospitalFlatAreas = new Dictionary<int, float>
        {
            { 1, 30 }, { 2, 35 }, { 3, 40 }, { 4, 45 }, { 5, 50 }, { 6, 55 }, {7, 60}
        };


        public static FlatViewModel FindFlatByNumber(int flatNumber) 
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

                return FindFlat(flatNumber);
            }
        }

        private static FlatViewModel FindFlat(int flatNumber) 
        {
            var targetDecorator =
                sectionFloors[SectionType.Furniture]
                    .SelectMany(floor => floor.FlatsEnumerable)
                    .FirstOrDefault(flatDecorator => flatDecorator.Flat.Number == flatNumber);
            if (targetDecorator != null)
            {
                return targetDecorator.Flat;
            }

            var hospitalFlat =
                sectionFloors[SectionType.Hospital]
                    .SelectMany(floor => floor.FlatsEnumerable)
                    .FirstOrDefault(decorator => decorator.Flat.Number == flatNumber);

            if (hospitalFlat == null)
            {
                throw new InvalidOperationException($"Квартиры с номером '{ flatNumber }' не существует");
            }

            return hospitalFlat.Flat;
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
            Dictionary<int, float> areasDict = section == SectionType.Furniture ? mebelFlatAreas : hospitalFlatAreas;

            for (int floorIndex = 1; floorIndex <= StaticsProvider.FloorsCount; floorIndex++)
            {
                var floorViewModel = new FloorViewModel { FloorNumber = floorIndex, Section = section };

                var flats = new List<FlatViewModel>();
                for (int flatIndex = 1; flatIndex < flatsPerFloor + 1; flatIndex++)
                {
                    var flatModel = new FlatModel
                    {
                        Area = areasDict[flatIndex],
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