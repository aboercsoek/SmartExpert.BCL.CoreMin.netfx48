//--------------------------------------------------------------------------
// File:    ActiveTimeTrigger.cs
// Content:	Implementation of class ActiveTimeTrigger
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Threading;
using SmartExpert.Messaging;

#endregion

namespace SmartExpert.Threading
{
	///<summary>TODO: Description of class ActiveTimeTrigger</summary>
	public class ActiveTimeTrigger : IDisposable
	{
		#region Fields (8)

		// Thread that fires time triggered an event. 
		Thread m_TimeTriggerThread;
		// Is set to true during TimeTrigger stop and leads to the end of the time trigger thread.
		bool m_EndThread;

		bool m_Stopped;
		// Ensures that the TimeTrigger is only start once.
		bool m_Start;
		// Sync objects for lock statements
		object m_StartStopSync = new object();
		object m_CycleTimeSync = new object();
		// Thread cycle time in milliseconds
		int m_CycleTime;
		// Time trigger action delegate. 1.Parameter = current timestamp; 2.Parameter = actual cycle time
		Action<DateTime, int> m_TimeTriggerAction;

        #endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveTimeTrigger"/> class.
		/// </summary>
		/// <param name="cycleTime">The cycle time of the time trigger in milliseconds.</param>
		public ActiveTimeTrigger(int cycleTime)
			: this(cycleTime, ThreadPriority.Normal, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveTimeTrigger"/> class.
		/// </summary>
		/// <param name="cycleTime">The cycle time of the active time trigger in milliseconds.</param>
		/// <param name="threadPriority">The thread priority of the active time trigger.</param>
		public ActiveTimeTrigger(int cycleTime, ThreadPriority threadPriority)
			: this(cycleTime, threadPriority, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveTimeTrigger"/> class.
		/// </summary>
		/// <param name="cycleTime">The cycle time of the active time trigger in milliseconds.</param>
		/// <param name="threadPriority">The thread priority of the active time trigger.</param>
		/// <param name="activeTimeTriggerName">Name of the active time trigger thread.</param>
		public ActiveTimeTrigger(int cycleTime, ThreadPriority threadPriority, string activeTimeTriggerName)
		{
			EndThread = false;
			m_Stopped = true;
			CycleTime = cycleTime;
			m_TimeTriggerThread = new Thread(TimeTriggerThreadMethod);
			m_TimeTriggerThread.Priority = threadPriority;
			m_TimeTriggerThread.Name = (string.IsNullOrEmpty(activeTimeTriggerName)) ? "ActiveTimeTrigger" : activeTimeTriggerName;
			m_Start = false;
		}

		#endregion

		#region Thread Method

		/// <summary>
		/// Threadmethod that executes an action on a cycle time base.
		/// </summary>
		/// <remarks>Calls timeTriggerAction every (...) milliseconds. (...) is the value of the CycleTime property.</remarks>
		private void TimeTriggerThreadMethod()
		{
			int actualCycleTime = CycleTime;
			DateTime previousTimeStamp = DateTime.MinValue;
			do
			{
				int cycleTime = CycleTime;
				if (EndThread) break;

				int loopCount = cycleTime/10;

				DateTime currentTimeStamp = DateTime.Now;
				if (actualCycleTime == 0)
				{
					TimeSpan duration = currentTimeStamp - previousTimeStamp;
					
					int diff = cycleTime - (int)duration.TotalMilliseconds;
					if (diff > 0)
						Thread.Sleep(diff);

					duration = DateTime.Now - previousTimeStamp;
					actualCycleTime = (int)duration.TotalMilliseconds;
				}
				
				if (EndThread) break;

				EventHelper.FireDelegateAsync(m_TimeTriggerAction, currentTimeStamp, actualCycleTime);
				
				previousTimeStamp = currentTimeStamp;
				actualCycleTime = 0;

				if (EndThread) break;

				for(int i=0; i<loopCount; i++)
				{
					Thread.Sleep(9);
					if (EndThread) break;
				}

			} while (EndThread == false);
		}

		#endregion

		#region Dispose Pattern

		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="ActiveTimeTrigger"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock ( this )
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock ( this )
			{
				if ( m_Disposed == false )
				{
					Dispose(true);
				}
			}
		}

		private void Dispose(bool isDispose)
		{
			Stop();
			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~ActiveTimeTrigger()
		{
			Dispose(false);
		}

		#endregion

		#region Properties (2)

		private bool EndThread
		{
			get
			{
				return m_EndThread;
			}

			set
			{
				m_EndThread = value;
			}
		}

		/// <summary>
		/// Gets or sets the cycle time of the active time trigger.
		/// </summary>
		/// <value>The cycle time in milliseconds.</value>
		public int CycleTime
		{
			get
			{
				lock (m_CycleTimeSync)
				{
					return m_CycleTime;
				}
			}

			set
			{
				lock (m_CycleTimeSync)
				{
					m_CycleTime = value > 10 ? value : 10;
				}
			}
		}

		#endregion

		#region Methods (2)


		/// <summary>
		/// Starts the active time trigger.
		/// </summary>
		/// <param name="timeTriggerAction">The time trigger action.</param>
		public void Start(Action<DateTime, int> timeTriggerAction)
		{
			if (Disposed) throw new ObjectDisposedException("Object is already disposed");

			ArgChecker.ShouldNotBeNull(timeTriggerAction, "timeTriggerAction");

			lock (m_StartStopSync)
			{
				if (m_Start) return;

				m_TimeTriggerAction = timeTriggerAction;

				m_Start = true;
				EndThread = false;
				m_Stopped = false;
				m_TimeTriggerThread.Start();
			}
		}

		/// <summary>
		/// Stopps the Standard Schedules Message Engine
		/// </summary>
		public void Stop()
		{
			if (Disposed) throw new ObjectDisposedException("Object is already disposed");

			lock (m_StartStopSync)
			{
				if (m_Stopped) return;

				EndThread = true;

				if (!m_TimeTriggerThread.IsAlive) return;

				m_TimeTriggerThread.Join();
				m_Stopped = true;
				m_Start = false;
			}
		}

		#endregion
	}
}
