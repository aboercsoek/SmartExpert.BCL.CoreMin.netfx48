//--------------------------------------------------------------------------
// File:    QuickLogger.cs
// Content:	Implementation of class QuickLogger
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.Linq;

using SmartExpert.Interop;
using SmartExpert.SystemProcesses;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Logging
{
	///<summary>A Logger that uses Win32 OutputDebugString to log messages.</summary>
	public static class QuickLogger
	{
		private static bool m_Enabled = true;

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="QuickLogger"/> is enabled.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if enabled; otherwise, <see langword="false"/>.
		/// </value>
		public static bool Enabled
		{
			get { return m_Enabled; }
			set { m_Enabled = value; }
		}

		/// <summary>
		/// Win32 OutputDebugString logging method (does not add current process and thread infos to the message).
		/// </summary>
		/// <param name="logMessage">The log message.</param>
		public static void Log(string logMessage)
		{
			Log(logMessage, false);
		}

		/// <summary>
		/// Loging method that uses Win32 OutputDebugString to log message.
		/// </summary>
		/// <param name="logMessage">The log message.</param>
		/// <param name="addProcessAndThreadInfos">If true adds current process and thread infos to log message.</param>
		public static void Log(string logMessage, bool addProcessAndThreadInfos)
		{
			if ((m_Enabled == false) || (logMessage == null))
				return;

			Win32.OutputDebugString(addProcessAndThreadInfos
										? "{0} {1}".SafeFormatWith(ProcessHelper.GetCurrentProcessAndThreadString(), logMessage)
										: logMessage);
		}

		/// <summary>
		/// Logs the name value pair (does not add current process and thread infos to the message).
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public static void LogNameValue<T>(string name, T value)
		{
			Log(LogFormatHelper.FormatNameValue(name, value), false);
		}

		/// <summary>
		/// Logs the collection (does not add current process and thread infos to the message).
		/// </summary>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="collection">The collection.</param>
		public static void LogCollection(string collectionName, IEnumerable collection)
		{
			Log(LogFormatHelper.FormatCollection(collectionName, collection, 10), false);
		}

		/// <summary>
		/// Logs the collection (does not add current process and thread infos to the message).
		/// </summary>
		/// <param name="collectionName">Name of the collection.</param>
		/// <param name="collection">The collection.</param>
		/// <param name="maxItems">The max items.</param>
		public static void LogCollection(string collectionName, IEnumerable collection, int maxItems)
		{
			Log(LogFormatHelper.FormatCollection(collectionName, collection, maxItems), false);
		}

		/// <summary>
		/// Log exception details using Win32 OutputDebugString.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		public static void LogErrorDetails(Exception exception)
		{
			Log(LogFormatHelper.Format(LogMethod.Error, string.Empty, exception), true);
		}

		/// <summary>
		/// Log exception message using Win32 OutputDebugString.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		public static void LogErrorMessage(Exception exception)
		{
			Log(LogFormatHelper.Format(LogMethod.Warn, string.Empty, exception), true);
		}

	}
}
