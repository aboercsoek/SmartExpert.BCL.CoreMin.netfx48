//--------------------------------------------------------------------------
// File:    ServiceControlManagerEx.cs
// Content:	Implementation of class ServiceControlManagerEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.SystemServices
{
	///<summary>ServiceControlManagerEx provides helper methods for the work with windows services.</summary>
	public static class ServiceControlManagerEx
	{
		/// <summary>
		/// Check if Service exist.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <returns><see langword="true"/> if a service with the specified name exists, otherwise <see langword="false"/>.</returns>
		public static bool ServiceExists(string serviceName)
		{
			bool serviceExists = false;
			IntPtr svcHandle = Win32.NULL;
			IntPtr scmHandle = Win32.NULL;
			try
			{
				//scmHandle = UnsafeNativeMethods.OpenSCManager(null, null, 0x20000);
				scmHandle = AdvApi32.OpenSCManager(null, null, AdvApi32.ServiceControlManagerAccessRights.SC_MANAGER_ENUMERATE_SERVICE);
				if (scmHandle != Win32.NULL)
				{
					svcHandle = AdvApi32.OpenService(scmHandle, serviceName, AdvApi32.ServiceAccessRights.SERVICE_QUERY_CONFIG);
				}
				serviceExists = (svcHandle != Win32.NULL);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if (svcHandle != Win32.NULL)
					AdvApi32.CloseServiceHandle(svcHandle);

				if (scmHandle != Win32.NULL)
					AdvApi32.CloseServiceHandle(scmHandle);
			}
			return serviceExists;
		}


		/// <summary>
		/// Sets the service startup type.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <param name="startupType">Startup type of the service.</param>
		public static void SetServiceStartupType(string serviceName, int startupType)
		{
			SetServiceStartupType(serviceName, (ServiceStartType)startupType);
		}

		/// <summary>
		/// Sets the service startup type.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <param name="startupType">Startup type of the service.</param>
		public static void SetServiceStartupType(string serviceName, ServiceStartType startupType)
		{
			IntPtr scmHandle = AdvApi32.OpenSCManager(null, null, AdvApi32.ServiceControlManagerAccessRights.SC_MANAGER_ALL_ACCESS);
			if (scmHandle == Win32.NULL)
			{
				int win32ErrorCode = Marshal.GetLastWin32Error();
				throw new Win32ExecutionException(win32ErrorCode,
					"OpenSCManager Error: {1}".SafeFormatWith(serviceName, Win32Helper.GetWin32ErrorMessage(win32ErrorCode)));
			}
			try
			{
				IntPtr svcHandle = AdvApi32.OpenService(scmHandle, serviceName,
					AdvApi32.ServiceAccessRights.SERVICE_CHANGE_CONFIG | AdvApi32.ServiceAccessRights.SERVICE_QUERY_CONFIG);

				if (svcHandle == Win32.NULL)
				{
					int win32ErrorCode = Marshal.GetLastWin32Error();
					throw new Win32ExecutionException(win32ErrorCode,
						"OpenService({0}) Error: {1}".SafeFormatWith(serviceName, Win32Helper.GetWin32ErrorMessage(win32ErrorCode)));
				}
				try
				{
					var lpServiceConfig = new AdvApi32.ServiceConfig();
					int cbBytesNeeded;
					if (!AdvApi32.QueryServiceConfig(svcHandle, lpServiceConfig, Marshal.SizeOf(lpServiceConfig), out cbBytesNeeded))
					{
						int win32ErrorCode = Marshal.GetLastWin32Error();
						if (win32ErrorCode != 0x7a)
						{
							throw new Win32ExecutionException(win32ErrorCode,
								"QueryServiceConfig({0}) Error: {1}".SafeFormatWith(serviceName, Win32Helper.GetWin32ErrorMessage(win32ErrorCode)));
						}
					}

					if (lpServiceConfig.dwStartType == (int)startupType)
						return;

					if (!AdvApi32.ChangeServiceConfig(svcHandle, AdvApi32.ServiceType.SERVICE_NO_CHANGE, startupType,
																 AdvApi32.ServiceErrorControlType.SERVICE_NO_CHANGE, null, null,
																 Win32.NULL, null, null, null, null))
					{
						int win32ErrorCode = Marshal.GetLastWin32Error();
						throw new Win32ExecutionException(win32ErrorCode,
							"ChangeServiceConfig({0}) Error: {1}".SafeFormatWith(serviceName, Win32Helper.GetWin32ErrorMessage(win32ErrorCode)));
					}
				}
				finally
				{
					AdvApi32.CloseServiceHandle(svcHandle);
					svcHandle = Win32.NULL;
				}
			}
			finally
			{
				AdvApi32.CloseServiceHandle(scmHandle);
				scmHandle = Win32.NULL;
			}
		}

		/// <summary>
		/// Starts the service.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		public static void StartService(string serviceName)
		{
			//ULS.SendTraceTag(ULSTagID.tag_0000, ULSCat.msoulscat_WSS_General, ULSTraceLevel.Verbose, "Entered SPAdvApi32.StartService(%s)", new object[] { strServiceName });

			if (ServiceExists(serviceName))
			{
				var controller = new ServiceController(serviceName);
				var timeout = new TimeSpan(0, 0, 90);
				switch (controller.Status)
				{
					case ServiceControllerStatus.Stopped:
						controller.Start();
						break;

					case ServiceControllerStatus.StopPending:
						controller.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
						controller.Start();
						break;

					case ServiceControllerStatus.Running:
						return;

					case ServiceControllerStatus.PausePending:
						controller.WaitForStatus(ServiceControllerStatus.Paused, timeout);
						controller.Continue();
						break;

					case ServiceControllerStatus.Paused:
						controller.Continue();
						break;
				}

				controller.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}

			//ULS.SendTraceTag(ULSTagID.tag_0000, ULSCat.msoulscat_WSS_General, ULSTraceLevel.Verbose, "Exiting SPAdvApi32.StartService");
		}

		/// <summary>
		/// Stops the service.
		/// </summary>
		/// <param name="serviceName">Name of the service.</param>
		/// <returns></returns>
		public static bool StopService(string serviceName)
		{
			ServiceController controller;
			TimeSpan span;

			//ULS.SendTraceTag(ULSTagID.tag_0000, ULSCat.msoulscat_WSS_General, ULSTraceLevel.Verbose, "Entered SPAdvApi32.StopService(%s)", new object[] { strServiceName });
			if (ServiceExists(serviceName))
			{
				controller = new ServiceController(serviceName);
				span = new TimeSpan(0, 0, 90);
				switch (controller.Status)
				{
					case ServiceControllerStatus.Stopped:
						return true;

					case ServiceControllerStatus.StartPending:
						controller.WaitForStatus(ServiceControllerStatus.Running, span);
						controller.Stop();
						break;

					case ServiceControllerStatus.StopPending:
						break;

					case ServiceControllerStatus.Running:
						controller.Stop();
						break;

					case ServiceControllerStatus.ContinuePending:
						controller.WaitForStatus(ServiceControllerStatus.Running, span);
						controller.Stop();
						break;

					case ServiceControllerStatus.PausePending:
						controller.WaitForStatus(ServiceControllerStatus.Paused, span);
						controller.Stop();
						break;

					case ServiceControllerStatus.Paused:
						controller.Stop();
						break;
				}
			}
			else
			{
				return false;
			}

			controller.WaitForStatus(ServiceControllerStatus.Stopped, span);
			return true;
		}
	}
}
