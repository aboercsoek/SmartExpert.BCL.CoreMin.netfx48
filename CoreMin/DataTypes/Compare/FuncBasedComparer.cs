//--------------------------------------------------------------------------
// File:    Comparer.cs
// Content:	Implementation of class Comparer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Compare
{
	///<summary>Func delegate based Comparer</summary>
	/// <typeparam name="T">Type of the comparer.</typeparam>
	public sealed class FuncBasedComparer<T> : IComparer<T>, IComparer
	{
		/// <summary>
		/// Default Equality Comparer (<see cref="System.Collections.Generic.Comparer{T}.Default"/>)
		/// </summary>
		public static readonly IComparer<T> Default;
		private readonly Func<T, T, int> m_FuncComparer;

		/// <summary>
		/// Initializes the <see cref="FuncBasedComparer{T}"/> class.
		/// </summary>
		static FuncBasedComparer()
		{
			Default = Comparer<T>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FuncBasedComparer{T}"/> class.
		/// </summary>
		/// <param name="funcComparer">The comparer function delegate.</param>
		public FuncBasedComparer(Func<T, T, int> funcComparer)
		{
			m_FuncComparer = funcComparer;
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// <para>Value Condition Less than zero <paramref name="x"/> is less than <paramref name="y"/>.</para>
		/// <para>Zero <paramref name="x"/> equals <paramref name="y"/>.</para>
		/// <para>Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>.</para>
		/// </returns>
		int IComparer<T>.Compare(T x, T y)
		{
			return m_FuncComparer == null ? Default.Compare(x, y) : m_FuncComparer(x, y);
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// <para>Value Condition Less than zero <paramref name="x"/> is less than <paramref name="y"/>.</para>
		/// <para>Zero <paramref name="x"/> equals <paramref name="y"/>.</para>
		/// <para>Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>.</para>
		/// </returns>
		int IComparer.Compare(object x, object y)
		{
			return m_FuncComparer == null ? Default.Compare((T)x, (T)y) : m_FuncComparer((T)x, (T)y);
		}

		/// <summary>
		/// Static Factory Method
		/// </summary>
		/// <param name="funcComparer">The comparer function delegate.</param>
		/// <returns>The comparer instance.</returns>
		public static FuncBasedComparer<T> Create(Func<T, T, int> funcComparer)
		{
			return new FuncBasedComparer<T>(funcComparer);
		}
	}
}
