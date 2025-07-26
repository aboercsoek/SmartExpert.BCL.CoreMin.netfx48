//--------------------------------------------------------------------------
// File:    HashSetDebugView.cs
// Content:	Implementation of class HashSetDebugView
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Collections.Generic;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections
{
	///<summary>HashSet{T} debug view.</summary>
	internal class HashSetDebugView<T>
	{
		private readonly HashSet<T> m_HashSet;

		public HashSetDebugView(HashSet<T> set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}
			m_HashSet = set;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return m_HashSet.ToArray();
			}
		}
	}


}
