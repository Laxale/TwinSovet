using System;

using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Helpers.Attachments 
{
    /// <summary>
    /// Класс для настройки провайдеров аттачей.
    /// </summary>
    internal abstract class AttachmentProviderConfigBase 
    {
        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public abstract Func<AttachmentModelBase, bool> Predicate { get; }
    }
}