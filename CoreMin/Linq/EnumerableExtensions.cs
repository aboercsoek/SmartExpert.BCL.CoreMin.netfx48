//--------------------------------------------------------------------------
// File:    EnumerableExtensions.cs
// Content:	Implementation of class EnumerableExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copysecond © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SmartExpert.Error;

#endregion

// Uses the same namespace as IEnumerable<T> therewith the extension methods are available by using the System.Collections.Generic namespace.
namespace SmartExpert.Linq
{
	/// <summary>
	/// Represents extension methods for <see cref="IEnumerable{T}"/> types.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Convert dynamic paramter list of type T parameters to a sequence of type T.
		/// </summary>
		/// <typeparam name="T">Type of sequence items.</typeparam>
		/// <param name="items">The items.</param>
		/// <returns>A sequence of type T</returns>
		public static IEnumerable<T> ToEnumerable<T>(params T[] items)
		{
			if (items == null)
				yield break;

			foreach (var item in items)
				yield return item;
		}

		/// <summary>
		/// Convert a sequence of type <see cref="System.Collections.Generic.IEnumerable{T}"/> 
		/// to type <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>.
		/// </summary>
		/// <typeparam name="T">Item Type</typeparam>
		/// <param name="ienum">The sequence to convert.</param>
		/// <returns>
		/// The convertion result (instance of target type <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>).
		/// </returns>
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> ienum)
		{
			var oc = new ObservableCollection<T>();

			foreach (T i in ienum)
			{
				oc.Add(i);
			}

			return oc;
		}

		/// <summary>
		/// Retruns an empty IEnumerable[T] instance if <paramref name="sequence"/> is <see langword="null"/>; otherwise
		/// the <paramref name="sequence"/> itself.
		/// </summary>
		/// <typeparam name="T">The type of the <paramref name="sequence"/>.</typeparam>
		/// <param name="sequence">The sequence to map.</param>
		/// <returns>An empty IEnumerable[T] instance if <paramref name="sequence"/> is <see langword="null"/>; otherwise
		/// the <paramref name="sequence"/> self.</returns>
		public static IEnumerable<T> MapNullToEmpty<T>(this IEnumerable<T> sequence)
		{
			return (sequence ?? Enumerable.Empty<T>());
		}

		/// <summary>
		/// Provides a full outer join of two sequences.
		/// </summary>
		/// <typeparam name="T">Type of sequence one</typeparam>
		/// <typeparam name="U">Type of sequence two</typeparam>
		/// <param name="one">Sequence one.</param>
		/// <param name="two">Sequence two.</param>
		/// <param name="match">The match filter.</param>
		/// <returns>The result of the full outer join of sequence one and sequence two.</returns>
		public static IEnumerable<Tuple<T, U>> FullOuterJoin<T, U>(this IEnumerable<T> one, IEnumerable<U> two, Func<T, U, bool> match)
		{
			if (one == null)
				return Enumerable.Empty<Tuple<T, U>>();
			if (two == null)
				return Enumerable.Empty<Tuple<T, U>>();

			ArgChecker.ShouldNotBeNull(match, "match");

			var first = from a in one
						from b in
							(from b in two
							where match(a, b)
							select b).DefaultIfEmpty<U>()
						select Tuple.Create(a, b);

			var second = from b in two
						 from a in
							(from a in one
							where match(a, b)
							select a).DefaultIfEmpty<T>()
						 select Tuple.Create(a, b);

			return first.Concat(second).Distinct();
		}



		/// <summary>
		/// Determines whether the two collections are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input collections.</typeparam>
		/// <param name="first">An ICollection{T} to compare to the second collection.</param>
		/// <param name="second">An ICollection{T} to compare to the first collection.</param>
		/// <returns>
		/// <see langword="true"/> if the two collections are of equal length and their corresponding elements 
		/// are equal according to the default equality comparer for their type; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of first and second collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this ICollection<T> first, ICollection<T> second)
		{
			return first.EqualEnumerables(second, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An ICollection{T} to compare to the second sequence.</param>
		/// <param name="second">An IEnumerable{T} to compare to the first collection.</param>
		/// <returns>
		/// <see langword="true"/> if the two collections are of equal length and their corresponding elements 
		/// are equal according to the default equality comparer for their type; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of first collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this ICollection<T> first, IEnumerable<T> second)
		{
			return first.EqualEnumerables(second, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An IEnumerable{T} to compare to the second collection.</param>
		/// <param name="second">An ICollection{T} to compare to the first sequence.</param>
		/// <returns>
		/// <see langword="true"/> if the two collections are of equal length and their corresponding elements 
		/// are equal according to the default equality comparer for their type; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of second collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this IEnumerable<T> first, ICollection<T> second)
		{
			return first.EqualEnumerables(second, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An System.Collections.Generic.IEnumerable{T} to compare to second.</param>
		/// <param name="second">An System.Collections.Generic.IEnumerable{T} to compare to the first sequence.</param>
		/// <returns>
		/// <see langword="true"/> if the two collections are of equal length and their corresponding elements 
		/// are equal according to the default equality comparer for their type; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool EqualEnumerables<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			return first.EqualEnumerables(second, (l, r) => EqualityComparer<T>.Default.Equals(l, r));
		}

		
		/// <summary>
		/// Determines whether the two collections are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input collections.</typeparam>
		/// <param name="first">An ICollection{T} to compare to the second collection.</param>
		/// <param name="second">An ICollection{T} to compare to the first collection.</param>
		/// <param name="compare">The compare function delegate.</param>
		/// <returns>
		/// <see langword="true"/> if the two collections are of equal length and their corresponding elements 
		/// are equal according to the result of the compare function delegate; otherwise, false.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of first and second collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this ICollection<T> first, ICollection<T> second, Func<T, T, bool> compare)
		{
			if (first == second)
			{
				return true;
			}
			if ((first == null) || (second == null))
			{
				return false;
			}
			if (first.Count != second.Count)
			{
				return false;
			}
			return EqualEnumerablesImpl(first, second, compare);
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An ICollection{T} to compare to the second sequence.</param>
		/// <param name="second">An IEnumerable{T} to compare to the first collection.</param>
		/// <param name="compare">The compare function delegate.</param>
		/// <returns>
		/// <see langword="true"/> if the two sequences are of equal length and their corresponding elements 
		/// are equal according to the result of the compare function delegate; otherwise, false.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of first collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this ICollection<T> first, IEnumerable<T> second, Func<T, T, bool> compare)
		{
			int num;
			if (first == second)
			{
				return true;
			}
			if ((first == null) || (second == null))
			{
				return false;
			}
			if (second.TryGetFastCount(out num) && (first.Count != num))
			{
				return false;
			}
			return EqualEnumerablesImpl(first, second, compare);
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An IEnumerable{T} to compare to the second collection.</param>
		/// <param name="second">An ICollection{T} to compare to the first sequence.</param>
		/// <param name="compare">The compare function delegate.</param>
		/// <returns>
		/// <see langword="true"/> if the two sequences are of equal length and their corresponding elements 
		/// are equal according to the result of the compare function delegate; otherwise, false.
		/// </returns>
		/// <remarks>
		/// Static overload to prevent runtime type checks of second collection (fast count check).
		/// </remarks>
		public static bool EqualEnumerables<T>(this IEnumerable<T> first, ICollection<T> second, Func<T, T, bool> compare)
		{
			int num;
			if (first == second)
			{
				return true;
			}
			if ((first == null) || (second == null))
			{
				return false;
			}
			if (first.TryGetFastCount(out num) && (num != second.Count))
			{
				return false;
			}
			return EqualEnumerablesImpl(first, second, compare);
		}

		/// <summary>
		/// Determines whether the two sequences are equal by comparing the count and the elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">An System.Collections.Generic.IEnumerable{T} to compare to second.</param>
		/// <param name="second">An System.Collections.Generic.IEnumerable{T} to compare to the first sequence.</param>
		/// <param name="compare">The compare function delegate.</param>
		/// <returns>
		/// <see langword="true"/> if the two sequences are of equal length and their corresponding elements 
		/// are equal according to the result of the compare function delegate; otherwise, false.
		/// </returns>
		public static bool EqualEnumerables<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> compare)
		{

			int firstCount;
			int secondCount;

			if (first == second)
			{
				return true;
			}

			if ((first == null) || (second == null))
			{
				return false;
			}

			if ((first.TryGetFastCount(out firstCount) && second.TryGetFastCount(out secondCount)) && (firstCount != secondCount))
			{
				return false;
			}

			return EqualEnumerablesImpl(first, second, compare);
		}

		private static bool EqualEnumerablesImpl<T>(IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> compare)
		{
			using (IEnumerator<T> firstEnumerator = first.GetEnumerator())
			{
				using (IEnumerator<T> secondEnumerator = second.GetEnumerator())
				{
					do
					{
						bool nextLeftItemAvailable = firstEnumerator.MoveNext();
						bool nextRightItemAvailable = secondEnumerator.MoveNext();

						if (nextLeftItemAvailable && nextRightItemAvailable) continue;
						
						return (nextLeftItemAvailable == nextRightItemAvailable);

					} while (compare(firstEnumerator.Current, secondEnumerator.Current));
				}
			}

			return false;
		}

		/// <summary>
		/// Tries to determine the number of elements in <paramref name="sequence" /> as fast as possible.
		/// </summary>
		/// <typeparam name="T">The sequence item type.</typeparam>
		/// <param name="sequence">The sequence.</param>
		/// <returns>The sequence item count, or -1 if sequence is null.</returns>
		public static int FastCount<T>(this IEnumerable<T> sequence)
		{
			if (sequence == null)
				return -1;

			int fastCount;
			return sequence.TryGetFastCount(out fastCount) ? fastCount : sequence.Count();
		}

		/// <summary>
		/// Checks if the lenght of the collection can be checked fast.
		/// </summary>
		/// <param name="collection">The collection to prove.</param>
		/// <returns>
		/// Returns <see langword="true"/> if length of <paramref name="collection" /> can be checked fast and this check gives <c>0</c>.
		/// Otherwise returns <see langword="false"/> (it does not necessarily mean that <paramref name="collection" /> is not empty)
		/// </returns>
		public static bool CanBeProvenEmptyFast<T>(this IEnumerable<T> collection)
		{
			int num;
			return (collection.TryGetFastCount(out num) && (num == 0));
		}

		/// <summary>
		/// Tries to determine the number of elements in <paramref name="collection" /> by checking if <paramref name="collection" /> is of type
		/// ICollection{T} or ICollection or string, to be able to use the Count or Length properties of this types.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> is the method succeeded and the number of elements has been assigned to <paramref name="count" />, <see langword="false"/> otherwise.
		/// </returns>
		public static bool TryGetFastCount<T>(this IEnumerable<T> collection, out int count)
		{
			try
			{
				var isCollectionT = collection as ICollection<T>;
				if (isCollectionT != null)
				{
					count = isCollectionT.Count;
					return true;
				}
				var isCollection = collection as ICollection;
				if (isCollection != null)
				{
					count = isCollection.Count;
					return true;
				}
				var isString = collection as string;
				if (isString != null)
				{
					count = isString.Length;
					return true;
				}
			}
			catch (OverflowException)
			{
			}
			count = 0;
			return false;
		}



		/// <summary>
		/// Given a set of items (the parents) and a function that enumerates the children of a parent
		/// this method yields pairs of (parent,child) tuples.
		/// </summary>
		/// <typeparam name="TParent">The parent type.</typeparam>
		/// <typeparam name="TChild">The child type.</typeparam>
		/// <param name="parentItems">The parent items.</param>
		/// <param name="childItemsPredicate">The child items predicate.</param>
		/// <returns>The (parent,child) tuples.</returns>
		/// <exception cref="ArgNullException"><paramref name="parentItems"/> or <paramref name="childItemsPredicate"/> is <see langword="null"/>.</exception>
		public static IEnumerable<Tuple<TParent, TChild>> SelectManyPairs<TParent, TChild>(this IEnumerable<TParent> parentItems, Func<TParent, IEnumerable<TChild>> childItemsPredicate)
		{
			ArgChecker.ShouldNotBeNull(parentItems, "parentItems");
			ArgChecker.ShouldNotBeNull(childItemsPredicate, "childItemsPredicate");

			return from parent in parentItems
				   from child in childItemsPredicate(parent)
				   select new Tuple<TParent, TChild>(parent, child);
		}

		/// <summary>
		/// Runs an IF-THEN execution for all elements in the source items.
		/// </summary>
		/// <typeparam name="T">Source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="ifPredicate">The IF predicate.</param>
		/// <param name="thenAction">The THEN action.</param>
		/// <returns>Returns <see langword="true"/> if <strong>no</strong> exception was thrown during IF-THEN-ELSE execution; otherwise <see langword="false"/></returns>
		/// <exception cref="ArgNullException"><paramref name="items"/>, <paramref name="ifPredicate"/> or <paramref name="thenAction"/> is <see langword="null"/>.</exception>
		/// <seealso cref="Func{T,TResult}"/><seealso cref="Action{T}"/>
		public static bool IfThen<T>(this IEnumerable<T> items, Func<T, bool> ifPredicate, Action<T> thenAction)
		{
			bool result = true;
			ArgChecker.ShouldNotBeNull(items, "items");
			ArgChecker.ShouldNotBeNull(ifPredicate, "ifPredicate");
			ArgChecker.ShouldNotBeNull(thenAction, "thenAction");

			foreach (T element in items)
			{
				try
				{
					if (ifPredicate(element))
						thenAction(element);
				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;

					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// Runs an IF-THEN execution for all elements in the source items.
		/// </summary>
		/// <typeparam name="TSourceItem">Source items element type.</typeparam>
		/// <typeparam name="TIfCondition">IF condition type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="ifPredicate">The IF predicate.</param>
		/// <param name="ifCondition">The IF condition.</param>
		/// <param name="thenAction">The THEN action.</param>
		/// <returns>
		/// Returns <see langword="true"/> if <strong>no</strong> exception was thrown during IF-THEN-ELSE execution; otherwise <see langword="false"/>
		/// </returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="items"/>, <paramref name="ifPredicate"/> or <paramref name="thenAction"/> is <see langword="null"/>.</exception>
		public static bool IfThen<TSourceItem, TIfCondition>(this IEnumerable<TSourceItem> items,
															Func<TSourceItem, TIfCondition, bool> ifPredicate, TIfCondition ifCondition,
															Action<TSourceItem> thenAction)
		{
			bool result = true;
			ArgChecker.ShouldNotBeNull(items, "items");
			ArgChecker.ShouldNotBeNull(ifPredicate, "ifPredicate");
			ArgChecker.ShouldNotBeNull(thenAction, "thenAction");

			foreach (TSourceItem element in items)
			{
				try
				{
					if (ifPredicate(element, ifCondition))
						thenAction(element);
				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;

					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// Runs an IF-THEN-ELSE execution for all elements in the source items.
		/// </summary>
		/// <typeparam name="T">Source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="ifPredicate">The IF predicate.</param>
		/// <param name="thenAction">The THEN action.</param>
		/// <param name="elseAction">The ELSE action.</param>
		/// <returns>Returns <see langword="true"/> if <strong>no</strong> exception was thrown during IF-THEN-ELSE execution; otherwise <see langword="false"/></returns>
		/// <exception cref="ArgNullException"><paramref name="items"/>, <paramref name="ifPredicate"/>,<paramref name="thenAction"/> or <paramref name="elseAction"/> is <see langword="null"/>.</exception>
		/// <seealso cref="Func{T,TResult}"/><seealso cref="Action{T}"/>
		public static bool IfThenElse<T>(this IEnumerable<T> items, Func<T, bool> ifPredicate, Action<T> thenAction, Action<T> elseAction)
		{
			bool result = true;
			ArgChecker.ShouldNotBeNull(items, "items");
			ArgChecker.ShouldNotBeNull(ifPredicate, "ifPredicate");
			ArgChecker.ShouldNotBeNull(thenAction, "thenAction");
			ArgChecker.ShouldNotBeNull(elseAction, "elseAction");

			foreach (T element in items)
			{
				try
				{
					if (ifPredicate(element))
						thenAction(element);
					else
						elseAction(element);

				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;

					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// Runs an IF-THEN-ELSE execution for all elements in the source items.
		/// </summary>
		/// <typeparam name="TSourceItem">Source items element type.</typeparam>
		/// <typeparam name="TIfCondition">IF condition type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="ifPredicate">The IF predicate.</param>
		/// <param name="ifCondition">The IF condition.</param>
		/// <param name="thenAction">The THEN action.</param>
		/// <param name="elseAction">The ELSE action.</param>
		/// <returns>
		/// Returns <see langword="true"/> if <strong>no</strong> exception was thrown during IF-THEN-ELSE execution; otherwise <see langword="false"/>
		/// </returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="items"/>, <paramref name="ifPredicate"/>,<paramref name="thenAction"/> or <paramref name="elseAction"/> is <see langword="null"/>.</exception>
		public static bool IfThenElse<TSourceItem, TIfCondition>(this IEnumerable<TSourceItem> items, Func<TSourceItem, TIfCondition, bool> ifPredicate, TIfCondition ifCondition, Action<TSourceItem> thenAction, Action<TSourceItem> elseAction)
		{
			bool result = true;
			ArgChecker.ShouldNotBeNull(items, "items");
			ArgChecker.ShouldNotBeNull(ifPredicate, "ifPredicate");
			ArgChecker.ShouldNotBeNull(thenAction, "thenAction");
			ArgChecker.ShouldNotBeNull(elseAction, "elseAction");

			foreach (TSourceItem element in items)
			{
				try
				{
					if (ifPredicate(element, ifCondition))
						thenAction(element);
					else
						elseAction(element);

				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;

					result = false;
				}
			}

			return result;
		}
		/// <summary>
		/// Convert a items elements to type <see cref="System.Collections.Generic.HashSet{T}"/>.
		/// </summary>
		/// <typeparam name="T">The source items elements type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <returns>
		/// The convertion result (instance of target type <see cref="System.Collections.Generic.HashSet{T}"/>).
		/// </returns>
		/// <exception cref="ArgNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
		{
			ArgChecker.ShouldNotBeNull(items, "items");

			var s = new HashSet<T>(items);
			return s;
		}

		/// <summary>
		/// Converts aa eumerable items of type T into an eumerable items with an index.
		/// </summary>
		/// <typeparam name="T">The source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <returns>A IEnumerable-Collection of <see cref="IndexValuePair{T}"/> items.</returns>
		/// <exception cref="ArgNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static IEnumerable<IndexValuePair<T>> EnumWithIndex<T>(this IEnumerable<T> items)
		{
			ArgChecker.ShouldNotBeNull(items, "items");

			int i = 0;
			foreach (T item in items)
			{
				yield return new IndexValuePair<T>(item, i);
				i++;
			}
		}

		/// <summary>
		/// Runs an action for all elements in the source items.
		/// </summary>
		/// <typeparam name="T">The source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="action">The action that should be executed on each element in the source items.</param>
		/// <exception cref="ArgNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
		{
			#region PreConditions

			if (items == null)
				return; // Don't throw an error is source items are null. Just exit the method.

			ArgChecker.ShouldNotBeNull(action, "action");

			#endregion

			foreach (T element in items)
			{
				action(element);
			}
		}

		/// <summary>
		/// Runs an action for all elements in the source items. Action gets the current index as input parameter.
		/// </summary>
		/// <typeparam name="T">The source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="action">The action that should be executed on each element in the source items. Action is called with the current index.</param>
		/// <exception cref="ArgNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		public static void ForEachWithIndex<T>(this IEnumerable<T> items, Action<T, int> action)
		{
			#region PreConditions

			if (items == null)
				return; // Don't throw an error is source items are null. Just exit the method.

			ArgChecker.ShouldNotBeNull(action, "action");

			#endregion

			int num = 0;
			foreach (T local in items)
			{
				action(local, num);
				num++;
			}
		}

		/// <summary>
		/// Cast (using cast operator) all the items of the source collection into items the target collection.
		/// Method constrain: TSource must be sub type of TTarget (class inheritance or interface implementation).
		/// </summary>
		/// <typeparam name="TSource">The source items element type.</typeparam>
		/// <typeparam name="TTarget">The target items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <returns>The enumerable target collection with casted items of type TTarget.</returns>
		/// <exception cref="ArgNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TTarget> Cast<TSource, TTarget>(this IEnumerable<TSource> items)
			where TSource : TTarget
		{
			#region PreConditions

			if (items == null)
				throw new ArgumentNullException("items");

			#endregion

			IEnumerable<TTarget> targetItems = items.Select(sourceItem => (TTarget)sourceItem);
			return targetItems;
		}

		/// <summary>
		/// Converts all the items of type TSource in items to a new items of type TTarget according to converter
		/// </summary>
		/// <typeparam name="TSource">The source items element type.</typeparam>
		/// <typeparam name="TTarget">The target items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="converter">The type converter.</param>
		/// <returns>The converted source items.</returns>
		/// <exception cref="ArgNullException"><paramref name="items"/> or <paramref name="converter"/> is <see langword="null"/>.</exception>
		public static IEnumerable<TTarget> ConvertAll<TSource, TTarget>(this IEnumerable<TSource> items, Converter<TSource, TTarget> converter)
		{
			#region PreConditions

			if (items == null)
				throw new ArgumentNullException("items");

			if (converter == null)
				throw new ArgumentNullException("converter");

			#endregion

			IEnumerable<TTarget> targetItems = items.Select(sourceItem => converter(sourceItem));
			return targetItems;

		}

		/// <summary>
		/// Sorts the items
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <returns>The sorted source items.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static IEnumerable<T> Sort<T>(this IEnumerable<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			var list = new List<T>(items);
			list.Sort();

			return list;
		}

		/// <summary>
		/// Sorts the items
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="comparison">The compare delegate.</param>
		/// <returns>The sorted source items.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static IEnumerable<T> Sort<T>(this IEnumerable<T> items, Comparison<T> comparison)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			var list = new List<T>(items);
			Sort(list, comparison);

			return list;
		}

		/// <summary>
		/// Returns all the items in items1 that are not in items2
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items1">The first items.</param>
		/// <param name="items2">The second items.</param>
		/// <returns>All the items in items1 that are not in items2.</returns>
		/// <exception cref="ArgNullException"><paramref name="items1"/> or <paramref name="items2"/> is <see langword="null"/>.</exception>
		public static IEnumerable<T> Complement<T>(this IEnumerable<T> items1, IEnumerable<T> items2)
		{
			#region PreConditions

			ArgChecker.ShouldNotBeNull(items1, "items1");
			ArgChecker.ShouldNotBeNull(items2, "items2");

			#endregion

			return items1.Where(item => items2.Contains(item) == false);
		}

		/// <summary>
		/// Calls Dispose for all elements in the input.
		/// </summary>
		/// <typeparam name="T">The items element type. Type T must implement <see cref="IDisposable"/>.</typeparam>
		/// <param name="items">The items.</param>
		public static void DisposeAll<T>(this IEnumerable<T> items) where T : IDisposable
		{
			if (items == null)
				return;

			foreach (T element in items)
			{
				if (typeof(T).IsValueType == false)
				{
					// ReSharper disable CompareNonConstrainedGenericWithNull
					if (element == null) continue;
					// ReSharper restore CompareNonConstrainedGenericWithNull
				}

				element.Dispose();
			}
		}

		/// <summary>
		/// Adds all <paramref name="elements"/> to <paramref name="items"/>.
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">Collection where <paramref name="elements"/> should be added</param>
		/// <param name="elements">Elements that should be added to <paramref name="items"/></param>
		/// <exception cref="ArgNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
		public static void AddRange<T>(this ICollection<T> items, IEnumerable<T> elements)
		{
			#region PreConditions

			ArgChecker.ShouldNotBeNull(items, "items");

			#endregion

			if (elements == null)
				return;

			foreach (T o in elements)
				items.Add(o);
		}

		/// <summary>
		/// Index of the value in the items.
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="value">The value.</param>
		/// <returns>The index of value inside the items. Returns -1 if value was not found or theitems is <see langword="null"/></returns>
		public static int IndexOf<T>(this IEnumerable<T> items, T value)
		{
			if (items == null)
				return -1;

			return items.IndexOf(value, null);
		}

		/// <summary>
		/// Index of the value in the items.
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="value">The value.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>The index of value inside the items. Returns -1 if value was not found or theitems is <see langword="null"/></returns>
		public static int IndexOf<T>(this IEnumerable<T> items, T value, IComparer<T> comparer)
		{
			if (items == null)
				return -1;

			if (comparer == null)
				comparer = Comparer<T>.Default;

			IEnumerator<T> iter = items.GetEnumerator();
			int i = 0;

			while (iter.MoveNext())
			{
				T compValue = iter.Current;
				int ret = comparer.Compare(compValue, value);
				if (ret == 0)
					return i;
				++i;
			}

			return -1;
		}

		/// <summary>
		/// Finds the index of a specified <paramref name="value"/>.
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="value">The value.</param>
		/// <returns>The index of <paramref name="value"/> in the source items. If <paramref name="value"/> was not found -1.</returns>
		public static int FindIndex<T>(this IEnumerable<T> items, T value) where T : IEquatable<T>
		{
			if (items == null)
				return -1;

			bool isItemValueType = typeof(T).IsValueType;

			using (IEnumerator<T> iterator = items.GetEnumerator())
			{
				int index = 0;

				while (iterator.MoveNext())
				{
					if (isItemValueType == false)
						if (iterator.Current.IsNull())
							continue;

					if (value.Equals(iterator.Current) == false)
					{
						index++;
					}
					else
					{
						return index;
					}
				}
				return -1;
			}
		}

		/// <summary>
		/// Finds the index witch meets the condition of the provides predicate.
		/// </summary>
		/// <typeparam name="T">The items element type.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="criterion">The criterion.</param>
		/// <returns>Returns the first items index witch satisfies the condition of the predicate, or -1 if no element satisfies the condition.</returns>
		public static int FindIndex<T>(this IEnumerable<T> items, Predicate<T> criterion)
		{
			if (items == null)
				return -1;

			int num = 0;
			foreach (T local in items)
			{
				if (criterion(local))
					return num;

				num++;
			}
			return -1;
		}

		/// <summary>
		/// Converts items of type T to an array of type U.
		/// </summary>
		/// <typeparam name="TSource">The source items element type.</typeparam>
		/// <typeparam name="TTarget">The target array type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <param name="converter">The type converter.</param>
		/// <returns>The converted source items as array of type U.</returns>
		/// <exception cref="ArgNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
		public static TTarget[] UnsafeToArray<TSource, TTarget>(this IEnumerable items, Converter<TSource, TTarget> converter)
		{
			if (items == null)
				return new TTarget[0];

			ArgChecker.ShouldNotBeNull(converter, "converter");
			
			return Array.ConvertAll(items.UnsafeToArray<TSource>(), converter);
		}


		/// <summary>
		///  Converts items to an array.
		/// </summary>
		/// <typeparam name="T">The source items element type.</typeparam>
		/// <param name="items">The source items.</param>
		/// <returns>The source items as array.</returns>
		public static T[] UnsafeToArray<T>(this IEnumerable items)
		{
			return items == null ? new T[0] : CollectionHelper.UnsafeToArray<T>(items);
		}
	}
}
