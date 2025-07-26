//--------------------------------------------------------------------------
// File:    OperationExecutionFailedException.cs
// Content:	Implementation of class OperationExecutionFailedException
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
	/// Thrown when an error occurs during operation execution.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class OperationExecutionFailedException : TechException
	{
		private const int TypeErrorCode = 1013;

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationExecutionFailedException"/> class.
		/// </summary>
		public OperationExecutionFailedException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationExecutionFailedException"/> class.
		/// </summary>
		/// <param name="operation">The operation.</param>
		public OperationExecutionFailedException( string operation )
			: base(StringResources.ErrorOperationExecutionFailedTemplate1Arg.SafeFormatWith(operation))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationExecutionFailedException"/> class.
		/// </summary>
		/// <param name="operation">The name of the operation that failed.</param>
		/// <param name="cause">The error which causes this exception.</param>
		public OperationExecutionFailedException(string operation, Exception cause)
			: base(cause, StringResources.ErrorOperationExecutionFailedTemplate1Arg.SafeFormatWith(operation))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationExecutionFailedException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected OperationExecutionFailedException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context )
			: base(info, context)
		{
		}
	}
}
