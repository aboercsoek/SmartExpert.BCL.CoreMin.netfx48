//--------------------------------------------------------------------------
// File:    ReadOnlyDictionaryDebugView.cs
// Content:	Implementation of class ReadOnlyDictionaryDebugView
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
	///<summary>DebugView Proxy for IDictionary derived types.</summary>
	internal class DictionaryDebugView<TKey, TValue>
	{
		private IDictionary<TKey, TValue> m_ReadOnlyDictionary;

		public DictionaryDebugView(IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			m_ReadOnlyDictionary = dictionary;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<TKey, TValue>[] Items
		{
			get
			{
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[m_ReadOnlyDictionary.Count];
				m_ReadOnlyDictionary.CopyTo(array, 0);
				return array;
			}
		}

	}
}
