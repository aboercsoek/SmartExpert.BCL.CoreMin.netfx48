//--------------------------------------------------------------------------
// File:    PropertyBag.cs
// Content:	Implementation of class PropertyBag
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections
{

	/// <summary>
	/// The property bag class
	/// </summary>
	[Serializable]
	[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(PropertyBagDebugView))]
	public class PropertyBag : IPropertyBag
	{
		#region Private Fields

		readonly Dictionary<string, object> m_PropertyBag = new Dictionary<string, object>();

		#endregion

		#region IPropertyBag Members

		/// <summary>
		/// Gets the items count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get { return m_PropertyBag.Keys.Count; }
		}

		/// <summary>
		/// Gets the property keys.
		/// </summary>
		/// <value>The property keys.</value>
		public string[] Keys
		{
			get { return m_PropertyBag.Keys.ToArray(); }
		}

		/// <summary>
		/// Copies the elements of the property bag to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from the property bag. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.
		/// -or-arrayIndex is equal to or greater than the length of array.
		/// -or-The number of elements in the property bag is greater than the available space from arrayIndex to the end of the destination array.
		/// -or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((IDictionary<string, object>)m_PropertyBag).CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Determines whether a property with the specified <paramref name="key"/> is contained in the property bag.
		/// </summary>
		/// <param name="key">The property key to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the property bag contains a property with the specified <paramref name="key"/>; otherwise <see langword="false"/> is returned.
		/// </returns>
		public bool ContainsProperty( string key )
		{
			return m_PropertyBag.ContainsKey(key);
		}

		///// <summary>
		///// Gets the property value.
		///// </summary>
		///// <param name="key">The property key.</param>
		///// <returns>
		///// Returns the property value.
		///// </returns>
		
		/// <inheritdoc/>
		public object GetPropertyValue( string key )
		{
			return m_PropertyBag[key];
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <typeparam name="T">Required property value type.</typeparam>
		/// <param name="key">The property key.</param>
		/// <returns>
		/// Returns the property value for the specified <paramref name="key"/>.
		/// </returns>
		public T GetPropertyValue<T>(string key)
		{
			return m_PropertyBag[key].As<T>();
		}

		/// <summary>
		/// Gets the property value. Uses 'T:TypeNameOfT' as property key.
		/// </summary>
		/// <typeparam name="T">Required property value type.</typeparam>
		/// <returns>
		/// Returns the property value.
		/// </returns>
		public T GetPropertyValue<T>()
		{
			string key = "T:{0}".SafeFormatWith(typeof(T).GetTypeName());
			return m_PropertyBag[key].As<T>();
		}

		/// <summary>
		/// Tries to get the property value.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns>The property value if the key exists; otherwise <see langword="null"/>.</returns>
		public object TryGetPropertyValue( string key )
		{
			return m_PropertyBag.ContainsKey(key) ? m_PropertyBag[key] : null;
		}

		/// <summary>
		/// Tries to get the property value.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns>The property value if the key exists; otherwise <see langword="null"/>.</returns>
		public T TryGetPropertyValue<T>(string key)
		{
			return m_PropertyBag.ContainsKey(key) ? m_PropertyBag[key].As<T>() : default(T);
		}

		/// <summary>
		/// Tries to get the property value by type.
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <returns>The first property value of type T; or default(T) if no property value of type T was found in the property bag.</returns>
		public T TryGetPropertyValueByType<T>()
		{
			return TryGetPropertyValueByType<T>(true);
		}

		/// <summary>
		/// Tries to get the property value by type.
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <param name="strictTypeMatching">If set to true only exact type matches are returned. If set to false assignable type matches will also be taken into account.</param>
		/// <returns>The first property value of type T; or default(T) if no property value of type T was found in the property bag.</returns>
		public T TryGetPropertyValueByType<T>(bool strictTypeMatching)
		{
			string key = "T:{0}".SafeFormatWith(typeof(T).GetTypeName());
			
			if (ContainsProperty(key))
				return m_PropertyBag[key].As<T>();

			object strictResult = m_PropertyBag.Values.Where(value => value.GetType() == typeof (T)).FirstOrDefault();

			if (strictTypeMatching.IsTrue() || strictResult.IsDefaultValue().IsFalse())
				return strictResult.As<T>();

			return m_PropertyBag.Values.Where(value => typeof(T).IsAssignableFrom(value.GetType())).FirstOrDefault().As<T>();
		}

		/// <summary>
		/// Tries to get the property value.
		/// </summary>
		/// <typeparam name="T">Required property value type.</typeparam>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value.</param>
		/// <returns><see langword="true"/> if the property key exists; otherwise <see langword="false"/>.</returns>
		public bool TryGetPropertyValue<T>( string key, out T value )
		{
			var result = false;

			if (m_PropertyBag.ContainsKey(key))
			{
				if (m_PropertyBag[key] is T)
				{
					value = (T)m_PropertyBag[key];
					result = true;
				}
				else
				{
					value = default(T);
				}
			}
			else
			{
				value = default(T);
			}

			return result;
		}

		

		/// <summary>
		/// Inserts or updates the property.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value.</param>
		public void SetProperty( string key, object value )
		{
			ArgChecker.ShouldNotBeNullOrEmpty(key, "key");

			if ( m_PropertyBag.ContainsKey(key) )
			{
				m_PropertyBag[key] = value;
			}
			else
			{
				m_PropertyBag.Add(key, value);
			}
		}

		/// <summary>
		/// Inserts or updates the property. Uses 'T:TypeNameOfValue' as property key.
		/// </summary>
		/// <param name="value">The property value.</param>
		public void SetProperty(object value)
		{
			string key = "T:{0}".SafeFormatWith(value.GetType().GetTypeName());
			SetProperty(key, value);
		}

		/// <summary>
		/// Removes the property.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns><see langword="true"/> if the property was successfully removed; otherwise <see langword="false"/>.</returns>
		public bool RemoveProperty( string key )
		{
			return m_PropertyBag.Remove(key);
		}

		/// <summary>
		/// Clears the property bag.
		/// </summary>
		public void ClearContext()
		{
			m_PropertyBag.Clear();
		}

		#endregion

		#region IEnumerable<KeyValuePair<string, object>> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return m_PropertyBag.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ( (System.Collections.IEnumerable)m_PropertyBag ).GetEnumerator();
		}

		#endregion

		#region Disposable Pattern

		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="PropertyBag"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock ( this )
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock ( this )
			{
				if ( m_Disposed == false )
				{
					Dispose(true);
				}
			}
		}

		private void Dispose(bool isDispose)
		{
			m_PropertyBag.Clear();
			TypeHelper.DisposeIfNecessary(m_PropertyBag);

			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="PropertyBag"/> is reclaimed by garbage collection.
		/// </summary>
		~PropertyBag()
		{
			Dispose(false);
		}

		
		#endregion

		#region Internal Properties

		internal IDictionary<string, object> PropertyBagDictionary
		{
			get { return m_PropertyBag; }
		}

		#endregion

	}
}
