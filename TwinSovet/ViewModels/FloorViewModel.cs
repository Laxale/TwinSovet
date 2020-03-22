using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Data.Enums;


namespace TwinSovet.ViewModels 
{
    internal class FloorViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<FlatViewModel> flats = new ObservableCollection<FlatViewModel>();

        private int floorNumber;
        private string filterText;
        private int minFlatNumber;
        private int maxFlatNumber;
        private SectionType section;


        public FloorViewModel() 
        {
            FlatsView = CollectionViewSource.GetDefaultView(flats);
            FlatsEnumerable = flats;

            PropertyChanged += Self_OnPropertyChanged;
        }


        public ICollectionView FlatsView { get; }

        public IEnumerable<FlatViewModel> FlatsEnumerable { get; }

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

        public bool HasFilter => !string.IsNullOrWhiteSpace(FilterText);

        public string FilterText 
        {
            get => filterText;

            set
            {
                if (filterText == value) return;

                filterText = value;

                OnPropertyChanged();
            }
        }

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


        public void SetFlats(IEnumerable<FlatViewModel> flats) 
        {
            this.flats.Clear();
            this.flats.AddRange(flats);

            List<int> flatNumbers = this.flats.Select(flat => flat.Number).ToList();

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

            return flat.Number.ToString().ToLowerInvariant().Contains(FilterText.ToLowerInvariant());
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FilterText))
            {
                if (HasFilter)
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