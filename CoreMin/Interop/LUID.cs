//--------------------------------------------------------------------------
// File:    LUID.cs
// Content:	Implementation of struct LUID
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
	///<summary>Local unique identifier structure</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
// ReSharper disable InconsistentNaming
	internal struct LUID
// ReSharper restore InconsistentNaming
	{
		public uint LowPart;
		public uint HighPart;
	}
}
