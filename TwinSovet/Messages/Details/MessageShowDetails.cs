using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Details 
{
    internal abstract class MessageShowDetails<TDetails> where TDetails : ViewModelBase 
    {
        protected MessageShowDetails(TDetails viewModel) 
        {
            ViewModel = viewModel;
        }


        public TDetails ViewModel { get; }
    }
}