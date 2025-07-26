//--------------------------------------------------------------------------
// File:    BusinessException.cs
// Content:	Implementation of class BusinessException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

namespace SmartExpert.Error
{
	///<summary>Base Exception class for all business errors.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class BusinessException : BaseException
	{
		// Current BusinessException Max TypeErrorCode = 2001
		private const int TypeErrorCode = 2000;

		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessException"/> class.
		/// </summary>
		public BusinessException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BusinessException(string message, params object[] formatParameters)
			: base(formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessException"/> class.
		/// </summary>
		/// <param name="inner">The error which causes this exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public BusinessException(Exception inner, string message, params object[] formatParameters)
			: base(inner,
				formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BusinessException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected BusinessException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
			ErrorCode = TypeErrorCode;
		}
	}
}
