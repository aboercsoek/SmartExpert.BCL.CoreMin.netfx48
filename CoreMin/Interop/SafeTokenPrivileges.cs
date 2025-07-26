//--------------------------------------------------------------------------
// File:    SafeTokenPrivileges.cs
// Content:	Implementation of class SafeTokenPrivileges
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.Interop
{
	/// <summary>
	/// Safe Win32 TokenPrivilege Handle wrapper
	/// </summary>
	internal class SafeTokenPrivileges : SafeHandleZeroOrMinusOneIsInvalid
	{
		#region Ctors

		public SafeTokenPrivileges(uint size)
			: base(true)
		{
			m_Size = size;
			handle = Marshal.AllocHGlobal((int)size);
		}

		public SafeTokenPrivileges(TOKEN_PRIVILEGE tokenPrivileges)
			: base(true)
		{
			handle = MarshalManagedToNative(tokenPrivileges);
		}

		private SafeTokenPrivileges()
			: base(true)
		{
			handle = IntPtr.Zero;
		}

		#endregion

		#region Properties

		private uint m_Size;

		public uint Size
		{
			get { return m_Size; }
		}

		public static SafeTokenPrivileges NullHandle
		{
			get
			{
				return new SafeTokenPrivileges();
			}
		}

		#endregion

		#region Methods

		public TOKEN_PRIVILEGE MarshalNativeToManaged()
		{
			TOKEN_PRIVILEGE result;

			result.PrivilegeCount = (uint)Marshal.ReadInt32(handle);
			result.Privileges = new LUID_AND_ATTRIBUTES[result.PrivilegeCount];
			int offset = sizeof(uint);

			for (int i = 0; i < result.PrivilegeCount; i++)
			{
				result.Privileges[i].Luid.LowPart = (uint)Marshal.ReadInt32(handle, offset);
				offset += sizeof(uint);
				result.Privileges[i].Luid.HighPart = (uint)Marshal.ReadInt32(handle, offset);
				offset += sizeof(uint);
				result.Privileges[i].Attributes = (uint)Marshal.ReadInt32(handle, offset);
				offset += sizeof(uint);
			}

			return result;
		}

		protected override bool ReleaseHandle()
		{
			Marshal.FreeHGlobal(handle);
			return true;
		}

		private IntPtr MarshalManagedToNative(TOKEN_PRIVILEGE tokenPrivileges)
		{
			m_Size = (uint)(sizeof(uint) + tokenPrivileges.PrivilegeCount * Marshal.SizeOf(typeof(LUID_AND_ATTRIBUTES)));
			IntPtr result = Marshal.AllocHGlobal((int)m_Size);

			Marshal.WriteInt32(result, (int)tokenPrivileges.PrivilegeCount);

			int offset = sizeof(uint);

			for (int i = 0; i < tokenPrivileges.PrivilegeCount; i++)
			{
				Marshal.WriteInt32(result, offset, (int)tokenPrivileges.Privileges[i].Luid.LowPart);
				offset += sizeof(uint);
				Marshal.WriteInt32(result, offset, (int)tokenPrivileges.Privileges[i].Luid.HighPart);
				offset += sizeof(uint);
				Marshal.WriteInt32(result, offset, (int)tokenPrivileges.Privileges[i].Attributes);
				offset += sizeof(uint);
			}

			return result;
		}

		#endregion

	}
}
