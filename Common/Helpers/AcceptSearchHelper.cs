using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;


namespace Common.Helpers 
{
    /// <summary>
    /// Содержит логику, упрощающую использование <see cref="ISearchAcceptor"/>.
    /// </summary>
    public class AcceptSearchHelper
    {
        private readonly Dictionary<string, Action<bool>> propertySetters = new Dictionary<string, Action<bool>>();

        private Action<string> searchFilterSetter;


        /// <summary>
        /// Задать сеттер проперти фильтра поиска целевого объекта.
        /// </summary>
        /// <param name="searchFilterSetter">Сеттер проперти фильтра поиска целевого объекта.</param>
        /// <returns>this.</returns>
        public AcceptSearchHelper SetSearchFilterSetter(Action<string> searchFilterSetter)
        {
            this.searchFilterSetter = searchFilterSetter ?? throw new ArgumentNullException(nameof(searchFilterSetter));

            return this;
        }

        /// <summary>
        /// Добавить название свойства целевого объекта, доступного для поиска по нему, и сеттер свойства, которое используется для поиска по искомому свойству.
        /// <para>Например, свойству <code>string IssueInfo { get; set; }</code>  может соответствовать свойство <code>bool IsIssueInfoSearched { get; set; }</code>.</para>
        /// </summary>
        /// <param name="propertyName">Название свойства целевого объекта, доступного для поиска по этому названию.</param>
        /// <param name="isSearchedSetter">Сеттер свойства, которое используется для поиска по искомому свойству.</param>
        /// <returns>this.</returns>
        public AcceptSearchHelper AddIsPropertySearchedSetter(string propertyName, Action<bool> isSearchedSetter)
        {
            if (isSearchedSetter == null) throw new ArgumentNullException(nameof(isSearchedSetter));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            if (!propertySetters.ContainsKey(propertyName))
            {
                propertySetters.Add(propertyName, isSearchedSetter);
            }

            return this;
        }

        /// <summary>
        /// Очистить состояние поиска целевого объекта.
        /// </summary>
        public void ClearSearch()
        {
            searchFilterSetter(string.Empty);

            foreach (Action<bool> propertySetter in propertySetters.Values)
            {
                propertySetter(false);
            }
        }

        /// <summary>
        /// Принять запрос на поиск по данным свойствам целевого объекта, на который работает данный <see cref="AcceptSearchHelper"/>.
        /// </summary>
        /// <param name="searchedPropNames">Набор названий свойств объекта для поиска по ним.</param>
        /// <param name="filter">Фильтр поиска.</param>
        public void AcceptSearch(IEnumerable<string> searchedPropNames, string filter) 
        {
            searchedPropNames.AssertNotNull(nameof(searchedPropNames));

            if (string.IsNullOrWhiteSpace(filter))
            {
                ClearSearch();
                return;
            }

            searchFilterSetter(filter);

            foreach (string propName in searchedPropNames)
            {
                if (propertySetters.ContainsKey(propName))
                {
                    propertySetters[propName](true);
                }
            }
        }
    }
}
