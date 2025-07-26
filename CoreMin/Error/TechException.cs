//--------------------------------------------------------------------------
// File:    TechException.cs
// Content:	Implementation of class TechException
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
	///<summary>Base Exception class for all technical errors.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class TechException : BaseException
	{
		// Current TechException Max TypeErrorCode = 1025
		private const int TypeErrorCode = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="TechException"/> class.
		/// </summary>
		public TechException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TechException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public TechException(string message, params object[] formatParameters)
			: base(formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TechException"/> class.
		/// </summary>
		/// <param name="inner">The error which causes this exception.</param>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public TechException(Exception inner, string message, params object[] formatParameters)
			: base(inner,
				formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TechException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected TechException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
			ErrorCode = TypeErrorCode;
		}
	}
}
