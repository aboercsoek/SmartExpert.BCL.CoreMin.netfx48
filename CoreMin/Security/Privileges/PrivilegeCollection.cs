//--------------------------------------------------------------------------
// File:    PrivilegeCollection.cs
// Content:	Implementation of class PrivilegeCollection
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using SmartExpert.Interop;

#endregion

namespace SmartExpert.Security.Privileges
{
	/// <summary>
	/// This class provides function for enumeration and manipulation of windows privileges
	/// granted to <see cref="WindowsIdentity"/>.
	/// </summary>
	public class PrivilegeCollection : IEnumerable<string>
	{
		#region Private Fields

		private readonly WindowsIdentity m_Identity;
		private readonly Dictionary<string, Privilege> m_Privileges;

		#endregion

		#region Ctors

		/// <summary>
		/// Creates a new instance of this class for the specified <see cref="Identity"/>
		/// </summary>
		/// <param name="identity">The <see cref="WindowsIdentity"/> to use</param>
		public PrivilegeCollection(WindowsIdentity identity)
		{
			m_Privileges = new Dictionary<string, Privilege>();

			m_Identity = identity;
			Refresh();
		}

		/// <summary>
		/// Creates a new instance of this class using the current <see cref="WindowsIdentity"/>
		/// </summary>
		public PrivilegeCollection()
			: this(WindowsIdentity.GetCurrent())
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns the <see cref="WindowsIdentity"/> associated with this instance.
		/// </summary>
		public WindowsIdentity Identity
		{
			get { return m_Identity; }
		}

		/// <summary>
		/// Returns the number of privileges held by the associated <see cref="Identity"/>
		/// </summary>
		public int Count
		{
			get { return m_Privileges.Count; }
		}

		/// <summary>
		/// Returns the privilege with the specified name.
		/// </summary>
		/// <param name="privilegeName">The name of the privilege</param>
		/// <returns>The <see cref="Privilege"/>, if that privilege is available in the associated <see cref="WindowsIdentity"/></returns>
		/// <exception cref="SmartExpert.Error.PrivilegeNotHeldException">If the specified privilege is not held by the <see cref="WindowsIdentity"/></exception>
		public Privilege this[string privilegeName]
		{
			get
			{
				if (!m_Privileges.ContainsKey(privilegeName))
					throw new SmartExpert.Error.PrivilegeNotHeldException("The requested privilege is not held by the specified token.");

				return m_Privileges[privilegeName];
			}
		}

		#endregion

		#region IEnumerable<string> Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Privileges.Keys.GetEnumerator();
		}

		/// <summary>
		/// Returns an <see cref="IEnumerator{String}" /> which can be used to enumerate
		/// through the names of associated <see cref="WindowsIdentity"/>
		/// </summary>
		/// <returns>An <see cref="IEnumerator{String}"/> instance.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			return m_Privileges.Keys.GetEnumerator();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Removes all unnecessary priviliges from the associated <see cref="Identity"/> 
		/// </summary>
		public void ReducePrivilegesToMinimum(params string[] exceptions)
		{
			Privilege changeNotifyPrivilege = null;

			var privilegesToRemove = new List<PrivilegeAction>();

			foreach (var entry in m_Privileges.Where(pair => !exceptions.Contains(pair.Key)))
			{
				if (entry.Key.Equals(Privilege.ChangeNotify, StringComparison.InvariantCultureIgnoreCase))
				{
					changeNotifyPrivilege = entry.Value;
					continue;
				}

				privilegesToRemove.Add(new PrivilegeAction(entry.Value.Luid, PrivilegeState.Remove));
			}
			Privilege.AdjustTokenPrivileges(m_Identity, false, privilegesToRemove.ToArray());
			m_Privileges.Clear();

			if (changeNotifyPrivilege != null) m_Privileges.Add(changeNotifyPrivilege.Name, changeNotifyPrivilege);
		}

		/// <summary>
		/// Reloads the current state of all privileges from the <see cref="WindowsIdentity"/>
		/// </summary>
		public void Refresh()
		{
			uint size;
			Privilege privilege;
			TOKEN_PRIVILEGE result;

			if (!AdvApi32.GetTokenInformation(m_Identity.Token, TokenInformationClass.TokenPrivileges, SafeTokenPrivileges.NullHandle, 0, out size))
			{
				int lastError = Marshal.GetLastWin32Error();

				if (lastError != 0x18 && lastError != 0x7a) // 0x7a = Databuffer to small; this is the expected value
					throw new Win32Exception(lastError);
			}

			using (var handle = new SafeTokenPrivileges(size))
			{
				if (!AdvApi32.GetTokenInformation(m_Identity.Token, TokenInformationClass.TokenPrivileges, handle, size, out size))
					throw new Win32Exception(Marshal.GetLastWin32Error());

				result = handle.MarshalNativeToManaged();
			}

			m_Privileges.Clear();
			foreach (LUID_AND_ATTRIBUTES entry in result.Privileges)
			{
				privilege = new Privilege(this, m_Identity, entry.Luid, (PrivilegeState)entry.Attributes);

				m_Privileges.Add(privilege.Name, privilege);
			}
		}

		#endregion

		#region Internal Methods

		internal void RemovePrivilege(string name)
		{
			m_Privileges.Remove(name);
		}

		#endregion
	}
}
