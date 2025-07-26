//--------------------------------------------------------------------------
// Class:	FileRegion
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class FileRegion
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SmartExpert.Reflection.PeReader
{
	/// <summary>
	/// TODO: Description of class FileRegion
	/// </summary>
	public class FileRegion
	{
		/// <summary>
		/// 
		/// </summary>
		public long Start { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long Length { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long End
		{
			get{return Start + Length;}
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
			return GetType().Name + "  {" + Start.ToString("X8") + " - " + (Length + Start).ToString("X8") + "}";
		}

	}
}
