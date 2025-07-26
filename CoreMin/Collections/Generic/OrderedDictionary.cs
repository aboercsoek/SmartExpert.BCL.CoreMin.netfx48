//--------------------------------------------------------------------------
// File:    OrderedDictionary.cs
// Content:	Implementation of class OrderedDictionary
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;
using SmartExpert.Serialization;

#endregion

namespace SmartExpert.Collections.Generic
{
	/// <summary>
	/// An ordered Dictionary implementation
	/// </summary>
	/// <typeparam name="TKey">
	/// Dictionary key type.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// Dictionary value type.
	/// </typeparam>
	[XmlRoot(ElementName = "orderedDictionary", DataType = "SmartExpert.BCL35.Core.Collection.OrderedDictionary", Namespace = "http://www.smartexpert.de/smartlibrary/data/collections/2009")]
	public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IXmlSerializable
	{
		/// <summary>
		/// </summary>
		private Dictionary<TKey, TValue> m_Dictionary;

		/// <summary>
		/// </summary>
		private List<TKey> m_Keys;

		/// <summary>
		/// </summary>
		private List<TValue> m_Values;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}"/> class.
		/// </summary>
		public OrderedDictionary()
			: this(0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderedDictionary{TKey, TValue}"/> class.
		/// </summary>
		/// <param name="capacity">
		/// The capacity.
		/// </param>
		public OrderedDictionary( int capacity )
		{
			this.m_Dictionary = new Dictionary<TKey, TValue>(capacity);
			this.m_Keys = new List<TKey>(capacity);
			this.m_Values = new List<TValue>(capacity);
		}

		#region IDictionary<TKey,TValue> Members

		/// <summary>
		/// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <param name="key">
		/// The object to use as the key of the element to add.
		/// </param>
		/// <param name="value">
		/// The object to use as the value of the element to add.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="key"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
		/// </exception>
		public void Add( TKey key, TValue value )
		{
			this.m_Dictionary.Add(key, value);
			this.m_Keys.Add(key);
			this.m_Values.Add(value);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </exception>
		public void Clear()
		{
			this.m_Dictionary.Clear();
			this.m_Keys.Clear();
			this.m_Values.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">
		/// The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </param>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="key"/> is null.
		/// </exception>
		public bool ContainsKey( TKey key )
		{
			return this.m_Dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			IEnumerable<KeyValuePair<TKey, TValue>> result = m_Keys.Select((k, r) => new KeyValuePair<TKey, TValue>(k, m_Dictionary[k]));
			return result.GetEnumerator();
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <param name="key">
		/// The key of the element to remove.
		/// </param>
		/// <returns>
		/// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="key"/> is null.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.
		/// </exception>
		public bool Remove( TKey key )
		{
			this.RemoveFromLists(key);
			return this.m_Dictionary.Remove(key);
		}

		/// <summary>
		/// </summary>
		/// <param name="item">
		/// </param>
		void ICollection<KeyValuePair<TKey, TValue>>.Add( KeyValuePair<TKey, TValue> item )
		{
			this.Add(item.Key, item.Value);
		}

		/// <summary>
		/// </summary>
		/// <param name="item">
		/// </param>
		/// <returns>
		/// </returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains( KeyValuePair<TKey, TValue> item )
		{
			return this.m_Dictionary.Contains(item);
		}

		/// <summary>
		/// </summary>
		/// <param name="array">
		/// </param>
		/// <param name="arrayIndex">
		/// </param>
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
		{
			((ICollection<KeyValuePair<TKey, TValue>>)this.m_Dictionary).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// </summary>
		/// <param name="item">
		/// </param>
		/// <returns>
		/// </returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove( KeyValuePair<TKey, TValue> item )
		{
			bool flag = ((ICollection<KeyValuePair<TKey, TValue>>)this.m_Dictionary).Remove(item);
			if ( flag )
			{
				this.RemoveFromLists(item.Key);
			}

			return flag;
		}

		/// <summary>
		/// </summary>
		/// <returns>
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">
		/// The key whose value to get.
		/// </param>
		/// <param name="value">
		/// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		/// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="key"/> is null.
		/// </exception>
		public bool TryGetValue( TKey key, out TValue value )
		{
			return this.m_Dictionary.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets the number of elements contained in the dictionary.
		/// </summary>
		/// <value>Number of dictionary elements.</value>
		/// <returns>
		/// The number of elements contained in the dictionary.
		/// </returns>
		public int Count
		{
			get
			{
				return this.m_Dictionary.Count;
			}
		}

		/// <summary>
		/// Gets or sets the value for the specified key.
		/// </summary>
		/// <value>Value assoziated with the given key.</value>
		public TValue this[TKey key]
		{
			get
			{
				return this.m_Dictionary[key];
			}

			set
			{
				this.RemoveFromLists(key);
				this.m_Dictionary[key] = value;
				this.m_Keys.Add(key);
				this.m_Values.Add(value);
			}
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection{T}"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value>Collection of all dictionary keys.</value>
		/// <returns>
		/// An <see cref="T:System.Collections.Generic.ICollection{T}"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </returns>
		public ICollection<TKey> Keys
		{
			get
			{
				return this.m_Keys.AsReadOnly();
			}
		}

		/// <summary>
		/// </summary>
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_Dictionary).IsReadOnly;
			}
		}

		/// <summary>
		/// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </summary>
		/// <value>Collection of all dictionary values.</value>
		/// <returns>
		/// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
		/// </returns>
		public ICollection<TValue> Values
		{
			get
			{
				return this.m_Values.AsReadOnly();
			}
		}

		#endregion

		#region IXmlSerializable Members

		/// <summary>
		/// This method is reserved and is not used.
		/// </summary>
		/// <returns>
		/// null
		/// </returns>
		public XmlSchema GetSchema()
		{
			/*
			This method is reserved and should not be used. When implementing the IXmlSerializable interface, 
			you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a 
			custom schema is required, apply the System.Xml.Serialization.XmlSchemaProviderAttribute to the class.
			*/
			return null;
		}

		/// <summary>
		/// Generates an OrderedDictionary object from its XML representation.
		/// </summary>
		/// <param name="reader">
		/// The System.Xml.XmlReader stream from which the object is deserialized.
		/// </param>
		public void ReadXml( XmlReader reader )
		{
			DictionaryXmlSerializerHelper.Deserialize<TKey, TValue>(reader, this);
		}

		/// <summary>
		/// Converts an OrderedDictionary object into its XML representation.
		/// </summary>
		/// <param name="writer">
		/// The System.Xml.XmlWriter stream to which the object is serialized.
		/// </param>
		public void WriteXml( XmlWriter writer )
		{
			DictionaryXmlSerializerHelper.Serialize<TKey, TValue>(writer, this);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines whether the specified value contains value.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the specified value contains value; otherwise, <see langword="false"/>.
		/// </returns>
		public bool ContainsValue( TValue value )
		{
			return this.m_Dictionary.ContainsValue(value);
		}

		#endregion

		/// <summary>
		/// Removes a item from the lists.
		/// </summary>
		/// <param name="key">
		/// The key.
		/// </param>
		private void RemoveFromLists( TKey key )
		{
			int index = this.m_Keys.IndexOf(key);
			if (index == -1) return;
			this.m_Keys.RemoveAt(index);
			this.m_Values.RemoveAt(index);
		}
	}
}
