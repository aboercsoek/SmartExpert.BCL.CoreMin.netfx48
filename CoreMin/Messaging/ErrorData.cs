//--------------------------------------------------------------------------
// File:    errorData.cs
// Content:	Implementation of class errorData
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
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Messaging
{
	///<summary>Error Data Type</summary>
	[DebuggerDisplay("ErrorData: {ToString()}")]
	public class ErrorData
	{
		/// <summary>
		/// <see cref="ErrorData"/> factory method.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		/// <remarks>
		/// Sets the <see cref="ErrorType"/> of the result to <see cref="F:SmartExpert.BCL35.Core.Messaging.ErrorDataType.Error"/>.
		/// The factory method tries to get the <see cref="ErrorCode"/> from the specified <paramref name="exception"/>. 
		/// If no error code could be retrieved from the exception, <see cref="ErrorCode"/> will be set to 0.
		/// </remarks>
		public static ErrorData Create(Exception exception)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			return Create(exception, ErrorDataType.Error);
		}

		/// <summary>
		/// <see cref="ErrorData"/> factory method.
		/// </summary>
		/// <param name="exceptions">The exceptions.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		/// <remarks>
		/// Sets the <see cref="ErrorType"/> of the result to <see cref="F:SmartExpert.BCL35.Core.Messaging.ErrorDataType.Error"/>.
		/// The factory method tries to get the <see cref="ErrorCode"/> from the specified <paramref name="exceptions"/>. 
		/// If no error code could be retrieved from the exception, <see cref="ErrorCode"/> will be set to 0.
		/// </remarks>
		public static ErrorData Create(IEnumerable<Exception> exceptions)
		{
			ArgChecker.ShouldNotBeNull(exceptions, "exceptions");

			CombinedException combinedException = new CombinedException("Multi exceptions occured.", exceptions);
			return Create(combinedException, ErrorDataType.Error);
		}

		/// <summary>
		/// <see cref="ErrorData"/> factory method for fatal errors.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		/// <remarks>
		/// Sets the <see cref="ErrorType"/> of the result to <see cref="F:SmartExpert.BCL35.Core.Messaging.ErrorDataType.Error"/>.
		/// The factory method tries to get the <see cref="ErrorCode"/> from the specified <paramref name="exception"/>. 
		/// If no error code could be retrieved from the exception, <see cref="ErrorCode"/> will be set to 0.
		/// </remarks>
		public static ErrorData CreateFatalErrorData(Exception exception)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			return Create(exception, ErrorDataType.FatalError);
		}

		/// <summary>
		/// <see cref="ErrorData"/> factory method for warnings.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		/// <remarks></remarks>
		/// <remarks>
		/// Sets the <see cref="ErrorType"/> of the result to <see cref="F:SmartExpert.BCL35.Core.Messaging.ErrorDataType.Error"/>.
		/// The factory method tries to get the <see cref="ErrorCode"/> from the specified <paramref name="exception"/>. 
		/// If no error code could be retrieved from the exception, <see cref="ErrorCode"/> will be set to 0.
		/// </remarks>
		public static ErrorData CreateWarningErrorData(Exception exception)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			return Create(exception, ErrorDataType.Warning);
		}

		/// <summary>
		/// <see cref="ErrorData"/> factory method.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="errorDataType">The error data type.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		/// <remarks>
		/// The factory method tries to get the <see cref="ErrorCode"/> from the specified <paramref name="exception"/>. 
		/// If no error code could be retrieved from the exception, <see cref="ErrorCode"/> will be set to 0.
		/// </remarks>
		public static ErrorData Create(Exception exception, ErrorDataType errorDataType)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			int errorCode = exception.As<BaseException>().IsNotNull() ? exception.As<BaseException>().ErrorCode : -1;

			return Create(exception, errorDataType, errorCode);
		}

		/// <summary>
		/// <see cref="ErrorData"/> factory method.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="errorDataType">The error data type.</param>
		/// <param name="errorCode">The error code.</param>
		/// <returns>The created <see cref="ErrorData"/> object.</returns>
		public static ErrorData Create(Exception exception, ErrorDataType errorDataType, int errorCode)
		{
			ArgChecker.ShouldNotBeNull(exception, "exception");

			ErrorData errorData = new ErrorData
									{
										Error = exception,
										ErrorCode = errorCode,
										ExceptionText = exception.RenderException(),
										ErrorText = exception.RenderExceptionSummary(),
										ErrorTime = DateTime.Now,
										ErrorType = errorDataType
									};

			return errorData;
		}

		/// <summary>
		/// Gets or sets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; set; }

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		/// <value>The error code.</value>
		public int ErrorCode { get; set; }

		/// <summary>
		/// Gets or sets the error type.
		/// </summary>
		/// <value>The error type.</value>
		public ErrorDataType ErrorType { get; set; }

		/// <summary>
		/// Gets or sets the error time.
		/// </summary>
		/// <value>The error time.</value>
		public DateTime ErrorTime { get; set; }

		/// <summary>
		/// Gets or sets the error text.
		/// </summary>
		/// <value>The error message.</value>
		public string ErrorText { get; set; }

		/// <summary>
		/// Gets or sets the exception text.
		/// </summary>
		/// <value>The exception text.</value>
		public ExceptionText ExceptionText { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return "{0} | {1} | {2} | {3}".SafeFormatWith(ErrorCode, ErrorType, ErrorTime, ErrorText);
		}
	}


}

