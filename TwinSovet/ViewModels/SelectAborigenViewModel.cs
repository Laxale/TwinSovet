using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Common.Helpers;
using TwinSovet.Messages;
using TwinSovet.Properties;
using TwinSovet.Providers;
using TwinSovet.Helpers;

using PubSub;
using TwinSovet.Data.Providers;
using TwinSovet.Extensions;
using TwinSovet.ViewModels.Subjects;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class SelectAborigenViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<AborigenDecoratorViewModel> aborigenDecorators = new ObservableCollection<AborigenDecoratorViewModel>();

        
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

            IEnumerable<AborigenDecoratorViewModel> decorators =
                AborigensProvider
                    .GetAborigens()
                    .Select(AborigenDecoratorViewModel.Create);

            DispatcherHelper.InvokeOnDispatcher(() => aborigenDecorators.AddRange(decorators));

            Task.Run(() => AborigensListViewModel.LoadAborigenFlats(aborigenDecorators));

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
            var decorator = (AborigenDecoratorViewModel) decoratorObj;

            bool isNameFiltered = decorator.AborigenEditable.FullNameInfo.ToLowerInvariant().Contains(FilterModel.LoweredFilter);

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