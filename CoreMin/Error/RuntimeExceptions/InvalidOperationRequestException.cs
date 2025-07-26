//--------------------------------------------------------------------------
// File:    InvalidOperationRequestException.cs
// Content:	Implementation of class InvalidOperationRequestException
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
	/// Thrown when an operation request is denyed due to the state of the object.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class InvalidOperationRequestException : TechException
	{
		private const int TypeErrorCode = 1011;

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidOperationRequestException"/> class.
		/// </summary>
		public InvalidOperationRequestException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidOperationRequestException"/> class.
		/// </summary>
		/// <param name="operation">The invalid requested operation name.</param>
		public InvalidOperationRequestException( string operation )
			: base(StringResources.ErrorInvalidOperationRequestTemplate1Arg.SafeFormatWith(operation))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidOperationRequestException"/> class.
		/// </summary>
		/// <param name="operation">The invalid requested operation name.</param>
		/// <param name="message">The error message.</param>
		public InvalidOperationRequestException(string operation, string message)
			: base(StringResources.ErrorInvalidOperationRequestTemplate1Arg.SafeFormatWith(operation) + "\n" + message.SafeString())
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidOperationRequestException"/> class.
		/// </summary>
		/// <param name="operation">The invalid requested operation name.</param>
		/// <param name="cause">The error which causes this exception.</param>
		public InvalidOperationRequestException(string operation, Exception cause)
			: base(cause, StringResources.ErrorInvalidOperationRequestTemplate1Arg.SafeFormatWith(operation))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidOperationRequestException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected InvalidOperationRequestException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context )
			: base(info, context)
		{
		}
	}
}
