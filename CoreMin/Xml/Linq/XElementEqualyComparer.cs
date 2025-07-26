//--------------------------------------------------------------------------
// File:    XElementEqualyComparer.cs
// Content:	Implementation of class XElementEqualyComparer
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

// Uses the same namespace as XDocument therewith the extension methods are available by using the System.Xml.Linq namespace.
namespace SmartExpert.Xml.Linq
{
	///<summary>An XElement Equaly Comparer</summary>
	public class XElementEqualyComparer : IEqualityComparer<XElement>
	{
		readonly Func<XElement, int> m_HashGenerator;

		/// <summary>
		/// Initializes a new instance of the <see cref="XElementEqualyComparer"/> class.
		/// </summary>
		public XElementEqualyComparer()
		{
			m_HashGenerator = null;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="XElementEqualyComparer"/> class.
		/// </summary>
		/// <param name="hashGenerator">The hash generator.</param>
		public XElementEqualyComparer(Func<XElement, int> hashGenerator)
		{
			m_HashGenerator = hashGenerator;
		}

		#region IEqualityComparer<T> Members

		/// <summary>
		/// Determines whether the specified xml elements are equal.
		/// </summary>
		/// <param name="x">The first element to compare.</param>
		/// <param name="y">The second element to compare.</param>
		/// <returns>true if the specified elements are equal; otherwise, false.</returns>
		public bool Equals(XElement x, XElement y)
		{
			if (m_HashGenerator == null)
				return x.IsDeepEquals(y);

			return GetHashCode(x) == GetHashCode(y);
		}

		/// <summary>
		/// Returns a hash code for the specified element. Calls the hash generator delegate witch was spefified in the constructor.
		/// </summary>
		/// <param name="obj">The xml element for witch a hash code is to be returned.</param>
		/// <returns>
		/// A hash code for the specified element. 
		/// </returns>
		public int GetHashCode(XElement obj)
		{
			if (m_HashGenerator == null)
				return obj.GetDeepHashCode();

			return m_HashGenerator(obj);
		}

		#endregion
	}
}
