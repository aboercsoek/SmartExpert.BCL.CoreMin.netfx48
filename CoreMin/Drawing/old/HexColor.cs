namespace SmartExpert.Drawing
{
	/// <summary>
	/// A struct representing a Hex color representation of a color.
	/// </summary>
	public struct HexColor
	{
		private readonly string m_Value;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="HexColor" /> struct.
		/// </summary>
		/// <param name="value">The hex value.</param>
		public HexColor(string value)
		{
			m_Value = value;
		}
		
		/// <summary>
		/// Gets the hex value of the <see cref="HexColor" /> class.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return m_Value;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			string value = ColorConverter.KnownColors.ContainsKey(m_Value) ? ColorConverter.KnownColors[m_Value] : m_Value;
			return value.EnsureStartsWith("#").ToUpperInvariant();
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(HexColor instance1, HexColor instance2)
		{
			// Return true if the fields match:
			return instance1.Value.Equals(instance2);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(HexColor instance1, HexColor instance2)
		{
			return !(instance1 == instance2);
		}
		
		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object" /> is equal
		/// to the current <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with the current
		/// <see cref="T:System.Object" />.</param>
		/// <returns>
		/// True if the specified <see cref="T:System.Object" /> is equal to the
		/// current <see cref="T:System.Object" />; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is HexColor))
			{
				return false;
			}

			var other = (HexColor)obj;

			return Value.Equals(other.Value);
		}

		/// <summary>
		/// Compares this <see cref="HexColor"/> instance to another <see cref="HexColor"/> instance.
		/// </summary>
		/// <param name="otherHex">The other <see cref="HexColor"/> instance.</param>
		public bool Equals(HexColor otherHex)
		{
			return Value.Equals(otherHex.Value);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}