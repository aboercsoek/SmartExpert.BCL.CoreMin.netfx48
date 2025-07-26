//--------------------------------------------------------------------------
// File:    Privilege.cs
// Content:	Implementation of class Privilege
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Privileges
{
	/// <summary>
	/// This class allows the manipulation of the privileges held by a logon token.
	/// </summary>
	public class Privilege
	{
		#region Constant Values
		/// <summary>
		/// <para>Required to assign the primary token of a process.</para>
		/// <para>User Right: Replace a process-level token.</para>
		/// </summary>
		public const string AssignPrimaryToken = "SeAssignPrimaryTokenPrivilege";
		/// <summary>
		/// <para>Required to generate audit-log entries. Give this privilege to secure servers.</para>
		/// <para>User Right: Generate security audits.</para>
		/// </summary>
		public const string Audit = "SeAuditPrivilege";
		/// <summary>
		/// <para>Required to perform backup operations.
		/// This privilege causes the system to grant all read access control to any file, 
		/// regardless of the access control list (ACL) specified for the file. Any access 
		/// request other than read is still evaluated with the ACL.
		/// The following access rights are granted if this privilege is held:</para>
		/// <para> - READ_CONTROL</para>
		/// <para> - ACCESS_SYSTEM_SECURITY</para>
		/// <para> - FILE_GENERIC_READ</para>
		/// <para> - FILE_TRAVERSE</para>
		/// <para>User Right: Back up files and directories.</para>
		/// </summary>
		public const string Backup = "SeBackupPrivilege";
		/// <summary>
		/// <para>Required to receive notifications of changes to files or directories. 
		/// This privilege also causes the system to skip all traversal access checks. 
		/// It is enabled by default for all users.</para>
		/// <para>User Right: Bypass traverse checking.</para>
		/// </summary>
		public const string ChangeNotify = "SeChangeNotifyPrivilege";
		/// <summary>
		/// <para>Required to create named file mapping objects in the global namespace 
		/// during Terminal Services sessions. This privilege is enabled by default for 
		/// administrators, services, and the local system account.</para>
		/// <para>User Right: Create global objects.</para>
		/// <para>Windows XP/2000: This privilege is not supported. Note that this value is supported starting with 
		/// Windows Server 2003, Windows XP with SP2, and Windows 2000 with SP4.</para>
		/// </summary>
		public const string CreateGlobal = "SeCreateGlobalPrivilege";
		/// <summary>
		/// <para>Required to create a paging file.</para>
		/// <para>User Right: Create a pagefile.</para>
		/// </summary>
		public const string CreatePageFile = "SeCreatePagefilePrivilege";
		/// <summary>
		/// <para>Required to create a permanent object.</para>
		/// <para>User Right: Create permanent shared objects.</para>
		/// </summary>
		public const string CreatePermanent = "SeCreatePermanentPrivilege";
		/// <summary>
		/// <para>Required to create a symbolic link.</para>
		/// <para>User Right: Create symbolic links.</para>
		/// </summary>
		public const string CreateSymbolicLink = "SeCreateSymbolicLinkPrivilege";
		/// <summary>
		/// <para>Required to create a primary token.</para>
		/// <para>User Right: Create a token object.</para>
		/// </summary>
		public const string CreateToken = "SeCreateTokenPrivilege";
		/// <summary>
		/// Required to debug and adjust the memory of a process owned by another account. 
		/// <para>User Right: Debug programs.</para>
		/// </summary>
		public const string Debug = "SeDebugPrivilege";
		/// <summary>
		/// Required to mark user and computer accounts as trusted for delegation.
		/// <para>User Right: Enable computer and user accounts to be trusted for delegation.</para>
		/// </summary>
		public const string EnableDelegation = "SeEnableDelegationPrivilege";
		/// <summary>
		/// Required to impersonate.
		/// <para>User Right: Impersonate a client after authentication.</para>
		/// <para>Windows XP/2000: This privilege is not supported. Note that this value is supported starting with 
		/// Windows Server 2003, Windows XP with SP2, and Windows 2000 with SP4.</para>
		/// </summary>
		public const string Impersonate = "SeImpersonatePrivilege";
		/// <summary>
		/// Required to increase the base priority of a process.
		/// <para>User Right: Increase scheduling priority.</para>
		/// </summary>
		public const string IncreaseBasePriority = "SeIncreaseBasePriorityPrivilege";
		/// <summary>
		/// Required to increase the quota assigned to a process.
		/// <para>User Right: Adjust memory quotas for a process.</para>
		/// </summary>
		public const string IncreaseQuota = "SeIncreaseQuotaPrivilege";
		/// <summary>
		/// Required to allocate more memory for applications that run in the context of users.
		/// <para>User Right: Increase a process working set.</para>
		/// </summary>
		public const string IncreaseWorkingSet = "SeIncreaseWorkingSetPrivilege";
		/// <summary>
		/// Required to load or unload a device driver.
		/// <para>User Right: Load and unload device drivers.</para>
		/// </summary>
		public const string LoadDriver = "SeLoadDriverPrivilege";
		/// <summary>
		/// Required to lock physical pages in memory.
		/// <para>User Right: Lock pages in memory.</para>
		/// </summary>
		public const string LockMemory = "SeLockMemoryPrivilege";
		/// <summary>
		/// Required to create a computer account.
		/// <para>User Right: Add workstations to domain.</para>
		/// </summary>
		public const string MachineAccount = "SeMachineAccountPrivilege";
		/// <summary>
		/// The SeManageVolumePrivilege will let nonadministrators and remote 
		/// users do administrative disk tasks on a machine.
		/// </summary>
		public const string ManageVolume = "SeManageVolumePrivilege";
		/// <summary>
		/// Required to gather profiling information for one process.
		/// <para>User Right: Profile single process.</para>
		/// </summary>
		public const string ProfileSingleProcess = "SeProfileSingleProcessPrivilege";
		/// <summary>
		/// Required to modify the mandatory integrity level of an object.
		/// <para>User Right: Modify an object label.</para>
		/// </summary>
		public const string Relabel = "SeRelabelPrivilege";
		/// <summary>
		/// Required to shut down a system using a network request.
		/// <para>User Right: Force shutdown from a remote system.</para>
		/// </summary>
		public const string RemoteShutdown = "SeRemoteShutdownPrivilege";
		/// <summary>
		/// The meaning of this privilege is currenty unknown.
		/// <para></para>
		/// </summary>
		public const string ReserveProcessor = "SeReserveProcessorPrivilege";
		/// <summary>
		/// Required to perform restore operations. 
		/// This privilege causes the system to grant all write access control to any file, 
		/// regardless of the ACL specified for the file. Any access request other than write 
		/// is still evaluated with the ACL. Additionally, this privilege enables you to 
		/// set any valid user or group SID as the owner of a file.
		/// <para>The following access rights are granted if this privilege is held:</para>
		/// <para> - WRITE_DAC</para>
		/// <para> - WRITE_OWNER</para>
		/// <para> - ACCESS_SYSTEM_SECURITY</para>
		/// <para> - FILE_GENERIC_WRITE</para>
		/// <para> - FILE_ADD_FILE</para>
		/// <para> - FILE_ADD_SUBDIRECTORY</para>
		/// <para> - DELETE</para>
		/// <para>User Right: Restore files and directories.</para>
		/// </summary>
		public const string Restore = "SeRestorePrivilege";
		/// <summary>
		/// Required to perform a number of security-related functions, 
		/// such as controlling and viewing audit messages. This privilege 
		/// identifies its holder as a security operator.
		/// <para>User Right: Manage auditing and security log.</para>
		/// </summary>
		public const string Security = "SeSecurityPrivilege";
		/// <summary>
		/// Required to shut down a local system.
		/// <para>User Right: Shut down the system.</para>
		/// </summary>
		public const string Shutdown = "SeShutdownPrivilege";
		/// <summary>
		/// Required to synchronize directory service data.
		/// </summary>
		public const string SyncAgent = "SeSyncAgentPrivilege";
		/// <summary>
		/// Required to modify the nonvolatile RAM of systems that use this type of 
		/// memory to store configuration information.
		/// <para>User Right: Modify firmware environment values.</para>
		/// </summary>
		public const string SystemEnvironment = "SeSystemEnvironmentPrivilege";
		/// <summary>
		/// Required to gather profiling information for the entire system.
		/// <para>User Right: Profile system performance.</para>
		/// </summary>
		public const string SystemProfile = "SeSystemProfilePrivilege";
		/// <summary>
		/// Required to modify the system time.
		/// <para>User Right: Change the system time.</para>
		/// </summary>
		public const string SystemTime = "SeSystemtimePrivilege";
		/// <summary>
		/// Required to take ownership of an object without being granted discretionary access. 
		/// This privilege allows the owner value to be set only to those values that the holder 
		/// may legitimately assign as the owner of an object.
		/// <para>User Right: Take ownership of files or other objects.</para>
		/// </summary>
		public const string TakeOwnership = "SeTakeOwnershipPrivilege";
		/// <summary>
		/// Identifies its holder as part of the trusted computer base. Some trusted, 
		/// protected subsystems are granted this privilege.
		/// <para>User Right: Act as part of the operating system.</para>
		/// </summary>
		public const string TrustedComputingBase = "SeTcbPrivilege";
		/// <summary>
		/// Required to adjust the time zone associated with the computer's internal clock.
		/// <para>User Right: Change the time zone.</para>
		/// </summary>
		public const string TimeZone = "SeTimeZonePrivilege";
		/// <summary>
		/// Required to access Credential Manager as a trusted caller.
		/// <para>User Right: Access Credential Manager as a trusted caller.</para>
		/// </summary>
		public const string TrustedCredentialManagerAccess = "SeTrustedCredManAccessPrivilege";
		/// <summary>
		/// Required to undock a laptop.
		/// <para>User Right: Remove computer from docking station.</para>
		/// </summary>
		public const string Undock = "SeUndockPrivilege";
		/// <summary>
		/// Required to read unsolicited input from a terminal device.
		/// <para>User Right: Not applicable.</para>
		/// </summary>
		public const string UnsolicitedInput = "SeUnsolicitedInputPrivilege";

		#endregion

		#region Private Fields
		private string m_Name;
		private string m_DisplayName;
		private LUID m_Luid;
		private PrivilegeState m_State;
		private WindowsIdentity m_Identity;
		private PrivilegeCollection m_Privileges;
		#endregion

		#region Ctor

		internal Privilege(PrivilegeCollection privileges, WindowsIdentity identity, LUID luid, PrivilegeState state)
		{
			m_Luid = luid;
			m_Privileges = privileges;
			m_Identity = identity;
			m_Name = LookupPrivilegeName();
			m_State = state;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns the name of the privilege, e.g. SeRemoteShutdownPrivilege.
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Returns the <see cref="PrivilegeState"/> of the Privilege represented by this instance.
		/// </summary>
		public PrivilegeState State
		{
			get
			{
				return m_State;
			}
		}

		/// <summary>
		/// Returns the display name of the current privilege.
		/// </summary>
		public string DisplayName
		{
			get
			{
				if (string.IsNullOrEmpty(m_DisplayName)) m_DisplayName = LookupPrivilegeDisplayName();
				return m_DisplayName;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Enables the privilige represented by the current instance.
		/// </summary>
		public void Enable()
		{
			AdjustTokenPrivileges(m_Identity, false, new PrivilegeAction(m_Luid, PrivilegeState.Enable));
			m_State = PrivilegeState.Enable;
		}

		/// <summary>
		/// Disabled the privilige represented by the current instance.
		/// </summary>
		public void Disable()
		{
			AdjustTokenPrivileges(m_Identity, false, new PrivilegeAction(m_Luid, PrivilegeState.Disable));
			m_State = PrivilegeState.Disable;
		}

		/// <summary>
		/// Removes the privilege represented by this instance. Once a privilige is removed
		/// from a token, it cannot be added back.
		/// </summary>
		public void Remove()
		{
			AdjustTokenPrivileges(m_Identity, false, new PrivilegeAction(m_Luid, PrivilegeState.Remove));
			m_State = PrivilegeState.Remove;
			Privileges.RemovePrivilege(Name);
		}

		/// <summary>
		/// Returns the <see cref="Name"/> of the privilege.
		/// </summary>
		/// <returns>The name of the privilege represented by this instance.</returns>
		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region Internal Properties

		internal LUID Luid
		{
			get { return m_Luid; }
		}

		internal PrivilegeCollection Privileges
		{
			get { return m_Privileges; }
		}

		#endregion

		#region Internal static Methods

		internal static void AdjustTokenPrivileges(WindowsIdentity identity, bool disableAllPrivileges, params PrivilegeAction[] privilegesToModify)
		{
			TOKEN_PRIVILEGE newState;
			bool result;
			uint returnLength = 0;

			newState = new TOKEN_PRIVILEGE();
			newState.PrivilegeCount = (uint)privilegesToModify.Length;
			newState.Privileges = new LUID_AND_ATTRIBUTES[privilegesToModify.Length];

			for (int i = 0; i < privilegesToModify.Length; i++)
			{
				newState.Privileges[i] = new LUID_AND_ATTRIBUTES();
				newState.Privileges[i].Attributes = (uint)privilegesToModify[i].State;
				newState.Privileges[i].Luid = privilegesToModify[i].Privilege;
			}


			using (SafeTokenPrivileges handle = new SafeTokenPrivileges(newState))
			{
				result = AdvApi32.AdjustTokenPrivileges(identity.Token, disableAllPrivileges, handle, handle.Size, IntPtr.Zero, ref returnLength);

				if (result.IsFalse())
					throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());

				int lastError = Marshal.GetLastWin32Error();
				if (lastError != 0)
				{
					switch (lastError)
					{
						case 0x005:
							throw new UnauthorizedAccessException();
						case 0x008:
							throw new OutOfMemoryException();
						case 0x514:
							throw new SmartExpert.Error.PrivilegeNotHeldException("The requested privilege is not held by the specified token.");
						case 0x543:
							throw new UnauthorizedAccessException();
						default:
							throw Marshal.GetExceptionForHR(lastError);
					}

				}

			}
		}

		#endregion

		#region Private Helper Methods

		private string LookupPrivilegeName()
		{
			StringBuilder name;
			uint length;
			bool result;

			name = new StringBuilder(256);
			length = (uint)name.Capacity;

			result = AdvApi32.LookupPrivilegeName(null, ref m_Luid, name, ref length);

			if (!result) throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());

			return name.ToString();
		}

		private string LookupPrivilegeDisplayName()
		{
			StringBuilder sb;
			uint length;
			uint languageId;
			bool result;

			sb = new StringBuilder(256);
			length = (uint)sb.Capacity;

			result = AdvApi32.LookupPrivilegeDisplayName(null, Name, sb, ref length, out languageId);
			if (!result)
			{
				if (length > sb.Capacity)
				{
					length++;
					sb.Capacity = (int)length + 1;
					result = AdvApi32.LookupPrivilegeDisplayName(null, Name, sb, ref length, out languageId);
				}
			}
			if (!result)
			{
				throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
			}
			return sb.ToString();
		}

		#endregion

	}
}
