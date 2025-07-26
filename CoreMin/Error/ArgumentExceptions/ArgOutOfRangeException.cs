//--------------------------------------------------------------------------
// File:    ArgOutOfRangeException.cs
// Content:	Implementation of class ArgOutOfRangeException
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
	///<summary>Argument out of range validation exception class.</summary>
	/// <typeparam name="TValue">The Argument type.</typeparam>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgOutOfRangeException<TValue> : TechException
	{
		private const int TypeErrorCode = 1005;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgOutOfRangeException{TValue}"/> class.
		/// </summary>
		public ArgOutOfRangeException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgOutOfRangeException{TValue}"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		public ArgOutOfRangeException( TValue argValue, string argName )
			: base(StringResources.ErrorArgumentOutOfRangeValidationTemplate4Args.SafeFormatWith(argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgOutOfRangeException{TValue}"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="validMinValue">Valid min value.</param>
		/// <param name="validMaxValue">Valid max value.</param>
		public ArgOutOfRangeException( TValue argValue, string argName, TValue validMinValue, TValue validMaxValue )
			: base(StringResources.ErrorArgumentOutOfRangeValidationWithRangeTemplate4Args.SafeFormatWith(argName, argValue, validMinValue, validMaxValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgOutOfRangeException{TValue}"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The message template.</param>
		public ArgOutOfRangeException( TValue argValue, string argName, string message )
			: base(message.IsFormatString() ? message.SafeFormatWith(argName, argValue) : "{0} [Arg:{1},Value:{2}]".SafeFormatWith(message, argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TechException"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="cause">The error which causes this exception.</param>
		/// <param name="message">The message template.</param>
		public ArgOutOfRangeException( TValue argValue, string argName, Exception cause, string message )
			: base(cause, message.IsFormatString() ? message.SafeFormatWith(argName, argValue) : "{0} [Arg:{1},Value:{2}]".SafeFormatWith(message, argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ArgOutOfRangeException{TValue}"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgOutOfRangeException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
