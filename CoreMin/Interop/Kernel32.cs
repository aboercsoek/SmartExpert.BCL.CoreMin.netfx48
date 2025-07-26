//--------------------------------------------------------------------------
// File:    CredUi.cs
// Content: credui.dll Native Method Imports
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides kernel32.dll native methods and helper methods</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static partial class Kernel32
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string KERNEL32 = "kernel32.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		#endregion

		#endregion

		#region DllImports

		//SetLocalTime C# Signature
		[DllImport(KERNEL32)]
		public static extern bool SetLocalTime([In] ref Systemtime lpLocalTime);

		[DllImport(KERNEL32, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool GetComputerNameEx(ComputerNameFormat NameType, [Out] StringBuilder lpBuffer, ref int lpnSize);


		#endregion

		// ReSharper restore InconsistentNaming
	}
}
