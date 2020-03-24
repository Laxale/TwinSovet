using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Microsoft.Practices.ObjectBuilder2;

using Prism.Commands;

using PubSub;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;
using TwinSovet.Helpers;
using TwinSovet.Messages;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels 
{
    internal class FurnitureSectionPlanViewModel : SectionViewModelBase 
    {
        public FurnitureSectionPlanViewModel() 
        {
            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план мебельной секции"));
        }


        public override SectionType TypeOfSection { get; } = SectionType.Furniture;
    }
}