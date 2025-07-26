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
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.Messaging
{
	/// <summary>
	/// Try Catch Finally Execution Helper with logging and retry feature.
	/// </summary>
	public static partial class ExecuteHelper
	{
		
		#region TryCatch & TryCatchLog with BoolMessageData result

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <returns>The result message</returns>
		public static BoolMessageData TryCatchReturnMessage(Func<BoolMessageData> action)
		{
			return TryCatchLogReturnMessage(action, LogMethod.None, ErrorDataType.Error);
		}

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <returns>The result message</returns>
		public static BoolMessageData TryCatchLogReturnMessage(Func<BoolMessageData> action)
		{
			return TryCatchLogReturnMessage(action, LogMethod.Debug, ErrorDataType.Error);
		}

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="action">The action that should be executed.</param>
		/// <param name="catchLogMethod">The log method used for error logging.</param>
		/// <returns>The result message</returns>
		public static BoolMessageData TryCatchLogReturnMessage(Func<BoolMessageData> action, LogMethod catchLogMethod)
		{
			return TryCatchLogReturnMessage(action, catchLogMethod, ErrorDataType.Error);
		}

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="tryAction">The action that should be executed.</param>
		/// <param name="catchLogMethod">The log method used for error logging.</param>
		/// <param name="catchErrorType">The error data type of the result message if catch was called.</param>
		/// <returns>The result message</returns>
		public static BoolMessageData TryCatchLogReturnMessage(Func<BoolMessageData> tryAction, LogMethod catchLogMethod, ErrorDataType catchErrorType)
		{
			ArgChecker.ShouldNotBeNull(tryAction, "tryAction");

			BoolMessageData result;
			try
			{
				result = tryAction();
			}
			catch (Exception ex)
			{
				QuickLogger.Log(ex.Message);
				
				if (ex.IsFatal())
					throw;

				result = new BoolMessageData(false, ex.Message, ex);
				result.SetErrorData(ErrorData.Create(ex, catchErrorType));
			}
			return result;
		}

		/// <summary>
		/// Executes an action inside a try catch and logs any exceptions.
		/// </summary>
		/// <param name="tryAction">The action that should be executed.</param>
		/// <param name="catchCallback">The error callback function.</param>
		/// <returns>The result message</returns>
		public static BoolMessageData TryCatchReturnMessage(Func<BoolMessageData> tryAction, Func<Exception, BoolMessageData> catchCallback)
		{
			BoolMessageData result;
			try
			{
				result = tryAction();
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;

				result = catchCallback.SafeExecute(ex);
			}
			return result;
		}


		/// <summary>
		/// Safe error callback execution.
		/// </summary>
		/// <param name="callback">The error callback.</param>
		/// <param name="exception">The exception.</param>
		/// <returns>The result message.</returns>
		private static BoolMessageData SafeExecute(this Func<Exception, BoolMessageData> callback, Exception exception)
		{
			BoolMessageData result = BoolMessageData.False;

			if (exception.IsNull())
				return result;

			if (callback.IsNull())
			{
				result = new BoolMessageData(false, exception.Message, exception);
				result.SetErrorData(ErrorData.Create(exception));
				return result;
			}

			try
			{
				result = callback(exception);

				if ((result == null)||(result.Success))
				{
					result = new BoolMessageData(false, exception.Message, exception);
					result.SetErrorData(ErrorData.Create(exception));
				}
			}
			catch (Exception catchCallBackException)
			{
				if (catchCallBackException.IsFatal())
					throw;

				result = new BoolMessageData(false, exception.Message, exception);
				result.SetErrorData(ErrorData.Create(exception));
			}

			return result;
		}

		#endregion TryCatch & TryCatchLog with BoolMessageData result

	}
}
