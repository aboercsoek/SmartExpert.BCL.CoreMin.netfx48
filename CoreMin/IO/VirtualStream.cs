//--------------------------------------------------------------------------
// Class:	VirtualStream
// Copyright © 2008 Andreas Börcsök. All rights reserved
// Content:	Virtual stream class
// Author:	Andreas Börcsök
// Website:	http://www.csharp-world.net
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Implements a virtual stream, i.e. the always seekable stream which
	/// uses configurable amount of memory to reduce a memory footprint and 
	/// temporarily stores remaining data in a temporary file on disk.
	/// </summary>
	public sealed class VirtualStream : Stream, IDisposable
	{

		#region Constructors

		/// <summary>
		/// Initializes a VirtualStream instance with default parameters (1MB memory buffer, allow overflow to disk).
		/// </summary>
		public VirtualStream()
			: this(DEFAULTMEMORYSIZE, MemoryFlag.AutoOverFlowToDisk, new MemoryStream())
		{
		}

		/// <summary>
		/// Initializes a VirtualStream instance with memory buffer size.
		/// </summary>
		/// <param name="bufferSize">Memory buffer size</param>
		public VirtualStream(int bufferSize)
			: this(bufferSize, MemoryFlag.AutoOverFlowToDisk, new MemoryStream(bufferSize))
		{
		}

		/// <summary>
		/// Initializes a VirtualStream instance with a default memory size and memory flag specified.
		/// </summary>
		/// <param name="flag"><see cref="MemoryFlag"/> that controls the memory management of <see cref="VirtualStream"/>.</param>
		public VirtualStream(MemoryFlag flag)
			: this(DEFAULTMEMORYSIZE, flag,
			(flag == MemoryFlag.OnlyToDisk) ? CreatePersistentStream() : new MemoryStream())
		{
		}

		/// <summary>
		/// Initializes a VirtualStream instance with a memory buffer size and memory flag specified.
		/// </summary>
		/// <param name="bufferSize">Memory buffer size</param>
		/// <param name="flag">Memory flag</param>
		public VirtualStream(int bufferSize, MemoryFlag flag)
			: this(bufferSize, flag,
			(flag == MemoryFlag.OnlyToDisk) ? CreatePersistentStream() : new MemoryStream(bufferSize))
		{
		}

		/// <summary>
		/// Initializes a VirtualStream instance with a memory buffer size, memory flag and underlying stream
		/// specified.
		/// </summary>
		/// <param name="bufferSize">Memory buffer size</param>
		/// <param name="flag">Memory flag</param>
		/// <param name="dataStream">Underlying stream</param>
		private VirtualStream(int bufferSize, MemoryFlag flag, Stream dataStream)
		{
			if (null == dataStream)
				throw new ArgumentNullException("dataStream");

			m_IsInMemory = (flag != MemoryFlag.OnlyToDisk);
			m_MemoryStatus = flag;
			bufferSize = Math.Min(bufferSize, MEMORYTHRESHOLD);
			m_ThresholdSize = bufferSize;

			m_WrappedStream = m_IsInMemory ? dataStream : new BufferedStream(dataStream, bufferSize);
			m_IsDisposed = false;
		}

		#endregion

		#region Stream Methods and Properties

		/// <summary>
		/// Gets a flag indicating whether a stream can be read.
		/// </summary>
		override public bool CanRead
		{
			get { return m_WrappedStream.CanRead; }
		}
		/// <summary>
		/// Gets a flag indicating whether a stream can be written.
		/// </summary>
		override public bool CanWrite
		{
			get { return m_WrappedStream.CanWrite; }
		}
		/// <summary>
		/// Gets a flag indicating whether a stream can seek.
		/// </summary>
		override public bool CanSeek
		{
			get { return true; }
		}
		/// <summary>
		/// Returns the length of the source stream.
		/// </summary>
		override public long Length
		{
			get { return m_WrappedStream.Length; }
		}

		/// <summary>
		/// Gets or sets a position in the stream.
		/// </summary>
		override public long Position
		{
			get { return m_WrappedStream.Position; }
			set { m_WrappedStream.Seek(value, SeekOrigin.Begin); }
		}

		/// <summary>
		/// <see cref="Stream.Close()"/>
		/// </summary>
		/// <remarks>
		/// Calling other methods after calling Close() may result in a ObjectDisposedException beeing throwed.
		/// </remarks>
		override public void Close()
		{
			if (!m_IsDisposed)
			{
				GC.SuppressFinalize(this);
				Cleanup();
			}
		}

		/// <summary>
		/// <see cref="Stream.Flush()"/>
		/// </summary>
		/// <remarks>
		/// Flushes the underlying stream.
		/// </remarks>
		override public void Flush()
		{
			ThrowIfDisposed();
			m_WrappedStream.Flush();
		}

		/// <summary>
		/// <see cref="System.IO.Stream.Read"/>
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns>
		/// The number of bytes read
		/// </returns>
		/// <remarks>
		/// May throw <see cref="ObjectDisposedException"/>.
		/// It will read from cached persistence stream
		/// </remarks>
		override public int Read(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			return m_WrappedStream.Read(buffer, offset, count);
		}

		/// <summary>
		/// <see cref="System.IO.Stream.Seek"/>
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns>
		/// The current position
		/// </returns>
		/// <remarks>
		/// May throw <see cref="ObjectDisposedException"/>.
		/// It will cache any new data into the persistence stream
		/// </remarks>
		override public long Seek(long offset, SeekOrigin origin)
		{
			ThrowIfDisposed();
			return m_WrappedStream.Seek(offset, origin);
		}

		/// <summary>
		/// <see cref="System.IO.Stream.SetLength"/>
		/// </summary>
		/// <param name="length"></param>
		/// <remarks>
		/// May throw <see cref="ObjectDisposedException"/>.
		/// </remarks>
		override public void SetLength(long length)
		{
			ThrowIfDisposed();

			// Check if new position is greater than allowed by threshold
			if (m_MemoryStatus == MemoryFlag.AutoOverFlowToDisk &&
				m_IsInMemory &&
				length > m_ThresholdSize)
			{
				// Currently in memory, and the new write will push it over the limit
				// Switching to Persist Stream
				BufferedStream persistStream = new BufferedStream(CreatePersistentStream(), m_ThresholdSize);

				// Copy current wrapped memory stream to the persist stream
				CopyStreamContent((MemoryStream)m_WrappedStream, persistStream);

				// Close old wrapped stream
				if (m_WrappedStream != null)
					m_WrappedStream.Close();

				m_WrappedStream = persistStream;
				m_IsInMemory = false;
			}

			// Set new length for the wrapped stream
			m_WrappedStream.SetLength(length);
		}

		/// <summary>
		/// <see cref="System.IO.Stream.Write"/>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// </summary>
		/// <remarks>
		/// Write to the underlying stream.
		/// </remarks>
		override public void Write(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();

			// Check if new position after write is greater than allowed by threshold
			if (m_MemoryStatus == MemoryFlag.AutoOverFlowToDisk &&
				m_IsInMemory &&
				(count + m_WrappedStream.Position) > m_ThresholdSize)
			{
				// Currently in memory, and the new write will push it over the limit
				// Switching to Persist Stream
				BufferedStream persistStream = new BufferedStream(CreatePersistentStream(), m_ThresholdSize);

				// Copy current wrapped memory stream to the persist stream
				CopyStreamContent((MemoryStream)m_WrappedStream, persistStream);

				// Close old wrapped stream
				if (m_WrappedStream != null)
					m_WrappedStream.Close();

				m_WrappedStream = persistStream;
				m_IsInMemory = false;
			}

			m_WrappedStream.Write(buffer, offset, count);
		}

		#endregion

		#region IDisposable Interface

		/// <summary>
		/// <see cref="IDisposable.Dispose"/>
		/// </summary>
		/// <remarks>
		/// It will call <see cref="Close()"/>
		/// </remarks>
		public new void Dispose()
		{
			base.Dispose();
			Close();
		}

		#endregion

		#region Private Utility Functions

		/// <summary>
		/// Utility method called by the Finalize(), Close() or Dispose() to close and release
		/// both the source and the persistence stream.
		/// </summary>
		private void Cleanup()
		{
			if (!m_IsDisposed)
			{
				m_IsDisposed = true;
				if (null != m_WrappedStream)
				{
					m_WrappedStream.Close();
					m_WrappedStream = null;
				}
			}
		}

		/// <summary>
		/// Copies source memory stream to the target stream.
		/// </summary>
		/// <param name="source">Source memory stream</param>
		/// <param name="target">Target stream</param>
		private void CopyStreamContent(MemoryStream source, Stream target)
		{
			// Remember position for the source stream
			long currentPosition = source.Position;

			// Read and write in chunks each thresholdSize
			byte[] tempBuffer = new Byte[m_ThresholdSize];
			int read;

			source.Position = 0;
			while ((read = source.Read(tempBuffer, 0, tempBuffer.Length)) != 0)
				target.Write(tempBuffer, 0, read);

			// Set target's stream position to be the same as was in source stream. This is required because 
			// target stream is going substitute source stream.
			target.Position = currentPosition;

			// Restore source stream's position (just in case to preserve the source stream's state)
			source.Position = currentPosition;
		}

		/// <summary>
		/// Called by other methods to check the stream state.
		/// It will thorw <see cref="ObjectDisposedException"/> if the stream was closed or disposed.
		/// </summary>
		private void ThrowIfDisposed()
		{
			if (m_IsDisposed || null == m_WrappedStream)
				throw new ObjectDisposedException("VirtualStream");
		}

		/// <summary>
		/// Utility method.
		/// Creates a FileStream with a unique name and the temporary and delete-on-close attributes.
		/// </summary>
		/// <returns>
		/// The temporary persistence stream
		/// </returns>
		public static Stream CreatePersistentStream()
		{
			StringBuilder name = new StringBuilder(256);

			if (0 == GetTempFileName(Path.GetTempPath(), "DekaFitIrisImport", 0, name))
				throw new IOException("GetTempFileName Failed.", Marshal.GetHRForLastWin32Error());

			IntPtr handle = CreateFile(name.ToString(), (UInt32)FileAccess.ReadWrite, 0, IntPtr.Zero, (UInt32)FileMode.Create, 0x04000100, IntPtr.Zero);

			// FileStream constructor will throw exception if handle is zero or -1.
			return new FileStream(new SafeFileHandle(handle, true), FileAccess.ReadWrite);
		}

		[DllImport("kernel32.dll")]
		private static extern UInt32 GetTempFileName
			(
			string path,
			string prefix,
			UInt32 unique,
			StringBuilder name
			);

		[DllImport("kernel32.dll")]
		private static extern IntPtr CreateFile
			(
			string name,
			UInt32 accessMode,
			UInt32 shareMode,
			IntPtr security,
			UInt32 createMode,
			UInt32 flags,
			IntPtr template
			);

		#endregion

		#region Public Nested Types

		/// <summary>
		/// Memory handling.
		/// </summary>
		public enum MemoryFlag
		{
			///<summary>VirtualStream holds data stream up to a threshold size into memory, otherwise uses BufferedStream with usage of temp files</summary>
			AutoOverFlowToDisk = 0,
			///<summary>VirtualStream holds data stream always in memory</summary>
			OnlyInMemory = 1,
			///<summary>VirtualStream holds data stream always in a BufferedStream with usage of temp files</summary>
			OnlyToDisk = 2
		}

		#endregion

		#region Private Constants

		private const int MEMORYTHRESHOLD = 40 * 1024 * 1024;		// The maximum possible memory consumption (40Mb)
		private const int DEFAULTMEMORYSIZE = 1 * 1024 * 1024;			// Default memory consumption (1MB)

		#endregion

		#region Private Members

		private Stream m_WrappedStream;
		private bool m_IsDisposed;
		private bool m_IsInMemory;
		private int m_ThresholdSize;
		private MemoryFlag m_MemoryStatus;

		#endregion
	}
}
