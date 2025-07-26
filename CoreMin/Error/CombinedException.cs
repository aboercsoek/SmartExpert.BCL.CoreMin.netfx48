//--------------------------------------------------------------------------
// File:    CombinedException.cs
// Content:	Implementation of class CombinedException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Error
{
	/// <summary>
	/// Generic exception for combining several other exceptions
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class CombinedException : TechException
	{
		private const int TypeErrorCode = 1009;

		/// <summary>
		/// Initializes a new instance of the <see cref="CombinedException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="innerExceptions">All exceptions that are combined by this exception.</param>
		public CombinedException( string message, IEnumerable<Exception> innerExceptions )
			: base(message)
		{
			ErrorCode = TypeErrorCode;
			InnerExceptions = innerExceptions;
		}

		/// <summary>
		/// Gets The errors that caused this exception.
		/// </summary>
		/// <value>The errors that caused this exception.</value>
		public IEnumerable<Exception> InnerExceptions { get; protected set; }
	}
}
