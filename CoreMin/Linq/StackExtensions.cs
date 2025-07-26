//--------------------------------------------------------------------------
// File:    StackExtensions.cs
// Content:	Implementation of class StackExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as Stack<T> therewith the extension methods are available by using the System.Collections.Generic namespace.
namespace SmartExpert.Linq
{
	///<summary>Represents extension methods for <see cref="Stack{T}"/> type.</summary>
	public static class StackExtensions
	{
		/// <summary>
		/// Pushes a range of items.
		/// </summary>
		/// <typeparam name="T">The item type.</typeparam>
		/// <param name="stack">The stack.</param>
		/// <param name="items">The items.</param>
		/// <exception cref="ArgNullException">If <paramref name="stack"/> is <see langword="null"/>.</exception>
		public static void PushRange<T>( this Stack<T> stack, IEnumerable<T> items )
		{
			ArgChecker.ShouldNotBeNull(stack, "stack");

			items.Foreach(stack.Push);
		}

	}
}
