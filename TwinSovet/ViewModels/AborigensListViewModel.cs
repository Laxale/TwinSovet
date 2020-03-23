using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Commands;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Helpers;
using TwinSovet.Messages;
using TwinSovet.Properties;

using PubSub;
using TwinSovet.Data.Extensions;
using TwinSovet.Data.Providers;
using TwinSovet.Providers;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class AborigensListViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<AborigenInListDecoratorViewModel> aborigenDecorators = new ObservableCollection<AborigenInListDecoratorViewModel>();


        public AborigensListViewModel() 
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

            Task.Run(() => LoadAborigenFlats(decorators));

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                aborigenDecorators.AddRange(decorators);
            });

            FilterModel.PropertyChanged += Filter_OnPropertyChanged;

            AborigensProvider.EventAborigenAdded += AborigensProvider_OnAborigenAdded;
        }

        private void AborigensProvider_OnAborigenAdded(AborigenModel aborigen) 
        {
            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                aborigenDecorators.Add(new AborigenInListDecoratorViewModel(new AborigenViewModel(aborigen.Clone())));
            });
        }


        internal static void LoadAborigenFlats(List<AborigenInListDecoratorViewModel> list) 
        {
            //
            foreach (AborigenInListDecoratorViewModel aborigenDecorator in list)
            {
                aborigenDecorator.Flat = FlatsProvider.FindFlatOfAborigen(aborigenDecorator.Aborigen.GetId());
            }
        }
        

        private bool IsInFilter(object aborigenObj) 
        {
            var decorator = (AborigenInListDecoratorViewModel)aborigenObj;

            return 
                decorator.Aborigen.FullNameInfo.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ||
                decorator.Aborigen.Email.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ||
                decorator.Aborigen.PhoneNumber.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ||
                decorator.Aborigen.LocalizedGender.ToLowerInvariant().Contains(FilterModel.LoweredFilter);
        }


        private void Filter_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FilterModel.FilterText))
            {
                AborigensView.Filter = FilterModel.HasFilter ? (Predicate<object>)IsInFilter : null;
            }
        }
    }
}