using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Details 
{
    internal class MessageShowAborigenDetails : MessageShowDetails<AborigenDecoratorViewModel> 
    {
        public MessageShowAborigenDetails(AborigenDecoratorViewModel aborigenDecorator) : base(aborigenDecorator)
        {
            
        }
    }
}