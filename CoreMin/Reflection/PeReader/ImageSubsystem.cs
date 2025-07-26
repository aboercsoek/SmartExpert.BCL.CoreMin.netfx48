//--------------------------------------------------------------------------
// File:    ImageSubsystem.cs
// Content:	Definition of enumeration ImageSubsystem
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Reflection.PeReader
{
	///<summary>The image subsystem type</summary>
	public enum ImageSubsystem : ushort
	{
		///<summary>Unknown subsystem.</summary>
		Unknown = 0,
		/// <summary>
		/// No subsystem required (device drivers and native system processes).
		/// </summary>
		Native = 1,
		/// <summary>
		/// Windows graphical user interface (GUI) subsystem.
		/// </summary>
		WindowsGui = 2,
		/// <summary>
		/// Windows character-mode user interface (CUI) subsystem.
		/// </summary>
		WindowsCui = 3,
		/// <summary>
		/// OS/2 CUI subsystem.
		/// </summary>
		Os2Cui = 5,
		/// <summary>
		/// POSIX CUI subsystem.
		/// </summary>
		PosixCui = 7,
		/// <summary>
		/// Windows CE system.
		/// </summary>
		WindowsCeGui = 9,
		/// <summary>
		/// Extensible Firmware Interface (EFI) application.
		/// </summary>
		EfiApplication = 10,
		/// <summary>
		/// EFI driver with boot services.
		/// </summary>
		EfiBootServiceDriver = 11,
		/// <summary>
		/// EFI driver with run-time services.
		/// </summary>
		EfiRuntimeDriver = 12,
		/// <summary>
		/// EFI ROM image.
		/// </summary>
		EfiRom = 13,
		/// <summary>
		/// XBOX system.
		/// </summary>
		XBOX = 14,
		/// <summary>
		/// Boot application.
		/// </summary>
		WindowsBootApplication = 16 
	}
}
