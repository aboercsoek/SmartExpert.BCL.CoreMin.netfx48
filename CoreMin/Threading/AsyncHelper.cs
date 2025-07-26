//--------------------------------------------------------------------------
// File:    AsyncHelper.cs
// Content:	Implementation of class AsyncHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.Threading
{
	///<summary>Asynchronous execution helper class</summary>
	public static class AsyncHelper
	{
		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <param name="action">The action.</param>
		public static void CallAsync(Action action)
		{
			CallAsync(action, (string)null);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="Action{T}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam">The parameter for the action method</param>
		public static void CallAsync<T>(Action<T> action, T actionParam)
		{
			CallAsync(action, actionParam, (string)null);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <see cref="Action{T1,T2}"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <see cref="Action{T1,T2}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		public static void CallAsync<T1, T2>(Action<T1, T2> action, T1 actionParam1, T2 actionParam2)
		{
			CallAsync(action, actionParam1, actionParam2, (string)null);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <typeparam name="T3">The type of the third <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		/// <param name="actionParam3">The third parameter for the action method</param>
		public static void CallAsync<T1, T2, T3>(Action<T1, T2, T3> action, T1 actionParam1, T2 actionParam2, T3 actionParam3)
		{
			CallAsync(action, actionParam1, actionParam2, actionParam3, (string)null);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="actionInvocationErrorLogContext">The action invocation error log context.</param>
		public static void CallAsync(Action action, string actionInvocationErrorLogContext)
		{
			action.BeginInvoke(ar =>
			{
				var a = (Action)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (actionInvocationErrorLogContext.IsNotNull())
						QuickLogger.Log(ex.RenderExceptionDetails());
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="Action{T}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam">The parameter for the action method</param>
		/// <param name="actionInvocationErrorLogContext">The action invocation error log context.</param>
		public static void CallAsync<T>(Action<T> action, T actionParam, string actionInvocationErrorLogContext)
		{
			action.BeginInvoke(actionParam, ar =>
			{
				var a = (Action<T>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (actionInvocationErrorLogContext.IsNotNull())
						QuickLogger.Log(ex.RenderExceptionDetails());
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <see cref="Action{T1,T2}"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <see cref="Action{T1,T2}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		/// <param name="actionInvocationErrorLogContext">The action invocation error log context.</param>
		public static void CallAsync<T1, T2>(Action<T1, T2> action, T1 actionParam1, T2 actionParam2, string actionInvocationErrorLogContext)
		{
			action.BeginInvoke(actionParam1, actionParam2, ar =>
			{
				var a = (Action<T1,T2>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (actionInvocationErrorLogContext.IsNotNull())
						QuickLogger.Log(ex.RenderExceptionDetails());
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <typeparam name="T3">The type of the third <see cref="Action{T1,T2,T3}"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		/// <param name="actionParam3">The third parameter for the action method</param>
		/// <param name="actionInvocationErrorLogContext">The action invocation error log context.</param>
		public static void CallAsync<T1, T2, T3>(Action<T1, T2, T3> action, T1 actionParam1, T2 actionParam2, T3 actionParam3, string actionInvocationErrorLogContext)
		{
			action.BeginInvoke(actionParam1, actionParam2, actionParam3, ar =>
			{
				var a = (Action<T1, T2, T3>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (actionInvocationErrorLogContext.IsNotNull())
						QuickLogger.Log(ex.RenderExceptionDetails());
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="action"/> throws an exception.</param>
		public static void CallAsync(Action action, Action<Exception> catchHandler)
		{
			action.BeginInvoke(ar =>
			{
				var a = (Action)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="function"/> asyncronous.
		/// </summary>
		/// <typeparam name="T">The type of the <paramref name="function"/> result.</typeparam>
		/// <param name="function">The function delegate.</param>
		/// <param name="resultReceiver">The result action.</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="function"/> throws an exception.</param>
		public static void CallAsync<T>(Func<T> function, Action<T> resultReceiver, Action<Exception> catchHandler)
		{
			function.BeginInvoke(ar =>
			{
				var a = (Func<T>)ar.AsyncState;
				try
				{
					var result = a.EndInvoke(ar);
					resultReceiver(result);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, function);
		}

		/// <summary>
		/// Calls the <paramref name="function"/> asyncronous.
		/// </summary>
		/// <typeparam name="TResult">The type of the <paramref name="function"/> result.</typeparam>
		/// <typeparam name="TParam">The type of the <paramref name="function"/> argument.</typeparam>
		/// <param name="function">The function delegate.</param>
		/// <param name="funcParam">The parameter for the function method</param>
		/// <param name="resultReceiver">The result action.</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="function"/> throws an exception.</param>
		public static void CallAsync<TParam, TResult>(Func<TParam, TResult> function, TParam funcParam, Action<TResult> resultReceiver, Action<Exception> catchHandler)
		{
			function.BeginInvoke(funcParam, ar =>
			{
				var a = (Func<TParam, TResult>)ar.AsyncState;
				try
				{
					var result = a.EndInvoke(ar);
					resultReceiver(result);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, function);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T">The type of the <paramref name="action"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam">The parameter for the action method</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="action"/> throws an exception.</param>
		public static void CallAsync<T>(Action<T> action, T actionParam, Action<Exception> catchHandler)
		{
			action.BeginInvoke(actionParam, ar =>
			{
				var a = (Action<T>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <paramref name="action"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <paramref name="action"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="action"/> throws an exception.</param>
		public static void CallAsync<T1, T2>(Action<T1, T2> action, T1 actionParam1, T2 actionParam2, Action<Exception> catchHandler)
		{
			action.BeginInvoke(actionParam1, actionParam2, ar =>
			{
				var a = (Action<T1, T2>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, action);
		}

		/// <summary>
		/// Calls the <paramref name="action"/> asyncronous.
		/// </summary>
		/// <typeparam name="T1">The type of the first <paramref name="action"/> argument.</typeparam>
		/// <typeparam name="T2">The type of the second <paramref name="action"/> argument.</typeparam>
		/// <typeparam name="T3">The type of the third <paramref name="action"/> argument.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="actionParam1">The first parameter for the action method</param>
		/// <param name="actionParam2">The second parameter for the action method</param>
		/// <param name="actionParam3">The third parameter for the action method</param>
		/// <param name="catchHandler">The catch handler action that should be called if <paramref name="action"/> throws an exception.</param>
		public static void CallAsync<T1, T2, T3>(Action<T1, T2, T3> action, T1 actionParam1, T2 actionParam2, T3 actionParam3, Action<Exception> catchHandler)
		{
			action.BeginInvoke(actionParam1, actionParam2, actionParam3, ar =>
			{
				var a = (Action<T1, T2, T3>)ar.AsyncState;
				try
				{
					a.EndInvoke(ar);
				}
				catch (Exception ex)
				{
					if (catchHandler.IsNotNull()) catchHandler(ex);
				}
			}, action);
		}
	}

}


