//--------------------------------------------------------------------------
// File:    ColorUtil.cs
// Content:	Implementation of class ColorUtil
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using SmartExpert.Calculation;

#endregion

namespace SmartExpert.Drawing
{
	///<summary>Encapsulates methods relating to colors.</summary>
	public static class ColorUtil
	{
		
		#region ctors

		/// <summary>
		/// Initializes static members of the <see cref="ColorConverter"/> class.
		/// </summary>
		static ColorUtil()
		{
			KnownColors = new KnownColors();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Provides a dictionary with known colors and their hex representation.
		/// </summary>
		public static KnownColors KnownColors { get; private set; }

		#endregion


		#region RGB Helper Methods

		/// <summary>
		/// Convert 8bit RGB component [0 - 255] into 32bit RGB component [0.0 - 1.0].
		/// </summary>
		/// <param name="rgbComponent">The 8bit RGB component.</param>
		/// <returns>The 32bit RGB component</returns>
		public static float Rgb8ToRgb32(byte rgbComponent)
		{
			return (float)Math.Round(rgbComponent/255.0, 4);
		}

		/// <summary>
		/// Gamma-compress RGB component.
		/// </summary>
		/// <param name="rgbComponent">The RGB component [0.0 - 1.0].</param>
		/// <returns>The gamma-compressed RGB component</returns>
		public static double GammaCompressRgbComponent(double rgbComponent)
		{
			double c = rgbComponent.ClampToRange0To1();
			double result = c > 0.03928 ? Math.Round(Math.Pow(((0.055 + c) / 1.055), 2.4), 4) : Math.Round(c / 12.92, 4);
			return result;
		}

		#endregion

		#region WebColorString Helper Methods

		/// <summary>
		/// Converts RGB values to a web color string. Output format: #RRGGBB (e.g. #804020)
		/// </summary>
		/// <param name="redValue">The red value.</param>
		/// <param name="greenValue">The green value.</param>
		/// <param name="blueValue">The blue value.</param>
		/// <returns>The web color string.</returns>
		public static string ToWebColorString(byte redValue, byte greenValue, byte blueValue)
		{
			const string formatString = "#{0:x2}{1:x2}{2:x2}";
			string colorString = string.Format(CultureInfo.InvariantCulture, formatString, redValue, greenValue, blueValue);
			return colorString;
		}

		/// <summary>
		/// Converts RGBA values to a web color string. Output format: #AARRGGBB (e.g. #FF804020)
		/// </summary>
		/// <param name="redValue">The red value.</param>
		/// <param name="greenValue">The green value.</param>
		/// <param name="blueValue">The blue value.</param>
		/// <param name="alphaValue">The alpha value.</param>
		/// <returns>The web color string.</returns>
		public static string ToWebColorString(byte redValue, byte greenValue, byte blueValue, byte alphaValue)
		{
			const string formatString = "#{0:x2}{1:x2}{2:x2}{3:x2}";
			string colorString = string.Format(CultureInfo.InvariantCulture, formatString, alphaValue, redValue, greenValue, blueValue);
			return colorString;
		}

		/// <summary>
		/// Converts a web color string into a <see cref="Color"/>.
		/// </summary>
		/// <param name="webColorString">The web color string.</param>
		/// <returns>The <see cref="Color"/> result.</returns>
		public static Color WebColorStringToColor(string webColorString)
		{
			if (webColorString.IsNullOrEmptyWithTrim())
			{
				return Colors.Black;
			}
			if (webColorString.Length < 3 || webColorString.Length > 9)
			{
				return Colors.Black;
			}

			string hexString = KnownColors.ContainsKey(webColorString.Trim()) ? KnownColors[webColorString.Trim()] : webColorString.Trim();
			hexString = hexString.Substring(0, 1) == "#" ? hexString.Substring(1) : hexString;

			if (hexString.Length == 3)
			{
				var tmp = "{0}{1}{2}{3}{4}{5}".SafeFormatWith(hexString[0], hexString[0], hexString[1], hexString[1], hexString[2], hexString[2]);
				hexString = tmp;
			}

			byte r, g, b;
			if (hexString.Length == 6)
			{
				var redSucceeded = byte.TryParse(hexString.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out r);
				var greenSucceeded = byte.TryParse(hexString.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out g);
				var blueSucceeded = byte.TryParse(hexString.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out b);
				if (redSucceeded && greenSucceeded && blueSucceeded)
				{
					return Color.FromRgb(r, g, b);
				}
			}

			if (hexString.Length == 8)
			{
				byte a;
				var alphaSucceeded = byte.TryParse(hexString.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out a);
				var redSucceeded = byte.TryParse(hexString.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out r);
				var greenSucceeded = byte.TryParse(hexString.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out g);
				var blueSucceeded = byte.TryParse(hexString.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out b);
				if (alphaSucceeded && redSucceeded && greenSucceeded && blueSucceeded)
				{
					return Color.FromArgb(a, r, g, b);
				}
			}

			return Colors.Black;
		}

		#endregion

		internal static double[] StringComponentsToColor(int num, string s)
		{
			if ((num != 3) || (num != 4))
			{
				throw new ArgumentOutOfRangeException("num");
			}
			char[] seps = { ',' };
			var rawComponents = s.Split(seps, num);
			var components = rawComponents.Select(t => double.Parse(t, CultureInfo.InvariantCulture)).ToArray();
			return components;
		}
	}
}
