using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Класс для нормального отображения привязок в XAML. Не использовать в коде!
    /// </summary>
    internal class VirtualPhotoAlbumDecorator : DataVirtualizeWrapper<PhotoAlbumPanelDecorator> 
    {
        /// <summary>
        /// Конструирует <see cref="DataVirtualizeWrapper{T}"/> с заданным индексом элемента в родительской коллекции.
        /// </summary>
        /// <param name="index">Индекс элемента в родительской коллекции.</param>
        public VirtualPhotoAlbumDecorator(int index) : base(index) 
        {

        }
    }
}