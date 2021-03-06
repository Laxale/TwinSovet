﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class VirtualPhotoDecorator : DataVirtualizeWrapper<PhotoPanelDecorator> 
    {
        /// <summary>
        /// Конструирует <see cref="DataVirtualizeWrapper{T}"/> с заданным индексом элемента в родительской коллекции.
        /// </summary>
        /// <param name="index">Индекс элемента в родительской коллекции.</param>
        public VirtualPhotoDecorator(int index) : base(index) 
        {

        }
    }
}