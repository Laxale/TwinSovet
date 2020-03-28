using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Common.Helpers;
using Prism.Commands;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Helpers;
using TwinSovet.Messages;
using TwinSovet.Properties;

using PubSub;
using TwinSovet.Data.Extensions;
using TwinSovet.Data.Providers;
using TwinSovet.Extensions;
using TwinSovet.Providers;
using TwinSovet.ViewModels.Subjects;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class AborigensListViewModel : ViewModelBase 
    {
        private readonly ObservableCollection<AborigenDecoratorViewModel> aborigenDecorators = new ObservableCollection<AborigenDecoratorViewModel>();


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

            IEnumerable<AborigenDecoratorViewModel> decorators = 
                AborigensProvider
                    .GetAborigens()
                    .Select(AborigenDecoratorViewModel.Create);

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                aborigenDecorators.AddRange(decorators);
            });

            Task.Run(() => LoadAborigenFlats(aborigenDecorators));

            FilterModel.PropertyChanged += Filter_OnPropertyChanged;

            AborigensProvider.EventAborigenAdded += AborigensProvider_OnAborigenAdded;
        }

        private void AborigensProvider_OnAborigenAdded(AborigenModel aborigen) 
        {
            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                aborigenDecorators.Add(AborigenDecoratorViewModel.Create(aborigen.Clone()));
            });
        }


        internal static void LoadAborigenFlats(IEnumerable<AborigenDecoratorViewModel> list) 
        {
            //
            foreach (AborigenDecoratorViewModel aborigenDecorator in list)
            {
                aborigenDecorator.Flat = FlatsProvider.FindFlatOfAborigen(aborigenDecorator.AborigenEditable.GetId());
            }
        }
        

        private bool IsInFilter(object aborigenObj) 
        {
            var decorator = (AborigenDecoratorViewModel)aborigenObj;

            return 
                decorator.AborigenEditable.FullNameInfo?.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ?? 
                decorator.AborigenEditable.Email?.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ??
                decorator.AborigenEditable.PhoneNumber?.ToLowerInvariant().Contains(FilterModel.LoweredFilter) ??
                decorator.AborigenEditable.LocalizedGender.ToLowerInvariant().Contains(FilterModel.LoweredFilter);
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