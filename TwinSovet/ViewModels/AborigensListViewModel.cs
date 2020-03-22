using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Helpers;
using TwinSovet.Messages;
using TwinSovet.Properties;

using PubSub;

using TwinSovet.Providers;


namespace TwinSovet.ViewModels 
{
    internal class AborigensListViewModel : ViewModelBase 
    {
        private string filterText;
        private string loweredFilter;

        private readonly ObservableCollection<AborigenInListDecoratorViewModel> aborigenDecorators = 
            new ObservableCollection<AborigenInListDecoratorViewModel>();


        public AborigensListViewModel() 
        {
            AborigensView = CollectionViewSource.GetDefaultView(aborigenDecorators);

            this.Publish(new MessageInitializeModelRequest(this, Resources.LoadingAborigensList));
        }


        public ICollectionView AborigensView { get; }

        public bool HasFilter => !string.IsNullOrWhiteSpace(FilterText);

        public string FilterText 
        {
            get => filterText;

            set
            {
                if (filterText == value) return;

                filterText = value;
                loweredFilter = value?.ToLowerInvariant();

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFilter));
            }
        }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            var random = new Random();
            var list = new List<AborigenInListDecoratorViewModel>();
            for (int index = 0; index < 100; index++)
            {
                var aborigenModel = new FlatAborigenModel
                {
                    Email = "test@mail.foo",
                    Name = $"Крутое имя { random.Next(0, 1000) }",
                    Surname = $"Фамилия { random.Next(0, 1000) }",
                    PhoneNumber = "666-666",
                    Otchestvo = $"Отчество { random.Next(0, 1000) }",
                    Gender = index % 2 == 0 ? GenderType.Man : GenderType.Woman
                };
                var aborigenViewModel = new FlatAborigenViewModel(aborigenModel);
                var decorator = new AborigenInListDecoratorViewModel(aborigenViewModel);

                list.Add(decorator);
            }

            Task.Run(() => LoadAborigenFlats(list));

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                aborigenDecorators.AddRange(list);
            });

            PropertyChanged += Self_OnPropertyChanged;
        }


        private void LoadAborigenFlats(List<AborigenInListDecoratorViewModel> list) 
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
                decorator.Aborigen.FullNameInfo.ToLowerInvariant().Contains(loweredFilter) ||
                decorator.Aborigen.Email.ToLowerInvariant().Contains(loweredFilter) ||
                decorator.Aborigen.PhoneNumber.ToLowerInvariant().Contains(loweredFilter) ||
                decorator.Aborigen.LocalizedGender.ToLowerInvariant().Contains(loweredFilter);
        }


        private void Self_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(FilterText))
            {
                AborigensView.Filter = HasFilter ? (Predicate<object>)IsInFilter : null;
            }
        }
    }
}