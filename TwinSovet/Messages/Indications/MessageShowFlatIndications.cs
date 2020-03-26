using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Indications 
{
    internal class MessageShowFlatIndications : MessageShowIndications 
    {
        public MessageShowFlatIndications(FlatDecoratorViewModel flatDecorator) 
        {
            FlatDecorator = flatDecorator;
        }


        public FlatDecoratorViewModel FlatDecorator { get; }
    }
}