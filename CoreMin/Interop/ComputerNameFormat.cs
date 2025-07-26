//--------------------------------------------------------------------------
// File:    ComputerNameFormat.cs
// Content:	Definition of enumeration ComputerNameFormat
// Author:	Andreas Börcsök
// Website:	http://smartexpert.boercsoek.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Interop
{
	///<summary>Computer Name Format enumeration</summary>
	internal enum ComputerNameFormat
	{
		ComputerNameNetBIOS,
		ComputerNameDnsHostname,
		ComputerNameDnsDomain,
		ComputerNameDnsFullyQualified,
		ComputerNamePhysicalNetBIOS,
		ComputerNamePhysicalDnsHostname,
		ComputerNamePhysicalDnsDomain,
		ComputerNamePhysicalDnsFullyQualified
	}
}
