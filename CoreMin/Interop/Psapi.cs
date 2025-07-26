//--------------------------------------------------------------------------
// File:    Psapi.cs
// Content: Psapi.dll Native Method Imports
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides Psapi.dll native methods, structures and definitions</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static partial class Psapi
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string PSAPI = "Psapi.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		#endregion

		#endregion

		#region DllImports

		#region Process Memory Info Imports

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport(PSAPI, SetLastError = true)]
		public static extern bool GetProcessMemoryInfo(IntPtr process, [In, Out] ref ProcessMemoryInformation info, int size);

		#endregion

		#endregion

		// ReSharper restore InconsistentNaming
	}
}
