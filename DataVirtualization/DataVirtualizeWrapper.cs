using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace DataVirtualization 
{
    /// <summary>
    /// Врапер для асинхронных виртуальных данных в коллекции <see cref="AsyncVirtualizingCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class DataVirtualizeWrapper<T> : INotifyPropertyChanged where T : class 
	{
        private T data;

        public event Action<DataVirtualizeWrapper<T>> Loaded = sender => { };

		public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Конструирует <see cref="DataVirtualizeWrapper{T}"/> с заданным индексом элемента в родительской коллекции.
        /// </summary>
        /// <param name="index">Индекс элемента в родительской коллекции.</param>
		public DataVirtualizeWrapper(int index) 
		{
			Index = index;
		}


		public int Index { get; }

        public int ItemNumber => Index + 1;

        /// <summary>
        /// Возвращает флаг - загружаются ли сейчас данные данного врапера. То есть отсутствует ли объект <see cref="Data"/>.
        /// </summary>
	    public bool IsLoading => Data == null;

        /// <summary>
        /// Возвращает объект данных внутри данного врапера. Может быть <c>null</c>, если данные ещё не загружены.
        /// </summary>
	    public T Data 
		{
			get => data;

	        internal set
			{
				data = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(Data));

                if (data != null)
                {
                    Loaded(this);
                }
			}
		}

        /// <summary>
        /// Возвращает флаг - привязан ли хоть один слушатель к событию <see cref="PropertyChanged"/>.
        /// </summary>
		public bool IsInUse => PropertyChanged != null;


	    /// <summary>Возвращает строку, представляющую текущий объект.</summary>
	    /// <returns>Строка, представляющая текущий объект.</returns>
	    public override string ToString() 
	    {
	        string dataString = IsLoading ? "<null>" : Data.ToString();

	        return $"Wrapper over item [{ dataString }]";
	    }


	    private void OnPropertyChanged([CallerMemberName]string propertyName = null) 
		{
			//System.Diagnostics.Debug.Assert(this.GetType().GetProperty(propertyName) != null);
			var handler = PropertyChanged;
		    handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}