//--------------------------------------------------------------------------
// File:    LogContext.cs
// Content:	Implementation of class LogContext
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Logging
{
	///<summary>Logging context constants.</summary>
	/// <remarks>These string constants can be used as context parameter values for the LogService and LogManager methods.</remarks>
	public class LogContext
	{
		#region BCL Framework releated Context Definitions

		/// <summary>
		/// Base Class Library log context 
		/// </summary>
		public const string BCL = "Framework";
		/// <summary>
		/// Internal log context 
		/// </summary>
		internal const string Internal = "Internal";
		/// <summary>
		/// Application Services log context
		/// </summary>
		public const string APP_SERVICES = "Application Services";
		/// <summary>
		/// Common Service log context
		/// </summary>
		public const string COMMON = "Common";


		#endregion

		#region Test related Context Definitions
	
		/// <summary>
		/// Test log context 
		/// </summary>
		public const string TEST = "Test";
		/// <summary>
		/// Example log context 
		/// </summary>
		public const string EXAMPLE = "Example";
		/// <summary>
		/// Evaluation log context 
		/// </summary>
		public const string EVAL = "Evaluation";

		#endregion

		#region Monitoring & Instrumentation related Context Definitions

		/// <summary>
		/// Performance log context
		/// </summary>
		public const string PERFORMANCE = "Performance";
		/// <summary>
		/// Health log context 
		/// </summary>
		public const string HEALTH = "Health";
		/// <summary>
		/// Health log context 
		/// </summary>
		public const string DIAGNOSTICS = "Diagnostics";

		#endregion

		#region Audit & Business related Context Definitions

		/// <summary>
		/// Audit log context 
		/// </summary>
		public const string AUDIT = "Audit";
		/// <summary>
		/// Report log context
		/// </summary>
		public const string REPORT = "Report";


		#endregion

		#region Error related Context Definitions

		/// <summary>
		/// Fault management log context
		/// </summary>
		public const string FAULT = "Fault Management";
		/// <summary>
		/// Notification log context
		/// </summary>
		public const string NOTIFICATION = "Notification";
		/// <summary>
		/// Validation log context
		/// </summary>
		public const string VALIDATION = "Validation";

		#endregion

		#region Layer related Context Definitions

		/// <summary>
		/// Config log context 
		/// </summary>
		public const string CONFIG = "Config";


		/// <summary>
		/// Client log context
		/// </summary>
		public const string CLIENT = "Client";

		/// <summary>
		/// Host log context
		/// </summary>
		public const string HOST = "Host";

		/// <summary>
		/// WinService Host log context
		/// </summary>
		public const string WIN_SVC_HOST = "Win Service Host";

		/// <summary>
		/// Web Service Host log context
		/// </summary>
		public const string WEB_SVC_HOST = "Web Service Host";

		/// <summary>
		/// Service log context
		/// </summary>
		public const string SERVICE = "Service";


		/// <summary>
		/// Process log context
		/// </summary>
		public const string PROCESS = "Process";

		/// <summary>
		/// Workflow log context
		/// </summary>
		public const string WORKFLOW = "Workflow";


		/// <summary>
		/// Data Service log context
		/// </summary>
		public const string DATA_SERVICE = "Data Service";

		/// <summary>
		/// Service Agent log context
		/// </summary>
		public const string SERVICE_AGENT = "Service Agent";

		/// <summary>
		/// Data Access log context
		/// </summary>
		public const string DATA_ACCESS = "Data Access";

		#endregion

	}
}
