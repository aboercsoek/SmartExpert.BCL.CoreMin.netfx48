//--------------------------------------------------------------------------
// File:    NameGenerator.cs
// Content:	Implementation of class NameGenerator
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{

	/// <summary>
	/// A unique name generator (Format: _{Guid}_{Id}). 
	/// The guid prefix will be generated only once during application lifetime.
	/// The Id value is a long number, that will be incremented by 1 in an 
	/// atomic operation when Next ist called.
	/// </summary>
	public class NameGenerator
	{
		private long m_Id;
		private static readonly NameGenerator Instance = new NameGenerator();
		private readonly string m_Prefix = ("_" + Guid.NewGuid().ToString().Replace('-', '_') + "_");

		private NameGenerator()
		{
		}

		/// <summary>
		/// Get the next unique name
		/// </summary>
		/// <returns>The unique name</returns>
		public static string Next()
		{
			long num = Interlocked.Increment(ref Instance.m_Id);
			return (Instance.m_Prefix + num.ToString(CultureInfo.InvariantCulture));
		}
	}


}
