//--------------------------------------------------------------------------
// File:    InvalidCredentialException.cs
// Content:	Implementation of class InvalidCredentialException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// This <see cref="Exception"/> indicates, that invalid credentials were supplied.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class InvalidCredentialException : TechException
	{
		private const int TypeErrorCode = 1023;

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		public InvalidCredentialException() { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="message">The message of the exception</param>
		public InvalidCredentialException(string message) : base(message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="message">The message of the exception</param>
		/// <param name="cause">The error which causes this exception.</param>
		public InvalidCredentialException(string message, Exception cause) : base(cause, message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="context">A <see cref="SerializationInfo"/> to use.</param>
		/// <param name="info">A <see cref="StreamingContext"/> to use.</param>
		protected InvalidCredentialException(
		  SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
