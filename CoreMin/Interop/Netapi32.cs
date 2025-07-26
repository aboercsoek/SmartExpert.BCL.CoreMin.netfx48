//--------------------------------------------------------------------------
// File:    Netapi32.cs
// Content:	Implementation of class Netapi32
// Author:	Andreas Börcsök
// Website:	http://smartexpert.boercsoek.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides Netapi32.dll native methods and helper methods</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static partial class Netapi32
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string NETAPI32 = "Netapi32.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		public const int NERR_PasswordFilterError = 0xA91;
		public const int NERR_PasswordNotComplexEnough = 0xA90;
		public const int NERR_PasswordTooLong = 0xA8F;
		public const int NERR_PasswordTooShort = 0x8C5;

		public const int NET_VALIDATE_PASSWORD_LAST_SET = 0x01;
		public const int NET_VALIDATE_BAD_PASSWORD_TIME = 0x02;
		public const int NET_VALIDATE_LOCKOUT_TIME = 0x04;
		public const int NET_VALIDATE_BAD_PASSWORD_COUNT = 0x08;
		public const int NET_VALIDATE_PASSWORD_HISTORY_LENGTH = 0x10;
		public const int NET_VALIDATE_PASSWORD_HISTORY = 0x20;

		public const int UF_DONT_EXPIRE_PASSWD = 0x10000;

		public const int DOMAIN_ALIAS_RID_ADMINS = 0x220;
		public const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;

		#endregion

		#endregion

		#region DllImports

		[DllImport(NETAPI32, CharSet = CharSet.Unicode)]
		public static extern int NetShareGetInfo(string servername, string netname, uint level, out SafeCloseHandle bufptr);

		[DllImport(NETAPI32)]
		public static extern int NetLocalGroupAddMembers(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string groupname, 
			int level, 
			ref LOCALGROUP_MEMBERS_INFO_0 Lgrmi0_sid, 
			int totalentries);
		
		[DllImport(NETAPI32)]
		public static extern int NetLocalGroupDelMembers(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string groupname, 
			int level, 
			ref LOCALGROUP_MEMBERS_INFO_0 Lgrmi0_sid, 
			int totalentries);
		
		[DllImport(NETAPI32)]
		public static extern int NetLocalGroupGetInfo(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string groupname, 
			int level, 
			out IntPtr bufptr);
		
		[DllImport(NETAPI32)]
		public static extern int NetUserDel(
			[MarshalAs(UnmanagedType.LPWStr)] string servername, 
			[MarshalAs(UnmanagedType.LPWStr)] string username);
		
		[DllImport(NETAPI32, CharSet = CharSet.Unicode)]
		public static extern uint NetValidatePasswordPolicy(
			[MarshalAs(UnmanagedType.LPWStr)] string ServerName, 
			IntPtr Qualifier, 
			NET_VALIDATE_PASSWORD_TYPE ValidationType, 
			[In] ref NET_VALIDATE_PASSWORD_RESET_INPUT_ARG InputArg, 
			[Out] IntPtr OutputArg);

		[DllImport(NETAPI32, CharSet = CharSet.Unicode)]
		public static extern int NetValidatePasswordPolicyFree(IntPtr OutputArg);

		#endregion

		#region Structs & Enums

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct LOCALGROUP_MEMBERS_INFO_0
		{
			public SafeGlobalAllocHandle Lgrmi0_sid;
		}

		public enum NET_VALIDATE_PASSWORD_TYPE
		{
			NetValidateAuthentication = 1,
			NetValidatePasswordChange = 2,
			NetValidatePasswordReset = 3
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NET_VALIDATE_PASSWORD_RESET_INPUT_ARG
		{
			public NET_VALIDATE_PERSISTED_FIELDS InputPersistedFields;
			
			[MarshalAs(UnmanagedType.LPWStr)]
			public string ClearPassword;
			
			[MarshalAs(UnmanagedType.LPWStr)]
			public string UserAccountName;
			
			public NET_VALIDATE_PASSWORD_HASH HashedPassword;
			
			[MarshalAs(UnmanagedType.I1)]
			public bool PasswordMustChangeAtNextLogon;
			
			[MarshalAs(UnmanagedType.I1)]
			public bool ClearLockout;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NET_VALIDATE_PASSWORD_HASH
		{
			public uint Length;
			public IntPtr Hash;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NET_VALIDATE_PERSISTED_FIELDS
		{
			public uint PresentFields;
			public FILETIME PasswordLastSet;
			public FILETIME BadPasswordTime;
			public FILETIME LockoutTime;
			public uint BadPasswordCount;
			public uint PasswordHistoryLength;
			public IntPtr PasswordHistory;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct FILETIME
		{
			public int DwLowDateTime;
			public int DwHighDateTime;
		}




		#endregion

		// ReSharper restore InconsistentNaming
	}
}
