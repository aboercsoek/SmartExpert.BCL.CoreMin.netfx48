//--------------------------------------------------------------------------
// File:    UnitConverter.cs
// Content:	Implementation of class UnitConverter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Calculation
{

	/// <summary>
	/// The math unit converter class
	/// </summary>
	public static class UnitConverter
	{
		/// <summary>
		/// Converts Degrees to radians
		/// </summary>
		/// <param name="deg">The degree value [0 .. 360]</param>
		/// <returns>The corresponding radian value.</returns>
		public static double DegreesToRadians( double deg )
		{
			double rad = deg * Math.PI / 180.0;
			return rad;
		}

		/// <summary>
		/// Converts Radians to Degrees
		/// </summary>
		/// <param name="rad">The radian value [0 .. 2*PI].</param>
		/// <returns>The corresponding degree value.</returns>
		public static double RadiansToDegrees( double rad )
		{
			return ( 180 / Math.PI ) * rad;
		}
	}
}
