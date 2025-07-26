//--------------------------------------------------------------------------
// File:    SecurityImpersonationLevel.cs
// Content:	Definition of enumeration SecurityImpersonationLevel
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Interop
{
	///<summary>Security Impersonation Level enumeration</summary>
	internal enum SecurityImpersonationLevel
	{
		Anonymous = 0x00,
		Identification = 0x01,
		Impersonation = 0x02,
		Delegation = 0x03
	}
}
