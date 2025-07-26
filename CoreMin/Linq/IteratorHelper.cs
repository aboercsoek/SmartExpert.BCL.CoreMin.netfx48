//--------------------------------------------------------------------------
// File:    IteratorHelper.cs
// Content:	Implementation of class IteratorHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SmartExpert.Linq
{
	/// <summary>
	/// A container for static helper methods that are used for manipulating and computing iterators.
	/// </summary>
	public static class IteratorHelper
	{
		/// <summary>
		/// Compares two enumerations of elements for equality by calling the Equals method on each pair of elements.
		/// The enumerations must be of equal length, or must both be null, in order to be considered equal.
		/// </summary>
		/// <typeparam name="T">The element type of the collection</typeparam>
		/// <param name="left">An enumeration of elements. The enumeration may be null, but the elements may not.</param>
		/// <param name="right">An enumeration of elements. The enumeration may be null, but the elements may not.</param>
		/// <returns>True if the enumerables are equal; otherwise false.</returns>
		public static bool EnumerablesAreEqual<T>(IEnumerable<T> left, IEnumerable<T> right)
		{
			if ( left == null ) 
				return (right == null || !right.GetEnumerator().MoveNext());

			IEnumerator<T> leftEnum = left.GetEnumerator();
			
			if ( right == null )
				return !leftEnum.MoveNext();
			
			IEnumerator<T> rightEnum = right.GetEnumerator();

			while ( leftEnum.MoveNext() )
			{
				if ( !rightEnum.MoveNext() ) 
					return false;

				if ( !leftEnum.Current.Equals(rightEnum.Current) ) 
					return false;
			}
			return !rightEnum.MoveNext();
		}

		/// <summary>
		/// Compares two enumerations of elements for equality by calling the Equals method on each pair of elements.
		/// The enumerations must be of equal length, or must both be null, in order to be considered equal.
		/// </summary>
		/// <typeparam name="T">The element type of the collection</typeparam>
		/// <param name="left">An enumeration of elements. The enumeration may be null, but the elements may not.</param>
		/// <param name="right">An enumeration of elements. The enumeration may be null, but the elements may not.</param>
		/// <param name="comparer">An object that compares two enumeration elements for equality.</param>
		/// <returns>True if the enumerables are equal; otherwise false.</returns>
		public static bool EnumerablesAreEqual<T>( IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer )
		{
			if ( left == null ) 
				return right == null || !right.GetEnumerator().MoveNext();
			
			IEnumerator<T> leftEnum = left.GetEnumerator();
			
			if ( right == null ) 
				return !leftEnum.MoveNext();
			
			IEnumerator<T> rightEnum = right.GetEnumerator();
			
			while ( leftEnum.MoveNext() )
			{
				if ( !rightEnum.MoveNext() ) 
					return false;
				
				if ( !comparer.Equals(leftEnum.Current, rightEnum.Current) ) 
					return false;
			}
			
			return !rightEnum.MoveNext();
		}

		/// <summary>
		/// Returns an enumerable containing no objects.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <returns>Returns an enumerable containing no objects.</returns>
		public static IEnumerable<T> GetEmptyEnumerable<T>()
		{
			yield break;
		}

		/// <summary>
		/// Returns an enumerable containing a single object.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="t">The item of the single item enumerable.</param>
		/// <returns>Returns an enumerable containing a single object.</returns>
		public static IEnumerable<T> GetSingleItemEnumerable<T>( T t )
		{
			yield return t;
		}

		/// <summary>
		/// Returns an <see cref="IEnumerable{T}"/> collection that acts like cast on <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="TSource">Source collection item type.</typeparam>
		/// <typeparam name="TTarget">Target collection item type.</typeparam>
		/// <param name="source">The source collection</param>
		/// <returns>An <see cref="IEnumerable{T}"/> collection of type <typeparamref name="TTarget"/>.</returns>
		public static IEnumerable<TTarget> GetConversionEnumerable<TSource, TTarget>( IEnumerable<TSource> source ) where TSource : TTarget
		{
			return source.Select(s => (TTarget) s);
		}

		/// <summary>
		/// Returns an <see cref="IEnumerable{T}"/> collection that acts like cast on <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="TSource">Source collection item type.</typeparam>
		/// <typeparam name="TTarget">Target collection item type.</typeparam>
		/// <param name="source">The source collection</param>
		/// <returns>An <see cref="IEnumerable{T}"/> collection of type <typeparamref name="TTarget"/>.</returns>
		public static IEnumerable<TTarget> GetFilterEnumerable<TSource, TTarget>( IEnumerable<TSource> source )
			where TSource : class
			where TTarget : class
		{
			return source.OfType<TTarget>();
		}

		/// <summary>
		/// True if the given enumerable is not null and contains at least one element.
		/// </summary>
		/// <typeparam name="T">The collection item type.</typeparam>
		/// <param name="enumerable">The collection</param>
		/// <returns><see langword="true"/> if the enumerable object is not empty; otherwise <see langword="false"/>.</returns>
		public static bool EnumerableIsNotEmpty<T>(IEnumerable<T> enumerable)
		{
			return enumerable != null && enumerable.GetEnumerator().MoveNext();
		}

		/// <summary>
		/// True if the given enumerable is null or contains no elements
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The collection</param>
		/// <returns><see langword="true"/> if the enumerable object is empty; otherwise <see langword="false"/>.</returns>
		public static bool EnumerableIsEmpty<T>(IEnumerable<T> enumerable)
		{
			return !EnumerableIsNotEmpty(enumerable);
		}

		/// <summary>
		/// True if the given enumerable is not null and contains the given element.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The collection</param>
		/// <param name="element">The element to search for.</param>
		/// <returns><see langword="true"/> if the enumerable object contains the element; otherwise <see langword="false"/>.</returns>
		public static bool EnumerableContains<T>(IEnumerable<T> enumerable, T element)
		  where T : class
		// ensures enumerable == null ==> result == false;
		// ensures enumerable != null ==> result == exists{T t in enumerable; t == element};
		{
			return enumerable != null && enumerable.Any(elem => ReferenceEquals(elem, element));
		}

		/// <summary>
		/// Returns the number of elements in the given enumerable. A null enumerable is allowed and results in 0.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The collection</param>
		/// <returns>The number of elements in the given enumerable.</returns>
		public static long EnumerableCount<T>(IEnumerable<T> enumerable)
		{
			if ( enumerable == null ) 
				return 0;
			
			long result = 0;
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			while ( enumerator.MoveNext() )
			{
				result++;
			}
			
			return result;
		}

		/// <summary>
		/// Returns true if the number of elements in the given enumerable equals the specified length. A null enumerable is allowed and
		/// has length 0.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The collection</param>
		/// <param name="length">The length to check.</param>
		/// <returns>True if the number of elements in the given enumerable equals the specified length; otherwise false.</returns>
		public static bool EnumerableHasLength<T>(IEnumerable<T> enumerable, int length)
		{
			if ( enumerable == null ) 
				return ( length == 0 );

			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			while ( length > 0 )
			{
				if ( !enumerator.MoveNext() )
				{
					return false;
				}
				length--;
			}
			
			return ( !enumerator.MoveNext() );
		}


		/// <summary>
		/// Returns enumeration being a concatenation of parameters.
		/// </summary>
		/// <typeparam name="T">The element type of the collection</typeparam>
		/// <param name="left">An enumeration of elements. The enumeration may be null.</param>
		/// <param name="right">An enumeration of elements. The enumeration may be null.</param>
		/// <returns>Concatenation of left and right enumerables.</returns>
		public static IEnumerable<T> Concat<T>( IEnumerable<T> left, IEnumerable<T> right )
		{
			if ( left != null )
			{
				foreach ( T e in left )
				{
					yield return e;
				}
			}

			if ( right != null )
			{
				foreach ( T e in right )
				{
					yield return e;
				}
			}
		}

	}

}
