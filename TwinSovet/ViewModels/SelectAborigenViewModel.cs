using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Messages;
using TwinSovet.Properties;
using TwinSovet.Providers;
using TwinSovet.Helpers;

using PubSub;
using TwinSovet.Data.Providers;
using LocRes = TwinSovet.Localization.Properties.Resources;


namespace TwinSovet.ViewModels 
{
    internal class SelectAborigenViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<AborigenInListDecoratorViewModel> aborigenDecorators = new ObservableCollection<AborigenInListDecoratorViewModel>();

        
        public SelectAborigenViewModel() 
        {
            AborigensView = CollectionViewSource.GetDefaultView(aborigenDecorators);

            this.Publish(new MessageInitializeModelRequest(this, LocRes.LoadingAborigensList));
        }


        public ICollectionView AborigensView { get; }

        public FilterViewModel FilterModel { get; } = new FilterViewModel();


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            List<AborigenInListDecoratorViewModel> decorators =
                AborigensProvider.GetAborigens()
                    .Select(aborigenModel => new AborigenInListDecoratorViewModel(new AborigenViewModel(aborigenModel)))
                    .ToList();

            DispatcherHelper.InvokeOnDispatcher(() => aborigenDecorators.AddRange(decorators));

            Task.Run(() =>
            {
                //
                AborigensListViewModel.LoadAborigenFlats(decorators);
            });

            FilterModel.PropertyChanged += FilterModel_OnPropertyChanged;
        }


        private void FilterModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FilterModel.FilterText))
            {
                AborigensView.Filter = FilterModel.HasFilter ? (Predicate<object>) IsInFilter : null;
            }
        }


        private bool IsInFilter(object decoratorObj) 
        {
            var decorator = (AborigenInListDecoratorViewModel) decoratorObj;

            bool isNameFiltered = decorator.Aborigen.FullNameInfo.ToLowerInvariant().Contains(FilterModel.LoweredFilter);

            if (isNameFiltered)
            {
                return true;
            }

            if (decorator.Flat != null)
            {
                return decorator.Flat.Number.ToString().Contains(FilterModel.LoweredFilter);
            }

            return false;
        }
    }
}