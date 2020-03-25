using System;
using System.Collections.Generic;

using TwinSovet.Interfaces;
using TwinSovet.ViewModels;


namespace TwinSovet.Providers 
{
    internal class AllFloorsProvider 
    {
        private static readonly object Locker = new object();

        private static AllFloorsProvider instance;


        private AllFloorsProvider() 
        {
            FurnitureFloorsProvider = SectionFloorsProvider.CreateFurnitureProvider();
            HospitalFloorsProvider = SectionFloorsProvider.CreateHospitalProvider();
        }


        /// <summary>
        /// Единственный инстанс <see cref="AllFloorsProvider"/>.
        /// </summary>
        public static AllFloorsProvider Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new AllFloorsProvider());
                }
            }
        }


        public IFloorsProvider FurnitureFloorsProvider { get; }

        public IFloorsProvider HospitalFloorsProvider { get; }


        public FlatViewModel FindFlatByNumber(int flatNumber) 
        {
            FlatViewModel flat = FurnitureFloorsProvider.FindFlatByNumber(flatNumber);

            if (flat != null) return flat;

            flat = HospitalFloorsProvider.FindFlatByNumber(flatNumber);

            if (flat == null)
            {
                throw new InvalidOperationException($"Квартиры с номером '{ flatNumber }' не существует");
            }

            return flat;
        }
    }
}