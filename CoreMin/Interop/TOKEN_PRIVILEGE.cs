//--------------------------------------------------------------------------
// File:    TOKEN_PRIVILEGE.cs
// Content:	Implementation of struct TOKEN_PRIVILEGE
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Token Privilege structure</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
// ReSharper disable InconsistentNaming
	internal struct TOKEN_PRIVILEGE
// ReSharper restore InconsistentNaming
	{
		public uint PrivilegeCount;
		public LUID_AND_ATTRIBUTES[] Privileges;
	}
}
