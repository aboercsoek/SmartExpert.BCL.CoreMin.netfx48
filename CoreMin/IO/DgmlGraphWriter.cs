 //--------------------------------------------------------------------------
// File:    DgmlGraphWriter.cs
// Content:	Implementation of class DgmlGraphWriter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
 using System;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.Globalization;
 using System.Xml;
 using SmartExpert.Collections.Graphs;

#endregion

namespace SmartExpert.IO
{

	/// <summary>
	/// A DGML graph writer
	/// </summary>
	/// <remarks>
	/// More information on DGML at http://msdn.microsoft.com/en-us/library/ee842619(VS.100).aspx
	/// </remarks>
	public sealed class DgmlGraphWriter : IDisposable
	{
		#region Private Members
	
		private Dictionary<string, int> m_Aliases;
		private XmlTextWriter m_Writer;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes the graph writer
		/// </summary>
		/// <param name="writer"></param>
		private DgmlGraphWriter(XmlTextWriter writer)
		{
			m_Writer = writer;
			//m_Writer.HtmlEscaping = true;
		}

		#endregion

		#region Events
	
		/// <summary>
		/// Raised when closed
		/// </summary>
		public event EventHandler<EventArgs> Closed;

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates a new DGML writer
		/// </summary>
		/// <param name="writer">The xml text writer to use.</param>
		/// <returns>The created DGML writer instance.</returns>
		public static DgmlGraphWriter Create(XmlTextWriter writer)
		{
			return new DgmlGraphWriter(writer);
		}

		/// <summary>
		/// Closes the writer
		/// </summary>
		public void Close()
		{
			XmlTextWriter writer = m_Writer;
			if (writer != null)
			{
				writer.Close();
				m_Writer = null;
			}
			EventHandler<EventArgs> closed = Closed;
			if (closed != null)
			{
				closed(this, EventArgs.Empty);
			}
			Closed = null;
		}
		

		/// <summary>
		/// Defines an alias for the avlue
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The alias of the value.</returns>
		public string DefineAlias(string value)
		{
			int num;
			if (m_Aliases == null)
			{
				m_Aliases = new Dictionary<string, int>(StringComparer.Ordinal);
			}
			if (!m_Aliases.TryGetValue(value, out num))
			{
				m_Aliases[value] = num = m_Aliases.Count;
			}
			return FormatAlias(num);
		}

		/// <summary>
		/// Disposes the graph writer
		/// </summary>
		public void Dispose()
		{
			Close();
		}

		/// <summary>
		/// Writes the attribue pair
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		public void WriteAttributeString(string name, string value)
		{
			m_Writer.WriteAttributeString(name, value);
		}

		/// <summary>
		/// Writes the category attribute
		/// </summary>
		/// <param name="category">The category name.</param>
		public void WriteCategoryAttribute(string category)
		{
			WriteAttributeString("Category", category);
		}

		/// <summary>
		/// Writes a condition element
		/// </summary>
		/// <param name="expression">The expression text of the condition element.</param>
		public void WriteCondition(string expression)
		{
			m_Writer.WriteStartElement("Condition");
			m_Writer.WriteAttributeString("Expression", expression);
			m_Writer.WriteEndElement();
			m_Writer.Flush();
		}

		/// <summary>
		/// Writes a link that specifes that a group contains a node
		/// </summary>
		/// <param name="group">The group name.</param>
		/// <param name="target">The target node name.</param>
		public void WriteContainsLink(string group, string target)
		{
			WriteStartLink(group, target);
			WriteCategoryAttribute("Contains");
			WriteEndLink();
		}

		/// <summary>
		/// Writes the end of the categories
		/// </summary>
		public void WriteEndCategories()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the end of a Category element
		/// </summary>
		public void WriteEndCategory()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Closes the 'DirectedGraph' element
		/// </summary>
		public void WriteEndDirectedGraph()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Ends a Link element
		/// </summary>
		public void WriteEndLink()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Closes the 'Nodes' element
		/// </summary>
		public void WriteEndLinks()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Ends a node element
		/// </summary>
		public void WriteEndNode()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Closes the 'Nodes' element
		/// </summary>
		public void WriteEndNodes()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the end of a Properties element
		/// </summary>
		public void WriteEndProperties()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the end of the style section
		/// </summary>
		public void WriteEndStyle()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the end of the style section
		/// </summary>
		public void WriteEndStyles()
		{
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes a Group attribute
		/// </summary>
		/// <param name="expanded">if set to true the group is in the expanded state; otherwise in the collapsed state.</param>
		public void WriteGroupAttribute(bool expanded)
		{
			WriteAttributeString("Group", expanded ? "Expanded" : "Collapsed");
		}

		/// <summary>
		/// Writes the label attribute
		/// </summary>
		/// <param name="label">The label text.</param>
		public void WriteLabelAttribute(string label)
		{
			WriteAttributeString("Label", label);
		}

		/// <summary>
		/// Writes the nodes and links of the graph
		/// </summary>
		/// <typeparam name="TVertex">Vertex type.</typeparam>
		/// <typeparam name="TEdge">Edge type.</typeparam>
		/// <param name="graph">The directed graph object.</param>
		/// <param name="vertexIdentities">Vertex to string converter function delegate.</param>
		/// <param name="vertexWriter">Vertex writer action delegate.</param>
		/// <param name="nodeWriter">additional node writer</param>
		/// <param name="edgeWriter">Edge writer action delegate.</param>
		/// <param name="linkWriter">additional link writer</param>
		public void WriteNodesAndLinks<TVertex, TEdge>(
						DirectedGraph<TVertex, TEdge> graph, 
						Func<TVertex, string> vertexIdentities, 
						Action<DgmlGraphWriter, TVertex> vertexWriter, 
						Action<DgmlGraphWriter, DirectedGraph<TVertex, TEdge>> nodeWriter, 
						Action<DgmlGraphWriter, TEdge> edgeWriter, 
						Action<DgmlGraphWriter, DirectedGraph<TVertex, TEdge>> linkWriter) where TEdge: IEdge<TVertex>
		{
			Dictionary<TVertex, string> dictionary = new Dictionary<TVertex, string>();
			WriteStartNodes();
			foreach (TVertex vertex in graph.Vertices)
			{
				string str;
				dictionary[vertex] = str = vertexIdentities(vertex);
				WriteStartNode(str);
				if (vertexWriter != null)
				{
					vertexWriter(this, vertex);
				}
				WriteEndNode();
			}
			if (nodeWriter != null)
			{
				nodeWriter(this, graph);
			}
			WriteEndNodes();
			WriteStartLinks();
			foreach (TEdge edge in graph.Edges)
			{
				WriteStartLink(dictionary[edge.Source], dictionary[edge.Target]);
				if (edgeWriter != null)
				{
					edgeWriter(this, edge);
				}
				WriteEndLink();
			}
			if (linkWriter != null)
			{
				linkWriter(this, graph);
			}
			WriteEndLinks();
		}

		/// <summary>
		/// Writes a property definition
		/// </summary>
		/// <param name="id">The property id.</param>
		/// <param name="label">The property label text.</param>
		/// <param name="description">The property description text.</param>
		/// <param name="dataType">The type code of the property.</param>
		public void WriteProperty(string id, string label, string description, TypeCode dataType)
		{
			m_Writer.WriteStartElement("Property");
			m_Writer.WriteAttributeString("Id", id);
			m_Writer.WriteAttributeString("Label", label);
			m_Writer.WriteAttributeString("Description", description);
			m_Writer.WriteAttributeString("DataType", dataType.ToString());
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes a property setter
		/// </summary>
		/// <param name="property">The property name.</param>
		/// <param name="expression">The property setter expression string.</param>
		public void WriteSetterExpression(string property, string expression)
		{
			m_Writer.WriteStartElement("Setter");
			m_Writer.WriteAttributeString("Property", property);
			m_Writer.WriteAttributeString("Expression", expression);
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes a property setter
		/// </summary>
		/// <param name="property">The property name.</param>
		/// <param name="value">The property value.</param>
		public void WriteSetterValue(string property, string value)
		{
			m_Writer.WriteStartElement("Setter");
			m_Writer.WriteAttributeString("Property", property);
			m_Writer.WriteAttributeString("Value", value);
			m_Writer.WriteEndElement();
		}

		/// <summary>
		/// Writes the start of the categories
		/// </summary>
		public void WriteStartCategories()
		{
			m_Writer.WriteStartElement("Categories");
		}

		/// <summary>
		/// Writes a Category element
		/// </summary>
		/// <param name="id"></param>
		/// <param name="label"></param>
		public void WriteStartCategory(string id, string label)
		{
			m_Writer.WriteStartElement("Category");
			m_Writer.WriteAttributeString("Id", id);
			m_Writer.WriteAttributeString("Label", label);
		}

		/// <summary>
		/// Opens the 'DirectedGraph' element
		/// </summary>
		/// <param name="title">The title of the directed graph.</param>
		public void WriteStartDirectedGraph(string title)
		{
			m_Writer.WriteStartElement("DirectedGraph");
			m_Writer.WriteAttributeString("Title", title);
			m_Writer.WriteAttributeString("xmlns", "http://schemas.microsoft.com/vs/2009/dgml");
		}

		/// <summary>
		/// Starts a named Link element.
		/// </summary>
		/// <param name="source">The link source.</param>
		/// <param name="target">The link target.</param>
		public void WriteStartLink(string source, string target)
		{
			m_Writer.WriteStartElement("Link");
			m_Writer.WriteAttributeString("Source", DefineAlias(source));
			m_Writer.WriteAttributeString("Target", DefineAlias(target));
		}

		/// <summary>
		/// Opens the 'Links' element
		/// </summary>
		public void WriteStartLinks()
		{
			m_Writer.WriteStartElement("Links");
		}

		/// <summary>
		/// Starts a node element
		/// </summary>
		/// <param name="id">The node id.</param>
		public void WriteStartNode(string id)
		{
			m_Writer.WriteStartElement("Node");
			m_Writer.WriteAttributeString("Id", DefineAlias(id));
			m_Writer.WriteAttributeString("Label", FormatLabel(id));
		}

		/// <summary>
		/// Opens the 'Nodes' element
		/// </summary>
		public void WriteStartNodes()
		{
			m_Writer.WriteStartElement("Nodes");
		}

		/// <summary>
		/// Writes the start of a 'Properties' element
		/// </summary>
		public void WriteStartProperties()
		{
			m_Writer.WriteStartElement("Properties");
		}

		/// <summary>
		/// Writes the start of a style section
		/// </summary>
		/// <param name="targetType">The DGML target style type.</param>
		/// <param name="groupLabel">The group label.</param>
		/// <param name="valueLabel">The value label.</param>
		public void WriteStartStyle(DgmlStyleTargetType targetType, string groupLabel, string valueLabel)
		{
			m_Writer.WriteStartElement("Style");
			m_Writer.WriteAttributeString("TargetType", targetType.ToString());
			m_Writer.WriteAttributeString("GroupLabel", groupLabel);
			m_Writer.WriteAttributeString("ValueLabel", valueLabel);
		}

		/// <summary>
		/// Writes the default style section.
		/// </summary>
		public void WriteStartStyles()
		{
			m_Writer.WriteStartElement("Styles");
			WriteStartStyle(DgmlStyleTargetType.Node, "Methods", "Methods");
			WriteSetterValue("NodeRadius", "1");
			WriteSetterValue("Style", "Plain");
			WriteSetterValue("Background", "GhostWhite");
			WriteSetterValue("ShadowDepth", "2");
			WriteEndStyle();
		}

		#endregion

		#region Private Methods

		[Conditional("DEBUG")]
		// ReSharper disable UnusedMember.Local
		private void EnsureWriter()
		// ReSharper restore UnusedMember.Local
		{
		}

		private static string FormatAlias(int alias)
		{
			return alias.ToString(CultureInfo.InvariantCulture);
		}

		private static string FormatLabel(string label)
		{
			return StringHelper.Join("\n", StringHelper.SquareChunk(label, new[] { '.', ',', '+', '_', '<', '(' }));
		}

		#endregion
	}
}

