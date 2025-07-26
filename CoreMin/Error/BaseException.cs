//--------------------------------------------------------------------------
// File:    BaseException.cs
// Content:	Implementation of class BaseException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


//using SmartExpert.SystemManagement;

#endregion

namespace SmartExpert.Error
{

	/// <summary>
	/// This simply extends the <see cref="Exception"/> class
	/// by adding a variable length parameter list in the basic
	/// constructor which takes the exception message, and then
	/// apply string.Format if necessary, which is an incredibly
	/// common expectation when throwing exceptions, and should have been
	/// part of the base exception class.
	/// </summary>
	/// 
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class BaseException : Exception
	{
		private const int TypeErrorCode = 1;
// ReSharper disable InconsistentNaming
		/// <summary>Key for userfriendly error message in Exception.Data</summary>
		private const string USER_FRIENDLY_MESSAGE = "UserFriendlyMessage";
// ReSharper restore InconsistentNaming

		/// <summary>Set or get the userfriendly error message</summary>
		public string UserFriendlyMessage
		{
			get
			{
				return (Data.Contains(USER_FRIENDLY_MESSAGE)) ? (string)Data[USER_FRIENDLY_MESSAGE] : string.Empty;
			}
			set
			{
				Data[USER_FRIENDLY_MESSAGE] = value ?? string.Empty;
			}
		}

		private int m_ErrorCode = TypeErrorCode;

		/// <summary>Liefert den Fehlercode, der zu einer Exception hinterlegt ist.
		/// Default ist 1, das kann in speziellen abgeleiteten Klassen überschrieben werden.
		/// </summary>
		public int ErrorCode
		{
			get { return m_ErrorCode; }
			set { m_ErrorCode = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Exception"/> class.
		/// </summary>
		public BaseException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BaseException(string message, params object[] formatParameters)
			: base(formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseException"/> class.
		/// </summary>
		/// <param name="inner">The error which causes this exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BaseException(Exception inner, string message, params object[] formatParameters)
			: base(
				formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected BaseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

	}
}