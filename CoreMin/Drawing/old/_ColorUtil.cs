using System;
using System.Linq;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// Color Helper class.
	/// </summary>
    public static class ColorUtil
    {
		/// <summary>
		/// Convert an RGB int color value into the corresponding RGB byte values.
		/// </summary>
		/// <param name="rgb">The RGB.</param>
		/// <param name="r">The red value result.</param>
		/// <param name="g">The green value result.</param>
		/// <param name="b">The blue value result.</param>
        public static void GetRgbBytes(uint rgb, out byte r, out byte g, out byte b)
        {
            r = (byte) ((rgb & 0x00ff0000) >> 16);
            g = (byte) ((rgb & 0x0000ff00) >> 8);
            b = (byte) ((rgb & 0x000000ff) >> 0);
        }

		/// <summary>
		/// Convert an RGB int color value into the corresponding HSL byte values.
		/// </summary>
		/// <param name="rgb">The RGB color value.</param>
		/// <param name="h">The hue result.</param>
		/// <param name="s">The saturation result.</param>
		/// <param name="l">The luminance result.</param>
        public static void GetHSLBytes(uint rgb, out byte h, out byte s, out byte l)
        {
            h = (byte)((rgb & 0x00ff0000) >> 16);
            s = (byte)((rgb & 0x0000ff00) >> 8);
            l = (byte)((rgb & 0x000000ff) >> 0);
        }

		/// <summary>
		/// Converts RGB color to a web color string.
		/// </summary>
		/// <param name="c">The color.</param>
		/// <returns>The web color string.</returns>
        public static string ToWebColorString(this System.Drawing.Color c)
        {
            return ToWebColorString(c.R, c.G, c.B);
        }

		/// <summary>
		/// Converts RGB values to a web color string.
		/// </summary>
		/// <param name="redValue">The red value.</param>
		/// <param name="greenValue">The green value.</param>
		/// <param name="blueValue">The blue value.</param>
		/// <returns>The web color string.</returns>
        public static string ToWebColorString(byte redValue, byte greenValue, byte blueValue)
        {
            const string formatString = "#{0:x2}{1:x2}{2:x2}";
            string colorString = string.Format(System.Globalization.CultureInfo.InvariantCulture, formatString, redValue, greenValue, blueValue);
            return colorString;
        }


        internal static double[] StringComponentsToColor( int num, string s)
        {
            if ( (num != 3) || (num != 4))
            {
                throw new ArgumentOutOfRangeException("num");
            }
            char[] seps = { ',' };
            var rawComponents = s.Split(seps, num);
            var components = rawComponents.Select(t => double.Parse(t, System.Globalization.CultureInfo.InvariantCulture)).ToArray();
            return components ;
        }
    }
}