//--------------------------------------------------------------------------
// File:    ReadOnlyCollection.cs
// Content:	Implementation of class ReadOnlyCollection
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace SmartExpert.Collections.Generic
{
	/// <summary>
	/// Wraps an existing ICollection as read only, following the pattern of 
	/// ReadOnlyCollection to simply no-op modifying functions intead of throwing
	/// Exceptions.
	/// </summary>
	/// <typeparam name="T">Item Type of the Collection.</typeparam>
	[Serializable, DebuggerTypeProxy(typeof(CollectionDebugView<>)), DebuggerDisplay("Count = {Count}")]
	public class ReadOnlyCollection<T> : ICollection<T>
	{
		#region Private Members

		private ICollection<T> m_CollectionToWrap;

		#endregion

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyCollection{T}"/> class.
		/// </summary>
		/// <param name="collectionToWrap">The collection to wrap.</param>
		public ReadOnlyCollection(ICollection<T> collectionToWrap)
		{
			m_CollectionToWrap = collectionToWrap ?? new List<T>();
		}

		#endregion

        #region Public static Methods

		/// <summary>
		/// Returned a read only wrapper around the collectionToWrap.
		/// </summary>
		/// <param name="collectionToWrap">The collection to wrap.</param>
		/// <returns>Readonly collection mapper around a given collection.</returns>
		public static ReadOnlyCollection<T> AsReadOnly( ICollection<T> collectionToWrap )
		{
			return new ReadOnlyCollection<T>(collectionToWrap ?? new List<T>());
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Gets the value at the specified index. Set
		/// does not change a read only Collection
		/// </summary>
		/// <value>The element with the specified key.</value>
		public T this[int index]
		{
			get { return m_CollectionToWrap.ElementAt(index); }
		}

		/// <summary>
		/// Add does not change a ReadOnlyCollection
		/// </summary>
		/// <param name="item">The object to add to the <see cref="ReadOnlyCollection{T}"></see>.</param>
		public void Add(T item)
		{
		}

		/// <summary>
		/// Clear does not change a ReadOnlyCollection
		/// </summary>
		public void Clear()
		{
		}

		/// <summary>
		/// Determines whether the <see cref="ReadOnlyCollection{T}"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlyCollection{T}"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="ReadOnlyCollection{T}"></see>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return m_CollectionToWrap.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the <see cref="ReadOnlyCollection{T}"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="ReadOnlyCollection{T}"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="ReadOnlyCollection{T}"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			m_CollectionToWrap.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="ReadOnlyCollection{T}"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="ReadOnlyCollection{T}"></see>.</returns>
		public int Count
		{
			get { return m_CollectionToWrap.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ReadOnlyCollection{T}"></see> is read-only.
		/// </summary>
		/// <value>Returns always true.</value>
		/// <returns>true if the <see cref="ReadOnlyCollection{T}"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Remove does not change a ReadOnlyCollection
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="ReadOnlyCollection{T}"></see>.</param>
		/// <returns>
		/// Returns always false.
		/// </returns>
		public bool Remove(T item)
		{
			return false;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator{T}"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_CollectionToWrap.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_CollectionToWrap.GetEnumerator();
		}

		#endregion

	}
}