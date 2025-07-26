using System;
using SmartExpert.Calculation;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A struct representing a color comprised of Cyan, Magenta, Yellow, Key (Black) and Alpha channels.
	/// </summary>
	public struct CmykaColor
	{
		private readonly double m_Alpha;

		private readonly double m_Cyan;

		private readonly double m_Key;

		private readonly double m_Magenta;

		private readonly double m_Yellow;

		/// <summary>
		/// Initializes a new instance of the <see cref="CmykaColor" /> struct.
		/// </summary>
		/// <param name="cyan">The cyan channel.</param>
		/// <param name="magenta">The magenta channel.</param>
		/// <param name="yellow">The yellow channel.</param>
		/// <param name="key">The key (black) channel.</param>
		/// <param name="alpha">The alpha channel.</param>
		public CmykaColor(double cyan, double magenta, double yellow, double key, double alpha)
		{
			m_Cyan = cyan.Clamp(0.0, 1.0, 5);
			m_Magenta = magenta.Clamp(0.0, 1.0, 5);
			m_Yellow = yellow.Clamp(0.0, 1.0, 5);
			m_Key = key.Clamp(0.0, 1.0, 5);
			m_Alpha = alpha.Clamp(0.0, 1.0, 5);
		}

		/// <summary>
		/// Gets the alpha (opacity) channel of the <see cref="CmykaColor"/> instance.
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
		/// Gets the cyan channel of the <see cref="CmykaColor"/> instance.
		/// </summary>
		/// <value>The cyan channel.</value>
		public double Cyan
		{
			get
			{
				return m_Cyan;
			}
		}

		/// <summary>
		/// Gets the key (black) channel of the <see cref="CmykaColor"/> instance.
		/// </summary>
		/// <value>The key channel.</value>
		public double Key
		{
			get
			{
				return m_Key;
			}
		}

		/// <summary>
		/// Gets the magenta channel of the <see cref="CmykaColor"/> instance.
		/// </summary>
		/// <value>The magenta channel.</value>
		public double Magenta
		{
			get
			{
				return m_Magenta;
			}
		}

		/// <summary>
		/// Gets the yellow channel of the <see cref="CmykaColor"/> instance.
		/// </summary>
		/// <value>The yellow channel.</value>
		public double Yellow
		{
			get
			{
				return m_Yellow;
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
			return "cmyka({0},{1},{2},{3},{4})"
						.SafeFormatWith(Math.Round(Cyan, 2), Math.Round(Magenta, 2), Math.Round(Yellow, 2), Math.Round(Key, 2), 
										Math.Round(Alpha, 2));
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="instance1">The instance1.</param>
		/// <param name="instance2">The instance2.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==(CmykaColor instance1, CmykaColor instance2)
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
		public static bool operator !=(CmykaColor instance1, CmykaColor instance2)
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
			if (obj == null || !(obj is CmykaColor))
			{
				return false;
			}

			var other = (CmykaColor)obj;

			return AreEqual(this, other);
		}

		/// <summary>
		/// Equalses the specified other instance.
		/// </summary>
		/// <param name="otherInstance">The other instance.</param>
		/// <returns></returns>
		public bool Equals(CmykaColor otherInstance)
		{
			return AreEqual(this, otherInstance);
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return (int)Cyan ^ (int)Magenta ^ (int)Yellow ^ (int)Key ^ (int)Alpha;
		}

		private static bool AreEqual(CmykaColor instance1, CmykaColor instance2)
		{
			return (instance1.Cyan - instance2.Cyan).IsNearZero() &&
				   (instance1.Magenta - instance2.Magenta).IsNearZero() &&
				   (instance1.Yellow - instance2.Yellow).IsNearZero() &&
				   (instance1.Key - instance2.Key).IsNearZero() &&
				   (instance1.Alpha - instance2.Alpha).IsNearZero();

		}
	}
}