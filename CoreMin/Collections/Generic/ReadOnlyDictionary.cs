//--------------------------------------------------------------------------
// File:    ReadOnlyDictionary.cs
// Content:	Implementation of class ReadOnlyDictionary
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
using System.Runtime.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections.Generic
{
	/// <summary>
	/// Represents a read only wrapper around a generic IDictionary. The design pattern
	/// mirrors ReadOnlyCollection, and follows the apparent pattern that write operations
	/// do not throw an exception, but simply make no change to the underlying collection.
	/// </summary>
	/// <typeparam name="TKey">Type of the dictionary key.</typeparam>
	/// <typeparam name="TValue">Type of the dictionary value.</typeparam>
	[Serializable, DebuggerTypeProxy(typeof(DictionaryDebugView<,>)), DebuggerDisplay("Count = {Count}")]
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, ISerializable, IDeserializationCallback
	{
		#region Private Members
		
		private IDictionary<TKey, TValue> m_DictionaryToWrap;
		
		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="dictionaryToWrap">The dictionary to wrap.</param>
		public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionaryToWrap)
		{
			m_DictionaryToWrap = dictionaryToWrap ?? new Dictionary<TKey, TValue>();
		}

		#endregion

		#region Public static Methods

		/// <summary>
		/// Returns a read only dictionary.
		/// </summary>
		/// <param name="dictionaryToWrap">The dictionary to wrap.</param>
		/// <returns>The read only dictionary.</returns>
		public static ReadOnlyDictionary<TKey, TValue> AsReadOnly(IDictionary<TKey, TValue> dictionaryToWrap)
		{
			return new ReadOnlyDictionary<TKey, TValue>(dictionaryToWrap);
		}

		#endregion

		#region IDeserializationCallback Members

		/// <summary>
		/// Runs when the entire object graph has been deserialized.
		/// </summary>
		/// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
		public void OnDeserialization(object sender)
		{
			IDeserializationCallback callback = m_DictionaryToWrap as IDeserializationCallback;
			if (callback != null)
				callback.OnDeserialization(sender);
		}

		#endregion

		#region ISerializable Members

		/// <summary>
		/// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"></see>) for this serialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			ISerializable serializable = m_DictionaryToWrap as ISerializable;
			if (serializable != null)
				serializable.GetObjectData(info, context);
		}

		#endregion

		#region IDictionary Members

		/// <summary>
		/// Add does not change a read only Dictionary
		/// </summary>
		/// <param name="key">The <see cref="T:System.Object"></see> to use as the key of the element to add.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to use as the value of the element to add.</param>
		public void Add(object key, object value)
		{
		}

		/// <summary>
		/// Determines whether the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object.</param>
		/// <returns>
		/// true if the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> contains an element with the key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool Contains(object key)
		{
			return ((IDictionary)m_DictionaryToWrap).Contains(key);
		}

		/// <summary>
		/// Returns an <see cref="IDictionaryEnumerator"></see> object for the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object.
		/// </summary>
		/// <returns>
		/// An <see cref="IDictionaryEnumerator"></see> object for the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object.
		/// </returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)m_DictionaryToWrap).GetEnumerator();
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> object has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			get { return true; }
		}

		/// <summary>
		/// Gets an <see cref="ICollection"></see> containing the keys of the <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="ReadOnlyDictionary{TKey,TValue}"></see> containing the keys of the object that implements <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.</returns>
		ICollection IDictionary.Keys
		{
			get { return ((IDictionary)m_DictionaryToWrap).Keys; }
		}

		/// <summary>
		/// Remove does not affect a read only Dictionary
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		public void Remove(object key)
		{
		}

		/// <summary>
		/// Gets an <see cref="ICollection"></see> containing the values in the <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.
		/// </summary>
		/// <value>Collection of all dictionary values.</value>
		/// <returns>An <see cref="ICollection"></see> containing the values in the object that implements <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.</returns>
		ICollection IDictionary.Values
		{
			get { return ((IDictionary)m_DictionaryToWrap).Values; }
		}

		/// <summary>
		/// Gets the <see cref="System.Object"/> with the specified key. Set
		/// does not affect a ReadOnlyDictionary
		/// </summary>
		/// <value>Dictionary value assoziated with the given key.</value>
		public object this[object key]
		{
			get { return ((IDictionary)m_DictionaryToWrap)[key]; }
			set { }
		}

		/// <summary>
		/// Copies the elements of the <see cref="ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
		public void CopyTo(Array array, int index)
		{
			((IDictionary)m_DictionaryToWrap).CopyTo(array, index);
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
		public bool IsSynchronized
		{
			get { return ((IDictionary)m_DictionaryToWrap).IsSynchronized; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public object SyncRoot
		{
			get { return ((IDictionary)m_DictionaryToWrap).SyncRoot; }
		}

		#endregion

		#region IDictionary<TKey,TValue> Members

		/// <summary>
		/// Add does not change a read only Dictionary
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		public void Add(TKey key, TValue value)
		{
		}

		/// <summary>
		/// Determines whether the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.</param>
		/// <returns>
		/// true if the <see cref="ReadOnlyDictionary{TKey,TValue}"></see> contains an element with the key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">key is null.</exception>
		public bool ContainsKey(TKey key)
		{
			return m_DictionaryToWrap.ContainsKey(key);
		}

		/// <summary>
		/// Gets a read only <see cref="ICollection{TKey}"></see> containing the keys of the <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="ICollection{TKey}"></see> containing the keys of the object that implements <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.</returns>
		public ICollection<TKey> Keys
		{
			get { return ReadOnlyCollection<TKey>.AsReadOnly(m_DictionaryToWrap.Keys); }
		}

		/// <summary>
		/// Remove does not change a read only Dictionary
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// Always false (read only dictionary).
		/// </returns>
		public bool Remove(TKey key)
		{
			return false;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; 
		/// otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
		/// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return m_DictionaryToWrap.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets an <see cref="ICollection{TValue}"></see> containing the values in the <see cref="ReadOnlyDictionary{TKey,TValue}"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="ICollection{T}"/> object containing the values in the dictionary.</returns>
		public ICollection<TValue> Values
		{
			get { return ReadOnlyCollection<TValue>.AsReadOnly(m_DictionaryToWrap.Values); }
		}

		/// <summary>
		/// Gets the value with the specified key. Set
		/// does not change a read only Dictionary
		/// </summary>
		/// <value>The element with the specified key.</value>
		public TValue this[TKey key]
		{
			get { return m_DictionaryToWrap[key]; }
			set { }
		}

		/// <summary>
		/// Add does not change a read only dictionary
		/// </summary>
		/// <param name="item">The object to add to the dictionary.</param>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
		}

		/// <summary>
		/// Clear does not change a read only Dictionary.
		/// </summary>
		public void Clear()
		{
		}

		/// <summary>
		/// Determines whether the dictionary contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the dictionary.</param>
		/// <returns>
		/// true if item is found in the dictionary; otherwise, false.
		/// </returns>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return m_DictionaryToWrap.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the dictionary to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from the dictionary. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.
		/// -or-arrayIndex is equal to or greater than the length of array.
		/// -or-The number of elements in the source dictionary is greater than the available space from arrayIndex to the end of the destination array.
		/// -or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			m_DictionaryToWrap.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the dictionary.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the dictionary.</returns>
		public int Count
		{
			get { return m_DictionaryToWrap.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether the dictionary is read-only.
		/// </summary>
		/// <value>Returns true.</value>
		/// <returns>Returns true.</returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		/// Remove does not change a read only Dictionary
		/// </summary>
		/// <param name="item">The object to remove from the dictionary.</param>
		/// <returns>
		/// Always returns false.
		/// </returns>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return false;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="IEnumerator{T}"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return m_DictionaryToWrap.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary)m_DictionaryToWrap).GetEnumerator();
		}

		#endregion

	}
}