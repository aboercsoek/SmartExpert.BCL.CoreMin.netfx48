//--------------------------------------------------------------------------
// File:    IdentityResolver.cs
// Content:	Implementation of class IdentityResolver
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using SmartExpert.Error;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Identity
{
	/// <summary>
	/// Provides the ability to translate a <see cref="NTAccount"/> to a <see cref="SecurityIdentifier"/>
	/// and vice versa, optionally using a remote computer for the translation process.
	/// </summary>
	public class IdentityResolver : IEquatable<IdentityReference>
	{
		#region Private Fields

		private readonly string m_ComputerName;
		private NTAccount m_Account;
		private AccountType m_AccountType;
		private SecurityIdentifier m_Sid;

		#endregion

		#region Ctors

		/// <summary>
		/// Creates a new <see cref="IdentityResolver"/> instance. 
		/// A remote computer is not used for the translation process
		/// </summary>
		/// <param name="identity">The identity to translate</param>
		/// <remarks><see cref="IdentityReference"/> is the abstract base class of the types: <see cref="SecurityIdentifier"/> and <see cref="NTAccount"/></remarks>
		public IdentityResolver(IdentityReference identity) : this(identity, null) { }

		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="identity">The identity to translate</param>
		/// <param name="computerName">The computer to use for the translation</param>
		/// <remarks>The remote computer is not used for translation, if
		/// the provided identity is a <see cref="SecurityIdentifier"/> and the domain
		/// sid of that identity equals the domain sid of the current user</remarks>
		public IdentityResolver(IdentityReference identity, string computerName)
		{
			m_Sid = identity as SecurityIdentifier;
			m_Account = identity as NTAccount;
			
			WindowsIdentity wi = WindowsIdentity.GetCurrent();
			SecurityIdentifier domainSid = (wi == null) ? null : wi.User;

			if (m_Sid == null || !m_Sid.IsEqualDomainSid(domainSid))
			{
				m_ComputerName = computerName;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns the <see cref="SecurityIdentifier"/> of this instance.
		/// </summary>
		public SecurityIdentifier Sid
		{
			get
			{
				if (m_Sid == null) TranslateFromNTAccount();
				return m_Sid;
			}
		}

		/// <summary>
		/// Returns the <see cref="NTAccount"/> of this instance.
		/// </summary>
		public NTAccount Account
		{
			get
			{
				if (m_Account == null) TranslateFromSecurityDescriptor();
				return m_Account;
			}
		}

		/// <summary>
		/// Returns the <see cref="AccountType"/> of this instance
		/// </summary>
		public AccountType AccountType
		{
			get
			{
				if (m_Sid == null) TranslateFromNTAccount();
				else if (m_Account == null) TranslateFromSecurityDescriptor();

				return m_AccountType;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Tries to translate IdentityReference to SID or NTAccount.
		/// </summary>
		/// <returns>true if translation was successfull, false if translation failed.</returns>
		public bool TryTranslation()
		{
			if (m_Sid != null && m_Account == null)
			{
				NTAccount account;
				if (TryTranslateFromSecurityDescriptor(out account)) return true;
			}
			else if (m_Sid == null && m_Account == null)
			{
				SecurityIdentifier sid;
				if (TryTranslateFromNTAccount(out sid)) return true;
			}
			return false;
		}

		/// <summary>
		/// Compares this identity with the specified othe identity.
		/// </summary>
		/// <param name="other">The identity to compare this identity with.</param>
		/// <returns>True, if both identities represent the same identity</returns>
		public bool Equals(IdentityReference other)
		{
			if (other == null) return false;

			if (other is NTAccount) return Account.Equals(other);
			if (other is SecurityIdentifier) Sid.Equals((SecurityIdentifier)other);

			return false;
		}

		#endregion

		#region Private Helper Methods

// ReSharper disable InconsistentNaming
		private void TranslateFromNTAccount()
// ReSharper restore InconsistentNaming
		{
			SecurityIdentifier sid;
			bool result = TryTranslateFromNTAccount(out sid);

			CheckTranslationResult(result);

			m_Sid = sid;
		}

// ReSharper disable InconsistentNaming
		private bool TryTranslateFromNTAccount(out SecurityIdentifier sid)
// ReSharper restore InconsistentNaming
		{
			var binarySid = new byte[SecurityIdentifier.MaxBinaryLength];
			var binarySidLength = (uint)binarySid.Length;
			var referencedDomain = new StringBuilder(0xff);
			var referencedDomainLength = (uint)referencedDomain.Capacity;

			bool result = AdvApi32.LookupAccountName(m_ComputerName, m_Account.Value,
														  binarySid, ref binarySidLength, referencedDomain,
														  ref referencedDomainLength, out m_AccountType);

			sid = new SecurityIdentifier(binarySid, 0);
			return result;
		}

		private void CheckTranslationResult(bool result)
		{
			if (!result)
			{
				var error = (WindowsStatusCode)Marshal.GetLastWin32Error();
				if (error == WindowsStatusCode.NoMappingPerformed)
				{
					throw new IdentityNotMappedException(string.Format("Failed to retrieve the SID for user {0}. No mapping was performed by the operating system.", m_Account.Value));
				}
				throw new Win32ExecutionException((int)error, "LookupAccount");
			}
		}

		private void TranslateFromSecurityDescriptor()
		{
			NTAccount account;
			bool result = TryTranslateFromSecurityDescriptor(out account);

			CheckTranslationResult(result);

			m_Account = account;
		}

		private bool TryTranslateFromSecurityDescriptor(out NTAccount account)
		{
			var name = new StringBuilder(0x100);
			var domain = new StringBuilder(0xff);

			uint nameLength = (uint)name.Capacity - 2;
			uint domainLength = (uint)domain.Capacity - 2;

			var binarySid = new byte[m_Sid.BinaryLength];
			m_Sid.GetBinaryForm(binarySid, 0);

			bool result = AdvApi32.LookupAccountSid(
				m_ComputerName, binarySid, name, ref nameLength, domain, ref domainLength, out m_AccountType);
			account = new NTAccount(domain.ToString(), name.ToString());
			return result;
		}

		#endregion
	}
}
