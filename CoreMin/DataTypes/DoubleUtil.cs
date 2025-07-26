//--------------------------------------------------------------------------
// File:    DoubleUtil.cs
// Content:	Implementation of class DoubleUtil
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using SmartExpert.Calculation;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// <see cref="System.Double"/> helper and extension methods.
	/// </summary>
	public static class DoubleUtil
	{
		// Const values come from sdk\inc\crt\float.h
		private const double DblEpsilon = 2.2204460492503131E-16;
		private const double ZeroThreshold = 2.2204460492503131E-15;

		/// <summary>
		/// Performs an conversion from <see cref="System.Double"/> to <see cref="System.Int32"/>.
		/// </summary>
		/// <param name="val">The source value.</param>
		/// <returns>The result of the conversion.</returns>
		public static int ToInt(this double val)
		{
			if (val <= 0.0)
			{
				return (int)(val - 0.5);
			}
			return (int)(val + 0.5);
		}

		/// <summary>
		/// Performs an conversion from <see cref="System.Double"/> to <see cref="System.Byte"/>.
		/// Clamps the source value to the byte value range from [0 .. 255].
		/// </summary>
		/// <param name="val">The source value.</param>
		/// <returns>The result of the conversion.</returns>
		public static byte BoundToByte(this double val)
		{
			if (val <= 0.0)
				return 0;

			if (val >= 255.0)
				return 255;

			return (byte)(val + 0.5);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsCloseTo(this double value1, double value2)
		{
			return AreClose(value1, value2);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is greater than, but not close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is greater than, but not close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsGreaterThan(this double value1, double value2)
		{
			return GreaterThan(value1, value2);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is greater than or close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is greater than or close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsGreaterThanOrClose(this double value1, double value2)
		{
			return GreaterThanOrClose(value1, value2);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is less than, but not close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is less than, but not close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsLessThan(this double value1, double value2)
		{
			return LessThan(value1, value2);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is less than or close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is less than or close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsLessThanOrClose(this double value1, double value2)
		{
			return LessThanOrClose(value1, value2);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="val"/> is between 0.0 and 1.0 (inclusive).
		/// </summary>
		/// <param name="val">The value.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="val"/> is between 0.0 and 1.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsBetweenZeroAndOne(this double val)
		{
			return (val.IsGreaterThanOrClose(0.0) && val.IsLessThanOrClose(1.0));
		}

		/// <summary>
		/// Determines whether the value of <paramref name="value"/> is 1.0, or close to 1.0.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="value"/> is 1.0, or close to 1.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNearOne(this double value)
		{
			return NearOne(value);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="value"/> is 1.0, or close to 1.0.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="value"/> is 1.0, or close to 1.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool NearOne(double value)
		{
			return (Math.Abs(value - 1.0) < ZeroThreshold);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="value"/> is 0.0, or close to 0.0.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="value"/> is o.0, or close to 0.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNearZero(this double value)
		{
			return NearZero(value);
		}

		/// <summary>
		/// NearZero - Returns whether or not the double is "close" to 0.  Same as AreClose(double, 0), but this is faster.
		/// </summary>
		/// <returns>
		/// bool - the result of the AreClose comparision.
		/// </returns>
		/// <param name="value"> The double to compare to 0. </param>
		public static bool NearZero(double value)
		{
			return (Math.Abs(value) < ZeroThreshold);
		}

		/// <summary>
		/// AreClose - Returns whether or not two doubles are "close".  That is, whether or 
		/// not they are within epsilon of each other.  Note that this epsilon is proportional
		/// to the numbers themselves to that AreClose survives scalar multiplication.
		/// There are plenty of ways for this to return false even for numbers which
		/// are theoretically identical, so no code calling this should fail to work if this 
		/// returns false.  This is important enough to repeat:
		/// NB: NO CODE CALLING THIS FUNCTION SHOULD DEPEND ON ACCURATE RESULTS - this should be
		/// used for optimizations *only*.
		/// </summary>
		/// <returns>
		/// bool - the result of the AreClose comparision.
		/// </returns>
		/// <param name="value1"> The first double to compare. </param>
		/// <param name="value2"> The second double to compare. </param>
		public static bool AreClose(double value1, double value2)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (value1 == value2)
			// ReSharper restore CompareOfFloatsByEqualityOperator
			{
				return true;
			}
			double num = ((Math.Abs(value1) + Math.Abs(value2)) + 10.0) * DblEpsilon;
			double num2 = value1 - value2;
			return ((-num < num2) && (num > num2));
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is greater than, but not close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is greater than, but not close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool GreaterThan(double value1, double value2)
		{
			return ((value1 > value2) && !AreClose(value1, value2));
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is greater than or close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is greater than or close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			if (value1 <= value2)
			{
				return AreClose(value1, value2);
			}
			return true;
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is less than, but not close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is less than, but not close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool LessThan(double value1, double value2)
		{
			return ((value1 < value2) && !AreClose(value1, value2));
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is less than or close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is less than or close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool LessThanOrClose(double value1, double value2)
		{
			if (value1 >= value2)
			{
				return AreClose(value1, value2);
			}
			return true;
		}

		/// <summary>
		/// This is a variant of mod that wraps the mod result to avoid negative results. this is what Python's mod operator does.
		/// </summary>
		/// <param name="x">x value</param>
		/// <param name="y">y value</param>
		/// <returns>x modulo y and wrap result to a non negative value.</returns>
		public static double ModWrapAngle(double x, double y)
		{
			if (Math.Abs(y - 0.0) < double.Epsilon)
			{
				throw new DivideByZeroException();
			}

			double r = x % y;
			if (r > 0 && y < 0)
			{
				r = r + y;
			}
			else if (r < 0 && y > 0)
			{
				r = r + y;
			}
			return r;
		}


		/// <summary>
		/// Wraps a rad angle value into a range from 0 to 1.
		/// </summary>
		/// <param name="radians">The rad angle value.</param>
		/// <returns>The wrapped number.</returns>
		public static double RadiansToRangeFrom0To1(double radians)
		{
			return WrapAngleFrom0To1(radians / (Math.PI * 2));
		}

		/// <summary>
		/// Wraps a number around so that it always fits between 0.0 and 1.0. negative numbers will wrap around 
		/// to the correct positive number.
		/// </summary>
		/// <remarks>
		/// if the input number is already in the range, no change will occur
		/// </remarks>
		/// <param name="v">input value </param>
		/// <returns>The wrapped number</returns>
		public static double WrapAngleFrom0To1(double v)
		{
			const double min = 0.0;
			const double max = 1.0;
			
			if (v.IsInRange(min, max))
			{
				// the number is already in the range so do nothing
				return v;
			}

			return ModWrapAngle(v, max);
		}

		/// <summary>
		/// Rounds value to the nearest fractional value.
		/// </summary>
		/// <param name="value">the value to round</param>
		/// <param name="snapValue"> round to this value (must be greater than 0.0)</param>
		/// <returns>the rounded value</returns>
		public static double Round(double value, double snapValue)
		{
			return Round(value, MidpointRounding.AwayFromZero, snapValue);
		}

		/// <summary>
		/// rounds val to the nearest fractional value 
		/// </summary>
		/// <param name="value">the value to round</param>
		/// <param name="rounding">what kind of rounding</param>
		/// <param name="snapValue"> round to this value (must be greater than 0.0)</param>
		/// <returns>the rounded value</returns>
		public static double Round(double value, MidpointRounding rounding, double snapValue)
		{
			if (snapValue <= 0)
			{
				throw new ArgumentOutOfRangeException("snapValue", "must be greater than or equal to 0.0");
			}
			double retval = Math.Round((value / snapValue), rounding) * snapValue;
			return retval;
		}

		/// <summary>
		/// Rounds up to the nearest even number.
		/// </summary>
		/// <param name="v">The value.</param>
		/// <param name="amount">The amount.</param>
		/// <returns></returns>
		public static double RoundUp(double v, double amount)
		{
			const MidpointRounding rounding = MidpointRounding.ToEven;
			var result = Round(v + (amount / 2.0), rounding, amount);
			return result;
		}

		/// <summary>
		/// Determines whether <paramref name="number"/> is a positive real number ( number > 0).
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is greater than zero; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsPositiveRealNumber(this double number)
		{
			return IsRealNumber(number) && number > 0;
		}

		/// <summary>
		/// Determines whether <paramref name="number"/> is a negative real number ( number &lt; 0).
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is less than zero; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNegativeRealNumber(this double number)
		{
			return IsRealNumber(number) && number < 0;
		}

		/// <summary>
		/// Determines whether <paramref name="number"/> is a real number => number is not NaN and not Infinity!
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is a real number; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsRealNumber(this double number)
		{
			return !double.IsNaN(number) && !double.IsInfinity(number);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="number"/> is not a number (NaN)!
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is not a number (NaN); otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNaN(this double number)
		{
			var union = new NanUnion
			{
				DoubleValue = number
			};
			ulong num = union.UlongValue & 0xfff0000000000000;
			ulong num2 = union.UlongValue & 0xfffffffffffffL;
			if ((num != 0x7ff0000000000000L) && (num != 0xfff0000000000000))
			{
				return false;
			}
			return (num2 != 0L);
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct NanUnion
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
			[FieldOffset(0)]
			internal double DoubleValue;
			[FieldOffset(0)]
			internal readonly ulong UlongValue;
		}
	}
}