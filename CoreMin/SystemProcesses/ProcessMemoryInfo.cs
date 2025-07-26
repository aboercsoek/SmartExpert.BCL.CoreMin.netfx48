//--------------------------------------------------------------------------
// File:    ProcessMemoryInfo.cs
// Content:	Implementation of class ProcessMemoryInfo
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.SystemProcesses
{
	/// <summary>
	/// Contains memory statistics for a process.
	/// </summary>
	[Serializable]
	public class ProcessMemoryInfo
	{
		#region Public Properties

		/// <summary>
		/// Gets the process information assoziated with the process memory info.
		/// </summary>
		public string ProcessNameAndId { get; private set; }
		/// <summary>
		/// Gets the process memory info timestamp.
		/// </summary>
		public DateTime Timestamp { get; private set; }
		
		/// <summary>
		/// Gets a value indicating whether this instance has valid memory infos.
		/// </summary>
		/// <value><see langword="true"/> if this instance has valid memory infos; otherwise, <see langword="false"/>.</value>
		public bool IsValidMemoryInfo { get; private set; }

		/// <summary>
		/// The number of page faults.
		/// </summary>
		public long PageFaultCount { get; private set; }
		
		/// <summary>
		/// The peak working set size, in bytes.
		/// </summary>
		public long PeakWorkingSetSize { get; private set; }
		/// <summary>
		/// The current working set size, in bytes.
		/// </summary>
		public long WorkingSetSize { get; private set; }
		
		/// <summary>
		/// The peak paged pool usage, in bytes.
		/// </summary>
		public long QuotaPeakPagedPoolUsage { get; private set; }
		/// <summary>
		/// The current paged pool usage, in bytes.
		/// </summary>
		public long QuotaPagedPoolUsage { get; private set; }
		
		/// <summary>
		/// The peak nonpaged pool usage, in bytes.
		/// </summary>
		public long QuotaPeakNonPagedPoolUsage { get; private set; }
		/// <summary>
		/// The current nonpaged pool usage, in bytes.
		/// </summary>
		public long QuotaNonPagedPoolUsage { get; private set; }
		
		/// <summary>
		/// The current space allocated for the pagefile, in bytes. Those pages may or may not be in memory.
		/// </summary>
		public long PagedMemorySize { get; private set; }
		/// <summary>
		/// The peak space allocated for the pagefile, in bytes.
		/// </summary>
		public long PeakPagedMemorySize { get; private set; }
		
		/// <summary>
		/// The current amount of memory that cannot be shared with other processes, in bytes. Private bytes include memory that is committed and marked MEM_PRIVATE, data that is not mapped, and executable pages that have been written to.
		/// </summary>
		public long PrivateMemorySize { get; private set; }

		/// <summary>
		/// The amount of virtual memory, in bytes, allocated for process.
		/// </summary>
		public long VirtualMemorySize { get; private set; }
		/// <summary>
		/// The maximum amount of virtual memory, in bytes, allocated for the associated process since it was started.
		/// </summary>
		public long PeakVirtualMemorySize { get; private set; }

		/// <summary>
		/// The amount of system memory, in bytes, allocated for the associated process that can be written to the virtual memory paging file.
		/// </summary>
		public long PagedSystemMemorySize { get; private set; }
		/// <summary>
		/// The amount of system memory, in bytes, allocated for the associated process that cannot be written to the virtual memory paging file.
		/// </summary>
		public long NonpagedSystemMemorySize { get; private set; }

		#endregion

		#region Public Static Create Methods

		/// <summary>
		/// Creates the process memory info for the specified process.
		/// </summary>
		/// <param name="process">The process.</param>
		/// <param name="description">The description text for the process memory info data.</param>
		/// <returns>The process memory info.</returns>
		public static ProcessMemoryInfo Create(Process process, string description)
		{
			var result = new ProcessMemoryInfo();
			ProcessMemoryInformation pmi;
			
			result.IsValidMemoryInfo = false;

			result.ProcessNameAndId = "[Process: ID={0}|Name={1}|Description={2}]"
				.SafeFormatWith(ProcessHelper.SafeGetProcessId(process), ProcessHelper.SafeGetProcessName(process), description);

			if (TryGetProcessMemoryInfo(process, out pmi))
			{
				result.Timestamp = DateTime.Now;
				result.PageFaultCount = pmi.PageFaultCount;
				
				result.PeakWorkingSetSize = process.PeakWorkingSet64;
				result.WorkingSetSize = process.WorkingSet64;
				
				result.QuotaPeakPagedPoolUsage = (uint)pmi.QuotaPeakPagedPoolUsage;
				result.QuotaPagedPoolUsage = (uint)pmi.QuotaPagedPoolUsage;
				
				result.QuotaPeakNonPagedPoolUsage = (uint)pmi.QuotaPeakNonPagedPoolUsage;
				result.QuotaNonPagedPoolUsage = (uint)pmi.QuotaNonPagedPoolUsage;
				
				result.PagedMemorySize = process.PagedMemorySize64;
				result.PeakPagedMemorySize = process.PeakPagedMemorySize64;

				result.PagedSystemMemorySize = process.PagedSystemMemorySize64;
				result.NonpagedSystemMemorySize = process.NonpagedSystemMemorySize64;

				result.VirtualMemorySize = process.VirtualMemorySize64;
				result.PeakVirtualMemorySize = process.PeakVirtualMemorySize64;
				
				result.PrivateMemorySize = process.PrivateMemorySize64;

				result.IsValidMemoryInfo = true;
			}

			return result; 
		}

		/// <summary>
		/// Creates the process memory info for the specified process.
		/// </summary>
		/// <param name="process">The process to get memory info from.</param>
		/// <returns>The process memory info of the specified process.</returns>
		public static ProcessMemoryInfo Create(Process process)
		{
			return Create(process, string.Empty);
		}

		/// <summary>
		/// Creates the process memory info for the current process.
		/// </summary>
		/// <param name="description">The description text for the process memory info data.</param>
		/// <returns>The process memory info of the current process.</returns>
		public static ProcessMemoryInfo CreateForCurrentProcess(string description)
		{
			using (var currentProcess = Process.GetCurrentProcess())
			{
				return Create(currentProcess, description);
			}
		}

		/// <summary>
		/// Creates the process memory info for the current process.
		/// </summary>
		/// <returns>The process memory info of the current process.</returns>
		public static ProcessMemoryInfo CreateForCurrentProcess()
		{
			using (var currentProcess = Process.GetCurrentProcess())
			{
				return Create(currentProcess, "Current Process Memory Info");
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Tries to query the current memory status.
		/// </summary>
		/// <param name="process">The process to get the process memory info from.</param>
		/// <param name="memoryInfo">The memory info of the specified process.</param>
		/// <returns><see langword="true"/> if getting process memory info was successful, otherwise <see langword="false"/>.</returns>
		private static bool TryGetProcessMemoryInfo(Process process, out ProcessMemoryInformation memoryInfo)
		{
			memoryInfo = new ProcessMemoryInformation();
			return Psapi.GetProcessMemoryInfo(process.Handle, ref memoryInfo, Marshal.SizeOf(typeof(ProcessMemoryInformation)));
		}

		#endregion
	}
}
