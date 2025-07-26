//--------------------------------------------------------------------------
// File:    StringConverter.cs
// Content:	Implementation of a string converter class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using SmartExpert.Error;
using SmartExpert.Security.Crypto;
using System.Xml.Linq;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// Common conversion tasks such as parsing string values into various types.
	/// </summary>
	public static class StringConverter
	{
		#region Special To...-Methods

		/// <summary>
		/// Computes the MD5 hash for the given string value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The MD5 hash string.</returns>
		[DebuggerStepThrough]
		public static string ToMd5Hash(this string value)
		{
			return value.IsNotEmpty() ? CryptographyHelper.ComputeHashString(HashAlgorithmType.Md5, value) : string.Empty;
		}

		/// <summary>
		/// Convert string to byte array.
		/// </summary>
		/// <param name="value">The input string.</param>
		/// <returns>The characters of the string as a sequence of bytes.</returns>
		[DebuggerStepThrough]
		public static byte[] ToByteArray(this string value)
		{
			return StringHelper.GetBytesFromString(value);
		}

		/// <summary>
		/// Convert string to instance of type <see cref="System.IO.FileInfo"/>.
		/// </summary>
		/// <param name="filePath">The file path string.</param>
		/// <returns>
		/// An instance of target type <see cref="System.IO.FileInfo"/>, or <see langword="null"/> if <paramref name="filePath"/> is not a file path to an existing file.
		/// </returns>
		[DebuggerStepThrough]
		public static FileInfo ToFileInfo(this string filePath)
		{
			if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
				return null;

			return new FileInfo(filePath);
		}

		#endregion

		#region ToInvariantString-Methods

		/// <summary>
		/// Converts the value into a string by using a TypeConverter for type of val, if available.
		/// </summary>
		/// <typeparam name="T">The source type.</typeparam>
		/// <param name="value">Value to convert into an string</param>
		/// <param name="nullString">null value string result.</param>
		/// <returns>Converted object string.</returns>
		[DebuggerStepThrough]
		public static string ToInvariantString<T>(this T value, string nullString)
		{
			if (value.IsDefaultValue())
			{
				if (value.As<object>() == null)
					return nullString;
			}

			if (typeof(T) == TypeOf.String)
			{
				return value.As<string>();
			}
			if (typeof(T) == TypeOf.Type)
			{
				return value.As<Type>().GetTypeName();
			}
			try
			{
				TypeConverter converter = TypeDescriptor.GetConverter(value);

				string result = converter.ConvertToInvariantString(value);

				return result ?? value.ToString();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return nullString;
		}

		/// <summary>
		/// Converts the value of Type T into a string using a TypeConverter for Type T if available.
		/// </summary>
		/// <typeparam name="T">The source type.</typeparam>
		/// <param name="value">Value to convert into an string</param>
		/// <returns>Converted object string.</returns>
		[DebuggerStepThrough]
		public static string ToInvariantString<T>(this T value)
		{
			if (value.IsDefaultValue())
			{
				if (value.Is<ValueType>() == false) //if (value.As<object>() == null)
					return string.Empty;
			}

			var convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(CultureInfo.InvariantCulture);
			}

			if (value.GetType() == TypeOf.String)
				return value.ToString();

			if (typeof (T) == TypeOf.Type)
				return "typeof(" + value.As<Type>().GetTypeName() + ")";

			if (typeof (T) == typeof (XElement))
				return value.As<XElement>().ToString(SaveOptions.None);

			if (typeof (T) == typeof (XDocument))
				return value.As<XDocument>().ToString(SaveOptions.None);

			try
			{
				TypeConverter converter = TypeDescriptor.GetConverter(value);
				
				string result = converter.ConvertToInvariantString(value);

				return result ?? value.ToString();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return value.ToString();
		}

		#endregion

		#region FromInvariantString-Methods

		/// <summary>
		/// Convert string to instance of type T, using the invariante culture.
		/// </summary>
		/// <typeparam name="T">The target type.</typeparam>
		/// <param name="value">The source value to convert.</param>
		/// <returns>The conversion result of Type T.</returns>
		[DebuggerStepThrough]
		public static T FromInvariantString<T>(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return default(T);
			
			return typeof(T) == TypeOf.String ? value.As<T>() : FromInvariantString(value, default(T));
		}

		/// <summary>
		/// Convert string to instance of type T, using the invariante culture.
		/// </summary>
		/// <typeparam name="T">The target type.</typeparam>
		/// <param name="value">The source value to convert.</param>
		/// <param name="defaultValue">The default value to return if value is null or empty or conversion is not possible.</param>
		/// <returns>The conversion result of Type T.</returns>
		[DebuggerStepThrough]
		public static T FromInvariantString<T>(this string value, T defaultValue)
		{
			if (string.IsNullOrEmpty(value))
				return defaultValue;

			if (typeof(T) == TypeOf.String)
				return value.As<T>();

			try
			{
				if (typeof(T).IsEnum)
				{
					return FromStringToEnumHelper<T>(typeof(T), value);
				}
				if (typeof(T) == typeof(XElement))
				{
					return (T)(object)XElement.Parse(value, LoadOptions.None);
				}
				if (typeof(T) == typeof(XDocument))
				{
					return (T)(object)XDocument.Parse(value, LoadOptions.None);
				}

				TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
				return converter.ConvertFromInvariantString(value).As<T>();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return defaultValue;
		}

		internal static T FromStringToEnum<T>(string value) where T : struct
		{
			T? result = EnumHelper.Parse<T>(value, true);
			
			return result.HasValue ? result.Value : default(T);
		}

		private static T FromStringToEnumHelper<T>(Type enumType, string value)
		{
			if (m_StringToEnumMethod == null)
			{
				m_StringToEnumMethod = typeof(StringConverter).GetMethod("FromStringToEnum", BindingFlags.NonPublic | BindingFlags.Static);
			}

			return (T)m_StringToEnumMethod.MakeGenericMethod(new Type[] { enumType }).Invoke(null, new object[] { value });
		}

		private static MethodInfo m_StringToEnumMethod;


		/// <summary>
		/// Convert string to instance of <paramref name="type"/>, using the invariante culture.
		/// </summary>
		/// <param name="value">The string value to convert.</param>
		/// <param name="type">The target type.</param>
		/// <returns>The converted object of Type <paramref name="type"/>.</returns>
		[DebuggerStepThrough]
		internal static object FromInvariantString(this string value, Type type)
		{
			if (value.IsNullOrEmpty())
				return null;

			if (type.IsNull())
				return null;

			if (type == TypeOf.String)
				return value;

			try
			{
				if (type == typeof(XElement))
				{
					return XElement.Parse(value, LoadOptions.None);
				}
				if (type == typeof(XDocument))
				{
					return XDocument.Parse(value, LoadOptions.None);
				}

				TypeConverter converter = TypeDescriptor.GetConverter(type);
				return converter.ConvertFromInvariantString(value);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;	
			}

			return null;
		}


		#endregion
	}
}