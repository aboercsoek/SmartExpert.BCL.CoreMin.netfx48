//--------------------------------------------------------------------------
// File:    XDocumentExtensions.cs
// Content:	Implementation of class XDocumentExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as XDocument therewith the extension methods are available by using the System.Xml.Linq namespace.
namespace SmartExpert.Xml.Linq
{
	///<summary>Represents extension methods for <see cref="XDocument"/> type.</summary>
	public static class XDocumentExtensions
	{
		#region XDocument iterator externsions

		/// <summary>
		/// XObject walker. Iterates over all XObject elements (XElement, XAttribute) of a XDocument object.
		/// </summary>
		/// <param name="xDoc">The XML document to walk through.</param>
		/// <returns>All XObject nodes inside a XDocument instance.</returns>
		/// <exception cref="ArgNullException">If <paramref name="xDoc"/> or <paramref name="xDoc"/>.Root is <see langword="null"/>.</exception>
		public static IEnumerable<XObject> XObjectWalker(this XDocument xDoc)
		{
			ArgChecker.ShouldNotBeNull(xDoc, "xDoc");
			ArgChecker.ShouldNotBeNull(xDoc.Root, "xDoc.Root");

			IEnumerable<XElement> childNodes = xDoc.Root.DescendantsAndSelf();
			foreach ( XElement childNode in childNodes )
			{
				yield return childNode;

				foreach ( XAttribute attr in childNode.Attributes() )
				{
					yield return attr;
				}
			}
		}


		/// <summary>
		/// XElement walker. Iterates over all XElement nodes of a XDocument object.
		/// </summary>
		/// <param name="xDoc">The XML document to walk through.</param>
		/// <returns>All XElement nodes inside a XDocument object.</returns>
		/// <exception cref="ArgNullException">If <paramref name="xDoc"/> or <paramref name="xDoc"/>.Root is <see langword="null"/>.</exception>
		public static IEnumerable<XElement> XElementWalker( this XDocument xDoc )
		{
			ArgChecker.ShouldNotBeNull(xDoc, "xDoc");
			ArgChecker.ShouldNotBeNull(xDoc.Root, "xDoc.Root");

			IEnumerable<XElement> childNodes = xDoc.Root.DescendantsAndSelf();
			foreach ( XElement childNode in childNodes )
			{
				yield return childNode;
			}
		}


		/// <summary>
		/// XObject walker. Iterates over all XObject elements (XElement, XAttribute) of a XDocument object.
		/// </summary>
		/// <param name="xDoc">The XML document to walk through.</param>
		/// <param name="foundElement">Action to call on found element during xml document iteration.</param>
		/// <param name="foundAttribute">Action to call on found attribute during xml document iteration.</param>
		/// <exception cref="ArgNullException">
		/// If <paramref name="xDoc"/> or <paramref name="foundElement"/> or <paramref name="foundAttribute"/> is <see langword="null"/>.
		/// </exception>
		public static void XObjectWalker( this XDocument xDoc, Action<XElement> foundElement, Action<XAttribute> foundAttribute )
		{
			ArgChecker.ShouldNotBeNull(xDoc, "xDoc");
			ArgChecker.ShouldNotBeNull(xDoc.Root, "xDoc.Root");
			ArgChecker.ShouldNotBeNull(foundElement, "foundElement");
			ArgChecker.ShouldNotBeNull(foundAttribute, "foundAttribute");

			IEnumerable<XElement> childNodes = xDoc.Root.DescendantsAndSelf();
			foreach ( XElement childNode in childNodes )
			{
				foundElement(childNode);

				foreach ( XAttribute attr in childNode.Attributes() )
				{
					foundAttribute(attr);
				}
			}
		}

		#endregion

		#region XSLT Extensions

		/// <summary>
		/// Transform XML document by using a stream that contains the XSL stylesheet.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="stylesheet">The stream that contains the XSL stylesheet.</param>
		/// <returns>The tansformed XML document.</returns>
		public static XDocument XslTransform(this XDocument document, Stream stylesheet)
		{
			return XslTransform(document, new XmlTextReader(stylesheet));
		}

		/// <summary>
		/// Transform XML document by using a XML document that contains the XSL stylesheet.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="stylesheet">The XML document that contains the XSL stylesheet.</param>
		/// <returns>The tansformed XML document.</returns>
		public static XDocument XslTransform(this XDocument document, XDocument stylesheet)
		{
			return XslTransform(document, stylesheet.CreateReader());
		}

		/// <summary>
		/// Transform XML document by using a XML reader that provides access to the XSL stylesheet.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="stylesheet">The XML reader that provides access to the XSL stylesheet.</param>
		/// <returns>The tansformed XML document.</returns>
		public static XDocument XslTransform(this XDocument document, XmlReader stylesheet)
		{
			var xslTransformer = new XslCompiledTransform();
			xslTransformer.Load(stylesheet);

			var documentStream = new MemoryStream();
			var writer = new XmlTextWriter(new StreamWriter(documentStream));

			xslTransformer.Transform(document.CreateReader(), null, writer);

			documentStream.Position = 0;
			return XDocument.Load(new XmlTextReader(documentStream));
		}

		#endregion
	}
}
