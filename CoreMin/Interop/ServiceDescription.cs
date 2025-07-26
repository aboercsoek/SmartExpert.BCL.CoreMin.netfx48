//--------------------------------------------------------------------------
// File:    ServiceDescription.cs
// Content:	Implementation of struct ServiceDescription
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Service Description structure</summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct ServiceDescription
	{
		public string lpDescription;
	}
}
