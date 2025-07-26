//--------------------------------------------------------------------------
// File:    Tree.cs
// Content:	Implementation of class Tree
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections.Generic
{
	///<summary>A generic tree class.</summary>
	///<typeparam name="T">Tree element type.</typeparam>
	public sealed class Tree<T>
	{
		/// <summary>The root of the tree.</summary>
		public TreeNode<T> Root;

	}
}
