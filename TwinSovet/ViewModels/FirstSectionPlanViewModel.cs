using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using PubSub;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Helpers;
using TwinSovet.Messages;


namespace TwinSovet.ViewModels 
{
    internal class FirstSectionPlanViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<FloorViewModel> floors = new ObservableCollection<FloorViewModel>();

        private int minFlatNumber;
        private int maxFlatNumber;


        public FirstSectionPlanViewModel() 
        {
            FloorsView = CollectionViewSource.GetDefaultView(floors);

            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план мебельной секции"));
        }

        
        public ICollectionView FloorsView { get; }

        public int MinFlatNumber 
        {
            get => minFlatNumber;

            set
            {
                if (minFlatNumber == value) return;

                minFlatNumber = value;

                OnPropertyChanged();
            }
        }

        public int MaxFlatNumber 
        {
            get => maxFlatNumber;

            set
            {
                if (maxFlatNumber == value) return;

                maxFlatNumber = value;

                OnPropertyChanged();
            }
        }


        public void SetFloors(IEnumerable<FloorViewModel> floors) 
        {
            this.floors.Clear();
            this.floors.AddRange(floors);

            MinFlatNumber = this.floors.Min(floor => floor.FlatsView.OfType<FlatViewModel>().Min(flat => flat.Number));
            MaxFlatNumber = this.floors.Max(floor => floor.FlatsView.OfType<FlatViewModel>().Max(flat => flat.Number));
        }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            int flatNumber = 1;
            var floorModels = new List<FloorViewModel>();
            for (int floorIndex = 1; floorIndex <= StaticsProvider.FloorsCount; floorIndex++)
            {
                var floorViewModel = new FloorViewModel { FloorNumber = floorIndex, Section = SectionType.Furniture };

                var flats = new List<FlatViewModel>();
                for (int flatIndex = 0; flatIndex < StaticsProvider.FlatsPerMebelFloor; flatIndex++)
                {
                    var flatModel = new FlatModel
                    {
                        Area = 40,
                        FloorNumber = floorViewModel.FloorNumber,
                        Number = flatNumber,
                        Section = SectionType.Furniture
                    };
                    var flat = new FlatViewModel(flatModel);
                    flats.Add(flat);

                    flatNumber++;
                }

                floorViewModel.SetFlats(flats);
                floorModels.Add(floorViewModel);
            }

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                SetFloors(floorModels);
            });
            //SetFloors(floors);
        }
    }
}