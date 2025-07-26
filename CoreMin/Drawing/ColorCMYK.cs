//--------------------------------------------------------------------------
// File:    ColorCMYK.cs
// Content:	Implementation of struct ColorCMYK
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Media;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{

	/// <summary>
	/// CMYK Color data type
	/// </summary>
// ReSharper disable InconsistentNaming
    public struct ColorCMYK
    {
        private readonly double m_Cyan;
        private readonly double m_Magenta;
        private readonly double m_Yellow;
        private readonly double m_BlackKey;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorCMYK"/> struct.
		/// </summary>
		/// <param name="c">The cyan value.</param>
		/// <param name="m">The magenta value.</param>
		/// <param name="y">The yellow value.</param>
		/// <param name="k">The black key value.</param>
		public ColorCMYK(double c, double m, double y, double k)
		{
			CheckCmyk(c, m, y, k);
			m_Cyan = c;
			m_Magenta = m;
			m_Yellow = y;
			m_BlackKey = k;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorCMYK"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorCMYK(System.Drawing.Color color)
		{
			var temp = FromColor(color);
			m_Cyan = temp.m_Cyan;
			m_Magenta = temp.m_Magenta;
			m_Yellow = temp.m_Yellow;
			m_BlackKey = temp.m_BlackKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorCMYK"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorCMYK(ColorRGB color)
		{
			var temp = FromColor(color);
			m_Cyan = temp.m_Cyan;
			m_Magenta = temp.m_Magenta;
			m_Yellow = temp.m_Yellow;
			m_BlackKey = temp.m_BlackKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorCMYK"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorCMYK(Color color)
		{
			var temp = FromColor(color);
			m_Cyan = temp.m_Cyan;
			m_Magenta = temp.m_Magenta;
			m_Yellow = temp.m_Yellow;
			m_BlackKey = temp.m_BlackKey;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorCMYK"/> struct.
		/// </summary>
		/// <param name="rgba">The RGBA value.</param>
		public ColorCMYK(UInt32 rgba)
		{
			var temp = FromRgba(rgba);
			m_Cyan = temp.m_Cyan;
			m_Magenta = temp.m_Magenta;
			m_Yellow = temp.m_Yellow;
			m_BlackKey = temp.m_BlackKey;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Cyan value.
		/// </summary>
        public double C
        {
            get { return m_Cyan; }
        }

		/// <summary>
		/// Gets the Magenta value.
		/// </summary>
        public double M
        {
            get { return m_Magenta; }
        }

		/// <summary>
		/// Gets the Yellow value.
		/// </summary>
        public double Y
        {
            get { return m_Yellow; }
        }

		/// <summary>
		/// Gets the Black key value.
		/// </summary>
        public double K
        {
            get { return m_BlackKey; }
        }

		#endregion

		#region Public Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture,
					"cmyk({0:#0.##%}, {1:#0.##%}, {2:#0.##%}, {3:#0.##%})",
					Math.Round(m_Cyan, 2), Math.Round(m_Magenta, 2), Math.Round(m_Yellow, 2), Math.Round(m_BlackKey, 2));
        }

		/// <summary>
		/// Implicit conversion from CMYK color to System.Drawing.Color
		/// </summary>
		/// <param name="color">The CMYK color.</param>
		/// <returns>The System.Drawing.Color result.</returns>
		public static explicit operator System.Drawing.Color(ColorCMYK color)
		{
			return color.ToSystemDrawingColor();
		}

		/// <summary>
		/// Parses the specified string into a CMYK color value.
		/// </summary>
		/// <param name="s">The string to parse.</param>
		/// <returns>The CMYK color value.</returns>
		public static ColorCMYK Parse(string s)
		{

			var components = ColorUtil.StringComponentsToColor(4, s);
			var c = new ColorCMYK(components[0], components[1], components[2], components[3]);
			return c;
		}

		/// <summary>
		/// Create a CMYK Color instance from a WPF color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created CMYK Color.</returns>
		public static ColorCMYK FromColor(Color color)
		{
			return FromColor(new ColorRGB(color));
		}

		/// <summary>
		/// Create a CMYK Color instance from a GDI color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created CMYK Color.</returns>
		public static ColorCMYK FromColor(System.Drawing.Color color)
		{
			return FromColor(new ColorRGB(color));
		}

		#endregion

		#region Private Methods

		private static void CheckCmyk(double c, double m, double y, double k)
		{
			if (!c.IsInRange0To1())
			{
				throw new ColorException("CMYK C out of range");
			}
			if (!m.IsInRange0To1())
			{
				throw new ColorException("CMYK M out of range");
			}
			if (!y.IsInRange0To1())
			{
				throw new ColorException("CMYK Y out of range");
			}
			if (!k.IsInRange0To1())
			{
				throw new ColorException("CMYK K out of range");
			}
		}

		private static ColorCMYK FromColor(ColorRGB rgbcolor)
		{
			double cmykCyan = 1 - rgbcolor.R;
			double cmykMagenta = 1 - rgbcolor.G;
			double cmykYellow = 1 - rgbcolor.B;

			double blackKeyValue = MathUtil.Min(cmykCyan, cmykMagenta, cmykYellow);

			if (blackKeyValue.IsNearOne())
			{
				cmykCyan = cmykMagenta = cmykYellow = 0.0;
			}
			else
			{
				cmykCyan = (cmykCyan - blackKeyValue) / (1 - blackKeyValue);
				cmykMagenta = (cmykMagenta - blackKeyValue) / (1 - blackKeyValue);
				cmykYellow = (cmykYellow - blackKeyValue) / (1 - blackKeyValue);
			}

			var cmyk = new ColorCMYK(
				Math.Min(1f, Math.Max(0f, cmykCyan)), 
				Math.Min(1f, Math.Max(0f, cmykMagenta)),
				Math.Min(1f, Math.Max(0f, cmykYellow)), 
				Math.Min(1f, Math.Max(0f, blackKeyValue)));

			return cmyk;
		}

        private static ColorCMYK FromRgba(UInt32 rgba)
        {
            return FromColor(new ColorRGB(rgba));
        }

        private System.Drawing.Color ToSystemDrawingColor()
        {
            var rgbcolor = new ColorRGB(this);
            return (System.Drawing.Color) rgbcolor;
        }

		#endregion
		
    }
	// ReSharper restore InconsistentNaming
}