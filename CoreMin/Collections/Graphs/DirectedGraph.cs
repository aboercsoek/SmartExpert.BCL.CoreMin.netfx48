//--------------------------------------------------------------------------
// File:    DirectedGraph.cs
// Content:	Implementation of class DirectedGraph
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SmartExpert.Collections.Graphs
{
	/// <summary>
	/// A directed graph
	/// </summary>
	/// <typeparam name="TVertex"></typeparam>
	/// <typeparam name="TEdge"></typeparam>
	public class DirectedGraph<TVertex, TEdge> where TEdge : IEdge<TVertex>
	{
		private readonly IEqualityComparer<TEdge> m_EdgeComparer;
		private int m_EdgeCount;
		private readonly Dictionary<TVertex, HashSet<TEdge>> m_VertedEdges;

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectedGraph{TVertex,TEdge}" /> class.
		/// </summary>
		public DirectedGraph()
			: this(0, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectedGraph{TVertex,TEdge}" /> class.
		/// </summary>
		/// <param name="vertexCapacity">The vertex capacity.</param>
		/// <param name="vertexComparer">The vertex comparer.</param>
		/// <param name="edgeComparer">The edge comparer.</param>
		public DirectedGraph(int vertexCapacity, IEqualityComparer<TVertex> vertexComparer, IEqualityComparer<TEdge> edgeComparer)
		{
			if (vertexComparer == null)
			{
				vertexComparer = EqualityComparer<TVertex>.Default;
			}
			if (edgeComparer == null)
			{
				edgeComparer = EqualityComparer<TEdge>.Default;
			}
			m_VertedEdges = new Dictionary<TVertex, HashSet<TEdge>>(vertexCapacity, vertexComparer);
			m_EdgeComparer = edgeComparer;
		}

		/// <summary>
		/// Adds the edge.
		/// </summary>
		/// <param name="edge">The edge.</param>
		/// <returns></returns>
		public bool AddEdge(TEdge edge)
		{
			HashSet<TEdge> set;
			bool flag = false;
			if (m_VertedEdges.TryGetValue(edge.Source, out set))
			{
				flag = set.Add(edge);
			}
			if (flag)
			{
				m_EdgeCount++;
			}
			return flag;
		}

		/// <summary>
		/// Adds the edges.
		/// </summary>
		/// <param name="edges">The edges.</param>
		public void AddEdgeRange(IEnumerable<TEdge> edges)
		{
			foreach (TEdge local in edges)
			{
				AddEdge(local);
			}
		}

		/// <summary>
		/// Adds the vertex.
		/// </summary>
		/// <param name="vertex">The vertex.</param>
		/// <returns></returns>
		public bool AddVertex(TVertex vertex)
		{
			if (m_VertedEdges.ContainsKey(vertex))
			{
				return false;
			}
			m_VertedEdges.Add(vertex, new HashSet<TEdge>(m_EdgeComparer));
			return true;
		}

		/// <summary>
		/// Adds the vertex range.
		/// </summary>
		/// <param name="vertices">The vertices.</param>
		/// <returns>number of vertices added</returns>
		public int AddVertexRange(IEnumerable<TVertex> vertices)
		{
			int num = 0;
			foreach (TVertex local in vertices)
			{
				HashSet<TEdge> set;
				if (!m_VertedEdges.TryGetValue(local, out set))
				{
					m_VertedEdges.Add(local, new HashSet<TEdge>(m_EdgeComparer));
					num++;
				}
			}
			return num;
		}

		/// <summary>
		/// Gets a value indicating if the graph contains a particular vertex
		/// </summary>
		/// <param name="vertex"></param>
		/// <returns></returns>
		public bool ContainsVertex(TVertex vertex)
		{
			return m_VertedEdges.ContainsKey(vertex);
		}

		/// <summary>
		/// Gets the number of out edges from a vertex
		/// </summary>
		/// <param name="vertex">The vertex.</param>
		/// <returns></returns>
		public int OutEdgeCount(TVertex vertex)
		{
			return m_VertedEdges[vertex].Count;
		}

		/// <summary>
		/// Gets the out edge of a vertex
		/// </summary>
		/// <param name="vertex">The vertex.</param>
		/// <returns></returns>
		public IEnumerable<TEdge> OutEdges(TVertex vertex)
		{
			return m_VertedEdges[vertex];
		}

		/// <summary>
		/// Gets the edge count.
		/// </summary>
		/// <value>The edge count.</value>
		public int EdgeCount
		{
			get
			{
				return m_EdgeCount;
			}
		}

		/// <summary>
		/// Gets the edges.
		/// </summary>
		/// <value>The edges.</value>
		public IEnumerable<TEdge> Edges
		{
			get
			{
				//List<TEdge> edges = new List<TEdge>();
				//foreach (var vertedEdge in m_VertedEdges)
				//{
				//    edges.AddRange(vertedEdge.Value);
				//}
				return m_VertedEdges.SelectMany(verted => verted.Value);
			}
		}

		/// <summary>
		/// Gets the vertex count.
		/// </summary>
		/// <value>The vertex count.</value>
		public int VertexCount
		{
			get
			{
				return m_VertedEdges.Count;
			}
		}

		/// <summary>
		/// Gets the vertices.
		/// </summary>
		/// <value>The vertices.</value>
		public IEnumerable<TVertex> Vertices
		{
			get
			{
				return m_VertedEdges.Keys;
			}
		}

	}
}
