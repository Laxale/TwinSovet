using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.ViewModels
{
    internal class AborigenInListDecoratorViewModel : ViewModelBase 
    {
        private FlatViewModel flat;


        public AborigenInListDecoratorViewModel(FlatAborigenViewModel aborigen) 
        {
            Aborigen = aborigen;
        }


        public FlatAborigenViewModel Aborigen { get; }

        public FlatViewModel Flat 
        {
            get => flat;

            set
            {
                if (flat == value) return;

                flat = value;

                OnPropertyChanged();
            }
        }
    }
}