//--------------------------------------------------------------------------
// File:    StreamReaderExtensions.cs
// Content:	Implementation of class ReaderExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	///<summary>Reader (StreamReader, ...) extension methods.</summary>
	public static class StreamReaderExtensions
	{
		/// <summary>
		/// Enumerates over all lines of stream.
		/// </summary>
		/// <param name="streamReader">The stream reader witch holds the stream.</param>
		/// <returns>Enumerable string collection (lines).</returns>
		public static IEnumerable<string> EnumLines( this StreamReader streamReader )
		{
			return ReaderHelper.EnumLines(streamReader);
		}


		/// <summary>
		/// Enumerates over all lines of string.
		/// </summary>
		/// <param name="stringReader">The string reader witch holds the string.</param>
		/// <returns>Enumerable string collection (lines).</returns>
		public static IEnumerable<string> EnumLines( this StringReader stringReader )
		{
			return ReaderHelper.EnumLines(stringReader);
		}

	}
}
