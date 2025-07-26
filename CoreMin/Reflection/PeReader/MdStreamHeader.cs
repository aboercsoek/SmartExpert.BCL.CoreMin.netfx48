//--------------------------------------------------------------------------
// Class:	MDStreamHeader
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class MDStreamHeader
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
	/// TODO: Description of class MDStreamHeader
	/// </summary>
	public class MdStreamHeader : FileRegion
	{
		#region Constructors (1) 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public MdStreamHeader(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			Offset = reader.ReadUInt32();
			Size = reader.ReadUInt32();

			//How IS the name stored anyway?  It seems subtly different for stream names... it's certainly not what the book says.
			var chars = new char[32];
			int index = 0;
			byte character;
			while ((character = reader.ReadByte()) != 0)
				chars[index++] = (char)character;

			index++;
			int padding = ((index % 4) != 0) ? (4 - (index % 4)) : 0;
			reader.ReadBytes(padding);

			Name = new String(chars).Trim(new Char[] { '\0' });

			if (Name == "#Strings")
			{
				Type = StreamType.StrType;
			}
			else if (Name == "#US" || Name == "#Blob")
			{
				Type = StreamType.BlobType;
			}
			else if (Name == "#GUID")
			{
				Type = StreamType.GuidType;
			}
			else
			{
				Type = StreamType.TableType;
			}

			Length = reader.BaseStream.Position - Start;

		}

		#endregion Constructors 

		#region Read only Properties (4) 

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint Offset { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public StreamType Type { get; private set; }

		#endregion Read only Properties 

		/// <summary>
		/// 
		/// </summary>
		public enum StreamType { 
			/// <summary>String Type</summary>
			StrType,
			/// <summary>Blob Type</summary>
			BlobType, 
			/// <summary>Guid Type</summary>
			GuidType, 
			/// <summary>Table Type</summary>
			TableType 
		};

	}
}
