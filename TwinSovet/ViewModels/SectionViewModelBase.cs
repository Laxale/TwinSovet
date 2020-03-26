using System;
using System.ComponentModel;
using System.Linq;

using Common.Helpers;
using DataVirtualization;

using TwinSovet.Data.Enums;
using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.Providers;

using Prism.Commands;

using Microsoft.Practices.ObjectBuilder2;


namespace TwinSovet.ViewModels
{
    internal abstract class SectionViewModelBase : SubjectEntityViewModel 
    {
        private const int pageSize = 4;
        private const int pageTimeout = int.MaxValue;

        private readonly IFloorsProvider floorsProvider;

        private int minFlatNumber;
        private int maxFlatNumber;
        private int loadProgress;
        private bool isOrphanHighlighted = true;


        protected SectionViewModelBase(AllFloorsProvider allFloorsProvider) 
        {
            floorsProvider = 
                SectionTypeWrapper == SectionType.Furniture ? 
                    allFloorsProvider.FurnitureFloorsProvider :
                    allFloorsProvider.HospitalFloorsProvider;

            CommandHighlightOrphanFlats = new DelegateCommand(HighlightOrphanFlatsImpl, () => IsReady);
        }


        public DelegateCommand CommandHighlightOrphanFlats { get; }


        /// <summary>
        /// Возвращает текущий прогресс загрузки.
        /// </summary>
        public int LoadProgress 
        {
            get => loadProgress;

            private set
            {
                if (loadProgress == value) return;

                loadProgress = value;

                OnPropertyChanged();
            }
        }

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

        public abstract SectionType TypeOfSection { get; }


        private SectionType SectionTypeWrapper => TypeOfSection;


        /// <summary>
        /// Возвращает ссылку на виртуальную коллекцию этажей.
        /// </summary>
        public AsyncVirtualizingCollection<FloorDecoratorViewModel> FloorWrappersCollection { get; private set; }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            SetFlatNumbersRange();

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                FloorWrappersCollection = new AsyncVirtualizingCollection<FloorDecoratorViewModel>(floorsProvider, pageSize, pageTimeout);
            });

            OnPropertyChanged(nameof(FloorWrappersCollection));

            RefreshCollection();

            PropertyChanged += Self_OnPropertyChanged;
            FlatFilterModel.PropertyChanged += FlatFilterModel_OnPropertyChanged;
            FloorFilterModel.PropertyChanged += FloorFilterModel_OnPropertyChanged;
        }


        void PrintTime(int entry)
        {
            Console.WriteLine($"{ TypeOfSection } [{ entry }] : { DateTime.Now:O}");
        }
       

        private void HighlightOrphanFlatsImpl() 
        {
            floorsProvider.ForEach(floorDecorator => floorDecorator.OriginaFloorViewModel.FlatsEnumerable.ForEach(flatDecorator =>
            {
                if (!flatDecorator.HasOwner)
                {
                    flatDecorator.IsOrphanHighlighted = isOrphanHighlighted;
                }
            }));

            isOrphanHighlighted = !isOrphanHighlighted;

            RefreshCollection();
        }

        private bool IsFloorInFilter(FloorDecoratorViewModel floorDecorator) 
        {
            return floorDecorator.OriginaFloorViewModel.FloorNumber.ToString().ToLowerInvariant().Contains(FloorFilterModel.LoweredFilter);
        }

        private bool IsFlatInFilter(FlatDecoratorViewModel flatDecorator) 
        {
            string loweredFlatFiler = FlatFilterModel.LoweredFilter;

            if (flatDecorator.Flat.Number.ToString().ToLowerInvariant().Contains(loweredFlatFiler))
            {
                flatDecorator.IsHighlighted = true;
                return true;
            }

            flatDecorator.IsHighlighted = false;
            return false;
        }

        private void SetFlatFilters() 
        {
            floorsProvider.ForEach(decorator =>
            {
                decorator.OriginaFloorViewModel.FilterModel.FilterText = FloorFilterModel.FilterText;
            });

            //FloorWrappersCollection.Refresh();
        }

        private void ClearFlatsHighlighting() 
        {
            floorsProvider.ForEach(floorDecorator => floorDecorator.OriginaFloorViewModel.FlatsEnumerable.ForEach(flat => flat.IsHighlighted = false));
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(IsReady))
            {
                CommandHighlightOrphanFlats.RaiseCanExecuteChanged();
            }
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
                floorsProvider.SetFilter(decorator => decorator.OriginaFloorViewModel.FlatsEnumerable.Any(IsFlatInFilter));
            }
            else
            {
                floorsProvider.SetFilter(null);
                ClearFlatsHighlighting();
            }

            SetFlatFilters();

            RefreshCollection();
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
                floorsProvider.SetFilter(IsFloorInFilter);
            }
            else
            {
                floorsProvider.SetFilter(null);
            }

            ClearFlatsHighlighting();
            RefreshCollection();
        }


        private void RefreshCollection() 
        {
            FloorWrappersCollection.Refresh();
        }

        private void SetFlatNumbersRange() 
        {
            if (TypeOfSection == SectionType.Furniture)
            {
                MinFlatNumber = StaticsProvider.MinFlatNumber;
                MaxFlatNumber = StaticsProvider.FlatsInFurnitureSection;
            }
            else
            {
                MinFlatNumber = StaticsProvider.FlatsInFurnitureSection + 1;
                MaxFlatNumber = StaticsProvider.MaxFlatNumber;
            }
        }
    }
}