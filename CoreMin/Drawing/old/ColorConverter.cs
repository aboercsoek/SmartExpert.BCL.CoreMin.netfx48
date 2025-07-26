using System;
using System.Globalization;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A class that converts all the available color schemas (RGB, HLS, HSV, HEX and CMYK) between each other.
	/// </summary>
	public static class ColorConverter
	{
		#region Private Constant Members

		private const double OneSixth = 1.0/6.0;

		private const double OneThird = 1.0/3.0;

		private const double TwoThird = 2.0/3.0;

		#endregion

		#region ctors

		/// <summary>
		/// Initializes static members of the <see cref="ColorConverter"/> class.
		/// </summary>
		static ColorConverter()
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

		#region CmykaTo... Converters

		/// <summary>
		/// Converts a <see cref="CmykaColor"/> to a <see cref="HexColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="CmykaColor"/> color.</param>
		/// <returns><see cref="HexColor"/></returns>
		public static HexColor ToHex(this CmykaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHex();
		}

		/// <summary>
		/// Converts a <see cref="CmykaColor"/> to a <see cref="HlsaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="CmykaColor"/> color.</param>
		/// <returns><see cref="HlsaColor"/></returns>
		public static HlsaColor ToHlsa(this CmykaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHlsa();
		}

		/// <summary>
		/// Converts a <see cref="CmykaColor"/> to a <see cref="HsvaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="CmykaColor"/> color.</param>
		/// <returns><see cref="HsvaColor"/></returns>
		public static HsvaColor ToHsva(this CmykaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHsva();
		}

		/// <summary>
		/// Converts a <see cref="CmykaColor"/> to a <see cref="RgbaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="CmykaColor"/> color.</param>
		/// <returns><see cref="RgbaColor"/></returns>
		public static RgbaColor ToRgba(this CmykaColor color)
		{
			double r = 1 - color.Cyan;
			double g = 1 - color.Magenta;
			double b = 1 - color.Yellow;
			//r *= 255;
			//g *= 255;
			//b *= 255;
			return new RgbaColor(r, g, b, color.Alpha);
		}

		#endregion

		#region HexTo... Converters

		/// <summary>
		/// Converts a <see cref="HexColor"/> to a <see cref="CmykaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HexColor"/> color.</param>
		/// <returns><see cref="CmykaColor"/></returns>
		public static CmykaColor ToCmyka(this HexColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToCmyka();
		}

		/// <summary>
		/// Converts a <see cref="HexColor"/> to a <see cref="HlsaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HexColor"/> color.</param>
		/// <returns><see cref="HlsaColor"/></returns>
		public static HlsaColor ToHlsa(this HexColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHlsa();
		}

		/// <summary>
		/// Converts a <see cref="HexColor"/> to a <see cref="HsvaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HexColor"/> color.</param>
		/// <returns><see cref="HsvaColor"/></returns>
		public static HsvaColor ToHsva(this HexColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHsva();
		}

		/// <summary>
		/// Converts a <see cref="HexColor"/> to a <see cref="RgbaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HexColor"/> color.</param>
		/// <returns><see cref="RgbaColor"/></returns>
		public static RgbaColor ToRgba(this HexColor color)
		{
			if (color.Value.IsNullOrEmptyWithTrim())
			{
				return new RgbaColor(0, 0, 0, 1.0);
			}

			string value = KnownColors.ContainsKey(color.Value) ? KnownColors[color.Value] : color.Value;

			string str;
			byte r, g, b;

			if (value.Substring(0, 1) == "#")
				str = value.Substring(1);
			else if (value.Substring(0, 2) == "0x")
				str = value.Substring(2);
			else
				str = value;

			if (str.Length == 3)
			{
				var tmp = "{0}{1}{2}{3}{4}{5}".SafeFormatWith(str[0], str[0], str[1], str[1], str[2], str[2]);
				str = tmp;
			}

			if (str.Length == 6)
			{
				var redSucceeded = byte.TryParse(str.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out r);
				var greenSucceeded = byte.TryParse(str.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out g);
				var blueSucceeded = byte.TryParse(str.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out b);
				if (redSucceeded && greenSucceeded && blueSucceeded)
				{
					return new RgbaColor(r/255.0, g/255.0, b/255.0, 1.0);
				}
			}

			if (str.Length == 8)
			{
				byte a;
				var alphaSucceeded = byte.TryParse(str.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out a);
				var redSucceeded = byte.TryParse(str.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out r);
				var greenSucceeded = byte.TryParse(str.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out g);
				var blueSucceeded = byte.TryParse(str.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out b);
				if (alphaSucceeded && redSucceeded && greenSucceeded && blueSucceeded)
				{
					return new RgbaColor(r / 255.0, g / 255.0, b / 255.0, a / 255.0);
				}
			}

			return new RgbaColor(0, 0, 0, 1.0);
		}

		#endregion

		#region HlsaTo... converters

		/// <summary>
		/// Converts a <see cref="HlsaColor"/> to a <see cref="CmykaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HlsaColor"/> color.</param>
		/// <returns><see cref="CmykaColor"/></returns>
		public static CmykaColor ToCmyka(this HlsaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToCmyka();
		}

		/// <summary>
		/// Converts a <see cref="HlsaColor"/> to a <see cref="HexColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HlsaColor"/> color.</param>
		/// <returns><see cref="HexColor"/></returns>
		public static HexColor ToHex(this HlsaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHex();
		}

		/// <summary>
		/// Converts a <see cref="HlsaColor"/> to a <see cref="HsvaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HlsaColor"/> color.</param>
		/// <returns><see cref="HsvaColor"/></returns>
		public static HsvaColor ToHsva(this HlsaColor color)
		{
			double l = color.Luminance;
			double s = color.Saturation;
			double hue = color.Hue;
			l *= 2;
			s *= (l <= 1) ? l : 2 - l;
			double value = (l + s) / 2;
			double saturation = (2 * s) / (l + s);
			hue = double.IsNaN(hue) ? 0.0 : hue;
			value = double.IsNaN(value) ? 0.0 : value;
			saturation = double.IsNaN(saturation) ? 0.0 : saturation;
			return new HsvaColor(hue, saturation, value, color.Alpha);
		}

		/// <summary>
		/// Converts a <see cref="HlsaColor"/> to a <see cref="RgbaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HlsaColor"/> color.</param>
		/// <returns><see cref="RgbaColor"/></returns>
		public static RgbaColor ToRgba(this HlsaColor color)
		{
			double h = color.Hue / 360;
			double l = color.Luminance;
			double s = color.Saturation;
			double a = color.Alpha;
			double m2;

			if (s.IsNearZero())
				return new RgbaColor(l, l, l, a);

			if (l <= 0.5)
				m2 = l*(1.0 + s);
			else
				m2 = l + s - (l*s);

			double m1 = (2.0 * l) - m2;
			double r = V(m1, m2, h + OneThird);
			double g = V(m1, m2, h);
			double b = V(m1, m2, h - OneThird);

			return new RgbaColor(r, g, b, a);
		}

		#endregion

		#region HsvaTo... Converters

		/// <summary>
		/// Converts a <see cref="HsvaColor"/> to a <see cref="CmykaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HsvaColor"/> color.</param>
		/// <returns><see cref="CmykaColor"/></returns>
		public static CmykaColor ToCmyka(this HsvaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToCmyka();
		}

		/// <summary>
		/// Converts a <see cref="HsvaColor"/> to a <see cref="HexColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HsvaColor"/> color.</param>
		/// <returns><see cref="HexColor"/></returns>
		public static HexColor ToHex(this HsvaColor color)
		{
			RgbaColor rgba = color.ToRgba();
			return rgba.ToHex();
		}

		/// <summary>
		/// Converts a <see cref="HsvaColor"/> to a <see cref="HlsaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HsvaColor"/> color.</param>
		/// <returns><see cref="HlsaColor"/></returns>
		public static HlsaColor ToHlsa(this HsvaColor color)
		{
			double hue = color.Hue;
			double luminance = (2 - color.Saturation) * color.Value;
			double saturation = color.Saturation * color.Value;
			saturation /= luminance <= 1 ? luminance : 2 - luminance;
			luminance /= 2;
			hue = double.IsNaN(hue) ? 0.0 : hue;
			luminance = double.IsNaN(luminance) ? 0.0 : luminance;
			saturation = double.IsNaN(saturation) ? 0.0 : saturation;
			return new HlsaColor(hue, luminance, saturation, color.Alpha);
		}

		/// <summary>
		/// Converts a <see cref="HsvaColor"/> to a <see cref="RgbaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="HsvaColor"/> color.</param>
		/// <returns><see cref="RgbaColor"/></returns>
		public static RgbaColor ToRgba(this HsvaColor color)
		{
			double h = color.Hue / 360;
			double s = color.Saturation;
			double v = color.Value;
			double a = color.Alpha;
			if (s.IsNearZero())
			{
				return new RgbaColor(v, v, v, a);
			}

			double i = decimal.Truncate((decimal)h * 6).As<double>();
			double f = (h * 6.0) - i;
			double p = v * (1.0 - s);
			double q = v * (1.0 - (s * f));
			double t = v * (1.0 - (s * (1.0 - f)));
			i = i % 6;

			//v *= 255;
			//t *= 255;
			//p *= 255;
			//q *= 255;

			if (i.IsNearZero())
			{
				return new RgbaColor(v, t, p, a);
			}

			if (i.IsNearOne())
			{
				return new RgbaColor(q, v, p, a);
			}

			if ((i-2).IsNearZero())
			{
				return new RgbaColor(p, v, t, a);
			}

			if ((i - 3).IsNearZero())
			{
				return new RgbaColor(p, q, v, a);
			}

			if ((i - 4).IsNearZero())
			{
				return new RgbaColor(t, p, v, a);
			}

			if ((i - 5).IsNearZero())
			{
				return new RgbaColor(v, p, q, a);
			}

			return new RgbaColor(0, 0, 0, a);
		}

		#endregion

		#region RgbaTo... Converters

		/// <summary>
		/// Converts a <see cref="RgbaColor"/> to a <see cref="CmykaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="RgbaColor"/> color.</param>
		/// <returns><see cref="CmykaColor"/></returns>
		public static CmykaColor ToCmyka(this RgbaColor color)
		{
			double key = 0;
			double low = 1.0;

			double cyan = (1.0 - color.Red);
			if (low > cyan)
			{
				low = cyan;
			}

			double magenta = (1.0 - color.Green);

			if (low > magenta)
			{
				low = magenta;
			}

			double yellow = (1.0 - color.Blue);

			if (low > yellow)
			{
				low = yellow;
			}

			if (low > 0.0)
			{
				key = low;
			}

			return new CmykaColor(cyan, magenta, yellow, key, color.Alpha);
		}

		/// <summary>
		/// Converts a <see cref="RgbaColor"/> to a <see cref="HexColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="RgbaColor"/> color.</param>
		/// <returns><see cref="HexColor"/></returns>
		public static HexColor ToHex(this RgbaColor color)
		{
			string hex = string.Format("{0}{1}{2}{3}",
				Convert.ToByte(color.Alpha * 255).ToString("X2"), Convert.ToByte(color.Red * 255).ToString("X2"),
				Convert.ToByte(color.Green * 255).ToString("X2"), Convert.ToByte(color.Blue * 255).ToString("X2"));

			return new HexColor(hex);
		}

		/// <summary>
		/// Converts a <see cref="RgbaColor"/> to a <see cref="HlsaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="RgbaColor"/> color.</param>
		/// <returns><see cref="HlsaColor"/></returns>
		public static HlsaColor ToHlsa(this RgbaColor color)
		{
			double r = color.Red;
			double g = color.Green;
			double b = color.Blue;
			double a = color.Alpha;
			double maxc = Math.Max(r, Math.Max(g, b));
			double minc = Math.Min(r, Math.Min(g, b));
			double l = (minc + maxc) / 2.0;
			double s;
			double h;
			
			if ((minc - maxc).IsNearZero())
				return new HlsaColor(0.0, l, 0.0, a);

			if (l <= 0.5)
				s = (maxc - minc)/(maxc + minc);
			else
				s = (maxc - minc)/(2.0 - maxc - minc);

			double rc = (maxc - r) / (maxc - minc);
			double gc = (maxc - g) / (maxc - minc);
			double bc = (maxc - b) / (maxc - minc);
			
			if ((r - maxc).IsNearZero())
				h = bc - gc;
			else if ((g - maxc).IsNearZero())
				h = 2.0 + rc - bc;
			else
				h = 4.0 + gc - rc;

			h = h / 6.0;
			h = CustomMod(h);
			return new HlsaColor(h * 360, l, s, a);
		}

		/// <summary>
		/// Converts a <see cref="RgbaColor"/> to a <see cref="HsvaColor"/>.
		/// </summary>
		/// <param name="color">The <see cref="RgbaColor"/> color.</param>
		/// <returns><see cref="HsvaColor"/></returns>
		public static HsvaColor ToHsva(this RgbaColor color)
		{
			double r = color.Red;
			double g = color.Green;
			double b = color.Blue;
			double a = color.Alpha;
			double maxc = Math.Max(r, Math.Max(g, b));
			double minc = Math.Min(r, Math.Min(g, b));
			double v = maxc;
			double h;
			if ((minc - maxc).IsNearZero())
				return new HsvaColor(0.0, 0.0, v, a);

			double s = (maxc - minc) / maxc;
			double rc = (maxc - r) / (maxc - minc);
			double gc = (maxc - g) / (maxc - minc);
			double bc = (maxc - b) / (maxc - minc);
			if ((r - maxc).IsNearZero())
				h = bc - gc;
			else if ((g - maxc).IsNearZero())
				h = 2.0 + rc - bc;
			else
				h = 4.0 + gc - rc;

			h = h / 6.0;
			h = CustomMod(h);
			return new HsvaColor(h * 360, s, v, a);
		}

		#endregion

		#region Private Methods

		private static double V(double m1, double m2, double hue)
		{
			hue = CustomMod(hue);

			if (hue < OneSixth)
			{
				return m1 + (((m2 - m1) * hue) * 6.0);
			}

			if (hue < 0.5)
			{
				return m2;
			}

			if (hue < TwoThird)
			{
				return m1 + (((m2 - m1) * (TwoThird - hue)) * 6.0);
			}

			return m1;
		}

		private static double CustomMod(double number)
		{
			if (number > 0)
			{
				return number - Math.Floor(number);
			}
			
			if (number < 0)
			{
				double abs = Math.Abs(number);
				return 1 - (abs - Math.Floor(abs));
			}

			return 0;
		}

		#endregion
	}
}