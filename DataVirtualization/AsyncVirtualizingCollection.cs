using System;
using System.Collections.Generic;
using System.Threading;


namespace DataVirtualization 
{
	/// <summary>
	/// Derived VirtualizatingCollection, performing loading asychronously.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	public sealed partial class AsyncVirtualizingCollection<T> where T : class 
    {
        private bool isLoading;
	    private bool isInitializing;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncVirtualizingCollection{T}"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageTimeout">The page timeout.</param>
        public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
			: base(itemsProvider, pageSize, pageTimeout)
		{
            SynchroContext = SynchronizationContext.Current;
        }


        /// <summary>
        /// Gets the synchronization context used for UI-related operations. This is obtained as
        /// the current SynchroContext when the AsyncVirtualizingCollection is created.
        /// </summary>
        /// <value>The synchronization context.</value>
        private SynchronizationContext SynchroContext { get; }

        /// <summary>
		/// Gets or sets a value indicating whether the collection is loading page(s).
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this collection is loading; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoading 
		{
			get => isLoading;

		    private set
			{
                if (value == isLoading) return;

                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
		}

		/// <summary>
        /// Возвращает флаг - вычисляет ли сейчас коллекция количество элементов <see cref="VirtualizingCollection{T}.Count"/>.
        /// </summary>
		public bool IsInitializing 
		{
			get => isInitializing;

		    private set
			{
                if (value == isInitializing) return;

                isInitializing = value;
                OnPropertyChanged(nameof(IsInitializing));
            }
		}


        /// <summary>
        /// Asynchronously loads the count of items.
        /// </summary>
        protected override void LoadCount() 
		{
			if (Count == 0)
			{
				IsInitializing = true;
			}

			ThreadPool.QueueUserWorkItem(LoadCountWork);
		}

        /// <summary>
	    /// Asynchronously loads the page.
	    /// </summary>
	    /// <param name="pageIndex"></param>
	    /// <param name="pageLength"></param>
	    protected override void LoadPage(int pageIndex, int pageLength) 
		{
            IsLoading = true;
            ThreadPool.QueueUserWorkItem(LoadPageWork, new [] { pageIndex, pageLength });
        }

        protected override void OnDelayedDataFound() 
        {
            base.OnDelayedDataFound();

            if (IsLoading) return;

            Console.WriteLine("Loading delayed data");
            LoadCount();
        }

        /// <summary>
        /// Performed on background thread.
        /// </summary>
        /// <param name="args">None required.</param>
        private void LoadCountWork(object args) 
        {
            int _count = FetchCount();
            SynchroContext.Send(LoadCountCompleted, _count);
        }

        /// <summary>
        /// Performed on UI-thread after LoadCountWork.
        /// </summary>
        /// <param name="args">Number of items returned.</param>
        private void LoadCountCompleted(object args) 
        {
            int newCount = (int)args;
            TakeNewCount(newCount);

            IsInitializing = false;
        }

        private void TakeNewCount(int newCount) 
	    {
            // если число объектов в коллекции не изменилось, но изменились сами объекты, то сравнение с Count полностью некорректно
            //if(newCount != Count)
            {
                Count = newCount;
                
	            ClearCache();

                FireCollectionReset();
            }
	    }

	    /// <summary>
	    /// Performed on background thread.
	    /// </summary>
	    /// <param name="state">Object (array) containing this method arguments.</param>
	    private void LoadPageWork(object state) 
		{
            int[] args = (int[])state;
            int pageIndex = args[0];
            int pageLength = args[1];

            IList<T> dataItems = FetchPage(pageIndex, pageLength, out int overallCount);

            SynchroContext.Send(LoadPageCompleted, new object[] { pageIndex, dataItems, overallCount });
        }

        /// <summary>
        /// Performed on UI-thread after LoadPageWork.
        /// </summary>
        /// <param name="state">Object (array) containing this method arguments.</param>
        private void LoadPageCompleted(object state) 
		{
			object[] args = (object[])state;
			int pageIndex = (int)args[0];
			IList<T> dataItems = (IList<T>)args[1];
			int newCount = (int)args[2];

            if (newCount != Count)
            {
                TakeNewCount(newCount);
            }

            PopulatePage(pageIndex, dataItems);

            IsLoading = false;

            EventFetchedData(dataItems);
        }
    }
}