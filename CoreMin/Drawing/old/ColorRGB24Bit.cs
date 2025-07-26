using System.Drawing;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A RGB color type with R,G,B as byte values.
	/// </summary>
    public struct ColorRGB24Bit
    {
        private readonly byte m_Red;
        private readonly byte m_Green;
        private readonly byte m_Blue;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="redValue">The red value.</param>
		/// <param name="greenValue">The green value.</param>
		/// <param name="blueValue">The blue value.</param>
		public ColorRGB24Bit(byte redValue, byte greenValue, byte blueValue)
        {
            m_Red = redValue;
            m_Green = greenValue;
            m_Blue = blueValue;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="redValue">The red value.</param>
		/// <param name="greenValue">The green value.</param>
		/// <param name="blueValue">The blue value.</param>
        public ColorRGB24Bit(short redValue, short greenValue, short blueValue)
        {
            m_Red = (byte) redValue;
            m_Green = (byte) greenValue;
			m_Blue = (byte)blueValue;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="rgbValue">The RGB value.</param>
        public ColorRGB24Bit(int rgbValue)
        {
            ColorUtil.GetRgbBytes((uint) rgbValue, out m_Red, out m_Green, out m_Blue);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="rgbValue">The RGB value.</param>
        public ColorRGB24Bit(uint rgbValue)
        {
            ColorUtil.GetRgbBytes(rgbValue, out m_Red, out m_Green, out m_Blue);
        }


		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorRGB24Bit(Color color)
        {
            m_Red = color.R;
            m_Green = color.G;
            m_Blue = color.B;
        }


		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorRGB24Bit(ColorRGB color)
        {
            ColorRGB.CheckRGBInRange(color.R, color.G, color.B);
            m_Red = (byte) (color.R*255);
            m_Green = (byte) (color.G*255);
            m_Blue = (byte) (color.B*255);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorRGB24Bit(ColorHSV color)
        {
            var temp = FromColor(color);
            m_Red = temp.m_Red;
            m_Green = temp.m_Green;
            m_Blue = temp.m_Blue;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorRGB24Bit(ColorHSL color)
        {
            var temp = FromColor(color);
            m_Red = temp.m_Red;
            m_Green = temp.m_Green;
            m_Blue = temp.m_Blue;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorRGB24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorRGB24Bit(ColorCMYK color)
        {
            var temp = FromColor(color);
            m_Red = temp.m_Red;
            m_Green = temp.m_Green;
            m_Blue = temp.m_Blue;
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Red Value.
		/// </summary>
		public byte R
        {
            get { return m_Red; }
        }

		/// <summary>
		/// Gets the Green value.
		/// </summary>
        public byte G
        {
            get { return m_Green; }
        }

		/// <summary>
		/// Gets the Blue value.
		/// </summary>
        public byte B
        {
            get { return m_Blue; }
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
            var s = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}({1},{2},{3})",
                                  typeof (ColorRGB24Bit).Name, m_Red, m_Green, m_Blue);
            return s;
        }

        /// <summary>
        /// A convenient explicit cast to System.Drawing.Color 
        /// </summary>
        /// <param name="color">The source color</param>
        /// <returns>The convertion result.</returns>
        public static explicit operator Color(ColorRGB24Bit color)
        {
            return Color.FromArgb(color.m_Red, color.m_Green, color.m_Blue);
        }

        /// <summary>
        /// a convenient explicit cast to an int
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static explicit operator int(ColorRGB24Bit color)
        {
            return color.ToRGB();
        }

        /// <summary>
        /// a convenient explicit cast an int to a ColorRGB24Bit
        /// </summary>
        /// <param name="rgbint"></param>
        /// <returns></returns>
        public static explicit operator ColorRGB24Bit(int rgbint)
        {
            return new ColorRGB24Bit(rgbint);
        }


		/// <summary>
		/// Convert the RGB color into a web color string.
		/// </summary>
		/// <returns>The web color string.</returns>
        public string ToWebColorString()
        {
            return ColorUtil.ToWebColorString(m_Red, m_Green, m_Blue);
        }


		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
        public override bool Equals(object other)
        {
            return other is ColorRGB24Bit && Equals((ColorRGB24Bit) other);
        }

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="lhs">The LHS.</param>
		/// <param name="rhs">The RHS.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
        public static bool operator ==(ColorRGB24Bit lhs, ColorRGB24Bit rhs)
        {
            return lhs.Equals(rhs);
        }

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="lhs">The LHS.</param>
		/// <param name="rhs">The RHS.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
        public static bool operator !=(ColorRGB24Bit lhs, ColorRGB24Bit rhs)
        {
            return !lhs.Equals(rhs);
        }

        private bool Equals(ColorRGB24Bit other)
        {
            return (m_Red == other.m_Red && m_Green == other.m_Green && m_Blue == other.m_Blue);
        }

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
        public override int GetHashCode()
        {
            return ToRGB();
        }

		/// <summary>
		/// Blends two colors by using a ratio.
		/// </summary>
		/// <param name="a">Color a</param>
		/// <param name="b">Color b.</param>
		/// <param name="ratio">The ratio to apply.</param>
		/// <returns>The blend result color.</returns>
        public static ColorRGB24Bit Blend(ColorRGB24Bit a, ColorRGB24Bit b, double ratio)
        {
			var normRatio = DrawingUtil.ClampToRangeFrom0To1(ratio);
            var antiRatio = 1.0 - normRatio;
            var newRed = (a.m_Red*normRatio) + (b.m_Red*antiRatio);
            var newGreen = (a.m_Green*normRatio) + (b.m_Green*antiRatio);
            var newBlue = (a.m_Blue*normRatio) + (b.m_Blue*antiRatio);
            return new ColorRGB24Bit((byte) newRed, (byte) newGreen, (byte) newBlue);
        }

        /// <summary>
        /// Returns an int containing RGB values.
        /// </summary>
        /// <returns>The the rgb color as int value.</returns>
        public int ToRGB()
        {
            return (m_Red << 16) | (m_Green << 8) | (m_Blue);
        }


        /// <summary>
        /// Parses a web color string of form "#ffffff"
        /// </summary>
        /// <param name="webcolor"></param>
        /// <returns></returns>
        public static ColorRGB24Bit ParseWebColorString(string webcolor)
        {
            var outputcolor= TryParseWebColorString(webcolor);
            if (!outputcolor.HasValue)
            {
                var s = string.Format("Failed to parse color string \"{0}\"", webcolor);
                throw new ColorException(s);
            }

            return outputcolor.Value;
        }

        /// <summary>
        /// Try to parse a web color string into a ColorRGB24Bit value.
        /// </summary>
        /// <example>
        /// Sample usage:
        ///
        /// System.Drawing.Color c;
        /// bool result = TryParseRGBWebColorString("#ffffff", ref c);
        /// if (result)
        /// {
        ///    //it was correctly parsed
        /// }
        /// else
        /// {
        ///    //it was not correctly parsed
        /// }
        ///
        /// </example>
        /// <param name="webcolor"></param>
        ///<returns></returns>
        public static ColorRGB24Bit? TryParseWebColorString(string webcolor)
        {
            // fail if string is null
            if (webcolor == null)
            {
                return null;
            }

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }


            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // strip leading # if it is there
            while (webcolor.StartsWith("#"))
            {
                webcolor = webcolor.Substring(1);
            }

            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // fail if string doesn't have exactly 6 digits
            if (webcolor.Length != 6)
            {
                return null;
            }

            int currentColor;
            bool result = System.Int32.TryParse(webcolor, System.Globalization.NumberStyles.HexNumber, null, out currentColor);

            if (!result)
            {
                // fail if parsing didn't work
                return null;
            }

            // at this point parsing worked

            // the integer value is converted directly to an rgb value

            var theColor = new ColorRGB24Bit(currentColor);
            return theColor;
        }


		/// <summary>
		/// Gets the rgb color intensity.
		/// </summary>
		/// <returns>the rgb color intensity.</returns>
		public byte GetIntensityByte()
		{
			return (byte)((((7471 * B) + (38470 * G)) + (19595 * R)) >> 16);
		}

		/// <summary>
		/// Gets the rgb color intensity.
		/// </summary>
		/// <returns>the rgb color intensity.</returns>
		public double GetIntensity()
		{
			return ((((0.114 * B) + (0.587 * G)) + (0.299 * R)) / 255.0);
		}

		/// <summary>
		/// Returns the gray value of the RGB color.
		/// </summary>
		/// <returns></returns>
		public ColorRGB24Bit ToGray()
		{
			byte intensity = GetIntensityByte();
			return new ColorRGB24Bit(intensity, intensity, intensity);
		}






		#endregion

		#region Private Methods

		private static ColorRGB24Bit FromColor(ColorHSV color)
		{
			return new ColorRGB24Bit(new ColorRGB(color));
		}

		private static ColorRGB24Bit FromColor(ColorHSL color)
		{
			return new ColorRGB24Bit(new ColorRGB(color));
		}

		private static ColorRGB24Bit FromColor(ColorCMYK color)
		{
			return new ColorRGB24Bit(new ColorRGB(color));
		}

		#endregion

	}
}