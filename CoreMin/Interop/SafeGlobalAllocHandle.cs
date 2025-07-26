//--------------------------------------------------------------------------
// File:    SafeGlobalAllocHandle.cs
// Content:	Implementation of class SafeGlobalAllocHandle
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Represents a wrapper class for global alloc handles.</summary>
	public sealed class SafeGlobalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeGlobalAllocHandle()
			: base(false)
		{
			SetHandle(IntPtr.Zero);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeGlobalAllocHandle"/> class.
		/// </summary>
		/// <param name="cb">The byte count to allocate in the unmanaged memory of the process.</param>
		public SafeGlobalAllocHandle(int cb)
			: base(true)
		{
			SetHandle(Marshal.AllocHGlobal(cb));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeGlobalAllocHandle"/> class.
		/// </summary>
		/// <param name="str">The managed string witch content should be copied into the unmanaged memory of the process.</param>
		public SafeGlobalAllocHandle(string str)
			: base(true)
		{
			SetHandle(Marshal.StringToHGlobalUni(str));
		}

		/// <summary>
		/// Copies data from the unmanaged memory into the specified byte array.
		/// </summary>
		/// <param name="destinationBytes">The destination bytes.</param>
		/// <param name="startIndex">The start index.</param>
		/// <param name="cbBytes">The count of bytes to copy.</param>
		public void Copy(byte[] destinationBytes, int startIndex, int cbBytes)
		{
			Marshal.Copy(handle, destinationBytes, startIndex, cbBytes);
		}

		/// <summary>
		/// Marshals the managed type data into unmanaged memory.
		/// </summary>
		/// <param name="structure">The managed type data to copy.</param>
		/// <param name="deleteOld">if set to <see langword="true"/> delete the old memory handle.</param>
		/// <exception cref="InvalidOperationException">If the global alloc handle is invalid.</exception>
		public void MarshalStructure(object structure, bool deleteOld)
		{
			if (IsInvalid)
			{
				throw new InvalidOperationException();
			}
			Marshal.StructureToPtr(structure, handle, deleteOld);
		}

		/// <summary>
		/// Marshals the unmanaged memory where the handle points to into a managed type.
		/// </summary>
		/// <typeparam name="T">The managed type.</typeparam>
		/// <returns>The marshall result.</returns>
		public T MarshalToStructure<T>()
		{
			return (T)Marshal.PtrToStructure(handle, typeof(T));
		}

		/// <summary>
		/// Frees the handle by calling Marshal.FreeHGlobal.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <see langword="false"/>.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			if (!IsInvalid)
			{
				Marshal.FreeHGlobal(handle);
				SetHandleAsInvalid();
			}
			return true;
		}


		/// <summary>
		/// Gets an safe empty memory handle.
		/// </summary>
		public static SafeGlobalAllocHandle Empty
		{
			get
			{
				return new SafeGlobalAllocHandle();
			}
		}
	}


}
