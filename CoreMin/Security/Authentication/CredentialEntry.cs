//--------------------------------------------------------------------------
// File:    CredentialEntry.cs
// Content:	Implementation of class CredentialEntry
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// Represents a windows identity store.
	/// </summary>
	public class CredentialEntry : IDisposable
	{
		#region Private Fields

		private object m_SyncRoot = new object();

		private SafeUserToken m_Handle;
		private LogonType m_LogonType;
		private DateTime m_Created;
		private DateTime m_LastAccessed;
		private string m_Username;
		private string m_Domain;
		private SecureString m_SecurePassword;

		#endregion

		#region Ctor

		/// <summary>
		/// Creates a new instance of this class and logs on as the specified user.
		/// </summary>
		/// <param name="username">The name of the user to log on to (either as 'username' or as 'domain\username')</param>
		/// <param name="password">The password of the useraccount</param>
		/// <param name="logonType">The <see cref="LogonType"/> to use.</param>
		/// <exception cref="ArgNullException">Is thrown is <paramref name="username"/> or <paramref name="password"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown is <paramref name="username"/> is <see cref="F:System.String.Empty"/>.</exception>
		public CredentialEntry(string username, SecureString password, LogonType logonType)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(username, "username");
			ArgChecker.ShouldNotBeNull(password, "password");

			Created = DateTime.Now;
			LogonType = logonType;

			string[] parts = username.Split('\\');
			if (parts.Length == 2)
			{
				m_Domain = parts[0];
				m_Username = parts[1];
			}
			else m_Username = username;

			m_SecurePassword = password;
		}

		#endregion

		#region Dispose Pattern

		private bool m_Disposed;

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="CredentialEntry"/> is reclaimed by garbage collection.
		/// </summary>
		~CredentialEntry()
		{
			Cleanup();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock (m_SyncRoot)
			{
				//Check to see if Dispose(  ) has already been called
				if (m_Disposed == false)
				{
					Cleanup();
					m_Disposed = true;
					//Take yourself off the finalization queue
					//to prevent finalization from executing a second time.
					GC.SuppressFinalize(this);
				}
			}

		}

		/// <summary>
		/// .Cleanup used resources
		/// </summary>
		protected virtual void Cleanup()
		{
			/*Do cleanup here*/
			SafeUserToken token = Interlocked.Exchange(ref m_Handle, null);
			if (token != null) token.Dispose();
			m_SecurePassword.Dispose();
		}

		/// <summary>
		/// Determines whether this instance is disposed.
		/// </summary>
		/// <exception cref="ObjectDisposedException">Is thrown if the instance is disposed.</exception>
		protected void IsDisposed()
		{
			if (Disposed)
				throw new ObjectDisposedException("CredentialEntry");
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="CredentialEntry"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock (m_SyncRoot)
				{
					return m_Disposed;
				}
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the <see cref="LogonType"/> to use.
		/// </summary>
		public LogonType LogonType
		{
			get { IsDisposed(); return m_LogonType; }
			set { IsDisposed(); m_LogonType = value; }
		}

		/// <summary>
		/// Returns the <see cref="DateTime"/> when the object was created.
		/// </summary>
		public DateTime Created
		{
			get { IsDisposed(); return m_Created; }
			private set { IsDisposed(); m_Created = value; }
		}

		/// <summary>
		/// Gets or sets the username for this <see cref="CredentialEntry"/>
		/// </summary>
		public string Username
		{
			get { IsDisposed(); LastAccessed = DateTime.Now; return m_Username; }
			set { IsDisposed(); m_Username = value; }
		}
		/// <summary>
		/// Gets or sets the domain for this <see cref="CredentialEntry"/>
		/// </summary>
		public string Domain
		{
			get { IsDisposed(); LastAccessed = DateTime.Now; return m_Domain; }
			set { IsDisposed(); m_Domain = value; }
		}

		/// <summary>
		/// Gets or sets the password for this <see cref="CredentialEntry"/>
		/// </summary>
		public SecureString Password
		{
			get { IsDisposed(); LastAccessed = DateTime.Now; return m_SecurePassword; }
			set
			{
				IsDisposed(); 
				if (value.IsNotNull())
				{
					m_SecurePassword.Dispose();
					m_SecurePassword = value;
				}
			}
		}

		/// <summary>
		/// Returns the <see cref="DateTime"/> when the object was last accessed.
		/// </summary>
		public DateTime LastAccessed
		{
			get { IsDisposed(); return m_LastAccessed; }
			private set { IsDisposed(); m_LastAccessed = value; }
		}

		/// <summary>
		/// Returns the <see cref="WindowsIdentity"/> represented by this instance.
		/// </summary>
		public WindowsIdentity Identity
		{
			get { IsDisposed(); LastAccessed = DateTime.Now; return GetWindowsIdentity(); }
		}

		#endregion

		#region Private Helper Methods

		private WindowsIdentity GetWindowsIdentity()
		{
			IsDisposed();
			IntPtr passwordPointer = IntPtr.Zero;

			lock (m_SyncRoot)
			{
				// Probe for sufficient stack space
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					passwordPointer = Marshal.SecureStringToGlobalAllocUnicode(m_SecurePassword);

					LogonProviderType provider = (LogonType == LogonType.NewCredentials) ? LogonProviderType.WinNT50 : LogonProviderType.Default;

					bool result = AdvApi32.LogonUser2(m_Username, m_Domain, passwordPointer, LogonType, provider, out m_Handle);

					if (!result)
					{
						throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
					}
				}
				finally
				{
					if (passwordPointer != IntPtr.Zero) Marshal.ZeroFreeGlobalAllocUnicode(passwordPointer);
				}
			}

			return m_Handle.GetWindowsIdentity();
		}

		#endregion
	}
}
