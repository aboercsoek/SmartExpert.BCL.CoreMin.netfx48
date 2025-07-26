//--------------------------------------------------------------------------
// File:    TreeWalker.cs
// Content:	Implementation of class TreeWalker
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

namespace SmartExpert.Collections.Generic
{
	/// <summary>
	/// The tree walker class.
	/// </summary>
	public static class TreeWalker
	{
		#region Nested Types

		/// <summary>
		/// The walk event type enumeration
		/// </summary>
		public enum WalkEventType
		{
			/// <summary>WalkEvent: Enter container</summary>
			EnterContainer,
			/// <summary>WalkEvent: Exit container</summary>
			ExitContainer,
			/// <summary>WalkEvent: Handle container item.</summary>
			HandleItem
		};


		/// <summary>
		/// The tree walker event structure.
		/// </summary>
		/// <typeparam name="T">The tree node type.</typeparam>
		public struct WalkEvent<T>
		{
			/// <summary>The walk event type</summary>
			public readonly WalkEventType EventType;
			/// <summary>The tree node associated with this walk event.</summary>
			public readonly T Node;

			/// <summary>
			/// Initializes a new instance of the <see cref="WalkEvent{T}"/> struct.
			/// </summary>
			/// <param name="node">The node.</param>
			/// <param name="eventType">The event_type.</param>
			public WalkEvent( T node, WalkEventType eventType )
			{
				Node = node;
				EventType = eventType;
			}

			/// <summary>
			/// Gets a value indicating whether this is a EnterContainer walk event.
			/// </summary>
			/// <value>
			/// 	<see langword="true"/> if this walk event is a EnterContainer event; otherwise, <see langword="false"/>.
			/// </value>
			public bool HasEnteredContainer
			{
				get { return EventType == WalkEventType.EnterContainer; }
			}

			/// <summary>
			/// Gets a value indicating whether this is a ExitContainer walk event.
			/// </summary>
			/// <value>
			/// 	<see langword="true"/> if this walk event is a ExitContainer event; otherwise, <see langword="false"/>.
			/// </value>
			public bool HasExitedContainer
			{
				get { return EventType == WalkEventType.ExitContainer; }
			}

			/// <summary>
			/// Gets a value indicating whether this is a HandleItem walk event.
			/// </summary>
			/// <value>
			/// 	<see langword="true"/> if this walk event is a HandleItem event; otherwise, <see langword="false"/>.
			/// </value>
			public bool HasHandledNonContainer
			{
				get { return EventType == WalkEventType.HandleItem; }
			}
		}


		/// <summary>
		/// WalkState is an internal struct used when traversing the DOM.
		/// It preserves the state in the non-recursive, stack-based traversal of the DOM.
		/// </summary>
		/// <typeparam name="T">The tree node type</typeparam>
		private struct WalkState<T>
		{
			internal readonly T Node;
			internal bool Entered;
			internal IEnumerator<T> ChildEnumerator;

			public WalkState( T node, IEnumerator<T> e )
			{
				Node = node;
				Entered = false;
				ChildEnumerator = e;
			}
		}

		#endregion

		#region Delegates

		/// <summary>
		/// The child enumerator delegate.
		/// </summary>
		/// <typeparam name="T">The enumerator item type.</typeparam>
		/// <param name="item">The parent item.</param>
		/// <returns>The child items of <paramref name="item"/></returns>
		public delegate IEnumerable<T> ChildEnumerator<T>( T item );

		/// <summary>
		/// The is container delegate.
		/// </summary>
		/// <typeparam name="T">The container item type</typeparam>
		/// <param name="item">The parent item.</param>
		/// <returns>Returns true if item has child items.</returns>
		public delegate bool IsContainer<T>( T item );

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Walks through a specified node.
		/// </summary>
		/// <typeparam name="T">The tree node type.</typeparam>
		/// <param name="node">The tree node to walk through.</param>
		/// <param name="enumChildren">The child enumerator method.</param>
		/// <returns>A IEnumerable-Collection of WalkEvents.</returns>
		public static IEnumerable<WalkEvent<T>> Walk<T>( T node, ChildEnumerator<T> enumChildren )
		{
			return Walk(node, n => true, enumChildren);
		}


		/// <summary>
		/// Walks through a specified node.
		/// </summary>
		/// <typeparam name="T">The tree node type.</typeparam>
		/// <param name="node">The tree node to walk through.</param>
		/// <param name="isContainer">The is container predicate.</param>
		/// <param name="enumChildren">The child enumerator method.</param>
		/// <returns>A IEnumerable-Collection of WalkEvents.</returns>
		/// <remarks>
		/// Walks a Node in a depth-first manner without recursion.
		/// It returns a series of object that indicate one of three things:
		/// <list type="bullet">
		///		<item>
		///		<description>whether it has gone "down" into an element</description></item>
		///		<item>
		///		<description>it is processing a non-element child of an element</description></item>
		///		<item>
		///		<description>whether it has gone "up" from an element (i.e. it is finished with that element and its descendants)</description></item>
		///	</list>
		/// </remarks>
		public static IEnumerable<WalkEvent<T>> Walk<T>( T node, IsContainer<T> isContainer, ChildEnumerator<T> enumChildren )
		{
			var walkStack = new Stack<WalkState<T>>();
		    walkStack.PushRange(null);

			var childEnumerator = enumChildren(node).GetEnumerator();

			// put the first item on the stack 
			walkStack.Push(new WalkState<T>(node, childEnumerator));

			// traverse down the tree using a stack. As long as something is on the stack, we are not done
			while ( walkStack.Count > 0 )
			{
				var curItem = walkStack.Pop();

				if ( curItem.Entered == false )
				{
					curItem.Entered = true;

					var walkEvent = new WalkEvent<T>(curItem.Node, WalkEventType.EnterContainer);
					yield return walkEvent;
				}

				bool b = curItem.ChildEnumerator.MoveNext();
				if ( b )
				{
					T childNode = curItem.ChildEnumerator.Current;

					if ( isContainer(childNode) )
					{
						// this Node may contain more nodes so at this point we explore it's children
						walkStack.Push(curItem);
						walkStack.Push(new WalkState<T>(childNode, enumChildren(childNode).GetEnumerator()));
					}
					else
					{
						var walkEvent = new WalkEvent<T>(childNode, WalkEventType.HandleItem);
						yield return walkEvent;
						walkStack.Push(curItem);
					}
				}
				else
				{
					curItem.ChildEnumerator.Dispose();

					// all the child nodes have been processed, now finish with this Node
					if ( !isContainer(curItem.Node) )
					{
						string msg = string.Format("internal error: only container nodes should be returned, instead found {0}", curItem.Node);
						throw new InvalidOperationException(msg);
					}

					var walkEvent = new WalkEvent<T>(curItem.Node, WalkEventType.ExitContainer);
					yield return walkEvent;
				}
			}
		}

		#endregion
	}
}
