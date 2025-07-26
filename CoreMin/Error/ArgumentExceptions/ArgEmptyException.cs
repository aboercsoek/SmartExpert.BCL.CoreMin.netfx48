//--------------------------------------------------------------------------
// File:    ArgEmptyException.cs
// Content:	Implementation of class ArgEmptyException
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
	///<summary>Argument is empty validation exception class.</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgEmptyException : TechException
	{
		private const int TypeErrorCode = 1003;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgEmptyException"/> class.
		/// </summary>
		public ArgEmptyException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgEmptyException"/> class.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		public ArgEmptyException( string argName )
			: base(StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg.SafeFormatWith(argName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgEmptyException"/> class.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The message.</param>
		public ArgEmptyException( string argName, string message )
			: base(message.IsFormatString() ? message.SafeFormatWith(argName) : "{0} [Arg:{1}]".SafeFormatWith(message, argName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgEmptyException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgEmptyException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
