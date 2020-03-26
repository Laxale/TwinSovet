using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Details 
{
    internal class MessageShowFlatDetails : MessageShowDetails<FlatDecoratorViewModel> 
    {
        public MessageShowFlatDetails(FlatDecoratorViewModel flatDecorator) : base(flatDecorator) 
        {

        }
    }
}