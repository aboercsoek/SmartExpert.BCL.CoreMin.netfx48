//--------------------------------------------------------------------------
// File:    AccountType.cs
// Content:	Definition of enumeration AccountType
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Security.Identity
{
	/// <summary>
	/// Defines the various account types of a Windows account
	/// </summary>
	public enum AccountType
	{
		/// <summary>
		/// No account type
		/// </summary>
		None = 0,
		/// <summary>
		/// The account is a user
		/// </summary>
		User,
		/// <summary>
		/// The account is a security group
		/// </summary>
		Group,
		/// <summary>
		/// The account defines a domain
		/// </summary>
		Domain,
		/// <summary>
		/// The account is an alias
		/// </summary>
		Alias,
		/// <summary>
		/// The account is a well-known group, such as BUILTIN\Administrators
		/// </summary>
		WellknownGroup,
		/// <summary>
		/// The account was deleted
		/// </summary>
		DeletedAccount,
		/// <summary>
		/// The account is invalid
		/// </summary>
		Invalid,
		/// <summary>
		/// The type of the account is unknown
		/// </summary>
		Unknown,
		/// <summary>
		/// The account is a computer account
		/// </summary>
		Computer
	}
}
