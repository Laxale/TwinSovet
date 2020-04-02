using System;

using TwinSovet.Data.Enums;


namespace TwinSovet.ViewModels.Subjects 
{
    internal class HouseViewModel : SubjectEntityViewModelBase 
    {
        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.House;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo { get; } = "Twin House";
    }
}