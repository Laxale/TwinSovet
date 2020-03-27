using System;
using System.Collections.Generic;
using System.Linq;

using DataVirtualization;

using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Interfaces 
{
    /// <summary>
    /// Интерфейс для реализации провайдера аттачей.
    /// </summary>
    internal interface IAttachmentsProvider : IItemsProvider<AttachmentPanelDecoratorBase_NonGeneric> 
    {
        /// <summary>
        /// Обновить кэш аттачей.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Задать фильтр выборки отображаемых декораторов.
        /// </summary>
        /// <param name="predicate">Функция выборки отображаемых декораторо. Может быть null.</param>
        void SetFilter(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate);
    }
}