//--------------------------------------------------------------------------
// File:    CredUiInfo.cs
// Content:	Implementation of struct CredUiInfo
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Credential UI info structure</summary>
	internal struct CredUiInfo
	{
		// ReSharper disable InconsistentNaming
		public int cbSize;
		public IntPtr hwndParent;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszMessageText;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszCaptionText;
		public IntPtr hbmBanner;
		// ReSharper restore InconsistentNaming
	}
}
