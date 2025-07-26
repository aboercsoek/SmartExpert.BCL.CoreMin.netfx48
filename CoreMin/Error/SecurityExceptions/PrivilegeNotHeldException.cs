//--------------------------------------------------------------------------
// File:    PrivilegeNotHeldException.cs
// Content:	Implementation of class PrivilegeNotHeldException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Xml.Serialization;

#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// This exception is thrown when an attempt is made to activate a privilege
	/// which is not currently held by the logon token.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class PrivilegeNotHeldException : TechException
	{
		private const int TypeErrorCode = 1015;

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		public PrivilegeNotHeldException() { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message
		/// </summary>
		/// <param name="message">The error message</param>
		public PrivilegeNotHeldException(string message) : base(message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message and inner exception
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="cause">The error which causes this exception.</param>
		public PrivilegeNotHeldException(string message, Exception cause) : base(cause, message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class using the specified 
		/// <see cref="System.Runtime.Serialization.SerializationInfo"/> and
		/// <see cref="System.Runtime.Serialization.StreamingContext" /> Scope.
		/// </summary>
		/// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo"/>.</param>
		/// <param name="scope">A <see cref="System.Runtime.Serialization.StreamingContext" />.</param>
		protected PrivilegeNotHeldException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext scope)
			: base(info, scope) { }
	}
}
