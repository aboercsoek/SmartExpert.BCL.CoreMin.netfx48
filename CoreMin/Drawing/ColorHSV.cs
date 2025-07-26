using System;
using System.Globalization;
using System.Windows.Media;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{

	/// <summary>
	/// HSV Color data type
	/// </summary>
    public struct ColorHSV
    {
        private readonly double m_Hue;
		private readonly double m_Saturation;
        private readonly double m_Value;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="sat">The saturation.</param>
		/// <param name="val">The color value (brightness).</param>
		public ColorHSV(double hue, double sat, double val)
		{
			CheckHsv(hue, sat, val);
			m_Hue = hue;
			m_Saturation = sat;
			m_Value = val;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="hsv">The HSV color.</param>
		public ColorHSV(ColorHSV hsv)
		{
			m_Hue = hsv.m_Hue;
			m_Saturation = hsv.m_Saturation;
			m_Value = hsv.m_Value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorHSV(ColorRGB color)
		{
			var temp = FromColor(color);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Value = temp.m_Value;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="rgba">The RGBA value.</param>
		public ColorHSV(UInt32 rgba)
		{
			var temp = FromRgba(rgba);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Value = temp.m_Value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="c">The color.</param>
		public ColorHSV(Color c)
		{
			var temp = FromColor(c);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Value = temp.m_Value;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSV"/> struct.
		/// </summary>
		/// <param name="c">The color.</param>
		public ColorHSV(System.Drawing.Color c)
		{
			var temp = FromColor(c);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Value = temp.m_Value;

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the hue.
		/// </summary>
		public double Hue
        {
            get { return m_Hue; }
        }

		/// <summary>
		/// Gets the saturation.
		/// </summary>
        public double Saturation
        {
            get { return m_Saturation; }
        }

		/// <summary>
		/// Gets the color tonal value.
		/// </summary>
        public double Value
        {
            get { return m_Value; }
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
				"hsv({0}°, {1:#0.##%}, {2:#0.##%})", (m_Hue * 360).ToInt(), Math.Round(m_Saturation, 2), Math.Round(m_Value, 2));
        }

        /// <summary>
		/// HSV Color to System.Drawing.Color operator.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The .NET system drawing RGB color value.</returns>
		public static explicit operator System.Drawing.Color(ColorHSV color)
		{
			return color.ToSystemDrawingColor();
		}

		/// <summary>
		/// HSV Color to System.Windows.Media.Color operator.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The System.Windows.Media.Color result value.</returns>
		public static explicit operator Color(ColorHSV color)
		{
			return color.ToColor();
		}
		
		/// <summary>
		/// Parses the specified HSV color string.
		/// </summary>
		/// <param name="s">The HSV color string.</param>
		/// <returns>The parsed HSV color.</returns>
		public static ColorHSV ParseHsv(string s)
		{
			var components = ColorUtil.StringComponentsToColor(3, s);
			var hsv = new ColorHSV(components[0], components[1], components[2]);
			return hsv;
		}

		/// <summary>
		/// Create a HSV Color instance from a WPF color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created HSV Color.</returns>
		public static ColorHSV FromColor(Color color)
		{
			var rgb = new ColorRGB(color);
			var hsv = FromColor(rgb);
			return hsv;
		}

		/// <summary>
		/// Create a HSV Color instance from a GDI color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created HSV Color.</returns>
		public static ColorHSV FromColor(System.Drawing.Color color)
		{
			var rgb = new ColorRGB(color);
			var hsv = FromColor(rgb);
			return hsv;
		}

		#endregion

		#region Private Methods

		private static void CheckHsv(double hue, double sat, double val)
		{
			if (!hue.IsInRange0To1())
			{
				throw new ColorException("HSV Hue out of range");
			}
			if (!sat.IsInRange0To1())
			{
				throw new ColorException("HSV Saturation  out of range");
			}
			if (!val.IsInRange0To1())
			{
				throw new ColorException("HSV Value  out of range");
			}
		}

		private static ColorHSV FromColor(ColorRGB rgbcolor)
        {
            double max = MathUtil.Max(rgbcolor.R, rgbcolor.G, rgbcolor.B);
			double min = MathUtil.Min(rgbcolor.R, rgbcolor.G, rgbcolor.B);

            double theHue;
            double theSat;
			double theVal = Math.Min(1.0, Math.Max(0.0, max));

            double delta = max - min;

			if (delta.IsNearZero() || max.IsNearZero())
            {
				theHue = 0;
				theSat = 0;
                return new ColorHSV(theHue, theSat, theVal);
            }
			
			theSat = delta/max;

			if ((rgbcolor.R - max).IsNearZero())
			{
				theHue = (rgbcolor.G - rgbcolor.B) / delta;
			}
			else if ((rgbcolor.G - max).IsNearZero())
			{
				theHue = 2.0 + (rgbcolor.B - rgbcolor.R) / delta;
			}
			else
			{
				theHue = 4.0 + (rgbcolor.R - rgbcolor.G) / delta;
			}
			
			if (theHue < 0)
			{
				theHue += 6.0;
			}

			theHue /= 6.0; // scale hue to between 0.0 and 1.0

			return new ColorHSV(theHue, theSat, theVal);
        }

        private static ColorHSV FromRgba(UInt32 color)
        {
            return FromColor(new ColorRGB(color));
        }

		
        private System.Drawing.Color ToSystemDrawingColor()
        {
            var rgbcolor = new  ColorRGB(this);
            return (System.Drawing.Color) rgbcolor;
        }

		private Color ToColor()
		{
			var rgbcolor = new ColorRGB(this);
			return (Color)rgbcolor;
		}

		#endregion
	}

}