//--------------------------------------------------------------------------
// File:    TestEnumNoFlags.cs
// Content:	Definition of enumeration TestEnumNoFlags
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------

namespace SmartExpert.Test.Repository
{
	///<summary>Test Enum with no Flags attribute</summary>
	public enum TestEnumNoFlags
	{
		///<summary>Zero Testvalue.</summary>
		Zero = 0,
		///<summary>One Testvalue.</summary>
		[DisplayName("OneName")]
		One = 1,
		///<summary>Two Testvalue.</summary>
		[DisplayName("TwoName", "TwoKey")]
		Two = 2,
		///<summary>Three Testvalue.</summary>
		[DisplayName("ThreeName", null)]
		Three = 3,
		///<summary>Four Testvalue.</summary>
		[DisplayName(null, "FourKey")]
		Four = 4,
		///<summary>Five Testvalue.</summary>
		[DisplayName(null, null)]
		Five = 5,
		///<summary>Six Testvalue.</summary>
		[DisplayName("SixName", "")]
		Six = 6,
		///<summary>Seven Testvalue.</summary>
		[DisplayName("", "SevenKey")]
		Seven = 7,
		///<summary>Eight Testvalue.</summary>
		[DisplayName("", "")]
		Eight = 8,
	}
}
