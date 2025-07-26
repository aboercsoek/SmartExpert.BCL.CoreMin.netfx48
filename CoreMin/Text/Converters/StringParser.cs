//--------------------------------------------------------------------------
// File:    StringParser.cs
// Content:	Implementation of a string parsing helper class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Globalization;
using System.Xml;
using SmartExpert.Error;
using SmartExpert.Time;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// String parsing helper class
	/// </summary>
	public static class StringParser
	{
		#region Parse...-Methods

		/// <summary>
		/// Parses the String to Int32. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed Int32 value.</returns>
		public static int ParseInt32(string str)
		{
			return ParseInt32(str, 0);
		}

		/// <summary>
		/// Parses the String to Int32.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed Int32 value.</returns>
		public static int ParseInt32(string str, int defaultValue)
		{
			int result;
			if (!int.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parses the hex string to Int32. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed Int32 value.</returns>
		public static int ParseInt32Hex(string str)
		{
			return ParseInt32Hex(str, 0);
		}

		/// <summary>
		/// Parses the hex string to Int32.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed Int32 value.</returns>
		public static int ParseInt32Hex(string str, int defaultValue)
		{
			int result;

			if (string.IsNullOrEmpty(str))
				return defaultValue;

			if (str[0] == 'x' || str[0] == 'X')
			{
				str = str.Substring(1);
			}
			else if (str.Length > 1 && str[0] == '0' && (str[1] == 'x' || str[1] == 'X'))
			{
				str = str.Substring(2);
			}

			try
			{
				result = int.Parse(str, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Parses the short. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed Int16 value.</returns>
		public static short ParseInt16(string str)
		{
			return ParseInt16(str, 0);
		}

		/// <summary>
		/// Parses the short.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static short ParseInt16(string str, short defaultValue)
		{
			short result;
			if (!short.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parses the long. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed Int64 value.</returns>
		public static long ParseInt64(string str)
		{
			return ParseInt64(str, 0L);
		}

		/// <summary>
		/// Parses the long.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed Int64 value.</returns>
		public static long ParseInt64(string str, long defaultValue)
		{
			long result;
			if (!long.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parses the unsigned long. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed UInt64 value.</returns>
		public static ulong ParseUInt64(string str)
		{
			return ParseUInt64(str, 0UL);
		}

		/// <summary>
		/// Parses the unsigned long.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed UInt64 value.</returns>
		public static ulong ParseUInt64(string str, ulong defaultValue)
		{
			ulong result;
			if (!ulong.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parses the String to float. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed float value.</returns>
		public static float ParseFloat(string str)
		{
			return ParseFloat(str, 0f);
		}

		/// <summary>
		/// Parses the String to float.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed float value.</returns>
		public static float ParseFloat(string str, float defaultValue)
		{
			if (str.IsNullOrEmptyWithTrim())
				return defaultValue;
			
			var whitespaceChars = new [] { ' ', '\t', '\n', '\r' };
			var local = str.Trim(whitespaceChars).ToUpper();

			if (local == "-INF")
				return float.NegativeInfinity;
			if (local == "INF")
				return float.PositiveInfinity;
			
			float result;
			
			if (!float.TryParse(local, 
								NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, 
								NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			else if ((Math.Abs(result) < float.Epsilon)&&(local[0] == '-'))
			{
				return 0f;
			}
			
			return result;
		}

		/// <summary>
		/// Parses the String to double. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed double value.</returns>
		public static double ParseDouble(string str)
		{
			return ParseDouble(str, 0.0);
		}

		/// <summary>
		/// Parses the String to double.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed double value.</returns>
		public static double ParseDouble(string str, double defaultValue)
		{
			if (str.IsNullOrEmptyWithTrim())
				return defaultValue;

			var whitespaceChars = new[] { ' ', '\t', '\n', '\r' };
			var local = str.Trim(whitespaceChars).ToUpper();

			if (local == "-INF")
				return double.NegativeInfinity;
			if (local == "INF")
				return double.PositiveInfinity;

			double result;
			if (!double.TryParse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			else if ((Math.Abs(result) < double.Epsilon) && (local[0] == '-'))
			{
				return 0.0;
			}
			return result;
		}

		/// <summary>
		/// Parses the String to decimal. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed decimal value.</returns>
		public static decimal ParseDecimal(string str)
		{
			return ParseDecimal(str, 0);
		}

		/// <summary>
		/// Parses String to decimal.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The parsed decimal value.</returns>
		public static decimal ParseDecimal(string str, decimal defaultValue)
		{
			if (str.IsNullOrEmptyWithTrim())
				return defaultValue;

			decimal result;
			if (!decimal.TryParse(str, NumberStyles.AllowDecimalPoint | NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parse string to bool.
		/// </summary>
		/// <param name="boolString">The string to parse.</param>
		/// <returns>The parsed bool result (default value = false).</returns>
		public static bool ParseBool(string boolString)
		{
			return ParseBool(boolString, false);
		}

		/// <summary>
		/// Parse string to bool.
		/// </summary>
		/// <param name="boolString">The string to parse.</param>
		/// <param name="defaultValue">The default value used if parsing failed.</param>
		/// <returns>The parsed bool result.</returns>
		public static bool ParseBool(string boolString, bool defaultValue)
		{
			if (boolString.IsNullOrEmptyWithTrim())
				return defaultValue;

			switch (boolString.Trim().ToLower())
			{
				case "true":
				case "yes":
				case "on":
				case "wahr":
				case "ja":
				case "ein":
				case "enable":
				case "y":
				case "j":
				case "1":
					return true;

				case "false":
				case "no":
				case "off":
				case "falsch":
				case "nein":
				case "aus":
				case "disable":
				case "n":
				case "0":
					return false;
			}

			try
			{
				int i = Convert.ToInt32(boolString);
				return (i != 0);
			}
			catch (InvalidCastException /*ex*/)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Parse object to bool.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>If parsing was successfull <see langword="true"/>; otherwise <paramref name="defaultValue"/>.</returns>
		public static bool ParseBool(object obj, bool defaultValue)
		{
			if (obj == null)
				return defaultValue;

			try
			{
				if (!(obj is string))
				{
					bool ret = Convert.ToBoolean(obj);
					return ret;
				}
			}
			catch (FormatException /*ex*/)
			{
			}

			return ParseBool(obj.ToString(), defaultValue);
		}

		/// <summary>
		/// Parses the byte. Default 0
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The byte value.</returns>
		public static byte ParseByte(string str)
		{
			return ParseByte(str, 0);
		}

		/// <summary>
		/// Parses the byte.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The byte value.</returns>
		public static byte ParseByte(string str, byte defaultValue)
		{
			byte result;
			if (!byte.TryParse(str, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Parse to GUID.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The parsed GUID value, or Guid.Empty if the parsing fails.</returns>
		public static Guid ParseGuid(string str)
		{
			return ParseGuid(str, Guid.Empty);
		}

		/// <summary>
		/// Parse to GUID.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The GUID value.</returns>
		public static Guid ParseGuid(string str, Guid defaultValue)
		{
			if (str.IsNullOrEmptyWithTrim())
				return defaultValue;

			Guid result = defaultValue;

			try
			{
				result = new Guid(str);
			}
			catch (ArgumentException)
			{
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}

			return result;
		}

		/// <summary>
		/// Parse to DateTime.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <returns>The DateTime value, or DateTime.MinValue.</returns>
		public static DateTime ParseDateTime(string str)
		{
			return ParseDateTime(str, DateTime.MinValue);
		}

		/// <summary>
		/// Parse to DateTime.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The DateTime value.</returns>
		public static DateTime ParseDateTime(string str, DateTime defaultValue)
		{
			if (str.IsNullOrEmptyWithTrim())
				return defaultValue;

			DateTime result = defaultValue;

			try
			{
#pragma warning disable 612,618
				result = XmlConvert.ToDateTime(str);
#pragma warning restore 612,618
			}
			catch(Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return result;
		}

		/// <summary>
		/// Parse to DateTime.
		/// </summary>
		/// <param name="str">The string to parse.</param>
		/// <param name="dateTimeOption">The DateTime serialization mode.</param>
		/// <returns>The DateTime value, or DateTime.MinValue is parsing failed.</returns>
		public static DateTime ParseDateTime(string str, XmlDateTimeSerializationMode dateTimeOption)
		{
			if (str.IsNullOrEmptyWithTrim())
				return DateTime.MinValue;

			DateTime result = DateTime.MinValue;

			try
			{
				result = XmlConvert.ToDateTime(str, dateTimeOption);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return result;
		}

		/// <summary>
		/// Parses a URI string.
		/// </summary>
		/// <param name="str">The URI string.</param>
		/// <returns>The parsed <see cref="Uri"/> or <see langword="null"/> if parsing failed.</returns>
		public static Uri ParseUri(string str)
		{
			return ParseUri(str, null);
		}

		/// <summary>
		/// Parses a URI string.
		/// </summary>
		/// <param name="str">The URI string.</param>
		/// <param name="defaultValue">The default <see cref="Uri"/>.</param>
		/// <returns>The parsed <see cref="Uri"/> or the default URI if parsing failed.</returns>
		public static Uri ParseUri(string str, Uri defaultValue)
		{
			Uri result;
			if (!Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		/// <summary>
		/// Tries to parse a URI string.
		/// </summary>
		/// <param name="str">The URI string.</param>
		/// <param name="uri">The parsed <see cref="Uri"/>.</param>
		/// <returns><see langword="true"/> if parsing was successful; otherwise <see langword="false"/>.</returns>
		public static bool TryParseUri(string str, out Uri uri)
		{
			uri = ParseUri(str);

			return uri.IsNotNull();
		}

		/// <summary>
		/// Parses a version string.
		/// </summary>
		/// <param name="str">The version string.</param>
		/// <returns>The parsed <see cref="Version"/> or null if the parsing failed.</returns>
		public static Version ParseVersion(string str)
		{
			return ParseVersion(str, null);
		}

		/// <summary>
		/// Parses a version string.
		/// </summary>
		/// <param name="str">The version string.</param>
		/// <param name="defaultVersion">The default version.</param>
		/// <returns>The parsed <see cref="Version"/> or the default version if the parsing failed.</returns>
		public static Version ParseVersion(string str, Version defaultVersion)
		{
			Version result = defaultVersion;
			try
			{
				result = new Version(str);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			return result;
		}

		/// <summary>
		/// Tries to parse a version string.
		/// </summary>
		/// <param name="str">The version string.</param>
		/// <param name="version">The parsed version.</param>
		/// <returns>true if parsing was successful; otherwise false.</returns>
		public static bool TryParseVersion(string str, out Version version)
		{
			version = ParseVersion(str);
			return version.IsNotNull();
		}

		#endregion

		#region Is...-Methods

		/// <summary>
		/// Determines if the specified string contains a Int16 value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a Int16 value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsInt16String(string str)
		{
			Int16 trash;
			return Int16.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a Int32 value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a Int32 value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsInt32String(string str)
		{
			Int32 trash;
			return Int32.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a Int64 value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a Int64 value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsInt64String(string str)
		{
			Int64 trash;
			return Int64.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a double value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a double value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDoubleString(string str)
		{
			double trash;
			return double.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a decimal value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a decimal value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDecimalString(string str)
		{
			decimal trash;
			return decimal.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a float value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a float value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsFloatString(string str)
		{
			float trash;
			return float.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a bool value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a bool value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsBooleanString(string str)
		{
			bool trash;
			return bool.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a byte value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a byte value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsByteString(string str)
		{
			byte trash;
			return byte.TryParse(str, out trash);
		}

		/// <summary>
		/// Determines if the specified string contains a TimeSpan value.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the string contains a TimeSpan value; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsTimeSpanString(string str)
		{
			TimeSpan trash;
			return DateTimeHelper.TryParseTimeSpan(str, out trash);
		}


		#endregion

	}
}