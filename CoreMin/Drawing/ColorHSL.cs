using System;
using System.Globalization;
using System.Windows.Media;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// HSL color data type
	/// </summary>
    public struct ColorHSL
    {
        private readonly double m_Hue;
        private readonly double m_Saturation;
		private readonly double m_Lightness;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="lightness">The lightness.</param>
		public ColorHSL(double hue, double saturation, double lightness)
		{
			CheckHslInRange(hue, saturation, lightness);
			m_Hue = hue;
			m_Saturation = saturation;
			m_Lightness = lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="hsl">The HSL.</param>
		public ColorHSL(ColorHSL hsl)
		{
			m_Hue = hsl.m_Hue;
			m_Saturation = hsl.m_Saturation;
			m_Lightness = hsl.m_Lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorHSL(ColorHSV color)
		{
			var rgbColor = new ColorRGB(color);
			var hslColor = new ColorHSL(rgbColor);
			m_Hue = hslColor.Hue;
			m_Saturation = hslColor.Saturation;
			m_Lightness = hslColor.Lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorHSL(ColorRGB color)
		{
			var temp = FromColor(color);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Lightness = temp.m_Lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorHSL(Color color)
		{
			var temp = FromColor(color);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Lightness = temp.m_Lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorHSL(System.Drawing.Color color)
		{
			var temp = FromColor(color);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Lightness = temp.m_Lightness;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL"/> struct.
		/// </summary>
		/// <param name="color">The rgb color (as int value).</param>
		public ColorHSL(UInt32 color)
		{
			var temp = FromRgba(color);
			m_Hue = temp.m_Hue;
			m_Saturation = temp.m_Saturation;
			m_Lightness = temp.m_Lightness;
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
		/// Gets the luminance.
		/// </summary>
		public double Lightness
        {
            get { return m_Lightness; }
        }

		#endregion

		#region Public  Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
        {
			return string.Format(CultureInfo.InvariantCulture,
				"hsl({0}°, {1:#0.##%}, {2:#0.##%})", (m_Hue * 360).ToInt(), Math.Round(m_Saturation, 2), Math.Round(m_Lightness, 2));
        }

		/// <summary>
		/// Checks if all the HSL values are in range. Throws an ColorException if at least one value is out of range.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="lightness">The lightness.</param>
		internal static void CheckHslInRange(double hue, double saturation, double lightness)
        {
			if (!hue.IsInRange0To1())
            {
                throw new ColorException("HSL Hue out of range");
            }

			if (!saturation.IsInRange0To1())
            {
                throw new ColorException("HSL Saturation  out of range");
            }

			if (!lightness.IsInRange0To1())
            {
				throw new ColorException("HSL Lightness out of range");
            }
        }

		/// <summary>
		/// HSL color to System.Drawing.Color converter.
		/// </summary>
		/// <param name="color">The HSL color.</param>
		/// <returns>The .NET system ARGB color type.</returns>
        public static explicit operator System.Drawing.Color(ColorHSL color)
        {
            return color.ToSystemDrawingColor();
        }

		/// <summary>
		/// Adds the specified hue, saturation, and luminace delta values to the HSL color and returns the new HSL color.
		/// </summary>
		/// <param name="hDelta">The hue delta.</param>
		/// <param name="sDelta">The saturation delta.</param>
		/// <param name="lDelta">The lightness delta.</param>
		/// <returns>The new HSL color after the add delta operation.</returns>
		public ColorHSL Add(double hDelta, double sDelta, double lDelta)
		{
			double newH = DoubleUtil.WrapAngleFrom0To1(m_Hue + hDelta);
			double newS = (m_Saturation + sDelta).ClampToRange0To1();
			double newL = (m_Lightness + lDelta).ClampToRange0To1();
			return new ColorHSL(newH, newS, newL);
		}

		/// <summary>
		/// Parses the specified HSL color string.
		/// </summary>
		/// <param name="s">The HSL color string.</param>
		/// <returns>The parsed HSL color result.</returns>
		public static ColorHSL Parse(string s)
		{
			var components = ColorUtil.StringComponentsToColor(3, s);
			var hsl = new ColorHSL(components[0], components[1], components[2]);
			return hsl;
		}

		/// <summary>
		/// Create a HSL Color instance from a GDI color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created HSL Color.</returns>
		public static ColorHSL FromColor(System.Drawing.Color color)
		{
			return FromColor(new ColorRGB(color));
		}

		/// <summary>
		/// Create a HSL Color instance from a WPF color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created HSL Color.</returns>
		public static ColorHSL FromColor(Color color)
		{
			return FromColor(new ColorRGB(color));
		}

		#endregion

		#region Private Methods

		private static ColorHSL FromColor(ColorRGB rgbColor)
		{
			// taken from: http://www.tecgraf.puc-rio.br/~mgattass/color/HSLtoRGB.htm
			double max = MathUtil.Max(rgbColor.R, rgbColor.G, rgbColor.B);
			double min = MathUtil.Min(rgbColor.R, rgbColor.G, rgbColor.B);
			double delta = max - min;

			double outH;
			double outS;
			double outL = (max + min) / 2.0;

			if (delta.IsNearZero())
			{
				outH = 0.0;
				outS = 0.0;
			}
			else
			{
				outS = (outL <= 0.5) ? delta / (max + min) : delta / ((2.0 - min) - max);
				
				if ((rgbColor.R - max).IsNearZero())
				{
					outH = (rgbColor.G - rgbColor.B) / delta;
				}
				else if ((rgbColor.G - max).IsNearZero())
				{
					outH = 2f + ((rgbColor.B - rgbColor.R) / delta);

				}
				else
				{
					outH = 4f + ((rgbColor.R - rgbColor.G) / delta);
				}

				if (outH < 0.0)
				{
					outH += 6.0;
				}
				outH /= 6.0;
			}

			return new ColorHSL(
					Math.Min(1f, Math.Max(0f, outH)), 
					Math.Min(1f, Math.Max(0f, outS)), 
					Math.Min(1f, Math.Max(0f, outL)));
		}


		private static ColorHSL FromRgba(UInt32 rgba)
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
}