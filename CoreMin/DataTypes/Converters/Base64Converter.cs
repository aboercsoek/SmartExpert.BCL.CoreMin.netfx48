//--------------------------------------------------------------------------
// File:    Base64Converter.cs
// Content:	Implementation of a base64 string converter class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Text;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// Base64 converter
	/// </summary>
	public static class Base64Converter
	{
		#region Base64-Methods

		/// <summary>
		/// Decode base64 encoded string.
		/// </summary>
		/// <param name="str">The base64 encoded string.</param>
		/// <returns>The decoded string.</returns>
		public static string FromBase64ToString(string str)
		{
			return FromBase64ToString(str, Encoding.UTF8);
		}

		/// <summary>
		/// Decode base64 encoded string.
		/// </summary>
		/// <param name="str">The base64 encoded string.</param>
		/// <param name="encoding">The encoding to use to build the result.</param>
		/// <returns>The decoded string.</returns>
		public static string FromBase64ToString(string str, Encoding encoding)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			ArgChecker.ShouldNotBeNull(encoding, "encoding");

			byte[] bytes = Convert.FromBase64String(str);
			return encoding.GetString(bytes);
		}

		/// <summary>
		/// Decode base64 encoded string.
		/// </summary>
		/// <param name="str">The base64 encoded string.</param>
		/// <returns>The decoded string.</returns>
		public static byte[] FromBase64ToByteArray(string str)
		{
			return string.IsNullOrEmpty(str) ? new byte[0] : Convert.FromBase64String(str);
		}

		/// <summary>
		/// Base64 string encoding.
		/// </summary>
		/// <param name="str">The string to encode.</param>
		/// <returns>
		/// The base64 encoded string.
		/// </returns>
		public static string ToBase64String(string str)
		{
			return ToBase64String(str, Encoding.UTF8);
		}

		/// <summary>
		/// Converts a string into a base64 encoded string.
		/// </summary>
		/// <param name="str">The string to encode.</param>
		/// <param name="encoding">The encoding that should be used to convert the input string into a byte array.</param>
		/// <returns>
		/// The base64 encoded string.
		/// </returns>
		public static string ToBase64String(string str, Encoding encoding)
		{
			if (string.IsNullOrEmpty(str))
				return str;

			ArgChecker.ShouldNotBeNull(encoding, "encoding");

			return Convert.ToBase64String(encoding.GetBytes(str));
		}

		/// <summary>
		/// Converts a byte array into a base64 encoded string.
		/// </summary>
		/// <param name="data">The byte array to convert.</param>
		/// <returns>
		/// The base64 encoded string.
		/// </returns>
		public static string ToBase64String(byte[] data)
		{
			return data.IsNullOrEmpty() ? string.Empty : Convert.ToBase64String(data);
		}

		#endregion
	}
}