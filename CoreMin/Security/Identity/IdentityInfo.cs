//--------------------------------------------------------------------------
// File:    IdentityInfo.cs
// Content:	Implementation of class IdentityInfo
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Security.Principal;

#endregion

namespace SmartExpert.Security.Identity
{
	///<summary>User identity information class.</summary>
	public class IdentityInfo
	{
		/// <summary>
		/// The username of the identity.
		/// </summary>
		public readonly string UserName;

		/// <summary>
		/// The security identifier (SID) of the identity.
		/// </summary>
// ReSharper disable InconsistentNaming
		public readonly SecurityIdentifier SID;
// ReSharper restore InconsistentNaming

		internal IdentityInfo(string userName, SecurityIdentifier sid)
		{
			UserName = userName;
			SID = sid;
		}
	}
}
