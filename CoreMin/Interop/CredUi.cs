//--------------------------------------------------------------------------
// File:    CredUi.cs
// Content: credui.dll Native Method Imports
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using SmartExpert.Security.Authentication;

#endregion

namespace SmartExpert.Interop
{
	///<summary>Provides credui.dll native methods, structures and definitions</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static partial class CredUi
	{
		// ReSharper disable InconsistentNaming

		#region Constant declarations

		#region Common constant declarations

		private const string CREDUI = "credui.dll";

		internal static readonly IntPtr NULL = IntPtr.Zero;
		internal const int FALSE = 0;
		internal const int TRUE = 1;

		#endregion

		#endregion

		#region DllImports

		#region Credential UI Imports

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[DllImport(CREDUI, EntryPoint = "CredUIConfirmCredentialsW", CharSet = CharSet.Unicode)]
		public static extern QueryCredentialError CredUIConfirmCredentials(
			[In] string targetName,
			[In, MarshalAs(UnmanagedType.Bool)] bool confirm);


		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[DllImport(CREDUI, EntryPoint = "CredUIPromptForCredentialsW", CharSet = CharSet.Unicode)]
		public static extern unsafe QueryCredentialError CredUIPromptForCredentials(ref CredUiInfo creditUR,
				  [In] string targetName,
				  [In] IntPtr reserved1,
				  [In] int iError,
				  [In, Out] StringBuilder userName,
				  [In] int maxUserName,
				  [In, Out] char* password,
				  [In] int maxPassword,
				  [In, Out, MarshalAs(UnmanagedType.Bool)] ref bool iSave,
				  QueryCredentialOptions flags);


		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[DllImport(CREDUI, EntryPoint = "CredUICmdLinePromptForCredentialsW", CharSet = CharSet.Unicode)]
		public static extern unsafe QueryCredentialError CredUICmdLinePromptForCredentials(
			[In] string targetName,
			[In] IntPtr reserved,
			[In] int authError,
			[In, Out] StringBuilder userName,
			[In] int usernNameBufferSize,
			[In, Out] char* password,
			[In] int passwordBufferSize,
			[In, Out, MarshalAs(UnmanagedType.Bool)] ref bool saveCredential,
			[In] QueryCredentialOptions flags);

		#endregion

		#endregion

		// ReSharper restore InconsistentNaming
	}
}
