//--------------------------------------------------------------------------
// File:    AdvApi32.cs
// Content:	advapi32.dll Native Method Imports
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using SmartExpert.Security.Authentication;
using SmartExpert.Security.Identity;
using SmartExpert.SystemServices;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides advapi32.dll native methods, structures and definitions</summary>
	/// <remarks>
	/// IMPORTANT! Rules for authoring the class (v1.1):
	/// (1) All the function declarations MUST be 64-bit aware.
	/// (2) When copypasting from older declarations, you MUST check against the MSDN help or header declaration, 
	/// and you MUST ensure that each parameter has a proper size.
	/// (3) Call the Wide version of the functions (UCS-2-LE) unless there's a strong reason for calling the ANSI version 
	/// (such a reason MUST be indicated in XmlDoc). <c>CharSet = CharSet.Unicode</c>.
	/// (4) ExactSpelling MUST be TRUE. Add the "…W" suffix wherever needed.
	/// (5) SetLastError SHOULD be considered individually for each function. Setting it to <see langword="true"/> allows to report the errors,
	/// but slows down the execution of critical members.
	/// (6) These properties MUST be explicitly set on DllImport attributes of EACH import: 
	/// CharSet, PreserveSig, SetLastError, ExactSpelling.
	/// (7) CLR names MUST be used for types instead of C# ones, eg "Int32" not "int" and "Int64" not "long".
	/// This greately improves the understanding of the parameter sizes.
	/// (8) Sign of the types MUST be favored, eg "DWORD" is "UInt32" not "Int32".
	/// (9) Unsafe pointer types should be used for explicit and implicit pointers rather than IntPtr. 
	/// This way we outline the unsafety of the native calls, and also make it more clear for the 64bit transition.
	/// Eg "HANDLE" is "void*". If the rule forces you to mark some assembly as unsafe, it's an indication a managed utility
	/// incapsulating the call and the handle should be provided in one of the already-unsafe assemblies.
	/// (A) Same rules must apply to members of the structures.
	/// (B) All of the structures MUST have the [StructLayout(LayoutKind.Sequential)], [NoReorder] attributes, as appropriate.
	/// </remarks>
	[SuppressUnmanagedCodeSecurity]
	internal static class AdvApi32
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string ADVAPI32 = "advapi32.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		#endregion

		#region Service related constant declarations

		public const int SC_MANAGER_CREATE_SERVICE = 0x0002;
		public const int SERVICE_WIN32_OWN_PROCESS = 0x0010;
		//int SERVICE_DEMAND_START = 0x00000003;
		public const int SERVICE_ERROR_NORMAL = 0x0001;

		public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
		public const int SERVICE_QUERY_CONFIG = 0x0001;
		public const int SERVICE_CHANGE_CONFIG = 0x0002;
		public const int SERVICE_QUERY_STATUS = 0x0004;
		public const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
		public const int SERVICE_START = 0x0010;
		public const int SERVICE_STOP = 0x0020;
		public const int SERVICE_PAUSE_CONTINUE = 0x0040;
		public const int SERVICE_INTERROGATE = 0x0080;
		public const int SERVICE_USER_DEFINED_CONTROL = 0x0100;

		public const int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
			SERVICE_QUERY_CONFIG |
			SERVICE_CHANGE_CONFIG |
			SERVICE_QUERY_STATUS |
			SERVICE_ENUMERATE_DEPENDENTS |
			SERVICE_START |
			SERVICE_STOP |
			SERVICE_PAUSE_CONTINUE |
			SERVICE_INTERROGATE |
			SERVICE_USER_DEFINED_CONTROL);

		public const int SERVICE_AUTO_START = 0x00000002;

		public const int SERVICE_CONFIG_DESCRIPTION = 1;
		public const int SERVICE_CONFIG_FAILURE_ACTIONS = 2;
		public const int SERVICE_CONFIG_DELAYED_AUTO_START_INFO = 3;
		public const int SERVICE_CONFIG_FAILURE_ACTIONS_FLAG = 4;
		public const int SERVICE_CONFIG_SERVICE_SID_INFO = 5;
		public const int SERVICE_CONFIG_REQUIRED_PRIVILEGES_INFO = 6;
		public const int SERVICE_CONFIG_PRESHUTDOWN_INFO = 7;


		/// <summary>
		/// Access Rights for a Service 
		/// </summary>
		[Flags]
		internal enum ServiceAccessRights
		{
			/// <summary>
			/// Required to call the QueryServiceConfig and QueryServiceConfig2 functions to query the service configuration.
			/// </summary>
			SERVICE_QUERY_CONFIG = 0x0001,
			/// <summary>
			/// Required to call the ChangeServiceConfig or ChangeServiceConfig2 function to change the service configuration. Because this grants the caller the right to change the executable file that the system runs, it should be granted only to administrators.
			/// </summary>
			SERVICE_CHANGE_CONFIG = 0x0002,
			/// <summary>
			/// Required to call the QueryServiceStatusEx function to ask the service control manager about the status of the service.
			/// </summary>
			SERVICE_QUERY_STATUS = 0x0004,
			/// <summary>
			/// Required to call the EnumDependentServices function to enumerate all the services dependent on the service.
			/// </summary>
			SERVICE_ENUMERATE_DEPENDENTS = 0x0008,
			/// <summary>
			/// Required to call the StartService function to start the service.
			/// </summary>
			SERVICE_START = 0x0010,
			/// <summary>
			/// Required to call the ControlService function to stop the service.
			/// </summary>
			SERVICE_STOP = 0x0020,
			/// <summary>
			/// Required to call the ControlService function to pause or continue the service.
			/// </summary>
			SERVICE_PAUSE_CONTINUE = 0x0040,
			/// <summary>
			/// Required to call the ControlService function to ask the service to report its status immediately.
			/// </summary>
			SERVICE_INTERROGATE = 0x0080,
			/// <summary>
			/// Required to call the ControlService function to specify a user-defined control code.
			/// </summary>
			SERVICE_USER_DEFINED_CONTROL = 0x0100,
			/// <summary>
			/// Includes STANDARD_RIGHTS_REQUIRED in addition to all access rights in this table.
			/// </summary>
			SERVICE_ALL_ACCESS = 0xf01ff,
		}

		/// <summary>
		/// Access Rights for the Service Control Manager 
		/// </summary>
		[Flags]
		public enum ServiceControlManagerAccessRights
		{
			/// <summary>
			/// Required to connect to the service control manager.
			/// </summary>
			SC_MANAGER_CONNECT = 0x0001,
			/// <summary>
			/// Required to call the CreateService function to create a service object and add it to the database.
			/// </summary>
			SC_MANAGER_CREATE_SERVICE = 0x0002,
			/// <summary>
			/// Required to call the EnumServicesStatusEx function to list the services that are in the database.
			/// </summary>
			SC_MANAGER_ENUMERATE_SERVICE = 0x0004,
			/// <summary>
			/// Required to call the LockServiceDatabase function to acquire a lock on the database.
			/// </summary>
			SC_MANAGER_LOCK = 0x0008,
			/// <summary>
			/// Required to call the QueryServiceLockStatus function to retrieve the lock status information for the database.
			/// </summary>
			SC_MANAGER_QUERY_LOCK_STATUS = 0x0010,
			/// <summary>
			/// Required to call the NotifyBootConfigStatus function.
			/// </summary>
			SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020,
			/// <summary>
			/// Includes STANDARD_RIGHTS_REQUIRED
			/// </summary>
			SC_MANAGER_ALL_ACCESS = 0xF003F,
		}

		public enum ServiceErrorControlType
		{
			SERVICE_NO_CHANGE = -1,
			SERVICE_ERROR_IGNORE = 0,
			SERVICE_ERROR_NORMAL = 1,
			SERVICE_ERROR_SEVERE = 2,
			SERVICE_ERROR_CRITICAL = 3,
		}

		[Flags]
		public enum ServiceType
		{
			SERVICE_NO_CHANGE			= -1,
			SERVICE_KERNEL_DRIVER		= 0x001,
			SERVICE_FILE_SYSTEM_DRIVER	= 0x002,
			SERVICE_ADAPTER				= 0x004,
			SERVICE_RECOGNIZER_DRIVER	= 0x008,
			SERVICE_DRIVER				= 0x00B,
			SERVICE_WIN32_OWN_PROCESS	= 0x010,
			SERVICE_WIN32_SHARE_PROCESS = 0x020,
			SERVICE_WIN32				= 0x030,
			SERVICE_INTERACTIVE_PROCESS = 0x100,
			SERVICE_TYPE_ALL			= 0x13f,
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class ServiceConfig
		{
			internal int dwServiceType;
			internal int dwStartType;
			internal int dwErrorControl;
			internal IntPtr lpBinaryPathName = Win32.NULL;
			internal IntPtr lpLoadOrderGroup = Win32.NULL;
			internal int dwTagId;
			internal IntPtr lpDependencies = Win32.NULL;
			internal IntPtr lpServiceStartName = Win32.NULL;
			internal IntPtr lpDisplayName = Win32.NULL;
		}

		#endregion Service related constant declarations

		#endregion

		#region DllImports

		#region Sid Methods Import

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern bool ConvertSidToStringSid(IntPtr ptrSid, out IntPtr ptrStringSid);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern bool AllocateAndInitializeSid(IntPtr pIdentifierAuthority, byte nSubAuthorityCount, int dwSubAuthority0, int dwSubAuthority1, int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4, int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, out IntPtr pSid);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern bool CheckTokenMembership(IntPtr TokenHandle, IntPtr SidToCheck, out bool IsMember);


		#endregion

		#region Close Handle Methods Import

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseServiceHandle([In] IntPtr svcHandle);

		#endregion

		#region Impersonation Methods Import

		[DllImport(ADVAPI32, SetLastError = true)]
		internal static extern bool ImpersonateSelf( [In] SecurityImpersonationLevel level );

		/// <summary> The RevertToSelf function terminates the impersonation of a client application.
		/// </summary>
		/// <returns>
		/// <para>If the function succeeds, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero. 
		/// To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/></para>
		/// </returns>
		/// <remarks> More infos: <a href="ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/secauthz/security/reverttoself.htm">RevertToSelf</a>
		/// </remarks>
		[DllImport(ADVAPI32, SetLastError = true)]
		internal static extern int RevertToSelf();

		#endregion

		#region User logon Methods Import

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LogonUser( 
			[In] String userName, 
			[In] String domain, 
			[In] String password, 
			[In] LogonType logonType, 
			[In] LogonProviderType logonProvider,
			out SafeUserToken tokenHandle);

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, EntryPoint="LogonUser", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LogonUser2(
			[In] String userName, 
			[In] String domain, 
			[In] IntPtr password, 
			[In] LogonType logonType, 
			[In] LogonProviderType logonProvider,
			out SafeUserToken tokenHandle);

		#endregion

		#region Token access Methods Import

		[DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenThreadToken( 
			[In] IntPtr threadHandle, 
			[In] TokenAccessLevels desiredAccess, 
			[In] bool openAsSelf,
			out SafeUserToken tokenHandle);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenProcessToken(
			[In] IntPtr processToken, 
			[In] TokenAccessLevels desiredAccess,
			[In, Out] ref SafeUserToken tokenHandle);

		[DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DuplicateToken(
			[In] IntPtr token, 
			[In] SecurityImpersonationLevel level,
			out SafeUserToken newToken);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool AdjustTokenPrivileges([In] IntPtr userToken,
			[In, MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
			[In] SafeTokenPrivileges newState,
			[In] uint bufferLength, 
			[In, Out] IntPtr previousState,
			[In, Out] ref uint returnLength);

		[DllImport(ADVAPI32, CharSet = CharSet.Auto, SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetTokenInformation([In] IntPtr tokenHandle,
			[In] TokenInformationClass tokenInformationClass,
			[In] SafeTokenPrivileges tokenInformation,
			[In] uint tokenInformationLength, out uint returnLength);

		#endregion

		#region Lookup Account and Privilege Method Imports

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupAccountSid(
			[In] string systemName,
			[In, MarshalAs(UnmanagedType.LPArray)] byte[] sid,
			[Out] StringBuilder name,
			[In, Out] ref uint nameLength,
			[Out] StringBuilder referencedDomainName,
			[In, Out] ref uint referencedDomainNameLength,
			[Out] out AccountType usage);

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupAccountName(
			[In] string systemName,
			[In] string accountName,
			[Out, MarshalAs(UnmanagedType.LPArray)] byte[] sid,
			[In, Out] ref uint sidSize,
			[Out] StringBuilder referencedDomainName,
			[In, Out] ref uint referencedDomainNameLength,
			[Out] out AccountType accountType);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool LookupAccountName(
			string lpSystemName, 
			string lpAccountName, 
			IntPtr Sid, 
			ref int cbSid, 
			[Out] StringBuilder ReferencedDomainName, 
			ref int cchReferencedDomainName,
			out AccountType accountType);


		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeDisplayName(
			[In] string systemName,
			[In] string privilegeName,
			[In] StringBuilder displayName,
			[In, Out] ref uint displayNameLength,
			out uint languageId);

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport(ADVAPI32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeName(
			string systemName, 
			ref LUID luid,
			StringBuilder privilegeName, 
			ref uint privilegeNameLength);

		#endregion

		#region Service Methods Import

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern IntPtr OpenSCManager(string lpMachineName, string lpServiceControlDatabaseName, 
													ServiceControlManagerAccessRights desiredAccess);

		[DllImport(ADVAPI32)]
		public static extern IntPtr OpenSCManager(string lpMachineName, string lpServiceControlDatabaseName, int desiredAccess);

		[DllImport(ADVAPI32)]
		public static extern IntPtr CreateService(IntPtr svcHandle, string lpSvcName, string lpDisplayName,
			int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
			string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);

		[DllImport(ADVAPI32)]
		public static extern int StartService(IntPtr svcHandle, int dwNumServiceArgs, string lpServiceArgVectors);

		[DllImport(ADVAPI32, SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr svcHandle, string lpSvcName, int dwNumServiceArgs);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern IntPtr OpenService(IntPtr hServiceControlManager, string lpServiceName, ServiceAccessRights desiredAccess);

		[DllImport(ADVAPI32)]
		public static extern int DeleteService(IntPtr svcHandle);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool QueryServiceConfig(IntPtr svcHandle,
			[MarshalAs(UnmanagedType.LPStruct)]ServiceConfig lpServiceConfig,
			int cbBufSize, out int cbBytesNeeded);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool QueryServiceConfig(IntPtr svcHandle, IntPtr lpServiceConfig, int cbBufSize, out int cbBytesNeeded);

		[DllImport(ADVAPI32, CharSet = CharSet.Unicode)]
		internal static extern bool ChangeServiceConfig(IntPtr svcHandle, ServiceType dwServiceType, ServiceStartType dwStartType, 
			ServiceErrorControlType dwErrorControl, string lpBinaryPathName, 
			string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, 
			string lpServiceStartName, string lpPassword, string lpDisplayName);

		[DllImport(ADVAPI32, SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ChangeServiceConfig2(IntPtr svcHandle, int dwInfoLevel, 
														[MarshalAs(UnmanagedType.Struct)] ref ServiceDescription lpInfo);

		#endregion

		#endregion

		// ReSharper restore InconsistentNaming
	}
}
