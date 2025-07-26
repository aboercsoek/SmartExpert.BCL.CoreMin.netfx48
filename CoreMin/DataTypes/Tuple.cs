//--------------------------------------------------------------------------
// File:    Tuple.cs
// Content:	Implementation of class Tuple
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#if NET2_0 || NET3_0 || NET3_5

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
	/// Tuple factory methods
	/// </summary>
	public static class Tuple
	{
		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <param name="item1">The tuple item</param>
		/// <returns>A tuple with item1 as value stored in tuple.Item1</returns>
		public static Tuple<T1> Of<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <param name="item1">The tuple item</param>
		/// <returns>A tuple with item1 as value stored in tuple.Item1</returns>
		public static Tuple<T1> Create<T1>(T1 item1)
		{
			return new Tuple<T1>(item1);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <returns>A tuple with item1 and item2 as values in tuple.Item1 and tuple.Item2</returns>
		public static Tuple<T1, T2> Of<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <returns>A tuple with item1 and item2 as values in tuple.Item1 and tuple.Item2</returns>
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <typeparam name="T3">The type of the tuple item 3.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <param name="item3">The item3.</param>
		/// <returns>A tuple with item1, item2 and item3 as values in tuple.Item1, tuple.Item2 and tuple.Item3</returns>
		public static Tuple<T1, T2, T3> Of<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <typeparam name="T3">The type of the tuple item 3.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <param name="item3">The item3.</param>
		/// <returns>A tuple with item1, item2 and item3 as values in tuple.Item1, tuple.Item2 and tuple.Item3</returns>
		public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new Tuple<T1, T2, T3>(item1, item2, item3);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <typeparam name="T3">The type of the tuple item 3.</typeparam>
		/// <typeparam name="T4">The type of the tuple item 4.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <param name="item3">The item3.</param>
		/// <param name="item4">The item4.</param>
		/// <returns>A tuple with item1, item2, item3 and item4 as values in tuple.Item1, tuple.Item2, tuple.Item3 and tuple.Item4</returns>
		public static Tuple<T1, T2, T3, T4> Of<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		/// <summary>
		/// Create Tuple.
		/// </summary>
		/// <typeparam name="T1">The type of the tuple item 1.</typeparam>
		/// <typeparam name="T2">The type of the tuple item 2.</typeparam>
		/// <typeparam name="T3">The type of the tuple item 3.</typeparam>
		/// <typeparam name="T4">The type of the tuple item 4.</typeparam>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <param name="item3">The item3.</param>
		/// <param name="item4">The item4.</param>
		/// <returns>A tuple with item1, item2, item3 and item4 as values in tuple.Item1, tuple.Item2, tuple.Item3 and tuple.Item4</returns>
		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

	}

	///<summary>The 1-tuple structure</summary>
	/// <typeparam name="T1">The type of the 1. tuple element.</typeparam>
	/// <remarks>See <a href="http://de.wikipedia.org/wiki/Tupel" target="_blank">http://de.wikipedia.org/wiki/Tupel</a> to get more information about tuples.</remarks>
	[Serializable]
	public sealed class Tuple<T1>
	{
		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple{T1}"/> struct.
		/// </summary>
		/// <param name="item1">The first tuple item.</param>
		public Tuple(T1 item1)
		{
			Item1 = item1;
		}

		#endregion

		#region Properties

		/// <summary>Tuple item 1</summary>
		public T1 Item1 { get; set; }

		#endregion

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1}"/> to <typeparamref name="T1"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T1(Tuple<T1> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item1;
			}
			return default(T1);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Convert tuple to type <typeparamref name="T1"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T1"/>).
		/// </returns>
		public T1 ToItem1()
		{
			return Item1;
		}

		/// <summary>
		/// Enumerates the tuple items.
		/// </summary>
		/// <returns>The tuple items</returns>
		public IEnumerable<object> EnumItems()
		{
			yield return Item1;
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return "[{0}]".SafeFormatWith(Item1);
		}

		#endregion
	}


	///<summary>The 2-tuple structure</summary>
	/// <typeparam name="T1">The type of the 1. tuple element.</typeparam>
	/// <typeparam name="T2">The type of the 2. tuple element.</typeparam>
	/// <remarks>See <a href="http://de.wikipedia.org/wiki/Tupel" target="_blank">http://de.wikipedia.org/wiki/Tupel</a> to get more information about tuples.</remarks>
	[Serializable]
	public sealed class Tuple<T1, T2>
	{
		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple{T1, T2}"/> struct.
		/// </summary>
		/// <param name="item1">The first tuple item.</param>
		/// <param name="item2">The second tuple item.</param>
		public Tuple( T1 item1, T2 item2 )
		{
			Item1 = item1;
			Item2 = item2;
		}

		#endregion

		#region Properties

		/// <summary>Tuple item 1</summary>
		public T1 Item1 { get; set; }
		/// <summary>Tuple item 2</summary>
		public T2 Item2 { get; set; }

		#endregion

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2}"/> to <typeparamref name="T1"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T1(Tuple<T1, T2> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item1;
			}
			return default(T1);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2}"/> to <typeparamref name="T2"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T2(Tuple<T1, T2> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item2;
			}
			return default(T2);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Convert tuple to type <typeparamref name="T1"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T1"/>).
		/// </returns>
		public T1 ToItem1()
		{
			return Item1;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T2"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T2"/>).
		/// </returns>
		public T2 ToItem2()
		{
			return Item2;
		}

		/// <summary>
		/// Enumerates the tuple items.
		/// </summary>
		/// <returns>The tuple items</returns>
		public IEnumerable<object> EnumItems()
		{
			yield return Item1;
			yield return Item2;
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return "[{0}, {1}]".SafeFormatWith(Item1, Item2);
		}

		#endregion
	}

	/// <summary>
	/// The 3-tuple structure
	/// </summary>
	/// <typeparam name="T1">The type of the 1. tuple element.</typeparam>
	/// <typeparam name="T2">The type of the 2. tuple element.</typeparam>
	/// <typeparam name="T3">The type of the 3. tuple element.</typeparam>
	/// <remarks>See <a href="http://de.wikipedia.org/wiki/Tupel" target="_blank">http://de.wikipedia.org/wiki/Tupel</a> to get more information about tuples.</remarks>
	[Serializable]
	public sealed class Tuple<T1, T2, T3>
	{
		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple{T1, T2, T3}"/> struct.
		/// </summary>
		/// <param name="item1">The 1. tuple item.</param>
		/// <param name="item2">The 2. tuple item.</param>
		/// <param name="item3">The 3. tuple item.</param>
		public Tuple( T1 item1, T2 item2, T3 item3 )
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		#endregion

		#region Properties

		/// <summary>Tuple item 1</summary>
		public T1 Item1 { get; set; }
		/// <summary>Tuple item 2</summary>
		public T2 Item2 { get; set; }
		/// <summary>Tuple item 3</summary>
		public T3 Item3 { get; set; }

		#endregion

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3}"/> to <typeparamref name="T1"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T1(Tuple<T1, T2, T3> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item1;
			}
			return default(T1);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3}"/> to <typeparamref name="T2"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T2(Tuple<T1, T2, T3> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item2;
			}
			return default(T2);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3}"/> to <typeparamref name="T3"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T3(Tuple<T1, T2, T3> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item3;
			}
			return default(T3);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3}"/> to <see name="Tuple{T1, T2}"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator Tuple<T1, T2>(Tuple<T1, T2, T3> tuple)
		{
			if (tuple != null)
			{
				return new Tuple<T1, T2>(tuple.Item1, tuple.Item2);
			}
			return default(Tuple<T1, T2>);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Convert tuple to type <typeparamref name="T1"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T1"/>).
		/// </returns>
		public T1 ToItem1()
		{
			return Item1;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T2"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T2"/>).
		/// </returns>
		public T2 ToItem2()
		{
			return Item2;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T3"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T3"/>).
		/// </returns>
		public T3 ToItem3()
		{
			return Item3;
		}

		/// <summary>
		/// Enumerates the tuple items.
		/// </summary>
		/// <returns>The tuple items</returns>
		public IEnumerable<object> EnumItems()
		{
			yield return Item1;
			yield return Item2;
			yield return Item3;
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return "[{0}, {1}, {2}]".SafeFormatWith(Item1, Item2, Item3);
		}

		#endregion
	}

	/// <summary>
	/// The 4-tuple structure
	/// </summary>
	/// <typeparam name="T1">The type of the 1. tuple element.</typeparam>
	/// <typeparam name="T2">The type of the 2. tuple element.</typeparam>
	/// <typeparam name="T3">The type of the 3. tuple element.</typeparam>
	/// <typeparam name="T4">The type of the 4. tuple element.</typeparam>
	/// <remarks>See <a href="http://de.wikipedia.org/wiki/Tupel" target="_blank">http://de.wikipedia.org/wiki/Tupel</a> to get more information about tuples.</remarks>
	[Serializable]
	public sealed class Tuple<T1, T2, T3, T4>
	{

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple{T1, T2, T3, T4}"/> struct.
		/// </summary>
		/// <param name="item1">The first tuple item.</param>
		/// <param name="item2">The second tuple item.</param>
		/// <param name="item3">The third tuple item.</param>
		/// <param name="item4">The fourth tuple item.</param>
		public Tuple( T1 item1, T2 item2, T3 item3, T4 item4 )
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}

		#endregion

		#region Properties

		/// <summary>Tuple item 1</summary>
		public T1 Item1 { get; set; }
		/// <summary>Tuple item 2</summary>
		public T2 Item2 { get; set; }
		/// <summary>Tuple item 3</summary>
		public T3 Item3 { get; set; }
		/// <summary>Tuple item 4</summary>
		public T4 Item4 { get; set; }

		#endregion

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <typeparamref name="T1"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T1(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item1;
			}
			return default(T1);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <typeparamref name="T2"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T2(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item2;
			}
			return default(T2);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <typeparamref name="T3"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T3(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item3;
			}
			return default(T3);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <typeparamref name="T4"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T4(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return tuple.Item4;
			}
			return default(T4);
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <see name="Tuple{T1, T2}"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator Tuple<T1, T2>(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return new Tuple<T1, T2>(tuple.Item1, tuple.Item2);
			}
			return default(Tuple<T1, T2>);
		}


		/// <summary>
		/// Performs an implicit conversion from <see cref="SmartExpert.Tuple{T1,T2,T3,T4}"/> to <see name="Tuple{T1, T2, T3}"/>.
		/// </summary>
		/// <param name="tuple">The tuple.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator Tuple<T1, T2, T3>(Tuple<T1, T2, T3, T4> tuple)
		{
			if (tuple != null)
			{
				return new Tuple<T1, T2, T3>(tuple.Item1, tuple.Item2, tuple.Item3);
			}
			return default(Tuple<T1, T2, T3>);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Convert tuple to type <typeparamref name="T1"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T1"/>).
		/// </returns>
		public T1 ToItem1()
		{
			return Item1;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T2"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T2"/>).
		/// </returns>
		public T2 ToItem2()
		{
			return Item2;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T3"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T3"/>).
		/// </returns>
		public T3 ToItem3()
		{
			return Item3;
		}

		/// <summary>
		/// Convert tuple to type <typeparamref name="T4"/>.
		/// </summary>
		/// <returns>
		/// The convertion result (instance of target type <typeparamref name="T4"/>).
		/// </returns>
		public T4 ToItem4()
		{
			return Item4;
		}

		/// <summary>
		/// Enumerates the tuple items.
		/// </summary>
		/// <returns>The tuple items</returns>
		public IEnumerable<object> EnumItems()
		{
			yield return Item1;
			yield return Item2;
			yield return Item3;
			yield return Item4;
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return "[{0}, {1}, {2}, {3}]".SafeFormatWith(Item1, Item2, Item3, Item4);
		}

		#endregion
	}
}

#endif
