//--------------------------------------------------------------------------
// File:    StreamHelper.cs
// Content:	Implementation of class StreamHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	///<summary>Provides <see cref="Stream"/> helper methods.</summary>
	public static class StreamHelper
	{
		/// <summary>
		/// Copies the stream.
		/// </summary>
		/// <param name="src">The source stream.</param>
		/// <param name="dest">The destination stream.</param>
		/// <exception cref="ArgNullException">
		///		Is thrown if <paramref name="src"/> or <paramref name="dest"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgException{TValue}">
		///		Is thrown if <paramref name="src"/> stream is not readable or <paramref name="dest"/> stream is not writeable
		/// </exception>
		public static void CopyStream( Stream src, Stream dest )
		{
			ArgChecker.ShouldNotBeNull(src, "src");
			ArgChecker.ShouldNotBeNull(dest, "dest");

			if ( src.CanRead == false )
				throw new ArgException<Stream>(src, "src", "Source stream is not readable.");
			if ( dest.CanWrite == false )
				throw new ArgException<Stream>(dest, "dest", "Destination stream is not writeable.");

			var buffer = new byte[4096];
			while ( true )
			{
				int count = src.Read(buffer, 0, 4096);
				
				if (count == 0) return;

				dest.Write(buffer, 0, count);
			}
		}

		/// <summary>
		/// Converts a string object into a stream object.
		/// </summary>
		/// <param name="text">The string.</param>
		/// <returns>Stream with string value as content.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="text"/>  is <see langword="null"/>.</exception>
		public static Stream StringAsStream( string text )
		{
			ArgChecker.ShouldNotBeNull(text, "text");

			var stream = new MemoryStream();
			
			stream.StreamWriter(sw => { sw.Write(text); sw.Flush(); });
			stream.Rewind();

			return stream;
		}

		/// <summary>
		/// Stream to Byte-Array.
		/// </summary>
		/// <param name="inStream">The input stream.</param>
		/// <returns>The Byte-Array result.</returns>
		public static byte[] StreamToBytes(Stream inStream)
		{
			ArgChecker.ShouldNotBeNull(inStream, "inStream");

			using (var outStream = new MemoryStream())
			{
				Transfer(inStream, outStream);

				return outStream.ToArray();
			}
		}

		/// <summary>
		/// Stream to Byte-Array.
		/// </summary>
		/// <param name="inStream">The input stream.</param>
		/// <param name="count">The byte count that should be copied.</param>
		/// <returns>The Byte-Array result.</returns>
		public static byte[] StreamToBytes(Stream inStream, int count)
		{
			ArgChecker.ShouldNotBeNull(inStream, "inStream");

			if (count <= 0)
				return EmptyArray<byte>.Instance;

			var buffer = new byte[count];
			int num = inStream.Read(buffer, 0, count);

			if (num == buffer.Length)
				return buffer;

			if (num <= 0)
				return EmptyArray<byte>.Instance;

			var destinationArray = new byte[num];
			Array.Copy(buffer, destinationArray, destinationArray.Length);

			return destinationArray;
		}

		/// <summary>
		/// Byte-Array to Stream Converter.
		/// </summary>
		/// <param name="data">The byte data.</param>
		/// <returns>The result stream.</returns>
		public static Stream BytesToStream(byte[] data)
		{
			ArgChecker.ShouldNotBeNull(data, "data");

			return new MemoryStream(data);
		}

		/// <summary>
		/// Copies the input stream into an memory stream.
		/// </summary>
		/// <param name="sourceStream">The input stream to copy.</param>
		/// <returns>The memory stream that contains the input stream data.</returns>
		public static Stream CopyToMemoryStream(Stream sourceStream)
		{
			ArgChecker.ShouldNotBeNull(sourceStream, "sourceStream");
			
			if (sourceStream.CanSeek)
				return new MemoryStream(StreamToBytes(sourceStream, (int) sourceStream.Length));

			var outStream = new MemoryStream();
			Transfer(sourceStream, outStream);
			outStream.Rewind();
			
			return outStream;
		}

		/// <summary>
		/// Discards the rest of the stream by moving the current stream position to the end of the stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public static void DiscardRest(Stream stream)
		{
			ArgChecker.ShouldNotBeNull(stream, "stream");

			if (stream.CanSeek)
			{
				stream.Seek(0L, SeekOrigin.End);
			}
			else
			{
				var buffer = new byte[4096];
				while (stream.Read(buffer, 0, buffer.Length) != 0) {}
			}
		}

		/// <summary>
		/// Extracts the byte region from a stream. Region starts after startBytes where found in the stream and ends before endBytes was found in the stream.
		/// </summary>
		/// <param name="startBytes">The start bytes.</param>
		/// <param name="endBytes">The end bytes.</param>
		/// <param name="sourceStream">The stream.</param>
		/// <returns>The extracted byte region</returns>
		public static byte[] ExtractByteRegion(byte[] startBytes, byte[] endBytes, Stream sourceStream)
		{
			ArgChecker.ShouldNotBeNull(startBytes, "startBytes");
			ArgChecker.ShouldNotBeNull(endBytes, "endBytes");
			ArgChecker.ShouldNotBeNull(sourceStream, "sourceStream");

			int num = sourceStream.ReadByte();
			long startBytesPos = -1L;
			while ((num != -1) && (startBytesPos == -1L))
			{
				if (num == startBytes[0])
				{
					long position = sourceStream.Position;
					bool startByteFound = true;
					for (int i = 1; startByteFound && (i < startBytes.Length); i++)
					{
						startByteFound = sourceStream.ReadByte() == startBytes[i];
					}
					if (startByteFound)
					{
						num = sourceStream.ReadByte();
						startBytesPos = position;
					}
					else
					{
						sourceStream.Seek(position, SeekOrigin.Begin);
						num = sourceStream.ReadByte();
					}
				}
				else
				{
					num = sourceStream.ReadByte();
				}
			}
			if (startBytesPos == -1L)
			{
				return EmptyArray<byte>.Instance;
			}

			var stream = new MemoryStream();
			stream.Write(startBytes, 0, startBytes.Length);
			while (num != -1)
			{
				stream.WriteByte((byte)num);
				if (num == endBytes[0])
				{
					bool endByteFound = true;
					for (int j = 1; endByteFound && (j < endBytes.Length); j++)
					{
						num = sourceStream.ReadByte();
						stream.WriteByte((byte)num);
						endByteFound = num == endBytes[j];
					}
					if (endByteFound)
					{
						break;
					}
				}
				num = sourceStream.ReadByte();
			}
			return stream.ToArray();
		}

		/// <summary>
		/// Stream to Stream transfer.
		/// </summary>
		/// <param name="inStream">The in stream.</param>
		/// <param name="outStream">The out stream.</param>
		/// <returns>The transfered byte count.</returns>
		public static long Transfer(Stream inStream, Stream outStream)
		{
			return Transfer(inStream, outStream, 8192, true);
		}

		/// <summary>
		/// Stream to Stream transfer.
		/// </summary>
		/// <param name="inStream">The in stream.</param>
		/// <param name="outStream">The out stream.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		/// <param name="flush">if set to <see langword="true"/> [flush].</param>
		/// <returns>The transfered byte count.</returns>
		public static long Transfer(Stream inStream, Stream outStream, int bufferSize, bool flush)
		{
			ArgChecker.ShouldNotBeNull(inStream, "inStream");
			ArgChecker.ShouldNotBeNull(outStream, "outStream");
			
			if (bufferSize <= 0)
				bufferSize = 8192;

			int bytesReadCount;
			long totalTransferCount = 0L;
			var buffer = new byte[bufferSize];

			while ((bytesReadCount = inStream.Read(buffer, 0, bufferSize)) != 0)
			{
				if (bytesReadCount < 0)
				{
					break;
				}
				outStream.Write(buffer, 0, bytesReadCount);
				totalTransferCount += bytesReadCount;
			}
			if (flush)
			{
				outStream.Flush();
			}
			return totalTransferCount;
		}

		/// <summary>
		/// Incremental Stream to Stream transfer.
		/// </summary>
		/// <param name="inStream">The in stream.</param>
		/// <param name="outStream">The out stream.</param>
		/// <param name="buffer">The transfer buffer.</param>
		/// <param name="byteCount">The transfer byte count.</param>
		/// <returns><see langword="true"/> if the transfer was successfull; otherwise <see langword="false"/>.</returns>
		public static bool TransferIncremental(Stream inStream, Stream outStream, ref byte[] buffer, out long byteCount)
		{
			ArgChecker.ShouldNotBeNull(inStream, "inStream");
			ArgChecker.ShouldNotBeNull(outStream, "outStream");

			if (buffer == null)
				buffer = new byte[8192];

			int count = inStream.Read(buffer, 0, buffer.Length);
			if (count != 0)
			{
				outStream.Write(buffer, 0, count);
				byteCount = count;
				return true;
			}
			byteCount = 0L;
			return false;
		}


	}
}
