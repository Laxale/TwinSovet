using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Extensions
{
    internal static class ViewModelExtensions 
    {
        public static AborigenViewModel Clone(this AborigenViewModel aborigen, bool isReadOnly) 
        {
            return new AborigenViewModel(aborigen.GetModel(), isReadOnly);
        }
    }
}