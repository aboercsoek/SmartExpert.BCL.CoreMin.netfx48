//--------------------------------------------------------------------------
// File:    ErrorDataType.cs
// Content:	Implementation of class ErrorDataType
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


namespace SmartExpert.Messaging
{
	/// <summary>
	/// Error Data Type enum
	/// </summary>
	public enum ErrorDataType
	{
		/// <summary>Fatal Error Type => FaultManager: Log to file and EventLog, Send E-Mail Notification</summary>
		FatalError = 0,
		/// <summary>Error Type => FaultManager: Log to file and EventLog</summary>
		Error = 1,
		/// <summary>Argument Validation Error Type => FaultManager: Log to file</summary>
		ArgValidation = 2,
		/// <summary>Business Rule Validation Error Type => FaultManager: Log to audit log</summary>
		BusinessRule = 3,
		/// <summary>Warning Type => FaultManager: Log to file</summary>
		Warning = 4,
	}
}
