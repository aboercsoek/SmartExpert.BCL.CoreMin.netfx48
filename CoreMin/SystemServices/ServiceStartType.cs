//--------------------------------------------------------------------------
// File:    ServiceStartType.cs
// Content:	Definition of enumeration ServiceStartType
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.SystemServices
{
	///<summary>Windows Service Start Type Enumeration</summary>
	public enum ServiceStartType
	{
		/// <summary>
		/// Does not change the start type of the service
		/// </summary>
		ServiceNoChange = -1,
		/// <summary>
		/// Start during boot sequence
		/// </summary>
		ServiceBootStart = 0,
		/// <summary>
		/// Start at system start
		/// </summary>
		ServiceSystemStart = 1,
		/// <summary>
		/// Auto start up
		/// </summary>
		ServiceAutoStart = 2,
		/// <summary>
		/// Start on demand
		/// </summary>
		ServiceDemandStart = 3,
		/// <summary>
		/// Start is disabled
		/// </summary>
		ServiceDisabled = 4,
	}
}
