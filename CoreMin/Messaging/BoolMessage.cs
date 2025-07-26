//--------------------------------------------------------------------------
// File:    BoolMessage.cs
// Content:	Implementation of class BoolMessage
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

#endregion

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


namespace SmartExpert.Messaging
{
	/// <summary>
	/// Combines a boolean succes/fail flag with a error/status message.
	/// </summary>
	public class BoolMessage
	{
		/// <summary>
		/// True message.
		/// </summary>
		public static BoolMessage True { get { return new BoolMessage(true, string.Empty); } }

		/// <summary>
		/// False message.
		/// </summary>
		public static BoolMessage False { get { return new BoolMessage(false, string.Empty); } }

		/// <summary>
		/// Success / failure ?
		/// </summary>
		public readonly bool Success;

		/// <summary>
		/// Error message for failure, status message for success.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Set the readonly fields.
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public BoolMessage(bool success, string message)
		{
			Success = success;
			Message = message;
		}
	}
}

