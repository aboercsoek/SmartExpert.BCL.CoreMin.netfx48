﻿//--------------------------------------------------------------------------
// File:    ArgChecker.cs
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class ArgChecker
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SmartExpert.Error;
using SmartExpert.IO;
using SmartExpert.RegularExpression;
using JetBrains.Annotations;

#endregion

namespace SmartExpert
{
	/// <summary>
	/// ArgChecker provides help methods for parameter checking.
	/// </summary>
	public static class ArgChecker
	{
		#region ShouldNotBeNull methods

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/>.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/></note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/></exception>
		[AssertionMethod]
		public static void ShouldNotBeNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object argValue, [InvokerParameterName] string argName)
		{
			if ( argValue == null )
				throw new ArgNullException(argName);
		}


		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/>.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/></note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object argValue, [InvokerParameterName] string argName, string errorMessage)
		{
			if ( argValue == null )
				throw new ArgNullException(argName, string.IsNullOrEmpty(errorMessage) ? StringResources.ErrorShouldNotBeNullValidationTemplate1Arg : errorMessage);
		}

		#endregion ShouldNotBeNull methods

		#region ShouldNotBeNullOrEmpty methods

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or empty.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is an empty string.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argValue"/> is an empty string.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName)
		{
			ShouldNotBeNull(argValue, argName);

			if ( argValue.Length == 0 )
				throw new ArgEmptyException(argName);
		}

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or <see cref="String.Empty"/>.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is an empty string.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <paramref name="argValue"/> is an empty string.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName, string errorMessage)
		{
			ShouldNotBeNull(argValue, argName, errorMessage);

			if ( argValue.Length == 0 )
			{
				throw new ArgEmptyException(argName, string.IsNullOrEmpty(errorMessage)
				                                      	? StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg
				                                      	: errorMessage);
			}
		}
		
		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or <see cref="Guid.Empty"/>.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is <see cref="Guid.Empty"/>.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argValue"/> is <see cref="Guid.Empty"/>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Guid? argValue, [InvokerParameterName]string argName)
		{
			ShouldNotBeNull(argValue, argName);

			if (argValue.Value == Guid.Empty)
			{
				throw new ArgEmptyException(argName, StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg);
			}
		}

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or <see cref="Guid.Empty"/>.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is <see cref="Guid.Empty"/>.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <paramref name="argValue"/> is an empty Guid.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Guid? argValue, [InvokerParameterName] string argName, string errorMessage)
		{
			ShouldNotBeNull(argValue, argName, errorMessage);

			if (argValue.Value == Guid.Empty)
			{
				throw new ArgEmptyException(argName, string.IsNullOrEmpty(errorMessage)
														? StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg
														: errorMessage);
			}
		}

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or empty.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> contains an empty string.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <paramref name="argValue"/> is an empty StringBuilder.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] StringBuilder argValue, [InvokerParameterName] string argName)
		{
			ShouldNotBeNull(argValue, argName);

			if (argValue.IsNullOrEmpty())
			{
				throw new ArgEmptyException(argName, StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg);
			}
		}

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> or empty.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> contains an empty string.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgNullOrEmptyException">Is thrown if <paramref name="argValue"/> is an empty StringBuilder.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] StringBuilder argValue, [InvokerParameterName] string argName, string errorMessage)
		{
			ShouldNotBeNull(argValue, argName, errorMessage);

			if (argValue.IsNullOrEmpty())
			{
				throw new ArgEmptyException(argName, string.IsNullOrEmpty(errorMessage)
														? StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg
														: errorMessage);
			}
		}

		/// <summary>
		/// Check if <paramref name="argValue"/> is not <see langword="null"/> and not an empty collection.
		/// <note>Throws an <see cref="ArgNullException"/> if <paramref name="argValue"/> is <see langword="null"/>.</note>
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is an empty collection.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argValue"/> is an empty collection.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] IEnumerable argValue, [InvokerParameterName] string argName)
		{
			ShouldNotBeNull(argValue, argName);

			if (argValue.AsSequence<object>().Any())
				return;

			throw new ArgEmptyException(argName);
		}

		#endregion ShouldNotBeNullOrEmpty methods

		#region ShouldNotBeEmpty methods

		/// <summary>
		/// Check if the <see cref="Guid"/> argument is not <see cref="Guid.Empty"/>.
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is <see cref="Guid.Empty"/>.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argValue"/> is an empty Guid.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeEmpty(Guid argValue, [InvokerParameterName] string argName)
		{
			if ( argValue == Guid.Empty )
				throw new ArgEmptyException(argName);
		}

		/// <summary>
		/// Check if the <see cref="Guid"/> argument is not <see cref="Guid.Empty"/>.
		/// <note>Throws an <see cref="ArgEmptyException"/> if <paramref name="argValue"/> is <see cref="Guid.Empty"/>.</note>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argValue"/> is an empty Guid.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldNotBeEmpty(Guid argValue, [InvokerParameterName] string argName, string errorMessage)
		{
			if ( argValue == Guid.Empty )
			{
				throw new ArgEmptyException(argName, string.IsNullOrEmpty(errorMessage)
				                                      	? StringResources.ErrorArgumentNotEmptyValidationTemplate1Arg
				                                      	: errorMessage);
			}
		}

		#endregion ShouldNotBeEmpty methods

		#region ShouldBeTrue, ShouldBeFalse methods

		/// <summary>
		/// Checks if the argument condition is true.
		/// </summary>
		/// <exception cref="ArgException{T}">Is thrown if <paramref name="argCondition"/> is false.</exception>
		/// <param name="argCondition">The argument condition.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeTrue([AssertionCondition(AssertionConditionType.IS_TRUE)]bool argCondition, [InvokerParameterName] string argName, string errorMessage)
		{
			if (argCondition == false)
				throw new ArgException<bool>(false, argName, errorMessage);
		}

		/// <summary>
		/// Checks if the argument condition is false.
		/// </summary>
		/// <exception cref="ArgException{T}">Is thrown if <paramref name="argCondition"/> is true.</exception>
		/// <param name="argCondition">The argument condition.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="errorMessage">The error message.</param>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeFalse([AssertionCondition(AssertionConditionType.IS_FALSE)]bool argCondition, [InvokerParameterName] string argName, string errorMessage)
		{
			if (argCondition)
				throw new ArgException<bool>(true, argName, errorMessage);
		}

		#endregion ShouldBeTrue, ShouldBeFalse methods

		#region ShouldBeInRange methods

		/// <summary>
		/// Checks if the argument is in a specified range. Specified minValue and maxValue are valid values of argValue.
		/// </summary>
		/// <typeparam name="T">The argument type.</typeparam>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="minValue">The valid min value.</param>
		/// <param name="maxValue">The valid max value.</param>
		/// <exception cref="ArgOutOfRangeException{T}">Is thrown if <paramref name="argValue"/> is less than <paramref name="minValue"/> or greater than <paramref name="maxValue"/>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeInRange<T>(T argValue, [InvokerParameterName] string argName, T minValue, T maxValue) where T : IComparable<T>
		{
			if (((IComparable<T>)maxValue).CompareTo(minValue) >= 0)
			{
				if (((IComparable<T>)argValue).CompareTo(minValue) < 0)
					throw new ArgOutOfRangeException<T>(argValue, argName, minValue, maxValue);

				if (((IComparable<T>)argValue).CompareTo(maxValue) > 0)
					throw new ArgOutOfRangeException<T>(argValue, argName, minValue, maxValue);
			}
			else
			{
				if (((IComparable<T>)argValue).CompareTo(maxValue) < 0)
					throw new ArgOutOfRangeException<T>(argValue, argName, maxValue, minValue);

				if (((IComparable<T>)argValue).CompareTo(minValue) > 0)
					throw new ArgOutOfRangeException<T>(argValue, argName, maxValue, minValue);
				
			}
		}

		#endregion

		#region ShouldBeExistingFile, ShouldBeExistingDirectory methods

		/// <summary>
		/// Check if the <paramref name="argFilePath"/> value contains a path to a existing file.
		/// </summary>
		/// <param name="argFilePath">The argument file path value.</param>
		/// <param name="argName">The Name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argFilePath"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argFilePath"/> is an empty string.</exception>
		/// <exception cref="FilePathTooLongException">Is thrown if <paramref name="argFilePath"/> length is too long (see <see cref="FileHelper.MAXIMUM_FILE_NAME_LENGTH"/>).</exception>
		/// <exception cref="ArgFilePathException">Is thrown if the <paramref name="argFilePath"/> value is not a path to a existing file.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeExistingFile([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argFilePath, [InvokerParameterName] string argName)
		{
			ShouldNotBeNullOrEmpty(argFilePath, argName);

			if (argFilePath.Length > FileHelper.MAXIMUM_FILE_NAME_LENGTH)
				throw new FilePathTooLongException(argFilePath, argName);

			if (File.Exists(argFilePath) == false)
				throw new ArgFilePathException(argFilePath, argName);
		}

		/// <summary>
		/// Check if the <paramref name="argFileInfo"/> is a existing file.
		/// </summary>
		/// <param name="argFileInfo">The file info to check.</param>
		/// <param name="argName">The Name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argFileInfo"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argFileInfo"/> is an empty string.</exception>
		/// <exception cref="FilePathTooLongException">Is thrown if <paramref name="argFileInfo"/> length is too long (see <see cref="FileHelper.MAXIMUM_FILE_NAME_LENGTH"/>).</exception>
		/// <exception cref="ArgFilePathException">Is thrown if the <paramref name="argFileInfo"/> value is not a path to a existing file.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeExistingFile([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] FileInfo argFileInfo, [InvokerParameterName] string argName)
		{
			ShouldNotBeNull(argFileInfo, argName);

			if (argFileInfo.Exists == false)
				throw new ArgFilePathException(argFileInfo, argName);
		}

		/// <summary>
		/// Check if the <paramref name="argDirectoryPath"/> value contains a path to a existing directory.
		/// </summary>
		/// <param name="argDirectoryPath">The argument directory path value.</param>
		/// <param name="argName">The Name of the argument.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argDirectoryPath"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="argDirectoryPath"/> is an empty string.</exception>
		/// <exception cref="DirectoryPathTooLongException">Is thrown if <paramref name="argDirectoryPath"/> length is too long (see <see cref="FileHelper.MAXIMUM_FOLDER_NAME_LENGTH"/>).</exception>
		/// <exception cref="ArgDirectoryPathException">Is thrown if the <paramref name="argDirectoryPath"/> value is not a path to a existing directory.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeExistingDirectory([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argDirectoryPath, [InvokerParameterName] string argName)
		{
			ShouldNotBeNullOrEmpty(argDirectoryPath, argName);

			if (argDirectoryPath.Length > FileHelper.MAXIMUM_FOLDER_NAME_LENGTH)
				throw new DirectoryPathTooLongException(argDirectoryPath, argName);

			if (Directory.Exists(argDirectoryPath) == false)
				throw new ArgDirectoryPathException(argDirectoryPath, argName);
		}

		#endregion

		#region ShouldMatch methods

		/// <summary>
		/// Check if the <paramref name="argValue"/> value mathes the <paramref name="regexPattern">Regular Expression</paramref>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="regexPattern">The regular expression to match.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> or <paramref name="regexPattern"/> is <see langword="null"/></exception>
		/// <exception cref="ArgException{String}">Is thrown if the <paramref name="argValue"/> value does not mathes the <paramref name="regexPattern">Regular Expression</paramref>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldMatch([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName, string regexPattern)
		{
			ShouldNotBeNull(argValue, argName);
			ShouldNotBeNull(regexPattern, "regexPattern");
			Regex regex = new Regex(regexPattern);
			ShouldMatch(argValue, argName, regex);
		}

		/// <summary>
		/// Check if the <paramref name="argValue"/> value mathes the <paramref name="regex">Regular Expression</paramref>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="regex">The regular expression to match.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> or <paramref name="regex"/> is <see langword="null"/></exception>
		/// <exception cref="ArgException{String}">Is thrown if the <paramref name="argValue"/> value does not mathes the <paramref name="regex">Regular Expression</paramref>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldMatch([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName, Regex regex)
		{
			ShouldNotBeNull(argValue, argName);
			ShouldNotBeNull(regex, "regex");

			if (RegexHelper.MatchAny(argValue, regex) == -1)
				throw new ArgException<string>(argValue, argName,
				                               "Argument {0} does not match the regular expresssion '{1}' (argument value: {2})".
				                               	SafeFormatWith(argName, regex.ToString(), argValue));
		}


		/// <summary>
		/// Check if the <paramref name="argValue"/> value mathes the <paramref name="regexPattern">Regular Expression</paramref>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="regexPattern">The regular expression to match.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> or <paramref name="regexPattern"/> is <see langword="null"/></exception>
		/// <exception cref="ArgException{String}">Is thrown if the <paramref name="argValue"/> value does not mathes the <paramref name="regexPattern">Regular Expression</paramref>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldMatch([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName, string regexPattern, string errorMessage)
		{
			ShouldNotBeNull(argValue, argName);
			ShouldNotBeNull(regexPattern, "regexPattern");
			Regex regex = new Regex(regexPattern);
			ShouldMatch(argValue, argName, regex, errorMessage);
		}

		/// <summary>
		/// Check if the <paramref name="argValue"/> value mathes the <paramref name="regex">Regular Expression</paramref>
		/// </summary>
		/// <param name="argValue">The argument value.</param>
		/// <param name="argName">The name of the argument.</param>
		/// <param name="regex">The regular expression to match.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="argValue"/> or <paramref name="regex"/> is <see langword="null"/></exception>
		/// <exception cref="ArgException{String}">Is thrown if the <paramref name="argValue"/> value does not mathes the <paramref name="regex">Regular Expression</paramref>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldMatch([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string argValue, [InvokerParameterName] string argName, Regex regex, string errorMessage)
		{
			ShouldNotBeNull(argValue, argName);
			ShouldNotBeNull(regex, "regex");

			if (RegexHelper.MatchAny(argValue, regex) == -1)
				throw new ArgException<string>(argValue, argName, errorMessage);
		}

		#endregion

		#region ShouldBeAssignable methods

		/// <summary>
		/// Verifies that an argument type is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy).
		/// </summary>
		/// <param name="targetType">The argument type that will be assigned to.</param>
		/// <param name="sourceType">The type of the value being assigned.</param>
		/// <param name="sourceArgumentName">Argument name.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="sourceType"/> or <paramref name="targetType"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidTypeCastException">Is thrown if <paramref name="targetType"/> is not assignable from <paramref name="sourceType"/>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeAssignableFrom([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type sourceType, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type targetType, [InvokerParameterName] string sourceArgumentName)
		{
			if (targetType == null)
			{
				throw new ArgNullException("targetType", StringResources.ErrorShouldNotBeNullValidationTemplate1Arg);
			}
			if (sourceType == null)
			{
				throw new ArgNullException("sourceType", StringResources.ErrorShouldNotBeNullValidationTemplate1Arg);
			}
			if (targetType.IsAssignableFrom(sourceType) == false)
			{

				throw new InvalidTypeCastException("Argument {0} error. {1}".SafeFormatWith(sourceArgumentName.SafeString(), StringResources.ErrorTypesAreNotAssignableTemplate2Args.SafeFormatWith(sourceType, targetType)));
			}
		}

		/// <summary>
		/// Verifies that an argument type is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy).
		/// </summary>
		/// <typeparam name="TSource">The argument type that will be assigned to.</typeparam>
		/// <typeparam name="TTarget">The type of the value being assigned.</typeparam>
		/// <param name="sourceArgumentName">Argument name.</param>
		/// <exception cref="InvalidTypeCastException">Is thrown if <typeparamref name="TTarget"/> is not assignable from <typeparamref name="TSource"/>.</exception>
		[DebuggerStepThrough, AssertionMethod]
		public static void ShouldBeAssignableFrom<TSource, TTarget>([InvokerParameterName] string sourceArgumentName)
		{
			ShouldBeAssignableFrom(typeof(TSource), typeof(TTarget), sourceArgumentName.SafeString());
		}

		#endregion

		#region ShouldBeInstanceOfType methods

		/// <summary>
		/// Verifies that an argument instance is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy, or instance can be 
		/// assigned through a runtime wrapper, as is the case for COM Objects).
		/// </summary>
		/// <param name="targetType">The argument type that will be assigned to.</param>
		/// <param name="assignmentInstance">The instance that will be assigned.</param>
		/// <param name="argumentName">Argument name.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="assignmentInstance"/> or <paramref name="targetType"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidTypeCastException">Is thrown if <paramref name="assignmentInstance"/> is not an instance of <paramref name="targetType"/>.</exception>
		public static void ShouldBeInstanceOfType(Type targetType, object assignmentInstance, string argumentName)
		{
			if (assignmentInstance == null)
			{
				throw new ArgNullException("assignmentInstance", StringResources.ErrorShouldNotBeNullValidationTemplate1Arg);
			}
			if (targetType == null)
			{
				throw new ArgNullException("targetType", StringResources.ErrorShouldNotBeNullValidationTemplate1Arg);
			}

			if (targetType.IsInstanceOfType(assignmentInstance) == false)
			{

				throw new InvalidTypeCastException("Argument {0} error. {1}".SafeFormatWith(argumentName.SafeString(), StringResources.ErrorTypesAreNotAssignableTemplate2Args.SafeFormatWith(GetTypeName(assignmentInstance), targetType)));
			}
		}

		/// <summary>
		/// Verifies that an argument type is assignable from the provided type (meaning
		/// interfaces are implemented, or classes exist in the base class hierarchy).
		/// </summary>
		/// <typeparam name="TTarget">The type of the value being assigned.</typeparam>
		/// <param name="assignmentInstance">The instance that will be assigned.</param>
		/// <param name="argumentName">Argument name.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="assignmentInstance"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidTypeCastException">Is thrown if <paramref name="assignmentInstance"/> is not an instance of <typeparamref name="TTarget"/>.</exception>
		public static void ShouldBeInstanceOfType<TTarget>(object assignmentInstance, string argumentName)
		{
			ShouldBeInstanceOfType(typeof(TTarget), assignmentInstance, argumentName.SafeString());
		}


		private static string GetTypeName(object assignmentInstance)
		{
			try
			{
				return assignmentInstance.GetType().FullName;
			}
			catch (Exception)
			{
				return "<unknown>";
			}
		}



		#endregion
	}
}
