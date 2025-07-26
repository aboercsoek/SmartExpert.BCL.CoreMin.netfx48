//--------------------------------------------------------------------------
// File:    IAssemblyName.cs
// Content:	Definition of interface IAssemblyName
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{

	/// <summary>
	/// The assembly name interface.
	/// </summary>
	[ComImport, Guid("CD193BC0-B4BC-11D2-9833-00C04FC31D2E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAssemblyName
	{
		[PreserveSig]
		int SetProperty( uint PropertyId, IntPtr pvProperty, uint cbProperty );
		[PreserveSig]
		int GetProperty( uint PropertyId, IntPtr pvProperty, ref uint pcbProperty );
		[PreserveSig]
		int Finalize();
		[PreserveSig]
		int GetDisplayName( StringBuilder szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags );
		[PreserveSig]
		int BindToObject( object refIID, object pAsmBindSink, IApplicationContext pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv );
		[PreserveSig]
		int GetName( out uint lpcwBuffer, out int pwzName );
		[PreserveSig]
		int GetVersion( out uint pdwVersionHi, out uint pdwVersionLow );
		[PreserveSig]
		int IsEqual( IAssemblyName pName, uint dwCmpFlags );
		[PreserveSig]
		int Clone( out IAssemblyName pName );
	}
}
