using System;
using System.Globalization;
using System.Windows.Media;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// RGBA color data type. R, G, B, A values are double values with range from 0 to 1
	/// </summary>
	public struct ColorRGB : IFormattable
    {
        private readonly double m_Red;
        private readonly double m_Green;
        private readonly double m_Blue;
		private readonly double m_Alpha;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="r">The r.</param>
		/// <param name="g">The g.</param>
		/// <param name="b">The b.</param>
		/// <param name="a">The alpha value.</param>
		public ColorRGB(double r, double g, double b, double a)
		{
			CheckRgbaInRange(r, g, b, a);
			m_Red = Math.Round(r, 3);
			m_Green = Math.Round(g, 3);
			m_Blue = Math.Round(b, 3);
			m_Alpha = Math.Round(a, 3);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="c">The c.</param>
		public ColorRGB(Color c)
		{
			var rgb = FromColor(c);
			m_Red = rgb.m_Red;
			m_Green = rgb.m_Green;
			m_Blue = rgb.m_Blue;
			m_Alpha = rgb.m_Alpha;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="rgbaValue">The RGBA value.</param>
		public ColorRGB(UInt32 rgbaValue)
		{
			var temp = FromColor(rgbaValue);
			m_Red = temp.m_Red;
			m_Green = temp.m_Green;
			m_Blue = temp.m_Blue;
			m_Alpha = temp.m_Alpha;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorRGB(System.Drawing.Color color)
		{
			var temp = FromSystemDrawingColor(color);
			m_Red = temp.m_Red;
			m_Green = temp.m_Green;
			m_Blue = temp.m_Blue;
			m_Alpha = temp.m_Alpha;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorRGB(ColorHSV color)
		{
			var temp = FromColor(color);
			m_Red = temp.m_Red;
			m_Green = temp.m_Green;
			m_Blue = temp.m_Blue;
			m_Alpha = temp.m_Alpha;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorRGB(ColorHSL color)
		{
			var temp = FromColor(color);
			m_Red = temp.m_Red;
			m_Green = temp.m_Green;
			m_Blue = temp.m_Blue;
			m_Alpha = temp.m_Alpha;

		}


		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public ColorRGB(ColorCMYK color)
		{
			var temp = FromColor(color);
			m_Red = temp.m_Red;
			m_Green = temp.m_Green;
			m_Blue = temp.m_Blue;
			m_Alpha = temp.m_Alpha;

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Red Value.
		/// </summary>
		public double R
        {
            get { return m_Red; }
        }

		/// <summary>
		/// Gets the Green Value.
		/// </summary>
        public double G
        {
            get { return m_Green; }
        }

		/// <summary>
		/// Gets the Blue value.
		/// </summary>
        public double B
        {
            get { return m_Blue; }
        }

		/// <summary>
		/// Gets the Alpha value.
		/// </summary>
		public double A
		{
			get { return m_Alpha; }
		}

		#endregion

		#region operators

		/// <summary>
		/// Performs an explicit conversion from <see cref="SmartExpert.Drawing.ColorRGB"/> to <see cref="System.Drawing.Color"/>.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator System.Drawing.Color(ColorRGB color)
		{
			return color.ToSystemDrawingColor();
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="SmartExpert.Drawing.ColorRGB"/> to <see cref="System.Windows.Media.Color"/>.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Color(ColorRGB color)
		{
			return color.ToColor();
		}

		#endregion

		#region Public Methods

		#region ToString Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString()
		{
			return ConvertToString(null, null);
		}

		#region IFormattable Members

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return ConvertToString(format, formatProvider);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		string IFormattable.ToString(string format, IFormatProvider formatProvider)
		{
			return ConvertToString(format, formatProvider);
		}

		#endregion

		internal string ConvertToString(string format, IFormatProvider provider)
		{
			if (format.IsNullOrEmptyWithTrim())
				return string.Format(CultureInfo.InvariantCulture, 
					"rgba({0:#0.##%}, {1:#0.##%}, {2:#0.##%}, {3:#0.##%})", 
					Math.Round(m_Red, 2), Math.Round(m_Green, 2), Math.Round(m_Blue, 2), Math.Round(m_Alpha, 2));
			
			if (format.Trim().ToUpperInvariant() == "X")
				return ToColor().ToWebColorString(true);

			return string.Format(CultureInfo.InvariantCulture,
					"rgba({0:#0.##%}, {1:#0.##%}, {2:#0.##%}, {3:#0.##%})",
					Math.Round(m_Red, 2), Math.Round(m_Green, 2), Math.Round(m_Blue, 2), Math.Round(m_Alpha, 2));
		}

		#endregion

		/// <summary>
		/// Parses the specified RGB color string.
		/// </summary>
		/// <param name="webColorString">The web color string.</param>
		/// <returns>The parsed RGB color value.</returns>
		public static ColorRGB Parse(string webColorString)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(webColorString, "webColorString");
			return FromColor(ColorUtil.WebColorStringToColor(webColorString));
		}

		/// <summary>
		/// Create a ColorRGB instance from a GDI color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created ColorRGB.</returns>
		public static ColorRGB FromSystemDrawingColor(System.Drawing.Color color)
		{
			return FromColor(Color.FromArgb(color.A, color.R, color.G, color.B));
		}

		/// <summary>
		/// Create a ColorRGB instance from a WPF color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The created ColorRGB.</returns>
		public static ColorRGB FromColor(Color color)
		{
			var c = new ColorRGB(
							ColorUtil.Rgb8ToRgb32(color.R), 
							ColorUtil.Rgb8ToRgb32(color.G), 
							ColorUtil.Rgb8ToRgb32(color.B), 
							ColorUtil.Rgb8ToRgb32(color.A));
			return c;
		}

		/// <summary>
		/// Blends two RGBA color values.
		/// </summary>
		/// <param name="a">RGBA color A.</param>
		/// <param name="b">RGBA color B.</param>
		/// <param name="ratio">The blending ratio.</param>
		/// <returns></returns>
		public static ColorRGB Blend(ColorRGB a, ColorRGB b, double ratio)
		{
			var normRatio = ratio.ClampToRange0To1();
			var antiRatio = 1.0 - normRatio;
			var newRed = (a.m_Red * normRatio) + (b.m_Red * antiRatio);
			var newGreen = (a.m_Green * normRatio) + (b.m_Green * antiRatio);
			var newBlue = (a.m_Blue * normRatio) + (b.m_Blue * antiRatio);
			var newAlpha = (a.m_Alpha * normRatio) + (b.m_Alpha * antiRatio);
			return new ColorRGB(newRed, newGreen, newBlue, newAlpha);
		}

		/// <summary>
		/// Converts the current color to the luma lightness value Y' of that color.
		/// Luma is the weighted average of gamma-corrected R, G, and B, 
		/// based on their contribution to perceived luminance.
		/// </summary>
		/// <remarks>For more info: 
		/// http://en.wikipedia.org/wiki/Luma_%28video%29
		/// http://en.wikipedia.org/wiki/HSV_color_space#Disadvantages</remarks>
		/// <returns>The luminance of the current color.</returns>
		public double ToLumaRec709()
		{
			//return ((0.299 * R) + (0.587 * G) + (0.114 * B));
			//return ((0.2126 * R) + (0.7152 * G) + (0.0722 * B));
			// 
			return Math.Round(
					(0.2126 * ColorUtil.GammaCompressRgbComponent(R)) + 
					(0.7152 * ColorUtil.GammaCompressRgbComponent(G)) + 
					(0.0722 * ColorUtil.GammaCompressRgbComponent(B)), 4);
		}

		/// <summary>
		/// Converts the current color to Gray by using the luma lightness value Y'.
		/// </summary>
		/// <remarks>For more info: 
		/// http://en.wikipedia.org/wiki/Luma_%28video%29
		/// http://en.wikipedia.org/wiki/HSV_color_space#Disadvantages</remarks>
		/// <returns>The color intensity as a gray color value.</returns>
		public Color ToGrayColorFromLumaRec709()
		{
			double luma = ToLumaRec709();
			var rgbColor = new ColorRGB(luma, luma, luma, A);
			return (Color) rgbColor;
		}

		/// <summary>
		/// Returns the L* value of this color in the L*a*b* color space.
		/// </summary>
		/// <remarks>For more info: 
		/// http://en.wikipedia.org/wiki/L*a*b*
		/// http://en.wikipedia.org/wiki/HSV_color_space#Disadvantages</remarks>
		/// <returns>The L* value [0 - 100]</returns>
		public byte ToLabL()
		{
			double y = Math.Round(
							(0.2126 * ColorUtil.GammaCompressRgbComponent(R)) +
							(0.7152 * ColorUtil.GammaCompressRgbComponent(G)) +
							(0.0722 * ColorUtil.GammaCompressRgbComponent(B)), 4);

			double y1 = y > 0.008856 ? Math.Round(Math.Pow(y, 1 / 3.0), 4) : Math.Round(7.787 * y + 16.0 / 116.0, 4);
			double labL = Math.Round(116 * y1 - 16, 0);

			return labL.BoundToByte();
		}

		/// <summary>
		/// Converts the current color to Gray by using the L*a*b* L* lightness value.
		/// </summary>
		/// <remarks>For more info: 
		/// http://en.wikipedia.org/wiki/L*a*b*
		/// http://en.wikipedia.org/wiki/HSV_color_space#Disadvantages</remarks>
		/// <returns>The color intensity as a gray color value.</returns>
		public Color ToGrayColorFromLabL()
		{
			double labl = ToLabL() / 100.0;
			var rgbColor = new ColorRGB(labl, labl, labl, A);
			return (Color)rgbColor;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Checks if the RGBA values are in range (0 .. 1). Throws a ColorException if  at least one value is out of range.
		/// </summary>
		/// <param name="r">The red value.</param>
		/// <param name="g">The green value.</param>
		/// <param name="b">The blue value.</param>
		/// <param name="a">The alpha value.</param>
		private static void CheckRgbaInRange(double r, double g, double b, double a)
		{
			if (!r.IsInRange0To1())
			{
				throw new ColorException("RGBA Red out of range");
			}
			if (!g.IsInRange0To1())
			{
				throw new ColorException("RGBA Green out of range");
			}
			if (!b.IsInRange0To1())
			{
				throw new ColorException("RGBA Blue out of range");
			}
			if (!a.IsInRange0To1())
			{
				throw new ColorException("RGBA Alpha out of range");
			}

		}

		#region ToColor Methods

		private Color ToColor()
		{
			return Color.FromArgb(
				(A * 255.0).BoundToByte(), (R * 255.0).BoundToByte(),
				(G * 255.0).BoundToByte(), (B * 255.0).BoundToByte());
		}

		private System.Drawing.Color ToSystemDrawingColor()
		{
			return System.Drawing.Color.FromArgb(
				(A * 255.0).BoundToByte(), (R * 255.0).BoundToByte(),
				(G * 255.0).BoundToByte(), (B * 255.0).BoundToByte());
		}

		#endregion

		#region Private FromColor Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB"/> struct.
		/// </summary>
		/// <param name="rgbaValue">The RGBA value.</param>
		private static ColorRGB FromColor(UInt32 rgbaValue)
		{
			var a = (byte)((rgbaValue & 0xff000000) >> 24);
			var r = (byte)((rgbaValue & 0x00ff0000) >> 16);
			var g = (byte)((rgbaValue & 0x0000ff00) >> 8);
			var b = (byte)((rgbaValue & 0x000000ff) >> 0);

			return new ColorRGB(r / 255.0, g / 255.0, b / 255.0, a / 255.0);
		}

		private static ColorRGB FromColor(ColorHSV hsvColor)
		{
			ColorRGB rgbColor;

			if (hsvColor.Saturation.IsNearZero())
			{
				// Make it some kind of gray
				rgbColor = new ColorRGB(hsvColor.Value, hsvColor.Value, hsvColor.Value, 1.0);
			}
			else
			{
				//if (hsvColor.Hue.IsNearOne())
				//{
				//    hsvColor = new ColorHSV(0.0, hsvColor.Saturation, hsvColor.Value);
				//}

				const double step = 1.0 / 6.0;

				double vh = hsvColor.Hue / step;
				var vi = (int)Math.Floor(vh);

				double v0 = vh - vi;
				double v1 = hsvColor.Value * (1.0 - hsvColor.Saturation);
				double v2 = hsvColor.Value * (1.0 - (hsvColor.Saturation * v0));
				double v3 = hsvColor.Value * (1.0 - (hsvColor.Saturation * (1.0 - v0)));

				switch (vi)
				{
					case 0:
						{
							rgbColor = new ColorRGB(hsvColor.Value, v3, v1, 1.0);
							break;
						}
					case 1:
						{
							rgbColor = new ColorRGB(v2, hsvColor.Value, v1, 1.0);
							break;
						}
					case 2:
						{
							rgbColor = new ColorRGB(v1, hsvColor.Value, v3, 1.0);
							break;
						}
					case 3:
						{
							rgbColor = new ColorRGB(v1, v2, hsvColor.Value, 1.0);
							break;
						}
					case 4:
						{
							rgbColor = new ColorRGB(v3, v1, hsvColor.Value, 1.0);
							break;
						}
					default:
						{
							rgbColor = new ColorRGB(hsvColor.Value, v1, v2, 1.0);
							break;
						}
				}
			}

			return rgbColor;
		}

		private static ColorRGB FromColor(ColorHSL hslColor)
		{
			double hue = Math.Min(1f, Math.Max(0f, hslColor.Hue));
			double luminance = Math.Min(1f, Math.Max(0f, hslColor.Lightness));
			double saturation = Math.Min(1f, Math.Max(0f, hslColor.Saturation));


			double r, g, b;

			if (saturation.IsNearZero())
			{
				r = luminance; //RGB results
				g = luminance;
				b = luminance;
			}
			else
			{
				double v2;
				hue = (hue - Math.Floor(hue)) * 6.0;

				if (luminance <= 0.5f)
				{
					v2 = luminance * (1f + saturation);
				}
				else
				{
					v2 = (luminance + saturation) - (luminance * saturation);
				}
				double v3 = (2f * luminance) - v2;
				r = HlsValue(v3, v2, hue + 2f);
				g = HlsValue(v3, v2, hue);
				b = HlsValue(v3, v2, hue - 2f);

			}

			return new ColorRGB(
				Math.Min(1.0, Math.Max(0.0, r)), 
				Math.Min(1.0, Math.Max(0.0, g)), 
				Math.Min(1.0, Math.Max(0.0, b)), 
				1.0);
		}

		private static ColorRGB FromColor(ColorCMYK cmyk)
        {
            double cmyCyan = cmyk.C*(1 - cmyk.K) + cmyk.K;
            double cmyMagenta = cmyk.M*(1 - cmyk.K) + cmyk.K;
            double cmyYellow = cmyk.Y*(1 - cmyk.K) + cmyk.K;

            double red = 1 - cmyCyan;
            double green = 1 - cmyMagenta;
            double blue = 1 - cmyYellow;

            var rgb = new ColorRGB(red, green, blue, 1.0);
            return rgb;
        }

		#endregion

		private static double HlsValue(double n1, double n2, double hue)
		{
			if (hue < 0f)
			{
				hue += 6f;
			}
			else if (hue >= 6f)
			{
				hue -= 6f;
			}
			if (hue < 1f)
			{
				return (n1 + ((n2 - n1) * hue));
			}
			if (hue < 3f)
			{
				return n2;
			}
			if (hue < 4f)
			{
				return (n1 + ((n2 - n1) * (4f - hue)));
			}
			return n1;
		}


		#endregion

		
	}
}