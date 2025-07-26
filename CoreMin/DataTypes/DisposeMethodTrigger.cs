//--------------------------------------------------------------------------
// File:    DisposeMethodTrigger.cs
// Content:	Implementation of class DisposeMethodTrigger
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	
	/// <summary>
	/// Puts an IDisposable wrapper around a callback method allowing any 
	/// method to be used with the C# using statement. 
	/// </summary>
	public sealed class DisposeMethodTrigger : IDisposable
	{
		private readonly Action m_DisposeMethod;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisposeMethodTrigger"/> class.
		/// </summary>
		/// <param name="disposeMethod">The action that is executed if <see cref="Dispose"/> is called.</param>
		public DisposeMethodTrigger(Action disposeMethod)
		{
			ArgChecker.ShouldNotBeNull(disposeMethod, "disposeMethod");
			m_DisposeMethod = disposeMethod;
		}

		/// <summary>
		/// Executes an application-defined task while disposing.
		/// </summary>
		public void Dispose()
		{
			m_DisposeMethod();
		}

	}



}
