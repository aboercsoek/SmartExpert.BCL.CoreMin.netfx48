//--------------------------------------------------------------------------
// File:    StringHelper.cs
// Content:	Implementation of String helper class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SmartExpert.Error;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// String manipulation and generation methods, as well as string array manipulation.
	/// </summary>
	public static class StringHelper
	{
		#region Private and Public Static Members

		/// <summary>
		/// Char array with default quote char (").
		/// </summary>
		public static readonly char[] DefaultQuoteSensitiveChars = new[] { '\"' };

		private static readonly Random Random;
		
		#endregion

		#region Static Ctor

		/// <summary>
		/// Static ctor
		/// </summary>
		static StringHelper()
		{
			Random = new Random(unchecked((int)DateTime.UtcNow.Ticks));
		}

		#endregion

		#region Safe ToString methods

		/// <summary>
		/// Safe ToString-Operation.
		/// </summary>
		/// <param name="obj">The object to convert to safe string.</param>
		/// <returns>If <paramref name="obj"/> is null String.Empty; otherwise obj.ToInvariantString(String.Empty).</returns>
		[DebuggerStepThrough]
		internal static string SafeToString(object obj)
		{
			try
			{
				return (obj == null) ? String.Empty : obj.ToInvariantString(String.Empty);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			return String.Empty;
		}

		/// <summary>
		/// Safe ToString-Operation.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>If <paramref name="obj"/> is <see langword="null"/> the safe ToString value of <paramref name="defaultValue"/>; otherwise the value of obj.ToString().</returns>
		[DebuggerStepThrough]
		internal static string SafeToString(object obj, string defaultValue)
		{
			try
			{
				return (obj == null) ? SafeToString(defaultValue) : obj.ToInvariantString(defaultValue);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return SafeToString(defaultValue);
		}

		#endregion

		#region Safe string formating methods

		/// <summary>
		/// Formats a name value pair.
		/// </summary>
		/// <typeparam name="T">Type of the value.</typeparam>
		/// <param name="name">The name</param>
		/// <param name="value">The value</param>
		/// <returns>The formatted name value pair string.</returns>
		/// <remarks>
		/// <para>Format: "{name} ({value type}) = {value}"</para>
		/// </remarks>
		public static string FormatNameValue<T>(string name, T value)
		{
			if (value.Is<string>())
			{
				return "{0} ({1}) = {2}".SafeFormatWith(name.SafeString(), "string", value.As<string>().SafeString("<null>"));
			}

			if (value.As<IEnumerable>().IsNotNull())
			{
				return FormatCollection(name, value.As<IEnumerable>(), 10);
			}

			return "{0} ({1}) = {2}".SafeFormatWith(name.SafeString(), typeof(T).GetTypeName(), value.ToInvariantString("<null>"));
		}

		/// <summary>
		/// Formats a collection.
		/// </summary>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="collection">The collection.</param>
		/// <param name="maxItems">The max items.</param>
		/// <returns>The formatted collection string.</returns>
		/// <remarks>
		/// <para>Format: "{collectionName} ({collection type}) = { item1, item2, item3, ... }"</para>
		/// <para>Format: "{collectionName} ({collection type}) = { ... }"</para>
		/// <para>Format: "{collectionName} ({collection type}) = { &lt;empty&gt; }"</para>
		/// <para>Format: "{collectionName} = &lt;null&gt;"</para>
		/// </remarks>
		public static string FormatCollection(string collectionName, IEnumerable collection, int maxItems)
		{
			var sb = new StringBuilder();

			if (collection == null)
			{
				return string.IsNullOrEmpty(collectionName) ? "Collection = <null>" : "{0} = <null>".SafeFormatWith(collectionName);
			}

			sb.Append(string.IsNullOrEmpty(collectionName)
						? "Collection {0} Items".SafeFormatWith(collection.GetType().GetTypeName())
						: "{0} ({1})".SafeFormatWith(collectionName, collection.GetType().GetTypeName()));

			sb.Append(" = ");

			if (maxItems < 0)
				maxItems = 0;

			int count = 0;
			bool firstLoop = true;
			const string separator = ", ";

			sb.Append("{ ");
			foreach (var o in collection)
			{
				count++;

				if (firstLoop)
				{
					firstLoop = false;

					if (maxItems == 0)
					{
						sb.Append("...");
						break;
					}
				}
				else
				{
					if (count > maxItems)
					{
						sb.Append(", ...");
						break;
					}

					sb.Append(separator);
				}

				sb.Append(o.ToInvariantString("<null>"));

			}
			if (firstLoop)
				sb.Append("<empty>");

			sb.Append(" }");

			return sb.ToString();

		}

		/// <summary>
		/// Formats the specified format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		[StringFormatMethod("format")]
		internal static string SafeFormat(string format, params object[] args)
		{
			if (format == null)
				return String.Empty;

			if (args.IsNullOrEmpty())
				return format;

			try
			{
// ReSharper disable CoVariantArrayConversion
				return String.Format(CultureInfo.InvariantCulture, format, args.Select(arg => arg.ToInvariantString("<null>")).ToArray());
// ReSharper restore CoVariantArrayConversion
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;

				var sb = new StringBuilder();
				StringBuilderHelper.FormatFallback(ex, sb, format, args);
				return sb.ToString();
			}
		}



		#endregion

		#region String List to Multiline String

		/// <summary>
		/// Converts a string list into a multi line string. Puts \n between the lines
		/// </summary>
		/// <param name="strList">The string list</param>
		/// <returns>The multi line string</returns>
		[DebuggerStepThrough]
		public static string StringList2MultiLine(ICollection<string> strList)
		{
			if (strList.IsNullOrEmpty())
				return string.Empty;

			if (strList.Count == 1)
				return strList.First().SafeString();

			var sb = new StringBuilder();
			int maxCount = strList.Count;
			int currentCount = 1;
			foreach (string item in strList)
			{
				if (currentCount < maxCount)
					sb.AppendLine(item.SafeString());
				else
					sb.Append(item.SafeString());
				currentCount++;
			}
			return sb.ToString();
		}

		/// <summary>
		/// Converts a string array into a multi line string. Puts \n between the lines
		/// </summary>
		/// <param name="strArray">The string array</param>
		/// <returns>The multi line string</returns>
		[DebuggerStepThrough]
		public static string StringArray2MultiLine(string[] strArray)
		{
			if (strArray.IsNullOrEmpty())
				return string.Empty;
			if (strArray.Length == 1)
				return strArray[0].SafeString();

			var sb = new StringBuilder();
			int maxCount = strArray.Length;
			int currentCount = 1;
			foreach (string item in strArray)
			{
				if (currentCount < maxCount)
					sb.AppendLine(item.SafeString());
				else
					sb.Append(item.SafeString());
				currentCount++;
			}
			return sb.ToString();
		}

		#endregion

		#region Join & Split Methods

		/// <summary>
		/// Concatenates a specified separator System.String between each element of
		/// <paramref name="collection"/>, yielding a single concatenated string.
		/// </summary>
		/// <param name="collection">Collection of strings.</param>
		/// <param name="separator">A System.String.</param>
		/// <returns>A System.String consisting of the elements of value interspersed with the separator string.</returns>
		public static string Join(this IEnumerable<string> collection, string separator)
		{
			#region PreConditions

			ArgChecker.ShouldNotBeNull(collection, "collection");

			#endregion

			return string.Join(separator ?? string.Empty, collection.ToArray());
		}

		/// <summary>
		/// Joins the variable char-array chars to a string. 
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <param name="chars">The chars.</param>
		/// <returns></returns>
		public static string Join(string separator, params char[] chars)
		{
			string result = null;

			if (chars != null)
			{
				int l = chars.Length;
				for (int i = 0; i < l; i++)
				{
					if (i > 0)
					{
						result += separator;
					}
					result += chars[i];
				}
			}

			return result;
		}


		/// <summary>
		/// Joins the specified items using the default appender.
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		public static string Join<T>(string separator, params T[] items)
		{
			return Join(separator, items, (sb, item) => sb.Append<T>(item));
		}

		/// <summary>
		/// Joins array of Type T values using the specified separator.
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <param name="items">The items.</param>
		/// <param name="appender">The appender (Convert type T object to string an append the result to a given StringBuilder).</param>
		/// <returns>The joined items.</returns>
		public static string Join<T>(string separator, T[] items, Action<StringBuilder, T> appender)
		{
			ArgChecker.ShouldNotBeNull(items, "items");

			if (items.IsNullOrEmpty())
			{
				return string.Empty;
			}
			if (separator == null)
			{
				separator = string.Empty;
			}

			var builder = new StringBuilder(items.Length * (separator.Length + 10));
			bool flag = true;
			foreach (T local in items)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					builder.Append(separator);
				}
				appender(builder, local);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Joins the specified items using a custom
		/// appender.
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <param name="items">The items.</param>
		/// <param name="appender">The appender.</param>
		/// <returns></returns>
		public static string Join<T>(string separator, ICollection<T> items, Action<StringBuilder, T> appender)
		{
			ArgChecker.ShouldNotBeNull(items, "items");

			if (items.IsNullOrEmpty())
			{
				return string.Empty;
			}
			if (separator == null)
			{
				separator = string.Empty;
			}
			var builder = new StringBuilder(items.Count * (separator.Length + 10));
			bool flag = true;
			foreach (T local in items)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					builder.Append(separator);
				}
				appender(builder, local);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Joins sequence of Type T items using the specified separator.
		/// </summary>
		/// <param name="separator">The separator.</param>
		/// <param name="items">The items.</param>
		/// <param name="appender">The appender (Convert type T object to string an append the result to a given StringBuilder).</param>
		/// <returns>The joined items.</returns>
		public static string Join<T>(string separator, IEnumerable<T> items, Action<StringBuilder, T> appender)
		{
			ArgChecker.ShouldNotBeNull(items, "items");

			if (separator == null)
			{
				separator = string.Empty;
			}

			var builder = new StringBuilder();
			bool firstIteration = true;
			foreach (T local in items)
			{
				if (firstIteration)
				{
					firstIteration = false;
				}
				else
				{
					builder.Append(separator);
				}

				appender(builder, local);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Splits <paramref name="str"/> based on the index. The first element
		/// is the left portion, and the second element
		/// is the right portion. The character at index <paramref name="index"/>
		/// is either included at the end of the left portion, or at the
		/// beginning of the right portion, depending on <paramref name="isIndexInFirstPortion"/>
		/// The return result is never null, and the elements
		/// are never null, so one of the elements may be an empty string.
		/// </summary>
		/// <param name="str">The string to split.</param>
		/// <param name="index">The index where the should be splitted.</param>
		/// <param name="isIndexInFirstPortion">if set to <see langword="true"/> [is index in first portion].</param>
		/// <returns>The Split-Operation string-Array result.</returns>
		public static string[] SplitOn(string str, int index, bool isIndexInFirstPortion)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(str, "str");
			string one, two;
			if (index == -1)
			{
				one = str;
				two = "";
			}
			else
			{
				if (index == 0)
				{
					if (isIndexInFirstPortion)
					{
						one = str[0].ToString(CultureInfo.InvariantCulture);
						two = str.Substring(1);
					}
					else
					{
						one = "";
						two = str;
					}
				}
				else if (index == str.Length - 1)
				{
					if (isIndexInFirstPortion)
					{
						one = str;
						two = "";
					}
					else
					{
						one = str.Substring(0, str.Length - 1);
						two = str[str.Length - 1].ToString(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					one = str.Substring(0, isIndexInFirstPortion ? index + 1 : index);
					two = str.Substring(isIndexInFirstPortion ? index + 1 : index);
				}
			}

			return new[] { one, two };
		}

		/// <summary>
		/// Splits a string in order to create a square of lines
		/// </summary>
		/// <param name="value"></param>
		/// <param name="separators"></param>
		/// <returns></returns>
		public static string[] SquareChunk(string value, params char[] separators)
		{
			if (value.SafeLength() <= 0)
			{
				return new[] { string.Empty };
			}

			var list = new List<int>();
			var num2 = (int)Math.Sqrt(value.Length);
			int num3 = 0;
			
			for (int i = 0; i < value.Length; i++)
			{
				char ch = value[i];
				if (separators.Contains(ch).IsFalse()) continue;

				num2 = Math.Max(num2, i - num3);
				num3 = i;
				list.Add(num3);
			}

			num2 = Math.Max(num2, 1);
			list.Add(value.Length);

			var list2 = new List<string>();
			int startIndex = 0;
			for (int j = 0; j < list.Count; j++)
			{
				int num7 = list[j];
				int length = num7 - startIndex;
				
				if (length < num2) continue;

				list2.Add(value.Substring(startIndex, length));
				startIndex = num7;
			}
			if (startIndex < value.Length)
			{
				list2.Add(value.Substring(startIndex));
			}
			return list2.ToArray();
		}


		#endregion

		#region Byte-Array convertion to and from string

		/// <summary>
		/// Gets the bytes from string.
		/// </summary>
		/// <param name="str">The STR.</param>
		/// <returns>The characters of the string as a sequence of bytes.</returns>
		public static byte[] GetBytesFromString(string str)
		{
			return string.IsNullOrEmpty(str) ? new byte[0] : Encoding.Unicode.GetBytes(str);
		}

		/// <summary>
		/// Gets the string from bytes.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>The decoding string result.</returns>
		public static string GetStringFromBytes(byte[] data)
		{
			// Strings in .NET are always UTF16
			return Encoding.Unicode.GetString(data);
		}

		#endregion

		#region Padding Methods

		/// <summary>
		/// Returns a string of length <paramref name="length"/> with 0's padded to the left, if necessary.
		/// </summary>
		/// <param name="val">The padding value.</param>
		/// <param name="length">The padding length.</param>
		/// <returns>Returns a string of length <paramref name="length"/> with 0's padded to the left, if necessary.</returns>
		public static string PadIntegerLeft(int val, int length)
		{
			return PadIntegerLeft(val, length, '0');
		}

		/// <summary>
		/// Pads the integer left.
		/// </summary>
		/// <param name="val">The padding value.</param>
		/// <param name="length">The padding length.</param>
		/// <param name="pad">The padding char.</param>
		/// <returns>The the padding left string result.</returns>
		public static string PadIntegerLeft(int val, int length, char pad)
		{
			string result = val.ToString(CultureInfo.InvariantCulture);
			while (result.Length < length)
			{
				result = pad + result;
			}
			return result;
		}

		/// <summary>
		/// Returns a string of length <paramref name="length"/> with
		/// 0's padded to the right, if necessary.
		/// </summary>
		/// <param name="val">The padding value.</param>
		/// <param name="length">The padding length.</param>
		/// <returns>The the padding right string result.</returns>
		public static string PadIntegerRight(int val, int length)
		{
			return PadIntegerRight(val, length, '0');
		}

		/// <summary>
		/// Pads the integer right.
		/// </summary>
		/// <param name="val">The value to pad.</param>
		/// <param name="length">The padding length.</param>
		/// <param name="pad">The padding char.</param>
		/// <returns>The the padding right string result.</returns>
		public static string PadIntegerRight(int val, int length, char pad)
		{
			string result = val.ToString(CultureInfo.InvariantCulture);
			while (result.Length < length)
			{
				result += pad;
			}
			return result;
		}

		/// <summary>
		/// Formats the <paramref name="value" /> to a string, adding leading zeros so that all of the numbers up to <paramref name="maxvalue" />, inclusively, had the same number of characters in their string representation when formatted thru this function.
		/// </summary>
		public static string ToStringWithLeading(int value, int maxvalue, CultureInfo culture)
		{
			if (value >= maxvalue)
			{
				return value.ToString(culture);
			}
			return value.ToString(string.Format("D{0}", ((int)Math.Floor(Math.Log(maxvalue, 10.0))) + 1), culture);
		}


		#endregion

		#region Remove Chars Methods

		/// <summary>
		/// Removes all characters passed in from the string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="chars"></param>
		/// <returns></returns>
		public static string RemoveCharacters(string str, params char[] chars)
		{
			if (chars != null)
			{
				str = Regex.Replace(str, "[" + new string(chars) + "]+", "");
			}
			return str;
		}

		/// <summary>
		/// Remove all characters that are not in the passed in array from the string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="chars"></param>
		/// <returns></returns>
		public static string RemoveCharactersInverse(string str, params char[] chars)
		{
			if (chars != null)
			{
				str = Regex.Replace(str, "[^" + new string(chars) + "]+", "");
			}
			return str;
		}

		#endregion

		#region Special String helper methods (Random, CRC32, ...)

		/// <summary>
		/// Returns a string of length <paramref name="size"/> filled
		/// with random ASCII characters in the range A-Z, a-z. If <paramref name="lowerCase"/>
		/// is <see langword="true"/>, then the range is only a-z.
		/// <note>If size is lower or equal 0 size will be set to 1, and if size is greater 4096 size will be set to 4096.</note>
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="lowerCase">if set to <see langword="true"/> [lower case].</param>
		/// <returns>The generated random string.</returns>
		public static string RandomString(int size, bool lowerCase)
		{
			if (size <= 0)
				size = 1;
			if (size > 4096)
				size = 4096;
			
			var builder = new StringBuilder(size);
			int low = 65; // 'A'
			int high = 91; // 'Z' + 1
			if (lowerCase)
			{
				low = 97; // 'a';
				high = 123; // 'z' + 1
			}
			for (int i = 0; i < size; i++)
			{
				char ch = Convert.ToChar(Random.Next(low, high));
				builder.Append(ch);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Calculates the CRC32.
		/// </summary>
		/// <param name="str">The STR.</param>
		/// <returns>CRC32 value of the string.</returns>
		public static uint CalculateCrc32(string str)
		{
			return Crc32Helper.Compute(str);
		}

		/// <summary>
		/// Replaces NewLine character with HTML br element.
		/// </summary>
		/// <param name="text">String to convert.</param>
		/// <returns>Converted string</returns>
		public static string ReplaceNewLineWithHtmlBr(string text)
		{
			string result = text;

			if (!string.IsNullOrEmpty(text))
			{
				result = text.Replace("\r\n", "<br />").Replace("\n", "<br />");
			}
			return result;
		}


		#endregion

	}
}