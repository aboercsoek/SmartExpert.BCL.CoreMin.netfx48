//--------------------------------------------------------------------------
// File:    LsaAccountManager.cs
// Content:	Implementation of class LsaAccountManager
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using SmartExpert.Error;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.Security.LSA
{

	///<summary>Local Security Authority Account Manager</summary>
	public class LsaAccountManager : IDisposable
	{
		#region Private Members

		private PrincipalContext m_PrincipalContext;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LsaAccountManager"/> class.
		/// </summary>
		public LsaAccountManager()
		{
			m_PrincipalContext = new PrincipalContext(ContextType.Machine);
		}

		#endregion

		#region Dispose Pattern

		private bool m_Disposed;
		private readonly object m_Lock = new object();

		/// <summary>
		/// Gets a value indicating whether this <see cref="LsaAccountManager"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock (m_Lock)
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock (m_Lock)
			{
				if (m_Disposed == false)
				{
					Dispose(true);
				}
			}
		}

		private void CheckDisposed()
		{
			lock (m_Lock)
			{
				if (m_Disposed) throw new AccessToDisposedObjectException("LsaAccountManager");
			}
		}

		private void Dispose(bool isDispose)
		{
			// ################## Dispose Items here
			m_PrincipalContext.Dispose();

			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="LsaAccountManager"/> is reclaimed by garbage collection.
		/// </summary>
		~LsaAccountManager()
		{
			Dispose(false);
		}

		#endregion

		#region User Management

		/// <summary>
		/// Sets the user password
		/// </summary>
		/// <param name="userName">The user name whos password should be changed.</param>
		/// <param name="newPassword">The new password to use.</param>
		/// <returns>Returns <see langword="true"/> if user password was successfully changed; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool SetUserPassword(string userName, string newPassword)
		{
			CheckDisposed();

			try
			{
				UserPrincipal userPrincipal = GetUser(userName);

				if (userPrincipal.IsNull())
					return false;

				userPrincipal.SetPassword(newPassword);
				return true;
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;

				QuickLogger.Log("ERROR: " + ex.RenderExceptionMessage());
			}

			return false;
		}

		/// <summary>
		/// Enables a disabled user account
		/// </summary>
		/// <param name="userName">The user account name to enable.</param>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public void EnableUserAccount(string userName)
		{
			CheckDisposed();

			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal.IsNull())
				return;
			if (userPrincipal.Enabled.HasValue)
			{
				if (userPrincipal.Enabled == true)
					return;
			}

			userPrincipal.Enabled = true;
			userPrincipal.Save();
		}

		/// <summary>
		/// Force disabling of a user account
		/// </summary>
		/// <param name="userName">The name of the user to disable</param>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public void DisableUserAccount(string userName)
		{
			CheckDisposed();

			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal.IsNull())
				return;
			if (userPrincipal.Enabled.HasValue == false)
				return;
			if (userPrincipal.Enabled.Value == false)
				return;

			userPrincipal.Enabled = false;
			userPrincipal.Save();
		}

		/// <summary>
		/// Force that the password of a user expires now.
		/// </summary>
		/// <param name="userName">The user whos password should expire now.</param>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public void ExpireUserPassword(string userName)
		{
			CheckDisposed();

			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal.IsNull())
				return;
			if (userPrincipal.PasswordNeverExpires)
				return;

			userPrincipal.ExpirePasswordNow();
			userPrincipal.Save();

		}

		/// <summary>
		/// Unlocks a locked user account
		/// </summary>
		/// <param name="userName">The username to unlock</param>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public void UnlockUserAccount(string userName)
		{
			CheckDisposed();

			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal.IsNull())
				return;

			userPrincipal.UnlockAccount();
			userPrincipal.Save();
		}

		/// <summary>
		/// Creates a new user on the local machine
		/// </summary>
		/// <param name="userName">The username of the new user</param>
		/// <param name="password">The password of the new user</param>
		/// <param name="firstName">The first name of the new user</param>
		/// <param name="lastName">The last name of the new user</param>
		/// <returns>Returns the <see cref="UserPrincipal"/> object of the user.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public UserPrincipal CreateNewUser(string userName, string password, string firstName, string lastName)
		{
			CheckDisposed();

			if (DoesUserExist(userName))
				return GetUser(userName);

			var userPrincipal = new UserPrincipal(m_PrincipalContext, userName, password, true /*Enabled or not*/)
			                    	{
			                    		UserPrincipalName = userName,
			                    		GivenName = firstName,
			                    		Surname = lastName,
			                    		PasswordNeverExpires = true,
			                    		PasswordNotRequired = false
			                    	};

			userPrincipal.Save();

			return userPrincipal;

		}

		/// <summary>
		/// Deletes a user on the local machine
		/// </summary>
		/// <param name="userName">The username you want to delete</param>
		/// <returns>Returns <see langword="true"/> if user was successfully deleted; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool DeleteUser(string userName)
		{
			CheckDisposed();

			try
			{
				if (userName.IsNullOrEmptyWithTrim())
					return false;

				UserPrincipal userPrincipal = GetUser(userName);

				if (userPrincipal.IsNull())
					return false;

				userPrincipal.Delete();
				return true;
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
				
				QuickLogger.Log("ERROR: " + exception.RenderExceptionMessage());

				return false;
			}
		}

		#endregion

		#region Group Management

		/// <summary>
		/// Creates a new group in Active Directory
		/// </summary>
		/// <param name="groupName">The name of the new group</param>
		/// <param name="sDescription">The description of the new group</param>
		/// <returns>Retruns the GroupPrincipal object</returns>
		/// <exception cref="ArgNullException">Is thrown if groupName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if groupName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public GroupPrincipal CreateNewGroup(string groupName, string sDescription)
		{
			CheckDisposed();
			ArgChecker.ShouldNotBeNullOrEmpty(groupName, "groupName");

			if (DoesGroupExist(groupName))
				return GetGroup(groupName);

			var groupPrincipal = new GroupPrincipal(m_PrincipalContext, groupName)
									{
										Description = sDescription,
										GroupScope = GroupScope.Local
									};

			groupPrincipal.Save();

			return groupPrincipal;
		}

		/// <summary>
		/// Adds the user to a given group
		/// </summary>
		/// <param name="userName">The user you want to add to a group</param>
		/// <param name="groupName">The group the user should be added to</param>
		/// <returns>Returns true if successful</returns>
		/// <exception cref="ArgNullException">Is thrown if userName or groupName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName or groupName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool AddUserToGroup(string userName, string groupName)
		{
			CheckDisposed();

			ArgChecker.ShouldNotBeNullOrEmpty(userName, "userName");
			ArgChecker.ShouldNotBeNullOrEmpty(groupName, "groupName");

			try
			{
				UserPrincipal userPrincipal = GetUser(userName);
				if (userPrincipal == null)
					return false;

				GroupPrincipal groupPrincipal = GetGroup(groupName);
				if (groupPrincipal == null)
					return false;

				if (IsUserGroupMember(userName, groupName))
					return true;

				groupPrincipal.Members.Add(userPrincipal);
				groupPrincipal.Save();

				return true;
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
				
				QuickLogger.Log("ERROR: " + exception.RenderExceptionMessage());

				return false;
			}
		}

		/// <summary>
		/// Removes user from a given group
		/// </summary>
		/// <param name="userName">The user you want to remove from a group</param>
		/// <param name="groupName">The group you want the user to be removed from</param>
		/// <returns>Returns true if successful</returns>
		/// <exception cref="ArgNullException">Is thrown if userName or groupName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName or groupName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool RemoveUserFromGroup(string userName, string groupName)
		{
			CheckDisposed();

			ArgChecker.ShouldNotBeNullOrEmpty(userName, "userName");
			ArgChecker.ShouldNotBeNullOrEmpty(groupName, "groupName");

			try
			{
				UserPrincipal userPrincipal = GetUser(userName);
				if (userPrincipal == null)
					return false;

				GroupPrincipal groupPrincipal = GetGroup(groupName);
				if (groupPrincipal == null)
					return false;

				if (IsUserGroupMember(userName, groupName) == false)
					return true;

				groupPrincipal.Members.Remove(userPrincipal);
				groupPrincipal.Save();


				return true;
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;
				
				QuickLogger.Log("ERROR: " + exception.RenderExceptionMessage());

				return false;
			}
		}

		/// <summary>
		/// Checks if a group is a member of a given group
		/// </summary>
		/// <param name="subGroupName">The name of the group you want to check</param>
		/// <param name="groupName">The parent group name to check the membership.</param>
		/// <returns>Returns <see langword="true"/> if the group is a group member; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool IsSubGroup(string subGroupName, string groupName)
		{
			CheckDisposed();

			if (subGroupName.IsNullOrEmptyWithTrim())
				return false;

			if (groupName.IsNullOrEmptyWithTrim())
				return false;

			GroupPrincipal subGroupPrincipal = GetGroup(subGroupName);
			if (subGroupPrincipal == null)
				return false;

			GroupPrincipal groupPrincipal = GetGroup(groupName);
			
			return (groupPrincipal.IsNotNull() && groupPrincipal.Members.Contains(subGroupPrincipal));
		}

		/// <summary>
		/// Checks if a user is a member of a given group
		/// </summary>
		/// <param name="userName">The user you want to check</param>
		/// <param name="groupName">The group to check the membership.</param>
		/// <returns>Returns <see langword="true"/> if the user is a group member; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool IsUserGroupMember(string userName, string groupName)
		{
			CheckDisposed();

			if (userName.IsNullOrEmptyWithTrim())
				return false;

			if (groupName.IsNullOrEmptyWithTrim())
				return false;

			UserPrincipal userPrincipal = GetUser(userName);
			if (userPrincipal == null)
				return false;

			GroupPrincipal groupPrincipal = GetGroup(groupName);
			
			return (groupPrincipal.IsNotNull() && groupPrincipal.Members.Contains(userPrincipal));
		}

		/// <summary>
		/// Gets the users group memberships.
		/// </summary>
		/// <param name="userName">The user to get the group memberships</param>
		/// <returns>Returns the group memberships of the user.</returns>
		/// <example>
		/// <code lang="cs" title="GetUserGroups example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetUserGroups" />
		/// </example>		
		/// <exception cref="ArgNullException">Is thrown if userName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public IEnumerable<GroupPrincipal> GetUserGroups(string userName)
		{
			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal == null)
				return Enumerable.Empty<GroupPrincipal>();

			PrincipalSearchResult<Principal> principalSearchResult = userPrincipal.GetGroups();

			return principalSearchResult.AsSequence<GroupPrincipal>().ToList();
		}

		/// <summary>
		/// Gets the users authorization groups.
		/// </summary>
		/// <param name="userName">The user to get the authorization group memberships.</param>
		/// <returns>Returns authorization groups of the user.</returns>
		/// <example>
		/// <code lang="cs" title="GetUserAuthorizationGroups example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetUserAuthorizationGroups" />
		/// </example>		
		/// <exception cref="ArgNullException">Is thrown if userName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public IEnumerable<GroupPrincipal> GetUserAuthorizationGroups(string userName)
		{
			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal == null)
				return Enumerable.Empty<GroupPrincipal>();

			PrincipalSearchResult<Principal> principalSearchResult = userPrincipal.GetAuthorizationGroups();

			return principalSearchResult.AsSequence<GroupPrincipal>().ToList();
		}


		#endregion

		#region User & Group Search

		/// <summary>
		/// Gets a certain user on the local machine
		/// </summary>
		/// <param name="userName">The user name to get</param>
		/// <returns>Returns the UserPrincipal Object for the specified user name or <see langword="null"/> if user does not exist.</returns>
		/// <example>
		/// <code lang="cs" title="GetUser example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetUser" />
		/// </example>
		/// <exception cref="ArgNullException">Is thrown if userName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public UserPrincipal GetUser(string userName)
		{
			CheckDisposed();
			ArgChecker.ShouldNotBeNullOrEmpty(userName, "userName");

			var userPrincipal = UserPrincipal.FindByIdentity(m_PrincipalContext, userName);
			return userPrincipal;
		}

		/// <summary>
		/// Gets a certain group on the loacl machine
		/// </summary>
		/// <param name="groupName">The group to get</param>
		/// <returns>Returns the GroupPrincipal Object</returns>
		/// <example>
		/// <code lang="cs" title="GetGroup example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetGroup" />
		/// </example>
		/// <exception cref="ArgNullException">Is thrown if groupName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if groupName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public GroupPrincipal GetGroup(string groupName)
		{
			CheckDisposed();
			ArgChecker.ShouldNotBeNullOrEmpty(groupName, "groupName");

			var groupPrincipal = GroupPrincipal.FindByIdentity(m_PrincipalContext, groupName);
			return groupPrincipal;
		}

		/// <summary>
		/// Gets all local users.
		/// </summary>
		/// <returns>A sequence with all local users.</returns>
		/// <example>
		/// <code lang="cs" title="GetAllLocalUsers example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalUsers" />
		/// </example>
		public IEnumerable<UserPrincipal> GetAllLocalUsers()
		{
			using (var u = new UserPrincipal(m_PrincipalContext))
			{
				// Create a PrincipalSearcher object.     
				var ps = new PrincipalSearcher(u);

				// Searches for all security groups
				using (PrincipalSearchResult<Principal> results = ps.FindAll())
				{
					return results.AsSequence<UserPrincipal>().ToList();
				}
			}
		}

		/// <summary>
		/// Gets all local groups.
		/// </summary>
		/// <returns>A sequence with all local groups.</returns>
		/// <example>
		/// <code lang="cs" title="GetAllLocalGroups example" numberLines="false" outlining="true" source=".\examples\Sample_CoreMin_SecurityLsaAccountManager.cs" region="Sample_CoreMin_Security_M_LsaAccountManager_GetAllLocalGroups" />
		/// </example>		
		public IEnumerable<GroupPrincipal> GetAllLocalGroups()
		{
			using (var g = new GroupPrincipal(m_PrincipalContext))
			{
				// Create a PrincipalSearcher object.     
				var ps = new PrincipalSearcher(g);

				// Searches for all security groups
				using (PrincipalSearchResult<Principal> results = ps.FindAll())
				{
					return results.AsSequence<GroupPrincipal>().ToList();
				}
			}
		}

		#endregion

		#region Validate Methods

		/// <summary>
		/// Validates the username and password credentials.
		/// </summary>
		/// <param name="userName">The username to validate</param>
		/// <param name="password">The password of the username to validate</param>
		/// <returns>Returns <see langword="true"/> if user name and password are valid; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgNullException">Is thrown if userName or password are <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool ValidateCredentials(string userName, string password)
		{
			CheckDisposed();
			ArgChecker.ShouldNotBeNullOrEmpty(userName, "userName");
			ArgChecker.ShouldNotBeNull(password, "password");

			return m_PrincipalContext.ValidateCredentials(userName, password);

		}

		/// <summary>
		/// Checks if the User Account is expired
		/// </summary>
		/// <param name="userName">The username to check</param>
		/// <returns>Returns <see langword="true"/> if expired; otherwise <see langword="false"/>.</returns>
		/// <exception cref="ArgNullException">Is thrown if userName is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if userName is empty.</exception>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool IsUserExpired(string userName)
		{
			CheckDisposed();
			ArgChecker.ShouldNotBeNullOrEmpty(userName, "userName");

			UserPrincipal userPrincipal = GetUser(userName);

			if (userPrincipal.IsNull())
				return true;

			return userPrincipal.AccountExpirationDate == null;
		}

		/// <summary>
		/// Checks if user exsists on the local machine
		/// </summary>
		/// <param name="userName">The user name to check</param>
		/// <returns>Returns <see langword="true"/> if a user with the specified user name exists; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool DoesUserExist(string userName)
		{
			CheckDisposed();

			if (userName.IsNullOrEmptyWithTrim())
				return false;

			return GetUser(userName) != null;
		}

		/// <summary>
		/// Checks if group exsists on the local machine
		/// </summary>
		/// <param name="groupName">The group name to check</param>
		/// <returns>Returns <see langword="true"/> if a group with the specified group name exists; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool DoesGroupExist(string groupName)
		{
			CheckDisposed();

			if (groupName.IsNullOrEmptyWithTrim())
				return false;

			return GetGroup(groupName) != null;
		}

		/// <summary>
		/// Checks if user account is locked.
		/// </summary>
		/// <param name="userName">The username to check</param>
		/// <returns>Retruns <see langword="true"/> if Account is locked; otherwise <see langword="false"/>.</returns>
		/// <exception cref="AccessToDisposedObjectException">Thrown if access to this method occure after the object was disposed.</exception>
		public bool IsAccountLocked(string userName)
		{
			CheckDisposed();

			if (DoesUserExist(userName).IsFalse())
				return false;

			UserPrincipal userPrincipal = GetUser(userName);
			return userPrincipal.IsAccountLockedOut();
		}

		#endregion


		/// <summary>
		/// Gets the name of the IIS worker process group.
		/// </summary>
		/// <value>
		/// The name of the IIS worker process group.
		/// </value>
		public string IisWorkerProcesgroupName
		{
			get
			{
				return Environment.OSVersion.Version.Major <= 5 ? "IIS_WPG" : "IIS_IUSRS";
			}
		}
	}

	// ReSharper restore InconsistentNaming
}
