﻿//--------------------------------------------------------------------------
// File:    IndexValuePair.cs
// Content:	Implementation of struct IndexValuePair
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.InteropServices;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	///<summary>Value with Index Data Structure</summary>
	/// <typeparam name="T">The value type.</typeparam>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct IndexValuePair<T>
	{
		/// <summary>The value index.</summary>
		public readonly int Index;
		/// <summary>The value.</summary>
		public readonly T Value;

		/// <summary>
		/// Initializes a new instance of the <see cref="IndexValuePair{T}"/> struct.
		/// </summary>
		/// <param name="item">The item value.</param>
		/// <param name="index">The item index.</param>
		public IndexValuePair( T item, int index )
		{
			Value = item;
			Index = index;
		}
	}
}
