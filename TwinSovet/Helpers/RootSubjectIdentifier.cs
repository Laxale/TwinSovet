using System;

using TwinSovet.Data.Enums;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Helpers 
{
    internal static class RootSubjectIdentifier 
    {
        public static string FlatPrefix { get; } = "Flat";

        public static string FloorPrefix { get; } = "Floor";

        public static string SectionPrefix { get; } = "Section";

        public static string HousePrefix { get; } = "House";


        public static string Identify(SubjectEntityViewModel subjectModel) 
        {
            if (subjectModel is AborigenDecoratorViewModel aborigen)
            {
                return aborigen.AborigenReadOnly.GetId();
            }

            if (subjectModel is FlatViewModel flat)
            {
                return $"{ FlatPrefix }_{ flat.Number }";
            }

            if (subjectModel is FloorViewModel floor)
            {
                return $"{ FloorPrefix }_{ floor.FloorNumber }";
            }

            if (subjectModel is FurnitureSectionPlanViewModel)
            {
                return $"{ SectionPrefix }_{ SectionType.Furniture }";
            }

            if (subjectModel is HospitalSectionPlanViewModel)
            {
                return $"{ SectionPrefix }_{ SectionType.Hospital }";
            }

            if (subjectModel is HouseViewModel)
            {
                return HousePrefix;
            }

            throw new NotImplementedException();
        }
    }
}