//--------------------------------------------------------------------------
// File:    TreeNode.cs
// Content:	Implementation of class TreeNode
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Collections.Generic
{
	///<summary>The generic tree node class.</summary>
	/// <typeparam name="T">The tree node type.</typeparam>
	public sealed class TreeNode<T>
	{
		#region Private Fields

		private TreeNode<T> m_Parent;
		private Tree<T> m_Tree;
		private List<TreeNode<T>> m_Children;
		private T m_Data;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
		/// </summary>
		public TreeNode()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
		/// </summary>
		/// <param name="data">The tree node data.</param>
		public TreeNode( T data )
		{
			Data = data;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the tree node data.
		/// </summary>
		/// <value>The tree node data.</value>
		public T Data
		{
			get { return m_Data; }
			set { m_Data = value; }
		}

		/// <summary>
		/// Gets the parent tree node.
		/// </summary>
		/// <value>The parent tree node.</value>
		public TreeNode<T> Parent
		{
			get { return m_Parent; }
		}

		/// <summary>
		/// Gets the tree.
		/// </summary>
		/// <value>The tree instance.</value>
		public Tree<T> Tree
		{
			get { return m_Tree; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Addes a child node to this node
		/// </summary>
		/// <param name="child">The tree node that should be added as child to this node.</param>
		public void AddChild( TreeNode<T> child )
		{
			if ( child.Tree != null )
				throw new ArgumentException("node to add already attached to tree");

			if ( child.Parent != null )
				throw new ArgumentException("node to add already attached to parent");

			if ( m_Children == null )
			{
				m_Children = new List<TreeNode<T>>();
			}

			m_Children.Add(child);
			child.m_Parent = this;
			child.m_Tree = Tree;
		}

		/// <summary>
		/// Removes a child node from this node.
		/// </summary>
		/// <param name="child">The child node to remove.</param>
		public void RemoveChild( TreeNode<T> child )
		{
			if ( child == null )
				throw new ArgumentNullException("child");

			if ( child.Tree != Tree )
				throw new ArgumentException("node belongs to a different tree");

			if ( child.Parent != this )
				throw new ArgumentException("not a child");

			if ( m_Children == null )
				throw new ArgumentException("not a child - node has no children");

			if ( m_Children.Count < 1 )
				throw new ArgumentException("not a child - node has no children");

			m_Children.Remove(child);

			child.m_Parent = null;
			child.m_Tree = null;

		}

		/// <summary>
		/// Enumerates over the child nodes of this node.
		/// </summary>
		/// <returns>IEnumerable-Collection of child nodes.</returns>
		public IEnumerable<TreeNode<T>> EnumChildren()
		{
			if ( m_Children == null )
				yield break;

			foreach ( var i in m_Children )
			{
				yield return i;
			}

		}

		/// <summary>
		/// Walks through the child nodes.
		/// </summary>
		/// <returns>A IEnumerable-Collection of WalkEvents.</returns>
		public IEnumerable<TreeWalker.WalkEvent<TreeNode<T>>> Walk()
		{
			return TreeWalker.Walk<TreeNode<T>>(this, n => n.EnumChildren());
		}

		#endregion
	}
}
