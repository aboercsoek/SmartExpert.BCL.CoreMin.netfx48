//--------------------------------------------------------------------------
// File:    ApplicationHelper.cs
// Content:	Implementation of class ApplicationHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Threading;
using System.Windows.Forms;
using SmartExpert.AppUtils;
using SmartExpert.Error;
using SmartExpert.Logging;
using SmartExpert.Web;

#endregion

namespace SmartExpert.WinForms
{
	///<summary>WinForms Application Helper</summary>
	public static class ApplicationHelper
	{
		#region Fault Handler Registrations

		/// <summary>
		/// Registers the critical fault handlers.
		/// </summary>
		public static void RegisterCriticalFaultHandlers()
		{
			if (WebHelper.IsWebHostingEnvironment.IsTrue()) return;

			Application.ThreadException += HandleThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
		}

		#endregion

		#region Fault Handlers

		/// <summary>
		/// Eventhandler nicht abgefangene Exceptions, die nicht auf GUI-Ebene auftraten.
		/// </summary>
		/// <param name="sender">Sender des Events</param>
		/// <param name="e">Informationen über nichtbehandelte Exception</param>
		static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				var ex = (Exception)e.ExceptionObject;
				var baseException = ex.As<BaseException>();
				if (baseException == null)
				{
					QuickLogger.Log(ex.RenderExceptionDetails());
					//AppContext.GetHostingAwareInstance().EventLogService.Error(ex, 1);
				}
				else
				{
					QuickLogger.Log(baseException.RenderExceptionDetails());
					//AppContext.GetHostingAwareInstance().EventLogService.Error(baseException, baseException.ErrorCode);
					
				}
			}
			catch (Exception exception)
			{
				try
				{
					if (exception.IsFatal())
						throw;

					MessageBox.Show(@"Fatal Error",
						@"Fatal Error. Application error during event log error reporting: "
						+ exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				finally
				{
					//AppContext.Shutdown();
					Application.Exit();
				}
			}
		}

		static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
		{
			var result = DialogResult.Cancel;
			try
			{
				result = ShowThreadExceptionDialog("Unhandled Thread Exception", e.Exception);
			}
			catch (Exception exception)
			{
				try
				{
					if (exception.IsFatal())
						throw;

					QuickLogger.Log(exception.RenderExceptionDetails());
					MessageBox.Show(@"Fatal Windows Forms Error",
						@"Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
				}
				finally
				{
					//AppContext.Shutdown();
					Application.Exit();
				}
			}

			if (result != DialogResult.Abort) return;

			//AppContext.Shutdown();
			Application.Exit();
		}

		static DialogResult ShowThreadExceptionDialog(string title, Exception e)
		{
			QuickLogger.Log(e.RenderExceptionDetails());

			string errorMsg = "A fatal error occured in application '{0}'. Error message: {1}\n\nStack Trace:\n{2}"
				.SafeFormatWith(AppHelper.ApplicationName, e.Message, e.StackTrace);

			return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
		}

		#endregion
	}
}
