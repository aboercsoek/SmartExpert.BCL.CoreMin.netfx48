//--------------------------------------------------------------------------
// File:    TypeOf.cs
// Content:	Implementation of class TypeOf
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

// ReSharper disable CheckNamespace
namespace SmartExpert
{
// ReSharper restore CheckNamespace

	///<summary>Common Type provider class</summary>
	public class TypeOf
	{
		/// <summary>
		/// System.Type Type
		/// </summary>
		public static readonly Type Type = typeof(Type);
		/// <summary>
		/// Bool Type
		/// </summary>
		public static readonly Type Boolean = typeof(bool);
		/// <summary>
		/// Int Type
		/// </summary>
		public static readonly Type Byte = typeof(Byte);
		/// <summary>
		/// Short Type
		/// </summary>
		public static readonly Type Int16 = typeof(Int16);
		/// <summary>
		/// Int Type
		/// </summary>
		public static readonly Type Int32 = typeof(Int32);
		/// <summary>
		/// Long Type
		/// </summary>
		public static readonly Type Int64 = typeof(Int64);
		/// <summary>
		/// Unsinged short Type
		/// </summary>
		public static readonly Type UInt16 = typeof(UInt16);
		/// <summary>
		/// Unsinged int Type
		/// </summary>
		public static readonly Type UInt32 = typeof(UInt32);
		/// <summary>
		/// Unsinged long Type
		/// </summary>
		public static readonly Type UInt64 = typeof(UInt64);
		/// <summary>
		/// Single (float) Type
		/// </summary>
		public static readonly Type Single = typeof(Single);
		/// <summary>
		/// Double Type
		/// </summary>
		public static readonly Type Double = typeof(Double);
		/// <summary>
		/// Double Type
		/// </summary>
		public static readonly Type Decimal = typeof(Decimal);
		/// <summary>
		/// Object Type
		/// </summary>
		public static readonly Type Object = typeof(Object);
		/// <summary>
		/// Char Type
		/// </summary>
		public static readonly Type Char = typeof(Char);
		/// <summary>
		/// String Type
		/// </summary>
		public static readonly Type String = typeof(String);
		/// <summary>
		/// TimeSpan Type
		/// </summary>
		public static readonly Type TimeSpan = typeof(TimeSpan);
		/// <summary>
		/// DateTime Type
		/// </summary>
		public static readonly Type DateTime = typeof(DateTime);
		/// <summary>
		/// Guid Type
		/// </summary>
		public static readonly Type Guid = typeof(Guid);
		/// <summary>
		/// void Type
		/// </summary>
		public static readonly Type Void = typeof(void);
	}
}
