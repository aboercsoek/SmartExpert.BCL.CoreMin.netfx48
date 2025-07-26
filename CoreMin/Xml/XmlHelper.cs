//--------------------------------------------------------------------------
// File:    XmlHelper.cs
// Content:	Implementation of class XmlHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Collections.Generic;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Xml
{
	/// <summary>
	/// Die Hilfsklasse XmlHelper stellt Methoden für den Umgang mit XML Dokumenten und XML Elementen zur Verfügung,
	/// sowie Basismethoden für XSL Transformationen.
	/// </summary>
	public static class XmlHelper
	{
		/// <summary>
		/// XML text encoding.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>XML encoded string.</returns>
		public static string XmlEncode(string text)
		{
			ArgChecker.ShouldNotBeNull(text, "text");

			string element = new XElement("t", text).ToString(SaveOptions.DisableFormatting);
			element = element.Substring(3,element.Length-7);

			return element;
		}

		/// <summary>
		/// Loads an embedded XmlDocument resource as stream and returns the stream.
		/// Trys to get the XmlDocument from the resources of the calling assembly
		/// and if not found from the resources of the entry assembly.
		/// </summary>
		/// <param name="xmlDocName">Name of the embedded XML file.</param>
		/// <returns>XmlDocument stream if found; otherwise <see langword="null"/>.</returns>
		public static Stream CreateXmlDocumentStreamFromEmbeddedResource(string xmlDocName)
		{
			Stream xmlStream = null;

			Assembly callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

			if (resName != null)
			{
				// get the resource into a stream
				xmlStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xmlStream == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

				if (resName != null)
					// get the resource into a stream
					xmlStream = entryAssembly.GetManifestResourceStream(resName);
			}

			return xmlStream;
		}

		/// <summary>
		/// Create a XmlDocument instance from a embedded resource
		/// </summary>
		/// <param name="xmlDocName">Name of the embedded XML file.</param>
		/// <returns>XmlDocument instance or <see langword="null"/> if not found.</returns>
		public static XmlDocument CreateXmlDocumentFromEmbeddedResource(string xmlDocName)
		{
			Stream xmlStream = null;

			Assembly callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

			if (resName != null)
			{
				// get the resource into a stream
				xmlStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xmlStream == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

				if (resName != null)
					// get the resource into a stream
					xmlStream = entryAssembly.GetManifestResourceStream(resName);
			}

			if (xmlStream == null) return null;

			try
			{
				var xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlStream);
				return xmlDocument;
			}
			finally
			{
				xmlStream.Dispose();
			}
		}

		/// <summary>
		/// Create a XDocument instance from a embedded resource
		/// </summary>
		/// <param name="xmlDocName">Name of the embedded XML file.</param>
		/// <returns>XDocument instance or <see langword="null"/> if not found.</returns>
		public static XDocument CreateXDocumentFromEmbeddedResource(string xmlDocName)
		{
			Stream xmlStream = null;

			Assembly callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

			if (resName != null)
			{
				// get the resource into a stream
				xmlStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xmlStream == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlDocName));

				if (resName != null)
					// get the resource into a stream
					xmlStream = entryAssembly.GetManifestResourceStream(resName);
			}

			if (xmlStream == null) return null;

			try
			{
				var xmlDocument = XDocument.Load(new XmlTextReader(xmlStream));
				return xmlDocument;
			}
			finally
			{
				xmlStream.Dispose();
			}
		}

		/// <summary>
		/// Walks through the specified XML node.
		/// </summary>
		/// <param name="node">The XML node to walk through.</param>
		/// <returns>A IEnumerable-Collection of XmlNode walk events.</returns>
		public static IEnumerable<TreeWalker.WalkEvent<XmlNode>> Walk( XmlNode node )
		{
			return TreeWalker.Walk( node,
									n => n.NodeType == XmlNodeType.Element,
									n => n.ChildNodes.AsEnumerable());
		}

		/// <summary>
		/// Validates a XAML document.
		/// </summary>
		/// <param name="filename">The XAML document filename.</param>
		/// <returns><see langword="true"/> if the file is a valid XAML document; otherwise <see langword="false"/>.</returns>
		public static bool ValidateXaml( string filename )
		{
			FileStream stream = null;
			object xamlInstance = null;
			try
			{
				var info = new FileInfo(filename);
				string baseUri = info.DirectoryName + @"\";
				stream = new FileStream(filename, FileMode.Open);
				xamlInstance = XamlReader.Load(XmlReader.Create(stream, null, baseUri));

			}
			catch ( Exception ex )
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if ( stream != null )
					stream.Close();
			}
			return ( xamlInstance != null );
		}
	}
}
