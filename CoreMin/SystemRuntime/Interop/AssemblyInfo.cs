//--------------------------------------------------------------------------
// File:    AssemblyInfo.cs
// Content:	Implementation of struct AssemblyInfo
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
	///<summary>Assembly information structure</summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct AssemblyInfo
	{
		public uint cbAssemblyInfo;
		public uint dwAssemblyFlags;
		public ulong uliAssemblySizeInKB;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszCurrentAssemblyPathBuf;
		public uint cchBuf;
	}
}
