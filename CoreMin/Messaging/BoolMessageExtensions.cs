//--------------------------------------------------------------------------
// File:    BoolMessageExtensions.cs
// Content:	Implementation of BoolMessage extension class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


namespace SmartExpert.Messaging
{
	/// <summary>
	/// Extensions to the BoolMessage type.
	/// </summary>
	public static class BoolMessageExtensions
	{
		/// <summary>
		/// Convert the result to an exit code.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>Returns 0 if <see cref="BoolMessage.Success"/> is <see langword="true"/>; otherwise -1</returns>
		public static int AsExitCode(this BoolMessage msg)
		{
			return msg.Success ? 0 : -1;
		}

		/// <summary>
		/// Convert the result to an exit code.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>Returns 0 if <see cref="BoolMessage.Success"/> is <see langword="true"/>; otherwise -1</returns>
		public static int AsExitCode(this BoolMessageItem msg)
		{
			return msg.Success ? 0 : -1;
		}

		/// <summary>
		/// Convert the result to an exit code.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>Returns 0 if <see cref="BoolMessage.Success"/> is <see langword="true"/>; otherwise -1</returns>
		public static int AsExitCode(this BoolMessageData msg)
		{
			return msg.Success ? 0 : -1;
		}

		/// <summary>
		/// Gets the error result.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The error result or null if <paramref name="boolMessageData"/> has no error result object.</returns>
		public static ErrorData GetErrorData(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			return (ErrorData)boolMessageData.Data.TryGetPropertyValue("$(errorData)");
		}

		/// <summary>
		/// Sets the error result.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <param name="errorData">The error result.</param>
		public static void SetErrorData(this BoolMessageData boolMessageData, ErrorData errorData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");
			ArgChecker.ShouldNotBeNull(errorData, "errorData");

			boolMessageData.Data.SetProperty("$(errorData)", errorData);
		}

		/// <summary>
		/// Sets the error result.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <param name="exception">The exception to store as 'errorData' item.</param>
		public static void SetErrorData(this BoolMessageData boolMessageData, Exception exception)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");
			ArgChecker.ShouldNotBeNull(exception, "exception");

			ErrorData errorData = ErrorData.Create(exception);
			boolMessageData.Data.SetProperty("$(errorData)", errorData);
			boolMessageData.Message = errorData.ErrorText;
		}

		/// <summary>
		/// Gets the exception.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The exception stored inside the message or null if <paramref name="boolMessageData"/> has no stored exception object.</returns>
		public static Exception GetException(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			ErrorData errorData = (ErrorData)boolMessageData.Data.TryGetPropertyValue("$(errorData)");
			if (errorData != null)
				return errorData.Error;
			return (Exception)boolMessageData.Data.TryGetPropertyValue("$(errorException)");
		}

		/// <summary>
		/// Gets the exception text.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The exception text stored inside the message or null if <paramref name="boolMessageData"/> has no stored error data object.</returns>
		public static ExceptionText GetExceptionText(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			ErrorData errorData = (ErrorData)boolMessageData.Data.TryGetPropertyValue("$(errorData)");
			return errorData != null ? errorData.ExceptionText : null;
		}

		/// <summary>
		/// Sets the exception.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <param name="exception">The exception to store.</param>
		public static void SetException(this BoolMessageData boolMessageData, Exception exception)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");
			ArgChecker.ShouldNotBeNull(exception, "exception");

			boolMessageData.Data.SetProperty("$(errorException)", exception);
			boolMessageData.Message = exception.Message;
		}


		/// <summary>
		/// Add message In-Data.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <param name="inMsgData">The IN message data.</param>
		public static BoolMessageData AddMsgInData(this BoolMessageData boolMessageData, object inMsgData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");
			boolMessageData.Data.SetProperty("$(inMsgData)", inMsgData);
			return boolMessageData;
		}

		/// <summary>
		/// Add message Out-Data.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <param name="outMsgData">The OUT message data.</param>
		public static BoolMessageData AddMsgOutData(this BoolMessageData boolMessageData, object outMsgData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");
			boolMessageData.Data.SetProperty("$(outMsgData)", outMsgData);
			return boolMessageData;
		}

		/// <summary>
		/// Gets the message In-Data.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The IN message data or <see langword="null"/>.</returns>
		public static object GetMsgInData(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			return boolMessageData.Data.TryGetPropertyValue("$(inMsgData)");
		}

		/// <summary>
		/// Gets the message Out-Data.
		/// </summary>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The OUT message data or <see langword="null"/>.</returns>
		public static object GetMsgOutData(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			return boolMessageData.Data.TryGetPropertyValue("$(outMsgData)");
		}

		/// <summary>
		/// Gets the typed message In-Data.
		/// </summary>
		/// <typeparam name="TInData">Type of the IN message data object.</typeparam>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The IN message data or <see langword="null"/>.</returns>
		public static TInData GetMsgInData<TInData>(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			return boolMessageData.GetMsgInData().As<TInData>();
		}

		/// <summary>
		/// Gets the typed message Out-Data.
		/// </summary>
		/// <typeparam name="TOutData">Type of the OUT message data object.</typeparam>
		/// <param name="boolMessageData">The bool message object.</param>
		/// <returns>The OUT message data or <see langword="null"/>.</returns>
		public static TOutData GetMsgOutData<TOutData>(this BoolMessageData boolMessageData)
		{
			ArgChecker.ShouldNotBeNull(boolMessageData, "BoolMessageData");

			return boolMessageData.GetMsgOutData().As<TOutData>();
		}
	}
}
