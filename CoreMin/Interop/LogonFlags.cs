//--------------------------------------------------------------------------
// File:    LogonFlags.cs
// Content:	Definition of enumeration LogonFlags
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.Interop
{
	/// <summary>
	/// Logon flags used by CreateProcess-Methods
	/// </summary>
	[Flags]
	internal enum LogonFlags : uint
	{
		LogonWithProfile = 0x00000001,
		LogonNetCredentialsOnly = 0x00000002,
		LogonZeroPasswordBuffer = 0x80000000
	}
}
