//--------------------------------------------------------------------------
// File:    SpinWaitLock.cs
// Content:	Implementation of class SpinWaitLock
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.Interop;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Threading
{
	///<summary>
	/// SpinWaitLock implementation with take Single/Multi CPU configuration into account. 
	/// </summary>
	public sealed class SpinWaitLock : IDisposable
	{
		#region Private Members

		private const Int32 FreeStateValue = 0;
		private const Int32 OwnedStateValue = 1;

		private static readonly Boolean IsSingleCpuMachine = (Environment.ProcessorCount == 1);

		private Int32 m_LockState; // Defaults to 0=FreeStateValue

		#endregion

		#region Nested Types

		private class LockWrapper : IDisposable
		{
			private SpinWaitLock m_Lock;
			public LockWrapper(SpinWaitLock spinWaitLock)
			{
				m_Lock = spinWaitLock;
				m_Lock.Enter();
			}

			void IDisposable.Dispose()
			{
				m_Lock.Exit();
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Enter the Lock and return a disposable object that releases the Lock on Dispose.
		/// </summary>
		/// <returns>A disposable object the releases the Lock on Dispose.</returns>
		public IDisposable Lock()
		{
			return new LockWrapper(this);
		}

		/// <summary>
		/// Enters the Lock.
		/// </summary>
		public void Enter()
		{
			Thread.BeginCriticalRegion();
			while (true)
			{
				// If resource available, set it to in-use and return
				if (Interlocked.Exchange(ref m_LockState, OwnedStateValue) == FreeStateValue)
				{
					break;
				}

				// Efficiently spin, until the resource looks like it might 
				// be free. 
				// NOTE: Just reading here (as compared to repeatedly 
				// NOTE: calling Exchange) improves performance because writing 
				// NOTE: forces all CPUs to update this value
				while (Thread.VolatileRead(ref m_LockState) == OwnedStateValue)
				{
					StallThread();
				}
			}
		}

		/// <summary>
		/// Tries to enter the Lock.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns><see langword="true"/> if Enter was successfull; otherwise <see langword="false"/>.</returns>
		public bool TryEnter(TimeSpan timeout)
		{
			DateTime end = DateTime.Now + timeout;

			Thread.BeginCriticalRegion();
			while (DateTime.Now < end)
			{
				// If resource available, set it to in-use and return
				if (Interlocked.Exchange(ref m_LockState, OwnedStateValue) == FreeStateValue)
				{
					return true;
				}

				// Efficiently spin, until the resource looks like it might be free. 
				// NOTE: Just reading here (as compared to repeatedly calling Exchange) improves 
				// NOTE: performance because writing forces all CPUs to update this value
				while (Thread.VolatileRead(ref m_LockState) == OwnedStateValue)
				{
					StallThread();
				}
			}
			Thread.EndCriticalRegion();
			return false;

		}

		/// <summary>
		/// Exits the Lock.
		/// </summary>
		public void Exit()
		{
			// Mark the resource as available
			Interlocked.Exchange(ref m_LockState, FreeStateValue);
			Thread.EndCriticalRegion();
		}

		#endregion

		#region Private Methods

		private static void StallThread()
		{
			if (IsSingleCpuMachine)
			{
				// On a single-CPU system, spinning does no good
				Win32.SwitchToThread();
			}
			else
			{
				// Multi-CPU system might be hyper-threaded, let other thread run
				Thread.SpinWait(1);
			}
		}

		void IDisposable.Dispose()
		{
			Exit();
		}

		#endregion
	}
}
