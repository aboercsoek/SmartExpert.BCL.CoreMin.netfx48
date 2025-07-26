//--------------------------------------------------------------------------
// File:    WinSecurityContext.cs
// Content:	Definition of enumeration WinSecurityContext
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Interop
{
	///<summary>Windows security context</summary>
	internal enum WinSecurityContext
	{
		Both = 3,
		Process = 2,
		Thread = 1
	}
}
