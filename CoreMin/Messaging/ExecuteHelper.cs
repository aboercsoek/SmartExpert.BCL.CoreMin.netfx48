//--------------------------------------------------------------------------
// File:    ExecuteHelper.cs
// Content:	Implementation of class ExecuteHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives
using System;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Logging;
using SmartExpert.CUI;

#endregion

namespace SmartExpert.Messaging
{
	/// <summary>
	/// Try Catch Finally Execution Helper with logging and retry feature.
	/// </summary>
	public static partial class ExecuteHelper
	{
		#region RunWithRetry Methods

		/// <summary>
		/// Repeats the action call upto 3 times if the call fails with an exception.
		/// Waits 1 second between retry calls.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>true if the action call succeeded, or false if the action failed more than 3 times.</returns>
		public static bool RunWithRetry(Action action)
		{
			return RunWithRetry(action, delegate(Exception ex, int wait)
			{
				if (ex is ArgumentException)
				{
					ConsoleHelper.WriteLine(ex.ToString(), ConsoleColor.Red);
					QuickLogger.LogErrorMessage(ex);
				}
				Console.WriteLine(@"{0}, retry in {1} seconds", ex.RenderExceptionMessage(), wait);
				QuickLogger.Log(@"{0}, retry in {1} seconds".SafeFormatWith(ex.RenderExceptionMessage(), wait));
			});
		}

		/// <summary>
		/// Repeats the action call upto 3 times if the call fails with an exception.
		/// Waits 1 second between retry calls.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="waitHandler">The wait handler (called if the action call fails).</param>
		/// <returns>true if the action call succeeded, or false if the action failed more than 3 times.</returns>
		public static bool RunWithRetry(Action action, Action<Exception, int> waitHandler)
		{
			return RunWithRetry(action, 3, 1, 3, waitHandler);
		}

		/// <summary>
		/// Trys to call an action upto <paramref name="retryCount"/> times if the call fails.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="retryCount">The retry count.</param>
		/// <param name="wait">The wait in seconds between retries.</param>
		/// <param name="outerRetryCount">The outer retry count.</param>
		/// <param name="waitHandler">The wait handler.</param>
		/// <returns>true if the action call succeeded, or false if the action failed more than <paramref name="retryCount"/> times.</returns>
		public static bool RunWithRetry(Action action, int retryCount, int wait, int outerRetryCount, Action<Exception, int> waitHandler)
		{
			if (action == null)
				return false;

			int num = wait;
			for (int i = 0; i <= outerRetryCount; i++)
			{
				for (int j = 0; j <= retryCount; j++)
				{
					try
					{
						action();
						return true;
					}
					catch (Exception exception)
					{
						if (exception.IsFatal())
							throw;

						if (waitHandler != null)
						{
							TryCatch(() => waitHandler(exception, wait));
						}
						Thread.Sleep(num * 1000);
					}
				}
				num *= 2;
			}
			return false;

		}

		#endregion

		#region TryCatch & TryCatchHandle Methods

		/// <summary>
		/// Executes an action inside a try catch and does not do anything when an exception occurrs.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		public static void TryCatch(Action action)
		{
			//ArgChecker.ShouldNotBeNull(action, "action");
			if (action == null)
				return;

			try
			{
				action();
			}
			catch(Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <param name="operation">The operation that should be executed.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static void TryCatchSwallowIfNotFatal(Action operation)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");
			if (operation == null)
				return;

			try
			{
				operation();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <typeparam name="TOpArg">Operation parameter type.</typeparam>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="operationParameter">The parameter value passed to the operation.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static void TryCatchSwallowIfNotFatal<TOpArg>(Action<TOpArg> operation, TOpArg operationParameter)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");
			if (operation == null)
				return;

			try
			{
				operation(operationParameter);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <typeparam name="TOpArg1">First operation parameter type.</typeparam>
		/// <typeparam name="TOpArg2">Second operation parameter type.</typeparam>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="operationParameter1">The first parameter value passed to the operation.</param>
		/// <param name="operationParameter2">The second parameter value passed to the operation.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static void TryCatchSwallowIfNotFatal<TOpArg1, TOpArg2>(Action<TOpArg1, TOpArg2> operation, TOpArg1 operationParameter1, TOpArg2 operationParameter2)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");
			if (operation == null)
				return;

			try
			{
				operation(operationParameter1, operationParameter2);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <typeparam name="TOpArg1">First operation parameter type.</typeparam>
		/// <typeparam name="TOpArg2">Second operation parameter type.</typeparam>
		/// <typeparam name="TOpArg3">Thrid operation parameter type.</typeparam>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="operationParameter1">The first parameter value passed to the operation.</param>
		/// <param name="operationParameter2">The second parameter value passed to the operation.</param>
		/// <param name="operationParameter3">The third parameter value passed to the operation.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static void TryCatchSwallowIfNotFatal<TOpArg1, TOpArg2, TOpArg3>(Action<TOpArg1, TOpArg2, TOpArg3> operation, TOpArg1 operationParameter1, TOpArg2 operationParameter2, TOpArg3 operationParameter3)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");
			if (operation == null)
				return;

			try
			{
				operation(operationParameter1, operationParameter2, operationParameter3);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <typeparam name="TResult">Result type of the operation.</typeparam>
		/// <param name="operation">The operation that should be executed.</param>
		/// <returns>The result of <paramref name="operation"/> or TResult default if an exception occured during execution.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static TResult TryCatchSwallowIfNotFatal<TResult>(Func<TResult> operation)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");

			TResult result = default(TResult);
			try
			{
				if (operation != null)
					result = operation();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			return result;
		}

		/// <summary>
		/// Executes an operation inside a try catch block. Swallows every exception that is not <see cref="ExceptionHelper.IsFatal">fatal</see>.
		/// </summary>
		/// <typeparam name="TOpArg">Operation parameter type.</typeparam>
		/// <typeparam name="TResult">Result type of the operation.</typeparam>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="operationParameter">The parameter value passed to the operation.</param>
		/// <returns>The result of <paramref name="operation"/> or TResult default if an exception occured during execution.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="operation"/> is <see langword="null"/>.</exception>
		public static TResult TryCatchSwallowIfNotFatal<TOpArg, TResult>(Func<TOpArg, TResult> operation, TOpArg operationParameter)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");

			TResult result = default(TResult);
			try
			{
				if (operation != null)
					result = operation(operationParameter);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			return result;
		}

		/// <summary>
		/// Executes an action inside a try catch and calls 
		/// <see cref="HandleException"/> inside the catch block.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		public static void TryCatchHandle(Action action)
		{
			//ArgChecker.ShouldNotBeNull(action, "action");
			if (action == null)
				return;

			TryCatchHandle(action, HandleException);
		}

		/// <summary>
		/// Executes an action inside a try catch and calls the <paramref name="exceptionHandler"/> inside the catch block.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="exceptionHandler">The exception action handler.</param>
		public static void TryCatchHandle(Action action, Action<Exception> exceptionHandler)
		{
			//ArgChecker.ShouldNotBeNull(action, "action");

			try
			{
				action();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;

				if (exceptionHandler != null)
					exceptionHandler(ex);
			}
		}

		#endregion

		#region TryCatchLog Methods

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		public static void TryCatchLog(Action action)
		{
			TryCatchLog(action, null, LogContext.FAULT);
		}


		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		public static void TryCatchLog(Action action, string errorMessage)
		{
			TryCatchLog(action, errorMessage, LogContext.FAULT);
		}


		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="context">The logging service context.</param>
		public static void TryCatchLog(Action action, string errorMessage, string context)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				if (context != null)
				{
					if (errorMessage.IsNullOrEmptyWithTrim())
					{
						if (context == LogContext.FAULT)
							QuickLogger.LogErrorDetails(ex);
						else
							QuickLogger.LogErrorMessage(ex);
					}
					else
						QuickLogger.Log("ERROR: {0}".SafeFormatWith(errorMessage.SafeString()));
				}

				if (ex.IsFatal())
					throw;
			}
		}

		#endregion

		#region TryCatchFinally Methods

		/// <summary>
		/// Executes an action inside a try catch and does not do anything when
		/// an exception occurrs.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="finallyHandler">The finally action handler.</param>
		public static void TryCatchFinally(Action action, Action finallyHandler)
		{
			TryCatchFinallySafe(string.Empty, action, HandleException, finallyHandler);
		}

		
		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="exceptionHandler">The exception action handler.</param>
		/// <param name="finallyHandler">The finally action handler.</param>
		public static void TryCatchFinallySafe(string errorMessage, Action action, Action<Exception> exceptionHandler, Action finallyHandler)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				if (errorMessage.IsNullOrEmptyWithTrim().IsFalse())
					QuickLogger.Log("ERROR: {0}".SafeFormatWith(errorMessage));

				if (ex.IsFatal())
					throw;

				if (exceptionHandler != null)
					exceptionHandler(ex);
			}
			finally
			{
				if (finallyHandler != null)
				{
					TryCatchHandle(finallyHandler);
				}
			}
		}

		#endregion

		#region TryCatchLog<T> & TryCatchLogRethrow<T> Methods

		/// <summary>
		/// Executes an operation inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="operation">The operation that should be executed.</param>
		public static T TryCatchLog<T>(Func<T> operation)
		{
			return TryCatchLog(operation, null);
		}

		/// <summary>
		/// Executes an operation inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		public static T TryCatchLog<T>(Func<T> operation, string errorMessage)
		{
			return TryCatchLog(operation, errorMessage, LogContext.DIAGNOSTICS);
		}


		/// <summary>
		/// Executes an operation inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="logErrorContext">The logging service context.</param>
		public static T TryCatchLog<T>(Func<T> operation, string errorMessage, string logErrorContext)
		{
			return TryCatchLogRethrow(operation, errorMessage, logErrorContext, false);
		}


		/// <summary>
		/// Executes an operation inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="operation">The operation that should be executed.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="context">The logging service context.</param>
		/// <param name="rethrow">Rethrow a catched exception.</param>
		public static T TryCatchLogRethrow<T>(Func<T> operation, string errorMessage, string context, bool rethrow)
		{
			//ArgChecker.ShouldNotBeNull(operation, "operation");

			T result = default(T);
			try
			{
				if (operation != null)
					result = operation();
			}
			catch (Exception ex)
			{
				string errorMsg = (errorMessage.IsNullOrEmptyWithTrim()) ? ex.Message : errorMessage;
				if (context != null)
				{
					if (context == LogContext.FAULT)
						QuickLogger.LogErrorDetails(ex);
					else
						QuickLogger.Log("ERROR: {0}".SafeFormatWith(errorMsg));
						
				}

				if (ex.IsFatal())
					throw;

				if (rethrow) throw;
			}
			return result;
		}

		#endregion

		#region Basic Exception Handler Methods

		/// <summary>
		/// Handle the highest level application exception.
		/// </summary>
		/// <param name="ex">The exception to handle.</param>
		public static void HandleException(Exception ex)
		{
			if (ex == null)
				return;

			QuickLogger.LogErrorDetails(ex);
		}


		/// <summary>
		/// Handle the highest level application exception.
		/// </summary>
		/// <param name="ex">The exception to handle.</param>
		public static void HandleExceptionLight(Exception ex)
		{
			if (ex == null)
				return;

			QuickLogger.LogErrorMessage(ex);
		}

		#endregion

	}
}
