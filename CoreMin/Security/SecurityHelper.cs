//--------------------------------------------------------------------------
// File:    SecurityUtils.cs
// Content:	Implementation of class SecurityUtils
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Web;
using SmartExpert.Error;
using SmartExpert.Interop;
using SmartExpert.Logging;
using SmartExpert.Security.Authentication;
using SmartExpert.Security.Identity;

#endregion

namespace SmartExpert.Security
{
	///<summary>Security helper methods.</summary>
	public static class SecurityHelper
	{
		#region LogonUser

		/// <summary>
		/// Logon as a different user.
		/// </summary>
		/// <param name="username">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>The windows identity.</returns>
		/// <example>
		/// <code lang="cs" title="LogonUser example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_ImpersonateAction" />
		/// <code lang="cs" title="LogonUser example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_HelperMethods" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_ImpersonateAction.txt" />
		/// </example>
		/// <exception cref="SecurityException">Is thrown if the login fails.</exception>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="username"/> or <paramref name="password"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="username"/> is <see cref="F:System.String.Empty"/></exception>
		public static WindowsIdentity LogonUser(string username, string password)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(password, "password");

			return LogonUser(username, ".", password);
		}

		/// <summary>
		/// Logon as a different user.
		/// </summary>
		/// <param name="username">Name of the user.</param>
		/// <param name="domain">The domain.</param>
		/// <param name="password">The password.</param>
		/// <returns>The windows identity.</returns>
		/// <exception cref="SecurityException">Is thrown if logon user fails.</exception>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="username"/> or <paramref name="domain"/> or <paramref name="password"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="username"/> is <see cref="F:System.String.Empty"/></exception>
		/// <example>
		/// <code lang="cs" title="Get identities example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities" />
		/// <code lang="cs" title="Get identities example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities_Helper" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities.txt" />
		/// </example>
		public static WindowsIdentity LogonUser(string username, string domain, string password)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(domain, "domain");
			ArgChecker.ShouldNotBeNull(password, "password");

			if (domain == string.Empty)
				domain = ".";

			return LogonUser(username, domain, password, LogonType.Interactive);
		}

		/// <summary>
		/// Logon as a different user.
		/// </summary>
		/// <param name="username">Name of the user.</param>
		/// <param name="domain">The domain.</param>
		/// <param name="password">The password.</param>
		/// <param name="logonType">The logon type</param>
		/// <returns>The windows identity.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="username"/> or <paramref name="domain"/> or <paramref name="password"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="username"/> is <see cref="F:System.String.Empty"/></exception>
		/// <exception cref="SecurityException">Is thrown if logon fails.</exception>
		/// <example>
		/// <code lang="cs" title="Get identities example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_LogonUserWithLogonType" />
		/// </example>
		public static WindowsIdentity LogonUser(string username, string domain, string password, LogonType logonType)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(domain, "domain");
			ArgChecker.ShouldNotBeNull(password, "password");

			if (domain == string.Empty)
				domain = ".";

			WindowsIdentity identity;

// ReSharper disable RedundantAssignment
			SafeUserToken userToken = SafeUserToken.ZeroToken; 
// ReSharper restore RedundantAssignment
			SafeUserToken userTokenDuplication = SafeUserToken.ZeroToken;

			RuntimeHelpers.PrepareConstrainedRegions();

			bool loggedOn = AdvApi32.LogonUser(username, domain, password, logonType, LogonProviderType.Default, out userToken);

			if (loggedOn)
			{
				try
				{
					// Create a duplication of the usertoken, this is a solution
					// for the known bug that is published under KB article Q319615.
					if (AdvApi32.DuplicateToken(userToken.DangerousGetHandle(), SecurityImpersonationLevel.Impersonation, out userTokenDuplication))
					{
						// Create windows identity from the token and impersonate the user.
						identity = userTokenDuplication.GetWindowsIdentity();
					}
					else
					{
						// Token duplication failed!
						string errorMessage = Win32Helper.GetLastWin32ErrorMessage();
						throw new SecurityException(errorMessage);
					}
				}
				finally
				{
					// Close usertoken handle duplication when created.
					if (userTokenDuplication.IsValidToken()) userTokenDuplication.Dispose();

					// Close usertoken handle when created.
					if (userToken.IsValidToken()) userToken.Dispose();
				}
			}
			else
			{
				// Get error message
				string errorMessage = Win32Helper.GetLastWin32ErrorMessage();
				// Close usertoken handle when created.
				if (userToken.IsValidToken()) userToken.Dispose();

				throw new SecurityException(errorMessage);
			}

			return identity;
		}

		#endregion

		#region LogonUser with secure password string

		/// <summary>
		/// Logon as a different user using secure password string.
		/// </summary>
		/// <param name="username">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>The windows identity.</returns>
		/// <example>
		/// <code lang="cs" title="LogonUser example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure.txt" />
		/// </example>
		/// <exception cref="Win32ExecutionException">Is thrown if the login fails.</exception>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="username"/> or <paramref name="password"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="username"/> is <see cref="F:System.String.Empty"/></exception>
		public static WindowsIdentity LogonUserSecure(string username, SecureString password)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(password, "password");

			return LogonUserSecure(username, ".", password);
		}

		/// <summary>
		/// Logon as a different user using secure password string.
		/// </summary>
		/// <param name="username">Name of the user.</param>
		/// <param name="domain">The domain.</param>
		/// <param name="password">The secure password.</param>
		/// <returns>The windows identity of the user.</returns>
		/// <example>
		/// <code lang="cs" title="LogonUser example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure.txt" />
		/// </example>
		/// <exception cref="Win32ExecutionException">Is thrown if logon user fails.</exception>
		/// <exception cref="ArgNullException">
		/// Is thrown if <paramref name="username"/> or <paramref name="domain"/> or <paramref name="password"/> is <see langword="null"/></exception>
		/// <exception cref="ArgEmptyException">
		/// Is thrown if <paramref name="username"/> or <paramref name="domain"/> is <see cref="F:System.String.Empty"/></exception>
		public static WindowsIdentity LogonUserSecure(string username, string domain, SecureString password)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNullOrEmpty(domain, "domain");
			ArgChecker.ShouldNotBeNull(password, "password");

// ReSharper disable RedundantAssignment
			SafeUserToken usertoken = SafeUserToken.ZeroToken;
// ReSharper restore RedundantAssignment
			IntPtr passwordPtr = IntPtr.Zero;

			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);

				bool loggedOn = AdvApi32.LogonUser2(username, domain, passwordPtr, LogonType.Interactive, LogonProviderType.Default, out usertoken);

				if (loggedOn.IsFalse())
				{
					// Get error message
					string errorMessage = Win32Helper.GetLastWin32ErrorMessage();

					// Close usertoken handle when created.
					if (usertoken.IsValidToken()) usertoken.Dispose();

					throw new SecurityException(errorMessage);
				}
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(passwordPtr);
			}

			return usertoken.GetWindowsIdentity();
		}

		#endregion

		#region CheckUser Helpers

		/// <summary>
		/// Check if the specified user has access writes to a directory
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password of the user.</param>
		/// <param name="directory">The directory that should be checked.</param>
		/// <returns><see langword="true"/> if the user have access to the directory; otherwise <see langword="false"/>.</returns>
		public static bool CheckUserAccessToDir(string userName, string password, string directory)
		{
			bool flag = true;
			
			SafeUserToken userToken = GetUserToken(userName, password, false);
			if (userToken == null)
				return false;
			
			WindowsImpersonationContext context = Impersonate(userToken);
			try
			{
				if (directory.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
				{
					bool existsButAccessDenied = false;
					DoesShareExist(directory, out existsButAccessDenied);
					if (existsButAccessDenied)
						throw new UnauthorizedAccessException();

					Directory.GetFiles(directory);
					return true;
				}

				if (Directory.Exists(directory))
				{
					Directory.GetFiles(directory);
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception exception)
			{
				if (exception is UnauthorizedAccessException)
					flag = false;
			}
			finally
			{
				context.Undo();
			}

			return flag;
		}

		/// <summary>
		/// Check if a shared directory exist und can be accessed by the current user.
		/// </summary>
		/// <param name="sharePath">The shared path.</param>
		/// <param name="existsButAccessDenied">[out] Is set to true if the shared path exist but cannot accessed by the current user.</param>
		/// <returns><see langword="true"/> if the shared path exist; otherwise <see langword="false"/>.</returns>
		public static bool DoesShareExist(string sharePath, out bool existsButAccessDenied)
		{
			SafeCloseHandle netShareInfoPtr = null;
			existsButAccessDenied = false;
			if (sharePath.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
			{
				string[] sharePathSplit = sharePath.Substring(2).Split(new char[] { '\\' });
				if (sharePathSplit.Length < 2)
				{
					return false;
				}
				string servername = sharePathSplit[0];
				string netname = sharePathSplit[1];
				try
				{
					switch (Netapi32.NetShareGetInfo(servername, netname, 1, out netShareInfoPtr))
					{
						case 0:
							return true;

						case 5:
							existsButAccessDenied = true;
							return true;
					}
				}
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;
				}
			}
			return false;
		}

		internal static SafeUserToken GetUserToken(string fullUserName, string password, bool usedForPasswordVerification)
		{
			string str;
			return GetUserToken(fullUserName, password, usedForPasswordVerification, out str);
		}

		internal static SafeUserToken GetUserToken(string fullUserName, string password, bool usedForPasswordVerification, out string errorMessage)
		{
			string str;
			string str2;
			errorMessage = string.Empty;
			string[] strArray = fullUserName.Split(new char[] { '\\' });
			if (strArray.Length == 1)
			{
				str = fullUserName;
				str2 = str.IndexOf('@') != -1 ? null : ".";
			}
			else
			{
				str2 = strArray[0];
				str = strArray[1];
			}
			SafeUserToken phToken = null;
			if (usedForPasswordVerification)
			{
				if (!AdvApi32.LogonUser(str, str2, password, LogonType.Network, 0, out phToken))
				{
					Exception exceptionForHR = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
					if (exceptionForHR != null)
					{
						errorMessage = exceptionForHR.Message;
					}
					return null;
				}
			}
			else if (!AdvApi32.LogonUser(str, str2, password, LogonType.NetworkClearText, 0, out phToken))
			{
				Exception exception2 = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
				if (exception2 != null)
				{
					errorMessage = exception2.Message;
				}
				return null;
			}
			if ((phToken != null) && !phToken.IsInvalid)
			{
				return phToken;
			}

			return null;
		}


		#endregion

		#region Impersonation

		/// <summary>Impersonate execution of <see cref="System.Action">action</see>.
		/// </summary>
		/// <param name="winIdentity">
		///		The <see cref="System.Security.Principal.WindowsIdentity">WindowsIdentity</see> used during impersonation.</param>
		/// <param name="action">
		///		The action that should be called during impersonation.</param>
		/// <example>
		/// <code lang="cs" title="ImpersonateAction example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_ImpersonateAction" />
		/// <code lang="cs" title="ImpersonateAction example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_HelperMethods" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_ImpersonateAction.txt" />
		/// </example>
		/// <exception cref="ArgNullException">Is thrown if winIdentity or action is <see langword="null"/>.</exception>
		public static void ImpersonateAction(WindowsIdentity winIdentity, Action action)
		{
			ArgChecker.ShouldNotBeNull(winIdentity, "winIdentity");
			ArgChecker.ShouldNotBeNull(action, "action");

			WindowsImpersonationContext impctx = null;
			try
			{
				impctx = winIdentity.Impersonate();
				action();
			}
			finally
			{
				if (impctx != null)
					impctx.Undo();
			}
		}

		private static WindowsImpersonationContext Impersonate(SafeUserToken userToken)
		{
			bool flag = false;
			WindowsImpersonationContext context = null;
			if ((userToken != null) && !userToken.IsInvalid)
			{
				context = WindowsIdentity.Impersonate(userToken.Handle);
				flag = true;
			}
			if (!flag)
			{
				throw new InvalidOperationException();
			}
			return context;
		}


		#endregion

		#region Process Identity

		/// <summary>Gets the current process identity information.</summary>
		/// <returns>The windows identity info of the current process..</returns>
		/// <example>
		/// <code lang="cs" title="Get current process identity info example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentProcessIdentityInfo" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetCurrentProcessIdentityInfo.txt" />
		/// </example>
		public static IdentityInfo GetCurrentProcessIdentityInfo()
		{
			IdentityInfo result = null;

			try
			{
				WindowsIdentity currentProcessIdentity = GetCurrentProcessIdentity();
				if (currentProcessIdentity != null)
				{
					result = new IdentityInfo(currentProcessIdentity.Name, currentProcessIdentity.User);
				}
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;

				QuickLogger.Log("ERROR: " + exception.RenderExceptionMessage());
			}

			return result;
		}

		/// <summary>Gets the current process windows identity.</summary>
		/// <returns>The windows identity of the current process.</returns>
		/// <example>
		/// <code lang="cs" title="Get identities example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities" />
		/// <code lang="cs" title="Get identities example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities_Helper" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities.txt" />
		/// </example>
		public static WindowsIdentity GetCurrentProcessIdentity()
		{
			int hr;
			SafeUserToken handle = GetCurrentProcessToken(TokenAccessLevels.MaximumAllowed, out hr);
			if ((handle == null) || handle.IsInvalid)
			{
				throw new SecurityException(Win32Helper.GetWin32ErrorMessage(hr));
			}
			return handle.GetWindowsIdentity();
		}

		#endregion

		#region Local System & Service Account Information

		private static string m_NtAuthorityName;
		/// <summary>
		/// Gets the NT Authority name of the current system (Local System Account Name).
		/// </summary>
		/// <returns>NT Authority name.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static string GetNtAuthorityName()
		{
			if (m_NtAuthorityName == null)
			{
				var identifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
				var account = (NTAccount)identifier.Translate(typeof(NTAccount));
				int index = account.Value.IndexOf('\\');
				m_NtAuthorityName = account.Value.Substring(0, index);
			}
			return m_NtAuthorityName;
		}

		private static string m_LocalSystemName;
		/// <summary>
		/// Gets the Local System account name of the current system.
		/// </summary>
		/// <returns>Local System account name.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static string GetLocalSystemName()
		{
			if (m_LocalSystemName == null)
			{
				var identifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
				var account = (NTAccount)identifier.Translate(typeof(NTAccount));
				string[] accountSplit = account.Value.Split('\\');
				if (m_NtAuthorityName == null) m_NtAuthorityName = accountSplit[0];
				m_LocalSystemName = accountSplit[1];
			}
			return m_LocalSystemName;
		}

		/// <summary>
		/// Gets the Local System account SID of the current system.
		/// </summary>
		/// <returns>Local System account SID.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static SecurityIdentifier GetLocalSystemSid()
		{
			
			var identifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
			return identifier;
		}

		private static string m_LocalServiceName;
		/// <summary>
		/// Gets the Local Service account name of the current system.
		/// </summary>
		/// <returns>Local Service account name.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <pre title="Output" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static string GetLocalServiceName()
		{
			if (m_LocalServiceName == null)
			{
				var identifier = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);
				var account = (NTAccount)identifier.Translate(typeof(NTAccount));
				string[] accountSplit = account.Value.Split('\\');
				if (m_NtAuthorityName == null) m_NtAuthorityName = accountSplit[0];
				m_LocalServiceName = accountSplit[1];
			}
			return m_LocalServiceName;
		}

		/// <summary>
		/// Gets the Local Service account SID of the current system.
		/// </summary>
		/// <returns>Local Service account SID.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static SecurityIdentifier GetLocalServiceSid()
		{

			var identifier = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);
			return identifier;
		}

		private static string m_NetworkServiceName;
		/// <summary>
		/// Gets the Network Service account name of the current system.
		/// </summary>
		/// <returns>Network Service account name.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static string GetNetworkServiceName()
		{
			if (m_NetworkServiceName == null)
			{
				var identifier = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
				var account = (NTAccount)identifier.Translate(typeof(NTAccount));
				string[] accountSplit = account.Value.Split('\\');
				if (m_NtAuthorityName == null) m_NtAuthorityName = accountSplit[0];
				m_NetworkServiceName = accountSplit[1];
			}
			return m_NetworkServiceName;
		}

		/// <summary>
		/// Gets the Network Service account SID of the current system.
		/// </summary>
		/// <returns>Network Service account SID.</returns>
		/// <example>
		/// <code lang="cs" title="Get Build-In Service Account Names" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetBuildInServiceAccountNames.txt" />
		/// </example>
		public static SecurityIdentifier GetNetworkServiceSid()
		{
			var identifier = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
			return identifier;
		}

		#endregion

		#region Current User Identity

		/// <summary>
		/// Gets the the SID of the current user.
		/// </summary>
		/// <returns>
		/// SID of the current user or <see langword="null"/> if user identity is <see langword="null"/> or not a <see cref="WindowsIdentity"/>.
		/// </returns>
		public static SecurityIdentifier GetCurrentUserSid()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent(false);
			return current == null ? null : current.User;
		}

		/// <summary>
		/// Gets the current username.
		/// </summary>
		/// <returns>The current username.</returns>
		/// <example>
		/// <code lang="cs" title="LogonUser example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_LogonUserSecure.txt" />
		/// </example>
		public static string GetCurrentUserName()
		{

			if (ServiceSecurityContext.Current != null)
			{
				if (string.IsNullOrEmpty(ServiceSecurityContext.Current.PrimaryIdentity.Name) == false)
					return ServiceSecurityContext.Current.PrimaryIdentity.Name;
			}

			if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null))
			{
				
				if (string.IsNullOrEmpty(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name) == false)
					return OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
			}

			if ((HttpContext.Current != null) && (HttpContext.Current.User != null))
			{
				if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) == false)
					return HttpContext.Current.User.Identity.Name;
			}

			WindowsIdentity current = WindowsIdentity.GetCurrent(false);

			return current == null ? string.Empty : current.Name;
		}

		/// <summary>
		/// Gets the current user identity.
		/// </summary>
		/// <returns>The current user identity.</returns>
		/// <example>
		/// <code lang="cs" title="Get identities example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities" />
		/// <code lang="cs" title="Get identities example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities_Helper" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities.txt" />
		/// </example>
		public static IIdentity GetCurrentUserIdentity()
		{
			if (ServiceSecurityContext.Current != null)
			{
				return ServiceSecurityContext.Current.PrimaryIdentity;
			}

			if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null))
			{
				if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null)
					return OperationContext.Current.ServiceSecurityContext.PrimaryIdentity;
			}

			if ((HttpContext.Current != null) && (HttpContext.Current.User != null))
			{
				return HttpContext.Current.User.Identity;
			}

			return WindowsIdentity.GetCurrent(false);
		}

		#endregion

		#region Current Thread Security Context

		/// <summary>
		/// Gets the current thread execution identity.
		/// </summary>
		/// <returns>The current thread execution identity.</returns>
		/// <example>
		/// <code lang="cs" title="Get identities example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities" />
		/// <code lang="cs" title="Get identities example ..." numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_SecuritySecurityHelper.cs" region="Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities_Helper" />
		/// <code title="Output" numberLines="false" outlining="false" source=".\examples\Sample_CoreMin_Security_M_SecurityHelper_GetCurrentIdentities.txt" />
		/// </example>
		public static WindowsIdentity GetCurrentThreadExecutionIdentity()
		{
			// Path to get the identity: Thread.CurrentThread.ExecutionContext.SecurityContext.WindowsIdentity
			// The tricky part is to optain the last two path parts because they are marked as internals!

			ExecutionContext ec = Thread.CurrentThread.ExecutionContext;

			if (ec == null)
				return null;

			PropertyInfo piSecurityContext = ec.GetType().GetProperty("SecurityContext",
				BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

			SecurityContext sc = piSecurityContext.GetValue(ec, null) as SecurityContext;

			if (sc == null)
				return null;

			PropertyInfo piWindowsIdentity = sc.GetType().GetProperty("WindowsIdentity",
				BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

			WindowsIdentity wi = piWindowsIdentity.GetValue(sc, null) as WindowsIdentity;

			return wi;
		}

		#endregion

		#region User Principals

		/// <summary>
		/// Find user principal by account name.
		/// </summary>
		/// <param name="username">The account name.</param>
		/// <returns>The principal of the user account.</returns>
		public static UserPrincipal FindUserPrincipalByName(string username)
		{
			var principalContext = new PrincipalContext(ContextType.Domain);
			
			return UserPrincipal.FindByIdentity(principalContext, username);
		}

		/// <summary>
		/// Find user principal by account name.
		/// </summary>
		/// <param name="username">The account name.</param>
		/// <param name="searchContext">The search scope.</param>
		/// <returns>The principal of the user account.</returns>
		public static UserPrincipal FindUserPrincipalByName(string username, ContextType searchContext)
		{
			var principalContext = new PrincipalContext(searchContext);

			return UserPrincipal.FindByIdentity(principalContext, username);
		}

		/// <summary>
		/// Find user principal by account name.
		/// </summary>
		/// <param name="contextType">The type of store to which the principal belongs (Machine, ApplicationDirectory, Domain).</param>
		/// <param name="nameOfMachineOrServerOrDomain">Name depends on the provided context type. My not be null for context type ApplicationDirectory.</param>
		/// <param name="username">The account name.</param>
		/// <returns>The principal of the user account.</returns>
		public static UserPrincipal FindUserPrincipalByName(ContextType contextType, string nameOfMachineOrServerOrDomain, string username)
		{
			var principalContext = new PrincipalContext(contextType, nameOfMachineOrServerOrDomain);
			
			return UserPrincipal.FindByIdentity(principalContext, username);
		}

		#endregion

		#region Roles

		///// <summary>
		///// Checks if the current Windows user is in specific role.
		///// </summary>
		///// <param name="role">Name of the role.</param>
		///// <returns>Is current Windows user is in role <see langword="true"/>, otherwise <see langword="false"/>.</returns>
		//public static bool IsCurrentUserInRole(string role)
		//{
		//    ArgChecker.ShouldNotBeNullOrEmpty(role, "role");

		//    WindowsIdentity current = WindowsIdentity.GetCurrent(false);
		//    if (current == null)
		//        return false;

		//    WindowsPrincipal authenticatedUser = new WindowsPrincipal(current);

		//    return authenticatedUser.IsInRole(role);
		//}

		///// <summary>
		///// Checks if a Windows user is in specific role.
		///// </summary>
		///// <param name="username">Windows username.</param>
		///// <param name="role">Name of the role.</param>
		///// <returns>Is Windows user is in role <see langword="true"/>, otherwise <see langword="false"/>.</returns>
		//public static bool IsUserInRole(string username, string role)
		//{
		//    ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
		//    ArgChecker.ShouldNotBeNullOrEmpty(role, "role");

		//    WindowsPrincipal userPrincipal = new WindowsPrincipal(new WindowsIdentity(username));

		//    return userPrincipal.IsInRole(role);
		//}

		#endregion

		#region Private Helper Methods

		private static SafeUserToken GetCurrentProcessToken(TokenAccessLevels desiredAccess, out int hr)
		{
			hr = 0;
			SafeUserToken handle = SafeUserToken.ZeroToken;
			if (!AdvApi32.OpenProcessToken(Win32.GetCurrentProcess(), desiredAccess, ref handle))
			{
				hr = Win32Helper.GetLastWin32ErrorValue();
			}
			return handle;
		}

		#endregion
	}
}
