﻿//--------------------------------------------------------------------------
// File:    IAssemblyEnum.cs
// Content:	Definition of interface IAssemblyEnum
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>TODO: Description of interface IAssemblyEnum</summary>
	[ComImport, Guid("21B8916C-F28E-11D2-A473-00C04F8EF448"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAssemblyEnum
	{
		[PreserveSig]
		int GetNextAssembly( out IApplicationContext ppAppCtx, out IAssemblyName ppName, uint dwFlags );
		[PreserveSig]
		int Reset();
		[PreserveSig]
		int Clone( out IAssemblyEnum ppEnum );
	}
}
