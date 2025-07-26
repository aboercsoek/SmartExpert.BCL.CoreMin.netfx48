//--------------------------------------------------------------------------
// File:    Win32ExecutionException.cs
// Content:	Implementation of class Win32ExecutionException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Error
{
	///<summary>Win32 execution failure exception.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class Win32ExecutionException : TechException
	{
		private const int TypeErrorCode = 1010;

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		public Win32ExecutionException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public Win32ExecutionException( string message, params object[] formatParameters )
			: base(formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		/// <param name="inner">The inner exception.</param>
		/// <param name="message">The error message.</param>
		/// <param name="formatParameters">The error message format parameters.</param>
		public Win32ExecutionException( Exception inner, string message, params object[] formatParameters )
			: base(inner, formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		/// <param name="lastWin32Error">Last Win32 error number.</param>
		/// <param name="win32FuncName">Win32 function name where the error occured.</param>
		public Win32ExecutionException( int lastWin32Error, string win32FuncName )
			: base(StringHelper.SafeFormat(StringResources.ErrorWin32ExecutionFailedTemplate2Args, lastWin32Error, win32FuncName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		/// <param name="lastWin32Error">Last Win32 error number.</param>
		/// <param name="win32FuncName">Win32 function name where the error occured.</param>
		/// <param name="message">The message.</param>
		public Win32ExecutionException( int lastWin32Error, string win32FuncName, string message )
			: base(StringHelper.SafeFormat(message ?? StringResources.ErrorWin32ExecutionFailedTemplate2Args, lastWin32Error, win32FuncName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Win32ExecutionException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected Win32ExecutionException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
