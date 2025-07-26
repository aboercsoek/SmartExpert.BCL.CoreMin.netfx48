using System;
using System.Windows.Media;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A struct representing a color comprised of Red, Green, Blue and Alpha channels.
	/// </summary>
	public struct RgbaColor
	{
		private readonly double m_Alpha;

		private readonly double m_Blue;

		private readonly double m_Green;

		private readonly double m_Red;

		/// <summary>
		/// Initializes a new instance of the <see cref="RgbaColor" /> struct.
		/// </summary>
		/// <param name="red">The red channel [0..255].</param>
		/// <param name="green">The green channel [0..255].</param>
		/// <param name="blue">The blue channel [0..255].</param>
		/// <param name="alpha">The alpha channel [0..1].</param>
		public RgbaColor(double red, double green, double blue, double alpha)
		{
			m_Red = red.Clamp(0.0, 1.0, 5);
			m_Green = green.Clamp(0.0, 1.0, 5);
			m_Blue = blue.Clamp(0.0, 1.0, 5);
			m_Alpha = alpha.Clamp(0.0, 1.0, 5);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RgbaColor" /> struct.
		/// </summary>
		/// <param name="color">The color.</param>
		public RgbaColor(Color color)
		{
			m_Red = color.R / 255.0;
			m_Green = color.G / 255.0;
			m_Blue = color.B / 255.0;
			m_Alpha = color.A / 255.0;
		}

		/// <summary>
		/// Gets the alpha (opacity) channel of the <see cref="RgbaColor"/> instance.
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
		/// Gets the blue channel of the <see cref="RgbaColor"/> instance.
		/// </summary>
		/// <value>The blue channel.</value>
		public double Blue
		{
			get
			{
				return m_Blue;
			}
		}

		/// <summary>
		/// Gets the green channel of the <see cref="RgbaColor"/> instance.
		/// </summary>
		/// <value>The green channel.</value>
		public double Green
		{
			get
			{
				return m_Green;
			}
		}

		/// <summary>
		/// Gets the red channel of the <see cref="RgbaColor"/> instance.
		/// </summary>
		/// <value>The red channel.</value>
		public double Red
		{
			get
			{
				return m_Red;
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
			return "rgba({0},{1},{2},{3})"
						.SafeFormatWith(Math.Round(Red, 2), Math.Round(Green, 2), Math.Round(Blue, 2), Math.Round(Alpha, 2));
		}

		///// <summary>
		///// Returns an int containing RGB values.
		///// </summary>
		///// <returns>The rgb color as int value.</returns>
		//public int ToRgbValue()
		//{
		//    return (m_Red.BoundToByte() << 16) | (m_Green.BoundToByte() << 8) | (m_Blue.BoundToByte());
		//}

		///// <summary>
		///// Returns an int containing RGBA values.
		///// </summary>
		///// <returns>The the rgba color as int value.</returns>
		//public int ToRgbaValue()
		//{
		//    return ((m_Alpha*255).BoundToByte() << 24) | (m_Red.BoundToByte() << 16) | (m_Green.BoundToByte() << 8) | (m_Blue.BoundToByte());
		//}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(RgbaColor instance1, RgbaColor instance2)
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
		public static bool operator !=(RgbaColor instance1, RgbaColor instance2)
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
			if (obj == null || !(obj is RgbaColor))
			{
				return false;
			}

			var other = (RgbaColor)obj;

			return AreEqual(this, other);
		}

		/// <summary>
		/// Equalses the specified other instance.
		/// </summary>
		/// <param name="otherInstance">The other instance.</param>
		/// <returns></returns>
		public bool Equals(RgbaColor otherInstance)
		{
			return AreEqual(this, otherInstance);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return Red.ToInt() ^ Green.ToInt() ^ Blue.ToInt() ^ Alpha.ToInt();
		}

		private static bool AreEqual(RgbaColor instance1, RgbaColor instance2)
		{
			return (instance1.Red - instance2.Red).IsNearZero() &&
				   (instance1.Green - instance2.Green).IsNearZero() &&
				   (instance1.Blue - instance2.Blue).IsNearZero() &&
				   (instance1.Alpha - instance2.Alpha).IsNearZero();
		}
	}
}