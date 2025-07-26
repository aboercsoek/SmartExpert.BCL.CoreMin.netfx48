//--------------------------------------------------------------------------
// File:    AsmNamePropertyFlags.cs
// Content:	Implementation of class AsmNamePropertyFlags
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
	///<summary>AssemblyName property flags</summary>
	internal static class AsmNamePropertyFlags
	{
		public const uint PUBLIC_KEY = 0;
		public const uint PUBLIC_KEY_TOKEN = 1;
		public const uint HASH_VALUE = 2;
		public const uint NAME = 3;
		public const uint MAJOR_VERSION = 4;
		public const uint MINOR_VERSION = 5;
		public const uint BUILD_NUMBER = 6;
		public const uint REVISION_NUMBER = 7;
		public const uint CULTURE = 8;
		public const uint PROCESSOR_ID_ARRAY = 9;
		public const uint OSINFO_ARRAY = 10;
		public const uint HASH_ALGID = 11;
		public const uint ALIAS = 12;
		public const uint CODEBASE_URL = 13;
		public const uint CODEBASE_LASTMOD = 14;
		public const uint NULL_PUBLIC_KEY = 15;
		public const uint NULL_PUBLIC_KEY_TOKEN = 16;
		public const uint CUSTOM = 17;
		public const uint NULL_CUSTOM = 18;
		public const uint MVID = 19;
		public const uint _32_BIT_ONLY = 20;
	}
}
