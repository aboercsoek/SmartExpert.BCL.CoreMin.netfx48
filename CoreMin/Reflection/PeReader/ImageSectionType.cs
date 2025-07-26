//--------------------------------------------------------------------------
// File:    ImageSectionType.cs
// Content:	Definition of enumeration ImageSectionType
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.Reflection.PeReader
{
	///<summary>Image Section Type</summary>
	[Flags]
	public enum ImageSectionType : uint
	{
		///<summary>No Section Type Flags set.</summary>
		None = 0,
		/// <summary>
		/// The section should not be padded to the next boundary. 
		/// This flag is obsolete and is replaced by ALIGN_1BYTES.
		/// </summary>
		TypeNoPad = 0x00000008,
		/// <summary>
		/// The section contains executable code.
		/// </summary>
		ContainsCode = 0x00000020,
		/// <summary>
		/// The section contains initialized data.
		/// </summary>
		ContainsInitializedData = 0x00000040,
		/// <summary>
		/// The section contains uninitialized data.
		/// </summary>
		ContainsUninitializedData = 0x00000080,
		/// <summary>
		/// Reserved.
		/// </summary>
		LnkOther = 0x00000100,
		/// <summary>
		/// The section contains comments or other information. 
		/// This is valid only for object files.
		/// </summary>
		LnkInfo = 0x00000200,
		/// <summary>
		/// The section will not become part of the image. 
		/// This is valid only for object files.
		/// </summary>
		LnkRemove = 0x00000800,
		/// <summary>
		/// The section contains COMDAT data. 
		/// This is valid only for object files.
		/// </summary>
		LnkComdat = 0x00001000,
		/// <summary>
		/// Reset speculative exceptions handling bits in the 
		/// TLB entries for this section.
		/// </summary>
		NoDeferSpeculativeExceptions = 0x00004000,
		/// <summary>
		/// The section contains data referenced through the global pointer.
		/// </summary>
		Gprel = 0x00008000,
		/// <summary>
		/// Reserved.
		/// </summary>
		MemPurgeable = 0x00020000,
		/// <summary>
		/// Reserved.
		/// </summary>
		MemLocked = 0x00040000,
		/// <summary>
		/// Reserved.
		/// </summary>
		MemPreload = 0x00080000,
		/// <summary>
		/// Align data on a 1-byte boundary. This is valid only for object files.
		/// </summary>
		Align1Bytes = 0x00100000,
		/// <summary>
		/// Align data on a 2-byte boundary. This is valid only for object files.
		/// </summary>
		Align2Bytes = 0x00200000,
		/// <summary>
		/// Align data on a 4-byte boundary. This is valid only for object files.
		/// </summary>
		Align4Bytes = 0x00300000,
		/// <summary>
		/// Align data on a 8-byte boundary. This is valid only for object files.
		/// </summary>
		Align8Bytes = 0x00400000,
		/// <summary>
		/// Align data on a 16-byte boundary. This is valid only for object files.
		/// </summary>
		Align16Bytes = 0x00500000,
		/// <summary>
		/// Align data on a 32-byte boundary. This is valid only for object files.
		/// </summary>
		Align32Bytes = 0x00600000,
		/// <summary>
		/// Align data on a 64-byte boundary. This is valid only for object files.
		/// </summary>
		Align64Bytes = 0x00700000,
		/// <summary>
		/// Align data on a 128-byte boundary. This is valid only for object files.
		/// </summary>
		Align128Bytes = 0x00800000,
		/// <summary>
		/// Align data on a 256-byte boundary. This is valid only for object files.
		/// </summary>
		Align256Bytes = 0x00900000,
		/// <summary>
		/// Align data on a 512-byte boundary. This is valid only for object files.
		/// </summary>
		Align512Bytes = 0x00A00000,
		/// <summary>
		/// Align data on a 1024-byte boundary. This is valid only for object files.
		/// </summary>
		Align1024Bytes = 0x00B00000,
		/// <summary>
		/// Align data on a 2048-byte boundary. This is valid only for object files.
		/// </summary>
		Align2048Bytes = 0x00C00000,
		/// <summary>
		/// Align data on a 4096-byte boundary. This is valid only for object files.
		/// </summary>
		Align4096Bytes = 0x00D00000,
		/// <summary>
		/// Align data on a 8192-byte boundary. This is valid only for object files.
		/// </summary>
		Align8192Bytes = 0x00E00000,
		/// <summary>
		/// The section contains extended relocations. The count of relocations for 
		/// the section exceeds the 16 bits that is reserved for it in the section header. 
		/// If the NumberOfRelocations field in the section header is 0xffff, the actual 
		/// relocation count is stored in the VirtualAddress field of the first relocation. 
		/// It is an error if LnkNrelocOvfl is set and there are fewer 
		/// than 0xffff relocations in the section.
		/// </summary>
		LnkNrelocOvfl = 0x01000000,
		/// <summary>
		/// The section can be discarded as needed.
		/// </summary>
		MemDiscardable = 0x02000000,
		/// <summary>
		/// The section cannot be cached.
		/// </summary>
		MemNotCached = 0x04000000,
		/// <summary>
		/// The section cannot be paged.
		/// </summary>
		MemNotPaged = 0x08000000,
		/// <summary>
		/// The section can be shared in memory.
		/// </summary>
		MemShared = 0x10000000,
		/// <summary>
		/// The section can be executed as code.
		/// </summary>
		MemExecute = 0x20000000,
		/// <summary>
		/// The section can be read.
		/// </summary>
		MemRead = 0x40000000,
		/// <summary>
		/// The section can be written to.
		/// </summary>
		MemWrite = 0x80000000
	}
}
