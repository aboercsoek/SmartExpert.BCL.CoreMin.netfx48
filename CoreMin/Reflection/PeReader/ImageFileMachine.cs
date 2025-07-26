//--------------------------------------------------------------------------
// Class:	ImageFileMachine
// Copyright © 2008 Andreas Börcsök
// Content:	Implementation of class ImageFileMachine
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
	/// TODO: Description of class ImageFileMachine
	/// </summary>
	internal class ImageFileMachine
	{
		public string Constant { get; set; }
		public ushort Value { get; set; }
		public string Description { get; set; }
	}
}
