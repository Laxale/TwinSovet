using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Data.Enums;
using TwinSovet.Helpers;


namespace TwinSovet.ViewModels 
{
    internal class FloorViewModel : SubjectEntityViewModel 
    {
        private readonly ObservableCollection<FlatDecoratorViewModel> flatDecorators = new ObservableCollection<FlatDecoratorViewModel>();

        private int floorNumber;
        private int minFlatNumber;
        private int maxFlatNumber;
        private SectionType section;


        public FloorViewModel() 
        {
            FlatsView = CollectionViewSource.GetDefaultView(flatDecorators);
            FlatsEnumerable = flatDecorators;

            PropertyChanged += Self_OnPropertyChanged;
        }


        public ICollectionView FlatsView { get; }

        public IEnumerable<FlatDecoratorViewModel> FlatsEnumerable { get; }

        public int FloorNumber 
        {
            get => floorNumber;

            set
            {
                if (floorNumber == value) return;

                floorNumber = value;

                OnPropertyChanged();
            }
        }
        
        public int MinFlatNumber 
        {
            get => minFlatNumber;

            private set
            {
                if (minFlatNumber == value) return;

                minFlatNumber = value;

                OnPropertyChanged();
            }
        }

        public int MaxFlatNumber 
        {
            get => maxFlatNumber;

            private set
            {
                if (maxFlatNumber == value) return;

                maxFlatNumber = value;

                OnPropertyChanged();
            }
        }

        public FilterViewModel FilterModel { get; } = new FilterViewModel();

        public SectionType Section 
        {
            get => section;

            set
            {
                if (section == value) return;

                section = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Floor;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo => $"{Section}; этаж {FloorNumber}";


        public void SetFlats(IEnumerable<FlatViewModel> flats) 
        {
            flatDecorators.Clear();
            flatDecorators.AddRange(flats.Select(flat => new FlatDecoratorViewModel(flat)));

            List<int> flatNumbers = flatDecorators.Select(decorator => decorator.Flat.Number).ToList();

            MinFlatNumber = flatNumbers.Min();
            MaxFlatNumber = flatNumbers.Max();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{FloorNumber} этаж секции {Section} | квартиры с {MinFlatNumber} по {MaxFlatNumber}";
        }


        private bool IsFlatInFilter(object flatObj) 
        {
            var flat = (FlatViewModel)flatObj;

            return flat.Number.ToString().ToLowerInvariant().Contains(FilterModel.FilterText.ToLowerInvariant());
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FilterModel.FilterText))
            {
                if (FilterModel.HasFilter)
                {
                    FlatsView.Filter = IsFlatInFilter;

                }
                else
                {
                    FlatsView.Filter = null;
                }
            }
        }
    }
}