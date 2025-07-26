//--------------------------------------------------------------------------
// File:    AutoReleaseReadWriterLock.cs
// Content:	Implementation of class AutoReleaseReadWriterLock
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Threading
{
	///<summary><see cref="ReaderWriterLockSlim"/> wrapper that provides auto release for reader und writer locks.</summary>
	public class AutoReleaseReaderWriterLock
	{
// ReSharper disable FieldCanBeMadeReadOnly.Local
		private AutoReaderRelease m_AutoReaderRelease;
		private AutoWriterRelease m_AutoWriterRelease;
		private AutoUpgradeableReaderRelease m_AutoUpgradeableReaderRelease;
		private ReaderWriterLockSlim m_ReadWriterLock = new ReaderWriterLockSlim();
// ReSharper restore FieldCanBeMadeReadOnly.Local

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoReleaseReaderWriterLock"/> class.
		/// </summary>
		public AutoReleaseReaderWriterLock()
		{
			m_AutoWriterRelease = new AutoWriterRelease(m_ReadWriterLock);
			m_AutoReaderRelease = new AutoReaderRelease(m_ReadWriterLock);
			m_AutoUpgradeableReaderRelease = new AutoUpgradeableReaderRelease(m_ReadWriterLock);
		}

		/// <summary>
		/// Create a AutoReleaseReaderWriterLock instance.
		/// </summary>
		/// <returns>The created AutoReleaseReaderWriterLock instance</returns>
		public static AutoReleaseReaderWriterLock Create()
		{
			return new AutoReleaseReaderWriterLock();
		}

		/// <summary>
		/// Gets the auto releaseable upgradeable read lock.
		/// </summary>
		public IDisposable UpgradeableReadLock
		{
			get
			{
				m_ReadWriterLock.EnterUpgradeableReadLock();
				return m_AutoUpgradeableReaderRelease;
			}
		}

		/// <summary>
		/// Gets the auto releaseable read lock.
		/// </summary>
		public IDisposable ReadLock
		{
			get
			{
				m_ReadWriterLock.EnterReadLock();
				return m_AutoReaderRelease;
			}
		}

		/// <summary>
		/// Gets the auto releaseable writer lock.
		/// </summary>
		public IDisposable WriteLock
		{
			get
			{
				m_ReadWriterLock.EnterWriteLock();
				return m_AutoWriterRelease;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct AutoUpgradeableReaderRelease : IDisposable
		{
// ReSharper disable FieldCanBeMadeReadOnly.Local
			private ReaderWriterLockSlim m_Lock;
// ReSharper restore FieldCanBeMadeReadOnly.Local
			public AutoUpgradeableReaderRelease(ReaderWriterLockSlim rwLock)
			{
				m_Lock = rwLock;
			}

			public void Dispose()
			{
				m_Lock.ExitUpgradeableReadLock();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct AutoReaderRelease : IDisposable
		{
// ReSharper disable FieldCanBeMadeReadOnly.Local
			private ReaderWriterLockSlim m_Lock;
// ReSharper restore FieldCanBeMadeReadOnly.Local
			public AutoReaderRelease(ReaderWriterLockSlim rwLock)
			{
				m_Lock = rwLock;
			}

			public void Dispose()
			{
				m_Lock.ExitReadLock();
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct AutoWriterRelease : IDisposable
		{
// ReSharper disable FieldCanBeMadeReadOnly.Local
			private ReaderWriterLockSlim m_Lock;
// ReSharper restore FieldCanBeMadeReadOnly.Local
			public AutoWriterRelease(ReaderWriterLockSlim rwLock)
			{
				m_Lock = rwLock;
			}

			public void Dispose()
			{
				m_Lock.ExitWriteLock();
			}
		}

	}
}
