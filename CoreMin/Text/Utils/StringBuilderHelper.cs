//--------------------------------------------------------------------------
// File:    StringBuilderHelper.cs
// Content:	Implementation of class StringBuilderHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

#endregion

namespace SmartExpert
{
	//*********************************************************************************************************
	//  Class: StringBuilderHelper
	//
	/// <summary>Utility methods for implementing <see cref="Object.ToString" /> overrides.</summary>
	/// 
	/// <remarks>All methods are static.</remarks>
	//*********************************************************************************************************
	public static class StringBuilderHelper
	{
		#region Public static helper methods

		//*****************************************************************************************************
		//	Method: AppendPropertyToString()
		//
		/// <summary>Appends a property value to a String with a newline.</summary>
		///
		/// <param name="stringBuilder">Object to append to.</param>
		/// <param name="indentationLevel">Indentation level.  Level 0 is "no indentation."</param>
		/// <param name="propertyName">Name of the property.  Can't be null or empty.</param>
		/// <param name="propertyValue">Value of the property.  Can be null or empty.</param>
		///
		/// <remarks>This method appends a property name, property value, and a newline to a String.
		/// </remarks>
		//*****************************************************************************************************
		public static void AppendPropertyToString(StringBuilder stringBuilder, Int32 indentationLevel, String propertyName, Object propertyValue )
		{
			ArgChecker.ShouldNotBeNull(stringBuilder, "stringBuilder");
			ArgChecker.ShouldNotBeNullOrEmpty(propertyName, "propertyName");
			ArgChecker.ShouldBeInRange(indentationLevel, "indentationLevel", 0, int.MaxValue);

			AppendPropertyToString(stringBuilder, indentationLevel, propertyName, propertyValue, true);
		}

		//*****************************************************************************************************
		//	Method: AppendPropertyToString()
		//
		/// <summary>Appends a property value to a String with an optional newline.</summary>
		///
		/// <param name="stringBuilder">Object to append to.</param>
		/// <param name="indentationLevel">Indentation level.  Level 0 is "no indentation."</param>
		/// <param name="propertyName">Name of the property.  Can't be null or empty.</param>
		/// <param name="propertyValue">Value of the property.  Can be null or empty.</param>
		/// <param name="bAppendLine">true to append a newline after the property name and value.</param>
		///
		/// <remarks>This method appends a property name, property value, and an optional newline to a String.
		/// </remarks>
		//*****************************************************************************************************
		public static void AppendPropertyToString(StringBuilder stringBuilder, Int32 indentationLevel, String propertyName, Object propertyValue, Boolean bAppendLine )
		{
			ArgChecker.ShouldNotBeNull(stringBuilder, "stringBuilder");
			ArgChecker.ShouldBeInRange(indentationLevel, "indentationLevel", 0, int.MaxValue);
			ArgChecker.ShouldNotBeNullOrEmpty(propertyName, "propertyName");

			AppendIndentationToString(stringBuilder, indentationLevel);

			stringBuilder.Append(propertyName);
            stringBuilder.Append(" = ");

			AppendObjectToString(stringBuilder, propertyValue);

			if ( bAppendLine )
			{
				stringBuilder.AppendLine();
			}
		}

		//*****************************************************************************************************
		//	Method: AppendIndentationToString()
		//
		/// <summary>Appends a specified number of indentation levels to a String.</summary>
		///
		/// <param name="stringBuilder">Object to append to.</param>
		/// <param name="indentationLevel">Indentation level.  Level 0 is "no indentation."</param>
		//*****************************************************************************************************
		public static void AppendIndentationToString( StringBuilder stringBuilder, Int32 indentationLevel )
		{
			ArgChecker.ShouldNotBeNull(stringBuilder, "stringBuilder");
			ArgChecker.ShouldBeInRange(indentationLevel, "indentationLevel", 0, int.MaxValue);

			// Use tabs for indentation.
			while ( indentationLevel > 0 )
			{
				stringBuilder.Append('\t');
				indentationLevel--;
			}
		}

		//*****************************************************************************************************
		//	Method: AppendObjectToString()
		//
		/// <summary>Appends an Object to a String.</summary>
		///
		/// <param name="stringBuilder">Object to append to.</param>
		/// <param name="obj">Object to append.  Can be null.</param>
		///
		/// <remarks>This method appends the String form of <paramref name="obj" /> to a String.
		/// If <paramref name="obj" /> is null, "[null]" is appended.
		/// </remarks>
		//*****************************************************************************************************
		public static void AppendObjectToString(StringBuilder stringBuilder, Object obj)
		{
			ArgChecker.ShouldNotBeNull(stringBuilder, "stringBuilder");

			stringBuilder.Append( ( obj == null ) ? NullString : obj.ToString() );
		}

		/// <summary>
		/// Format fallback method.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="sb">The sb.</param>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		[DebuggerStepThrough]
		[StringFormatMethod("format")]
		internal static void FormatFallback(Exception ex, StringBuilder sb, string format, params object[] args)
		{
			if (sb == null)
				return;

			sb.Append("*** Exception occured during formatting: ");

			if (ex != null)
			{
				sb.Append(ex.GetType().FullName).Append(": ")
				  .Append(ex.Message).Append(Environment.NewLine);
			}
			else
			{
				sb.Append(Environment.NewLine);
			}

			sb.Append("SafeFormat: '").Append(format.SafeString("<null>")).Append("'").Append(Environment.NewLine);

			if (args == null)
				sb.Append("args: <null>").Append(Environment.NewLine);
			else
			{
				for (int i = 0; i < args.Length; ++i)
					sb.Append("arg #")
					  .Append(i).Append(": '")
					  .Append(StringHelper.SafeToString(args[i], "<null>")).Append("'")
					  .Append(Environment.NewLine);
			}
		}

		#endregion

		#region Public constants

		/// <summary>
		/// String that represents a null object.
		/// </summary>
		public static String NullString = "[null]";

		#endregion
	}
}
