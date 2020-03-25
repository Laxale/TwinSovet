using System;

using TwinSovet.Data.Enums;
using TwinSovet.Messages;
using TwinSovet.Providers;

using TwinSovet.Enums;

using PubSub;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class FurnitureSectionPlanViewModel : SectionViewModelBase 
    {
        public FurnitureSectionPlanViewModel(AllFloorsProvider floorsProvider) : base(floorsProvider) 
        {
            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план мебельной секции"));
        }


        public override SectionType TypeOfSection { get; } = SectionType.Furniture;

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Section;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo { get; } = LocRes.Mebelnaya;
    }
}