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
using System.Security;
using SmartExpert;
using SmartExpert.Linq;
using SmartExpert.Logging;
using SmartExpert.SystemProcesses;

#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// Provides method time measurement and trace output when used as parameter of a using-Statement.
	/// </summary>
	/// <example>
	/// <code>
	/// using (new InternalMethodTracer(MethodBase.GetCurrentMethod()))
	/// {
	///		// Code ...
	/// }
	/// </code>
	/// </example>
	internal class InternalMethodTracer : IDisposable
	{
		[ThreadStatic] private static int m_InstanceDeepth;
		private MethodBase m_CurrentMethod;
		private Stopwatch m_StopWatch;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodTracer"/> class.
		/// </summary>
		/// <param name="currentMethod">The current method.</param>
		public InternalMethodTracer(MethodBase currentMethod)
		{
			m_CurrentMethod = currentMethod;
			m_StopWatch = new Stopwatch();
			Enter();
		}

		/// <summary>
		/// Write Exit message to internal logger method OutputDebugString
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			m_StopWatch.Stop();
			Exit(disposing);
		}


		private void Enter()
		{
			string scopeDeepth = string.Empty;
			if (m_InstanceDeepth > 0)
				scopeDeepth= new string(' ',m_InstanceDeepth*3);
			QuickLogger.Log(scopeDeepth + "-->", false);

			QuickLogger.Log(string.Format(CultureInfo.InvariantCulture, "{0}{1}Entering {2}.{3}", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_CurrentMethod.DeclaringType.Name, m_CurrentMethod.Name }), false);
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

			QuickLogger.Log(string.Format(CultureInfo.InvariantCulture, "{0}{1}Exiting {2}.{3}", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_CurrentMethod.DeclaringType.Name, m_CurrentMethod.Name }), false);
			QuickLogger.Log(string.Format(CultureInfo.InvariantCulture, "{0}{1}(Elapsed time: {2} seconds ({3} ms){4})", new object[] { scopeDeepth, ProcessHelper.GetCurrentProcessAndThreadString(), m_StopWatch.Elapsed.TotalSeconds.ToString("00.000", CultureInfo.InvariantCulture), m_StopWatch.ElapsedMilliseconds, fromDisposeMessage }), false);
			QuickLogger.Log(scopeDeepth + "<--", false);
			
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="InternalMethodTracer"/> is reclaimed by garbage collection.
		/// </summary>
		~InternalMethodTracer()
		{
			Dispose(false);
		}
	}


}
