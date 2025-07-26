//--------------------------------------------------------------------------
// File:    SerializationFormatters.cs
// Content:	Definition of enumaretion SerializationFormatters
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives



#endregion

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


namespace SmartExpert.Serialization
{
	/// <summary>
	/// Enum representing the serialization formatters supported by SmartExpert Framework.
	/// </summary>
	public enum SerializationFormatters
	{
		/// <summary>
		/// Use the standard Microsoft .NET <see cref="XmlSerializer"/>.
		/// </summary>
		XmlSerializer,
		/// <summary>
		/// Use the standard Microsoft .NET <see cref="BinaryFormatter"/>.
		/// </summary>
		BinaryFormatter,
		/// <summary>
		/// Use the Microsoft .NET 3.0
		/// <see cref="System.Runtime.Serialization.DataContractSerializer">DataContractSerializer</see> 
		/// provided as part of WCF.
		/// </summary>
		DataContractSerializer,
		/// <summary>
		/// Use the Microsoft .NET 3.0
		/// <see cref="System.Runtime.Serialization.NetDataContractSerializer">NetDataContractSerializer</see> 
		/// provided as part of WCF.
		/// </summary>
		NetDataContractSerializer
	}
}
