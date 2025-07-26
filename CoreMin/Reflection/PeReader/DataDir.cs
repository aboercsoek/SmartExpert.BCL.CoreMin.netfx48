//--------------------------------------------------------------------------
// Class:	DataDir
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class DataDir
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
	/// Represents the data directory.
	/// </summary>
	public class DataDir : FileRegion
	{
		/// <summary>
		/// The relative virtual address of the table.
		/// </summary>
		public uint Rva { get; private set; }

		/// <summary>
		/// The size of the table, in bytes.
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// The name of the data directory
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Create a new data directory instance
		/// </summary>
		/// <param name="reader">The binary reader of the file image.</param>
		/// <param name="name">The name of the data directory</param>
		public DataDir(BinaryReader reader, string name)
		{
			Start = reader.BaseStream.Position;
			Length = 8;
			Name = name;

			Rva = reader.ReadUInt32();
			Size = reader.ReadUInt32();
		}

		/// <summary>
		/// Gibt einen <see cref="T:System.String"/> zurück, der das aktuelle <see cref="T:System.Object"/> darstellt.
		/// </summary>
		/// <returns>
		/// Ein <see cref="T:System.String"/>, der das aktuelle <see cref="T:System.Object"/> darstellt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return base.ToString() + " " + Name + " points to {" + Rva.ToString("X8") + " - " + (Rva + Size).ToString("X8") + "}";
		}

	}
}
