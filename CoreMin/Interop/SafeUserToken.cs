//--------------------------------------------------------------------------
// File:    SafeUserToken.cs
// Content:	Implementation of class SafeUserToken
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.Interop
{
	/// <summary>
	/// Represents a wrapper class for user tokens.
	/// </summary>
	public class SafeUserToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeUserToken"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		protected SafeUserToken()
			: base(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeCloseHandle"/> class.
		/// </summary>
		/// <param name="handle">The handle.</param>
		public SafeUserToken(IntPtr handle)
			: base(true)
		{
		   SetHandle(handle);
		}

		/// <summary>
		/// Frees the handle by calling CloseHandle.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false"/>.
		/// </returns>
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return Win32.CloseHandle(handle);
		}

		/// <summary>
		/// Returns a <see cref="SafeUserToken"/> instance that was initialized with <see cref="F:System.IntPtr.Zero"/>.
		/// </summary>
		/// <value>The invalid token.</value>
		public static SafeUserToken ZeroToken
		{
			get
			{
				return new SafeUserToken(IntPtr.Zero);
			}
		}

		/// <summary>
		/// Gets the windows identity of the user token.
		/// </summary>
		/// <returns>
		/// Returns the windows identity.
		/// </returns>
		public WindowsIdentity GetWindowsIdentity()
		{
			return (IsInvalid == false) ? new WindowsIdentity(handle) : null;
		}

		internal IntPtr Handle
		{
			get
			{
				return handle;
			}
		}

	}

	/// <summary>
	/// <see cref="SafeUserToken"/> extension class.
	/// </summary>
	public static class SafeUserTokenExtensions
	{
		/// <summary>
		/// Determines whether the user token is valid.
		/// </summary>
		/// <param name="userToken">The user token.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified user token is valid; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsValidToken(this SafeUserToken userToken)
		{
			if (userToken.IsNull())
				return false;
			
			return !userToken.IsInvalid;
		}
	}
}
