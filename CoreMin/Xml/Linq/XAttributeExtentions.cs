//--------------------------------------------------------------------------
// File:    XAttributeExtentions.cs
// Content:	Implementation of class XAttributeExtentions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as XElement therewith the extension methods are available by using the System.Xml.Linq namespace.
namespace SmartExpert.Xml.Linq
{
	///<summary>Represents extension methods for <see cref="XElement"/> type.</summary>
	public static class XAttributeExtentions
	{
		/// <summary>
		/// Gets the deep hash code of the xml attribute.
		/// </summary>
		/// <param name="attr">The xml attribute.</param>
		/// <returns>
		/// Returns the deep hash code value, or 0 if the attribute is null.
		/// </returns>
		public static int GetDeepHashCode(this XAttribute attr)
		{
			if (attr == null)
				return 0;

			return (attr.Name.GetHashCode() ^ attr.Value.GetHashCode());
		}

	}
}
