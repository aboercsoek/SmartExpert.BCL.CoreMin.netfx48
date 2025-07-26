//--------------------------------------------------------------------------
// File:    OutputDebugStringTracer.cs
// Content:	Implementation of class OutputDebugStringTracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Interop;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{
	///<summary>
	/// Implementation of Logger OutputDebugStringTracer. Implements <see cref="ITracer"/> and uses OutputDebugString for output.
	///</summary>
	public class OutputDebugStringTracer : ITracer
	{
		#region Private members
		private int m_ProcessId = -1;
		private string m_ProcessName;
		#endregion Private members

		#region C'tors

		/// <summary>
		/// Default constructor of logger
		/// </summary>
		public OutputDebugStringTracer()
		{
			InitProcessInfo();
		}

		#endregion C'tors

		#region ITracer Members

		/// <summary>
		/// Gets the trace level.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
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
		/// 	<see langword="true"/> if [is trace enabled] [the specified level]; otherwise, <see langword="false"/>.
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
			//Process currentProcess = Process.GetCurrentProcess();
			string timeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
			string logEntry = string.Format("{0} ({1},{2}) {3}: {4}",
				timeStamp, GetProcessId(), Thread.CurrentThread.ManagedThreadId, context, text);

			Win32.OutputDebugString(logEntry);
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
			
			Win32.OutputDebugString(logEntry);
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
			//new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			if (m_ProcessName == null)
			{
				Process currentProcess = null;
				try
				{
					currentProcess = Process.GetCurrentProcess();
					m_ProcessId = currentProcess.Id;
					m_ProcessName = currentProcess.ProcessName;
				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;
				}
				finally
				{
					if (currentProcess != null)
						currentProcess.Dispose();
				}
			}
		}

		#endregion Private methods

	}
}
