using System;
using System.Collections.Generic;
using TwinSovet.Data.Enums;


namespace TwinSovet.ViewModels 
{
    /// <summary>
    /// Класс для обозначения вьюмоделей тех или иных сущностей дома. Будь то жители, квартиры, этажи, секции..
    /// </summary>
    internal abstract class SubjectEntityViewModel : AttachmentAcceptorViewModel 
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