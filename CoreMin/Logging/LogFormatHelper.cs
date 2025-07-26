//--------------------------------------------------------------------------
// File:    LogFormatHelper.cs
// Content:	Implementation of class LogFormatHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.Linq;
using System.Text;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;

#endregion


namespace SmartExpert.Logging
{
	/// <summary>
	/// Logging message formatting helper methods
	/// </summary>
	public static class LogFormatHelper
	{
		/// <summary>
		/// Builds the log message.
		/// <note>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {message}."</note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="message">The message.</param>
		/// <returns>Formatted log message.</returns>
		/// <remarks>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {message}."</remarks>
		public static string Format(LogMethod logMethod, string message)
		{
			return "{0}: {1}".SafeFormatWith(logMethod.GetDisplayName(), message.SafeString().EnsureEndsWith("."));
		}

		/// <summary>
		/// Builds the log message for the given activity context.
		/// <note>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}."</note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="activity">The activity.</param>
		/// <param name="message">The message.</param>
		/// <returns>Formatted message.</returns>
		/// <remarks>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}."</remarks>
		public static string Format(LogMethod logMethod, string activity, string message)
		{
			return "{0}: {1}: {2}".SafeFormatWith(logMethod.GetDisplayName(), activity, message.SafeString().EnsureEndsWith("."));
		}

		/// <summary>
		/// Builds the error log message.
		/// <note><para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {message}.\n{errorMsg}"</para>s 
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = exception.Message.</para></note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="message">The message.</param>
		/// <param name="exceptionToLog">The exception to log.</param>
		/// <returns>Formatted message.</returns>
		/// <remarks>
		/// <para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {message}.\n{errorMsg}"</para>
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = exception.Message.</para>
		/// </remarks>
		public static string Format(LogMethod logMethod, string message, Exception exceptionToLog)
		{
			if (exceptionToLog == null)
				return string.Empty;

			var exceptionText = exceptionToLog.RenderException();
			message = message.SafeString() == string.Empty ? message.SafeString() : message.EnsureEndsWith(".");
			string errorMsg = ((logMethod == LogMethod.Error) || (logMethod == LogMethod.Error)) ? exceptionText.FullText : exceptionText.Message;
			return "{0}: {1}\n{2}".SafeFormatWith(logMethod.GetDisplayName(), message, errorMsg);
		}

		/// <summary>
		/// Builds the error log message for the given activity context.
		/// <note><para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}.\n{errorMsg}"</para>
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = Exception Message.</para></note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="activity">The activity context.</param>
		/// <param name="message">The error message.</param>
		/// <param name="exceptionToLog">The exception to log.</param>
		/// <returns>The formatted error log message.</returns>
		/// <remarks>
		/// <para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}.\n{errorMsg}"</para>
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = exception.Message.</para>
		/// </remarks>
		public static string Format(LogMethod logMethod, string activity, Exception exceptionToLog, string message)
		{
			if (exceptionToLog == null)
				return string.Empty;

			var exceptionText = exceptionToLog.RenderException();
			message = message.SafeString() == string.Empty ? message.SafeString() : message.EnsureEndsWith(".");
			string errorMsg = ((logMethod == LogMethod.Error)||(logMethod == LogMethod.Error)) ? exceptionText.FullText : exceptionText.Message;
			return "{0}: {1}: {2}\n{3}".SafeFormatWith(logMethod.GetDisplayName(), activity, message, errorMsg);
		}

		/// <summary>
		/// Builds the log message for the given activity context.
		/// <note>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}."</note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="activity">The activity.</param>
		/// <param name="messageTemplate">The message template.</param>
		/// <param name="messageTemplateArgs">The message template arguments.</param>
		/// <returns>The formatted log message.</returns>
		/// <remarks>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}."</remarks>
		public static string Format(LogMethod logMethod, string activity, string messageTemplate, params object[] messageTemplateArgs)
		{
			return "{0}: {1}: {2}".SafeFormatWith(logMethod.GetDisplayName(), activity, messageTemplate.SafeFormatWith(messageTemplateArgs).EnsureEndsWith("."));
		}

		/// <summary>
		/// Builds the error log message for the given activity context.
		/// <note><para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}.\n{errorMsg}"</para>
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = exception.Message.</para></note>
		/// </summary>
		/// <param name="logMethod">The log method.</param>
		/// <param name="activity">The activity context.</param>
		/// <param name="messageTemplate">The error message template.</param>
		/// <param name="exceptionToLog">The exception to log.</param>
		/// <param name="messageTemplateArgs">The error message template arguments.</param>
		/// <returns>The formatted error log message.</returns>
		/// <remarks>
		/// <para>Format: "{<see cref="LogMethod">logMethod</see> Displayname}: {activity}: {message}.\n{errorMsg}"</para>
		/// <para>if (<see cref="LogMethod">logMethod</see> == Debug or Error) => errorMsg = Full Exception Details.</para>
		/// <para>else => errorMsg = exception.Message.</para>
		/// </remarks>
		public static string Format(LogMethod logMethod, string activity, Exception exceptionToLog, string messageTemplate, params object[] messageTemplateArgs)
		{
			if (exceptionToLog == null)
				return string.Empty;

			var exceptionText = exceptionToLog.RenderException();
			string errorMsg = ((logMethod == LogMethod.Error) || (logMethod == LogMethod.Error)) ? exceptionText.FullText : exceptionText.Message;

			return "{0}: {1}: {2}\n{3}".SafeFormatWith(logMethod.GetDisplayName(), activity,
			                                           messageTemplate.SafeFormatWith(messageTemplateArgs).EnsureEndsWith("."), errorMsg);
		}

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
				return "{0} ({1}) = {2}".SafeFormatWith(name.SafeString(), "String", value.As<string>().SafeString("<null>"));
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
		public static string FormatCollection<T>(string collectionName, T collection, int maxItems) where T : IEnumerable
		{
			var sb = new StringBuilder();

			if (collection.IsDefaultValue())
			{
				return collectionName.IsNullOrEmpty() ? "Collection ({0})= <null>".SafeFormatWith(typeof(T).GetTypeName()) 
														: "{0} ({1}) = <null>".SafeFormatWith(collectionName, typeof(T).GetTypeName());
			}

			sb.Append(string.IsNullOrEmpty(collectionName)
						? "Collection {0}".SafeFormatWith(collection.GetType().GetTypeName())
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
	}
}

