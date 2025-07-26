//--------------------------------------------------------------------------
// File:    ReaderHelper.cs
// Content:	Implementation of class ReaderHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
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

	/// <summary>
	/// Reader (StreamReader, ...) helper class
	/// </summary>
	public static class ReaderHelper
	{
		/// <summary>
		/// Enumerates over all lines of stream.
		/// </summary>
		/// <param name="streamReader">The stream reader witch holds the stream.</param>
		/// <returns>Enumerable string collection (lines).</returns>
		public static IEnumerable<string> EnumLines( StreamReader streamReader )
		{
			if (streamReader == null)
				yield break;

			while ( !streamReader.EndOfStream )
			{
				var line = streamReader.ReadLine();
				yield return line;
			}
		}

		/// <summary>
		/// Enumerates over all lines of string.
		/// </summary>
		/// <param name="stringReader">The string reader witch holds the string.</param>
		/// <returns>Enumerable string collection (lines).</returns>
		public static IEnumerable<string> EnumLines( StringReader stringReader )
		{
			if (stringReader == null)
				yield break;

			while ( true )
			{
				string line = stringReader.ReadLine();
	
				if ( line == null )
					yield break;

				yield return line;
			}
		}
	}
}
