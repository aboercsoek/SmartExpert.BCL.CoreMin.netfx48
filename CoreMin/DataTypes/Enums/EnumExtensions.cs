//--------------------------------------------------------------------------
// File:    EnumExtensions.cs
// Content:	Implementation of class EnumExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using System.Reflection;
using System.Text;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Reflection;

#endregion


namespace SmartExpert
{
// ReSharper disable SpecifyACultureInStringConversionExplicitly

	///<summary>Represents extension methods for <see cref="Enum"/> type.</summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets the display name of the enum value.
		/// </summary>
		/// <param name="enumeration">The enumeration.</param>
		/// <returns>The name of the EnumDisplay attribute if present; otherwise the enum ToString result.</returns>
		public static string GetDisplayName(this Enum enumeration)
		{
			Type type = enumeration.GetType();

			if (type.HasAttribute<FlagsAttribute>() == false)
			{
				MemberInfo memInfo = type.GetMember(enumeration.ToString()).FirstOrDefault();

				if (memInfo != null)
				{
					var attr = memInfo.GetAttribute<DisplayNameAttribute>(false);
					if ((attr != null) && (attr.Text.IsNullOrEmptyWithTrim() == false))
						return attr.Text;
				}

				return enumeration.ToString();
			}

			string[] memberStrings = enumeration.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

			var result = new StringBuilder();
			bool firstIteration = true;
			foreach (string member in memberStrings)
			{
				string text = member.Trim();
				MemberInfo memInfo = (type.GetMember(member.Trim())).FirstOrDefault();

				if (memInfo != null)
				{
					var attr = memInfo.GetAttribute<DisplayNameAttribute>(false);
					if ((attr != null) && (attr.Text.IsNullOrEmptyWithTrim() == false))
						text = attr.DisplayName;
				}

				if (firstIteration)
				{
					result.Append(text);
					firstIteration = false;
				}
				else
				{
					result.Append(", " + text);
				}

			}

			return result.ToString();
		}

		/// <summary>
		/// Gets the display name of the enum value.
		/// </summary>
		/// <param name="enumeration">The enumeration.</param>
		/// <returns>The name of the EnumDisplay attribute if present; otherwise the enum ToString result.</returns>
		public static string GetDisplayNameKey(this Enum enumeration)
		{
			Type type = enumeration.GetType();

			if (type.HasAttribute<FlagsAttribute>() == false)
			{
				MemberInfo memInfo = type.GetMember(enumeration.ToString()).FirstOrDefault();

				if (memInfo != null)
				{
					var attr = memInfo.GetAttribute<DisplayNameAttribute>(false);
					if ((attr != null) && (attr.DisplayNameKey.IsNullOrEmptyWithTrim() == false))
						return attr.DisplayNameKey;
				}

				return enumeration.ToString();
			}

			string[] memberStrings = enumeration.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

			var result = new StringBuilder();
			bool firstIteration = true;
			foreach (string member in memberStrings)
			{
				string text = member.Trim();
				MemberInfo memInfo = (type.GetMember(member.Trim())).FirstOrDefault();

				if (memInfo != null)
				{
					var attr = memInfo.GetAttribute<DisplayNameAttribute>(false);
					if ((attr != null) && (attr.DisplayNameKey.IsNullOrEmptyWithTrim() == false))
						text = attr.DisplayNameKey;
				}

				if (firstIteration)
				{
					result.Append(text);
					firstIteration = false;
				}
				else
				{
					result.Append(", " + text);
				}

			}

			return result.ToString();
		}

		/// <summary>
		/// Parse a specific enumeration string.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to parse.</param>
		/// <returns>The enumeration object, or null if parsing failed.</returns>
		public static TEnum? EnumParse<TEnum>(this string value) where TEnum : struct
		{
			return EnumHelper.Parse<TEnum>(value, false);
		}

		/// <summary>
		/// Parse a specific enumeration string.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to parse.</param>
		/// <param name="ignoreCase">if set to <see langword="true"/> ignore case while parsing.</param>
		/// <returns>The enumeration object, or null if parsing failed.</returns>
		public static TEnum? EnumParse<TEnum>(this string value, bool ignoreCase) where TEnum : struct
		{
			//ArgChecker.ShouldNotBeNull(value, "value");
			if (value == null)
				return null;

			value = value.Trim();
			//ArgChecker.ShouldNotBeNullOrEmpty(value, "value");
			if (value == string.Empty)
				return null;

			Type t = typeof(TEnum);

			if (!t.IsEnum)
			{
				throw new ArgException<string>(t.ToString(), "TEnum", "Type provided must be an Enum {0} is of type {1}.");
			}

			TEnum? enumType = EnumHelper.Parse<TEnum>(value, ignoreCase);

			return enumType;
		}

		/// <summary>
		/// Parse a specific display name enumeration string.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enum.</typeparam>
		/// <param name="value">The value to parse.</param>
		/// <param name="ignoreCase">if set to <see langword="true"/> ignore case while parsing.</param>
		/// <returns>The enumeration object, or null if parsing failed.</returns>
		public static TEnum? EnumParseDisplayName<TEnum>(this string value, bool ignoreCase) where TEnum : struct
		{
			//ArgChecker.ShouldNotBeNull(value, "value");
			if (value == null)
				return null;

			value = value.Trim();
			//ArgChecker.ShouldNotBeNullOrEmpty(value, "value");
			if (value == string.Empty)
				return null;

			Type t = typeof(TEnum);

			if (!t.IsEnum)
			{
				throw new ArgException<string>(t.ToString(), "TEnum", "Type provided must be an Enum {0} is of type {1}.");
			}

			string[] names = Enum.GetNames(typeof(TEnum));

			foreach (string name in names)
			{
				//MemberInfo[] memInfo = t.GetMember(name);
				//if (memInfo.Length <= 0) continue;

				//object[] attrs = memInfo[0].GetCustomAttributes(typeof(DisplayNameAttribute), false);
				//if (attrs.Length <= 0) continue;

				MemberInfo memInfo = (t.GetMember(name)).FirstOrDefault();
				if (memInfo == null) continue;

				var attr = memInfo.GetAttribute<DisplayNameAttribute>(false);
				if ((attr == null) || attr.DisplayName.IsNullOrEmptyWithTrim())
					continue;

				string displayName = attr.DisplayName.Trim();

				if (value.Contains(displayName) && (string.Equals(displayName, name, StringComparison.CurrentCultureIgnoreCase) == false))
					value = value.Replace(displayName, name);
			}

			TEnum? enumType = EnumHelper.Parse<TEnum>(value, ignoreCase);

			return enumType;
		}
	}

// ReSharper restore SpecifyACultureInStringConversionExplicitly

}
