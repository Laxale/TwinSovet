using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.ViewModels 
{
    internal class FilterViewModel : ViewModelBase 
    {
        private string filterText;
        

        public bool HasFilter => !string.IsNullOrWhiteSpace(FilterText);

        public string FilterText 
        {
            get => filterText;

            set
            {
                if (filterText == value) return;

                filterText = value;
                LoweredFilter = value?.ToLowerInvariant();

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFilter));
            }
        }

        public string LoweredFilter { get; private set; }
    }
}