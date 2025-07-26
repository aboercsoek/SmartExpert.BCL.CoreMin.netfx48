//--------------------------------------------------------------------------
// File:    StreamExtensions.cs
// Content:	Implementation of class StreamExtensions
// Author:	Andreas Börcsök
// Website:	http://smartexpert.boercsoek.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;

#endregion

namespace SmartExpert.IO
{
	///<summary>Stream extension methods.</summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Creates a stream reader over a stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="readAction">The read action.</param>
		public static void StreamReader(this Stream stream, Action<StreamReader> readAction)
		{
			ArgChecker.ShouldNotBeNull(stream, "stream");
			ArgChecker.ShouldNotBeNull(readAction, "readAction");

			new StreamReader(stream).WithDispose(readAction);
		}

		/// <summary>
		/// Creates a stream writer over a stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="writeAction">The write action.</param>
		public static void StreamWriter(this Stream stream, Action<StreamWriter> writeAction)
		{
			ArgChecker.ShouldNotBeNull(stream, "stream");
			ArgChecker.ShouldNotBeNull(writeAction, "writeAction");
			
			new StreamWriter(stream).WithDispose(writeAction);
		}

		/// <summary>
		/// Rewinds the stream to the beginning so that it could be reused for reading. 
		/// For example, this should be done to a MemoryStream after writing and before each reading. 
		/// Fluent.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>The rewinded stream.</returns>
		public static Stream Rewind(this Stream stream)
		{
			ArgChecker.ShouldNotBeNull(stream, "stream");
			ArgChecker.ShouldBeTrue(stream.CanSeek, "stream", "Stream is not seekable. Rewind require that stream feature!");
			
			stream.Seek(0L, SeekOrigin.Begin);
			return stream;
		}




	}
}
