//--------------------------------------------------------------------------
// File:    ReferenceEqualityComparerT.cs
// Content:	Implementation of class ReferenceEqualityComparer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

namespace SmartExpert.Compare
{
	/// <summary>
	/// An equality comparer that uses the object references to define equality.
	/// </summary>
	/// <typeparam name="T">The type of objects that should be compared.</typeparam>
	public class ReferenceEqualityComparer<T> : EqualityComparer<T> where T : class
	{
		private static IEqualityComparer<T> m_DefaultComparer;

		/// <summary>
		/// Determines if the two instances of type <typeparamref name="T"/> are the same instance.
		/// </summary>
		/// <param name="x">The first object.</param>
		/// <param name="y">The second object.</param>
		/// <returns>
		/// <see langword="true"/>, if the two object are the same reference; otherwise <see langword="false"/>.
		/// </returns>
		public override bool Equals(T x, T y)
		{
			return object.ReferenceEquals(x, y);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">Der Typ von <paramref name="obj"/> ist ein Referenztyp, und <paramref name="obj"/> ist null.</exception>
		public override int GetHashCode(T obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		/// <summary>
		/// Gets the default comparer two provide easy access and handling.
		/// </summary>
		public new static IEqualityComparer<T> Default
		{
			get
			{
				return (m_DefaultComparer ?? (m_DefaultComparer = new ReferenceEqualityComparer<T>()));
			}
		}
	}

}
