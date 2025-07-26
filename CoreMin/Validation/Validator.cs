//--------------------------------------------------------------------------
// File:    Validator.cs
// Content:	Implementation of class Validator
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert.RegularExpression;

#endregion

namespace SmartExpert
{
	///<summary>Validator provides common validation checks.</summary>
	public static class Validator
	{
		#region String Validation Methods

		/// <summary>
		/// Checks if the specified string value is NOT <see langword="null"/> or empty.
		/// </summary>
		/// <param name="valueToCheck">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="valueToCheck"/> NOT <see langword="null"/> or empty; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNotEmpty(string valueToCheck)
		{
			return valueToCheck.IsNullOrEmpty() == false;
		}

		/// <summary>
		/// Checks if the specified string value is NOT <see langword="null"/> or empty.
		/// </summary>
		/// <param name="valueToCheck">The value to check.</param>
		/// <param name="trimValueBeforeCheck">Is set to <see langword="true"/> the value will be trimmed before empty check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="valueToCheck"/> NOT <see langword="null"/> or empty, even after trim; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNotEmpty(string valueToCheck, bool trimValueBeforeCheck)
		{
			return valueToCheck.IsNullOrEmptyWithTrim() == false;
		}

		/// <summary>
		/// Checks if the specified string value is NOT <see langword="null"/>.
		/// </summary>
		/// <param name="valueToCheck">The value to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="valueToCheck"/> NOT <see langword="null"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNotNull(string valueToCheck)
		{
			return valueToCheck.IsNull() == false;
		}

		/// <summary>
		/// Validates if <paramref name="valueToCheck"/> has at least <paramref name="minLength"/> characters.
		/// </summary>
		/// <param name="valueToCheck">The value to check.</param>
		/// <param name="minLength">The minimum length.</param>
		/// <returns>
		///		<see langword="true"/> if <paramref name="valueToCheck"/> has at least <paramref name="minLength"/> characters; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool ValidateMinLength(string valueToCheck, int minLength)
		{
			if (minLength < 0) minLength = 0;

			return valueToCheck.SafeLength() >= minLength;
		}

		/// <summary>
		/// Validates if <paramref name="valueToCheck"/> has not more than <paramref name="maxLength"/> characters.
		/// </summary>
		/// <param name="valueToCheck">The value to check.</param>
		/// <param name="maxLength">The maximum length.</param>
		/// <returns>
		///		<see langword="true"/> if <paramref name="valueToCheck"/> has not more than <paramref name="maxLength"/> characters; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool ValidateMaxLength(string valueToCheck, int maxLength)
		{
			if (maxLength < 0) maxLength = 0;

			return valueToCheck.SafeLength() <= maxLength;
		}

		/// <summary>
		/// Validates if <paramref name="emailAddress"/> is a valid email address.
		/// <note>Uses <see cref="RegexExpressionStrings.EmailExpression"/> to check format compliance.</note>
		/// </summary>
		/// <param name="emailAddress">The email address to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="emailAddress"/> is a valid email address; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsValidEmailAddress(string emailAddress)
		{
			return emailAddress.IsNullOrEmptyWithTrim().IsFalse() && emailAddress.IsMatchingTo(RegexExpressionStrings.EmailExpression);
		}

		/// <summary>
		/// Validates if <paramref name="url"/> is a valid URL.
		/// <note>Uses <see cref="RegexExpressionStrings.UrlExpression"/> to check format compliance.</note>
		/// </summary>
		/// <param name="url">The URL to check.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="url"/> is a valid URL; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsValidUrl(string url)
		{
			return url.IsNullOrEmptyWithTrim().IsFalse() && url.IsMatchingTo(RegexExpressionStrings.UrlExpression);
		}

		#endregion
	}
}
