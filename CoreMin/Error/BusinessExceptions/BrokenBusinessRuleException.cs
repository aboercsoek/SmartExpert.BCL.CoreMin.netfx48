//--------------------------------------------------------------------------
// File:    BrokenBusinessRuleException.cs
// Content:	Implementation of class BrokenBusinessRuleException
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


#endregion

namespace SmartExpert.Error
{
	///<summary>This exception is used to mark business rule violations.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class BrokenBusinessRuleException : BusinessException
	{
		private const int TypeErrorCode = 2001;

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenBusinessRuleException"/> class.
		/// </summary>
		public BrokenBusinessRuleException() : base(StringResources.ErrorBusinessRuleViolation) { ErrorCode = TypeErrorCode; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenBusinessRuleException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BrokenBusinessRuleException(string message, params object[] formatParameters)
			: base(formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenBusinessRuleException"/> class.
		/// </summary>
		/// <param name="cause">The inner.</param>
		public BrokenBusinessRuleException(Exception cause)
			: base(StringResources.ErrorBusinessRuleViolation, cause)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenBusinessRuleException"/> class.
		/// </summary>
		/// <param name="cause">The inner.</param>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BrokenBusinessRuleException(Exception cause, string message, params object[] formatParameters)
			: base(
				formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message, cause)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BrokenBusinessRuleException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected BrokenBusinessRuleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ErrorCode = TypeErrorCode;
		}
	}
}
