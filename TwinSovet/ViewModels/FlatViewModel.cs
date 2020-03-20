using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;


namespace TwinSovet.ViewModels 
{
    internal class FlatViewModel : ViewModelBase 
    {
        private readonly FlatModel flatModel;

        private float area;
        private int number;
        private int floorNumber;
        private SectionType section;
        private FlatAborigenViewModel flatOwner;


        public FlatViewModel(FlatModel flatModel) 
        {
            this.flatModel = flatModel;

            Area = flatModel.Area;
            FloorNumber = flatModel.FloorNumber;
            Section = flatModel.Section;
            Number = flatModel.Number;
        }


        public float Area 
        {
            get => area;

            set
            {
                if (Math.Abs(area - value) < 0.01) return;

                area = value;

                OnPropertyChanged();
            }
        }

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

        public int Number 
        {
            get => number;

            set
            {
                if (number == value) return;

                number = value;

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

        public bool HasOwner => FlatOwner != null;

        public FlatAborigenViewModel FlatOwner 
        {
            get => flatOwner;

            set
            {
                if (flatOwner == value) return;

                flatOwner = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOwner));
            }
        }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{ Number } квартира { FloorNumber } этажа секции {Section}";
        }
    }
}