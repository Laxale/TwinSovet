using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PubSub;

using TwinSovet.Data.Enums;
using TwinSovet.Messages;


namespace TwinSovet.ViewModels 
{
    internal class HospitalSectionPlanViewModel : SectionViewModelBase 
    {
        public HospitalSectionPlanViewModel()
        {
            this.Publish(new MessageInitializeModelRequest(this, "Загружаем план больничной секции"));
        }


        public override SectionType TypeOfSection { get; } = SectionType.Hospital;
    }
}