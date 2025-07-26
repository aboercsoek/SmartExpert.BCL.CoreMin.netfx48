//--------------------------------------------------------------------------
// File:    QueryCredentialDialogException.cs
// Content:	Implementation of class QueryCredentialDialogException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SmartExpert.Security.Authentication;

#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// Exception thrown when a QueryCredentialDialog failed
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public sealed class QueryCredentialDialogException : TechException
	{
		private const int TypeErrorCode = 1016;

		private readonly QueryCredentialError m_Error;

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public QueryCredentialError Error
		{
			get { return m_Error; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryCredentialDialogException"/> class.
		/// </summary>
		public QueryCredentialDialogException() { ErrorCode = TypeErrorCode; }

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryCredentialDialogException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="error">The error which causes this exception.</param>
		public QueryCredentialDialogException(QueryCredentialError error, string message)
			: base(message)
		{
			ErrorCode = TypeErrorCode;
			m_Error = error;
		}

		///// <summary>
		///// Initializes a new instance of the <see cref="QueryCredentialDialogException"/> class.
		///// </summary>
		///// <param name="message">The message.</param>
		///// <param name="innerException">The error which causes this exception.</param>
		///// <param name="error">The error which causes this exception</param>
		//public QueryCredentialDialogException(QueryCredentialError error, string message, Exception innerException)
		//    : base(innerException, message)
		//{
		//    ErrorCode = TypeErrorCode;
		//    m_Error = error;
		//}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryCredentialDialogException"/> class.
		/// </summary>
		/// <param name="context">The <see cref="StreamingContext"/> to recreate the exception from.</param>
		/// <param name="info">The <see cref="SerializationInfo"/> to recreate the exception from.</param>
		private QueryCredentialDialogException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ErrorCode = TypeErrorCode;
			m_Error = (QueryCredentialError)info.GetInt32("Error");
		}

		/// <summary>
		/// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference.</exception>
		/// <PermissionSet>
		/// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
		/// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("Error", (int)m_Error);
		}
	}
}
