using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.Interfaces 
{
    internal interface IDetailedAttachnemtView 
    {
        event Action EventCancelRequest;

        DateTime CreationTime { get; }


        void FocusInnerBox();
    }
}