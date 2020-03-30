using System;
using System.Collections.Generic;
using System.Linq;


namespace TwinSovet.Interfaces 
{
    internal interface IDetailedAttachnemtView 
    {
        event Action EventCancelRequest;

        DateTime CreationTime { get; }


        void FocusInnerBox();
    }
}