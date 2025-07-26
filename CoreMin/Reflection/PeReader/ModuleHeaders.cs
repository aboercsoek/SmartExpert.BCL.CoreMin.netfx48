//--------------------------------------------------------------------------
// Class:	ModuleHeaders
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class ModuleHeaders
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Reflection.PeReader
{
	/// <summary>
	/// Encapsulates all PE file headers data.
	/// </summary>
	public class ModuleHeaders
	{

		#region Constructors (1) 

		/// <summary>
		/// Creates a new ModuleHeaders instance by reader all file headers.
		/// </summary>
		/// <param name="reader">The binary reader that reads the file.</param>
		public ModuleHeaders(BinaryReader reader)
		{
			//read coff, pe, section headers
			OsHeaders = new OsHeaders(reader);

			//find and read COR20 header
			try
			{
				reader.BaseStream.Position = Rva2Offset(OsHeaders.PeHeader.DataDirs[14].Rva);
				Cor20Header = new Cor20Header(reader);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal()) throw;
				return;
			}

			//find and read md headers
			try
			{
				reader.BaseStream.Position = Rva2Offset(Cor20Header.MetaData.Rva);
				MetaDataHeaders = new MetaDataHeaders(reader);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal()) throw;
				return;
			}

			try
			{
				reader.BaseStream.Position = MetaDataHeaders.TableStreamHeader.Offset + MetaDataHeaders.StorageSigAndHeader.Start;
				MetaDataTableHeader = new MetaDataTableHeader(reader);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal()) throw;
			}
		}

		#endregion Constructors 

		#region Read only Properties (4) 

		/// <summary>
		/// Standard operating system PE headers (COFF, PE, Sections).
		/// </summary>
		public OsHeaders OsHeaders { get; private set; }

		/// <summary>
		/// CLR Header
		/// </summary>
		public Cor20Header Cor20Header { get; private set; }

		/// <summary>
		/// Gets the Meta data headers.
		/// </summary>
		public MetaDataHeaders MetaDataHeaders { get; private set; }

		/// <summary>
		/// Gets the Meta data table headers
		/// </summary>
		public MetaDataTableHeader MetaDataTableHeader { get; private set; }

		#endregion Read only Properties 

		#region Public Methods (1) 

		/// <summary>
		/// Convert relative virtual address to offset
		/// </summary>
		/// <param name="rva">The relative virtual address to convert</param>
		/// <returns>The file image offset</returns>
		/// <exception cref="ModuleException">Is thrown if convertion of virtual address to offset is not possible.</exception>
		public uint Rva2Offset(uint rva)
		{
			foreach (var sh in OsHeaders.SectionHeaders)
			{
				// sh.SizeOfRawData => Math.Max(sh.VirtualSize, sh.SizeOfRawData)
				if ((sh.VirtualAddress <= rva) && (sh.VirtualAddress + Math.Max(sh.VirtualSize, sh.SizeOfRawData) > rva))
					return (sh.PointerToRawData + (rva - sh.VirtualAddress));
			}

			throw new ModuleException("Module:  Invalid RVA address.");
		}

		#endregion Public Methods 

	}
}
