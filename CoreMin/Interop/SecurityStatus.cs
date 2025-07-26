//--------------------------------------------------------------------------
// File:    SecurityStatus.cs
// Content:	Definition of enumeration SecurityStatus
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Interop
{
	/// <summary>
	/// Security status enumeration
	/// </summary>
	internal enum SecurityStatus : uint
	{
		BufferNotEnough = 0x80090321,
		CannotInstall = 0x80090307,
		CompAndContinue = 0x90314,
		CompleteNeeded = 0x90313,
		ContextExpired = 0x90317,
		ContinueNeeded = 0x90312,
		CredentialsNeeded = 0x90320,
		IncompleteCred = 0x80090320,
		IncompleteMessage = 0x80090318,
		InternalError = 0x80090304,
		InvalidHandle = 0x80090301,
		InvalidToken = 0x80090308,
		LogonDenied = 0x8009030c,
		MessageAltered = 0x8009030f,
		NoCredentials = 0x8009030e,
		NotOwner = 0x80090306,
		// ReSharper disable InconsistentNaming
		OK = 0x0,
		// ReSharper restore InconsistentNaming
		OutOfMemory = 0x80090300,
		PackageNotFound = 0x80090305,
		Renegotiate = 0x90321,
		TargetUnknown = 0x80090303,
		UnknownCertificate = 0x80090327,
		UnknownCredential = 0x8009030d,
		Unsupported = 0x80090302,
		UntrustedRoot = 0x80090325,
		WrongPrincipal = 0x80090322
	}
}
