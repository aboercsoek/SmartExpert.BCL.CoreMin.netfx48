//--------------------------------------------------------------------------
// File:    IPropertyBag.cs
// Content:	Definition of interface IPropertyBag
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
	/// The property bag interface.
	/// </summary>
	public interface IPropertyBag : IEnumerable<KeyValuePair<string, object>>, IDisposable
	{
		#region PropertyBag Properties

		/// <summary>
		/// Gets the items count.
		/// </summary>
		/// <value>The count.</value>
		int Count { get; }

		/// <summary>
		/// Gets the property keys.
		/// </summary>
		/// <value>The property keys.</value>
		string[] Keys { get; }

		#endregion

		#region GetPropertyValue Methods

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns>
		/// Returns the property value.
		/// </returns>
		object GetPropertyValue( string key );

		/// <summary>
		/// Gets the property value for the specified property key.
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <param name="key">The property key.</param>
		/// <returns>
		/// Returns the property value.
		/// </returns>
		T GetPropertyValue<T>(string key);

		/// <summary>
		/// Gets the property value. <note>Uses 'T:TypeNameOfT' as property key.</note>
		/// </summary>
		/// <typeparam name="T">Required property value type.</typeparam>
		/// <returns>
		/// Returns the property value.
		/// </returns>
		T GetPropertyValue<T>();

		#endregion

		#region TryGetPropertyValue Methods

		/// <summary>
		/// Tries to get the property value for the specified property <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns>The property value if the <paramref name="key"/> exists; otherwise <see langword="null"/> is returned.</returns>
		object TryGetPropertyValue( string key );

		/// <summary>
		/// Tries to get the property value for the specified property <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <param name="key">The property key.</param>
		/// <returns>The property value if the <paramref name="key"/> exists; otherwise <see langword="null"/> is returned.</returns>
		T TryGetPropertyValue<T>(string key);

		/// <summary>
		/// Tries to get the property value by type (uses strict type matching).
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <returns>The first property value of type <typeparamref name="T"/>; or <c>default(T)</c> if no property value of type <typeparamref name="T"/> was found in the property bag.</returns>
		T TryGetPropertyValueByType<T>();

		/// <summary>
		/// Tries to get the property value by type.
		/// </summary>
		/// <typeparam name="T">The property value type</typeparam>
		/// <param name="strictTypeMatching">If set to true only exact type matches are returned. If set to false assignable type matches will also be taken into account.</param>
		/// <returns>The first property value of type <typeparamref name="T"/>; or <c>default(T)</c> if no property value of type <typeparamref name="T"/> was found in the property bag.</returns>
		T TryGetPropertyValueByType<T>(bool strictTypeMatching);

		/// <summary>
		/// Tries to get the property value for the specified property <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T">The property value type.</typeparam>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value result.</param>
		/// <returns><see langword="true"/> if a property with the specified <paramref name="key"/> exists; otherwise <see langword="false"/>.</returns>
		bool TryGetPropertyValue<T>( string key, out T value );

		#endregion

		#region PropertyBag Item Methods

		/// <summary>
		/// Determines whether the specified property key exists.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified property key exists; otherwise, <see langword="false"/>.
		/// </returns>
		bool ContainsProperty(string key);

		/// <summary>
		/// Inserts or updates the property.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <param name="value">The property value.</param>
		void SetProperty( string key, object value );

		/// <summary>
		/// Inserts or updates the property. <note>Uses 'T:TypeNameOfValue' as property key.</note>
		/// </summary>
		/// <param name="value">The property value.</param>
		void SetProperty(object value);

		/// <summary>
		/// Removes the property from the property bag.
		/// </summary>
		/// <param name="key">The property key.</param>
		/// <returns><see langword="true"/> if the property was successfully removed; otherwise <see langword="false"/>.</returns>
		bool RemoveProperty( string key );

		#endregion

		#region PropertyBag Operations

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
		void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex);

		/// <summary>
		/// Clears the property bag.
		/// </summary>
		void ClearContext();

		#endregion
	}
}
