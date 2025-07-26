//--------------------------------------------------------------------------
// File:    ArgNullOrEmptyException.cs
// Content:	Implementation of class ArgNullOrEmptyException
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
	///<summary>Argument is null or empty validation exception class</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgNullOrEmptyException : TechException
	{
		private const int TypeErrorCode = 1004;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullOrEmptyException"/> class.
		/// </summary>
		public ArgNullOrEmptyException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullOrEmptyException"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		public ArgNullOrEmptyException( object argValue, string argName )
			: base(StringResources.ErrorShouldNotBeNullOrEmptyValidationTemplate2Args.SafeFormatWith(argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullOrEmptyException"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The message.</param>
		public ArgNullOrEmptyException( object argValue, string argName, string message )
			: base(message.SafeFormatWith(argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgNullOrEmptyException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgNullOrEmptyException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
