//--------------------------------------------------------------------------
// File:    EmptyDictionary.cs
// Content:	Implementation of class EmptyDictionary
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace SmartExpert.Linq
{
	/// <summary>
	/// Reuses the single instance of an empty dictionary (one per type).
	/// </summary>
	/// <typeparam name="TKey">The dictionary key item type.</typeparam>
	/// /// <typeparam name="TValue">The dictionary value item type.</typeparam>
	public static class EmptyDictionary<TKey,TValue>
	{
		/// <summary>
		/// The empty collection instance.
		/// </summary>
		public static readonly IDictionary<TKey,TValue> Instance;

		/// <summary>
		/// Initializes the <see cref="EmptyDictionary{TKey,TValue}"/> class.
		/// </summary>
		static EmptyDictionary()
		{
			Instance = new Dictionary<TKey,TValue>();
		}
	}
}
