//--------------------------------------------------------------------------
// File:    QueueExtensions.cs
// Content:	Implementation of class QueueExtensions
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

// Uses the same namespace as Queue therewith the extension methods are available by using the System.Collections.Generic namespace.
namespace SmartExpert.Linq
{
	///<summary>Represents extension methods for <see cref="Queue{T}"/> type.</summary>
	public static class QueueExtensions
	{
		/// <summary>
		/// Enqueues the range of items.
		/// </summary>
		/// <typeparam name="T">The item type.</typeparam>
		/// <param name="queue">The queue.</param>
		/// <param name="items">The items.</param>
		/// <exception cref="ArgNullException"><paramref name="queue"/> is <see langword="null"/>.</exception>
		public static void EnqueueRange<T>( this Queue<T> queue, IEnumerable<T> items )
		{
			ArgChecker.ShouldNotBeNull(queue, "queue");

			items.Foreach(queue.Enqueue);
		}

	}
}
