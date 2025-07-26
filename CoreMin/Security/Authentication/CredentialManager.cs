//--------------------------------------------------------------------------
// File:    CredentialManager.cs
// Content:	Implementation of class CredentialManager
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Security;

#endregion

namespace SmartExpert.Security.Authentication
{
	/// <summary>
	/// This allows the caching of user credential in the form of <see cref="CredentialEntry"/> objects for a certain amount of time.
	/// </summary>
	public static class CredentialManager
	{
		#region Private static Fields

		// Used to synchronize access to the credential cache
		private static object ms_SyncRoot = new object();
		
		// Credential cache
		private static Dictionary<string, CredentialEntry> ms_Cache = new Dictionary<string, CredentialEntry>(0, StringComparer.CurrentCultureIgnoreCase);
		
		// Default: 5 minutes
		private static TimeSpan? ms_SlidingExpirationTimeout = new TimeSpan(0, 5, 0);
		// Default: 1 hour
		private static TimeSpan? ms_AbsoluteExpirationTimeout = new TimeSpan(1, 0, 0);

		#endregion

		#region Public static Properties

		/// <summary>
		/// Gets or sets the timeout for the sliding expiration. If SlidingExpiration is null,
		/// this feature is disabled.
		/// </summary>
		public static TimeSpan? SlidingExpirationTimeout
		{
			get { return ms_SlidingExpirationTimeout; }
			set { ms_SlidingExpirationTimeout = value; }
		}


		/// <summary>
		/// Gets or sets the timeout for the absolute expiration. If AbsoluteExpiration is null,
		/// this feature is disabled.
		/// </summary>
		public static TimeSpan? AbsoluteExpirationTimeout
		{
			get { return ms_AbsoluteExpirationTimeout; }
			set { ms_AbsoluteExpirationTimeout = value; }
		}

		#endregion

		#region Public static Methods

		/// <summary>
		/// Adds a WindowsIdentity to the CredentialManager.
		/// </summary>
		/// <param name="id">The targetName for the identity</param>
		/// <param name="username">The username, either in the form 'username' or 'domain\username'</param>
		/// <param name="password">The passsword of the user</param>
		/// <param name="logonType">The <see cref="LogonType"/> to use.</param>
		/// <returns>The <see cref="CredentialEntry"/> for the specified user, or <see langword="null"/> 
		/// if an entry with the same <paramref name="id"/> already exists in the cache.</returns>
		public static CredentialEntry Add(string id, string username, SecureString password, LogonType logonType)
		{
			lock (ms_SyncRoot)
			{
				if (ms_Cache.ContainsKey(id) == false)
				{
					CredentialEntry entry = new CredentialEntry(username, password, logonType);
					ms_Cache.Add(id, entry);

					return entry;
				}
				return null;
			}
		}

		/// <summary>
		/// Adds the credential entry with the specified id to the credential manager.
		/// </summary>
		/// <param name="id">The target nmae of the entry</param>
		/// <param name="entry">The <see cref="CredentialEntry"/> to store.</param>
		/// <returns>The <see cref="CredentialEntry"/> that was specified by the entry parameter, or <see langword="null"/> 
		/// if an entry with the same <paramref name="id"/> already exists in the cache.</returns>
		public static CredentialEntry Add(string id, CredentialEntry entry)
		{
			lock (ms_SyncRoot)
			{
				if (ms_Cache.ContainsKey(id) == false)
				{
					ms_Cache.Add(id, entry);
					return entry;
				}

				return null;
			}
		}

		/// <summary>
		/// Returns the <see cref="CredentialEntry"/> with the specified id
		/// </summary>
		/// <param name="id">The id of stored credential</param>
		/// <returns>A <see cref="CredentialEntry"/> object, or <see langword="null"/> if the identity was not found.</returns>
		/// <remarks>
		/// GetIdentity checks the expiration timeouts before returning the identity. 
		/// If an entry is considered as expired it will be disposed and removed from the cache. 
		/// In that case <see langword="null"/> will be return to the caller.
		/// </remarks>
		public static CredentialEntry GetIdentity(string id)
		{
			CredentialEntry entry;

			lock (ms_SyncRoot)
			{
				if (ms_Cache.ContainsKey(id).IsFalse()) return null;
				entry = ms_Cache[id];
			}
			if ((AbsoluteExpirationTimeout.HasValue &&
				entry.Created + AbsoluteExpirationTimeout < DateTime.Now) ||
				(SlidingExpirationTimeout.HasValue && entry.LastAccessed + SlidingExpirationTimeout < DateTime.Now))
			{

				lock (ms_SyncRoot)
				{
					entry.Dispose();
					ms_Cache.Remove(id);
				}

				return null;
			}

			return entry;
		}

		/// <summary>
		/// Determines whether the identity with the specified id is stored in the cache.
		/// </summary>
		/// <param name="id">The identity id.</param>
		/// <returns>
		/// 	<see langword="true"/> if the cache contains the specified id; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool Contains(string id)
		{
			lock (ms_SyncRoot)
			{
				return ms_Cache.ContainsKey(id);
			}
		}

		/// <summary>
		/// Removes the identity with the specified id from the cache.
		/// </summary>
		/// <param name="id">The id to remove.</param>
		/// /// <remarks>The credentrial entry with the specified id will be disposed and than removed from the cache.</remarks>
		public static void Remove(string id)
		{
			lock (ms_SyncRoot)
			{
				if (ms_Cache.ContainsKey(id))
				{
					CredentialEntry entry = ms_Cache[id];
					entry.Dispose();
					ms_Cache.Remove(id);
				}
			}
		}

		/// <summary>
		/// Clears the credentials cache.
		/// </summary>
		/// <remarks>Every credentrial entry in the cache will be disposed and than removed from the cache.</remarks>
		public static void Clear()
		{
			lock (ms_SyncRoot)
			{
				foreach(var id in ms_Cache.Keys)
				{
					CredentialEntry entry = ms_Cache[id];
					entry.Dispose();
				}
				ms_Cache.Clear();
			}
		}

		#endregion

	}
}
