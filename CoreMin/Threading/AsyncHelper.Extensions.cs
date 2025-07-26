//--------------------------------------------------------------------------
// File:    AsyncHelperExtensions.cs
// Content:	Implementation of class AsyncHelperExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------


#region Using directives

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.Threading
{
	/// <summary>
	/// Asynchronous action call extension methods
	/// </summary>
	public static class AsyncHelperExtensions
	{
		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <param name="action">The action.</param>
		public static void CallAsync(this Action action)
		{
			AsyncHelper.CallAsync(action, string.Empty);
		}

		/// <summary>
		/// Calls the async.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="actionInvocationErrorLogContext">The action invocation error log context.</param>
		public static void CallAsync(this Action action, string actionInvocationErrorLogContext)
		{
			AsyncHelper.CallAsync(action, actionInvocationErrorLogContext);
		}

		/// <summary>
		/// Calls the async.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="action"/> throws an exception.</param>
		public static void CallAsync(this Action action, Action<Exception> catchHandler)
		{
			AsyncHelper.CallAsync(action, catchHandler);
		}
	}
}


