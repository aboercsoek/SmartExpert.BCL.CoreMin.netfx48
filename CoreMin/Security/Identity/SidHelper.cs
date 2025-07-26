//--------------------------------------------------------------------------
// File:    SidHelper.cs
// Content:	Implementation of class SidHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Identity
{
	///<summary>SID helper class</summary>
	public static class SidHelper
	{
		#region SID convert methods

		/// <summary>
		/// Converts a SID from binary representation to string representation.
		/// </summary>
		/// <param name="binarySid">The SID in binary form.</param>
		/// <returns>SID string representation.</returns>
		public static string ConvertBinarySidToStringSid(byte[] binarySid)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(binarySid, "binarySid");
			ArgChecker.ShouldBeInRange(binarySid.Length, "binarySid", 1, SecurityIdentifier.MaxBinaryLength);

			IntPtr ptrStringSid = Win32.NULL;
			IntPtr ptrBinarySid = Win32.NULL;

			try
			{
				ptrBinarySid = Win32.LocalAlloc(0, (IntPtr)binarySid.Length);
				Marshal.Copy(binarySid, 0, ptrBinarySid, binarySid.Length);
				if (ptrBinarySid == Win32.NULL)
				{
					throw new OperationExecutionFailedException("Win32.LocalAlloc");
				}

				if (AdvApi32.ConvertSidToStringSid(ptrBinarySid, out ptrStringSid).IsFalse())
				{
					// Get error message
					string errorMessage = Win32Helper.GetLastWin32ErrorMessage();
					throw new SecurityException(errorMessage);
				}

				string sidString = Marshal.PtrToStringUni(ptrStringSid);

				return sidString;
			}
			finally
			{
				if (ptrStringSid != Win32.NULL)
					Win32.LocalFree(ptrStringSid);

				if (ptrBinarySid != Win32.NULL)
					Win32.LocalFree(ptrBinarySid);
			}
		}

		/// <summary>
		/// Converts a SID from string representation to binary representation.
		/// </summary>
		/// <param name="stringSid">The SID in string format.</param>
		/// <returns>The binary SID</returns>
		public static byte[] ConvertStringSidToBinarySid(string stringSid)
		{
			var sid = new SecurityIdentifier(stringSid);
			return ConvertSidToBinarySid(sid);
		}

		/// <summary>
		/// Converts a Security-ID (SID) from it's binary representation into a <see cref="SecurityIdentifier"/> instance.
		/// </summary>
		/// <param name="binarySid">The SID in binary form.</param>
		/// <returns>The converted SID (<see cref="SecurityIdentifier"/>).</returns>
		public static SecurityIdentifier ConvertBinarySidToSid(byte[] binarySid)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(binarySid, "binarySid");
			ArgChecker.ShouldBeInRange(binarySid.Length, "binarySid", 1, SecurityIdentifier.MaxBinaryLength);

			return new SecurityIdentifier(binarySid, 0);
		}


		/// <summary>
		/// Converts a <see cref="SecurityIdentifier"/> instance into it's binary form.
		/// </summary>
		/// <param name="sid">The <see cref="SecurityIdentifier"/> to convert.</param>
		/// <returns>The SID in binary form.</returns>
		public static byte[] ConvertSidToBinarySid(SecurityIdentifier sid)
		{
			ArgChecker.ShouldNotBeNull(sid, "sid");
			var binarySid = new byte[sid.BinaryLength];
			sid.GetBinaryForm(binarySid, 0);
			return binarySid;
		}

		#endregion

		#region LookupAccount methods

		/// <summary>
		/// Lookups the SID for an account name.
		/// </summary>
		/// <param name="accountName">[IN] Name of the account used to lookup the SID.</param>
		/// <returns>SID of the account name</returns>
		public static SecurityIdentifier LookupAccountName(string accountName)
		{
			string domainName;
			return LookupAccountName(accountName, out domainName);
		}

		/// <summary>
		/// Lookups the SID for an account name.
		/// </summary>
		/// <param name="accountName">[IN] Name of the account used to lookup the SID.</param>
		/// <param name="domainName"> [OUT] Name of the domain where the account name belongs.</param>
		/// <returns>SID of the account name</returns>
		public static SecurityIdentifier LookupAccountName(string accountName, out string domainName)
		{
			AccountType sidTypeUser;
			return LookupAccountName(accountName, out domainName, out sidTypeUser);
		}

		/// <summary>
		/// Lookups the SID for an account name.
		/// </summary>
		/// <param name="accountName">[IN] Name of the account used to lookup the SID.</param>
		/// <param name="domainName"> [OUT] Name of the domain where the account name belongs.</param>
		/// <param name="sidAccountType">[OUT] Type of the account specified by the SID result.</param>
		/// <returns>SID of the account name</returns>
		public static SecurityIdentifier LookupAccountName(string accountName, out string domainName, out AccountType sidAccountType)
		{
			//ArgChecker.ShouldNotBeNullOrEmpty(strAccountName, "strAccountName");

			var binarySid = new byte[SecurityIdentifier.MaxBinaryLength];
			var binarySidLength = (uint)binarySid.Length;
			var referencedDomain = new StringBuilder(255);
			var referencedDomainLength = (uint)referencedDomain.Capacity;

			bool result = AdvApi32.LookupAccountName(null, accountName, binarySid, ref binarySidLength, referencedDomain, ref referencedDomainLength, out sidAccountType);

			if (result == false)
			{
				var error = (WindowsStatusCode)Marshal.GetLastWin32Error();

				if (error == WindowsStatusCode.NoMappingPerformed)
				{
					throw new IdentityNotMappedException(string.Format("Failed to retrieve the SID for user {0}. No mapping was performed by the operating system.", accountName));
				}

				throw new Win32ExecutionException((int)error, "LookupAccountName");
			}
			domainName = referencedDomain.ToString();

			return new SecurityIdentifier(binarySid, 0);

		}

		#endregion

		#region SID compare methods

		/// <summary>
		/// Are the SIDs in same domain.
		/// </summary>
		/// <param name="sid1">The first SID.</param>
		/// <param name="sid2">The second SID.</param>
		/// <returns>Returns <see langword="true"/> if the SIDs are SIDs of the same domain; otherwise <see langword="false"/>.</returns>
		public static bool AreSidsInSameDomain(SecurityIdentifier sid1, SecurityIdentifier sid2)
		{
			if (sid1 == null || sid2 == null)
				return false;

			return ((sid1.IsAccountSid() && sid2.IsAccountSid()) && sid1.AccountDomainSid.Equals(sid2.AccountDomainSid));
		}

		#endregion


	}
}
