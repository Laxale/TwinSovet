using System;
using System.Collections.Generic;
using System.Linq;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Providers 
{
    /// <summary>
    /// Реализация <see cref="IFloorsProvider"/> для конкретной секции дома.
    /// </summary>
    internal class SectionFloorsProvider : IFloorsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly Dictionary<int, float> mebelFlatAreas = new Dictionary<int, float>
        {
            { 1, 30 }, { 2, 35 }, { 3, 40 }, { 4, 45 }, { 5, 50 }, { 6, 55 }
        };
        private static readonly Dictionary<int, float> hospitalFlatAreas = new Dictionary<int, float>
        {
            { 1, 30 }, { 2, 35 }, { 3, 40 }, { 4, 45 }, { 5, 50 }, { 6, 55 }, {7, 60}
        };

        private readonly SectionType sectionType;
        private readonly List<FloorDecoratorViewModel> allFloors = new List<FloorDecoratorViewModel>();
        private readonly List<FloorDecoratorViewModel> predicatedFloors = new List<FloorDecoratorViewModel>();


        private SectionFloorsProvider(SectionType sectionType) 
        {
            if (sectionType == SectionType.None)
            {
                throw new InvalidOperationException($"Нельзя инициализировать провайдера значением '{ SectionType.None }'");
            }

            this.sectionType = sectionType;
        }


        public static IFloorsProvider CreateFurnitureProvider() 
        {
            return new SectionFloorsProvider(SectionType.Furniture);
        }

        public static IFloorsProvider CreateHospitalProvider() 
        {
            return new SectionFloorsProvider(SectionType.Hospital);
        }


        public FlatViewModel FindFlatByNumber(int flatNumber) 
        {
            lock (Locker)
            {
                VerifyHasInitedFloors();

                return FindFlat(flatNumber);
            }
        }

        public void ForEach(Action<FloorDecoratorViewModel> action) 
        {
            lock (Locker)
            {
                VerifyHasInitedFloors();

                allFloors.ForEach(action);
            }
        }

        public void SetFilter(Func<FloorDecoratorViewModel, bool> predicate) 
        {
            lock (Locker)
            {
                VerifyHasInitedFloors();

                predicatedFloors.Clear();
                predicatedFloors.AddRange( predicate == null ? allFloors : allFloors.Where(predicate));
            }
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount() 
        {
            lock (Locker)
            {
                VerifyHasInitedFloors();

                return predicatedFloors.Count;
            }
        }

        /// <summary>
        /// Проверить наличие кэшированного декоратора по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для проверки.</param>
        /// <returns>True, если есть хоть один декоратор, удовлетворяющий предикату.</returns>
        public bool Any(Func<FloorDecoratorViewModel, bool> predicate) 
        {
            lock (Locker)
            {
                VerifyHasInitedFloors();

                return predicate == null ? predicatedFloors.Any() : predicatedFloors.Any(predicate);
            }
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsCount">Items count to fetch.</param>
        /// <param name="overallCount">Total count of items in storage.</param>
        /// <returns></returns>
        public IList<FloorDecoratorViewModel> FetchRange(int startIndex, int itemsCount, out int overallCount) 
        {
            lock (Locker)
            {
                overallCount = predicatedFloors.Count;
                List<FloorDecoratorViewModel> decorators = predicatedFloors.Skip(startIndex).Take(itemsCount).ToList();

                //System.Threading.Thread.Sleep(1000);
                OnBeforeFetching(decorators);

                return decorators;
            }
        }


        private void VerifyHasInitedFloors() 
        {
            if (allFloors.Any()) return;

            int flatsPerFloor =
                sectionType == SectionType.Furniture ?
                    StaticsProvider.FlatsPerMebelFloor :
                    StaticsProvider.FlatsPerHospitalFloor;

            int startFlatNumber =
                sectionType == SectionType.Furniture ?
                    StaticsProvider.MinFlatNumber :
                    StaticsProvider.FlatsInFurnitureSection + 1;

            FillFloorsForSection(flatsPerFloor, startFlatNumber);
        }

        private FlatViewModel FindFlat(int flatNumber) 
        {
            FlatDecoratorViewModel targetFlat =
                allFloors
                    .SelectMany(decorator => decorator.OriginaFloorViewModel.FlatsEnumerable)
                    .FirstOrDefault(flatDecorator => flatDecorator.Flat.Number == flatNumber);

            return targetFlat?.Flat;
        }

        private void FillFloorsForSection(int flatsPerFloor, int startFlatNumber) 
        {
            const int startFloorNumber = 2;

            int sectionStartFlatNumber = startFlatNumber;
            Dictionary<int, float> areasDict = sectionType == SectionType.Furniture ? mebelFlatAreas : hospitalFlatAreas;

            for (int floorNumber = startFloorNumber; floorNumber <= StaticsProvider.TotalFloorsCount; floorNumber++)
            {
                var floorModel = new FloorModel
                {
                    FloorNumber = floorNumber, Section = sectionType,
                    MinFlatNumber = sectionStartFlatNumber + (floorNumber - startFloorNumber) * flatsPerFloor,
                };
                floorModel.MaxFlatNumber = floorModel.MinFlatNumber + flatsPerFloor - 1;

                var floorViewModel = new FloorViewModel(floorModel);

                var flats = new List<FlatViewModel>();
                for (int flatIndex = 1; flatIndex < flatsPerFloor + 1; flatIndex++)
                {
                    var flatModel = new FlatModel
                    {
                        Area = areasDict[flatIndex],
                        FloorNumber = floorViewModel.FloorNumber,
                        Number = startFlatNumber,
                        Section = sectionType
                    };
                    var flat = new FlatViewModel(flatModel);
                    flats.Add(flat);

                    startFlatNumber++;
                }

                floorViewModel.PopulateFlats(flats);
                var decorator = new FloorDecoratorViewModel(floorViewModel);
                allFloors.Add(decorator);
            }

            allFloors.Reverse();
            predicatedFloors.AddRange(allFloors);
        }

        private void OnBeforeFetching(List<FloorDecoratorViewModel> fetchedFloors) 
        {
            if (fetchedFloors.Any())
            {
                Console.WriteLine($"fetching '{ fetchedFloors.Count }' floors for '{ fetchedFloors[0].OriginaFloorViewModel.Section }'");
            }
            else
            {
                Console.WriteLine("fetching 0 floors");
            }
            foreach (FloorDecoratorViewModel fetchedFloor in fetchedFloors)
            {
                LoadFlatOwners(fetchedFloor);
            }
        }
        private void LoadFlatOwners(FloorDecoratorViewModel floorDecorator) 
        {
            foreach (FlatDecoratorViewModel flatDecorator in floorDecorator.OriginaFloorViewModel.FlatsEnumerable)
            {
                AborigenDecoratorViewModel owner = RelationsProvider.GetFlatOwner(flatDecorator.Flat.Number);

                flatDecorator.SetOwner(owner);
            }
        }
    }
}