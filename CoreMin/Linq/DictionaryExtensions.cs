//--------------------------------------------------------------------------
// File:    DictionaryExtensions.cs
// Content:	Implementation of class DictionaryExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as Dictionary<T,U> therewith the extension methods are available by using the System.Collections.Generic namespace.
namespace SmartExpert.Linq
{
	///<summary>Represents extension methods for <see cref="Dictionary{TKey,TValue}"/> type.</summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Gets the value associated with the specified key or a default value if the key does not exist.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
		/// <param name="dic">The source dictionary.</param>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="defaultValue">The default value that should be returned, if the dictionary does not contains a element with the specified key.</param>
		/// <returns>Returns the value associated with the specified key, if the key is found; otherwise, the specified <paramref name="defaultValue"/>.</returns>
		/// <exception cref="ArgNullException"><paramref name="dic"/> is <see langword="null"/>.</exception>
		public static TValue GetValue<TKey, TValue>( this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue )
		{
			ArgChecker.ShouldNotBeNull(dic, "dic");

			TValue theItem;
			bool contains = dic.TryGetValue(key, out theItem);
			if ( !contains )
			{
				return defaultValue;
			}

			return theItem;
		}


		/// <summary>
		/// Gets or create a dictionary item value.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="dic">The dictionary.</param>
		/// <param name="key">The key.</param>
		/// <param name="valueCreator">The value creator delegate.</param>
		/// <returns>The value.</returns>
		public static TValue GetOrCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueCreator)
		{
			TValue local;
			if (!dic.TryGetValue(key, out local))
			{
				local = valueCreator(key);
				dic.Add(key, local);
			}
			return local;
		}


	}
}
