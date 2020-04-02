using TwinSovet.Data.Enums;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.ViewModels.Subjects 
{
    /// <summary>
    /// Класс для обозначения вьюмоделей тех или иных сущностей дома. Будь то жители, квартиры, этажи, секции..
    /// </summary>
    internal abstract class SubjectEntityViewModelBase : AttachmentAcceptorViewModel 
    {
        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public abstract SubjectType TypeOfSubject { get; }

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public abstract string SubjectFriendlyInfo { get; }
    }
}