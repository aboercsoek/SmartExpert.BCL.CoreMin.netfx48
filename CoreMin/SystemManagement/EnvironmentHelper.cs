//--------------------------------------------------------------------------
// File:    EnvironmentHelper.cs
// Content:	Implementation of class EnvironmentHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.SystemManagement
{
	///<summary>Environment Utility class that provides infos about the current system.</summary>
	public static class EnvironmentHelper
	{
		/// <summary>
		/// Gets environment variables the safe way (returns string.Empty if not found).
		/// </summary>
		/// <param name="name">The environment variable name to get.</param>
		/// <returns>
		/// Returns the safe environment variable value.
		/// </returns>
		public static string GetSafeEnvironmentVariable(string name)
		{
			try
			{
				string environmentVariable = Environment.GetEnvironmentVariable(name);
				return string.IsNullOrEmpty(environmentVariable) ? string.Empty : environmentVariable;
			}
			catch (SecurityException)
			{
				return string.Empty;
			}
		}

		#region current Machine und User related helper methods.

		/// <summary>
		/// Name of the machine running the app
		/// </summary>
		public static string MachineName
		{
			get { return System.Environment.MachineName; }
		}

		/// <summary>
		/// Gets the user name that the app is running under
		/// </summary>
		public static string UserName
		{
			get { return System.Environment.UserName; }
		}

		/// <summary>
		/// Name of the domain that the app is running under
		/// </summary>
		public static string DomainName
		{
			get { return System.Environment.UserDomainName; }
		}

		#endregion

		#region current OS related helper methods.

		/// <summary>
		/// Name of the OS running
		/// </summary>
		public static string OSName
		{
			get { return System.Environment.OSVersion.Platform.ToString(); }
		}

		/// <summary>
		/// Version information about the OS running
		/// </summary>
		public static string OSVersion
		{
			get { return System.Environment.OSVersion.Version.ToString(); }
		}

		/// <summary>
		/// The service pack running on the OS
		/// </summary>
		public static string OSServicePack
		{
			get { return System.Environment.OSVersion.ServicePack; }
		}

		/// <summary>
		/// Full name, includes service pack, version, etc.
		/// </summary>
		public static string OSFullName
		{
			get { return System.Environment.OSVersion.VersionString; }
		}

		/// <summary>
		/// Is the current OS a 64 bit version.
		/// </summary>
		/// <returns>Returns true if the current OS is 64 bit; otherwise false.</returns>
		public static bool Is64Bit
		{
			get { return (Marshal.SizeOf(typeof(IntPtr)) == 8); }
		}

		/// <summary>
		/// Is the current OS a 32 bit version.
		/// </summary>
		/// <returns>Returns true if the current OS is 32 bit; otherwise false.</returns>
		public static bool Is32Bit
		{
			get { return (Marshal.SizeOf(typeof(IntPtr)) == 4); }
		}

		/// <summary>
		/// Is the current in the 32 bit WOW mode.
		/// </summary>
		/// <returns>Returns true if the current system is in the 32 bit WOW mode; otherwise false.</returns>
		public static bool Is32BitWow
		{
			get
			{
				return (Is32Bit && IsWowEnvironmentVariableSet);
			}
		}

		private static bool IsWowEnvironmentVariableSet
		{
			get
			{
				if (!m_isWowEnvironmentVariableSet.HasValue)
				{
					m_isWowEnvironmentVariableSet = new bool?(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")));
				}
				return m_isWowEnvironmentVariableSet.Value;
			}
		}

		private static bool? m_isWowEnvironmentVariableSet;

		#endregion

		#region Processor, Memory related helper methods

		/// <summary>
		/// Gets the current stack trace information
		/// </summary>
		public static string StackTrace
		{
			get { return System.Environment.StackTrace; }
		}

		/// <summary>
		/// Returns the number of processors on the machine
		/// </summary>
		public static int NumberOfProcessors
		{
			get { return System.Environment.ProcessorCount; }
		}

		/// <summary>
		/// The total amount of memory the GC believes is used by the application in bytes
		/// </summary>
		public static long TotalMemoryUsed
		{
			get { return GC.GetTotalMemory(true); }
		}

		/// <summary>
		/// The total amount of memory that is available in bytes
		/// </summary>
		public static long TotalPhysicalMemory
		{
			get
			{
				var wmiQuery = new ObjectQuery("SELECT * FROM Win32_LogicalMemoryConfiguration");
				var searcher = new ManagementObjectSearcher(wmiQuery);
				foreach ( ManagementObject wmiObject in searcher.Get() )
				{
					return long.Parse(wmiObject["TotalPhysicalMemory"].ToString()) * 1024;
				}
				return 0;
			}
		}

		#endregion
	}
}
