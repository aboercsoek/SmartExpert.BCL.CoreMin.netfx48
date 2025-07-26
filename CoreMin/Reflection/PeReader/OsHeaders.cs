//--------------------------------------------------------------------------
// Class:	OsHeaders
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class OsHeaders
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
	/// Represents the standard operating system PE headers (COFF, PE, Sections).
	/// </summary>
	public class OsHeaders : FileRegion
	{
		#region Constructors (1) 

		/// <summary>
		/// Creates a new OsHeaders instance using the specified binary reader.
		/// </summary>
		/// <param name="reader">The binary reader for the file image</param>
		/// <exception cref="ModuleException">Is thrown if file is not a PE executable.</exception>
		public OsHeaders(BinaryReader reader)
		{
			Start = 0;

			DosStub = new MsDosStub(reader);
			reader.BaseStream.Position = DosStub.PePos;

			// Read "PE\0\0" signature
			if (reader.ReadUInt32() != 0x00004550) throw new ModuleException("File is not a portable executable.");

			CoffHeader = new CoffHeader(reader);

			// Compute data sections offset
			var dataSectionsOffset = reader.BaseStream.Position + CoffHeader.OptionalHeaderSize;

			PeHeader = new PeHeader(reader);

			reader.BaseStream.Position = dataSectionsOffset;

			SectionHeaders = new SectionHeader[CoffHeader.NumberOfSections];

			for (var i = 0; i < SectionHeaders.Length; i++)
			{
				SectionHeaders[i] = new SectionHeader(reader);
			}

			Length = reader.BaseStream.Position;

		}

		#endregion Constructors 

		#region Read only Properties (4) 

		/// <summary>
		/// Returns the MsDosStub data.
		/// </summary>
		public MsDosStub DosStub { get; private set; }

		/// <summary>
		/// Gets the COFF header data.
		/// </summary>
		public CoffHeader CoffHeader { get; private set; }

		/// <summary>
		/// Gets the optional PE-header data.
		/// </summary>
		public PeHeader PeHeader { get; private set; }

		/// <summary>
		/// Gets the section headers.
		/// </summary>
		public SectionHeader[] SectionHeaders { get; private set; }

		#endregion Read only Properties 

	}
}
