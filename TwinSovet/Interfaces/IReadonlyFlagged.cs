using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.Interfaces 
{
    internal interface IReadonlyFlagged 
    {
        bool IsReadonly { get; }
    }
}