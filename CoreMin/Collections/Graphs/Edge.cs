//--------------------------------------------------------------------------
// File:    Edge.cs
// Content:	Implementation of class Edge
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;

#endregion

namespace SmartExpert.Collections.Graphs
{
	/// <summary>
	/// The IEdge implementation
	/// </summary>
	/// <typeparam name="TVertex">The vertex type</typeparam>
	[DebuggerDisplay("{source} -> {target}")]
	public class Edge<TVertex> : IEdge<TVertex>
	{
		private readonly TVertex m_Source;
		private readonly TVertex m_Target;

		/// <summary>
		/// Initalizes a new instance of the edge
		/// </summary>
		/// <param name="source">The source of the edge.</param>
		/// <param name="target">The target of the edge.</param>
		public Edge(TVertex source, TVertex target)
		{
			m_Source = source;
			m_Target = target;
		}

		/// <summary>
		/// Gets a string representation of the edge
		/// </summary>
		/// <returns>{Source} -> {Target}</returns>
		public override string ToString()
		{
			return String.Format("{0} -> {1}", m_Source, m_Target);
		}

		/// <summary>
		/// Gets the edge source
		/// </summary>
		/// <value>The source vertex of the edge.</value>
		public TVertex Source
		{
			get
			{
				return m_Source;
			}
		}

		/// <summary>
		/// Gets the edge target
		/// </summary>
		/// <value>The target vertex of the edge.</value>
		public TVertex Target
		{
			get
			{
				return m_Target;
			}
		}
	}
}
