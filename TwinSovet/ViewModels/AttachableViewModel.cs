using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Enums;


namespace TwinSovet.ViewModels 
{
    internal abstract class AttachableViewModel : ViewModelBase 
    {
        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public abstract AttachableEntityType EntityType { get; }
    }
}