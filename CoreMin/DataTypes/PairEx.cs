//--------------------------------------------------------------------------
// File:    PairEx.cs
// Content:	Implementation of class PairEx
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

namespace SmartExpert
{

	/// <summary>
	/// Pair Extension Methods.
	/// </summary>
	public static class PairEx
	{
		/// <summary>
		/// Gets the first pair item values.
		/// </summary>
		/// <typeparam name="T1">The type of the first pair item.</typeparam>
		/// <typeparam name="T2">The type of the second pair item.</typeparam>
		/// <param name="pairs">The pair collection.</param>
		/// <returns>
		/// Returns the first pair item values.
		/// </returns>
		public static ICollection<T1> GetFirstPairItems<T1, T2>(this ICollection<Pair<T1, T2>> pairs)
		{
			return new List<T1>(((IEnumerable<Pair<T1, T2>>)pairs).GetFirstPairItems());
		}

		/// <summary>
		/// Gets the first pair item values.
		/// </summary>
		/// <typeparam name="T1">The type of the first pair item.</typeparam>
		/// <typeparam name="T2">The type of the second pair item.</typeparam>
		/// <param name="pairs">The pair sequence.</param>
		/// <returns>
		/// Returns the first pair item values.
		/// </returns>
		public static IEnumerable<T1> GetFirstPairItems<T1, T2>(this IEnumerable<Pair<T1, T2>> pairs)
		{
			return pairs.Select(pair => pair.First);
		}

		/// <summary>
		/// Gets the second pair item values.
		/// </summary>
		/// <typeparam name="T1">The type of the first pair item.</typeparam>
		/// <typeparam name="T2">The type of the second pair item.</typeparam>
		/// <param name="pairs">The pair collection.</param>
		/// <returns>
		/// Returns the second pair item values.
		/// </returns>
		public static ICollection<T2> GetSecondPairItems<T1, T2>(this ICollection<Pair<T1, T2>> pairs)
		{
			return new List<T2>(((IEnumerable<Pair<T1, T2>>)pairs).GetSecondPairItems());
		}

		/// <summary>
		/// Gets the second pair item values.
		/// </summary>
		/// <typeparam name="T1">The type of the first pair item.</typeparam>
		/// <typeparam name="T2">The type of the second pair item.</typeparam>
		/// <param name="pairs">The pair sequence.</param>
		/// <returns>
		/// Returns the second pair item values.
		/// </returns>
		public static IEnumerable<T2> GetSecondPairItems<T1, T2>(this IEnumerable<Pair<T1, T2>> pairs)
		{
			return pairs.Select(pair => pair.Second);
		}

		/// <summary>
		/// Creates the pair.
		/// </summary>
		/// <typeparam name="T1">The type of the first pair item.</typeparam>
		/// <typeparam name="T2">The type of the second pair item.</typeparam>
		/// <param name="first">The first pair item.</param>
		/// <param name="second">The second pair item.</param>
		/// <returns>The new created pair instance</returns>
		public static Pair<T1, T2> CreatePair<T1, T2>(T1 first, T2 second)
		{
			return new Pair<T1, T2>(first, second);
		}
	}
}
