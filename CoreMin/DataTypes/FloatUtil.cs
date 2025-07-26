//--------------------------------------------------------------------------
// File:    FloatUtil.cs
// Content:	Implementation of class FloatUtil
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using SmartExpert.Calculation;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// <see cref="System.Single">Float</see> helper and extension methods.
	/// </summary>
	public static class FloatUtil
	{
		internal const float FltEpsilon = 1.192093E-07f;
		internal const float FltMaxPrecision = 1.677722E+07f;
		internal const float InverseFltMaxPrecision = (1f / FltMaxPrecision);

		/// <summary>
		/// Determines whether numerator/denominator is close to a divide by zero condition.
		/// </summary>
		/// <param name="numerator">The numerator.</param>
		/// <param name="denominator">The denominator.</param>
		/// <returns>
		///   <see langword="true"/> if numerator/denominator is close to a divide by zero condition; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsCloseToDivideByZero(float numerator, float denominator)
		{
			return (Math.Abs(denominator) <= (Math.Abs(numerator) * InverseFltMaxPrecision));
		}

		/// <summary>
		/// Performs an conversion from <see cref="System.Single">float</see> to <see cref="System.Int32"/>.
		/// </summary>
		/// <param name="val">The source value.</param>
		/// <returns>The int result of the conversion.</returns>
		public static int ToInt(this float val)
		{
			if (val <= 0.0f)
			{
				return (int)(val - 0.5f);
			}
			return (int)(val + 0.5f);
		}

		/// <summary>
		/// Performs an conversion from <see cref="System.Single" /> to <see cref="System.Byte"/>.
		/// Clamps the source value to the byte value range from [0 .. 255].
		/// </summary>
		/// <param name="val">The source value.</param>
		/// <returns>The result of the conversion.</returns>
		public static byte ClampToByte(this float val)
		{
			if (val <= 0.0f)
				return 0;

			if (val >= 255.0f)
				return 255;

			return (byte)(val + 0.5f);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsCloseTo(this float value1, float value2)
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
		public static bool IsGreaterThan(this float value1, float value2)
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
		public static bool IsGreaterThanOrClose(this float value1, float value2)
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
		public static bool IsLessThan(this float value1, float value2)
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
		public static bool IsLessThanOrClose(this float value1, float value2)
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
		public static bool IsBetweenZeroAndOne(this float val)
		{
			return (val.IsGreaterThanOrClose(0.0f) && val.IsLessThanOrClose(1.0f));
		}

		/// <summary>
		/// Determines whether the value of <paramref name="value"/> is 1.0, or close to 1.0.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="value"/> is 1.0, or close to 1.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsOne(this float value)
		{
			return (Math.Abs(value - 1.0f) < FltEpsilon * 10.0f);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="value"/> is 0.0, or close to 0.0.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if the value of <paramref name="value"/> is o.0, or close to 0.0; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsZero(this float value)
		{
			return (Math.Abs(value) < FltEpsilon * 10.0f);
		}

		/// <summary>
		/// Determines whether <paramref name="value1"/> is close to <paramref name="value2"/>.
		/// </summary>
		/// <param name="value1">The first value.</param>
		/// <param name="value2">The second value.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="value1"/> is close to <paramref name="value2"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool AreClose(float value1, float value2)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (value1 == value2)
			// ReSharper restore CompareOfFloatsByEqualityOperator
			{
				return true;
			}
			float num = ((Math.Abs(value1) + Math.Abs(value2)) + 10.0f) * FltEpsilon;
			float num2 = value1 - value2;
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
		public static bool GreaterThan(float value1, float value2)
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
		public static bool GreaterThanOrClose(float value1, float value2)
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
		public static bool LessThan(float value1, float value2)
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
		public static bool LessThanOrClose(float value1, float value2)
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
		public static float ModWrapAngle(float x, float y)
		{
			if (IsCloseToDivideByZero(x, y))
			{
				throw new DivideByZeroException();
			}

			float r = x % y;
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
		/// Wraps a number around so that it always fits between 0.0 and 1.0. negative numbers will wrap around 
		/// to the correct positive number.
		/// </summary>
		/// <remarks>
		/// if the input number is already in the range, no change will occur
		/// </remarks>
		/// <param name="v">input value </param>
		/// <returns>The wrapped number</returns>
		public static float WrapAngleFrom0To1(float v)
		{
			const float min = 0.0f;
			const float max = 1.0f;

			if (v.IsInRange(min, max))
			{
				// the number is already in the range so do nothing
				return v;
			}

			return ModWrapAngle(v, max);
		}

		/// <summary>
		/// Wraps a rad angle value into a range from 0 to 1.
		/// </summary>
		/// <param name="radians">The rad angle value.</param>
		/// <returns>The wrapped number.</returns>
		public static float RadiansToRangeFrom0To1(float radians)
		{
			return WrapAngleFrom0To1((radians / (Math.PI * 2.0f)).As<float>());
		}
		

		/// <summary>
		/// Rounds value to the nearest fractional value.
		/// </summary>
		/// <param name="value">the value to round</param>
		/// <param name="snapValue"> round to this value (must be greater than 0.0)</param>
		/// <returns>the rounded value</returns>
		public static float Round(float value, float snapValue)
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
		public static float Round(float value, MidpointRounding rounding, float snapValue)
		{
			if (snapValue <= 0f)
			{
				throw new ArgumentOutOfRangeException("snapValue", "must be greater than or equal to 0.0");
			}
			double retval = Math.Round((value / snapValue), rounding) * snapValue;
			return retval.As<float>();
		}

		/// <summary>
		/// Rounds up to the nearest even number.
		/// </summary>
		/// <param name="v">The value.</param>
		/// <param name="amount">The amount.</param>
		/// <returns></returns>
		public static float RoundUp(float v, float amount)
		{
			const MidpointRounding rounding = MidpointRounding.ToEven;
			float result = Round(v + (amount / 2.0f), rounding, amount);
			return result;
		}


		/// <summary>
		/// Determines whether <paramref name="number"/> is a positive real number ( number > 0).
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is greater than zero; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsPositiveRealNumber(this float number)
		{
			return IsRealNumber(number) && number > 0f;
		}

		/// <summary>
		/// Determines whether <paramref name="number"/> is a negative real number ( number &lt; 0).
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is less than zero; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNegativeRealNumber(this float number)
		{
			return IsRealNumber(number) && number < 0f;
		}

		/// <summary>
		/// Determines whether <paramref name="number"/> is a real number => number is not NaN and not Infinity!
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is a real number; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsRealNumber(this float number)
		{
			return !float.IsNaN(number) && !float.IsInfinity(number);
		}

		/// <summary>
		/// Determines whether the value of <paramref name="number"/> is not a number (NaN)!
		/// </summary>
		/// <param name="number">The number to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="number"/> is not a number (NaN); otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNaN(this float number)
		{
			return float.IsNaN(number);
		}

	}
}