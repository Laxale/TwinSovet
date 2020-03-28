using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;

using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Helpers 
{
    internal class VirtualFloorWrapper : DataVirtualizeWrapper<FloorDecoratorViewModel> 
    {
        /// <summary>
        /// Конструирует <see cref="DataVirtualizeWrapper{T}"/> с заданным индексом элемента в родительской коллекции.
        /// </summary>
        /// <param name="index">Индекс элемента в родительской коллекции.</param>
        public VirtualFloorWrapper(int index) : base(index) 
        {

        }
    }
}