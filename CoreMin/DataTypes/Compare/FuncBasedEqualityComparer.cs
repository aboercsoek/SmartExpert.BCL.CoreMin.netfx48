//--------------------------------------------------------------------------
// File:    FuncBasedEqualityComparer.cs
// Content:	Implementation of class FuncBasedEqualityComparer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Compare
{
	/// <summary>
	/// Func delegate based equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of the equality comparer.</typeparam>
	public sealed class FuncBasedEqualityComparer<T> : IEqualityComparer<T>
	{
		/// <summary>
		/// Default Equality Comparer (<see cref="System.Collections.Generic.EqualityComparer{T}.Default"/>)
		/// </summary>
		public static readonly IEqualityComparer<T> Default;
	
		private readonly Func<T, T, bool> m_FuncCompare;
	
		private readonly Func<T, int> m_FuncGetHashCode;

		/// <summary>
		/// Initializes the <see cref="FuncBasedEqualityComparer{T}"/> class.
		/// </summary>
		static FuncBasedEqualityComparer()
		{
			Default = EqualityComparer<T>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FuncBasedEqualityComparer{T}"/> class.
		/// </summary>
		/// <param name="funcCompare">The compare function delegate.</param>
		/// <param name="funcGetHashCode">The get hash code function delegate.</param>
		public FuncBasedEqualityComparer(Func<T, T, bool> funcCompare, Func<T, int> funcGetHashCode)
		{
			m_FuncCompare = funcCompare;
			m_FuncGetHashCode = funcGetHashCode;
		}

		/// <summary>
		/// Equalses the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		bool IEqualityComparer<T>.Equals(T x, T y)
		{
			return m_FuncCompare == null ? Default.Equals(x, y) : m_FuncCompare(x, y);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		int IEqualityComparer<T>.GetHashCode(T obj)
		{
			return m_FuncGetHashCode == null ? Default.GetHashCode(obj) : m_FuncGetHashCode(obj);
		}


		/// <summary>
		/// Gets the compare function delegate.
		/// </summary>
		/// <value>The compare function delegate.</value>
		public Func<T, T, bool> FuncCompare
		{
			get
			{
				return m_FuncCompare;
			}
		}


		/// <summary>
		/// Gets the get hash code function delegate.
		/// </summary>
		/// <value>The get hash code function delegate.</value>
		public Func<T, int> FuncGetHashCode
		{
			get
			{
				return m_FuncGetHashCode;
			}
		}


		/// <summary>
		/// Static Factory Method
		/// </summary>
		/// <param name="funcCompare">The compare function delegate.</param>
		/// <param name="funcGetHashCode">The get hash code function delegate.</param>
		/// <returns>The equality comparer instance.</returns>
		public static FuncBasedEqualityComparer<T> Create(Func<T, T, bool> funcCompare, Func<T, int> funcGetHashCode)
		{
			return new FuncBasedEqualityComparer<T>(funcCompare, funcGetHashCode);
		}
	}


}
