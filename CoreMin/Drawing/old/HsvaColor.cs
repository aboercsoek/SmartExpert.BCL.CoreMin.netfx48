
using System;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A struct representing a color comprised of Hue, Saturation, Value and Alpha channels.
	/// HSV is also often called HSB (B for brightness). 
	/// For more info: http://en.wikipedia.org/wiki/HSV_color_space
	/// </summary>
	public struct HsvaColor
	{
		private readonly double m_Alpha;

		private readonly double m_Hue;

		private readonly double m_Saturation;

		private readonly double m_Value;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="HsvaColor" /> struct.
		/// </summary>
		/// <param name="hue">The hue channel. [0.0 .. 360.0]</param>
		/// <param name="saturation">The saturation channel. [0.0 .. 1.0]</param>
		/// <param name="value">The value channel. [0.0 .. 1.0]</param>
		/// <param name="alpha">The alpha channel. [0.0 .. 1.0]</param>
		public HsvaColor(double hue, double saturation, double value, double alpha)
		{
			m_Hue = hue.Clamp(0.0, 360.0, 5);
			m_Saturation = saturation.Clamp(0.0, 1.0, 5);
			m_Value = value.Clamp(0.0, 1.0, 5);
			m_Alpha = alpha.Clamp(0.0, 1.0, 5);
		}
		
		/// <summary>
		/// Gets the alpha (opacity) channel of the <see cref="HsvaColor"/> instance.
		/// </summary>
		/// <value>The alpha channel.</value>
		public double Alpha
		{
			get
			{
				return m_Alpha;
			}
		}
		
		/// <summary>
		/// Gets the hue channel of the <see cref="HsvaColor"/> instance.
		/// </summary>
		/// <value>The hue channel.</value>
		public double Hue
		{
			get
			{
				return m_Hue;
			}
		}
		
		/// <summary>
		/// Gets the saturation channel of the <see cref="HsvaColor"/> instance.
		/// </summary>
		/// <value>The saturation channel.</value>
		public double Saturation
		{
			get
			{
				return m_Saturation;
			}
		}
		
		/// <summary>
		/// Gets the value channel of the <see cref="HsvaColor"/> instance.
		/// </summary>
		/// <value>The value channel.</value>
		public double Value
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
			return "hsva({0}°,{1},{2},{3})"
						.SafeFormatWith(Math.Round(Hue, 0), Math.Round(Saturation, 2), Math.Round(Value, 2), Math.Round(Alpha, 2));
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(HsvaColor instance1, HsvaColor instance2)
		{
			// Return true if the fields match:
			return AreEqual(instance1, instance2);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=(HsvaColor instance1, HsvaColor instance2)
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
			if (obj == null || !(obj is HsvaColor))
			{
				return false;
			}

			var other = (HsvaColor)obj;

			return AreEqual(this, other);
		}

		/// <summary>
		/// Equalses the specified other instance.
		/// </summary>
		/// <param name="otherInstance">The other instance.</param>
		/// <returns></returns>
		public bool Equals(HsvaColor otherInstance)
		{
			return AreEqual(this, otherInstance);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return (int)Hue ^ (int)Saturation ^ (int)Value ^ (int)Alpha;
		}

		private static bool AreEqual(HsvaColor instance1, HsvaColor instance2)
		{
			return (instance1.Hue - instance2.Hue).IsNearZero() &&
				   (instance1.Value - instance2.Value).IsNearZero() &&
				   (instance1.Saturation - instance2.Saturation).IsNearZero() &&
				   (instance1.Alpha - instance2.Alpha).IsNearZero();
		}
	}
}