using System;
using System.ComponentModel;
using System.Linq;

using Common.Helpers;

using DataVirtualization;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Commands;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Providers;
using TwinSovet.Interfaces;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Subjects
{
    internal abstract class SectionViewModelBase : SubjectEntityViewModelBase 
    {
        private const int pageSize = 4;
        private const int pageTimeout = int.MaxValue;

        private readonly IFloorsProvider floorsProvider;

        private int minFlatNumber;
        private int maxFlatNumber;
        private int loadProgress;
        private bool isCollapsingAll;
        private bool isOrphanHighlighted = true;


        protected SectionViewModelBase(AllFloorsProvider allFloorsProvider) 
        {
            floorsProvider = 
                SectionTypeWrapper == SectionType.Furniture ? 
                    allFloorsProvider.FurnitureFloorsProvider :
                    allFloorsProvider.HospitalFloorsProvider;

            CommandSetAllCollapsed = new DelegateCommand<bool?>(SetAllCollapsedImpl);
            CommandHighlightOrphanFlats = new DelegateCommand(HighlightOrphanFlatsImpl, () => IsReady);
        }

        
        /// <summary>
        /// Возвращает команду свернуть или развернуть все этажи.
        /// </summary>
        public DelegateCommand<bool?> CommandSetAllCollapsed { get; }

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

        public bool AreAllCollapsed => !floorsProvider.Any(floor => !floor.IsMinimized);
        

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
            floorsProvider.ForEach(floor => floor.PropertyChanged += FloorDecorator_OnPropertyChanged);
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

        private void SetAllCollapsedImpl(bool? areCollapsed) 
        {
            if (areCollapsed == null) return;

            isCollapsingAll = true;
            floorsProvider.ForEach(floor => floor.IsMinimized = areCollapsed.Value);
            isCollapsingAll = false;

            RaiseAreAllCollapsed();
        }

        private void RaiseAreAllCollapsed() 
        {
            OnPropertyChanged(nameof(AreAllCollapsed));
        }

        private bool IsFloorInFilter(FloorDecoratorViewModel floorDecorator) 
        {
            return floorDecorator.OriginaFloorViewModel.FloorNumber.ToString().ToLowerInvariant().Contains(FloorFilterModel.LoweredFilter);
        }

        private bool IsFlatInFilter(FlatDecoratorViewModel flatDecorator) 
        {
            if (flatDecorator.Flat.Number.ToString().ToLowerInvariant().Contains(FlatFilterModel.LoweredFilter))
            {
                return true;
            }

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
                floorsProvider.ForEach(floorDecorator => floorDecorator.OriginaFloorViewModel.FlatsEnumerable.ForEach(
                    flat =>
                    {
                        // выделяем все квартиры, номера которых попадают в фильтр
                        flat.IsHighlighted = IsFlatInFilter(flat);
                    }));
                floorsProvider.SetFilter(floorDecorator => floorDecorator.OriginaFloorViewModel.FlatsEnumerable.Any(IsFlatInFilter));
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

        private void FloorDecorator_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FloorDecoratorViewModel.IsMinimized))
            {
                if (isCollapsingAll) return;
                RaiseAreAllCollapsed();
            }
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