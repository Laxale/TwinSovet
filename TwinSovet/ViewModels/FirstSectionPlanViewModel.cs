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

        public FilterViewModel FlatFilterModel { get; } = new FilterViewModel();

        public FilterViewModel FloorFilterModel { get; } = new FilterViewModel();
        
        
        public void SetFloors(IEnumerable<FloorViewModel> floorModels) 
        {
            floors.Clear();
            floors.AddRange(floorModels);

            MinFlatNumber = floors.Min(floor => floor.FlatsView.OfType<FlatInListDecoratorViewModel>().Min(flatDecorator => flatDecorator.Flat.Number));
            MaxFlatNumber = floors.Max(floor => floor.FlatsView.OfType<FlatInListDecoratorViewModel>().Max(flatDecorator => flatDecorator.Flat.Number));
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

            FlatFilterModel.PropertyChanged += FlatFilterModel_OnPropertyChanged;
            FloorFilterModel.PropertyChanged += FloorFilterModel_OnPropertyChanged;
        }


        private void HighlightOrphanFlatsImpl() 
        {
            floors.ForEach(floor => floor.FlatsEnumerable.ForEach(flatDecorator =>
            {
                if (!flatDecorator.HasOwner)
                {
                    flatDecorator.IsOrphanHighlighted = isOrphanHighlighted;
                }
            }));

            isOrphanHighlighted = !isOrphanHighlighted;
        }

        private bool IsFloorInFilter(object floorObj) 
        {
            var floor = (FloorViewModel)floorObj;

            return floor.FloorNumber.ToString().ToLowerInvariant().Contains(FloorFilterModel.LoweredFilter);
        }

        private bool IsFlatInFilter(object floorObj) 
        {
            var floor = (FloorViewModel)floorObj;
            string loweredFlatFiler = FlatFilterModel.LoweredFilter;

            bool hasAnyFlats = false;
            foreach (var flatDecorator in floor.FlatsEnumerable)
            {
                if (flatDecorator.Flat.Number.ToString().ToLowerInvariant().Contains(loweredFlatFiler))
                {
                    hasAnyFlats = true;
                    flatDecorator.IsHighlighted = true;
                }
                else
                {
                    flatDecorator.IsHighlighted = false;
                }
            }

            return hasAnyFlats;
        }

        private void SetFlatFilters() 
        {
            foreach (FloorViewModel floor in floors)
            {
                floor.FilterModel.FilterText = FloorFilterModel.FilterText;
            }
        }

        private void ClearFlatsHighlighting() 
        {
            FloorsEnumerable.ForEach(floor => floor.FlatsEnumerable.ForEach(flat => flat.IsHighlighted = false));
        }


        private void FlatFilterModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (FloorFilterModel.HasFilter)
            {
                FloorFilterModel.PropertyChanged -= FloorFilterModel_OnPropertyChanged;
                FloorFilterModel.FilterText = null;
                FloorFilterModel.PropertyChanged += FloorFilterModel_OnPropertyChanged;
            }

            if (FlatFilterModel.HasFilter)
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
        
        private void FloorFilterModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (FlatFilterModel.HasFilter)
            {
                FlatFilterModel.PropertyChanged -= FlatFilterModel_OnPropertyChanged;
                FlatFilterModel.FilterText = null;
                FlatFilterModel.PropertyChanged += FlatFilterModel_OnPropertyChanged;
            }

            if (FloorFilterModel.HasFilter)
            {
                FloorsView.Filter = IsFloorInFilter;
            }
            else
            {
                FloorsView.Filter = null;
            }

            ClearFlatsHighlighting();
        }
    }
}