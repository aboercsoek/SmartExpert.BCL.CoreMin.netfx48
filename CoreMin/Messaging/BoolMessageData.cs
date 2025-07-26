//--------------------------------------------------------------------------
// File:    BoolMessageData.cs
// Content:	Implementation of class BoolMessageData
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Collections;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion


namespace SmartExpert.Messaging
{
	/// <summary>
	/// Combines a boolean succes/fail flag with a error/status message and a set of data properties.
	/// </summary>
	[DebuggerDisplay("BoolMessageData: {ToString()}")]
	public class BoolMessageData : BoolMessageItem
	{
		private const string ItemId = "$(item)";
		private PropertyBag m_PropertyBag;

		/// <summary>
		/// Empty true message.
		/// </summary>
		public static new BoolMessageData True { get { return new BoolMessageData(true, string.Empty); } }

		/// <summary>
		/// Empty false message.
		/// </summary>
		public static new BoolMessageData False { get { return new BoolMessageData(false, string.Empty); } }

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageData"/> class.
		/// </summary>
		/// <param name="success">if set to <see langword="true"/> [success].</param>
		/// <param name="message">The message.</param>
		public BoolMessageData(bool success, string message)
			: base(success, message, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageData"/> class.
		/// </summary>
		/// <param name="success">if set to <see langword="true"/> [success].</param>
		/// <param name="message">The message.</param>
		/// <param name="item">The data item.</param>
		public BoolMessageData(bool success, string message, object item)
			: base(success, message, item)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageData"/> class.
		/// </summary>
		/// <param name="exception">The exception to store.</param>
		public BoolMessageData(Exception exception)
			: this(exception, ErrorDataType.Error)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageData"/> class.
		/// </summary>
		/// <param name="exceptions">The exceptions to store.</param>
		public BoolMessageData(IEnumerable<Exception> exceptions)
			: this(new CombinedException("Multi exceptions occured.", exceptions), ErrorDataType.Error)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BoolMessageData"/> class.
		/// </summary>
		/// <param name="exception">The exception to store.</param>
		/// <param name="errorDataType">The error type.</param>
		public BoolMessageData(Exception exception, ErrorDataType errorDataType)
			: base(false, exception.Message, exception)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			ErrorData errorData = ErrorData.Create(exception, errorDataType);
			Data.SetProperty("$(errorData)", errorData);
			Message = errorData.ErrorText;
		}

		/// <summary>
		/// Return the data properties.
		/// </summary>
		public IPropertyBag Data
		{
			get { return m_PropertyBag ?? (m_PropertyBag = new PropertyBag()); }
		}

		/// <summary>
		/// Return the data item.
		/// </summary>
		public override object Item
		{
			get
			{
				if (m_PropertyBag == null)
					m_PropertyBag = new PropertyBag();

				return m_PropertyBag.TryGetPropertyValue(ItemId);
			}
			set
			{
				if (m_PropertyBag == null)
					m_PropertyBag = new PropertyBag();

				m_PropertyBag.SetProperty(ItemId, value);
			}
		}


		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "{0} | {1}".SafeFormatWith(Success, Message);
		}
	}
}
