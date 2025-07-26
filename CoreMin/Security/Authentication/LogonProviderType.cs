//--------------------------------------------------------------------------
// File:    LogonProvider.cs
// Content:	Definition of enumeration LogonProvider
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// The available logon provider.
	/// </summary>
	public enum LogonProviderType
	{
		// ReSharper disable InconsistentNaming

		/// <summary>
		/// Use the standard logon provider for the system. The default security provider 
		/// is negotiate, unless you pass NULL for the domain name and the user name 
		/// is not in UPN format. In this case, the default provider is NTLM. 
		/// </summary>
		/// <remarks>Windows 2000/NT: The default security provider is NTLM.</remarks>
		Default = 0,
		/// <summary>
		/// Use the Windows NT 3.5 logon provider
		/// </summary>
		WinNT35 = 1,
		/// <summary>
		/// Use the NTLM logon provider
		/// </summary>
		WinNT40 = 2,
		/// <summary>
		/// Use the negotiate logon provider.
		/// </summary>
		WinNT50 = 3

		// ReSharper restore InconsistentNaming
	}
}
