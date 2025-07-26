//--------------------------------------------------------------------------
// File:    ArrayExtensions.cs
// Content:	Implementation of Extensions class ArrayExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Collections.Generic;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as Array therewith the extension methods are available by using the System namespace.
namespace SmartExpert.Linq
{

	/// <summary>
	/// Represents extension methods for arrays.
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		/// Maps a null array to an empty array.
		/// </summary>
		/// <typeparam name="T">The array type</typeparam>
		/// <param name="array">The array to map.</param>
		/// <returns>An empty array if <paramref name="array"/> is <see langword="null"/>; otherwise <paramref name="array"/>.</returns>
		public static T[] MapNullToEmpty<T>(this T[] array)
		{
			return (array ?? EmptyArray<T>.Instance);
		}

		/// <summary>
		/// Safe array length operation.
		/// </summary>
		/// <typeparam name="T">The type of the array.</typeparam>
		/// <param name="array">The array to get the length for.</param>
		/// <returns>Retruns -1 if array is null; otherwise the array length will be returned.</returns>
		public static int SafeLength<T>(this T[] array )
		{
			if (array == null)
				return -1;
			return array.Length;
		}

		/// <summary>
		/// Returns a <see ref="ReadOnlyCollection{T}">read-only wrapper</see> for the specified array.
		/// </summary>
		/// <typeparam name="T">Array type</typeparam>
		/// <param name="array">Source array of type T</param>
		/// <returns>A <see cref="ReadOnlyCollection{T}"/> of the source array.</returns>
		/// <exception cref="ArgNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		public static ReadOnlyCollection<T> ToReadOnly<T>( this T[] array )
		{
			ArgChecker.ShouldNotBeNull(array, "array");

			return ReadOnlyCollection<T>.AsReadOnly(array);
		}

		/// <summary>
		/// Converts all elements of type <typeparamref name="TSource"/> in the specified <see cref="System.Array"/> into
		/// an array of type <typeparamref name="TTarget"/> bei using the specified <paramref name="converter"/>.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="array">The source array of type TSource.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>An array of type <typeparamref name="TTarget"/>, witch was genereated bei the specified converter.</returns>
		/// <exception cref="ArgNullException"><paramref name="array"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
		public static TTarget[] ConvertAll<TSource, TTarget>( this TSource[] array, Converter<TSource, TTarget> converter )
		{
			ArgChecker.ShouldNotBeNull(array, "array");
			ArgChecker.ShouldNotBeNull(converter, "converter");

			IEnumerable<TSource> enumerable = array;
			return enumerable.ConvertAll(converter).ToArray();
		}

		/// <summary>
		/// Bypasses elements in the array as long as the specified match condition is true and then returns the remaining elements an new array.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <param name="match">The match condition.</param>
		/// <returns>
		/// An array of type TSource that contains the elements from the input array starting at the first element in the linear series 
		/// that does not pass the test specified by match.
		/// </returns>
		/// <exception cref="ArgNullException"><paramref name="array"/> or <paramref name="match"/> is <see langword="null"/>.</exception>
		public static TSource[] SkipWhile<TSource>( this TSource[] array, Predicate<TSource> match )
		{
			ArgChecker.ShouldNotBeNull(array, "array");
			ArgChecker.ShouldNotBeNull(match, "match");

			Func<TSource, bool> func = t => match(t);

			return array.SkipWhile(func).ToArray();
		}

		/// <summary>
		/// Returns a specified number of contiguous elements from the start of the array.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <returns>An <see cref="Array"/> of type TSource that contains the specified number of elements from the start of the input array.</returns>
		/// <exception cref="ArgNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgOutOfRangeException{TValue}"><paramref name="count"/> is less than zero.</exception>
		public static TSource[] Take<TSource>( this TSource[] array, int count )
		{
			ArgChecker.ShouldNotBeNull(array, "array");

			//ArgChecker.ShouldBeInRange<int>(count, "count", 0, Int32.MaxValue);
			if (count <= 0)
				return new TSource[0];
			if (count >= array.Length)
				return array;

			IEnumerable<TSource> enumerable = array;
			return enumerable.Take(count).ToArray();
		}

		/// <summary>
		/// Returns elements from the source array as long as a specified match condition is true.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <param name="match">The match condition.</param>
		/// <returns>
		/// An <see cref="Array"/> of type TSource that contains the elements from the source array that occur 
		/// before the element at which the test no longer passes.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> or <paramref name="match"/> is <see langword="null"/>.</exception>
		public static TSource[] TakeWhile<TSource>( this TSource[] array, Predicate<TSource> match )
		{
			if ( array == null )
			{
				throw new ArgumentNullException("array");
			}
			if ( match == null )
			{
				throw new ArgumentNullException("match");
			}
			Func<TSource, bool> func = t => match(t);

			return array.TakeWhile(func).ToArray();
		}

		/// <summary>
		/// Bypasses a specified number of elements in the source array and then returns the remaining elements.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <param name="count">The number of elements to skip before returning the remaining elements.</param>
		/// <returns>An <see cref="Array"/> of type TSource that contains the elements that occur after the specified index in the source array.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		/// <exception cref="IndexOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		public static TSource[] Skip<TSource>( this TSource[] array, int count )
		{
			if ( array == null )
			{
				throw new ArgumentNullException("array");
			}
			if ( count < 0 )
			{
				return array;
			}
			IEnumerable<TSource> enumerable = array;
			return enumerable.Skip(count).ToArray();
		}

		/// <summary>
		/// Generates an array that contains one repeated value.
		/// </summary>
		/// <typeparam name="T">The element type.</typeparam>
		/// <param name="element">The element to repeat.</param>
		/// <param name="count">The repeat count.</param>
		/// <returns>An T[] array that contains the repeated value specified by <paramref name="element"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		public static T[] Repeat<T>( T element, int count )
		{
			if ( count <= 0 )
			{
				return new T[0];
			}
			return Enumerable.Repeat(element, count).ToArray();
		}

		/// <summary>
		/// Concatenates two arrays.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="first">The first array.</param>
		/// <param name="second">The second array.</param>
		/// <returns>An TSource[] array that contains the concatenated elements of the two input arrays.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.</exception>
		public static TSource[] Concat<TSource>( this TSource[] first, TSource[] second )
		{
			if ( first == null )
			{
				throw new ArgumentNullException("first");
			}
			if ( second == null )
			{
				throw new ArgumentNullException("second");
			}
			IEnumerable<TSource> enumerable = first;
			return enumerable.Concat(second).ToArray();
		}

		/// <summary>
		/// Reverses the specified array.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <returns>An array whose elements correspond to those of the source array in reverse order.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		public static TSource[] Reverse<TSource>( this TSource[] array )
		{
			if ( array == null )
			{
				throw new ArgumentNullException("array");
			}
			IEnumerable<TSource> enumerable = array;
			return enumerable.Reverse().ToArray();
		}

		/// <summary>
		/// Produces the set union of two arrays by using the default equality comparer.
		/// </summary>
		/// <typeparam name="TSource">The source array type. The TSource type must implement <see cref="IEquatable{TSource}"/>.</typeparam>
		/// <param name="array1">The first array.</param>
		/// <param name="array2">The second array.</param>
		/// <returns>An <see cref="Array"/> that contains the elements from both input arrays, excluding duplicates.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array1"/> or <paramref name="array2"/> is <see langword="null"/>.</exception>
		public static TSource[] Union<TSource>( this TSource[] array1, TSource[] array2 ) where TSource : IEquatable<TSource>
		{
			if ( array1 == null )
			{
				throw new ArgumentNullException("array1");
			}
			if ( array2 == null )
			{
				throw new ArgumentNullException("array2");
			}

			IEnumerable<TSource> enumerable1 = array1;
			return enumerable1.Union(array2).ToArray();
		}

		/// <summary>
		/// Returns all the items in <paramref name="array1"/> that are not in <paramref name="array2"/>.
		/// </summary>
		/// <typeparam name="TSource">The source array type. The TSource type must implement <see cref="IEquatable{TSource}"/>.</typeparam>
		/// <param name="array1">The array1.</param>
		/// <param name="array2">The array2.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="array1"/> or <paramref name="array2"/> is <see langword="null"/>.</exception>
		public static TSource[] Complement<TSource>( this TSource[] array1, TSource[] array2 ) where TSource : IEquatable<TSource>
		{
			if ( array1 == null )
			{
				throw new ArgumentNullException("array1");
			}
			if ( array2 == null )
			{
				throw new ArgumentNullException("array2");
			}
			IEnumerable<TSource> enumerable1 = array1;
			return enumerable1.Complement(array2).ToArray();
		}

		/// <summary>
		/// Produces the set difference of two arrays by using the default equality comparer to compare values.
		/// </summary>
		/// <typeparam name="TSource">The source array type. The TSource type must implement <see cref="IEquatable{TSource}"/>.</typeparam>
		/// <param name="array1">The array1.</param>
		/// <param name="array2">The array2.</param>
		/// <returns>An array that contains the set difference of the elements of the two arrays.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array1"/> or <paramref name="array2"/> is <see langword="null"/>.</exception>
		public static TSource[] Except<TSource>( this TSource[] array1, TSource[] array2 ) where TSource : IEquatable<TSource>
		{
			if ( array1 == null )
			{
				throw new ArgumentNullException("array1");
			}
			if ( array2 == null )
			{
				throw new ArgumentNullException("array2");
			}
			IEnumerable<TSource> enumerable1 = array1;
			return enumerable1.Except(array2).ToArray();
		}

		/// <summary>
		/// Produces the set intersection of the two arrays by using the default equality comparer to compare values.
		/// </summary>
		/// <typeparam name="TSource">The source array type. The TSource type must implement <see cref="IEquatable{TSource}"/>.</typeparam>
		/// <param name="array1">The array1.</param>
		/// <param name="array2">The array2.</param>
		/// <returns>An array that contains the elements that form the set intersection of the two arrays.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array1"/> or <paramref name="array2"/> is <see langword="null"/>.</exception>
		public static TSource[] Intersect<TSource>( this TSource[] array1, TSource[] array2 ) where TSource : IEquatable<TSource>
		{
			if ( array1 == null )
			{
				throw new ArgumentNullException("array1");
			}
			if ( array2 == null )
			{
				throw new ArgumentNullException("array2");
			}
			IEnumerable<TSource> enumerable1 = array1;
			return enumerable1.Intersect(array2).ToArray();
		}

		/// <summary>
		/// Returns distinct elements from the array by using the default equality comparer to compare values.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <returns>An array that contains distinct elements from the source array.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		public static TSource[] Distinct<TSource>( this TSource[] array )
		{
			if ( array == null )
			{
				throw new ArgumentNullException("array");
			}
			IEnumerable<TSource> enumerable = array;
			return enumerable.Distinct().ToArray();
		}


		/// <summary>
		/// Sorts the specified array.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <returns>The sorted source array.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
		public static TSource[] Sort<TSource>( this TSource[] array )
		{
			if ( array == null )
			{
				return new TSource[0];
				//throw new ArgumentNullException("array");
			}
			IEnumerable<TSource> enumerable = array;
			return enumerable.Sort().ToArray();
		}

		/// <summary>
		/// Filters the array based on a match condition.
		/// </summary>
		/// <typeparam name="TSource">The source array type.</typeparam>
		/// <param name="array">The source array.</param>
		/// <param name="match">The match condition.</param>
		/// <returns>The filtered source array.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> or <paramref name="match"/> is <see langword="null"/>.</exception>
		public static TSource[] Where<TSource>( this TSource[] array, Predicate<TSource> match )
		{
			if ( array == null )
			{
				return new TSource[0];
			}
			if ( match == null )
			{
				throw new ArgumentNullException("match");
			}
			
			Func<TSource, bool> func = t => match(t);

			IEnumerable<TSource> enumerable = array;

			return enumerable.Where(func).ToArray();
		}
	}
}
