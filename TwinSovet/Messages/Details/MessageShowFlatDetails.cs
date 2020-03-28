using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Details 
{
    internal class MessageShowFlatDetails : MessageShowDetails<FlatDecoratorViewModel> 
    {
        public MessageShowFlatDetails(FlatDecoratorViewModel flatDecorator) : base(flatDecorator) 
        {

        }
    }
}