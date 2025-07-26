//--------------------------------------------------------------------------
// File:    Program.MenuHandlers.cs
// Content:	Console Menu Handlers
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Drawing;

#endregion

namespace SmartExpert.Eval
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

	///<summary>Console App Menu Handlers</summary>
	partial class Program
	{
		[Description("General Evaluation 1")]
		internal static void Evaluation1()
		{
			try
			{
				#region Evaluation_Code

				// Evaluation code
				//string value = "NotDefined";
				string value = "Six";
				TestEnumNoFlags? result = value.EnumParse<TestEnumNoFlags>(false);
				//if (result.HasValue)
				if (result != null)
					ConsoleHelper.WriteLine(result.Value, ConsoleColor.DarkYellow);
				else
					ConsoleHelper.WriteLine("No Value!", ConsoleColor.DarkRed);

				string[] names = EnumHelper.GetNamesOfValue(typeof(TestEnumWithFlags), (TestEnumWithFlags) 0xFF);
				if (names != null)
					ConsoleHelper.WriteLine(names.Length, ConsoleColor.DarkYellow);

				#endregion

			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}

		[Description("General Evaluation 2")]
		internal static void Evaluation2()
		{
			try
			{
				#region Evaluation_Code

				// Evaluation code

				#endregion

			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}
	}
}
