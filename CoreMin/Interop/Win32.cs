//--------------------------------------------------------------------------
// File:    Win32.cs
// Content:	kernel32.dll and ole32.dll Native Method Imports
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides kernel32.dll and ole32.dll native methods, structures and definitions</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static class Win32
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string KERNEL32 = "kernel32.dll";
		private const string OLE32 = "ole32.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		#endregion

		#region Security related constant declarations

		//internal const uint STATUS_ACCESS_DENIED = 0xc0000022;
		//internal const uint STATUS_ACCOUNT_RESTRICTION = 0xc000006e;
		//internal const uint STATUS_INSUFFICIENT_RESOURCES = 0xc000009a;
		//internal const uint STATUS_NO_MEMORY = 0xc0000017;

		//internal const int ERROR_ACCESS_DENIED = 5;
		//internal const int ERROR_BAD_LENGTH = 0x18;
		//internal const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

		#endregion

		#endregion

		#region COM & DLL Imports

		#region COM Methods Imports
		
		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("51372ae0-cae7-11cf-be81-00aa00a2fa25")]
		internal interface IObjectContext
		{
		}

		[DllImport(OLE32)]
		internal static extern int CoGetObjectContext(ref Guid iid, out IObjectContext g);

		#endregion

		#region Memory Methods Import


		[DllImport(KERNEL32, CharSet = CharSet.Unicode)]
		internal static extern IntPtr LocalAlloc(uint uFlags, IntPtr cb);
		[DllImport(KERNEL32, CharSet = CharSet.Unicode)]
		internal static extern IntPtr LocalFree(IntPtr hlocal);

		#endregion

		#region Message Format and Output Methods Import

		[DllImport(KERNEL32, CharSet = CharSet.Auto)]
		internal static extern int FormatMessage(
			int dwFlags, 
			IntPtr lpSource, 
			int dwMessageId, 
			int dwLanguageId, 
			StringBuilder lpBuffer, 
			int nSize, 
			IntPtr vaListArguments);

		[DllImport(KERNEL32)]
		public static extern void OutputDebugString(string lpOutputString);

		#endregion
		
		#region Process and Thread Methods Import

		[DllImport(KERNEL32, SetLastError = true)]
		internal static extern IntPtr GetCurrentThread();

		[DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetCurrentProcess();

		[DllImport(KERNEL32, ExactSpelling = true)]
		internal static extern void SwitchToThread();

		#endregion

		#region Close Handle Methods Import

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool CloseHandle([In] IntPtr handle);

		#endregion

		#endregion

		// ReSharper restore InconsistentNaming
	}
}
