//--------------------------------------------------------------------------
// Class:	MetaDataTableHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class MetaDataTableHeader
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
	/// TODO: Description of class MetaDataTableHeader
	/// </summary>
	public class MetaDataTableHeader : FileRegion
	{

		#region Constructors (1) 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public MetaDataTableHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			TableLengths = new uint[64];

			Reserved = reader.ReadUInt32();
			MajorVersion = reader.ReadByte();
			MinorVersion = reader.ReadByte();
			HeapOffsetSizes = reader.ReadByte();
			RidPlaceholder = reader.ReadByte();
			MaskValid = reader.ReadUInt64();
			MaskSorted = reader.ReadUInt64();

			//read as many uints as there are bits set in maskvalid
			for (var i = 0; i < 64; i++)
			{
				var count = (uint)((((MaskValid >> i) & 1) == 0) ? 0 : reader.ReadInt32());
				TableLengths[i] = count;
			}

			Length = reader.BaseStream.Position - Start;
		}

		#endregion Constructors 

		#region Read only Properties (8) 

		/// <summary>
		/// 
		/// </summary>
		public byte HeapOffsetSizes { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public byte MajorVersion { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ulong MaskSorted { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ulong MaskValid { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public byte MinorVersion { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint Reserved { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public byte RidPlaceholder { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint[] TableLengths { get; private set; }

		#endregion Read only Properties 

	}
}
