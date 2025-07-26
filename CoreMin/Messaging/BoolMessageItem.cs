//--------------------------------------------------------------------------
// File:    BoolMessageItem.cs
// Content:	Implementation of class BoolMessageItem
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


namespace SmartExpert.Messaging
{
	/// <summary>
	/// Combines a boolean succes/fail flag with a error/status message and a assoziated data item.
	/// </summary>
	public class BoolMessageItem : BoolMessage
	{
		/// <summary>
		/// True message.
		/// </summary>
		public static new BoolMessageItem True { get { return new BoolMessageItem(true, string.Empty, null); } }

		/// <summary>
		/// False message.
		/// </summary>
		public static new BoolMessageItem False { get { return new BoolMessageItem(false, string.Empty, null); } }

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageItem"/> class.
		/// </summary>
		/// <param name="success">if set to <see langword="true"/> [success].</param>
		/// <param name="message">The message.</param>
		/// <param name="item">The data item.</param>
		public BoolMessageItem(bool success, string message, object item)
			: base(success, message)
		{
			Item = item;
		}

		/// <summary>
		/// Return the data item.
		/// </summary>
		public virtual object Item { get; set; }
	}
}
