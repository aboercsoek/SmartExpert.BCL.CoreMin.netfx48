//--------------------------------------------------------------------------
// File:    MethodTracer.cs
// Content:	Implementation of class MethodTracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SmartExpert;
using SmartExpert.Linq;
using SmartExpert.SystemProcesses;

#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// Provides method time measurement and trace output when used as parameter of a using-Statement.
	/// </summary>
	/// <example>
	/// <code lang="cs" title="Minimal MethodTracer usage example" numberLines="true" outlining="true">
	/// using (new MethodTracer(MethodBase.GetCurrentMethod()))
	/// {
	///		// Code ...
	/// }
	/// </code>
	/// </example>
	public class MethodTracer : IDisposable
	{
		[ThreadStatic]
		private static int m_InstanceDeepth;
		private MethodBase m_CurrentMethod;
		private Stopwatch m_StopWatch;
		private ITracer m_Tracer;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodTracer"/> class. Uses the default tracer (<see cref="OutputDebugStringTracer"/>) to trace out the scope messages.
		/// </summary>
		/// <param name="currentMethod">The current method.</param>
		/// <remarks><para>MethodTracer uses internally a <see cref="Stopwatch"/> instance to provide accurate time messurement.</para>
		/// <para>MethodTracer is capable of detecting inner scope calls (method with method tracer calls method that also uses method tracer) and provides indent of the inner call.</para></remarks>
		/// <example>
		/// <code lang="cs" title="MethodTracer Example (Default)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsMethodTracer.cs" region="MethodTracer_Default" />
		/// <code lang="cs" title="MethodTracer Example (Outer + Inner Scope)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsMethodTracer.cs" region="MethodTracer_OuterInnerScope" />
		/// </example>
		public MethodTracer(MethodBase currentMethod)
			:this(currentMethod, null)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodTracer"/> class.
		/// </summary>
		/// <param name="currentMethod">The current method.</param>
		/// <param name="tracer">The tracer to use.</param>
		/// <seealso cref="OutputDebugStringTracer"/> <seealso cref="ConsoleTracer"/>
		/// <remarks><para>MethodTracer uses internally a <see cref="Stopwatch"/> instance to provide accurate time messurement.</para>
		/// <para>MethodTracer is capable of detecting inner scope calls (method with method tracer calls method that also uses method tracer) and provides indent of the inner call.</para></remarks>
		/// <example>
		/// <code lang="cs" title="MethodTracer Example (Default)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsMethodTracer.cs" region="MethodTracer_Default" />
		/// <code lang="cs" title="MethodTracer Example (Outer + Inner Scope)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsMethodTracer.cs" region="MethodTracer_OuterInnerScope" />
		/// </example>
		public MethodTracer(MethodBase currentMethod, ITracer tracer)
		{
			m_Tracer = tracer ?? new OutputDebugStringTracer();
			m_CurrentMethod = currentMethod;
			m_StopWatch = new Stopwatch();
			Enter();
		}

		/// <summary>
		/// Writes exit scope messages to the tracer that is in use. Dispose is called during automaticly is MethodTracer was created in a using statement (look at the provided example).
		/// </summary>
		/// <example>
		/// <code lang="cs" title="MethodTracer Example (Default)" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_DiagnosticsMethodTracer.cs" region="MethodTracer_Default" />
		/// </example>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Writes exit scope messages to the tracer that is in use. Dispose is called during automaticly is MethodTracer was created in a using statement (look at the provided example).
		/// </summary>
		/// <param name="disposing"><see langword="true"/> if this method was called by Dispose() or false if the method was called by the finallizer.</param>
		protected virtual void Dispose(bool disposing)
		{
			m_StopWatch.Stop();
			Exit(disposing);
		}

		private void Enter()
		{
			string scopeDeepth = string.Empty;
			if (m_InstanceDeepth > 0)
				scopeDeepth = new string(' ', m_InstanceDeepth * 3);
			m_Tracer.Trace(TraceLevel.Verbose, "MethodTracer", scopeDeepth + "-->");

			m_Tracer.Trace(TraceLevel.Verbose, "MethodTracer", string.Format(CultureInfo.InvariantCulture, "{0}{1}Entering {2}.{3}", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_CurrentMethod.DeclaringType.Name, m_CurrentMethod.Name }));
			++m_InstanceDeepth;
			m_StopWatch.Start();
		}

		private void Exit(bool fromDispose)
		{
			--m_InstanceDeepth;
			string scopeDeepth = string.Empty;
			if (m_InstanceDeepth > 0)
				scopeDeepth = new string(' ', m_InstanceDeepth * 3);

			string fromDisposeMessage = string.Empty;
			if (fromDispose.IsFalse())
				fromDisposeMessage = " (from finalizer!)";

			m_Tracer.Trace(TraceLevel.Verbose, "MethodTracer", string.Format(CultureInfo.InvariantCulture, "{0}{1}Exiting {2}.{3}", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_CurrentMethod.DeclaringType.Name, m_CurrentMethod.Name }));
			m_Tracer.Trace(TraceLevel.Verbose, "MethodTracer", string.Format(CultureInfo.InvariantCulture, "{0}{1}(Elapsed time: {2} seconds ({3} ms){4})", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_StopWatch.Elapsed.TotalSeconds.ToString("00.000", CultureInfo.InvariantCulture), m_StopWatch.ElapsedMilliseconds, fromDisposeMessage }));
			m_Tracer.Trace(TraceLevel.Verbose, "MethodTracer", scopeDeepth + "<--");

		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="MethodTracer"/> is reclaimed by garbage collection.
		/// </summary>
		~MethodTracer()
		{
			Dispose(false);
		}
	}


}
