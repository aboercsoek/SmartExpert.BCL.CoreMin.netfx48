//--------------------------------------------------------------------------
// File:    DrawingUtil.cs
// Content:	Implementation of class DrawingUtil
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

using System.Linq;
#endregion

namespace SmartExpert.Drawing
{
	///<summary>Drawing Helper Methods</summary>
	public static class DrawingUtil
	{
		/// <summary>
		/// Helper function to calculate distance between two points (a,b) is the vector between the points.
		/// </summary>
		/// <param name="a">Value a</param>
		/// <param name="b">Value b</param>
		/// <returns>The distance between two points</returns>
		public static double Distance(double a, double b)
		{
			return Math.Sqrt((a * a) + (b * b));
		}

		/// <summary>
		/// Helper function to calculate distance between two points (a,b,c) defines a vector.
		/// </summary>
		/// <param name="a">Value a</param>
		/// <param name="b">Value b</param>
		/// <param name="c">Value c</param>
		/// <returns>The distance between two points</returns>
		public static double Distance(double a, double b, double c)
		{
			return Math.Sqrt((a * a) + (b * b) + (c * c));
		}

		/// <summary>
		/// Combines two input numbers in some proportion. 
		/// ratio=0.0 the first number is not used at all, 
		/// ratio=0.5 they are weight equally
		/// ratio=1.0 the first number completely dominates the value
		/// </summary>
		/// <param name="val0">First number</param>
		/// <param name="val1">Second number</param>
		/// <param name="ratio">The ratio value.</param>
		/// <returns>The blending result number.</returns>
		public static double BlendIntoRange0To1(double val0, double val1, double ratio)
		{
			var cratio = ClampToRangeFrom0To1(ratio);
			var v0 = val0 * cratio;
			var v1 = val1 * (1.0 - cratio);
			return v0 + v1;
		}

		/// <summary>
		/// Checks if a value is in a range (inclusive)
		/// </summary>
		/// <param name="val">The value to check</param>
		/// <param name="min">The inclusive min value.</param>
		/// <param name="max">The inclusive max value.</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRange(double val, double min, double max)
		{
			return ((min <= val) && (val <= max));
		}

		/// <summary>
		/// Checks if a value is in the range 0.0 to 1.0 inclusive
		/// </summary>
		/// <param name="val">The value to check</param>
		/// <returns>true if in range; otherwise false.</returns>
		public static bool IsInRangeFrom0To1(double val)
		{
			return IsInRange(val, 0.0, 1.0);
		}

		/// <summary>
		/// Given an input value will force the value to fit within the range (min,max) inclusive
		/// </summary>
		/// <param name="v">The value to clamp</param>
		/// <param name="min">The min clamp value</param>
		/// <param name="max">The max clamp value.</param>
		/// <returns>The clamped value</returns>
		public static double ClampToRange(double v, double min, double max)
		{
			if (v < min)
			{
				v = min;
			}
			else if (v > max)
			{
				v = max;
			}
			return v;
		}

		/// <summary>
		/// Given an input value, will limit it to the range 0.0 and 1.0 inclusive
		/// </summary>
		/// <param name="v">The value to clamp.</param>
		/// <returns>The clamped value</returns>
		public static double ClampToRangeFrom0To1(double v)
		{
			return ClampToRange(v, 0.0, 1.0);
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
		public static double Min(double a, double b, double c)
		{
			return (Math.Min(Math.Min(a, b), c));
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
		/// Wraps a number around so that it always fits between 0.0 and 1.0. negative numbers will wrap around to the correct positive number
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
			if (IsInRange(v, min, max))
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

	}
}
