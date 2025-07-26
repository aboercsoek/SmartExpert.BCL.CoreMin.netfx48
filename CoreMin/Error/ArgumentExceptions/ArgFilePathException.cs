//--------------------------------------------------------------------------
// File:    ArgFilePathException.cs
// Content:	Implementation of class ArgFilePathException
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
	///<summary>FilePath provided by Argument is not valid exception class</summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class ArgFilePathException : TechException
	{
		private const int TypeErrorCode = 1006;

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgFilePathException"/> class.
		/// </summary>
		public ArgFilePathException()
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgFilePathException"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		public ArgFilePathException( object argValue, string argName )
			: base(StringResources.ErrorArgumentFilePathExceptionTemplate2Args.SafeFormatWith(argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgFilePathException"/> class.
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The argument name.</param>
		/// <param name="message">The error message.</param>
		public ArgFilePathException( object argValue, string argName, string message )
			: base("{0} [Arg:{1},Value:{2}]".SafeFormatWith(message, argName, argValue))
		{
			ErrorCode = TypeErrorCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgFilePathException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected ArgFilePathException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ErrorCode = TypeErrorCode;
		}
	}
}
