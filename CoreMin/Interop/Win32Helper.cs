//--------------------------------------------------------------------------
// File:    Win32Helper.cs
// Content:	Implementation of class Win32Helper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Interop
{
	///<summary>Win32 helper methods.</summary>
	public class Win32Helper
	{

		#region Static Win32 Error Helper Methods

		/// <summary>
		/// Gets the last win32 error message value.
		/// </summary>
		/// <returns>
		/// Returns the last win32 error message.
		/// </returns>
		public static string GetLastWin32ErrorMessage()
		{
			return GetWin32ErrorMessage(GetLastWin32ErrorValue());
		}

		/// <summary>
		/// Gets the last win32 error value value.
		/// </summary>
		/// <returns>
		/// Returns the last win32 error value value.
		/// </returns>
		public static int GetLastWin32ErrorValue()
		{
			return GetHrForWin32Error(Marshal.GetLastWin32Error());
		}

		/// <summary>
		/// Gets the HResult for win32 error value.
		/// </summary>
		/// <param name="dwLastError">The dw last error.</param>
		/// <returns>
		/// Returns the hr for win32 error value.
		/// </returns>
		public static int GetHrForWin32Error(int dwLastError)
		{
			if ((dwLastError & 0x80000000L) == 0x80000000L)
			{
				return dwLastError;
			}
			return ((dwLastError & 0xffff) | -2147024896);
		}

		/// <summary>
		/// Gets the win32 error message value.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		/// <returns>
		/// Returns the win32 error message value.
		/// </returns>
		public static string GetWin32ErrorMessage(int errorCode)
		{
			StringBuilder msgBuffer = new StringBuilder(512);
			if (Win32.FormatMessage(12800, Win32.NULL, errorCode, 0, msgBuffer, msgBuffer.Capacity, Win32.NULL) != 0)
			{
				return msgBuffer.ToString();
			}
			return "Unknown error '{0}'".SafeFormatWith(errorCode);
		}

		#endregion

	}
}
