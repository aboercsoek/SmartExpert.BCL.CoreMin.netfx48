﻿//--------------------------------------------------------------------------
// File:    ActionBasedMenuItemCmd.cs
// Content:	Implementation of class ActionBasedMenuItemCmd
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.ComponentModel;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.CUI
{
	///<summary>Delegate-based menu item command class</summary>
	public class ActionBasedMenuItemCmd : MenuItemCommandBase
	{
		private readonly Action m_MenuItemAction;
		private readonly string m_MenuItemText;


		/// <summary>
		/// Initializes a new instance of the <see cref="ActionBasedMenuItemCmd"/> class.
		/// </summary>
		/// <param name="menuItemAction">The menu item action delegate.</param>
		public ActionBasedMenuItemCmd(Action menuItemAction)
		{
			ArgChecker.ShouldNotBeNull(menuItemAction, "menuItemAction");

			m_MenuItemAction = menuItemAction;
			m_MenuItemText = GetDescriptionFromOptionCodeDelegate();
		}

		/// <summary>
		/// Text to display in the menu.
		/// </summary>
		public override string Text
		{
			get { return m_MenuItemText; }
		}

		/// <summary>
		/// Execute the actual operation.
		/// </summary>
		protected override void DoExecute()
		{
			m_MenuItemAction();
		}

		/// <summary>
		/// Pull the description off attributes on the delegate passed in.
		/// This only works if you pass in actual methods, but that's ok
		/// for our purposes.
		/// </summary>
		/// <returns>Description text to display.</returns>
		private string GetDescriptionFromOptionCodeDelegate()
		{
			DescriptionAttribute description =
				m_MenuItemAction.Method.GetCustomAttributes(typeof (DescriptionAttribute), false)
					.AsSequence<DescriptionAttribute>().FirstOrDefault();

			return description == null ? "No description present" : description.Description;
		}
	}
}
