//--------------------------------------------------------------------------
// File:    SafeCloseHandle.cs
// Content:	Implementation of class SafeCloseHandle
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Safe Win32 close handle implementation.</summary>
	internal sealed class SafeServiceHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeServiceHandle"/> class.
		/// </summary>
		private SafeServiceHandle()
			: base(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeServiceHandle"/> class.
		/// </summary>
		/// <param name="handle">The handle.</param>
		internal SafeServiceHandle(IntPtr handle)
			: base(true)
		{
		   SetHandle(handle);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeServiceHandle"/> class.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <param name="ownsHandle">if set to <see langword="true"/> the handle will reliable released.</param>
		internal SafeServiceHandle( IntPtr handle, bool ownsHandle )
			: base(ownsHandle)
		{
			SetHandle(handle);
		}

		/// <summary>
		/// Frees the handle by calling CloseHandle.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false"/>.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			return AdvApi32.CloseServiceHandle(handle);
		}

		internal static SafeServiceHandle InvalidHandle
		{
			get
			{
				return new SafeServiceHandle(IntPtr.Zero);
			}
		}


	}


}
