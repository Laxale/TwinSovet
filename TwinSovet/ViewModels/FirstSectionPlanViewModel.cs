using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Commands;

using PubSub;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Helpers;
using TwinSovet.Messages;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels 
{
    internal class FirstSectionPlanViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<FloorViewModel> floors = new ObservableCollection<FloorViewModel>();

        private int minFlatNumber;
        private int maxFlatNumber;
        private string flatFilterText;
        private string floorFilterText;
        private bool isOrphanHighlighted = true;


        public FirstSectionPlanViewModel() 
        {
            FloorsView = CollectionViewSource.GetDefaultView(floors);

            FloorsEnumerable = floors;

            CommandHighlightOrphanFlats = new DelegateCommand(HighlightOrphanFlatsImpl, () => IsReady);

            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план мебельной секции"));
        }


        public DelegateCommand CommandHighlightOrphanFlats { get; }

        
        public ICollectionView FloorsView { get; }

        public IEnumerable<FloorViewModel> FloorsEnumerable { get; }

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

        public bool HasFloorFilter => !string.IsNullOrWhiteSpace(FloorFilterText);

        public string FloorFilterText 
        {
            get => floorFilterText;

            set
            {
                if (floorFilterText == value) return;

                floorFilterText = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FloorFilterText));
            }
        }

        public bool HasFlatFilter => !string.IsNullOrWhiteSpace(FlatFilterText);

        public string FlatFilterText 
        {
            get => flatFilterText;

            set
            {
                if (flatFilterText == value) return;

                flatFilterText = value;

                OnPropertyChanged();
            }
        }


        private string LoweredFlatFilter => FlatFilterText?.ToLowerInvariant();

        private string LoweredFloorFilter => FloorFilterText?.ToLowerInvariant();


        public void SetFloors(IEnumerable<FloorViewModel> floorModels) 
        {
            floors.Clear();
            floors.AddRange(floorModels);

            MinFlatNumber = floors.Min(floor => floor.FlatsView.OfType<FlatViewModel>().Min(flat => flat.Number));
            MaxFlatNumber = floors.Max(floor => floor.FlatsView.OfType<FlatViewModel>().Max(flat => flat.Number));
        }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            IEnumerable<FloorViewModel> floorModels = FloorsProvider.GetFloors(SectionType.Furniture);
            
            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                SetFloors(floorModels);
            });
            
            PropertyChanged += Self_OnPropertyChanged;
        }


        private void HighlightOrphanFlatsImpl() 
        {
            floors.ForEach(floor => floor.FlatsEnumerable.ForEach(flat =>
            {
                if (!flat.HasOwner)
                {
                    flat.IsOrphanHighlighted = isOrphanHighlighted;
                }
            }));

            isOrphanHighlighted = !isOrphanHighlighted;
        }

        private bool IsFloorInFilter(object floorObj) 
        {
            var floor = (FloorViewModel)floorObj;

            return floor.FloorNumber.ToString().ToLowerInvariant().Contains(LoweredFloorFilter);
        }

        private bool IsFlatInFilter(object floorObj) 
        {
            var floor = (FloorViewModel)floorObj;
            string loweredFlatFiler = LoweredFlatFilter;

            bool hasAnyFlats = false;
            foreach (FlatViewModel flatViewModel in floor.FlatsEnumerable)
            {
                if (flatViewModel.Number.ToString().ToLowerInvariant().Contains(loweredFlatFiler))
                {
                    hasAnyFlats = true;
                    flatViewModel.IsHighlighted = true;
                }
                else
                {
                    flatViewModel.IsHighlighted = false;
                }
            }

            return hasAnyFlats;
        }

        private void SetFlatFilters() 
        {
            foreach (FloorViewModel floor in floors)
            {
                floor.FilterText = FloorFilterText;
            }
        }

        private void ClearFlatsHighlighting() 
        {
            FloorsEnumerable.ForEach(floor => floor.FlatsEnumerable.ForEach(flat => flat.IsHighlighted = false));
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FloorFilterText))
            {
                if (HasFlatFilter)
                {
                    PropertyChanged -= Self_OnPropertyChanged;
                    FlatFilterText = null;
                    PropertyChanged += Self_OnPropertyChanged;
                }

                if (HasFloorFilter)
                {
                    FloorsView.Filter = IsFloorInFilter;
                }
                else
                {
                    FloorsView.Filter = null;
                }

                ClearFlatsHighlighting();
            }
            else if (e.PropertyName == nameof(FlatFilterText))
            {
                if (HasFloorFilter)
                {
                    PropertyChanged -= Self_OnPropertyChanged;
                    FloorFilterText = null;
                    PropertyChanged += Self_OnPropertyChanged;
                }

                if (HasFlatFilter)
                {
                    FloorsView.Filter = IsFlatInFilter;

                }
                else
                {
                    FloorsView.Filter = null;
                    ClearFlatsHighlighting();
                }

                SetFlatFilters();
            }
        }
    }
}