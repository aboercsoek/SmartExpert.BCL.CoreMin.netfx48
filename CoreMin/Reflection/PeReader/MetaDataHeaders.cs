//--------------------------------------------------------------------------
// Class:	MetaDataHeaders
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class MetaDataHeaders
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
	/// TODO: Description of class MetaDataHeaders
	/// </summary>
	public class MetaDataHeaders : FileRegion
	{
		#region Constructors (1) 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		public MetaDataHeaders(BinaryReader reader)
		{
			Start = reader.BaseStream.Position;

			StorageSigAndHeader = new StorageSigAndHeader(reader);

			for (var i = 0; i < StorageSigAndHeader.NumOfStreams; ++i)
			{
				var mds = new MdStreamHeader(reader);
				if (mds.Name == "#Strings")
				{
					StringStreamHeader = mds;
				}
				else if (mds.Name == "#Blob")
				{
					BlobStreamHeader = mds;
				}
				else if (mds.Name == "#GUID")
				{
					GuidStreamHeader = mds;
				}
				else if (mds.Name == "#US")
				{
					UsStreamHeader = mds;
				}
				else if (mds.Name == "#~")
				{
					TableStreamHeader = mds;
				}
				else
				{
					TableStreamHeader = mds;
				}
			}

			Length = reader.BaseStream.Position - Start;

		}

		#endregion Constructors 

		#region Read only Properties (6) 

		/// <summary>
		/// 
		/// </summary>
		public MdStreamHeader BlobStreamHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public MdStreamHeader GuidStreamHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public StorageSigAndHeader StorageSigAndHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public MdStreamHeader StringStreamHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public MdStreamHeader TableStreamHeader { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public MdStreamHeader UsStreamHeader { get; private set; }

		#endregion Read only Properties 

	}
}
