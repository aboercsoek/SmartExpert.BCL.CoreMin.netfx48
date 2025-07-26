//--------------------------------------------------------------------------
// File:    ProcessRunnerException.cs
// Content:	Implementation of class ProcessRunnerException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// An exception thrown during process executuion in ProcessRunner class."/>
	/// instance.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ProcessRunnerException : TechException
	{
		private const int TypeErrorCode = 1014;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessRunnerException"/> class.
		/// </summary>
		public ProcessRunnerException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessRunnerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ProcessRunnerException( string message )
			: base(message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessRunnerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="cause">The error which causes this exception.</param>
		public ProcessRunnerException( string message, Exception cause )
			: base(cause, message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessRunnerException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ProcessRunnerException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context )
			: base(info, context)
		{
		}
	}
}
