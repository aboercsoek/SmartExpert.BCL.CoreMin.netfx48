//--------------------------------------------------------------------------
// Class:	PeFileInfo
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class PeFileInfo
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Reflection.PeReader
{
	/// <summary>
	/// Portable Executable (PE) file info class
	/// </summary>
	public class PeFileInfo
	{
		private readonly string m_PeFileName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public PeFileInfo(string fileName)
		{
			m_PeFileName = fileName;
			
		}

		/// <summary>
		/// 
		/// </summary>
		public ModuleHeaders PeFileHeaders { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool ReadPeFileHeaders()
		{
			if (PeFileHeaders != null)
				return true;

			bool result = IsPeFile();
			
			if (result.IsFalse())
				return false;
			
			try
			{
				using (var fs = File.OpenRead(m_PeFileName))
				{
					using (var br = new BinaryReader(fs))
					{
						PeFileHeaders = new ModuleHeaders(br);
					}
				}
			}
			catch(Exception ex)
			{
				PeFileHeaders = null;
				result = false;
				if (ex.IsFatal())
					throw;
			}
			
			return result;
		}
		
		/// <summary>
		/// Checks if the file is a Portable Executable (PE) file.
		/// </summary>
		/// <returns><see langword="true"/> if the file is a Portable Executable (PE) file; otherwise <see langword="false"/>.</returns>
		public bool IsPeFile()
		{
			if (m_PeFileName.IsNullOrEmptyWithTrim())
				return false;
			if (File.Exists(m_PeFileName) == false)
				return false;
			if ((Path.GetExtension(m_PeFileName).SafeString().ToLower() != ".exe") && (Path.GetExtension(m_PeFileName).SafeString().ToLower() != ".dll"))
				return false;
			try
			{
				var buffer = new byte[0x1000];
				Stream stream = File.OpenRead(m_PeFileName);
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