//--------------------------------------------------------------------------
// File:    TrackedList.cs
// Content:	Implementation of class TrackedList
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Collections.Generic
{
	///<summary>List that trackes changes made to the list items</summary>
	[Serializable, DebuggerTypeProxy(typeof(CollectionDebugView<>)), DebuggerDisplay("Count = {Count}")]
	public class TrackedList<T> : IList<T>, IList
	{
		#region Private Members

		private List<ValueEl> m_CombinedValues;
		private List<T> m_RemovedValues;
		private DateTime m_LastChanged;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackedList&lt;T&gt;"/> class.
		/// </summary>
		public TrackedList()
		{
			m_CombinedValues = new List<ValueEl>();
			m_RemovedValues = new List<T>();
			m_LastChanged = DateTime.UtcNow;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TrackedList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="values">The values to track.</param>
		public TrackedList(IEnumerable<T> values)
		{
			m_CombinedValues = new List<ValueEl>();
			m_RemovedValues = new List<T>();
			m_LastChanged = DateTime.UtcNow;

			Load(values);
		}

		#endregion

		#region TrackedList Members

		/// <summary>
		/// Loads the specified values into the tracked list.
		/// </summary>
		/// <param name="values">The values to track.</param>
		public void Load(IEnumerable<T> values)
		{
			m_CombinedValues.Clear();
			m_RemovedValues.Clear();

			if (values == null)
				return;

			foreach (var local in values)
			{
				ValueEl item = new ValueEl
				{
					IsInserted = false,
					OriginalValue = new Pair<T, T>(local, local)
				};
				m_CombinedValues.Add(item);
			}
		}

		/// <summary>
		/// Resets the tracking.
		/// </summary>
		public void ResetTracking()
		{
			m_RemovedValues.Clear();
			foreach (var el in m_CombinedValues)
			{
				if (el.IsInserted)
				{
					el.IsInserted = false;
					el.OriginalValue = new Pair<T, T>(el.InsertedValue, el.InsertedValue);
				}
				else
				{
					Pair<T, T> originalValue = el.OriginalValue;
					if (!originalValue.First.Equals(originalValue.Second))
					{
						originalValue.First = originalValue.Second;
					}
				}
			}
		}

		internal void MarkChange()
		{
			m_LastChanged = DateTime.UtcNow;
		}

		/// <summary>
		/// Gets the last change timestamp.
		/// </summary>
		public DateTime LastChanged
		{
			get
			{
				return m_LastChanged;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="TrackedList&lt;T&gt;"/> has changed.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if changed; otherwise, <see langword="false"/>.
		/// </value>
		public bool HasChanged
		{
			get
			{
				if (m_RemovedValues.Count > 0)
					return true;

				foreach (var el in m_CombinedValues)
				{
					if (el.IsInserted)
						return true;
					
					if (!el.OriginalValue.First.Equals(el.OriginalValue.Second))
						return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Gets the changed values.
		/// </summary>
		public List<Pair<T, T>> ChangedValues
		{
			get
			{
				var result = new List<Pair<T, T>>();
				foreach (var el in m_CombinedValues)
				{
					if (!el.IsInserted && !el.OriginalValue.First.Equals(el.OriginalValue.Second))
					{
						result.Add(new Pair<T, T>(el.OriginalValue.First, el.OriginalValue.Second));
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the inserted values.
		/// </summary>
		public List<T> Inserted
		{
			get
			{
				var result = new List<T>();
				foreach (var el in m_CombinedValues)
				{
					if (el.IsInserted)
						result.Add(el.InsertedValue);
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the removed collection items.
		/// </summary>
		public List<T> Removed
		{
			get
			{
				return m_RemovedValues;
			}
		}

		#endregion

		#region IList<T> Members


		/// <summary>
		/// Returns the index the of value in the tracked list.
		/// </summary>
		/// <param name="value">The value to get the index for.</param>
		/// <returns>Returns the index of value or -1 if the value is not part of the tracked list.</returns>
		public int IndexOf(T value)
		{
			if (value.GetType().IsValueType.IsFalse())
			{
				ArgChecker.ShouldNotBeNull(value, "value");
			}

			int currentIndex = 0;
			foreach (var el in m_CombinedValues)
			{
				if (el.IsInserted && el.InsertedValue.Equals(value))
				{
					return currentIndex;
				}
				if (!el.IsInserted && el.OriginalValue.Second.Equals(value))
				{
					return currentIndex;
				}
				currentIndex++;
			}
			return -1;
		}

		/// <summary>
		/// Inserts the value at the specified index.
		/// </summary>
		/// <param name="index">The target index.</param>
		/// <param name="value">The value to insert.</param>
		/// <exception cref="ArgOutOfRangeException{TValue}">index is not a valid index in the tracked list.</exception>
		public void Insert(int index, T value)
		{
			ArgChecker.ShouldBeInRange(index, "index", 0, m_CombinedValues.Count - 1);

			if (value.GetType().IsValueType.IsFalse())
			{
				ArgChecker.ShouldNotBeNull(value, "value");
			}
			
			MarkChange();

			ValueEl item = new ValueEl
			{
				IsInserted = true,
				InsertedValue = value
			};
			m_CombinedValues.Insert(index, item);
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="ArgOutOfRangeException{TValue}">index is not a valid index in the tracked list.</exception>
		public void RemoveAt(int index)
		{
			ArgChecker.ShouldBeInRange(index, "index", 0, m_CombinedValues.Count - 1);
			
			MarkChange();

			ValueEl el = m_CombinedValues[index];
			if (el.IsInserted)
			{
				m_CombinedValues.RemoveAt(index);
			}
			else
			{
				Pair<T, T> originalValue = m_CombinedValues[index].OriginalValue;
				m_CombinedValues.RemoveAt(index);
				m_RemovedValues.Add(originalValue.First);
			}
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="ArgOutOfRangeException{TValue}">index is not a valid index in the tracked list.</exception>
		public T this[int index]
		{
			get
			{
				if ((index < 0) || (index >= m_CombinedValues.Count))
				{
					throw new ArgumentOutOfRangeException("index");
				}
				ValueEl el = m_CombinedValues[index];
				if (el.IsInserted)
				{
					return el.InsertedValue;
				}
				return el.OriginalValue.Second;
			}
			set
			{
				ArgChecker.ShouldBeInRange(index, "index", 0, m_CombinedValues.Count-1);
				
				if (value.GetType().IsValueType.IsFalse())
				{
					ArgChecker.ShouldNotBeNull(value, "value");
				}

				MarkChange();

				ValueEl el = m_CombinedValues[index];

				if (el.IsInserted)
					el.InsertedValue = value;
				else
					el.OriginalValue.Second = value;
			}
		}

		#endregion

		#region IList Members

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			
			set
			{
				ArgChecker.ShouldNotBeNull(value, "value");
				this[index] = (T)value;
			}
		}

		int IList.Add(object value)
		{
			ArgChecker.ShouldNotBeNull(value, "value");
			
			Add((T)value);

			return Count;
		}

		void IList.Clear()
		{
			Clear();
		}

		bool IList.Contains(object value)
		{
			ArgChecker.ShouldNotBeNull(value, "value");

			return Contains((T)value);
		}

		int IList.IndexOf(object value)
		{
			ArgChecker.ShouldNotBeNull(value, "value");

			return IndexOf((T)value);
		}

		void IList.Insert(int index, object value)
		{
			ArgChecker.ShouldNotBeNull(value, "value");

			Insert(index, (T)value);
		}

		void IList.Remove(object value)
		{
			ArgChecker.ShouldNotBeNull(value, "value");

			Remove((T)value);
		}

		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		bool IList.IsFixedSize
		{
			get { return IsFixedSize; }
		}

		bool IList.IsReadOnly
		{
			get { return IsReadOnly; }
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Adds an item to the <see cref="TrackedList{T}"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="TrackedList{T}"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="TrackedList{T}"/> is read-only.</exception>
		public void Add(T item)
		{
			MarkChange();
			var value = new ValueEl
			{
				IsInserted = true,
				InsertedValue = item
			};
			m_CombinedValues.Add(value);
		}

		/// <summary>
		/// Removes all items from the <see cref="TrackedList{T}"/>.
		/// </summary>
		public void Clear()
		{
			MarkChange();
			foreach (var el in m_CombinedValues)
			{
				if (!el.IsInserted)
					m_RemovedValues.Add(el.OriginalValue.First);
			}
			m_CombinedValues.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="TrackedList{T}"/> contains the specified value.
		/// </summary>
		/// <param name="value">The value to look for.</param>
		/// <returns>
		///   <see langword="true"/> if the <see cref="TrackedList{T}"/> contains the specified value; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Contains(T value)
		{
			return m_CombinedValues.Any(el => el.GetCurrentValue().Equals(value));
		}

		/// <summary>
		/// Copies the elements of the <see cref="TrackedList{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="TrackedList{T}"/>. 
		/// The <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.
		/// -or-arrayIndex is equal to or greater than the length of array.
		/// -or-The number of elements in the source <see cref="TrackedList{T}" /> is greater than the available space from arrayIndex to the end of the destination array.
		/// -or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			((ICollection)this).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Removes the specified value from the TrackedList{T}.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>Returns <see langword="true"/> if the item could be removed from the list; otherwise <see langword="false"/>.</returns>
		/// <remarks>The removed item will be added to the RemovedValues list for tracking purpose.</remarks>
		public bool Remove(T value)
		{
			MarkChange();
			foreach (var el in m_CombinedValues)
			{
				if (el.IsInserted && el.InsertedValue.Equals(value))
				{
					m_CombinedValues.Remove(el);
					return true;
				}
				if (!el.IsInserted && el.OriginalValue.Second.Equals(value))
				{
					m_CombinedValues.Remove(el);
					m_RemovedValues.Add(el.OriginalValue.First);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="TrackedList{T}"></see>.
		/// </summary>
		/// <returns>The number of elements contained in the <see cref="TrackedList{T}"></see>.</returns>
		public int Count
		{
			get
			{
				return m_CombinedValues.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="TrackedList{T}"></see> has a fixed size.
		/// </summary>
		/// <returns>Returns always <see langword="false"/>.</returns>
		public bool IsFixedSize
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="TrackedList{T}"></see> is read-only.
		/// </summary>
		/// <returns>Returns always <see langword="false"/>.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the <see cref="TrackedList{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the elements copied from <see cref="TrackedList{T}"/>. 
		/// The <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.
		/// -or-arrayIndex is equal to or greater than the length of array.
		/// -or-The number of elements in the source <see cref="TrackedList{T}" /> is greater than the available space from arrayIndex to the end of the destination array.
		/// -or-Type T cannot be cast automatically to the type of the destination array.</exception>
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(StringResources.TrackedCollectionNotOneDimensional);
			}
			if (arrayIndex >= array.GetLength(0))
			{
				throw new ArgumentException(StringResources.TrackedCollectionIndexNotInArray);
			}
			if ((array.GetLength(0) - arrayIndex) < m_CombinedValues.Count)
			{
				throw new ArgumentException(StringResources.TrackedCollectionArrayTooSmall);
			}
			foreach (var el in m_CombinedValues)
			{
				array.SetValue(el.GetCurrentValue(), arrayIndex);
				arrayIndex++;
			}
		}


		int ICollection.Count
		{
			get
			{
				return Count;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region Enumerator Methods

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator{T}"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new TrackedListEnumerator<T>("TrackedCollectionEnumerator", this, m_CombinedValues);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Nested Types

		// Nested Types
		internal class ValueEl
		{
			public T InsertedValue;
			public bool IsInserted;
			public Pair<T, T> OriginalValue;

			
			public T GetCurrentValue()
			{
				if (IsInserted)
				{
					return InsertedValue;
				}
				return OriginalValue.Second;
			}
		}

		#endregion
	}
}
