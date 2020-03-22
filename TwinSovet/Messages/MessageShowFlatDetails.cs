using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Messages
{
    internal class MessageShowFlatDetails 
    {
        public MessageShowFlatDetails(FlatViewModel flat) 
        {
            Flat = flat;
        }


        public FlatViewModel Flat { get; }
    }
}