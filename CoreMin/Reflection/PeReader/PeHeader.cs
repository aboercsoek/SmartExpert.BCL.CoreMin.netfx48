//--------------------------------------------------------------------------
// Class:	PeHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class PEHeader
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
#endregion

namespace SmartExpert.Reflection.PeReader
{
	/// <summary>
	/// Represents the optional header format.
	/// </summary>
	public class PeHeader : FileRegion
	{
		#region Constructors (1) 

		/// <summary>
		/// Creates a new PeHeader instance using the binary reader of the file image.
		/// </summary>
		/// <param name="reader">Binary reader that reads the file image.</param>
		/// <exception cref="ModuleException">Is thrown if the count of the data directories is less than 16.</exception>
		public PeHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			// Read Standard fields
			Magic = reader.ReadUInt16();
			
			if (Magic == 0x10b)
				PeFormat = "PE32";
			else if (Magic == 0x20b)
				PeFormat = "PE32+";
			else
				PeFormat = string.Empty;

			MajorLinkerVersion = reader.ReadByte();
			MinorLinkerVersion = reader.ReadByte();
			SizeOfCode = reader.ReadUInt32();
			SizeOfInitializedData = reader.ReadUInt32();
			SizeOfUninitializedData = reader.ReadUInt32();
			AddressOfEntryPoint = reader.ReadUInt32();
			BaseOfCode = reader.ReadUInt32();
			if (PeFormat != "PE32+")
				BaseOfData = reader.ReadUInt32();

			// Read NT-specific fields
			ImageBase = PeFormat == "PE32+" ? reader.ReadUInt64() : reader.ReadUInt32();

			SectionAlignment = reader.ReadUInt32();
			FileAlignment = reader.ReadUInt32();
			MajorOperatingSystemVersion = reader.ReadUInt16();
			MinorOperatingSystemVersion = reader.ReadUInt16();
			MajorImageVersion = reader.ReadUInt16();
			MinorImageVersion = reader.ReadUInt16();
			MajorSubsystemVersion = reader.ReadUInt16();
			MinorSubsystemVersion = reader.ReadUInt16();
			Win32VersionValue = reader.ReadUInt32();
			SizeOfImage = reader.ReadUInt32();
			SizeOfHeaders = reader.ReadUInt32();
			Checksum = reader.ReadUInt32();
			SubSystem = (ImageSubsystem)reader.ReadUInt16();
			DllCharacteristics = reader.ReadUInt16();

			SizeOfStackReserve = PeFormat == "PE32+" ? reader.ReadUInt64() : reader.ReadUInt32();
			SizeOfStackCommit = PeFormat == "PE32+" ? reader.ReadUInt64() : reader.ReadUInt32();
			SizeOfHeapReserve = PeFormat == "PE32+" ? reader.ReadUInt64() : reader.ReadUInt32();
			SizeOfHeapCommit = PeFormat == "PE32+" ? reader.ReadUInt64() : reader.ReadUInt32();
			LoaderFlags = reader.ReadUInt32();
			NumberOfDataDirectories = reader.ReadUInt32();
			if (NumberOfDataDirectories < 16)
				throw new ModuleException("PEHeader:  Invalid number of data directories in file header.");

			DataDirs = new DataDir[NumberOfDataDirectories];

			var peDirNames = new[] { "Export Table", "Import Table", "Resource Table", "Exception Table", "Certificate Table", "Base Relocation Table", "Debug", "Architecture", "Global Ptr", "TLS Table", "Load Config Table", "Bound Import", "IAT", "Delay Import Descriptor", "CLR Runtime Header", "Reserved" };


			for (var i = 0; i < NumberOfDataDirectories; ++i)
			{
				DataDirs[i] = new DataDir(reader, (i < 16) ? peDirNames[i] : "Unknown");
			}

			Length = reader.BaseStream.Position - Start;

		}

		#endregion Constructors 

		#region Read only Properties (31) 

		/// <summary>
		/// PE-Format of the Image: PE32/PE32+
		/// </summary>
		public string PeFormat { get; private set; }

		/// <summary>
		/// The state of the image file. This member can be one of the following values.
		/// 
		/// IMAGE_NT_OPTIONAL_HDR32_MAGIC		The file is an executable image.
		/// 0x10b								(32-bit application => PE32)
		/// 
		/// IMAGE_NT_OPTIONAL_HDR64_MAGIC		The file is an executable image.
		/// 0x20b								(64-bit application => PE32+)
		/// 
		/// IMAGE_ROM_OPTIONAL_HDR_MAGIC		The file is a ROM image.
		/// 0x107
		/// </summary>
		public uint Magic { get; private set; }

		/// <summary>
		/// The major version number of the linker.
		/// </summary>
		public byte MajorLinkerVersion { get; private set; }

		/// <summary>
		/// The minor version number of the linker.
		/// </summary>
		public byte MinorLinkerVersion { get; private set; }

		/// <summary>
		/// The size of the code section, in bytes, or the sum of all 
		/// such sections if there are multiple code sections.
		/// </summary>
		public uint SizeOfCode { get; private set; }

		/// <summary>
		/// The size of the initialized data section, in bytes, or the sum of 
		/// all such sections if there are multiple initialized data sections.
		/// </summary>
		public uint SizeOfInitializedData { get; private set; }

		/// <summary>
		/// The size of the uninitialized data section, in bytes, or the sum of 
		/// all such sections if there are multiple uninitialized data sections.
		/// </summary>
		public uint SizeOfUninitializedData { get; private set; }

		/// <summary>
		/// A pointer to the entry point function, relative to the image base address. 
		/// For executable files, this is the starting address. For device drivers, this 
		/// is the address of the initialization function. The entry point function is 
		/// optional for DLLs. When no entry point is present, this member is zero.
		/// </summary>
		public uint AddressOfEntryPoint { get; private set; }

		/// <summary>
		/// A pointer to the beginning of the code section, relative to the image base.
		/// </summary>
		public uint BaseOfCode { get; private set; }

		/// <summary>
		/// A pointer to the beginning of the data section, relative to the image base.
		/// </summary>
		public uint BaseOfData { get; private set; }

		/// <summary>
		/// The preferred address of the first byte of the image when it is loaded in memory. 
		/// This value is a multiple of 64K bytes. The default value for DLLs is 0x10000000. 
		/// The default value for applications is 0x00400000, except on Windows CE where 
		/// it is 0x00010000.
		/// </summary>
		public ulong ImageBase { get; private set; }

		/// <summary>
		/// The alignment of sections loaded in memory, in bytes. This value must be greater 
		/// than or equal to the FileAlignment member. The default value is the page size 
		/// for the system.
		/// </summary>
		public uint SectionAlignment { get; private set; }

		/// <summary>
		/// The alignment of the raw data of sections in the image file, in bytes. The value 
		/// should be a power of 2 between 512 and 64K (inclusive). The default is 512. 
		/// If the SectionAlignment member is less than the system page size, this member 
		/// must be the same as SectionAlignment.
		/// </summary>
		public uint FileAlignment { get; private set; }

		/// <summary>
		/// The major version number of the required operating system.
		/// </summary>
		public ushort MajorOperatingSystemVersion { get; private set; }

		/// <summary>
		/// The minor version number of the required operating system.
		/// </summary>
		public ushort MinorOperatingSystemVersion { get; private set; }

		/// <summary>
		/// The major version number of the image.
		/// </summary>
		public ushort MajorImageVersion { get; private set; }

		/// <summary>
		/// The minor version number of the image.
		/// </summary>
		public ushort MinorImageVersion { get; private set; }

		/// <summary>
		/// The major version number of the subsystem.
		/// </summary>
		public ushort MajorSubsystemVersion { get; private set; }

		/// <summary>
		/// The minor version number of the subsystem.
		/// </summary>
		public ushort MinorSubsystemVersion { get; private set; }

		/// <summary>
		/// This member is reserved and must be 0.
		/// </summary>
		public uint Win32VersionValue { get; private set; }

		/// <summary>
		/// The size of the image, in bytes, including all headers. 
		/// Must be a multiple of SectionAlignment.
		/// </summary>
		public uint SizeOfImage { get; private set; }

		/// <summary>
		/// The combined size of the following items, rounded to a multiple 
		/// of the value specified in the FileAlignment member.
		///		- e_lfanew member of IMAGE_DOS_HEADER
		///		- 4 byte signature
		///		- size of IMAGE_FILE_HEADER
		///		- size of optional header
		///		- size of all section headers
		/// </summary>
		public uint SizeOfHeaders { get; private set; }

		/// <summary>
		/// The image file checksum. The following files are validated at load time: 
		/// all drivers, any DLL loaded at boot time, and any DLL loaded into 
		/// a critical system process.
		/// </summary>
		public uint Checksum { get; private set; }

		/// <summary>
		/// The subsystem required to run this image. The following values are defined.
		/// </summary>
		public ImageSubsystem SubSystem { get; private set; }

		/// <summary>
		/// The DLL characteristics of the image.
		/// 
		/// 0x0040: The DLL can be relocated at load time.
		/// 
		/// 0x0080: Code integrity checks are forced. If you set this flag 
		///			and a section contains only uninitialized data, set the 
		///			PointerToRawData member of IMAGE_SECTION_HEADER for that 
		///			section to zero; otherwise, the image will fail to load 
		///			because the digital signature cannot be verified.
		/// 
		/// 0x0100: The image is compatible with data execution prevention (DEP).
		/// 
		/// 0x0200: The image is isolation aware, but should not be isolated.
		/// 
		/// 0x0400: The image does not use structured exception handling (SEH). 
		///			No handlers can be called in this image.
		/// 
		/// 0x0800: Do not bind the image.
		/// 
		/// 0x2000: A WDM driver.
		/// 
		/// 0x8000: The image is terminal server aware.
		/// </summary>
		public ushort DllCharacteristics { get; private set; }

		/// <summary>
		/// The number of bytes to reserve for the stack. Only the memory specified by the 
		/// SizeOfStackCommit member is committed at load time; the rest is made available 
		/// one page at a time until this reserve size is reached.
		/// </summary>
		public ulong SizeOfStackReserve { get; private set; }

		/// <summary>
		/// The number of bytes to commit for the stack.
		/// </summary>
		public ulong SizeOfStackCommit { get; private set; }

		/// <summary>
		/// The number of bytes to reserve for the local heap. Only the memory specified by 
		/// the SizeOfHeapCommit member is committed at load time; the rest is made available 
		/// one page at a time until this reserve size is reached.
		/// </summary>
		public ulong SizeOfHeapReserve { get; private set; }

		/// <summary>
		/// The number of bytes to commit for the local heap.
		/// </summary>
		public ulong SizeOfHeapCommit { get; private set; }

		/// <summary>
		/// This member is obsolete.
		/// </summary>
		public uint LoaderFlags { get; private set; }

		/// <summary>
		/// The number of directory entries in the remainder of the optional header. 
		/// Each entry describes a location and size.
		/// </summary>
		public uint NumberOfDataDirectories { get; private set; }

		/// <summary>
		/// A pointer to the first DataDir structure in the data directory.
		/// </summary>
		public DataDir[] DataDirs { get; private set; }

		#endregion Read only Properties 

	}
}
