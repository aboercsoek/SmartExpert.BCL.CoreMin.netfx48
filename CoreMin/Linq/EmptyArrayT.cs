//--------------------------------------------------------------------------
// File:    EmptyArrayT.cs
// Content:	Implementation of class EmptyArrayT
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Linq
{
	/// <summary>
	/// Reuses the single instance of an empty array (one per type).
	/// </summary>
	/// <typeparam name="T">The array item type.</typeparam>
	public static class EmptyArray<T>
	{
		/// <summary>
		/// The empty array instance.
		/// </summary>
		public static readonly T[] Instance;

		/// <summary>
		/// Initializes the <see cref="EmptyArray{T}"/> class.
		/// </summary>
		static EmptyArray()
		{
			Instance = new T[0];
		}
	}
}
