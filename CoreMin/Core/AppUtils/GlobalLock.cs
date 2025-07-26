//--------------------------------------------------------------------------
// File:    Globals.cs
// Content:	Implementation of class Globals
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.AppUtils
{
	/// <summary>
	/// Static class encasulating global elements.
	/// </summary>
	public static class Globals
	{
		/// <summary>
		/// All synchronization code should exclusively use this lock object,
		/// hence making it trivial to ensure that there are no deadlocks.
		/// It also means that the lock should never be held for long.
		/// In particular, no code holding this lock should ever wait on another thread.
		/// </summary>
		public static readonly object LockingObject = new object();

		//private const bool forceSingleStep = true;

		/// <summary>
		/// Signals a breakpoint to the attached Debugger.
		/// </summary>
		[DebuggerNonUserCode]
		public static void BreakForDebugging()
		{
			//if ( forceSingleStep )
			//{
				Debugger.Break();
			//}
		}
	}
}
