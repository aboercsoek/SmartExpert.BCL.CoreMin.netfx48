//--------------------------------------------------------------------------
// File:    AssemblyNameDisplayFlags.cs
// Content:	Definition of enumaretion AssemblyNameDisplayFlags
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>TODO: Description of enumeration AssemblyNameDisplayFlags</summary>
	[Flags]
	internal enum AssemblyNameDisplayFlags
	{
		VERSION = 0x01,
		CULTURE = 0x02,
		PUBLIC_KEY_TOKEN = 0x04,
		PROCESSORARCHITECTURE = 0x20,
		RETARGETABLE = 0x80,
		ALL = VERSION | CULTURE | PUBLIC_KEY_TOKEN | PROCESSORARCHITECTURE | RETARGETABLE
	}
}
