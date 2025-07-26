//--------------------------------------------------------------------------
// File:    AppEventLog.cs
// Content:	Implementation of class AppEventLog
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.AppUtils;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.SystemManagement
{
	/// <summary>
	/// Provides easy access to the application event log
	/// </summary>
	public class AppEventLog : IDisposable
	{
		#region Defines

		/// <summary>Base EventLogId for eventlog information entries</summary>
		public const int BASE_INFO_ID = 0;
		/// <summary>Base EventLogId for eventlog warning entries</summary>
		public const int BASE_WARNING_ID = 10000;
		/// <summary>Base EventLogId for eventlog error entries</summary>
		public const int BASE_ERROR_ID = 20000;
		/// <summary>Base EventLogId for eventlog audit entries</summary>
		public const int BASE_AUDIT_ID = 30000;

		#endregion

		#region Ctors, Dtor, Dispose

		/// <summary>
		/// Initializes a new instance of the <see cref="AppEventLog"/> class.
		/// </summary>
		public AppEventLog()
		{
			string source = AppHelper.ApplicationEventLogSource;
			SourceName = source;

			CreateApplicationSource();

			if (!EventLog.SourceExists(source))
				throw new NotSupportedException("EventSource " + source + " does not exist!");

			m_EventLog = new EventLog();
			m_EventLog.Source = source;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AppEventLog"/> class.
		/// </summary>
		/// <param name="source">The source.</param>
		public AppEventLog(string source)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(source, "source");

			SourceName = source;
			
			CreateApplicationSource();

			if (!EventLog.SourceExists(source))
				throw new NotSupportedException("EventSource " + source + " does not exist!");

			m_EventLog = new EventLog();
			m_EventLog.Source = source;
		}

		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="AppEventLog"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock ( this )
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock ( this )
			{
				if ( m_Disposed == false )
				{
					Dispose(true);
				}
			}
		}

		private void Dispose(bool isDispose)
		{
			TypeHelper.DisposeIfNecessary(m_EventLog);
			m_EventLog = null;

			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="AppEventLog"/> is reclaimed by garbage collection.
		/// </summary>
		~AppEventLog()
		{
			Dispose(false);
		}

		#endregion

		#region Static Application Eventlog Members

		static string m_SourceName;

		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		/// <value>The name of the application.</value>
		public static string SourceName
		{
			get
			{
				if (m_SourceName == null)
					m_SourceName = AppHelper.ApplicationEventLogSource;

				return m_SourceName;
			}
			set
			{
				if (value.IsNullOrEmptyWithTrim() == true)
					m_SourceName = AppHelper.ApplicationEventLogSource;
				else
					m_SourceName = value;
			}
		}

		//static AppEventLog m_AppEventLog;

		// <summary>
		// Gets the application log.
		// </summary>
		// <value>The application log.</value>
		//public static AppEventLog ApplicationLog
		//{
		//    get
		//    {
		//        if (m_AppEventLog != null)
		//            return m_AppEventLog;

		//        // try create the source (needs admin rights and should NOT be relied on)
		//        CreateApplicationSource();

		//        m_AppEventLog = new AppEventLog(SourceName);
		//        return m_AppEventLog;
		//    }
		//}

		/// <summary>
		/// Creates the application source.
		/// </summary>
		public static void CreateApplicationSource()
		{
			string logName = AppHelper.ApplicationEventLogName;
			string source = SourceName;

			if (EventLog.SourceExists(source)) return;

			EventLog.CreateEventSource(source, logName);
			AppEventLog appEventLog = new AppEventLog(source);

			string message = "EventLog/EventSource " + logName + "/" + source + " created at " + DateTime.Now + ".";
			appEventLog.Info(message);

			QuickLogger.Log(message);

		}

		/// <summary>
		/// Deletes the application source.
		/// </summary>
		public static void DeleteApplicationSource()
		{
			EventLog.DeleteEventSource(SourceName);

			string message = "EventSource " + SourceName + " deleted at " + DateTime.Now.ToString() + ".";
			QuickLogger.Log(message);

		}

		#endregion

		#region General EventLog Members

		EventLog m_EventLog;

		/// <summary>
		/// Gets the windows event log.
		/// </summary>
		/// <value>The windows event log used by AppEventLog class.</value>
		public EventLog EventLog
		{
			get { return m_EventLog; }
		}

		#endregion

		#region Application EventLog write access methods

		/// <summary>
		/// Writes a information event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Info(string message)
		{
			Write(message, EventLogEntryType.Information, BASE_INFO_ID);
		}

		/// <summary>
		/// Writes a information event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventId">The event id.</param>
		public void Info(string message, int eventId)
		{
			Write(message, EventLogEntryType.Information, eventId);
		}

		/// <summary>
		/// Writes a warning event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Warning(string message)
		{
			Write(message, EventLogEntryType.Warning, BASE_WARNING_ID);
		}

		/// <summary>
		/// Writes a warning event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventId">The event id.</param>
		public void Warning(string message, int eventId)
		{
			Write(message, EventLogEntryType.Warning, eventId);
		}

		/// <summary>
		/// Writes a error event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		public void Error(string message)
		{
			Write(message, EventLogEntryType.Error, BASE_ERROR_ID);
		}

		/// <summary>
		/// Writes a error event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventId">The event id.</param>
		public void Error(string message, int eventId)
		{
			Write(message, EventLogEntryType.Error, eventId);
		}

		/// <summary>
		/// Writes a error event message into the eventlog.
		/// </summary>
		/// <param name="ex">The error (exception) to log.</param>
		public void Error(Exception ex)
		{
			ArgChecker.ShouldNotBeNull(ex, "ex");

			int offset = 0;
			BaseException baseException = ex as BaseException;
			if (baseException != null)
				offset = baseException.ErrorCode;

			string msg = ExceptionHelper.GetExceptionText(ex);
			Write(msg, EventLogEntryType.Error, BASE_ERROR_ID + offset);
		}

		/// <summary>
		/// Writes a error event message into the eventlog.
		/// </summary>
		/// <param name="ex">The error (exception) to log.</param>
		/// <param name="eventId">The event id.</param>
		public void Error(Exception ex, int eventId)
		{
			ArgChecker.ShouldNotBeNull(ex, "ex");

			string msg = ExceptionHelper.GetExceptionText(ex);
			Write(msg, EventLogEntryType.Error, eventId);
		}

		/// <summary>
		/// Writes a specific event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		public void Write(string message, EventLogEntryType eventLogEntryType)
		{
			Write(message, eventLogEntryType, BASE_AUDIT_ID);
		}

		/// <summary>
		/// Writes a specific event message into the eventlog.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="eventLogEntryType">The event log entry type.</param>
		/// <param name="eventId">The event id.</param>
		public void Write(string message, EventLogEntryType eventLogEntryType, int eventId)
		{
			if (Disposed == true)
				throw new ObjectDisposedException("AppEventLog has already been disposed!");

			ArgChecker.ShouldNotBeNullOrEmpty(message, "message");


			// Log the EventLog write access to the logging service.
			QuickLogger.Log( 
				"Writing event log entry:" + Environment.NewLine +
				"    Log:   {0}\\{1} ({2})" + Environment.NewLine +
				"    Type:  {3}" + Environment.NewLine +
				"    ID:    {4}" + Environment.NewLine +
				"    Entry: {5}".SafeFormatWith(
				m_EventLog.MachineName, m_EventLog.Log, m_EventLog.Source,
				eventLogEntryType,
				eventId,
				message));


			// support message length >32766 byte ???
			if (message.Length > 32000)
				message = message.Substring(0, 32000) + " [...] (---cut---)";

			message = message + Environment.NewLine + new string('_', 40);

			m_EventLog.WriteEntry(message, eventLogEntryType, eventId);
		}

		#endregion
	
	}
}
