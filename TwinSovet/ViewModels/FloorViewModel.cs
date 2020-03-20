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
        private int minFlatNumber;
        private int maxFlatNumber;
        private SectionType section;


        public FloorViewModel() 
        {
            FlatsView = CollectionViewSource.GetDefaultView(flats);
        }


        public ICollectionView FlatsView { get; }

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
    }
}