//--------------------------------------------------------------------------
// File:    ProcessMemoryInformation.cs
// Content:	Implementation of struct ProcessMemoryInformation
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct ProcessMemoryInformation
	{
		private int cb;
		/// <summary>
		/// The number of page faults.
		/// </summary>
		public uint PageFaultCount;
		/// <summary>
		/// The peak working set size, in bytes.
		/// </summary>
		public UIntPtr PeakWorkingSetSize;
		/// <summary>
		/// The current working set size, in bytes.
		/// </summary>
		public UIntPtr WorkingSetSize;
		/// <summary>
		/// The peak paged pool usage, in bytes.
		/// </summary>
		public UIntPtr QuotaPeakPagedPoolUsage;
		/// <summary>
		/// The current paged pool usage, in bytes.
		/// </summary>
		public UIntPtr QuotaPagedPoolUsage;
		/// <summary>
		/// The peak nonpaged pool usage, in bytes.
		/// </summary>
		public UIntPtr QuotaPeakNonPagedPoolUsage;
		/// <summary>
		/// The current nonpaged pool usage, in bytes.
		/// </summary>
		public UIntPtr QuotaNonPagedPoolUsage;
		/// <summary>
		/// The current space allocated for the pagefile, in bytes. Those pages may or may not be in memory.
		/// </summary>
		public UIntPtr PagefileUsage;
		/// <summary>
		/// The peak space allocated for the pagefile, in bytes.
		/// </summary>
		public UIntPtr PeakPagefileUsage;
		/// <summary>
		/// The current amount of memory that cannot be shared with other processes, in bytes. Private bytes include memory that is committed and marked MEM_PRIVATE, data that is not mapped, and executable pages that have been written to.
		/// </summary>
		public UIntPtr PrivateUsage;
	}
}