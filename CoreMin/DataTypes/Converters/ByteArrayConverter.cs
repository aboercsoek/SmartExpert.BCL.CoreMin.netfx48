//--------------------------------------------------------------------------
// File:    ByteArrayConverter.cs
// Content:	Implementation of class ByteArrayConverter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;

#endregion

namespace SmartExpert
{
	///<summary>Byte Array convertion extension methods.</summary>
	public static class ByteArrayConverter
	{
		/// <summary>
		/// Gets the string from bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>The decoding string result.</returns>
		public static string ToUnicodeString(this byte[] data)
		{
			return StringHelper.GetStringFromBytes(data);
		}

		/// <summary>
		/// Convert byte array to hex string
		/// </summary>
		/// <param name="data">The byte data array to convert.</param>
		/// <returns>The hex string.</returns>
		public static string ToHexString(this byte[] data)
		{
			return HexConverter.ToHexString(data);
		}

		/// <summary>
		/// Converts a byte array into a base64 encoded string.
		/// </summary>
		/// <param name="data">The byte array to convert.</param>
		/// <returns>
		/// The base64 encoded string.
		/// </returns>
		public static string ToBase64String(this byte[] data)
		{
			return Base64Converter.ToBase64String(data);
		}
	}
}
