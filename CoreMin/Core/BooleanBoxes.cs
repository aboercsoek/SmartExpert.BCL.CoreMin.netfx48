//--------------------------------------------------------------------------
// File:    BooleanBoxes.cs
// Content:	Implementation of class BooleanBoxes
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;

#endregion

namespace SmartExpert
{
	///<summary>BooleanBoxes optimizes performance for boxing boolean values, by holding two boxed intances for <see langword="true"/> and <see langword="false"/> values.</summary>
	public static class BooleanBoxes
	{
		/// <summary>Boxed boolean instance, for <see langword="false"/> value.</summary>
		public static readonly object FalseBox = false;
		/// <summary>Boxed boolean instance, for <see langword="true"/> value.</summary>
		public static readonly object TrueBox = true;

		/// <summary>
		/// Performance optimized boxing of bool values.
		/// </summary>
		/// <param name="value">boolen value to box.</param>
		/// <returns>Returns <see cref="TrueBox"/> if <paramref name="value"/> is <see langword="true"/>, 
		/// or <see cref="FalseBox"/> if <paramref name="value"/> is <see langword="false"/>.</returns>
		public static object Box(bool value)
		{
			return value ? TrueBox : FalseBox;
		}
	}

}
