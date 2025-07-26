//--------------------------------------------------------------------------
// File:    ConsoleTracer.cs
// Content:	Implementation of class ConsoleTracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.SystemProcesses;

#endregion

namespace SmartExpert.Diagnostics
{
	///<summary>
	/// Implementation of Logger ConsoleTracer. Implements <see cref="ITracer"/> and uses Console for output.
	///</summary>
	public class ConsoleTracer : ITracer
	{
		#region Private members
		private int m_ProcessId = -1;
		#endregion Private members

		#region C'tors

		/// <summary>
		/// Default constructor of logger
		/// </summary>
		public ConsoleTracer()
		{
			InitProcessInfo();
		}

		#endregion C'tors

		#region ITracer Members

		/// <summary>
		/// Gets the trace level.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The current trace level.</returns>
		public TraceLevel GetTraceLevel(string context)
		{
			return TraceLevel.Verbose;
		}

		/// <summary>
		/// Determines whether [is trace enabled] [the specified level].
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="context">The context.</param>
		/// <returns>
		/// 	<see langword="true"/> if trace is enabled for a specified trace level; otherwise, <see langword="false"/>.
		/// </returns>
		public bool IsTraceEnabled(TraceLevel level, string context)
		{
			return true;
		}

		/// <summary>
		/// Logs the verbose.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="context">The context.</param>
		/// <param name="text">The text.</param>
		public void Trace(TraceLevel level, string context, string text)
		{
			string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
			string logEntry = string.Format("{0} ({1},{2}) {3}: {4}",
				timeStamp, GetProcessId(), Thread.CurrentThread.ManagedThreadId, context, text);

			ConsoleHelper.WriteLine(logEntry, ConsoleColor.Green);
		}

		/// <summary>
		/// Logs the verbose.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="context">The context.</param>
		/// <param name="text">The text.</param>
		/// <param name="exception">The exception.</param>
		public void Trace(TraceLevel level, string context, string text, Exception exception)
		{
			string s = ExceptionHelper.GetExceptionText(exception);
			string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
			string logEntry = string.Format("{0} ({1},{2}) {3}: {4}",
				timeStamp, GetProcessId(), Thread.CurrentThread.ManagedThreadId, context, text + Environment.NewLine + s);

			ConsoleHelper.WriteLine(logEntry, ConsoleColor.Red);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Returns the current Windows process ID.
		/// </summary>
		/// <returns></returns>
		private int GetProcessId()
		{
			return m_ProcessId;
		}

		/// <summary>
		/// Gets the the process id and process name of the current process.
		/// </summary>
		private void InitProcessInfo()
		{
			if (m_ProcessId == -1)
			{
				Process currentProcess = Process.GetCurrentProcess();
				try
				{
					m_ProcessId = ProcessHelper.SafeGetProcessId(currentProcess);
				}
				finally
				{
					currentProcess.Dispose();
				}
			}
		}

		#endregion Private methods
	}
}
