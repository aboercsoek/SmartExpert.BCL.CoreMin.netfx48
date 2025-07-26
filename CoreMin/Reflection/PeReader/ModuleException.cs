//--------------------------------------------------------------------------
// Class:	ModuleException
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class ModuleException
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SmartExpert.Reflection.PeReader
{
	/// <summary>
	/// TODO: Description of class ModuleException
	/// </summary>
	public class ModuleException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ModuleException(string message, Exception innerException)
			: base(message, innerException) 
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ModuleException(string message)
			: base(message) 
		{ }
	}
}
