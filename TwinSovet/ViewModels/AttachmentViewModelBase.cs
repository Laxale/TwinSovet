using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels 
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