//--------------------------------------------------------------------------
// File:    TokenInformationClass.cs
// Content:	Definition of enumeration TokenInformationClass
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.Interop
{
	/// <summary>
	/// Token Information Class enumeration
	/// </summary>
	[Serializable]
	internal enum TokenInformationClass
	{
		TokenUser				= 1,
		TokenGroups				= 2,
		TokenPrivileges			= 3,
		TokenOwner				= 4,
		TokenPrimaryGroup		= 5,
		TokenDefaultDacl		= 6,
		TokenSource				= 7,
		TokenType				= 8,
		TokenImpersonationLevel = 9,
		TokenStatistics			= 10,
		TokenRestrictedSids		= 11,
		TokenSessionId			= 12,
		TokenGroupsAndPrivileges =13,
		TokenSessionReference	= 14,
		TokenSandBoxInert		= 15
	}
}
