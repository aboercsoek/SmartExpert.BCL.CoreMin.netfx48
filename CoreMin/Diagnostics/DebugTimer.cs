//--------------------------------------------------------------------------
// File:    DebugTimer.cs
// Content:	Implementation of static helper class DebugTimer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// Provides static helpers methods that measure time from Start() to Stop() and output the timespan to the logging service.
	/// Calls to DebugTimer are only compiled in debug builds. [Conditional("DEBUG")]
	/// </summary>
	public static class DebugTimer
	{
		[ThreadStatic]
		static Stack<Stopwatch> ms_StopWatches;

		/// <summary>
		/// Starts a new stopwatch.
		/// </summary>
		[Conditional("DEBUG")]
		public static void Start()
		{
			if (ms_StopWatches == null)
			{
				ms_StopWatches = new Stack<Stopwatch>();
			}
			ms_StopWatches.Push(Stopwatch.StartNew());
		}

		/// <summary>
		/// Stops the last started stopwatch.
		/// </summary>
		/// <param name="description">The timer description.</param>
		[Conditional("DEBUG")]
		public static void Stop(string description)
		{
			Stopwatch watch = ms_StopWatches.Pop();
			watch.Stop();
			Debug.Write("\"" + description + "\" took " + (watch.ElapsedMilliseconds) + " ms");
		}

		/// <summary>
		/// Starts a new stopwatch and stops it when the returned object is disposed.
		/// </summary>
		/// <returns>
		/// Returns disposable object that stops the timer (in debug builds); or null (in release builds).
		/// </returns>
		public static IDisposable Time(string text)
		{
#if DEBUG
			Stopwatch watch = Stopwatch.StartNew();
			return new DisposeMethodTrigger(
				() => {
					watch.Stop();
					Debug.Write("\"" + text + "\" took " + (watch.ElapsedMilliseconds) + " ms"); 
				} );
#else
			return null;
#endif
		}
	}
}
