//--------------------------------------------------------------------------
// File:    Paragraph.cs
// Content:	Definition of enumaretion Paragraph
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.CUI
{
	///<summary>Additional paragraph support.</summary>
	[Flags]
	public enum Paragraph
	{
		/// <summary>No additional paragraphs are added.</summary>
		Default = 0,
		/// <summary>No additional paragraphs are added.</summary>
		AddNoParagraph = 0,
		/// <summary>Add paragraph before WriteLine.</summary>
		AddBefore = 1,
		/// <summary>Add paragraph after WriteLine.</summary>
		AddAfter = 2,
		/// <summary>Add paragraph before and after WriteLine.</summary>
		AddBeforeAndAfter = 3
	}
}
