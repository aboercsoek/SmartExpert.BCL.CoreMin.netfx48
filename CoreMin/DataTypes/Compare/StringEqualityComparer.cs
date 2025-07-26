//--------------------------------------------------------------------------
// File:    StringEqualityComparer.cs
// Content:	Implementation of class StringEqualityComparer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace SmartExpert.Compare
{
	/// <summary>
	/// A String equality comparer.
	/// </summary>
	[Obsolete("StringEqualityComparer is obsolete. Use System.StringComparer instead!")]
	public sealed class StringEqualityComparer : IEqualityComparer<string>
	{
		private readonly StringComparison m_ComparisonType;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="StringEqualityComparer"/> class.
		/// </summary>
		public StringEqualityComparer() : this(StringComparison.CurrentCulture)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringEqualityComparer"/> class.
		/// </summary>
		/// <param name="comparisonType">The string comparison type.</param>
		public StringEqualityComparer(StringComparison comparisonType)
		{
			m_ComparisonType = comparisonType;
		}

		/// <summary>
		/// Equalses the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		bool IEqualityComparer<string>.Equals(string x, string y)
		{
			return string.Compare(x, y, m_ComparisonType) == 0;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">The string to get the hashcode from.</param>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		int IEqualityComparer<string>.GetHashCode(string obj)
		{
			return (string.IsNullOrEmpty(obj)) ? 0 : obj.GetHashCode();
		}


		/// <summary>
		/// Static Factory Method for StringEqualityComparer instances.
		/// </summary>
		/// <param name="comparisonType">The string comparison type</param>
		/// <returns>The equality comparer instance.</returns>
		public static StringEqualityComparer Create(StringComparison comparisonType)
		{
			return new StringEqualityComparer(comparisonType);
		}
	}


}
