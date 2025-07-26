//--------------------------------------------------------------------------
// File:    PermissionException.cs
// Content:	Implementation of class PermissionException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Xml.Serialization;

#endregion

namespace SmartExpert.Error
{
	
	/// <summary>
	/// This exception is used to mark access violations.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class PermissionException : TechException
	{
		private const int TypeErrorCode = 1024;

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		public PermissionException() : base(StringResources.ErrorAccessViolation) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message
		/// </summary>
		/// <param name="message">The error message</param>
		public PermissionException(string message) : base(message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message and inner exception
		/// </summary>
		/// <param name="cause">The error which causes this exception.</param>
		public PermissionException(Exception cause) : base(StringResources.ErrorAccessViolation, cause) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message and inner exception
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="cause">The error which causes this exception.</param>
		public PermissionException(string message, Exception cause) : base(cause, message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class using the specified 
		/// <see cref="System.Runtime.Serialization.SerializationInfo"/> and
		/// <see cref="System.Runtime.Serialization.StreamingContext" /> Scope.
		/// </summary>
		/// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/>.</param>
		/// <param name="scope">The <see cref="System.Runtime.Serialization.StreamingContext" />.</param>
		protected PermissionException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext scope)
			: base(info, scope) { }
	}
}
