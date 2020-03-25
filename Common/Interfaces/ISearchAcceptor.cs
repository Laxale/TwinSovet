using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Interfaces 
{
    /// <summary>
    /// Интерфейс, объявляющий реализацию способной принимать запросы на поиск по свойствам объекта.
    /// </summary>
    public interface ISearchAcceptor 
    {
        /// <summary>
        /// Возвращает внешний фильтр поиска.
        /// </summary>
        string SearchFilter { get; }

        /// <summary>
        /// Возвращает флаг - заполнен ли сейчас НЕпустой фильтр.
        /// </summary>
        bool HasFilter { get; }

        /// <summary>
        /// Очистить состояние поиска.
        /// </summary>
        void ClearSearch();

        /// <summary>
        /// Воспринять внешний запрос на поиск элемента по введённому фильтру и названиям свойств объекта, по которым производится поиск.
        /// </summary>
        /// <param name="searchedPropNames">Набор названий свойств объекта, по которым производится поиск.</param>
        /// <param name="filter">Фильтр поиска, введённый юзером.</param>
        void AcceptSearch(IEnumerable<string> searchedPropNames, string filter);
    }
}