//--------------------------------------------------------------------------
// File:    PropertyBagDebugView.cs
// Content:	Implementation of class PropertyBagDebugView
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections
{
	///<summary>Debug View Proxy for IPropertyBag types</summary>
	internal class PropertyBagDebugView
	{
		private readonly IPropertyBag m_PropertyBag;

		public PropertyBagDebugView(IPropertyBag propertyBag)
		{
			m_PropertyBag = propertyBag;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<string, object>[] Items
		{
			get
			{
				var array = new KeyValuePair<string, object>[m_PropertyBag.Count];
				m_PropertyBag.CopyTo(array, 0);
				return array;
			}
		}
	}


}
