//--------------------------------------------------------------------------
// File:    ActionScopeTracer.cs
// Content:	Implementation of static class ActionScopeTracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// Provides time measurement and trace output when used as parameter of a using-Statement.
	/// </summary>
	/// <example lang="cs" title="Minimal ScopeTracer usage." numberLines="true" outlining="true">
	/// <code>
	/// using (new ActionScopeTracer())
	/// {
	///		// Code ...
	/// }
	/// </code>
	/// </example>
	public sealed class  ActionScopeTracer : IDisposable
	{
		#region Private Fields

		[ThreadStatic] private static int m_InstanceDeepth;
		
		private Stopwatch m_StopWatch;
		private string m_Message;
		private Action<int, string> m_StartAction;
		private Action<int, TimeSpan, string> m_EndAction;
		private int m_Deepth;

		#endregion 

		#region Ctor, Dispose pattern

		/// <summary>
		/// Initializes a new instance of the <see cref=" ScopeTracer"/> class.
		/// </summary>
		/// <param name="message">The scope trace message.</param>
		/// <param name="startAction">The scope start action.</param>
		/// <param name="endAction">The scope end action.</param>
		public ActionScopeTracer(string message, Action<int, string> startAction, Action<int, TimeSpan, string> endAction)
		{
			m_StartAction = startAction;
			m_EndAction = endAction;
			m_Message = message ?? string.Empty;
			m_StopWatch = new Stopwatch();
			Start();
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// </summary>
		~ActionScopeTracer()
		{
			Cleanup(false);
		}

		/// <summary>
		/// Writes 'Leave' trace message to used tracer instance.
		/// </summary>
		public void Dispose()
		{
			lock ( this )
			{
				//Check to see if Dispose(  ) has already been called
				if ( m_Disposed == false )
				{
					Cleanup(true);
					m_Disposed = true;
					//Take yourself off the finalization queue
					//to prevent finalization from executing a second time.
					GC.SuppressFinalize(this);
				}
			}

		}

		/// <summary>
		/// Stops the time measurement and write debug message.
		/// Is called wether from Dispose or from the destructor.
		/// </summary>
		private void Cleanup(bool fromDispose)
		{
			Stop(fromDispose);
		}

		private bool m_Disposed;

		#endregion

		#region Private Start- and Stop-Method

		/// <summary>
		/// Starts the stop watch of the scope tracer
		/// </summary>
		private void Start()
		{
			++m_InstanceDeepth;
			m_Deepth = m_InstanceDeepth;

			string traceMessage = "ENTER [#{0}|{1}]".SafeFormatWith(m_Deepth.ToString("00"), m_Message);
 
			m_StopWatch.Start();

			if (m_StartAction != null)
				m_StartAction(m_Deepth, traceMessage);
		}

		/// <summary>
		/// Stops the stop watch of the scope tracer
		/// </summary>
		private void Stop( bool fromDispose )
		{
			--m_InstanceDeepth;
			m_StopWatch.Stop();

			string mask = fromDispose ? "LEAVE [#{0}|time:{1} ms|{2}]" : "LEAVE [#{0}|time:{1} ms|{2} (from finalizer!)]";
			string traceMessage = mask.SafeFormatWith(m_Deepth.ToString("00"), m_StopWatch.ElapsedMilliseconds.ToString("0000"), m_Message);
			
			if (m_EndAction != null)
				m_EndAction(m_Deepth, m_StopWatch.Elapsed, traceMessage);
			m_StopWatch.Reset();
		}

		#endregion
	}
}