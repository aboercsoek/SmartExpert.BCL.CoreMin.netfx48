//--------------------------------------------------------------------------
// File:    PrivilegeState.cs
// Content:	Definition of enumeration PrivilegeState
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Security.Privileges
{
	/// <summary>
	/// Represents the various states of a privilege held by an access token.
	/// </summary>
	public enum PrivilegeState
	{
		/// <summary>
		/// The privilege is held by the token, but disabled
		/// </summary>
		Disable = 0x0,
		/// <summary>
		/// The privilege is enabled.
		/// </summary>
		Enable = 0x2,
		/// <summary>
		/// The privilege is enabled by default.
		/// </summary>
		EnableByDefault = 0x3,
		/// <summary>
		/// The privilege shall be permanently removed from the token.
		/// </summary>
		Remove = 0x4
	}
}
