//--------------------------------------------------------------------------
// File:    BoolExtensions.cs
// Content:	Implementation of class BoolExtensions
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

namespace SmartExpert
{
	///<summary><see cref="bool"/> extension methods.</summary>
	public static class BoolExtensions
	{
		/// <summary>
		/// Determines whether the specified bool value is false.
		/// </summary>
		/// <param name="boolValue">The bool value.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified bool value is false; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsFalse(this bool boolValue)
		{
			return (boolValue == false);
		}

		/// <summary>
		/// Determines whether the specified bool value is true.
		/// </summary>
		/// <param name="boolValue">The bool value.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified bool value is <see langword="true"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsTrue(this bool boolValue)
		{
			return boolValue;
		}
	}
}
