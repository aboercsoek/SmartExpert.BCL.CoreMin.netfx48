//--------------------------------------------------------------------------
// File:    TypeHelper.cs
// Content:	Implementation of class TypeHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// ReSharper disable CheckNamespace
namespace SmartExpert
{
// ReSharper restore CheckNamespace

	/// <summary>
	/// Provides common type helper methods.
	/// </summary>
	public static class TypeHelper
	{
		#region DeepClone helper methods

		/// <summary>
		/// Deep object clone.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <returns>The clone.</returns>
		public static object DeepClone(object original)
		{
			if (original.IsDefaultValue())
				return original;

			if (original.GetType().IsSerializable.IsFalse())
				throw new ArgException<Object>(original, "original", "The argument original is not serializable.");
	
			// Construct a temporary memory stream
			using (var stream = new MemoryStream())
			{
				// Construct a serialization formatter that does all the hard work
				var formatter = new BinaryFormatter
									{
										Context = new StreamingContext(StreamingContextStates.Clone)
									};
				// This line is explained in this chapter's "Streaming Contexts" section
				// Serialize the object graph into the memory stream
				formatter.Serialize(stream, original);
				// Seek back to the start of the memory stream before deserializing
				stream.Position = 0;
				// Deserialize the graph into a new set of objects and
				// return the root of the graph (deep copy) to the caller
				return formatter.Deserialize(stream);
			}

		}

		/// <summary>
		/// Deep object clone.
		/// </summary>
		/// <typeparam name="T">The type of the object that should be cloned.</typeparam>
		/// <param name="original">The original.</param>
		/// <returns>The clone.</returns>
		public static T DeepClone<T>(T original)
		{
			if (original.IsDefaultValue())
				return original;

			if (original.GetType().IsSerializable.IsFalse())
				throw new ArgException<T>(original, "original", "The argument original is not serializable.");

			// Construct a temporary memory stream
			using (var stream = new MemoryStream())
			{
				// Construct a serialization formatter that does all the hard work
				var formatter = new BinaryFormatter
									{
										Context = new StreamingContext(StreamingContextStates.Clone)
									};
				// This line is explained in this chapter's "Streaming Contexts" section
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

		#region Dispose helper methods

		/// <summary>
		/// Disposes the specified obj if the object has implemented <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="obj">The object that should be disposed.</param>
		public static void DisposeIfNecessary(object obj)
		{
			if (obj == null)
				return;

			if (obj.GetType().IsValueType)
				return;

			if (obj.GetType().IsCOMObject)
			{
				Marshal.ReleaseComObject(obj);
				return;
			}

			var id = obj as IDisposable;
			if (id != null)
				id.Dispose();
		}

		/// <summary>
		/// Disposes the elements of the sequence if the elements implemented <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="enumerable">The sequence of elements that should be disposed.</param>
		[DebuggerStepThrough]
		public static void DisposeElementsIfNecessary(IEnumerable enumerable)
		{
			if (enumerable == null)
				return;
			IEnumerator iter = enumerable.GetEnumerator();
			while (iter.MoveNext())
				DisposeIfNecessary(iter.Current);
		}

		/// <summary>
		/// Disposes the values of a dictionary if the type of the values implement <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="dict">The dictionary who's value items should be disposed.</param>
		[DebuggerStepThrough]
		public static void DisposeValuesIfNecessary(IDictionary dict)
		{
			if (dict == null)
				return;
			IDictionaryEnumerator iter = dict.GetEnumerator();
			while (iter.MoveNext())
				DisposeIfNecessary(iter.Value);
		}

		#endregion

	}
}
