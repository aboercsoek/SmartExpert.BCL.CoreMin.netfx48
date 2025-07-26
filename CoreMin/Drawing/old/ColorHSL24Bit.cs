namespace SmartExpert.Drawing
{
	/// <summary>
	/// HSL color data type.
	/// </summary>
    public struct ColorHSL24Bit
    {
        private readonly byte m_Hue;
        private readonly byte m_Saturation;
        private readonly byte m_Luminance;

		#region ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="luminance">The luminance.</param>
		public ColorHSL24Bit(byte hue, byte saturation, byte luminance)
        {
            m_Hue = hue;
            m_Saturation = saturation;
            m_Luminance = luminance;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="luminance">The luminance.</param>
        public ColorHSL24Bit(short hue, short saturation, short luminance)
        {
            m_Hue = (byte)hue;
            m_Saturation = (byte)saturation;
            m_Luminance = (byte)luminance;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorHSL24Bit(System.Drawing.Color color)
        {
            var rgb24 = new ColorRGB24Bit(color.ToArgb());
            var rgb = new ColorRGB(rgb24);
            var hsl = new ColorHSL(rgb);
            var hsl24 = new ColorHSL24Bit(hsl);
            m_Hue = hsl24.Hue;
            m_Saturation = hsl24.Saturation;
            m_Luminance = hsl24.Luminance;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorHSL24Bit(ColorHSL color)
        {
            m_Hue = (byte)(color.Hue * 255);
            m_Saturation = (byte)(color.Saturation * 255);
            m_Luminance = (byte)(color.Luminance * 255);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorHSL24Bit(ColorHSV color)
        {
            var rgb = new ColorHSL(color);
            m_Hue = (byte)(rgb.Hue * 255);
            m_Saturation = (byte)(rgb.Saturation * 255);
            m_Luminance = (byte)(rgb.Luminance * 255);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ColorHSL24Bit"/> struct.
		/// </summary>
		/// <param name="color">The color.</param>
        public ColorHSL24Bit(ColorRGB color)
        {
            var temp = FromColor(color);
            m_Hue = temp.m_Hue;
            m_Saturation = temp.m_Saturation;
            m_Luminance = temp.m_Luminance;
        }

		#endregion

		#region Properties


		/// <summary>
		/// Gets the hue.
		/// </summary>
		public byte Hue
        {
            get { return m_Hue; }
        }

		/// <summary>
		/// Gets the saturation.
		/// </summary>
        public byte Saturation
        {
            get { return m_Saturation; }
        }

		/// <summary>
		/// Gets the luminance.
		/// </summary>
		public byte Luminance
        {
            get { return m_Luminance; }
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
            var s = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}({1},{2},{3})",
                                  typeof(ColorHSL24Bit).Name, m_Hue, m_Saturation, m_Luminance);
            return s;
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
            return other is ColorHSL24Bit && Equals((ColorHSL24Bit)other);
        }

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="lhs">The LHS.</param>
		/// <param name="rhs">The RHS.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
        public static bool operator ==(ColorHSL24Bit lhs, ColorHSL24Bit rhs)
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
        public static bool operator !=(ColorHSL24Bit lhs, ColorHSL24Bit rhs)
        {
            return !lhs.Equals(rhs);
        }


		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
        public override int GetHashCode()
        {
            return ToHSL();
        }

		/// <summary>
		/// Blends two colors by using a ratio.
		/// </summary>
		/// <param name="a">Color a</param>
		/// <param name="b">Color b.</param>
		/// <param name="ratio">The ratio to apply.</param>
		/// <returns>The blend result color.</returns>
        public static ColorHSL24Bit Blend(ColorHSL24Bit a, ColorHSL24Bit b, double ratio)
        {
            var normRatio = DrawingUtil.ClampToRangeFrom0To1(ratio);
            var antiRatio = 1.0 - normRatio;
            var h = (a.m_Hue * normRatio) + (b.m_Hue * antiRatio);
            var s = (a.m_Saturation * normRatio) + (b.m_Saturation * antiRatio);
            var l = (a.m_Luminance * normRatio) + (b.m_Luminance * antiRatio);
            return new ColorHSL24Bit((byte)h, (byte)s, (byte)l);
        }

        /// <summary>
        /// Returns an int containing the RGB color value.
        /// </summary>
		/// <returns>The RGB color value</returns>
        public int ToHSL()
        {
            return (m_Hue << 16) | (m_Saturation << 8) | (m_Luminance);
        }

		#endregion

		#region Private Methods

		private static ColorHSL24Bit FromColor(ColorRGB color)
		{
			return new ColorHSL24Bit(new ColorHSL(color));
		}

		private bool Equals(ColorHSL24Bit other)
		{
			return (m_Hue == other.m_Hue && m_Saturation == other.m_Saturation && m_Luminance == other.m_Luminance);
		}

		#endregion

	}
}