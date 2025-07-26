//--------------------------------------------------------------------------
// File:    ObjectExtensions.cs
// Content:	Implementation of class ObjectExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace SmartExpert
{
	///<summary>Fluent <see cref="Object"/> and <see cref="Type"/> Extensions.</summary>
	public static class ObjectEx
	{

		#region Fluent version of 'is'

		/// <summary>
		/// Fluent version of CSharp 'is' keyword
		/// </summary>
		/// <typeparam name="T">Type to check</typeparam>
		/// <param name="item">value that should be checked</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is of Type <typeparamref name="T"/>; otherwise <see langword="false"/>.</returns>
		public static bool Is<T>(this Object item)
		{
			return  item.IsTypeOf(typeof(T));
		}

		/// <summary>
		/// Fluent version of CSharp 'is' keyword with type instance as argument.
		/// </summary>
		/// <param name="item">value that should be checked</param>
		/// <param name="type">The type to check value against.</param>
		/// <returns><see langword="true"/> if <paramref name="item"/> is of Type <paramref name="type"/>; otherwise <see langword="false"/>.</returns>
		public static bool IsTypeOf(this Object item, Type type)
		{
			if ((type == null)||(item == null))
				return false;

			if (Type.GetTypeFromHandle(Type.GetTypeHandle(item)) == type)
				return true;

			if (Type.GetTypeFromHandle(Type.GetTypeHandle(item)).IsSubclassOf(type))
				return true;

			return type.IsAssignableFrom(Type.GetTypeFromHandle(Type.GetTypeHandle(item)));
		}

		#endregion

		#region Fluent version of 'as'

		/// <summary>
		/// Fluent version of CSharp 'as' keyword
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="item">value to be casted</param>
		/// <returns>casted value</returns>
		public static T As<T>(this Object item)
		{
			if (item == null)
				return default(T);

			if (item is T)
				return (T)item;

			return default(T);
		}

		/// <summary>
		/// Fluent version of CSharp 'as' keyword applied to sequences.
		/// </summary>
		/// <typeparam name="TSource">Source sequence item type to cast from</typeparam>
		/// <typeparam name="TTarget">Target sequence item type to cast to</typeparam>
		/// <param name="source">The source sequence</param>
		/// <returns>The casted target sequence.</returns>
		public static IEnumerable<TTarget> AsSequence<TSource, TTarget>(this IEnumerable<TSource> source)
		{
			return source == null ? Enumerable.Empty<TTarget>() : source.Select(item => item.As<TTarget>());
		}

		/// <summary>
		/// Fluent version of CSharp 'as' keyword applied to sequences.
		/// </summary>
		/// <typeparam name="TTarget">Target sequence item type to cast to</typeparam>
		/// <param name="source">The source sequence</param>
		/// <returns>The casted target sequence.</returns>
		public static IEnumerable<TTarget> AsSequence<TTarget>(this IEnumerable source)
		{
			return source == null ? Enumerable.Empty<TTarget>() : source.OfType<TTarget>();
		}

		#endregion

		#region Fluent version of type casts

		/// <summary>
		/// Fluent version of type casts
		/// </summary>
		/// <typeparam name="TSource">Type to cast from</typeparam>
		/// <typeparam name="TTarget">Type to cast to</typeparam>
		/// <param name="item">value to be casted</param>
		/// <returns>The casted value</returns>
		public static TTarget Cast<TSource, TTarget>(this TSource item)
		{
			if (item.As<TTarget>().IsDefaultValue())
			{
				string sValue = item.ToInvariantString();
				var tVaule = sValue.FromInvariantString<TTarget>();

				return tVaule;
			}

			return item.As<TTarget>();
		}

		/// <summary>
		/// Fluent version of type casts for sequences
		/// </summary>
		/// <typeparam name="TSource">Source sequence item type to cast from</typeparam>
		/// <typeparam name="TTarget">Target sequence item type to cast to</typeparam>
		/// <param name="source">The source sequence</param>
		/// <returns>The casted target sequence (this will be a new sequence).</returns>
		public static IEnumerable<TTarget> CastSequence<TSource, TTarget>(this IEnumerable<TSource> source)
		{
			return source == null ? Enumerable.Empty<TTarget>() : 
									source.Select(item => item.Cast<TSource, TTarget>()).ToList();
		}

		#endregion

		#region Object factory methods

		///// <summary>
		///// Creates an instance of the given type
		///// </summary>
		///// <param name="type">Type to instantiate</param>
		///// <returns>Instance of the given Type</returns>
		//public static object New(this Type type)
		//{
		//    if (type == TypeOf.String)
		//        return "";
		//    if (type == typeof(Guid))
		//        return Guid.Empty;

		//    return (type == null) ? null : Activator.CreateInstance(type);
		//}


		#endregion

		#region With... methods

		/// <summary>
		/// Calls the action delegate with control as paramter and return control after the action call.
		/// </summary>
		/// <typeparam name="T">Type of control object</typeparam>
		/// <param name="control">The control object.</param>
		/// <param name="action">The action to apply.</param>
		/// <returns>The control object after action(control) was called.</returns>
		public static T With<T>(this T control, Action<T> action)
		{
			action(control);
			return control;
		}

		/// <summary>
		/// Calls action(disposable) and ensures disposing of disposable after that call.
		/// </summary>
		/// <typeparam name="T">Type of the disposable object.</typeparam>
		/// <param name="disposable">The disposable object.</param>
		/// <param name="action">The action that should be applied to disposable object.</param>
		public static void WithDispose<T>(this T disposable, Action<T> action) where T : IDisposable
		{
			using (disposable)
			{
				action(disposable);
			}
		}

		/// <summary>
		/// Calls action for each item in <paramref name="sequence"/> and ensures disposing each <paramref name="sequence"/> item after action was called.
		/// </summary>
		/// <typeparam name="T">Type of the disposable <paramref name="sequence"/> item.</typeparam>
		/// <param name="sequence">The disposable items sequence.</param>
		/// <param name="action">The action that should be applied to each disposable item inside <paramref name="sequence"/>.</param>
		public static void WithDispose<T>(this IEnumerable<T> sequence, Action<T> action) where T : IDisposable
		{
			foreach (var item in sequence)
			{
				using (item)
				{
					action(item);
				}
			}
		}

		/// <summary>
		/// Calls func(<paramref name="disposable"/>) and ensures disposing of <paramref name="disposable"/> after that call.
		/// </summary>
		/// <typeparam name="TDisposable">Type of the disposable object</typeparam>
		/// <typeparam name="TResult">Result type of func delegate.</typeparam>
		/// <param name="disposable">The disposable object.</param>
		/// <param name="func">The func delegate to apply on the disposable object.</param>
		/// <returns>Returns the result of func(<paramref name="disposable"/>)</returns>
		public static TResult WithDispose<TDisposable, TResult>(this TDisposable disposable, Func<TDisposable, TResult> func) where TDisposable : IDisposable
		{
			using (disposable)
			{
				return func(disposable);
			}
		}

		/// <summary>
		/// Calls func(<paramref name="disposable"/>, <paramref name="funcParam2"/>) and ensures disposing of <paramref name="disposable"/> after that call.
		/// </summary>
		/// <typeparam name="TDisposable">Type of the disposable object</typeparam>
		/// <typeparam name="TFunc2">Type of the second func parameter</typeparam>
		/// <typeparam name="TResult">Result type of func delegate.</typeparam>
		/// <param name="disposable">The disposable object.</param>
		/// <param name="funcParam2">The second func delegate argument.</param>
		/// <param name="func">The func delegate to apply.</param>
		/// <returns>Returns the result of func(<paramref name="disposable"/>, <paramref name="funcParam2"/>)</returns>
		public static TResult WithDispose<TDisposable, TFunc2, TResult>(this TDisposable disposable, TFunc2 funcParam2, Func<TDisposable, TFunc2, TResult> func) where TDisposable : IDisposable
		{
			using (disposable)
			{
				return func(disposable, funcParam2);
			}
		}

		#endregion

		#region Dispose helper methods

		/// <summary>
		/// <para>Disposes the specified object if the object has implemented <see cref="IDisposable"/>.</para>
		/// <para>If <paramref name="obj"/> has not implemented <see cref="IDisposable"/> or <paramref name="obj"/> is <see langword="null"/> nothing is done.</para>
		/// <para>If <paramref name="obj"/> is a COM-Object <see cref="Marshal.ReleaseComObject"/> is called.</para>
		/// </summary>
		/// <param name="obj">The obj.</param>
		public static void DisposeIfNecessary(this object obj)
		{
			TypeHelper.DisposeIfNecessary(obj);
		}

		/// <summary>
		/// <para>Disposes the elements of a sequence if the elements implemented <see cref="IDisposable"/>.</para>
		/// <para>If the <paramref name="sequence"/> items have not implemented <see cref="IDisposable"/> 
		/// or the <paramref name="sequence"/> is <see langword="null"/> nothing is done.</para>
		/// <para>If the <paramref name="sequence"/> items are COM-Objects <see cref="Marshal.ReleaseComObject"/> is called.</para>
		/// </summary>
		/// <param name="sequence">The sequence to dispose.</param>
		public static void DisposeElementsIfNecessary(this IEnumerable sequence)
		{
			TypeHelper.DisposeElementsIfNecessary(sequence);
		}

		#endregion

		#region Is... methods (Null, Empty, DefaultValue)

		/// <summary>
		/// Determines whether the specified instance is null. The method handels reference types and value types (value types always return false).
		/// </summary>
		/// <typeparam name="T">Type of the instance</typeparam>
		/// <param name="instance">The instance.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified instance is null; or <see langword="false"/> if T is a value type or T is not null.
		/// </returns>
		public static bool IsNull<T>(this T instance)
		{
			return !typeof(T).IsValueType && (Equals(instance, default(T)));
		}

		/// <summary>
		/// Determines whether the specified instance is not null. The method handels reference types and value types (value types always return true).
		/// </summary>
		/// <typeparam name="T">Type of the instance</typeparam>
		/// <param name="instance">The instance.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified instance is a value type or not null; or <see langword="false"/> if T is null.
		/// </returns>
		public static bool IsNotNull<T>(this T instance)
		{
			return (instance.IsNull() == false);
		}

		/// <summary>
		/// Determine if the array is null or empty.
		/// </summary>
		/// <typeparam name="T">The type of the array.</typeparam>
		/// <param name="array">The array to check.</param>
		/// <returns>Retruns true if array is null or empty; otherwise false.</returns>
		public static bool IsNullOrEmpty<T>(this T[] array)
		{
			if (array == null)
				return true;
			return array.Length == 0;
		}

		/// <summary>
		/// True if the given collection is not null and contains at least one element.
		/// </summary>
		/// <typeparam name="T">The collection item type.</typeparam>
		/// <param name="collection">The collection to check.</param>
		/// <returns><see langword="true"/> if the collection object is not empty; otherwise <see langword="false"/>.</returns>
		public static bool IsNotEmpty<T>(this ICollection<T> collection)
		{
			return collection != null && collection.GetEnumerator().MoveNext();
		}

		/// <summary>
		/// True if the given collection is null or contains no elements
		/// </summary>
		/// <typeparam name="T">The collection item type.</typeparam>
		/// <param name="collection">The collection to check.</param>
		/// <returns><see langword="true"/> if the collection object is empty or <see langword="null"/>; otherwise <see langword="false"/>.</returns>
		public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
		{
			return !IsNotEmpty(collection);
		}

		/// <summary>
		/// True if the given enumerable is not null and contains at least one element.
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The enumerable to check.</param>
		/// <returns><see langword="true"/> if the enumerable object is not empty; otherwise <see langword="false"/>.</returns>
		public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable != null && enumerable.GetEnumerator().MoveNext();
		}

		/// <summary>
		/// True if the given enumerable is null or contains no elements
		/// </summary>
		/// <typeparam name="T">The enumerable item type.</typeparam>
		/// <param name="enumerable">The enumerable to check.</param>
		/// <returns><see langword="true"/> if the enumerable object is empty or <see langword="null"/>; otherwise <see langword="false"/>.</returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return !IsNotEmpty(enumerable);
		}


		/// <summary>
		/// Gets whether the <paramref name="value" /> is the default value for this reference or value type.
		/// </summary>
		/// <returns>
		/// 	<see langword="true"/> if <paramref name="value" /> is the default value for this reference or value type; or <see langword="false"/> not.
		/// </returns>
		public static bool IsDefaultValue(this Object value)
		{
			if (ReferenceEquals(value, null))
			{
				return true;
			}

			Type type = value.GetType();

			if (type.IsValueType.IsFalse())
			{
				return false;
			}
			return Equals(Activator.CreateInstance(type), value);
		}

		/// <summary>
		/// Gets whether the <paramref name="value" /> is the default value for its reference or value type, or an empty string.
		/// </summary>
		/// <returns>
		/// 	<see langword="true"/> if <paramref name="value" /> is the default value for this reference or value type, or an empty string; otherwise <see langword="false"/>.
		/// </returns>
		public static bool IsDefaultValueOrEmptyString(this Object value)
		{
			if (ReferenceEquals(value, null))
			{
				return true;
			}

			Type type = value.GetType();

			if (type.IsValueType.IsFalse())
			{
				return ((value as string) == string.Empty);
			}
			return Equals(Activator.CreateInstance(type), value);
		}

		#endregion

		#region Object cloning methods

		/// <summary>
		/// Performs a deep clone on a object. Object must be serializable.
		/// </summary>
		/// <param name="original">Object that should be cloned.</param>
		/// <returns>Deep clone of original object</returns>
		public static T DeepClone<T>(this T original)
		{
			if (typeof(T).IsValueType == false)
			{
				if (Equals(original, default(T))) return default(T);
			}

			// Construct a temporary memory stream
			using (var stream = new MemoryStream())
			{
				// Construct a serialization formatter that does all the hard work
				var formatter = new BinaryFormatter();
				formatter.Context = new StreamingContext(StreamingContextStates.Clone);
				// Serialize the object graph into the memory stream
				formatter.Serialize(stream, original);
				// Seek back to the start of the memory stream before deserializing
				stream.Position = 0;
				// Deserialize the graph into a new set of objects and
				// return the root of the graph (deep copy) to the caller
				return (T)formatter.Deserialize(stream);
			}
		}

		#endregion

	}
}
