//--------------------------------------------------------------------------
// File:    AssemblyHelper.cs
// Content:	Implementation of class AssemblyHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Reflection
{
	///<summary>Assembly utility class.</summary>
	public static class AssemblyHelper
	{
		/// <summary>
		/// Checks if a file is a managed application or library.
		/// </summary>
		/// <param name="fileName">The file path.</param>
		/// <returns><see langword="true"/> if the file is a managed application or library; otherwise <see langword="false"/>.</returns>
		public static bool IsManaged( string fileName )
		{
			if (fileName.IsNullOrEmptyWithTrim())
				return false;
			if (File.Exists(fileName) == false)
				return false;
			if ((Path.GetExtension(fileName).SafeString().ToLower() != ".exe") && (Path.GetExtension(fileName).SafeString().ToLower() != ".dll"))
				return false;
			try
			{
				var buffer = new byte[0x1000];
				Stream stream = File.OpenRead(fileName);
				stream.Read(buffer, 0, 0x1000);
				stream.Close();

				// Has File a valid MsDosHeader
				if ( ( ( buffer[1] << 8 ) | buffer[0] ) != 0x5a4d )
					return false;

				// File offset 0x3C contains the file pointer to PE Signature
				int posPeSig = ( ( ( buffer[0x3f] << 24 ) | ( buffer[0x3e] << 16 ) ) | ( buffer[0x3d] << 8 ) ) | buffer[0x3c];

				// Check if File has a valid PE Signature
				if ((((( buffer[posPeSig + 3] << 24 ) | ( buffer[posPeSig + 2] << 16 )) | ( buffer[posPeSig + 1] << 8 )) | buffer[posPeSig] ) != 0x4550 )
					return false;

				int magic = ((buffer[posPeSig + 25] << 8) | buffer[posPeSig + 24]);
				
				int clrDataDirOffset = 208;
				if (magic == 0x020B)
					clrDataDirOffset = 224;

				int clrHeaderFieldsStartPos = ( posPeSig + 24 ) + clrDataDirOffset;
				int checkEndPos = clrHeaderFieldsStartPos + 8;
				
				int numCheck = 0;
				for ( int i = clrHeaderFieldsStartPos; i < checkEndPos; i++ )
				{
					numCheck |= buffer[i];
				}

				if ( numCheck != 0 )
					return true;
			}
			catch (IOException)
			{}
			catch ( UnauthorizedAccessException )
			{}
			catch ( NotSupportedException)
			{}

			return false;
		}

	}
}
