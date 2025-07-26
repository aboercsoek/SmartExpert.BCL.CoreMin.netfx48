//--------------------------------------------------------------------------
// File:    MathUtil.cs
// Content:	Implementation of class MathUtil
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SmartExpert.Calculation
{
	/// <summary>
	/// Provides static methods not included in the standard Math class.
	/// </summary>
	public static class MathUtil
	{
		#region Clamp Methods

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static byte Clamp(this byte value, byte min, byte max)
		{
			byte clampedValue = (value > max) ? max : value;

			return (clampedValue < min) ? min : clampedValue;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static int Clamp(this int value, int min, int max)
		{
			int clampedValue = (value > max) ? max : value;

			return (clampedValue < min) ? min : clampedValue;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static float Clamp(this float value, float min, float max)
		{
			float clampedValue = (value > max) ? max : value;

			return (clampedValue < min) ? min : clampedValue;
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>The clamped value.</returns>
		public static double Clamp(this double value, double min, double max)
		{
			double clampedValue = (value > max) ? max : value;

			return (clampedValue < min) ? min: clampedValue;
		}


		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="minimum">The minimum value.</param>
		/// <param name="maximum">The maximum value.</param>
		/// <param name="precision">The rounding precision value.</param>
		/// <returns>The clamped value.</returns>
		public static float Clamp(this float value, float minimum, float maximum, int precision)
		{
			float coercedValue = value.Clamp(minimum, maximum);

			return Math.Round(coercedValue, precision).As<float>();
		}

		/// <summary>
		/// Restricts a value to be within a specified range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="minimum">The minimum value.</param>
		/// <param name="maximum">The maximum value.</param>
		/// <param name="precision">The rounding precision value.</param>
		/// <returns>The clamped value.</returns>
		public static double Clamp(this double value, double minimum, double maximum, int precision)
		{
			double coercedValue = value.Clamp(minimum, maximum);
			return Math.Round(coercedValue, precision);
		}

		/// <summary>
		/// Given an input value, will limit it to the range 0.0f and 1.0f inclusive
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <returns>The clamped value</returns>
		public static float ClampToRange0To1(this float value)
		{
			return value.Clamp(0.0f, 1.0f);
		}

		/// <summary>
		/// Given an input value, will limit it to the range 0.0 and 1.0 inclusive
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <returns>The clamped value</returns>
		public static double ClampToRange0To1(this double value)
		{
			return value.Clamp(0.0, 1.0);
		}

		#endregion

		#region IsInRange Methods

		/// <summary>
		/// Checks if a value is within a specified range
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <param name="min">The minimum value.</param>
		/// <param name="max">The maximum value.</param>
		/// <returns>True if the values is within the range, fals otherwise</returns>
		public static bool IsInRange(this int value, int min, int max)
		{
			return value >= min && value <= max;
		}

		/// <summary>
		/// Checks if a value is in a range (inclusive)
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <param name="min">The inclusive min value.</param>
		/// <param name="max">The inclusive max value.</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRange(this float value, float min, float max)
		{
			return ((min <= value) && (value <= max));
		}

		/// <summary>
		/// Checks if a value is in a range (inclusive)
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <param name="min">The inclusive min value.</param>
		/// <param name="max">The inclusive max value.</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRange(this double value, double min, double max)
		{
			return ((min <= value) && (value <= max));
		}

		/// <summary>
		/// Checks if a value is in the range 0.0f to 1.0f inclusive
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRange0To1(this float value)
		{
			return IsInRange(value, 0.0f, 1.0f);
		}

		/// <summary>
		/// Checks if a value is in the range 0.0 to 1.0 inclusive
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRange0To1(this double value)
		{
			return IsInRange(value, 0.0, 1.0);
		}

		#endregion

		#region Min-Max Methods

		/// <summary>
		/// Returns the maximum value of three numbers
		/// </summary>
		/// <param name="a">Value one</param>
		/// <param name="b">Value two</param>
		/// <param name="c">Value three</param>
		/// <returns>The maximum of the three values.</returns>
		public static float Max(float a, float b, float c)
		{
			return (Math.Max(Math.Max(a, b), c));
		}

		/// <summary>
		/// Returns the maximum value of three numbers
		/// </summary>
		/// <param name="a">Value one</param>
		/// <param name="b">Value two</param>
		/// <param name="c">Value three</param>
		/// <returns>The maximum of the three values.</returns>
		public static double Max(double a, double b, double c)
		{
			return (Math.Max(Math.Max(a, b), c));
		}

		/// <summary>
		/// Returns the minimum value of three numbers
		/// </summary>
		/// <param name="a">Value one</param>
		/// <param name="b">Value two</param>
		/// <param name="c">Value three</param>
		/// <returns>The minimum of the three values.</returns>
		public static float Min(float a, float b, float c)
		{
			return (Math.Min(Math.Min(a, b), c));
		}

		/// <summary>
		/// Returns the minimum value of three numbers
		/// </summary>
		/// <param name="a">Value one</param>
		/// <param name="b">Value two</param>
		/// <param name="c">Value three</param>
		/// <returns>The minimum of the three values.</returns>
		public static double Min(double a, double b, double c)
		{
			return (Math.Min(Math.Min(a, b), c));
		}

		#endregion

		#region Blend Methods

		/// <summary>
		/// Combines two input numbers in some proportion. 
		/// ratio=0.0 the first number is not used at all, 
		/// ratio=0.5 they are weight equally
		/// ratio=1.0 the first number completely dominates the value
		/// </summary>
		/// <param name="val0">First number</param>
		/// <param name="val1">Second number</param>
		/// <param name="ratio">The ratio value. Range: [0.0f - 1.0f]</param>
		/// <returns>The blending result.</returns>
		public static float BlendWithRatio(float val0, float val1, float ratio)
		{
			var cratio = ratio.ClampToRange0To1();
			var v0 = val0 * cratio;
			var v1 = val1 * (1.0f - cratio);
			return v0 + v1;
		}

		/// <summary>
		/// Combines two input numbers in some proportion. 
		/// ratio=0.0 the first number is not used at all, 
		/// ratio=0.5 they are weight equally
		/// ratio=1.0 the first number completely dominates the value
		/// </summary>
		/// <param name="val0">First number</param>
		/// <param name="val1">Second number</param>
		/// <param name="ratio">The ratio value. Range: [0.0 - 1.0]</param>
		/// <returns>The blending result.</returns>
		public static double BlendWithRatio(double val0, double val1, double ratio)
		{
			var cratio = ratio.ClampToRange0To1();
			var v0 = val0 * cratio;
			var v1 = val1 * (1.0 - cratio);
			return v0 + v1;
		}

		#endregion

	}
}
