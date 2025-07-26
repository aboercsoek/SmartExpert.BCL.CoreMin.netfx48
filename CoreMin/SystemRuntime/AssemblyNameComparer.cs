//--------------------------------------------------------------------------
// File:    AssemblyNameComparer.cs
// Content:	Implementation of class AssemblyNameComparer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace SmartExpert.SystemRuntime
{
	///<summary>Comparer for AssemblyName objects.</summary>
	public sealed class AssemblyNameComparer : IComparer<AssemblyName>, IEqualityComparer<AssemblyName>
	{
		/// <summary>
		/// Compares two AssemblyNames and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first AssemblyName to compare.</param>
		/// <param name="y">The second AssemblyName to compare.</param>
		/// <returns>
		/// Fullname compare result.
		/// </returns>
		public int Compare(AssemblyName x, AssemblyName y)
		{
			if ((x == null) && (y == null))
			{
				return 0;
			}
			if (x == null)
			{
				return 1;
			}
			if (y == null)
			{
				return -1;
			}
			return String.CompareOrdinal(x.FullName, y.FullName);
		}

		/// <summary>
		/// Determines whether the specified AssemblyNames are equal.
		/// </summary>
		/// <param name="x">The first AssemblyName to compare.</param>
		/// <param name="y">The second AssemblyName to compare.</param>
		/// <returns>
		/// true if the specified AssemblyName are equal; otherwise, false.
		/// </returns>
		public bool Equals(AssemblyName x, AssemblyName y)
		{
			return (((x == null) && (y == null)) || (((x != null) && (y != null)) && (x.FullName == y.FullName)));
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">The AssemblyName object.</param>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public int GetHashCode(AssemblyName obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.FullName.GetHashCode();
		}
	}


}
