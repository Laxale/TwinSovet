using System;
using System.Collections;
using System.Collections.Generic;


namespace DataVirtualization 
{
    partial class VirtualizingCollection<TWrapped> : IList<DataVirtualizeWrapper<TWrapped>>, IList where TWrapped : class 
    {
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="ICollection"/> is read-only.
        /// </exception>
        public void Add(DataVirtualizeWrapper<TWrapped> item)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="IList"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="IList"/> is read-only.
        /// </exception>
        public void Insert(int index, DataVirtualizeWrapper<TWrapped> item)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (DataVirtualizeWrapper<TWrapped>)value);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="IList"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="IList"/> is read-only.
        /// </exception>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection"/>; 
        /// otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="ICollection"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="ICollection"/> is read-only.
        /// </exception>
        public bool Remove(DataVirtualizeWrapper<TWrapped> item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
		/// Not supported.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that 
		/// is the destination of the elements copied from <see cref="ICollection"/>. 
		/// The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="arrayIndex"/> is less than 0.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.
		/// -or-
		/// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
		/// -or-
		/// The number of elements in the source <see cref="ICollection"/> 
		/// is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
		/// -or-
		/// Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo(DataVirtualizeWrapper<TWrapped>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }
    }
}