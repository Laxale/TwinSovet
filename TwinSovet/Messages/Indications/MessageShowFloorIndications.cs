using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Indications 
{
    internal class MessageShowFloorIndications : MessageShowIndications 
    {
        public MessageShowFloorIndications(FloorDecoratorViewModel floorDecorator)
        {
            FloorDecorator = floorDecorator;
        }


        public FloorDecoratorViewModel FloorDecorator { get; }
    }
}