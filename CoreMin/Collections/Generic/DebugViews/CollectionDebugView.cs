//--------------------------------------------------------------------------
// File:    CollectionDebugView.cs
// Content:	Implementation of class CollectionDebugView
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace SmartExpert.Collections.Generic
{
	///<summary>Debug View Proxy for ICollection{T} types</summary>
	internal class CollectionDebugView<T>
	{
		private ICollection<T> m_Collection;

		public CollectionDebugView(ICollection<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			m_Collection = collection;
		}

		// Properties
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] array = new T[m_Collection.Count];
				m_Collection.CopyTo(array, 0);
				return array;
			}
		}

	}
}
