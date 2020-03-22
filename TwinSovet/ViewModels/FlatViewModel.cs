using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Properties;


namespace TwinSovet.ViewModels 
{
    internal class FlatViewModel : ViewModelBase 
    {
        private readonly FlatModel flatModel;

        private float area;
        private int number;
        private int floorNumber;
        private bool isHighlighted;
        private SectionType section;
        private bool isOrphanHighlighted;
        private FlatAborigenViewModel flatOwner;


        public FlatViewModel(FlatModel flatModel) 
        {
            this.flatModel = flatModel;

            Area = flatModel.Area;
            FloorNumber = flatModel.FloorNumber;
            Section = flatModel.Section;
            Number = flatModel.Number;

            if (Section == SectionType.Furniture)
            {
                SectionName = Resources.Mebelnaya;
            }
            else if (Section == SectionType.Hospital)
            {
                SectionName = Resources.Hospital;
            }

            OnPropertyChanged(nameof(FullFlatLocationInfo));
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

        public string SectionName { get; }

        public bool HasOwner => FlatOwner != null;

        public bool IsOrphanHighlighted 
        {
            get => isOrphanHighlighted;

            set
            {
                if (isOrphanHighlighted == value) return;

                isOrphanHighlighted = value;

                OnPropertyChanged();
            }
        }

        public bool IsHighlighted 
        {
            get => isHighlighted;

            set
            {
                if (isHighlighted == value) return;

                isHighlighted = value;

                OnPropertyChanged();
            }
        }

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

        public string FullFlatLocationInfo => $"{SectionName}; этаж {FloorNumber}; номер {Number}";


        public string GetId() 
        {
            return flatModel.Id;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{ Number } квартира { FloorNumber } этажа секции {Section}";
        }
    }
}