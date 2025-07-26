// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Andreas Börcsök" file="HashSet.cs">
//	 Copyright © 2009 Andreas Börcsök
// </copyright>
//
// <summary>
//   Implementation of class HashSet<T>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Collections.Generic
{
	/// <summary>
	/// Typed HashSet implementation.
	/// </summary>
	/// <typeparam name="T">
	/// The HashSet item type.
	/// </typeparam>
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(HashSetDebugView<>))]
	public class HashSet<T> : ICollection<T>
	{
		#region Members

		/// <summary>
		/// </summary>
		private readonly IEqualityComparer<T> m_Comparer;

		/// <summary>
		/// </summary>
		private Entry[] m_Entries;

		/// <summary>
		/// </summary>
		private int m_EntriesUsed;

		/// <summary>
		/// </summary>
		private int m_FirstFreeIndex;

		/// <summary>
		/// </summary>
		private int m_FreeCount;

		/// <summary>
		/// </summary>
		private int[] m_HashToEntryIndex;

		/// <summary>
		/// </summary>
		private int m_Version;

		#endregion

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		public HashSet()
			: this(0, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="enumerable">
		/// The sequence to add.
		/// </param>
		public HashSet(IEnumerable<T> enumerable)
			: this(enumerable, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">
		/// The comparer.
		/// </param>
		public HashSet(IEqualityComparer<T> comparer)
			: this(0, comparer)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">
		/// The capacity.
		/// </param>
		public HashSet(int capacity)
			: this(capacity, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">
		/// The collection.
		/// </param>
		/// <param name="comparer">
		/// The comparer.
		/// </param>
		/// <remarks>
		/// Static overload to prevent runtime type checks.
		/// </remarks>
		public HashSet(ICollection<T> collection, IEqualityComparer<T> comparer)
			: this(0, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}

			Initialize(collection.Count);
			foreach (T local in collection)
			{
				Add(local);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="enumerable">
		/// The enumerable.
		/// </param>
		/// <param name="comparer">
		/// The comparer.
		/// </param>
		public HashSet(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
			: this(0, comparer)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable");
			}

			var is2 = enumerable as ICollection<T>;
			if (is2 != null)
			{
				Initialize(is2.Count);
			}

			foreach (T local in enumerable)
			{
				Add(local);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashSet&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">
		/// The capacity.
		/// </param>
		/// <param name="comparer">
		/// The comparer.
		/// </param>
		public HashSet(int capacity, IEqualityComparer<T> comparer)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}

			if (capacity > 0)
			{
				Initialize(capacity);
			}

			if (comparer == null)
			{
				comparer = EqualityComparer<T>.Default;
			}

			m_Comparer = comparer;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer
		{
			get
			{
				return m_Comparer;
			}
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Clears the hash set.
		/// </summary>
		public void Clear()
		{
			if (m_EntriesUsed > 0)
			{
				for (int i = 0; i < m_HashToEntryIndex.Length; i++)
				{
					m_HashToEntryIndex[i] = -1;
				}

				Array.Clear(m_Entries, 0, m_Entries.Length);
				m_FirstFreeIndex = -1;
				m_EntriesUsed = 0;
				m_FreeCount = 0;
				m_Version++;
			}
		}

		/// <summary>
		/// Determines whether the hash set contains the specified element.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the hash set contains the specified element; otherwise, <see langword="false"/>.
		/// </returns>
		public bool Contains(T element)
		{
			return GetEntryIndex(element) >= 0;
		}

		/// <summary>
		/// Copies hash set elements to array
		/// </summary>
		/// <param name="array">
		/// The target array.
		/// </param>
		/// <param name="arrayIndex">
		/// Start index of the target array.
		/// </param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			CopyTo(array, arrayIndex, Count);
		}

		/// <summary>
		/// Removes the specified element.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// </returns>
		public bool Remove(T element)
		{
			if (m_HashToEntryIndex != null)
			{
				int num = m_Comparer.GetHashCode(element) & 0x7fffffff;
				int index = num % m_HashToEntryIndex.Length;
				int num3 = m_HashToEntryIndex[index];
				int prevIndex = -1;
				while (num3 >= 0)
				{
					Entry entry = m_Entries[num3];
					int nextEntryIndex = entry.NextEntryIndex;
					if ((entry.HashCode == num) && m_Comparer.Equals(entry.Element, element))
					{
						RemoveEntry(num3, prevIndex);
						m_Version++;
						return true;
					}

					prevIndex = num3;
					num3 = nextEntryIndex;
				}
			}

			return false;
		}

		/// <summary>
		/// </summary>
		/// <param name="item">
		/// </param>
		void ICollection<T>.Add(T item)
		{
			Add(item);
		}

		/// <summary>
		/// </summary>
		/// <returns>
		/// </returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// </summary>
		/// <returns>
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				return m_EntriesUsed - m_FreeCount;
			}
		}

		/// <summary>
		/// </summary>
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// </returns>
		public bool Add(T element)
		{
			T local;
			return AddImpl(element, out local);
		}

		/// <summary>
		/// </summary>
		/// <param name="element">
		/// </param>
		/// <param name="existingElement">
		/// </param>
		/// <returns>
		/// </returns>
		private bool AddImpl(T element, out T existingElement)
		{
			Entry entry;
			int myFirstFreeIndex;
			if (m_HashToEntryIndex == null)
			{
				m_HashToEntryIndex = EmptyArray<int>.Instance;
				Initialize(0);
			}

			int num = m_Comparer.GetHashCode(element) & 0x7fffffff;
			int index = num % m_HashToEntryIndex.Length;
			for (int i = m_HashToEntryIndex[index]; i >= 0; i = entry.NextEntryIndex)
			{
				entry = m_Entries[i];
				if ((entry.HashCode == num) && m_Comparer.Equals(element, entry.Element))
				{
					existingElement = entry.Element;
					return false;
				}
			}

			if (m_FreeCount > 0)
			{
				myFirstFreeIndex = m_FirstFreeIndex;
				m_FirstFreeIndex = m_Entries[myFirstFreeIndex].NextEntryIndex;
				m_FreeCount--;
			}
			else
			{
				if (m_EntriesUsed == m_Entries.Length)
				{
					Resize();
					index = num % m_HashToEntryIndex.Length;
				}

				myFirstFreeIndex = m_EntriesUsed;
				m_EntriesUsed++;
			}

			m_Entries[myFirstFreeIndex].HashCode = num;
			m_Entries[myFirstFreeIndex].NextEntryIndex = m_HashToEntryIndex[index];
			m_Entries[myFirstFreeIndex].Element = element;
			m_HashToEntryIndex[index] = myFirstFreeIndex;
			m_Version++;
			existingElement = default(T);
			return true;

		}

		/// <summary>
		/// </summary>
		/// <param name="collection">
		/// </param>
		/// <param name="other">
		/// </param>
		/// <returns>
		/// </returns>
		private static bool CollectionContainsAll(ICollection<T> collection, IEnumerable<T> other)
		{
			return other.All(collection.Contains);
		}

		/// <summary>
		/// Copies hash set elements to array.
		/// </summary>
		/// <param name="array">
		/// The target array.
		/// </param>
		public void CopyTo(T[] array)
		{
			CopyTo(array, 0, Count);
		}

		/// <summary>
		/// Copies hash set elements to array.
		/// </summary>
		/// <param name="array">
		/// The target array.
		/// </param>
		/// <param name="arrayIndex">
		/// Start index in the target array.
		/// </param>
		/// <param name="count">
		/// The max count of copied elements.
		/// </param>
		public void CopyTo(T[] array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			int num = count;
			int num2 = arrayIndex;
			foreach (T local in this)
			{
				if (num-- == 0)
				{
					break;
				}

				array[num2++] = local;
			}
		}

		/// <summary>
		/// Removes all elements that are in the other sequence form the hash set.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if (Count != 0)
			{
				if (other == this)
				{
					Clear();
				}
				else
				{
					foreach (T local in other)
					{
						Remove(local);
					}
				}
			}
		}

		/// <summary>
		/// Gets the entry index value.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// Returns the entry index value.
		/// </returns>
		private int GetEntryIndex(T key)
		{
			if (m_HashToEntryIndex != null)
			{
				Entry entry;
				int num = m_Comparer.GetHashCode(key) & 0x7fffffff;
				int index = num % m_HashToEntryIndex.Length;
				for (int i = m_HashToEntryIndex[index]; i >= 0; i = entry.NextEntryIndex)
				{
					entry = m_Entries[i];
					if ((entry.HashCode == num) && m_Comparer.Equals(key, entry.Element))
					{
						return i;
					}
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets the enumerator value.
		/// </summary>
		/// <returns>
		/// Returns the enumerator value.
		/// </returns>
		public ElementEnumerator GetEnumerator()
		{
			return new ElementEnumerator(this);
		}

		/// <summary>
		/// Gets the value value.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// Returns the value value.
		/// </returns>
		public T GetValue(T key)
		{
			int entryIndex = GetEntryIndex(key);
			if (entryIndex < 0)
			{
				throw new KeyNotFoundException();
			}

			return m_Entries[entryIndex].Element;
		}

		/// <summary>
		/// </summary>
		/// <param name="capacity">
		/// </param>
		private void Initialize(int capacity)
		{
			int num = PrimeFinder.NextPrime(capacity);
			m_HashToEntryIndex = new int[num];
			for (int i = 0; i < m_HashToEntryIndex.Length; i++)
			{
				m_HashToEntryIndex[i] = -1;
			}

			m_Entries = new Entry[num];
			m_FirstFreeIndex = -1;
		}

		/// <summary>
		/// Interns the specified key.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// </returns>
		public T Intern(T key)
		{
			T local;
			if (AddImpl(key, out local))
			{
				return key;
			}

			return local;
		}

		/// <summary>
		/// Intersects the with.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		private void IntersectWith(ICollection<T> other)
		{
			for (int i = 0; i < m_HashToEntryIndex.Length; i++)
			{
				int index = m_HashToEntryIndex[i];
				int prevIndex = -1;
				while (index >= 0)
				{
					Entry entry = m_Entries[index];
					if (!other.Contains(entry.Element))
					{
						RemoveEntry(index, prevIndex);
					}
					else
					{
						prevIndex = index;
					}

					index = entry.NextEntryIndex;
				}
			}
		}

		/// <summary>
		/// Intersects the with.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if ((other != this) && (Count != 0))
			{
				var is2 = other as ICollection<T>;
				if (is2 != null)
				{
					if (is2.Count == 0)
					{
						Clear();
						return;
					}

					if (is2 is HashSet<T>)
					{
						IntersectWith(is2);
						return;
					}
				}

				var array = new BitArray(m_EntriesUsed, false);
				foreach (T local in other)
				{
					int entryIndex = GetEntryIndex(local);
					if (entryIndex >= 0)
					{
						array.Set(entryIndex, true);
					}
				}

				for (int i = 0; i < m_HashToEntryIndex.Length; i++)
				{
					int index = m_HashToEntryIndex[i];
					int prevIndex = -1;
					while (index >= 0)
					{
						Entry entry = m_Entries[index];
						if (!array[index])
						{
							RemoveEntry(index, prevIndex);
						}
						else
						{
							prevIndex = index;
						}

						index = entry.NextEntryIndex;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="set">
		/// </param>
		/// <returns>
		/// </returns>
		private bool IsGreaterSet(HashSet<T> set)
		{
			return Comparer.Equals(set.Comparer) && (set.Count > Count);
		}

		/// <summary>
		/// </summary>
		/// <param name="other">
		/// </param>
		/// <returns>
		/// </returns>
		private bool IsSubsetOf(ICollection<T> other)
		{
			for (int i = 0; i < m_EntriesUsed; i++)
			{
				if ((m_Entries[i].HashCode >= 0) && !other.Contains(m_Entries[i].Element))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether [is subset of] [the specified other].
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if [is subset of] [the specified other]; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if ((Count != 0) && (other != this))
			{
				var set = other as HashSet<T>;
				if ((set != null) && Equals(set.Comparer, Comparer))
				{
					return (Count <= set.Count) && IsSubsetOf(set);
				}

				var array = new BitArray(m_EntriesUsed, false);
				foreach (T local in other)
				{
					int entryIndex = GetEntryIndex(local);
					if (entryIndex >= 0)
					{
						array.Set(entryIndex, true);
					}
				}

				for (int i = 0; i < m_EntriesUsed; i++)
				{
					if ((m_Entries[i].HashCode >= 0) && !array[i])
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether [is superset of] [the specified other].
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if [is superset of] [the specified other]; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsSupersetOf(HashSet<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if ((other == this) || (other.Count == 0))
			{
				return true;
			}

			if (IsGreaterSet(other))
			{
				return false;
			}

			return CollectionContainsAll(this, other);
		}

		/// <summary>
		/// Determines whether [is superset of] [the specified other].
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if [is superset of] [the specified other]; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsSupersetOf(ICollection<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if ((other != this) && (other.Count != 0))
			{
				return IsSupersetOfImpl(other);
			}

			return true;
		}

		/// <summary>
		/// Determines whether [is superset of] [the specified other].
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if [is superset of] [the specified other]; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
				return false;

// ReSharper disable PossibleMultipleEnumeration
			if ((other != this) && other.CanBeProvenEmptyFast().IsFalse())
				return IsSupersetOfImpl(other);
// ReSharper restore PossibleMultipleEnumeration

			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="other">
		/// </param>
		/// <returns>
		/// </returns>
		private bool IsSupersetOfImpl(IEnumerable<T> other)
		{
			var set = other as HashSet<T>;
			if ((set != null) && IsGreaterSet(set))
			{
				return false;
			}

			return CollectionContainsAll(this, other);
		}

		/// <summary>
		/// Overlapses the specified other.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// </returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if (Count == 0)
			{
				return false;
			}

			return other == this || other.Any(Contains);
		}

		/// <summary>
		/// </summary>
		/// <param name="entryIndex">
		/// </param>
		/// <param name="prevIndex">
		/// </param>
		private void RemoveEntry(int entryIndex, int prevIndex)
		{
			Entry entry = m_Entries[entryIndex];
			if (prevIndex < 0)
			{
				m_HashToEntryIndex[entry.HashCode % m_HashToEntryIndex.Length] = entry.NextEntryIndex;
			}
			else
			{
				m_Entries[prevIndex].NextEntryIndex = entry.NextEntryIndex;
			}

			m_Entries[entryIndex].HashCode = -1;
			m_Entries[entryIndex].NextEntryIndex = m_FirstFreeIndex;
			m_Entries[entryIndex].Element = default(T);
			m_FirstFreeIndex = entryIndex;
			m_FreeCount++;
		}

		/// <summary>
		/// Removes the where.
		/// </summary>
		/// <param name="predicate">
		/// The predicate.
		/// </param>
		/// <returns>
		/// </returns>
		public int RemoveWhere(Func<T, bool> predicate)
		{
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}

			if (m_HashToEntryIndex == null)
			{
				return 0;
			}

			int num = 0;
			for (int i = 0; i < m_HashToEntryIndex.Length; i++)
			{
				int index = m_HashToEntryIndex[i];
				int prevIndex = -1;
				while (index >= 0)
				{
					Entry entry = m_Entries[index];
					if (predicate(entry.Element))
					{
						RemoveEntry(index, prevIndex);
						num++;
					}
					else
					{
						prevIndex = index;
					}

					index = entry.NextEntryIndex;
				}
			}

			return num;
		}

		/// <summary>
		/// </summary>
		private void Resize()
		{
			int num = PrimeFinder.NextPrime(m_EntriesUsed * 2);
			var numArray = new int[num];
			for (int i = 0; i < numArray.Length; i++)
			{
				numArray[i] = -1;
			}

			var destinationArray = new Entry[num];
			Array.Copy(m_Entries, 0, destinationArray, 0, m_EntriesUsed);
			for (int j = 0; j < m_EntriesUsed; j++)
			{
				int index = destinationArray[j].HashCode % num;
				destinationArray[j].NextEntryIndex = numArray[index];
				numArray[index] = j;
			}

			m_HashToEntryIndex = numArray;
			m_Entries = destinationArray;
		}

		/// <summary>
		/// Sets the equals.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// </returns>
		public bool SetEquals(HashSet<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if (other == this)
			{
				return true;
			}

			if (!Equals(other.Comparer, Comparer))
			{
				return SetEqualsImpl(other);
			}

			if (Count != other.Count)
			{
				return false;
			}

			return CollectionContainsAll(this, other);
		}

		/// <summary>
		/// Sets the equals.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		/// <returns>
		/// </returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if (other == this)
			{
				return true;
			}

			var set = other as HashSet<T>;
			if ((set == null) || !Equals(set.Comparer, Comparer))
			{
				return SetEqualsImpl(other);
			}

			if (Count != set.Count)
			{
				return false;
			}

			return CollectionContainsAll(this, set);
		}

		/// <summary>
		/// </summary>
		/// <param name="other">
		/// </param>
		/// <returns>
		/// </returns>
		private bool SetEqualsImpl(IEnumerable<T> other)
		{
			var array = new BitArray(m_EntriesUsed, false);
			int num = 0;
			foreach (T local in other)
			{
				int entryIndex = GetEntryIndex(local);
				if (entryIndex < 0)
				{
					return false;
				}

				if (!array[entryIndex])
				{
					array.Set(entryIndex, true);
					num++;
				}
			}

			return num == Count;
		}


		/// <summary>
		/// Convert to type Array{T}.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type Array{T}).
		/// </returns>
		public T[] ToArray()
		{
			var array = new T[Count];
			CopyTo(array);
			return array;
		}

		/// <summary>
		/// Tries the get value.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// </returns>
		public bool TryGetValue(T key, out T value)
		{
			int entryIndex = GetEntryIndex(key);
			if (entryIndex < 0)
			{
				value = default(T);
				return false;
			}

			value = m_Entries[entryIndex].Element;
			return true;
		}

		/// <summary>
		/// Unions the with.
		/// </summary>
		/// <param name="other">
		/// The other.
		/// </param>
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}

			if (other != this)
			{
				foreach (T local in other)
				{
					Add(local);
				}
			}
		}

		#endregion

		#region Nested type: ElementEnumerator

		/// <summary>
		/// The Element Enumerator
		/// </summary>
		[Serializable]
		public struct ElementEnumerator : IEnumerator<T>
		{
			/// <summary>
			/// </summary>
			private readonly HashSet<T> m_MyHashSet;

			/// <summary>
			/// </summary>
			private readonly int m_MyVersion;

			/// <summary>
			/// </summary>
			private T m_MyCurrent;

			/// <summary>
			/// </summary>
			private int m_MyIndex;

			/// <summary>
			/// </summary>
			/// <param name="hashSet">
			/// </param>
			internal ElementEnumerator(HashSet<T> hashSet)
			{
				m_MyHashSet = hashSet;
				m_MyVersion = hashSet.m_Version;
				m_MyIndex = 0;
				m_MyCurrent = default(T);
			}

			#region IEnumerator<T> Members

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			///   </exception>
			public bool MoveNext()
			{
				if (m_MyVersion != m_MyHashSet.m_Version)
				{
					throw new InvalidOperationException("Collection has been modified");
				}

				Entry[] myEntries = m_MyHashSet.m_Entries;
				while (m_MyIndex < m_MyHashSet.m_EntriesUsed)
				{
					if (myEntries[m_MyIndex].HashCode >= 0)
					{
						m_MyCurrent = myEntries[m_MyIndex].Element;
						m_MyIndex++;
						return true;
					}

					m_MyIndex++;
				}

				m_MyIndex = m_MyHashSet.m_EntriesUsed + 1;
				m_MyCurrent = default(T);
				return false;
			}

			/// <summary>
			/// </summary>
			/// <exception cref="InvalidOperationException">
			/// </exception>
			void IEnumerator.Reset()
			{
				if (m_MyVersion != m_MyHashSet.m_Version)
				{
					throw new InvalidOperationException("Collection has been modified");
				}

				m_MyIndex = 0;
				m_MyCurrent = default(T);
			}

			/// <summary>
			/// Gets the current item.
			/// </summary>
			public T Current
			{
				get
				{
					if ((m_MyIndex == 0) || (m_MyIndex == (m_MyHashSet.m_EntriesUsed + 1)))
					{
						throw new InvalidOperationException("Enumeration was not started or already ended");
					}

					return m_MyCurrent;
				}
			}

			/// <summary>
			/// </summary>
			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			#endregion
		}

		#endregion

		#region Nested type: Entry

		/// <summary>
		/// </summary>
		private struct Entry
		{
			/// <summary>
			/// Element of set
			/// </summary>
			public T Element;

			/// <summary>
			/// Cached hash code of the key, -1 means entry is free
			/// </summary>
			public int HashCode;

			/// <summary>
			/// Index of next entry in the chain of keys with the same hashcodes (modulo size), -1 means last
			/// </summary>
			public int NextEntryIndex;
		}

		#endregion
	}

}
