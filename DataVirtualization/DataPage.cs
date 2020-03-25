using System;
using System.Collections.Generic;
using System.Linq;


namespace DataVirtualization 
{
    /// <summary>
    /// Страница данных для использования в <see cref="AsyncVirtualizingCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">Тип данных в коллекции.</typeparam>
    public class DataPage<T> where T : class 
    {
        public DataPage(int firstIndex, int pageLength) 
        {
            Items = new List<DataVirtualizeWrapper<T>>(pageLength);

            for (int i = 0; i < pageLength; i++)
            {
                Items.Add(new DataVirtualizeWrapper<T>(firstIndex + i));
            }

            TouchTime = DateTime.Now;
        }


        public IList<DataVirtualizeWrapper<T>> Items { get; }

        public DateTime TouchTime { get; set; }

        public bool IsInUse 
        {
            get { return Items.Any(wrapper => wrapper.IsInUse); }
        }


        public void Populate(IList<T> newItems) 
        {
            int i;
            int index = 0;
            for (i = 0; i < newItems.Count && i < Items.Count; i++)
            {
                Items[i].Data = newItems[i];
                index = Items[i].Index;
            }

            while (i < newItems.Count)
            {
                index++;
                Items.Add(new DataVirtualizeWrapper<T>(index) { Data = newItems[i]} );
                i++;
            }

            while (i < Items.Count)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }

        /// <summary>Возвращает строку, представляющую текущий объект.</summary>
        /// <returns>Строка, представляющая текущий объект.</returns>
        public override string ToString() 
        {
            return $"Page<{typeof(T).Name}> of { Items.Count } items(s)";
        }
    }
}