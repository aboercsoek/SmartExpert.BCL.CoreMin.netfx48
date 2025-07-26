//--------------------------------------------------------------------------
// Class:	StorageSigAndHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class StorageSigAndHeader
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
	/// Storage Signature and Storage Header
	/// </summary>
	public class StorageSigAndHeader : FileRegion
	{

		#region Constructors (1) 

		/// <summary>
		/// Creates a new instance of StorageSigAndHeader.
		/// </summary>
		/// <param name="reader">The file image binary reader to use.</param>
		/// <exception cref="ModuleException">Thrown if </exception>
		public StorageSigAndHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			//storage signature

			uint sig = reader.ReadUInt32();
			if (sig != 0x424A5342) throw new ModuleException("MetaData:  Incorrect signature.");
			MajorVersion = reader.ReadUInt16();
			MinorVersion = reader.ReadUInt16();
			reader.ReadUInt32();//extradata (unused)
			uint versionLength = reader.ReadUInt32();

			for (var i = 0; i < versionLength; ++i)
			{
				VersionString += (char)reader.ReadByte();
			}

			if ((versionLength % 4) != 0) reader.BaseStream.Position += 4 - (versionLength % 4); //padding


			//storage header
			reader.ReadByte();//flags(unused)
			reader.ReadByte();//padding
			NumOfStreams = reader.ReadUInt16();

			Length = reader.BaseStream.Position - Start;

		}

		#endregion Constructors 

		#region Read only Properties (4) 

		/// <summary>
		/// The major version.
		/// </summary>
		public ushort MajorVersion { get; private set; }

		/// <summary>
		/// The minor version.
		/// </summary>
		public ushort MinorVersion { get; private set; }

		/// <summary>
		/// Number of Streams.
		/// </summary>
		public ushort NumOfStreams { get; private set; }

		/// <summary>
		/// CLR runtime version string.
		/// </summary>
		public string VersionString { get; private set; }

		#endregion Read only Properties 

	}
}
