//--------------------------------------------------------------------------
// File:    LUID_AND_ATTRIBUTES.cs
// Content:	Implementation of struct LUID_AND_ATTRIBUTES
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Local unique identifier with attributes structure</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
// ReSharper disable InconsistentNaming
	internal struct LUID_AND_ATTRIBUTES
// ReSharper restore InconsistentNaming
	{
		public LUID Luid;
		public uint Attributes;
	}
}
