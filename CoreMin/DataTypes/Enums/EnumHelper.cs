//--------------------------------------------------------------------------
// File:    EnumHelper.cs
// Content:	Implementation of static class EnumHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2007 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// ReSharper disable CheckNamespace
namespace SmartExpert
{
// ReSharper restore CheckNamespace

	/// <summary>
	/// Enumeration Helper class
	/// </summary>
	public static class EnumHelper
	{
		/// <summary>
		/// Parses a specified enumeration string.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="str">The enumeration string.</param>
		/// <returns>Nullable enumeration value</returns>
		public static TEnum? Parse<TEnum>(string str) where TEnum : struct
		{
			return Parse<TEnum>(str, false);
		}

		/// <summary>
		/// Parses a specified enumeration string.
		/// </summary>
		/// <typeparam name="TEnum">The enum type.</typeparam>
		/// <param name="str">The enumeration string.</param>
		/// <param name="ignoreCase">if set to <see langword="true"/> [ignore case].</param>
		/// <returns>Nullable enumeration value</returns>
		public static TEnum? Parse<TEnum>(string str, bool ignoreCase) where TEnum : struct
		{
			try
			{
				var value = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);

				return value;
				//if (IsDefined(value))
				//{
				//	return value;
				//}
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}

			return null;
		}

		/// <summary>
		/// Determines whether the specified value is defined inside a enumeration.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type.</typeparam>
		/// <param name="value">The value to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified value is defined; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct
		{
			return Enum.IsDefined(typeof(TEnum), value);
		}

		/// <summary>
		/// Determines whether the specified value is defined inside a enumeration.
		/// </summary>
		/// <typeparam name="TEnum">The enumeration type.</typeparam>
		/// <param name="value">The value to check.</param>
		/// <param name="argumentName">Name of the argument.</param>
		/// <exception cref="System.ComponentModel.InvalidEnumArgumentException">Is thrown if the enumeration value is not defined.</exception>
		public static void VerifyIsDefined<TEnum>(TEnum value, string argumentName) where TEnum : struct
		{
			if (!IsDefined(value))
			{
				throw new InvalidEnumArgumentException(argumentName, Convert.ToInt32(value), typeof(TEnum));
			}
		}

		/// <summary>
		/// Gets all of the alternative names for the value of an enum.
		/// The value is not required to be coerced to the enum type, it must be of any integral type.
		/// </summary>
		/// <param name="type">Enum type.</param>
		/// <param name="value">Enum value to be named, must be of an integral type.</param>
		/// <returns>Returns the enum names that correspond to the given value.</returns>
		public static string[] GetNamesOfValue(Type type, object value)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!type.IsEnum)
			{
				throw new InvalidOperationException("The value must be of an enum type.");
			}

			return value.ToString().SafeString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			//return Enum.GetName(type, value).SafeString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// Unboxes an object of an integral type (intXXX, bool, byte, char, enum, etc) as a QWORD integer value.
		/// </summary>
		/// <returns>The unboxed numeric value.</returns>
// ReSharper disable UnusedMember.Local
		private static ulong UnboxQWord(object o)
// ReSharper restore UnusedMember.Local
		{
			switch (Convert.GetTypeCode(o))
			{
				case TypeCode.Empty:
				case TypeCode.Object:
				case TypeCode.DBNull:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
				case TypeCode.DateTime:
				case TypeCode.String:

					throw new InvalidOperationException("This type cannot be unboxed as a QWORD.");

				case TypeCode.Boolean:
					return Convert.ToUInt64(o);

				case TypeCode.Char:
					return (ulong)Convert.ToInt32(o);

				case TypeCode.SByte:
					return (byte)Convert.ToSByte(o);

				case TypeCode.Byte:
					return Convert.ToUInt64(o);

				case TypeCode.Int16:
					return (ushort)Convert.ToInt16(o);

				case TypeCode.UInt16:
					return Convert.ToUInt64(o);

				case TypeCode.Int32:
					return (ulong)Convert.ToInt32(o);

				case TypeCode.UInt32:
					return Convert.ToUInt64(o);

				case TypeCode.Int64:
					return (ulong)Convert.ToInt64(o);

				case TypeCode.UInt64:
					return Convert.ToUInt64(o);
			}
			throw new ArgumentOutOfRangeException();
		}


	}
}