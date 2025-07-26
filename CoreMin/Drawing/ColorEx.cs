//--------------------------------------------------------------------------
// File:    ColorEx.cs
// Content:	Implementation of class ColorEx
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

using System.Linq;
using System.Windows.Media;

#endregion

namespace SmartExpert.Drawing
{
	///<summary>Color extension methods</summary>
	public static class ColorEx
	{
		#region ToWebColorString extensions

		/// <summary>
		/// Convert color to a web color string. The alpha value will be ignored.
		/// Output format: #RRGGBB (e.g. #804020)
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <returns>The web color string result.</returns>
		public static string ToWebColorString(this Color color)
		{
			return ToWebColorString(color, false);
		}

		/// <summary>
		/// Convert color to a web color string.
		/// Output format without alpha: #RRGGBB (e.g. #804020)
		/// Output format with alpha: #AARRGGBB (e.g. #FF804020)
		/// </summary>
		/// <param name="color">The color to convert</param>
		/// <param name="useAlpha">Use alpha value in conversion?</param>
		/// <returns>The web color string result.</returns>
		public static string ToWebColorString(this Color color, bool useAlpha)
		{
			return useAlpha ? ColorUtil.ToWebColorString(color.R, color.G, color.B, color.A) : 
							  ColorUtil.ToWebColorString(color.R, color.G, color.B);
		}

		/// <summary>
		/// Convert color to a web color string. The alpha value will be ignored.
		/// Output format: #RRGGBB (e.g. #804020)
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <returns>The web color string.</returns>
		public static string ToWebColorString(this System.Drawing.Color color)
		{
			return ToWebColorString(color, false);
		}

		/// <summary>
		/// Converts color to a web color string.
		/// Output format without alpha: #RRGGBB (e.g. #804020)
		/// Output format with alpha: #AARRGGBB (e.g. #FF804020)
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <param name="useAlpha">Use alpha value in conversion?</param>
		/// <returns>The web color string result.</returns>
		public static string ToWebColorString(this System.Drawing.Color color, bool useAlpha)
		{
			return useAlpha ? ColorUtil.ToWebColorString(color.R, color.G, color.B, color.A) : 
							  ColorUtil.ToWebColorString(color.R, color.G, color.B);
		}

		#endregion

		/// <summary>
		/// Changes a GDI color object to an WPF one.
		/// </summary>
		/// <param name="color">The GDI color.</param>
		/// <returns>The WPF color result.</returns>
		public static Color GdiToWindows(this System.Drawing.Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Changes an WPF color object to a GDI one.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The GDI color object.</returns>
		public static System.Drawing.Color WindowsToGdi(this Color color)
		{
			return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
		}


	}
}
