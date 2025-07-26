//--------------------------------------------------------------------------
// File:    AsmNameObjCreateFlags.cs
// Content:	Implementation of class AsmNameObjCreateFlags
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>AssemblyName object create flags</summary>
	internal static class AsmNameObjCreateFlags
	{
		public const uint CANOF_PARSE_DISPLAY_NAME = 0x1;
		public const uint CANOF_SET_DEFAULT_VALUES = 0x2;
	}
}
