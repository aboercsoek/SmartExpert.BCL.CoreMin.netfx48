//--------------------------------------------------------------------------
// Class:	SectionHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class SectionHeader
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
	/// Repesents a section header of a PE file image.
	/// </summary>
	public class SectionHeader : FileRegion
	{

		#region Constructors (1) 

		/// <summary>
		/// Creates a new SectionHeader instance using the specified reader.
		/// </summary>
		/// <param name="reader">The file image binary reader</param>
		public SectionHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;
			Length = 40;

			for (var i = 0; i < 8; ++i)
			{
				byte b = reader.ReadByte();
				if (b != 0)
					Name += (char)b;
			}

			VirtualSize = reader.ReadUInt32();
			VirtualAddress = reader.ReadUInt32();
			SizeOfRawData = reader.ReadUInt32();
			PointerToRawData = reader.ReadUInt32();
			PointerToRelocations = reader.ReadUInt32();
			PointerToLinenumbers = reader.ReadUInt32();
			NumberOfRelocations = reader.ReadUInt16();
			NumberOfLinenumbers = reader.ReadUInt16();
			Characteristics = (ImageSectionType)reader.ReadUInt32();

		}

		#endregion Constructors 

		#region Read only Properties (10) 

		/// <summary>
		/// An 8-byte, null-padded UTF-8 string. There is no terminating null character if the 
		/// string is exactly eight characters long. For longer names, this member contains a 
		/// forward slash (/) followed by an ASCII representation of a decimal number that is an 
		/// offset into the string table. Executable images do not use a string table and do not 
		/// support section names longer than eight characters.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The total size of the section when loaded into memory, in bytes. If this value is greater 
		/// than the SizeOfRawData member, the section is filled with zeroes. This field is valid 
		/// only for executable images and should be set to 0 for object files.
		/// </summary>
		public uint VirtualSize { get; private set; }

		/// <summary>
		/// The address of the first byte of the section when loaded into memory, relative to the image 
		/// base. For object files, this is the address of the first byte before relocation is applied.
		/// </summary>
		public uint VirtualAddress { get; private set; }

		/// <summary>
		/// The size of the initialized data on disk, in bytes. This value must be a multiple of the 
		/// FileAlignment member of the PeHeader structure. If this value is less than the 
		/// VirtualSize member, the remainder of the section is filled with zeroes. If the section 
		/// contains only uninitialized data, the member is zero.
		/// </summary>
		public uint SizeOfRawData { get; private set; }

		/// <summary>
		/// A file pointer to the first page within the COFF file. This value must be a multiple of 
		/// the FileAlignment member of the PeHeader structure. If a section contains 
		/// only uninitialized data, set this member is zero.
		/// </summary>
		public uint PointerToRawData { get; private set; }

		/// <summary>
		/// A file pointer to the beginning of the relocation entries for the section. 
		/// If there are no relocations, this value is zero.
		/// </summary>
		public uint PointerToRelocations { get; private set; }

		/// <summary>
		/// A file pointer to the beginning of the line-number entries for the section. 
		/// If there are no COFF line numbers, this value is zero.
		/// </summary>
		public uint PointerToLinenumbers { get; private set; }

		/// <summary>
		/// The number of relocation entries for the section. 
		/// This value is zero for executable images.
		/// </summary>
		public ushort NumberOfRelocations { get; private set; }

		/// <summary>
		/// The number of line-number entries for the section.
		/// </summary>
		public ushort NumberOfLinenumbers { get; private set; }

		/// <summary>
		/// The characteristics of the image section.
		/// </summary>
		public ImageSectionType Characteristics { get; private set; }

		#endregion Read only Properties 

		#region Overriden Methods (1) 

		/// <summary>
		/// Gibt einen <see cref="T:System.String"/> zurück, der das aktuelle <see cref="T:System.Object"/> darstellt.
		/// </summary>
		/// <returns>
		/// Ein <see cref="T:System.String"/>, der das aktuelle <see cref="T:System.Object"/> darstellt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return base.ToString() + " " + Name + " raw data at offsets {" + PointerToRawData.ToString("X8") + " - " + (PointerToRawData + SizeOfRawData).ToString("X8") + "}";
		}

		#endregion Overriden Methods 

	}
}
