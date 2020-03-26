using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Details 
{
    internal class MessageShowAborigenDetails : MessageShowDetails<AborigenDecoratorViewModel> 
    {
        public MessageShowAborigenDetails(AborigenDecoratorViewModel aborigenDecorator) : base(aborigenDecorator)
        {
            
        }
    }
}