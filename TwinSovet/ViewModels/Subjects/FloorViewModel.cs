using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;


namespace TwinSovet.ViewModels.Subjects 
{
    internal class FloorViewModel : SubjectEntityViewModel 
    {
        private readonly ObservableCollection<FlatDecoratorViewModel> flatDecorators = new ObservableCollection<FlatDecoratorViewModel>();

        
        public FloorViewModel(FloorModel floorModel) 
        {
            FlatsView = CollectionViewSource.GetDefaultView(flatDecorators);
            FlatsEnumerable = flatDecorators;

            Section = floorModel.Section;
            FloorNumber = floorModel.FloorNumber;
            MinFlatNumber = floorModel.MinFlatNumber;
            MaxFlatNumber = floorModel.MaxFlatNumber;

            PropertyChanged += Self_OnPropertyChanged;
        }


        public ICollectionView FlatsView { get; }

        public IEnumerable<FlatDecoratorViewModel> FlatsEnumerable { get; }

        public int FloorNumber { get; }
        
        public int MinFlatNumber { get; }

        public int MaxFlatNumber { get; }

        public SectionType Section { get; }

        public FilterViewModel FilterModel { get; } = new FilterViewModel();

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Floor;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo => $"{Section}; этаж {FloorNumber}";


        public void PopulateFlats(IEnumerable<FlatViewModel> flats) 
        {
            Console.WriteLine($">>> populating flats for '{ this }'");
            flatDecorators.Clear();
            flatDecorators.AddRange(flats.Select(flat => new FlatDecoratorViewModel(flat)));
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

            return flat.Number.ToString().ToLowerInvariant().Contains(FilterModel.LoweredFilter);
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