//--------------------------------------------------------------------------
// File:    ConsolePromptForCredential.cs
// Content:	Implementation of class ConsolePromptForCredential
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Security;
using System.Text;
using System.Threading;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// Querying the user for a username / password combination on the console.
	/// </summary>
	public class ConsolePromptForCredential: IDisposable
	{
		#region Private Fields
		private string m_Username = "";
		private SecureString m_Password;
		private string m_TargetName = "";

		private bool m_SaveCredential;

		private QueryCredentialOptions m_Options = QueryCredentialOptions.CompleteUsername | QueryCredentialOptions.DoNotPersist;
		#endregion

		#region Ctors

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="targetName">The target name for this request.</param>
		public ConsolePromptForCredential(string targetName)
			: this(targetName, null)
		{ }

		/// <summary>
		/// Creates a new instance of this class, using the specified username.
		/// </summary>
		/// <param name="targetName">The target name for this request.</param>
		/// <param name="username">The username for this request.</param>
		public ConsolePromptForCredential(string targetName, string username)
			: this(targetName, username, QueryCredentialOptions.CompleteUsername | QueryCredentialOptions.DoNotPersist)
		{ }

		/// <summary>
		/// Creates a new instance of this class, using the specified username.
		/// </summary>
		/// <param name="targetName">The target name for this request.</param>
		/// <param name="options">The options for this request.</param>
		public ConsolePromptForCredential(string targetName, QueryCredentialOptions options)
			: this(targetName, null, options)
		{ }

		/// <summary>
		/// Creates a new instance of this class, using the specified username.
		/// </summary>
		/// <param name="targetName">The target name for this request.</param>
		/// <param name="options">The options for this request.</param>
		/// <param name="username">The username for this request.</param>
		public ConsolePromptForCredential(string targetName, string username, QueryCredentialOptions options)
		{
			m_TargetName = targetName;
			m_Options = options;
			m_Username = username;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// The targetname for the credential.
		/// </summary>
		public string TargetName
		{
			get { return m_TargetName; }
			set { m_TargetName = value; }
		}

		/// <summary>
		/// The <see cref="QueryCredentialOptions"/> used when displaying the dialog.
		/// </summary>
		public QueryCredentialOptions Options
		{
			get { return m_Options; }
			set { m_Options = value; }
		}

		/// <summary>
		/// Gets or sets the username used for this request. If set prior calling <see cref="Prompt"/>,
		/// the user is only prompted for a password.
		/// After a successful call to <see cref="Prompt"/>, this property contains the
		/// username specified by the user.
		/// </summary>
		public string Username
		{
			get { return m_Username; }
			set { m_Username = value; }
		}

		/// <summary>
		/// A <see cref="SecureString"/> containing the password.
		/// </summary>
		public SecureString Password
		{
			get { return m_Password; }
			set { m_Password = value; }
		}

		/// <summary>
		/// After a successful call to <see cref="Prompt"/>, this property returns true if the 
		/// user chose "Save password". False, otherwise.
		/// </summary>
		public bool SaveCredential
		{
			get { return m_SaveCredential; }
			set { m_SaveCredential = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Prompts the user for a username and/or password.
		/// </summary>
		/// <returns>True, if a username/password combination has been entered. False, otherwise.</returns>
		public bool Prompt()
		{
			StringBuilder username;
			char[] password = new char[256 + 1];

			unsafe
			{
				fixed (char* buffer = password)
				{
					Array.Clear(password, 0, password.Length);

					try
					{
						username = new StringBuilder(256+1);
						if (!string.IsNullOrEmpty(Username)) username.Append(Username);

						bool savePassword = m_SaveCredential;

						QueryCredentialError result = CredUi.CredUICmdLinePromptForCredentials(
							TargetName, IntPtr.Zero, 0, username, username.Capacity, buffer, 256+1, ref savePassword, Options);

						if (result == QueryCredentialError.Cancelled) return false;

						if (result != QueryCredentialError.None)
							throw new QueryCredentialDialogException(result, "An exception occured querying the user credentials.");

						m_SaveCredential = savePassword;
						m_Username = username.ToString();

						int length = 0;
						while (length < password.Length && password[length] != 0) length++;

						m_Password = new SecureString(buffer, length);
					}
					finally
					{
						Array.Clear(password, 0, password.Length);
					}
				}
			}
	   
			return true;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Releases all managed and unmanaged resources held by this instance
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Releases all managed and unmanaged resources held by this instance
		/// </summary>
		/// <param name="disposing">True, if called from the dispose method. False, otherwise</param>
		protected virtual void Dispose(bool disposing)
		{
			SecureString str;

			if (disposing)
			{
				str = Interlocked.Exchange(ref m_Password, null);
				if (str != null) str.Dispose();
			}
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ConsolePromptForCredential"/> is reclaimed by garbage collection.
		/// </summary>
		~ConsolePromptForCredential()
		{
			Dispose(false);
		}

		#endregion
	}
}
