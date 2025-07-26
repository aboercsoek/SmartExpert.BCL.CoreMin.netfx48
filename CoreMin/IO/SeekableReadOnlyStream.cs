//--------------------------------------------------------------------------
// Class:	SeekableReadOnlyStream
// Copyright © 2008 Andreas Börcsök. All rights reserved
// Content:	Seekable readonly stream class
// Author:	Andreas Börcsök
// Website:	http://www.csharp-world.net
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Implements a seekable read-only stream which uses buffering if
	/// underlying stream is not seekable. Buffer in memory has size
	/// threshold and overflows to disk (temporary file) if number of bytes.
	/// </summary>
	public class SeekableReadOnlyStream : Stream
	{
		/// <summary>
		/// Initializes a SeekableReadOnlyStream instance with base stream and 
		/// buffering stream.
		/// </summary>
		/// <param name="baseStream">Base stream</param>
		/// <param name="bufferingStream">Buffering stream</param>
		public SeekableReadOnlyStream(Stream baseStream, Stream bufferingStream)
		{
			if (null == baseStream)
				throw new ArgumentNullException("baseStream");
			if (null == bufferingStream)
				throw new ArgumentNullException("bufferingStream");
			
			// Sanity check - make sure that buffering stream is seekable
			if (!bufferingStream.CanSeek)
				throw new NotSupportedException("Buffering stream must be seekable");

			m_BaseStream = baseStream;
			m_BufferingStream = bufferingStream;
		}

		/// <summary>
		/// Initializes a SeekableReadOnlyStream instance with base stream and inherently uses
		/// VirtualStream instance as buffering stream.
		/// </summary>
		/// <param name="baseStream">Base stream</param>
		public SeekableReadOnlyStream(Stream baseStream) : this(baseStream, new VirtualStream())
		{
			// Empty
		}

		/// <summary>
		/// Initializes a SeekableReadOnlyStream instance with base stream and buffer size, and 
		/// inherently uses VirtualStream instance as buffering stream.
		/// </summary>
		/// <param name="baseStream">Base stream</param>
		/// <param name="bufferSize">Buffer size</param>
		public SeekableReadOnlyStream(Stream baseStream, int bufferSize) : this(baseStream, new VirtualStream(bufferSize))
		{
			// Empty
		}

		/// <summary>
		/// Gets a flag indicating whether this stream can be read.
		/// </summary>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a flag indicating whether this stream can be written to.
		/// </summary>
		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// Gets a flag indicating whether this stream is seekbale. Returns always true.
		/// </summary>
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		/// Gets or sets a stream position.
		/// </summary>
		public override long Position
		{
			get 
			{ 
				// Check if base stream is seekable
				if (m_BaseStream.CanSeek)
					return m_BaseStream.Position;

				return m_BufferingStream.Position; 
			}
			set
			{
				// Check if base stream is seekable
				if (m_BaseStream.CanSeek)
				{
					m_BaseStream.Position = value;
					return;
				}

				// Check if current position is the same as being set
				if (m_BufferingStream.Position == value)
					return;

				// Check if stream position is being set to the value which is in already
				// read to the buffering stream space, i.e. less than current buffering stream
				// position or less than length of the buffering stream
				if (value < m_BufferingStream.Position || value < m_BufferingStream.Length)
				{
					// Just change position in the buffering stream
					m_BufferingStream.Position = value;
				}
				else
				{
					//
					// Need to read buffer from the base stream from the current position in 
					// base stream to the position being set and write that buffer to the end
					// of the buffering stream
					//

					// Set position to the last byte in the buffering stream
					m_BufferingStream.Seek(0, SeekOrigin.End);

					// Read buffer from the base stream and write it to the buffering stream
					// in 4K chunks
					byte [] buffer = new byte[ 4096 ];
					long bytesToRead = value - m_BufferingStream.Position;
					while (bytesToRead > 0)
					{
						// Read to buffer 4K or byteToRead, whichever is less
						int bytesRead = m_BaseStream.Read(buffer, 0, (int) Math.Min(bytesToRead, buffer.Length));

						// Check if any bytes were read
						if (0 == bytesRead)
							break;
						
						// Write read bytes to the buffering stream
						m_BufferingStream.Write(buffer, 0, bytesRead);

						// Decrease bytes to read counter
						bytesToRead -= bytesRead;
					}

					//
					// Since this stream is not writable, any attempt to point Position beyond the length
					// of the base stream will not succeed, and buffering stream position will be set to the
					// last byte in the buffering stream.
					//
				}
			}
		}

		/// <summary>
		/// Seeks in stream. For this stream can be very expensive because entire base stream 
		/// can be dumped into buffering stream if SeekOrigin.End is used.
		/// </summary>
		/// <param name="offset">A byte offset relative to the origin parameter</param>
		/// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position</param>
		/// <returns>The new position within the current stream</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			// Check if base stream is seekable
			if (m_BaseStream.CanSeek)
				return m_BaseStream.Seek(offset, origin);

			if (SeekOrigin.Begin == origin)
			{
				// Just set the absolute position using Position property
				Position = offset;
				return Position;
			}

			if (SeekOrigin.Current == origin)
			{
				// Set the position using current Position property value plus offset
				Position = Position + offset;
				return Position;
			}
			
			if (SeekOrigin.End == origin)
			{
				//
				// Need to read all remaining not read bytes from the base stream to the 
				// buffering stream. We can't use offset here because stream size may not
				// be available because it's not seekable. Then we'll set the position 
				// based on buffering stream size.
				//

				// Set position to the last byte in the buffering stream
				m_BufferingStream.Seek(0, SeekOrigin.End);

				// Read all remaining bytes from the base stream to the buffering stream
				byte [] buffer = new byte[ 4096 ];
				for (;;)
				{
					// Read buffer from base stream
					int bytesRead = m_BaseStream.Read(buffer, 0, buffer.Length);
					
					// Break the reading loop if the base stream is exhausted
					if (0 == bytesRead)
						break;

					// Write buffer to the buffering stream
					m_BufferingStream.Write(buffer, 0, bytesRead);
				}

				// Now buffering stream size is equal to the base stream size. Set position
				// using begin origin
				Position = m_BufferingStream.Length - offset;
				return Position;
			}

			throw new NotSupportedException("Not supported SeekOrigin");
		}

		/// <summary>
		/// Gets the length in bytes of the stream. For this stream can be very expensive
		/// because entire base stream will be dumped into buffering stream.
		/// </summary>
		public override long Length
		{
			get
			{
				// Check if base stream is seekable
				if (m_BaseStream.CanSeek)
					return m_BaseStream.Length;

				// Preserve the current stream position
				long position = Position;

				// Seek to the end of stream
				Seek(0, SeekOrigin.End);

				// Length will be equal to the current position
				long length = Position;

				// Restore the current stream position
				Position = position;

				return length;
			}
		}

		/// <summary>
		/// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count- 1) replaced by the bytes read from the current source</param>
		/// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			// Check if base stream is seekable
			if (m_BaseStream.CanSeek)
				return m_BaseStream.Read(buffer, offset, count);

			int bytesReadTotal = 0;

			// Check if buffering stream has some bytes to read, starting from the
			// current position
			if (m_BufferingStream.Length > m_BufferingStream.Position)
			{
				// Read available bytes in buffering stream or count bytes to the buffer, whichever is less
				bytesReadTotal = m_BufferingStream.Read(buffer, offset, (int) Math.Min(m_BufferingStream.Length - m_BufferingStream.Position, count));
				
				// Account for bytes read from the buffering stream
				count -= bytesReadTotal;
				offset += bytesReadTotal;
			}

			// Check if we have any more bytes to read
			if (count > 0)
			{
				Debug.Assert(m_BufferingStream.Position == m_BufferingStream.Length);

				//
				// At this point, buffering stream has position set to its end. We need to read buffer from
				// the base stream and write it to the buffering stream
				//

				// Read count bytes from the base stream starting from offset
				int bytesRead = m_BaseStream.Read(buffer, offset, count);

				// Check if bytes were really read
				if (bytesRead > 0)
				{
					// Write number of read bytes to the buffering stream starting from offset in buffer
					m_BufferingStream.Write(buffer, offset, bytesRead);
				}

				// Add number of bytes read at this step to the number of totally read bytes
				bytesReadTotal += bytesRead;
			}

			return bytesReadTotal;
		}

		/// <summary>
		/// Writes to stream.
		/// </summary>
		/// <param name="buffer">Buffer to write to stream</param>
		/// <param name="offset">Stream offset to start write from</param>
		/// <param name="count">Number of bytes from buffer to write</param>
		/// <exception cref="NotSupportedException">Is thrown always</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Set stream length.
		/// </summary>
		/// <param name="value">Stream length</param>
		/// <exception cref="NotSupportedException">Is thrown always</exception>
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Closes base and buffering streams.
		/// </summary>
		public override void Close()
		{
			// Close underlying streams
			m_BaseStream.Close();
			m_BufferingStream.Close();
		}

		/// <summary>
		/// Flushes the stream.
		/// </summary>
		public override void Flush()
		{
			// Flush the buffering stream
			m_BufferingStream.Flush();
		}


		private Stream m_BaseStream;
		private Stream m_BufferingStream;
	}
}
