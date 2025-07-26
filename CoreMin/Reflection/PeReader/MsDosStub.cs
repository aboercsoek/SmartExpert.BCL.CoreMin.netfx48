//--------------------------------------------------------------------------
// Class:	MsDosStub
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class MsDosStub
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
	/// Repesents the MsDosStub of the file image.
	/// </summary>
	public class MsDosStub : FileRegion
	{

		#region Fields (1) 

		readonly uint m_PePos;

		#endregion Fields 

		#region Constructors (1) 

		/// <summary>
		/// Creates a new MsDosStub instance.
		/// </summary>
		/// <param name="reader">Binary reader of the file image.</param>
		/// <exception cref="ModuleException">Thrown if valid DOS header (0x5A4D) was not found.</exception>
		public MsDosStub(BinaryReader reader)
		{
			if (reader.ReadUInt16() != 0x5A4D)
				throw new ModuleException("MsDosHeader: Invalid DOS header.");

			// File offset 0x3C contains the file pointer to PE Signature
			reader.BaseStream.Position = 0x3c;
			m_PePos = reader.ReadUInt32();

			Start = 0;
			Length = 64;
		}

		#endregion Constructors 

		#region Read only Properties (2) 

		/// <summary>
		/// Gets the file pointer offset to PE Signature
		/// </summary>
		public uint PeHeaderOffset
		{
			get { return m_PePos; }
		}

		/// <summary>
		/// Gets the file pointer to PE Signature
		/// </summary>
		public uint PePos
		{
			get { return m_PePos; }
		}

		#endregion Read only Properties 

	}
}
