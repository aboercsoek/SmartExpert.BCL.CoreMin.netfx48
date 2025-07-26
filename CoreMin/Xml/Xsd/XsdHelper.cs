//--------------------------------------------------------------------------
// File:    XsdHelper.cs
// Content:	Implementation of class XsdHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Xml.Xsd
{
	/// <summary>
	/// Die Hilfsklasse XsdHelper stellt Methoden für den Umgang mit XML Dokumenten und XML Elementen zur Verfügung,
	/// sowie Basismethoden für XSL Transformationen.
	/// </summary>
	public static class XsdHelper
	{
		/// <summary>
		/// Converts the XML schema to a XML document.
		/// </summary>
		/// <param name="xmlSchema">The XML schema.</param>
		/// <returns>The XML document.</returns>
		public static XmlDocument ConvertSchemaToXmlDocument(XmlSchema xmlSchema)
		{
			ArgChecker.ShouldNotBeNull(xmlSchema, "xmlSchema");

			var xmlDoc = new XmlDocument();
			using (var memStream = new MemoryStream())
			{
				xmlSchema.Write(memStream);
				memStream.Seek(0, SeekOrigin.Begin);
				xmlDoc.Load(memStream);
				memStream.Close();
			}
			return xmlDoc;
		}

		/// <summary>
		/// Converts the XML schema to a XML document.
		/// </summary>
		/// <param name="xmlSchema">The XML schema.</param>
		/// <returns>The XML document.</returns>
		public static XDocument ConvertSchemaToXDocument(XmlSchema xmlSchema)
		{
			ArgChecker.ShouldNotBeNull(xmlSchema, "xmlSchema");

			XDocument xmlDoc;
			using (var memStream = new MemoryStream())
			{
				xmlSchema.Write(memStream);
				memStream.Seek(0, SeekOrigin.Begin);

				xmlDoc = XDocument.Load(XmlReader.Create(memStream));
				memStream.Close();
			}
			return xmlDoc;
		}

		/// <summary>
		/// Gets the XML-Rootelement form a given XML schema.
		/// </summary>
		/// <param name="xmlSchema"></param>
		/// <returns></returns>
		public static XmlQualifiedName GetRootElementName(XmlSchema xmlSchema)
		{
			ArgChecker.ShouldNotBeNull(xmlSchema, "xmlSchema");

			XmlQualifiedName result = null;
			foreach (DictionaryEntry xso in xmlSchema.Elements)
			{
				result = xso.Key as XmlQualifiedName;

				if (result != null) break;

				var xse = xso.Value as XmlSchemaElement;

				if (xse == null) continue;

				result = new XmlQualifiedName(xse.Name);
				break;
			}

			return result;
		}

		/// <summary>
		/// Loads an embedded resource XmlSchema as stream and returns the stream.
		/// Trys to get the XmlSchema from the resources of the calling assembly
		/// and if not found from the resources of the entry assembly.
		/// </summary>
		/// <param name="xmlSchemaName">Name of the XmlSchema.</param>
		/// <returns>XmlSchema stream if found; otherwise <see langword="null"/>.</returns>
		public static Stream CreateSchemaStreamFromEmbeddedResource(string xmlSchemaName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(xmlSchemaName, "xmlSchemaName");

			Stream xsdStream = null;

			var callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

			if (resName != null)
			{
				// get the resource into a stream
				xsdStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xsdStream == null)
			{
				var entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

				if (resName != null)
					// get the resource into a stream
					xsdStream = entryAssembly.GetManifestResourceStream(resName);
			}

			return xsdStream;
		}

		/// <summary>
		/// Create a XmlSchema instance from a embedded resource
		/// </summary>
		/// <param name="xmlSchemaName">Name of the embedded XSD file.</param>
		/// <returns>XmlSchema instance or <see langword="null"/> if not found.</returns>
		public static XmlSchema CreateSchemaFromEmbeddedResource(string xmlSchemaName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(xmlSchemaName, "xmlSchemaName");

			Stream xsdStream = null;

			var callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

			if (resName != null)
			{
				// get the resource into a stream
				xsdStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xsdStream == null)
			{
				var entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

				if (resName != null)
					// get the resource into a stream
					xsdStream = entryAssembly.GetManifestResourceStream(resName);
			}

			if (xsdStream == null) return null;

			try
			{
				XmlSchema xmlSchema = XmlSchema.Read(xsdStream, null);
				return xmlSchema;
			}
			finally
			{
				xsdStream.Dispose();
			}
		}

		/// <summary>
		/// Create a XmlSchemaSet instance from a embedded XSD file
		/// </summary>
		/// <param name="xmlSchemaName">Name of the embedded XSD file.</param>
		/// <returns>XmlSchemaSet instance or <see langword="null"/> if not found.</returns>
		public static XmlSchemaSet CreateSchemaSetFromEmbeddedResource(string xmlSchemaName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(xmlSchemaName, "xmlSchemaName");

			Stream xsdStream = null;

			var callingAssembly = Assembly.GetCallingAssembly();

			string[] resNames = callingAssembly.GetManifestResourceNames();
			string resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

			if (resName != null)
			{
				// get the resource into a stream
				xsdStream = callingAssembly.GetManifestResourceStream(resName);
			}

			if (xsdStream == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				// get the namespace
				resNames = entryAssembly.GetManifestResourceNames();
				resName = resNames.FirstOrDefault(name => name.Contains(xmlSchemaName));

				if (resName != null)
					// get the resource into a stream
					xsdStream = entryAssembly.GetManifestResourceStream(resName);
			}

			if (xsdStream == null) return null;

			try
			{
				XmlSchema xmlSchema = XmlSchema.Read(xsdStream, null);
				var xmlSchemaSet = new XmlSchemaSet();
				xmlSchemaSet.Add(xmlSchema);

				return xmlSchemaSet;
			}
			finally
			{
				xsdStream.Dispose();
			}
		}

		/// <summary>
		/// Liefert den TargetNamespace zurück, der innerhalb des XML-Schema definiert wurde.
		/// </summary>
		/// <param name="schemaFilePath">XML-Schema dessen targetnamespace-Wert zurückgeliefert werden soll.</param>
		/// <returns>targetnamespace-Wert der in dem XML-Schema festgelegt wurde.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="schemaFilePath"/> is null.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="schemaFilePath"/> is empty.</exception>
		/// <exception cref="ArgFilePathException">Is thrown if <paramref name="schemaFilePath"/> contains no path to a existing file.</exception>
		public static string GetTargetNamespaceFromSchema(string schemaFilePath)
		{
			string result;

			ArgChecker.ShouldNotBeNullOrEmpty(schemaFilePath, "schemaFilePath");
			ArgChecker.ShouldBeExistingFile(schemaFilePath, "schemaFilePath");

			using (var fileStream = new FileStream(schemaFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1))
			{
				result = GetTargetNamespaceFromSchema(fileStream);
			}

			return result;
		}

		/// <summary>
		/// Liefert den TargetNamespace zurück, der innerhalb des XML-Schema definiert wurde.
		/// </summary>
		/// <param name="schemaStream">XML-Schema Stream dessen targetnamespace-Wert zurückgeliefert werden soll.</param>
		/// <returns>targetnamespace-Wert der in dem XML-Schema festgelegt wurde.</returns>
		/// <exception cref="ArgNullException">Wird geworfen, wenn schemaStream den Wert null hat.</exception>
		public static string GetTargetNamespaceFromSchema(Stream schemaStream)
		{
			var result = string.Empty;

			ArgChecker.ShouldNotBeNull(schemaStream, "schemaStream");

			XmlReader reader = null;
			try
			{
				schemaStream.Seek(0, SeekOrigin.Begin);
				reader = XmlReader.Create(schemaStream);
				while (reader.Read())
				{
					if (reader.Name != "xs:schema") continue;
					reader.MoveToAttribute("targetNamespace");
					result = reader.Value;
				}
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return result;
		}

	}
}
