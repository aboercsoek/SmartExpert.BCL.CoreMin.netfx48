//--------------------------------------------------------------------------
// File:    IEdge.cs
// Content:	Definition of interface IEdge
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;

#endregion

namespace SmartExpert.Collections.Graphs
{
	/// <summary>
	/// A directed graph edge
	/// </summary>
	/// <typeparam name="TVertex">The vertex type.</typeparam>
	public interface IEdge<TVertex>
	{
		/// <summary>
		/// Gets the source vertex
		/// </summary>
		/// <value>The source.</value>
		TVertex Source { get; }

		/// <summary>
		/// Gets the target vertex
		/// </summary>
		/// <value>The target.</value>
		TVertex Target { get; }
	}
}
