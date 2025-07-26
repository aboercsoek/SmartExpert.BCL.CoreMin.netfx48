//--------------------------------------------------------------------------
// File:    IAppMenuView.cs
// Content:	Definition  of IAppMenuView interface
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace SmartExpert.CUI
{
	/// <summary>
	/// Application Menu View Contract
	/// </summary>
	public interface IAppMenuView
	{
		/// <summary>
		/// Gets a value indicating whether the user wants to quit the application.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if user hitted ESC to quit the application; otherwise, <see langword="false"/>.
		/// </value>
		bool ShouldQuit { get; }

		/// <summary>
		/// Inits the view.
		/// </summary>
		/// <param name="menuItems">The menu items.</param>
		void InitView(IEnumerable<string> menuItems);

		/// <summary>
		/// Displays the menu.
		/// </summary>
		void DisplayMenu();

		/// <summary>
		/// Clears the view.
		/// </summary>
		void ClearView();

		/// <summary>
		/// Writes the menu operation header.
		/// </summary>
		/// <param name="headerText">The menu operation header text.</param>
		void WriteMenuOperationHeader(string headerText);

		/// <summary>
		/// Shows the exception details.
		/// </summary>
		/// <param name="exception">The exception to show.</param>
		void ShowExceptionDetails(Exception exception);

		/// <summary>
		/// Waits for valid user menu item selection.
		/// </summary>
		/// <returns>Selected meu item index or -1 if user selected quit option.</returns>
		int WaitForValidUserInput();

		/// <summary>
		/// Prompts to continue after menu item operation execution.
		/// </summary>
		void PromptToContinue();
	}
}