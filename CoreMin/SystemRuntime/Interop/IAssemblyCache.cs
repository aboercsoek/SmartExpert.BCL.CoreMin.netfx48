//--------------------------------------------------------------------------
// File:    IAssemblyCache.cs
// Content:	Definition of interface IAssemblyCache
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>GAC native methods COM import interface.</summary>
	[SuppressUnmanagedCodeSecurity]
	[ComImport, Guid("E707DCDE-D1CD-11D2-BAB9-00C04F8ECEAE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAssemblyCache
	{
		[PreserveSig]
		int UninstallAssembly( uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, IntPtr pvReserved, out uint pulDisposition );
		[PreserveSig]
		int QueryAssemblyInfo( uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, ref AssemblyInfo pAsmInfo );
		[PreserveSig]
		int CreateAssemblyCacheItem( uint dwFlags, IntPtr pvReserved, out object ppAsmItem, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName );
		[PreserveSig]
		int CreateAssemblyScavenger( out object ppAsmScavenger );
		[PreserveSig]
		int InstallAssembly( uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszManifestFilePath, IntPtr pvReserved );
	}
}
