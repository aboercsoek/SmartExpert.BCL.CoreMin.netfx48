//--------------------------------------------------------------------------
// File:    XmlNodeListExtensions.cs
// Content:	Implementation of class XmlNodeListExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SmartExpert;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as XmlNodeList therewith the extension methods are available by using the System.Xml namespace.
namespace SmartExpert.Xml
{
	///<summary>Represents extension methods for <see cref="XmlNodeList"/> type.</summary>
	public static class XmlNodeListExtensions
	{
		/// <summary>
		/// Converts a XmlNodeList into a IEnumerable-Collection of XML nodes.
		/// </summary>
		/// <param name="nodeList">The XML nodes.</param>
		/// <returns>IEnumerable-Collection of XML nodes.</returns>
		public static IEnumerable<XmlNode> AsEnumerable( this XmlNodeList nodeList )
		{
			return nodeList.AsSequence<XmlNode>();
		}
	}
}
