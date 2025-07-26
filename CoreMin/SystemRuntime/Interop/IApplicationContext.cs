//--------------------------------------------------------------------------
// File:    IApplicationContext.cs
// Content:	Definition of interface IApplicationContext
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>TODO: Description of interface IApplicationContext</summary>
	[ComImport(), Guid("7C23FF90-33AF-11D3-95DA-00A024A85B51"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IApplicationContext
	{
		void SetContextNameObject( IAssemblyName pName );
		void GetContextNameObject( out IAssemblyName ppName );
		void Set( [MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags );
		void Get( [MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags );
		void GetDynamicDirectory( out int wzDynamicDir, ref uint pdwSize );
	}
}
