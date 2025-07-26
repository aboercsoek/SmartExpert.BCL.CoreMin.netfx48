//--------------------------------------------------------------------------
// Class:	Cor20Header
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class Cor20Header
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
	/// TODO: Description of class Cor20Header
	/// </summary>
	public class Cor20Header : FileRegion
	{
		#region Constructors (1) 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public Cor20Header(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			CB = reader.ReadUInt32();
			MajorRuntimeVersion = reader.ReadUInt16();
			MinorRuntimeVersion = reader.ReadUInt16();
			MetaData = new DataDir(reader, "MetaData");
			Flags = reader.ReadUInt32();
			EntryPointToken = reader.ReadUInt32();
			Resources = new DataDir(reader, "Resources");
			StrongNameSignature = new DataDir(reader, "StrongNameSignature");
			CodeManagerTable = new DataDir(reader, "CodeManagerTable");
			VTableFixups = new DataDir(reader, "VTableFixups");
			ExportAddressTableJumps = new DataDir(reader, "ExportAddressTableJumps");
			ManagedNativeHeader = new DataDir(reader, "ManagedNativeHeader");

			Length = reader.BaseStream.Position - Start;

		}

		#endregion Constructors 

		#region Read only Properties (12) 

		/// <summary>
		/// 
		/// </summary>
		public uint CB { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort MajorRuntimeVersion { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public ushort MinorRuntimeVersion { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir MetaData { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint Flags { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint EntryPointToken { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir Resources { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir StrongNameSignature { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir CodeManagerTable { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir VTableFixups { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir ExportAddressTableJumps { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DataDir ManagedNativeHeader { get; private set; }

		#endregion Read only Properties 

	}
}
