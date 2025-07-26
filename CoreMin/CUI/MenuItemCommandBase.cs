//--------------------------------------------------------------------------
// File:    MenuItemCommandBase.cs
// Content:	Implementation of class MenuItemCommandBase
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

namespace SmartExpert.CUI
{
	/// <summary>
	/// A class that encapsulates the behavior of a single menu item,
	/// and is responsible for it's execution.
	/// </summary>
	public abstract class MenuItemCommandBase
	{
		#region Public Methods

		/// <summary>
		/// Execute the menu option
		/// </summary>
		public void Execute()
		{
			DoExecute();
		}

		#endregion

		#region Abstract Members

		/// <summary>
		/// Text to display for menu item.
		/// </summary>
		public abstract string Text { get; }

		/// <summary>
		/// Execute the menu item operation.
		/// </summary>
		protected abstract void DoExecute();

		#endregion
	}
}
