using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Helpers;
using Common.Interfaces;


namespace TwinSovet.ViewModels 
{
    internal class FloorDecoratorViewModel : ViewModelBase, ISearchAcceptor 
    {
        private readonly AcceptSearchHelper searchHelper = new AcceptSearchHelper();

        private bool isMinimized;
        private string searchFilter;
        private bool isFloorNumberSearched;


        public FloorDecoratorViewModel(FloorViewModel originaFloorViewModel) 
        {
            OriginaFloorViewModel = originaFloorViewModel;
            PrepareSearch();
        }


        public FloorViewModel OriginaFloorViewModel { get; }

        /// <summary>
        /// Возвращает внешний фильтр поиска.
        /// </summary>
        public string SearchFilter 
        {
            get => searchFilter;

            protected set
            {
                if (searchFilter == value)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(searchFilter) && string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                searchFilter = value;
                HasFilter = !string.IsNullOrWhiteSpace(SearchFilter);

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFilter));
            }
        }

        /// <summary>
        /// Возвращает флаг - заполнен ли сейчас НЕпустой фильтр.
        /// </summary>
        public bool HasFilter { get; private set; }

        public bool IsMinimized 
        {
            get => isMinimized;

            set
            {
                if (isMinimized == value) return;

                isMinimized = value;

                OnPropertyChanged();
            }
        }

        public bool IsFloorNumberSearched 
        {
            get => isFloorNumberSearched;

            private set
            {
                if (isFloorNumberSearched == value) return;

                isFloorNumberSearched = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Очистить состояние поиска.
        /// </summary>
        public void ClearSearch() 
        {
            searchHelper.ClearSearch();
        }

        /// <summary>
        /// Воспринять внешний запрос на поиск элемента по введённому фильтру и названиям свойств объекта, по которым производится поиск.
        /// </summary>
        /// <param name="searchedPropNames">Набор названий свойств объекта, по которым производится поиск.</param>
        /// <param name="filter">Фильтр поиска, введённый юзером.</param>
        public void AcceptSearch(IEnumerable<string> searchedPropNames, string filter) 
        {
            searchHelper.AcceptSearch(searchedPropNames, filter);
        }


        private void PrepareSearch() 
        {
            searchHelper
                .SetSearchFilterSetter(searchFilter => SearchFilter = searchFilter)
                .AddIsPropertySearchedSetter(nameof(IsFloorNumberSearched), isSearched => IsFloorNumberSearched = isSearched)
                ;
        }

    }
}