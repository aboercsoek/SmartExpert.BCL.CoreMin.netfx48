//--------------------------------------------------------------------------
// File:    ScopeTracer.cs
// Content:	Implementation of static class ScopeTracer
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
	/// using (new ScopeTracer())
	/// {
	///		// Code ...
	/// }
	/// </code>
	/// </example>
	public sealed class  ScopeTracer : IDisposable
	{
		#region Private Fields

		[ThreadStatic] private static int m_InstanceDeepth;
		
		private Stopwatch m_StopWatch;
		private string m_Message;
		private ITracer m_Tracer;
		private int m_Deepth;

		#endregion 

		#region Ctor, Dispose pattern

		/// <summary>
		/// Initializes a new instance of the <see cref=" ScopeTracer"/> class.
		/// </summary>
		/// <example>
		/// <code lang="cs" title="ScopeTracer Example (Default)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_Default" />
		/// <code lang="cs" title="ScopeTracer Example (Outer + Inner Scope)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_OuterInnerScope" />
		/// </example>
		public ScopeTracer()
			: this(null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref=" ScopeTracer"/> class.
		/// </summary>
		/// <param name="message">The scope trace message.</param>
		/// <example>
		/// <code lang="cs" title="ScopeTracer Example (Message)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_Message" />
		/// <code lang="cs" title="ScopeTracer Example (Outer + Inner Scope)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_OuterInnerScope" />
		/// </example>
		public ScopeTracer(string message)
			: this(null, message)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref=" ScopeTracer"/> class.
		/// </summary>
		/// <param name="tracer">The tracer to use.</param>
		/// <example>
		/// <code lang="cs" title="ScopeTracer Example (Tracer)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_Tracer" />
		/// </example>
		public ScopeTracer(ITracer tracer) 
			: this(tracer, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref=" ScopeTracer"/> class.
		/// </summary>
		/// <param name="tracer">The tracer to use.</param>
		/// <param name="message">The scope trace message.</param>
		/// <example>
		/// <code lang="cs" title="ScopeTracer Example (Tracer + Message)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsScopeTracer.cs" region="ScopeTracer_TracerPlusMessage" />
		/// </example>
		public ScopeTracer(ITracer tracer, string message)
		{
			m_Tracer = tracer ?? new OutputDebugStringTracer();
			m_Message = message ?? string.Empty;
			m_StopWatch = new Stopwatch();
			Start();
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// </summary>
		~ScopeTracer()
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

			if (m_Tracer != null)
				m_Tracer.Trace(TraceLevel.Verbose, "ScopeTracer", traceMessage);
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
			
			if (m_Tracer != null)
				m_Tracer.Trace(TraceLevel.Verbose, "ScopeTracer", traceMessage);
			m_StopWatch.Reset();
		}

		#endregion
	}
}