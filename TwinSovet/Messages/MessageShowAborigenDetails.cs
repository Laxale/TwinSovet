﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.ViewModels;

namespace TwinSovet.Messages 
{
    internal class MessageShowAborigenDetails : MessageShowDetails<AborigenDecoratorViewModel> 
    {
        public MessageShowAborigenDetails(AborigenDecoratorViewModel aborigenDecorator) : base(aborigenDecorator)
        {
            
        }
    }
}