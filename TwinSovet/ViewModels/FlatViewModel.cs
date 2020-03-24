using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Extensions;
using TwinSovet.Views;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class FlatViewModel : ViewModelBase 
    {
        public FlatViewModel(FlatModel flatModel) 
        {
            Area = flatModel.Area;
            FloorNumber = flatModel.FloorNumber;
            Section = flatModel.Section;
            Number = flatModel.Number;

            if (Section == SectionType.Furniture)
            {
                SectionName = LocRes.Mebelnaya;
            }
            else if (Section == SectionType.Hospital)
            {
                SectionName = LocRes.Hospital;
            }

            OnPropertyChanged(nameof(FullFlatLocationInfo));
        }


        public float Area { get; }

        public int FloorNumber { get; }

        public int Number { get ; }

        public SectionType Section { get; }

        public string SectionName { get; }

        public string FullFlatLocationInfo => $"{SectionName}; этаж {FloorNumber}; номер {Number}";


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{ Number } квартира { FloorNumber } этажа секции {Section}";
        }
    }
}