using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.ViewModels;

namespace TwinSovet.Messages 
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