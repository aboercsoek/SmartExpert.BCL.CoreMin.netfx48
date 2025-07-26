//--------------------------------------------------------------------------
// File:    CollectionReadOnlyException.cs
// Content:	Implementation of class CollectionReadOnlyException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
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
	/// <summary>
	/// This exception is thrown when write access against a read only collection is tried.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	[SoapType(Namespace = "http://www.smartexpert.de/core/libs/2010")]
	public class CollectionReadOnlyException : TechException
	{
		private const int TypeErrorCode = 1019;
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		public CollectionReadOnlyException() { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message
		/// </summary>
		/// <param name="message">The error message</param>
		public CollectionReadOnlyException(string message) : base(message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class with the specified message and inner exception
		/// </summary>
		/// <param name="message">The error message</param>
		/// <param name="cause">The error which causes this exception.</param>
		public CollectionReadOnlyException(string message, Exception cause) : base(cause, message) { ErrorCode = TypeErrorCode; }
		/// <summary>
		/// Creates a new instance of this class using the specified 
		/// <see cref="System.Runtime.Serialization.SerializationInfo"/> and
		/// <see cref="System.Runtime.Serialization.StreamingContext" /> Scope.
		/// </summary>
		/// <param name="info">A <see cref="System.Runtime.Serialization.SerializationInfo"/>.</param>
		/// <param name="scope">A <see cref="System.Runtime.Serialization.StreamingContext" />.</param>
		protected CollectionReadOnlyException(
		  SerializationInfo info,
		  StreamingContext scope)
			: base(info, scope) { }
	}
}
