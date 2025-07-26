//--------------------------------------------------------------------------
// File:    ITracer.cs
// Content:	Definition of interface ITracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;

#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// Base contract for tracer implementations.
	/// </summary>
	/// <seealso cref="OutputDebugStringTracer"/> <seealso cref="ConsoleTracer"/>
	public interface ITracer
	{
		/// <summary>
		/// Gets the trace level.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The current trace level.</returns>
		TraceLevel GetTraceLevel( string context );

		/// <summary>
		/// Determines whether trace is enabled for a specified trace level.
		/// </summary>
		/// <param name="level">The trace level.</param>
		/// <param name="context">The context.</param>
		/// <returns>
		/// 	<see langword="true"/> if [is trace enabled] [the specified level]; otherwise, <see langword="false"/>.
		/// </returns>
		bool IsTraceEnabled( TraceLevel level, string context );

		/// <summary>
		/// Logs a message with a specific <see cref="TraceLevel"/>.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="context">The context.</param>
		/// <param name="text">The text.</param>
		void Trace( TraceLevel level, string context, string text );

		/// <summary>
		/// Logs a exception with a specific <see cref="TraceLevel"/>.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="context">The context.</param>
		/// <param name="text">The text.</param>
		/// <param name="exception">The exception.</param>
		void Trace( TraceLevel level, string context, string text, Exception exception );

	}
}
