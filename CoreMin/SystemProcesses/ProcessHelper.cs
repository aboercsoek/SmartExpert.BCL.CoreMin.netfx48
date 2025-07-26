//--------------------------------------------------------------------------
// File:    ProcessHelper.cs
// Content:	Implementation of class ProcessHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using SmartExpert.Error;

#endregion

namespace SmartExpert.SystemProcesses
{
	///<summary>Provides common <see cref="Process"/> helper methods.</summary>
	public static class ProcessHelper
	{
		/// <summary>
		/// Gets the unique identifier for the associated process..
		/// </summary>
		/// <remarks>SafeGetProcessId calls <see cref="Process.Id"/>, but throws no Exception if the execution fails.</remarks>
		/// <param name="process">The process.</param>
		/// <returns>The process id on success; otherwise -1</returns>
		public static int SafeGetProcessId(Process process)
		{
			if (process == null)
				return -1;

			try
			{
				return process.Id;
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
			}

			return -1;
		}

		/// <summary>
		/// Gets the name of the process.
		/// </summary>
		/// <remarks>SafeGetProcessName calls <see cref="Process.ProcessName"/>, but throws no Exception if the execution fails.</remarks>
		/// <param name="process">The process component.</param>
		/// <returns>The name of the process on success; otherwise an empty string.</returns>
		public static string SafeGetProcessName(Process process)
		{
			if (process == null)
				return string.Empty;

			try
			{
				return process.ProcessName;
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
			}

			return string.Empty;
		}

		/// <summary>
		/// Discards any information about the associated process that has been cached inside the process component.
		/// </summary>
		/// <remarks>SafeRefresh calls <see cref="Process.Refresh"/>, but throws no Exception if the execution fails.</remarks>
		/// <param name="process">The process component to refresh.</param>
		public static void SafeRefresh(Process process)
		{
			if (process == null)
				return;

			try
			{
				process.Refresh();
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
			}
		}


		/// <summary>
		/// Gets the amount of physical memory allocated for the current process.
		/// </summary>
		/// <returns>The amount of physical memory, in bytes, allocated for the current process.</returns>
		public static long GetCurrentProcessWorkingSetMemory()
		{
			Process currentProcess = null;
			try
			{
				currentProcess = Process.GetCurrentProcess();
				return currentProcess.WorkingSet64;
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if (currentProcess != null)
					currentProcess.Dispose();
			}

			return -1;
		}

		/// <summary>
		/// Gets the managed memory amount in bytes allocated by the current process.
		/// </summary>
		/// <returns>A number that is the best available approximation of the number of bytes currently allocated in managed memory.</returns>
		public static long GetCurrentProcessAllocatedManagedMemory()
		{
			return GetCurrentProcessAllocatedManagedMemory(true);
		}

		/// <summary>
		/// Gets the managed memory amount in bytes allocated by the current process.
		/// </summary>
		/// <param name="forceFullGarbageCollection"><see langword="true"/> to indicate that this method can wait for garbage collection to occur before returning; otherwise, <see langword="false"/>.</param>
		/// <returns>A number that is the best available approximation of the number of bytes currently allocated in managed memory.</returns>
		public static long GetCurrentProcessAllocatedManagedMemory(bool forceFullGarbageCollection)
		{
			return GC.GetTotalMemory(forceFullGarbageCollection);
		}

		/// <summary>
		/// Gets the name of the current process.
		/// </summary>
		public static string GetCurrentProcessName()
		{
			string result = string.Empty;

			Process currentProcess = null;
			try
			{
				currentProcess = Process.GetCurrentProcess();
				result = SafeGetProcessName(currentProcess);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if (currentProcess != null)
					currentProcess.Dispose();
			}

			return result;
		}

		/// <summary>
		/// Gets the the process id and process name of the current process as string.
		/// </summary>
		public static string GetCurrentProcessString()
		{
			string result = string.Empty;

			Process currentProcess = null;
			try
			{
				currentProcess = Process.GetCurrentProcess();
				int processId = SafeGetProcessId(currentProcess);
				string processName = SafeGetProcessName(currentProcess);
				result = "[Process: ID={0}|Name={1}]".SafeFormatWith(processId, processName);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if (currentProcess != null)
					currentProcess.Dispose();
			}

			return result;
		}

		/// <summary>
		/// Gets the the process id, process name, thread id and thread name of the current process and thread as string.
		/// </summary>
		public static string GetCurrentProcessAndThreadString()
		{
			string result = string.Empty;

			Process currentProcess = null;
			try
			{
				currentProcess = Process.GetCurrentProcess();
				int processId = SafeGetProcessId(currentProcess);
				string processName = SafeGetProcessName(currentProcess);
				result = "[Process: ID={0}|Name={1}] ".SafeFormatWith(processId, processName);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			finally
			{
				if (currentProcess != null)
					currentProcess.Dispose();
			}
			try
			{
				Thread currentThread = Thread.CurrentThread;
				int threadId = currentThread.ManagedThreadId;
				string threadName = currentThread.Name;
				result += "[Thread: ID={0}|Name={1}]".SafeFormatWith(threadId, threadName);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
			return result;
		}

		/// <summary>
		/// Runs a process in a different user context.
		/// </summary>
		/// <param name="appFilePath">The application file path.</param>
		/// <param name="arguments">The command line arguments.</param>
		/// <param name="loadUserProfile">If set to <see langword="true"/> the user profile will be loaded on process start.</param>
		/// <param name="domain">The domain.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="waitForExitInSeconds">The wait-for-exit timeout in seconds.</param>
		/// <returns>The process exit code or 0 if waitForExitInSeconds is &lt; 0.</returns>
		/// <remarks>For infos how <paramref name="waitForExitInSeconds"/> influences the process live time see the <see cref="Run"/> remarks.</remarks>
		public static int RunAs(string appFilePath, string arguments, bool loadUserProfile, string domain, string username, string password, int waitForExitInSeconds)
		{
			ArgChecker.ShouldNotBeNull(appFilePath, "username");
			ArgChecker.ShouldBeExistingFile(appFilePath, "appFilePath");
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(password, "password");

			int exitCode;
			SecureString securePassword = password.ToSecureString();
			try
			{
				var si = new ProcessStartInfo
				{
					FileName = appFilePath,
					Arguments = (arguments.IsNullOrEmptyWithTrim()) ? string.Empty : arguments,
					WorkingDirectory = Path.GetDirectoryName(appFilePath),
					UseShellExecute = false,
					LoadUserProfile = loadUserProfile,
					Domain = (domain.IsNullOrEmptyWithTrim()) ? null : domain,
					UserName = username,
					Password = securePassword
				};

				exitCode = Run(si, waitForExitInSeconds);
			}
			finally
			{
				securePassword.Dispose();
			}

			return exitCode;
		}

		/// <summary>
		/// Executes a Process.
		/// </summary>
		/// <param name="startInfo">The process start info.</param>
		/// <param name="waitForExitInSeconds">The time out in seconds.</param>
		/// <remarks>
		/// <para>If waitForExitInSeconds is &lt; 0 Run does not wait for process end.</para>
		/// <para>If waitForExitInSeconds is 0 Run waits till the process has ended.</para>
		/// <para>If waitForExitInSeconds is &gt; 0 Run waits up to waitForExitInSeconds for process end. 
		/// If the process is still running after that time Run will kill the process.</para>
		/// </remarks>
		/// <returns>The exit code if waitForExitInSeconds is &gt;= 0, or 0 if waitForExitInSeconds is &lt; 0.</returns>
		public static int Run(ProcessStartInfo startInfo, int waitForExitInSeconds)
		{
			ArgChecker.ShouldNotBeNull(startInfo, "startInfo");

			int exitCode;
			try
			{
				using (var process = Process.Start(startInfo))
				{
					if (process.IsNull())
						return -1;

					if (waitForExitInSeconds < 0)
						return 0;

					if (waitForExitInSeconds == 0)
					{
						process.WaitForExit();
					}
					else if (process.WaitForExit(waitForExitInSeconds * 1000) == false)
					{
						process.Kill();
					}

					exitCode = process.ExitCode;
				}
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
				exitCode = -1;
			}

			return exitCode;
		}

	}
}
