using TwinSovet.Data.Enums;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс для реализации аттачментов.
    /// В силу наследования от <see cref="AttachmentAcceptorViewModel"/> любому аттачу можно приложить дерево аттачей.
    /// </summary>
    internal abstract class AttachmentViewModelBase : AttachmentAcceptorViewModel 
    {
        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public abstract AttachmentType EntityType { get; }
    }
}