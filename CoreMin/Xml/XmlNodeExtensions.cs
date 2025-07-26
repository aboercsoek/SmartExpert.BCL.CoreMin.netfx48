//--------------------------------------------------------------------------
// File:    XmlNodeExtensions.cs
// Content:	Implementation of class XmlNodeExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Xml;
using SmartExpert;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as XmlNode therewith the extension methods are available by using the System.Xml namespace.
namespace SmartExpert.Xml
{
	///<summary>Represents extension methods for <see cref="XmlNode"/> type.</summary>
	public static class XmlNodeExtensions
    {
		/// <summary>
		/// Returns the inner text of the first <see cref="XmlNode"/> that matches the XPath expression.
		/// </summary>
		/// <param name="node">The XML node.</param>
		/// <param name="xpath">The XPath expression.</param>
		/// <returns>The inner text of the first <see cref="XmlNode"/> that matches the XPath expression, or <see langword="null"/> if there is no XPath expression match.</returns>
        public static string SelectSingleInnerText(this XmlNode node, string xpath )
        {
            return node.SelectSingleInnerText(xpath, null);
        }

		/// <summary>
		/// Returns the inner text of the first <see cref="XmlNode"/> that matches the XPath expression.
		/// </summary>
		/// <param name="node">The XML node.</param>
		/// <param name="xpath">The XPath expression.</param>
		/// <param name="nsmgr">The XML namespace manager.</param>
		/// <returns>The inner text of the first <see cref="XmlNode"/> that matches the XPath expression, or <see langword="null"/> if there is no XPath expression match.</returns>
        public static string SelectSingleInnerText(this XmlNode node, string xpath, XmlNamespaceManager nsmgr)
        {
            XmlNode tnode = node.SelectSingleNode(xpath,nsmgr);
            if (tnode == null)
            {
                return string.Empty;
            }
            return tnode.InnerText;
        }
    }
}
