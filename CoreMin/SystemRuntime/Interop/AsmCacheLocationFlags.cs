//--------------------------------------------------------------------------
// File:    ASM_CACHE.cs
// Content:	Implementation of class ASM_CACHE
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;

#endregion

namespace SmartExpert.SystemRuntime.Interop
{
	///<summary>Assembly cache location flags</summary>
	internal static class AsmCacheLocationFlags
	{
		public const uint ZAP = 1;
		public const uint GAC = 2;
		public const uint DOWNLOAD = 4;
	}
}
