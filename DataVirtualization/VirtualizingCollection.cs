using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace DataVirtualization 
{
	/// <summary>
	/// Specialized list implementation that provides data virtualization. The collection is divided up into pages,
	/// and pages are dynamically fetched from the IItemsProvider when required. Stale pages are removed after a
	/// configurable period of time.
	/// Intended for use with large collections on a network or disk resource that cannot be instantiated locally
	/// due to memory consumption or fetch latency.
	/// </summary>
	/// <remarks>
	/// The IList implmentation is not fully complete, but should be sufficient for use as read only collection 
	/// data bound to a suitable ItemsControl.
	/// </remarks>
	/// <typeparam name="TWrapped"></typeparam>
	public partial class VirtualizingCollection<TWrapped> where TWrapped : class
    {
        private const int dataReloadDelay = 100;
        private readonly Dictionary<int, DataPage<TWrapped>> dataPages = new Dictionary<int, DataPage<TWrapped>>();

        private int count = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection{T}"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageTimeout">The page timeout.</param>
        public VirtualizingCollection(IItemsProvider<TWrapped> itemsProvider, int pageSize, int pageTimeout) 
		{
			ItemsProvider = itemsProvider;
			PageSize = pageSize;
			PageTimeout = pageTimeout;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection{T}"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        public VirtualizingCollection(IItemsProvider<TWrapped> itemsProvider, int pageSize) 
		{
		    PageSize = pageSize;
		    ItemsProvider = itemsProvider;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection{T}"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        public VirtualizingCollection(IItemsProvider<TWrapped> itemsProvider) 
		{
			ItemsProvider = itemsProvider;
		}

        public bool AutoUpdateNotLoadedObjects { get; set; } = true;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// The first time this property is accessed, it will fetch the count from the IItemsProvider.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public virtual int Count 
        {
            get
	        {
	            if (count == -1)
	            {
	                count = 0;
	                LoadCount();
	            }

                return count;
	        }

            protected set => count = value;
        }

        /// <summary>
        /// Gets the item at the specified index. This property will fetch
        /// the corresponding page from the IItemsProvider if required.
        /// </summary>
        /// <value></value>
        public DataVirtualizeWrapper<TWrapped> this[int index] 
		{
			get
			{
                // determine which page and offset within page
				int pageIndex = index / PageSize;
				int pageOffset = index % PageSize;

				// request primary page
				RequestPage(pageIndex);

				// if accessing upper 50% then request next page
			    if (pageOffset > PageSize / 2 && pageIndex < Count / PageSize)
			    {
			        RequestPage(pageIndex + 1);
			    }

				// if accessing lower 50% then request prev page
			    if (pageOffset < PageSize / 2 && pageIndex > 0)
			    {
			        RequestPage(pageIndex - 1);
			    }

				// remove stale pages
				RemoveStalePages();

                DataPage<TWrapped> page = dataPages[pageIndex];
                // return requested item
                if (page.Items.Count <= pageOffset)
                {
                    // дурацкий костыль связан с тем, что во время асинхронного обновления коллекции UI делает скрытые неконтролируемые вызовы MeasureOverride() => this[index]
                    // эти вызовы иногда падают в IndexOutOfRangeException, потому что свойство Count уже обновлено, в таблица страниц - ещё НЕТ
                    // в таком виде хотя бы не происходит исключений, но объекты могут быть и в гуе пустышками, то есть придётся ещё раз обновить список, чтобы они стали нормальными
                    if(AutoUpdateNotLoadedObjects)
                    {
                        Console.WriteLine($"page offset out of range '{ pageIndex } | { pageOffset }'. Delayed update");
                        Task.Run(() =>
                        {
                            Thread.Sleep(dataReloadDelay);
                            OnDelayedDataFound();
                        });
                    }

                    return new DataVirtualizeWrapper<TWrapped>(index);
                }

                return page.Items[pageOffset];
			}

			set => throw new NotSupportedException();
		}

        object IList.this[int index] 
		{
		    get => this[index];

		    set => throw new NotSupportedException();
		}


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <remarks>
        /// This method should be avoided on large collections due to poor performance.
        /// </remarks>
        /// <returns>
        /// A <see cref="IEnumerator"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DataVirtualizeWrapper<TWrapped>> GetEnumerator() 
        {
            int currentCount = Count;
			for (int index = 0; index < currentCount; index++)
			{
				yield return this[index];
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator() 
		{
			return GetEnumerator();
		}

        bool IList.Contains(object value) 
		{
			return Contains((DataVirtualizeWrapper<TWrapped>)value);
		}

		public bool Contains(DataVirtualizeWrapper<TWrapped> item) 
		{
			foreach (DataPage<TWrapped> page in dataPages.Values)
			{
				if (page.Items.Contains(item))
				{
					return true;
				}
			}

			return false;
		}

        public void Clear() 
		{
			ClearCache();
		}

	    public void Refresh() 
	    {
	        Clear();
	        LoadCount();
	    }

        int IList.IndexOf(object value) 
		{
			return IndexOf((DataVirtualizeWrapper<TWrapped>)value);
		}

		public int IndexOf(DataVirtualizeWrapper<TWrapped> item) 
		{
			foreach (KeyValuePair<int, DataPage<TWrapped>> keyValuePair in dataPages)
			{
				int indexWithinPage = keyValuePair.Value.Items.IndexOf(item);
				if (indexWithinPage != -1)
				{
					return PageSize * keyValuePair.Key + indexWithinPage;
				}
			}
			return -1;
		}
        

        /// <summary>
	    /// Populates the page within the dictionary.
	    /// </summary>
	    /// <param name="pageIndex">Index of the page.</param>
	    /// <param name="dataItems">Items to populate the page with.</param>
	    protected void PopulatePage(int pageIndex, IList<TWrapped> dataItems) 
		{
			if (dataPages.TryGetValue(pageIndex, out DataPage<TWrapped> page))
            {
                page.Populate(dataItems);
            }
		}

		/// <summary>
		/// Removes all cached pages. This is useful when the count of the underlying collection changes.
		/// </summary>
		protected void ClearCache() 
		{
			dataPages.Clear();
		}
        
		/// <summary>
		/// Loads the count of items.
		/// </summary>
		protected virtual void LoadCount() 
		{
			count = FetchCount();
		}

        protected virtual void OnDelayedDataFound() { }

		/// <summary>
		/// Loads the page of items.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageLength">Number of items in the page.</param>
		protected virtual void LoadPage(int pageIndex, int pageLength) 
		{
		    PopulatePage(pageIndex, FetchPage(pageIndex, pageLength, out int totalCount));
			Count = totalCount;
		}

        /// <summary>
	    /// Fetches the requested page from the IItemsProvider.
	    /// </summary>
	    /// <param name="pageIndex">Index of the page.</param>
	    /// <param name="pageLength">Items count in the page.</param>
	    /// <param name="count">Total items count.</param>
	    /// <returns></returns>
	    protected IList<TWrapped> FetchPage(int pageIndex, int pageLength, out int count) 
		{
            return ItemsProvider.FetchRange(pageIndex * PageSize, pageLength, out count);
        }

		/// <summary>
		/// Fetches the count of itmes from the IItemsProvider.
		/// </summary>
		/// <returns></returns>
		protected int FetchCount() 
		{
			return ItemsProvider.FetchCount();
		}


        /// <summary>
        /// Cleans up any stale pages that have not been accessed in the period dictated by PageTimeout.
        /// </summary>
        private void RemoveStalePages() 
        {
            int[] keys = dataPages.Keys.ToArray();
            foreach (int key in keys)
            {
                // page 0 is a special case, since WPF ItemsControl access the first item frequently
                if (key != 0 && (DateTime.Now - dataPages[key].TouchTime).TotalMilliseconds > PageTimeout)
                {
                    bool removePage = true;
                    if (dataPages.TryGetValue(key, out DataPage<TWrapped> page))
                    {
                        removePage = !page.IsInUse;
                    }

                    if (removePage)
                    {
                        dataPages.Remove(key);
                        Console.WriteLine("Removed Page: " + key);
                    }
                }
            }
        }


        /// <summary>
        /// Makes a request for the specified page, creating the necessary slots in the dictionary, and updating the page touch time.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        private void RequestPage(int pageIndex) 
        {
            if (!dataPages.ContainsKey(pageIndex))
            {
                // Create a page of empty data wrappers
                int pageLength = Math.Min(PageSize, Count - pageIndex * PageSize);
                DataPage<TWrapped> page = new DataPage<TWrapped>(pageIndex * PageSize, pageLength);
                dataPages.Add(pageIndex, page);

                LoadPage(pageIndex, pageLength);
            }
            else
            {
                dataPages[pageIndex].TouchTime = DateTime.Now;
            }
        }
    }
}