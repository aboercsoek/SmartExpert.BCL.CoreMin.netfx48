//--------------------------------------------------------------------------
// File:    ArgNullException.cs
// Content:	Implementation of class ArgNullException
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
	///<summary>Argument is null validation exception class.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgNullException : TechException
	{
		private const int TypeErrorCode = 1002;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		public ArgNullException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		public ArgNullException( string argName )
			: base(StringResources.ErrorShouldNotBeNullValidationTemplate1Arg.SafeFormatWith(argName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The message.</param>
		public ArgNullException( string argName, string message )
			: base(message.IsFormatString() ? message.SafeFormatWith(argName) : "{0} [Arg:{1}]".SafeFormatWith(message, argName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public ArgNullException( string message, params object[] formatParameters )
			: base( (formatParameters != null && formatParameters.Length > 0 && message.IsFormatString()) ? message.SafeFormatWith(formatParameters) : message)
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		/// <param name="inner">The inner.</param>
		/// <param name="message">The message.</param>
		/// <param name="formatParameters">The format parameters.</param>
		public ArgNullException( Exception inner, string message, params object[] formatParameters )
			: base(
				formatParameters != null && formatParameters.Length > 0 ? message.SafeFormatWith(formatParameters) : message, inner)
		{
			ErrorCode = TypeErrorCode;
		}

		
		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgNullException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
