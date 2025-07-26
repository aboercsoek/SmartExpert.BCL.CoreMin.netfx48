﻿//--------------------------------------------------------------------------
// File:    ExceptionText.cs
// Content:	Implementation of class ExceptionText
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

namespace SmartExpert.Error
{
	/// <summary>
	/// Result of rendering an exception.
	/// </summary>
	[Serializable]
	public class ExceptionText
	{
		/// <summary>
		/// <para>String representation of the exception rendered by collecting all of the data about the original exception and all of the inner/related exceptions in the tree.</para>
		/// <para>A more detailed and well-organized counterpart for <see cref="T:System.Exception" />'s <see cref="M:System.Exception.ToString" /> method.</para>
		/// </summary>
		public readonly string FullText;
		/// <summary>
		/// <para>Message of the exception, into all inner exception messages are also included.</para>
		/// <para>A more detailed counterpart for <see cref="T:System.Exception" />'s <see cref="P:System.Exception.Message" /> property.</para>
		/// </summary>
		public readonly string Message;
		/// <summary>
		/// <para>User friendly message of the exception.</para>
		/// </summary>
		public readonly string UserFriendlyMessage;


		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionText"/> class.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="fullText">The full text of the exception.</param>
		/// <param name="userFriendlyMessage">The user friendly exception message.</param>
		public ExceptionText(string message, string fullText, string userFriendlyMessage)
		{
			ArgChecker.ShouldNotBeNull(message, "message");
			ArgChecker.ShouldNotBeNull(fullText, "fullText");


			Message = message;
			FullText = fullText;
			UserFriendlyMessage = userFriendlyMessage ?? string.Empty;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Message;
		}
	}


}
