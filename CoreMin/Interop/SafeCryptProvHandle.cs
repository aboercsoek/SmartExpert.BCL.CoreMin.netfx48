//--------------------------------------------------------------------------
// File:    SafeCryptProvHandle.cs
// Content:	Implementation of class SafeCryptProvHandle
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;

using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Represents a wrapper class for crypt provider handles.</summary>
	public sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Fields
		private const uint CryptMachineKeyset = 0x20;
		private const uint CryptNewkeyset = 8;
		private const int NteExists = -2146893809;

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeCryptProvHandle"/> class.
		/// </summary>
		/// <param name="handle">The handle.</param>
		public SafeCryptProvHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		/// <summary>
		/// Acquires the machine crypt context.
		/// </summary>
		/// <param name="keyContainerName">Name of the key container.</param>
		/// <param name="providerName">Name of the provider.</param>
		/// <param name="providerType">Type of the provider.</param>
		/// <returns>A safe crypt provider handle.</returns>
		public static SafeCryptProvHandle AcquireMachineContext(string keyContainerName, string providerName, uint providerType)
		{
			SafeCryptProvHandle handle;
			uint dwFlags = CryptMachineKeyset;
			bool flag = NativeMethods.CryptAcquireContextW(out handle, keyContainerName, providerName, providerType, dwFlags | CryptNewkeyset);
			int hr = Marshal.GetLastWin32Error();
			if (!flag && !NativeMethods.CryptAcquireContextW(out handle, keyContainerName, providerName, providerType, dwFlags))
			{
				hr = Marshal.GetLastWin32Error();
				if (hr != NteExists)
				{
					throw new CryptographicException(hr);
				}
			}
			return handle;
		}

		/// <summary>
		/// Frees the handle by calling CryptReleaseContext.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false"/>.
		/// </returns>
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CryptReleaseContext(base.handle, 0);
		}

		// Nested Types
		private static class NativeMethods
		{
			// Methods
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern bool CryptAcquireContextW(out SafeCryptProvHandle hCryptProv, [In, MarshalAs(UnmanagedType.LPWStr)] string pszContainer, [In, MarshalAs(UnmanagedType.LPWStr)] string pszProvider, [In] uint dwProvType, [In] uint dwFlags);
			[DllImport("advapi32.dll")]
			internal static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags);
		}
	}


}
