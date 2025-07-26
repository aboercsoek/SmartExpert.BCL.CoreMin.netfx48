//--------------------------------------------------------------------------
// File:    ArgException.cs
// Content:	Implementation of class ArgException
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
	///<summary>General argument validation failed exception.</summary>
	/// <typeparam name="TValue">The Argument type.</typeparam>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgException<TValue> : TechException
	{
		private const int TypeErrorCode = 1001;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgException{TValue}"/> class.
		/// </summary>
		/// <param name="argName">The argument name.</param>
		public ArgException( string argName )
			: base(StringResources.ErrorArgumentTemplate1Arg.SafeFormatWith(argName))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgException{TValue}"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		public ArgException( TValue argValue, string argName )
			: base(StringResources.ErrorArgumentValidationFailedTemplate2Args.SafeFormatWith(argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgException{TValue}"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The error message.</param>
		public ArgException( TValue argValue, string argName, string message )
			: base(message.IsFormatString() ? message.SafeFormatWith(argName, argValue) : "{0} [Arg:{1},Value:{2}]".SafeFormatWith(message, argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgException{TValue}"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgException( SerializationInfo info, StreamingContext context )
			: base(info, context)
		{
		}
	}
}
