//--------------------------------------------------------------------------
// File:    TestEnumWithFlags.cs
// Content:	Definition of enumeration TestEnumWithFlags
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------

using System;

namespace SmartExpert.Test.Repository
{
	///<summary>Test Enum with Flags attribute.</summary>
	[Flags]
	public enum TestEnumWithFlags
	{
		///<summary>Zero Testvalue.</summary>
		Zero = 0x00,
		///<summary>One Testvalue.</summary>
		[DisplayName("OneName")]
		One = 0x01,
		///<summary>Two Testvalue.</summary>
		[DisplayName("TwoName", "TwoKey")]
		Two = 0x02,
		///<summary>Three Testvalue.</summary>
		[DisplayName("ThreeName", null)]
		Three = 0x04,
		///<summary>Four Testvalue.</summary>
		[DisplayName(null, "FourKey")]
		Four = 0x08,
		///<summary>Five Testvalue.</summary>
		[DisplayName(null, null)]
		Five = 0x10,
		///<summary>Six Testvalue.</summary>
		[DisplayName("SixName", "")]
		Six = 0x20,
		///<summary>Seven Testvalue.</summary>
		[DisplayName("", "SevenKey")]
		Seven = 0x40,
		///<summary>Eight Testvalue.</summary>
		[DisplayName("", "")]
		Eight = 0x80,
	}
}
