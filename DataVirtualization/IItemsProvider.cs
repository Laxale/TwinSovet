using System;
using System.Collections.Generic;


namespace DataVirtualization 
{
	/// <summary>
	/// Represents a provider of collection details.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	public interface IItemsProvider<T> 
	{
		/// <summary>
		/// Fetches the total number of items available.
		/// </summary>
		/// <returns></returns>
		int FetchCount();

        /// <summary>
        /// Проверить наличие кэшированного декоратора по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для проверки.</param>
        /// <returns>True, если есть хоть один декоратор, удовлетворяющий предикату.</returns>
        bool Any(Func<T, bool> predicate);

        /// <summary>
        /// Обновить состояние провайдера.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Задать фильтр выборки отображаемых объектов.
        /// </summary>
        /// <param name="predicate">Функция выборки отображаемых объектов. Может быть null.</param>
        void SetFilter(Func<T, bool> predicate);

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsCount">Items count to fetch.</param>
        /// <param name="overallCount">Total count of items in storage.</param>
        /// <returns></returns>
        IList<T> FetchRange(int startIndex, int itemsCount, out int overallCount);
    }
}