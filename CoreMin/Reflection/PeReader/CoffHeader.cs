//--------------------------------------------------------------------------
// Class:	CoffHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class CoffHeader
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
	/// Represents the COFF header format.
	/// </summary>
	public class CoffHeader : FileRegion
	{
		readonly ImageFileMachine[] m_ImageFileMachines = new [] 
		{
			new ImageFileMachine {Constant = "UNKNOWN", Value = 0, Description = "Any machine type" },
			new ImageFileMachine {Constant = "I386", Value = 0x014c, Description = "Intel 386 or later. " },
			new ImageFileMachine {Constant = "R3000", Value = 0x0162, Description = "MIPS little endian" },
			new ImageFileMachine {Constant = "R4000",  Value = 0x0166, Description = "MIPS little endian." },
			new ImageFileMachine {Constant = "R10000", Value = 0x0168, Description = "MIPS little endian." },
			new ImageFileMachine {Constant = "WCEMIPSV2", Value = 0x0169, Description = "MIPS little endian running Microsoft Windows CE 2." },
			new ImageFileMachine {Constant = "ALPHA", Value = 0x0184, Description = "Alpha AXP." },
			new ImageFileMachine {Constant = "SH3", Value = 0x01a2, Description = "Hitachi SH3 little endian." },
			new ImageFileMachine {Constant = "SH3DSP", Value = 0x01a3, Description = "Hitachi SH3DSP little endian." },
			new ImageFileMachine {Constant = "SH3E", Value = 0x01a4, Description = "Hitachi SH3E little endian." },
			new ImageFileMachine {Constant = "SH4", Value = 0x01a6, Description = "Hitachi SH4." },
			new ImageFileMachine {Constant = "SH5", Value = 0x01a8, Description = "Hitachi SH5." },
			new ImageFileMachine {Constant = "ARM", Value = 0x01c0, Description = "ARM little endian." },
			new ImageFileMachine {Constant = "THUMB", Value = 0x01c2, Description = "ARM processor with Thumb decompressor." },
			new ImageFileMachine {Constant = "ARMNT", Value = 0x01c4, Description = "ARMv7 (or higher) Thumb mode only." },
			new ImageFileMachine {Constant = "AM33", Value = 0x01d3, Description = "Matsushita AM33." },
			new ImageFileMachine {Constant = "POWERPC", Value = 0x01F0, Description = "IBM PowerPC little endian." },
			new ImageFileMachine {Constant = "POWERPCFP", Value = 0x01F1, Description = "IBM PowerPC little endian with floating-point unit (FPU)" },
			new ImageFileMachine {Constant = "IA64", Value = 0x0200, Description = "Intel IA64 (Itanium)." },
			new ImageFileMachine {Constant = "MIPS16", Value = 0x0266, Description = "MIPS." },
			new ImageFileMachine {Constant = "ALPHA64", Value = 0x0284, Description = "ALPHA AXP64." },
			new ImageFileMachine {Constant = "AXP64", Value = 0x0284, Description = "ALPHA AXP64." },
			new ImageFileMachine {Constant = "MIPSFPU", Value = 0x0366, Description = "MIPS with FPU." },
			new ImageFileMachine {Constant = "MIPSFPU16", Value = 0x0466, Description = "MIPS16 with FPU." },
			new ImageFileMachine {Constant = "TRICORE", Value = 0x0520, Description = "Infineon." },
			new ImageFileMachine {Constant = "AMD64", Value = 0x8664, Description = "AMD X64 and Intel E64T architecture." },
			new ImageFileMachine {Constant = "M32R", Value = 0x9041, Description = "Mitsubishi M32R little endian." },
			new ImageFileMachine {Constant = "ARM64", Value = 0xaa64, Description = "ARMv8 in 64-bit mode." },
			new ImageFileMachine {Constant = "EBC", Value = 0x0ebc, Description = "EFI byte code." }
		};

		#region Constructors (1)

		/// <summary>
		/// Creates a new CoffHeader instance using the specified binary reader.
		/// </summary>
		/// <param name="reader">Binary reader that reads the file image.</param>
		public CoffHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;
			Length = 20;

			MachineRaw = reader.ReadUInt16();

			Machine = string.Empty;
			foreach (var item in m_ImageFileMachines)
			{
				if (item.Value == MachineRaw)
					Machine = item.Constant;
			}

			NumberOfSections = reader.ReadUInt16();
			TimeDateStamp = reader.ReadUInt32();
			SymbolTablePointer = reader.ReadUInt32();
			NumberOfSymbols = reader.ReadUInt32();
			OptionalHeaderSize = reader.ReadUInt16();
			CharacteristicsRaw = reader.ReadUInt16();
			Characteristics = (CharacteristicsType)CharacteristicsRaw;
		}

		#endregion Constructors

		#region Properties (7)

		/// <summary>
		/// The characteristics of the image (raw data).
		/// </summary>
		public ushort CharacteristicsRaw { get; private set; }

		/// <summary>
		/// The characteristics of the image.
		/// </summary>
		public CharacteristicsType Characteristics { get; private set; }

		/// <summary>
		/// The architecture type of the computer. An image file can only be run on the 
		/// specified computer or a system that emulates the specified computer (raw data).
		/// </summary>
		public ushort MachineRaw { get; private set; }

		/// <summary>
		/// The architecture type of the computer. An image file can only be run on the 
		/// specified computer or a system that emulates the specified computer (human readable form).
		/// </summary>
		public string Machine { get; private set; }

		/// <summary>
		/// The number of sections. This indicates the size of the section table, 
		/// which immediately follows the headers. Note that the Windows loader 
		/// limits the number of sections to 96.
		/// </summary>
		public ushort NumberOfSections { get; private set; }

		/// <summary>
		/// The number of symbols in the symbol table.
		/// </summary>
		public uint NumberOfSymbols { get; private set; }

		/// <summary>
		/// The size of the optional header, in bytes. This value should be 0 for object files.
		/// </summary>
		public ushort OptionalHeaderSize { get; private set; }

		/// <summary>
		/// The offset of the symbol table, in bytes, or zero if no COFF symbol table exists.
		/// </summary>
		public uint SymbolTablePointer { get; private set; }

		/// <summary>
		/// The low 32 bits of the time stamp of the image. This represents the 
		/// date and time the image was created by the linker. The value is 
		/// represented in the number of seconds elapsed since midnight (00:00:00), 
		/// January 1, 1970, Universal Coordinated Time, according to the system clock.
		/// </summary>
		public uint TimeDateStamp { get; private set; }

		#endregion Properties

	}
}
