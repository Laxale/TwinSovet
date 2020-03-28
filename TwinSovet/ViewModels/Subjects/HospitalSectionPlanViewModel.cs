using PubSub;

using TwinSovet.Data.Enums;
using TwinSovet.Messages;
using TwinSovet.Providers;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels.Subjects 
{
    internal class HospitalSectionPlanViewModel : SectionViewModelBase 
    {
        public HospitalSectionPlanViewModel(AllFloorsProvider floorsProvider) : base(floorsProvider) 
        {
            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план больничной секции"));
        }


        public override SectionType TypeOfSection { get; } = SectionType.Hospital;

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Section;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo { get; } = LocRes.Hospital;
    }
}