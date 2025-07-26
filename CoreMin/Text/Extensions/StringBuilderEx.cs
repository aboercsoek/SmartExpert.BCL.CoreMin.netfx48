//--------------------------------------------------------------------------
// File:    StringBuilderEx.cs
// Content:	Implementation of class StringBuilderEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using SmartExpert.Error;

#endregion

namespace SmartExpert
{
	///<summary>Contains extension methods for <see cref="StringBuilder"/> type.</summary>
	public static class StringBuilderEx
	{
		/// <summary>
		/// Clears the content of the specified string builder.
		/// </summary>
		/// <param name="stringBuilder">The string builder.</param>
		/// <returns>The string builder.</returns>
		public static StringBuilder Clear(this StringBuilder stringBuilder)
		{
			if (stringBuilder != null)
				stringBuilder.Length = 0;

			return stringBuilder;
		}

		/// <summary>
		/// Determines whether the specified string builder is null or empty.
		/// </summary>
		/// <param name="builder">The string builder.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified builder is null or empty; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNullOrEmpty(this StringBuilder builder)
		{
			return (builder == null) || (builder.Length == 0);
		}

		/// <summary>
		/// Determines whether the specified string builder is NOT null or empty.
		/// </summary>
		/// <param name="builder">The string builder.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified builder is NOT null or empty; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNotEmpty(this StringBuilder builder)
		{
			return builder.IsNullOrEmpty().IsFalse();
		}


		/// <summary>
		/// Appends the specified value to the string builder.
		/// </summary>
		/// <typeparam name="T">Type of the value that should be added.</typeparam>
		/// <param name="builder">The string builder where the value should be added.</param>
		/// <param name="value">The value to add.</param>
		/// <returns>The string builder instance where <paramref name="value"/> was added.</returns>
		public static StringBuilder Append<T>(this StringBuilder builder, T value)
		{
			if (builder == null)
				builder = new StringBuilder();

			return value.As<object>() == null ? builder : builder.Append(value.ToInvariantString());
		}

		/// <summary>
		/// Appends the formated string to a string builder.
		/// </summary>
		/// <param name="builder">The string builder.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format string arguments.</param>
		/// <returns>The string builder.</returns>
		[DebuggerStepThrough]
		[StringFormatMethod("format")]
		public static StringBuilder SafeAppendFormat(this StringBuilder builder, string format, params object[] args)
		{
			if (builder == null)
				builder = new StringBuilder();

			if (String.IsNullOrEmpty(format))
				return builder;

			if (args.IsNullOrEmpty())
				return builder.Append(format); 

			try
			{
				builder.Append(format.SafeFormatWith(args));
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;

				StringBuilderHelper.FormatFallback(ex, builder, format, args);
			}

			return builder;
		}

		/// <summary>
		/// Insert the specified text at the beginning of the string builder.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="text">The text to insert.</param>
		/// <returns>The string builder reference.</returns>
		public static StringBuilder Prepend(this StringBuilder builder, string text)
		{
			if (builder == null)
				builder = new StringBuilder();

			return text == null ? builder : builder.Insert(0, text);
		}

	}
}
