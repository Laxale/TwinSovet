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


        public AborigenInListDecoratorViewModel(AborigenViewModel aborigen) 
        {
            Aborigen = aborigen;
        }


        public AborigenViewModel Aborigen { get; }

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