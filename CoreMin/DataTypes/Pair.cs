//--------------------------------------------------------------------------
// File:    Pair.cs
// Content:	Implementation of structure Pair
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Compare;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	///<summary>Generic Pair data structure</summary>
	/// <typeparam name="T1">Type of the first pair item.</typeparam>
	/// <typeparam name="T2">Type of the second pair item.</typeparam>
	public struct Pair<T1,T2>
	{
		/// <summary>
		/// Empty Pair instance.
		/// </summary>
		public static readonly Pair<T1, T2> Empty;
		/// <summary>
		/// First pair item
		/// </summary>
		public T1 First;
		/// <summary>
		/// Second pair item
		/// </summary>
		public T2 Second;

		/// <summary>
		/// Initializes the <see cref="Pair&lt;T1, T2&gt;"/> struct.
		/// </summary>
		static Pair()
		{
			Empty = new Pair<T1, T2>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair&lt;T1, T2&gt;"/> struct.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		public Pair(T1 first, T2 second)
		{
			First = first;
			Second = second;
			
		}

		/// <summary>
		/// Default pair compare method.
		/// </summary>
		/// <param name="p1">The first pair.</param>
		/// <param name="p2">The second pair.</param>
		/// <returns>The compare result.</returns>
		public static int DefaultComparer(Pair<T1, T2> p1, Pair<T1, T2> p2)
		{
			int num = Comparer<T1>.Default.Compare(p1.First, p2.First);
			return (num != 0) ? num : Comparer<T2>.Default.Compare(p1.Second, p2.Second);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if ((obj is Pair<T1, T2>) == false)
			{
				return false;
			}
			Pair<T1, T2> pair = (Pair<T1, T2>)obj;
			return Equals(First, pair.First) && Equals(Second, pair.Second);
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			// Hint: 29 is a prime number used to get a better hash code result.
			return ((First.IsDefaultValue() == false) ? First.GetHashCode() : 0) + (29 * ((Second.IsDefaultValue() == false) ? Second.GetHashCode() : 0));
		}

		/// <summary>
		/// Determines whether this instance is empty.
		/// </summary>
		/// <returns>
		/// 	<see langword="true"/> if this instance is empty; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsEmpty()
		{
			return Equals(Empty);
		}

		/// <summary>
		/// Create a reverse instance. Pair{T1,T2} -> Pair{T2,T1}
		/// </summary>
		/// <returns>The new created reverse intance.</returns>
		public Pair<T2, T1> Reverse()
		{
			return new Pair<T2, T1>(Second, First);
		}

		/// <summary>
		/// Gets the pair comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public static IComparer<Pair<T1, T2>> Comparer
		{
			get
			{
				return new FuncBasedComparer<Pair<T1, T2>>(DefaultComparer);
			}
		}

	}
}
