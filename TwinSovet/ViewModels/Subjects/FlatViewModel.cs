﻿using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels.Subjects 
{
    internal class FlatViewModel : SubjectEntityViewModelBase 
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

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Flat;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo => FullFlatLocationInfo;

        public string FullFlatLocationInfo => $"{SectionName}; этаж {FloorNumber}; квартира {Number}";


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{ Number } квартира { FloorNumber } этажа секции {Section}";
        }
    }
}