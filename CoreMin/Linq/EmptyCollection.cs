//--------------------------------------------------------------------------
// File:    EmptyCollection.cs
// Content:	Implementation of class EmptyCollection
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
	/// Reuses the single instance of an empty collection (one per type).
	/// </summary>
	/// <typeparam name="T">The collection item type.</typeparam>
	public static class EmptyCollection<T>
	{
		/// <summary>
		/// The empty collection instance.
		/// </summary>
		public static readonly ICollection<T> Instance;

		/// <summary>
		/// Initializes the <see cref="EmptyCollection{T}"/> class.
		/// </summary>
		static EmptyCollection()
		{
			Instance = new List<T>();
		}
	}
}
