//--------------------------------------------------------------------------
// File:    XPathHelper.cs
// Content:	Implementation of class XPathHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Xml.XPath
{
	///<summary>Provides XPath helper methods.</summary>
	public static class XPathHelper
	{
		private static readonly string XPath;
		private static readonly string Attribute;

		static XPathHelper()
		{
			XPath = "/*[local-name()='{0}' and namespace-uri()='{1}']";
			Attribute = "[@{0}='{1}']";
		}

		/// <summary>
		/// Creates a XPath expression.
		/// </summary>
		/// <param name="namespaceUri">The namespace URI.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <param name="attributeValue">The attribute value.</param>
		/// <param name="localNames">The local names.</param>
		/// <returns>The XPath string.</returns>
		/// <remarks>
		///		Attribute name and value is only used if both values are not null or empty.
		///		Uses the equal condition between attribute name and value when generating the XPath string: [@name='value']
		/// </remarks>
		/// <exception cref="ArgNullException">Is thrown if <para>namespareUri</para> is <see langword="null"/>.</exception>
		public static string CreateXPath( string namespaceUri, string attributeName, string attributeValue, params string[] localNames )
		{
			ArgChecker.ShouldNotBeNull(namespaceUri, "namespaceUri");

			var builder = new StringBuilder();
			foreach ( string str in localNames )
			{
				builder.Append(string.Format(CultureInfo.InvariantCulture, XPath, new object[] { str, namespaceUri }));
			}
			string result = AddAttribute(builder.ToString(), attributeName, attributeValue);

			return result;
		}

		/// <summary>
		/// Builds an XPath string an adds it to a given XPath string.
		/// </summary>
		/// <param name="namespaceUri">The namespace of the local names.</param>
		/// <param name="baseXPath">The base XPath.</param>
		/// <param name="localNames">The local names.</param>
		/// <returns>The generated XPath string.</returns>
		/// <exception cref="ArgNullException">Is thrown if <para>namespareUri</para> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullException">Is thrown if <para>baseXPath</para> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullException">Is thrown if <para>localNames</para> is <see langword="null"/>.</exception>
		public static string AddXPath(string namespaceUri, string baseXPath, params string[] localNames)
		{
			ArgChecker.ShouldNotBeNull(namespaceUri, "namespaceUri");
			ArgChecker.ShouldNotBeNull(baseXPath, "baseXPath");
			ArgChecker.ShouldNotBeNull(localNames, "localNames");

			var builder = new StringBuilder(baseXPath);
			foreach ( string str in localNames )
			{
				builder.Append(string.Format(CultureInfo.InvariantCulture, XPath, new object[] { str, namespaceUri }));
			}

			return builder.ToString();
		}


		/// <summary>
		/// Adds a attribute to a given XPath string. Uses the equal condition between name and value.
		/// </summary>
		/// <param name="baseXPath">The base XPath.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <param name="attributeValue">The attribute value.</param>
		/// <returns>The generated XPath string.</returns>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <para>baseXPath</para> is <see langword="null"/> or empty.</exception>
		public static string AddAttribute(string baseXPath, string attributeName, string attributeValue)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(baseXPath, "baseXPath");

			var builder = new StringBuilder(baseXPath);
			
			if ( string.IsNullOrEmpty(attributeName).IsFalse() && string.IsNullOrEmpty(attributeValue).IsFalse() )
			{
				builder.Append(Attribute.SafeFormatWith( new object[] { attributeName, attributeValue }));
			}

			return builder.ToString();
		}

		/// <summary>
		/// Gets the inner text of the XPath result.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="xpath">The xpath.</param>
		/// <returns>Inner text of the XPath result if XPath is valid; otherwise an empty string.</returns>
		/// <exception cref="ArgNullException">Is thrown if <para>document</para> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <para>xpath</para> is <see langword="null"/> or empty.</exception>
		public static string GetInnerText(XmlDocument document, string xpath)
		{
			ArgChecker.ShouldNotBeNull(document, "document");
			ArgChecker.ShouldNotBeNullOrEmpty(xpath, "xpath");

			try
			{
				XmlNode node = document.SelectSingleNode(xpath);

				return node == null ? string.Empty : node.InnerText;
			}
			catch ( XPathException )
			{
				return string.Empty;
			}
		}


		/// <summary>
		/// Gets the inner text of the XPath result.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="xpath">The xpath.</param>
		/// <returns>Inner text of the XPath result if XPath is valid; otherwise an empty string.</returns>
		/// <exception cref="ArgNullException">Is thrown if <para>document</para> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <para>xpath</para> is <see langword="null"/> or empty.</exception>
		public static string GetInnerText(XDocument document, string xpath)
		{
			ArgChecker.ShouldNotBeNull(document, "document");
			ArgChecker.ShouldNotBeNullOrEmpty(xpath, "xpath");

			try
			{
				XElement node = document.XPathSelectElement(xpath);
				return node == null ? string.Empty : node.Value;
			}
			catch (XPathException)
			{
				return string.Empty;
			}
		}

	}
}
