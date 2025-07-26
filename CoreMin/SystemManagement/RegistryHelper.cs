//--------------------------------------------------------------------------
// File:    RegistryHelper.cs
// Content:	Implementation of class RegistryHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using System.Security;
using SmartExpert;
using SmartExpert.Linq;
using Microsoft.Win32;

#endregion

namespace SmartExpert.SystemManagement
{
	///<summary>Provides <see cref="Registry"/> helper methods.</summary>
	public static class RegistryHelper
	{
		/// <summary>
		/// Gets a registry value from the <see cref="Registry.LocalMachine">local machine</see> registry store.
		/// </summary>
		/// <param name="subKeyName">Name of the sub key.</param>
		/// <param name="keyName">Name of the key.</param>
		/// <returns>
		///		The value associated with the specified <paramref name="keyName"/>. 
		///		Returns <see langword="null"/> if the name/value pair does not exist in the specified subkey. 
		/// </returns>
		public static object GetRegistryValue( string subKeyName, string keyName )
		{
			RegistryKey key = null;
			
			try
			{
				key = Registry.LocalMachine.OpenSubKey(subKeyName, false);
				if ( key != null )
				{
					object obj = key.GetValue(keyName);
					if ( obj != null )
					{
						return obj;
					}
				}
			}
			catch ( ArgumentException )
			{
				return null;
			}
			catch ( ObjectDisposedException )
			{
				return null;
			}
			catch ( SecurityException )
			{
				return null;
			}
			finally
			{
				if ( key != null )
				{
					key.Close();
				}
			}
			return null;
		}


		/// <summary>
		/// Gets a registry key from the local machine registry store.
		/// </summary>
		/// <param name="subKeyName">Name of the sub key.</param>
		/// <param name="close">if set to <see langword="false"/> the return value is read- and writeable; otherwise the result is only readable.</param>
		/// <returns>
		///		The requested <see cref="RegistryKey"/> if <paramref name="subKeyName"/> exists 
		///		and the calling identity has the right to read this key; otherwise <see langword="null"/>.
		/// </returns>
		public static RegistryKey GetRegistryKey( string subKeyName, bool close )
		{
			RegistryKey key = null;
			RegistryKey resultKey;

			try
			{
				key = Registry.LocalMachine.OpenSubKey(subKeyName, !close);
				resultKey = key;
			}
			catch ( ArgumentException )
			{
				resultKey = null;
			}
			catch ( ObjectDisposedException )
			{
				resultKey = null;
			}
			catch ( SecurityException )
			{
				resultKey = null;
			}
			finally
			{
				if ( close && ( key != null ) )
				{
					key.Close();
				}
			}

			return resultKey;
		}


		/// <summary>
		/// Sets a registry value in the local machine registry store.
		/// </summary>
		/// <param name="subKeyName">Name of the sub key.</param>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="keyValue">The key value.</param>
		/// <returns><see langword="true"/> if the value was written; otherwise <see langword="false"/>.</returns>
		public static bool SetRegistryValue( string subKeyName, string keyName, string keyValue )
		{
			RegistryKey key = null;
			try
			{
				key = Registry.LocalMachine.OpenSubKey(subKeyName, true);
				if ( key == null )
				{
					key = Registry.LocalMachine.CreateSubKey(subKeyName);
					if ( key != null )
					{
						key.Close();
					}
					key = Registry.LocalMachine.OpenSubKey(subKeyName, true);
					if ( key == null )
					{
						return false;
					}
				}
				
				key.SetValue(keyName, keyValue, RegistryValueKind.String);
				return true;
			}
			catch ( ArgumentException )
			{
				return false;
			}
			catch ( ObjectDisposedException )
			{
				return false;
			}
			catch ( SecurityException  )
			{
				return false;
			}
			catch ( UnauthorizedAccessException  )
			{
				return false;
			}
			finally
			{
				if ( key != null )
				{
					key.Close();
				}
			}

			//return false;
		}

	}
}
