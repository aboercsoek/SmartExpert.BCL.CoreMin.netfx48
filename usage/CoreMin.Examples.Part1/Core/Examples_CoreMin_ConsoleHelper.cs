//--------------------------------------------------------------------------
// File:    Examples_CoreMin_ConsoleHelper.cs
// Content:	Implementation of class Examples_CoreMin_ConsoleHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.ComponentModel;
using System.Linq;
using System.Security;
using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Error;
using SmartExpert.IO;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Examples
{
// ReSharper disable InconsistentNaming

	///<summary>ExamplesCoreMinConsoleHelper</summary>
	public static class ExamplesCoreMinConsoleHelper
	{
		[Description("ConsoleHelper examples")]
		public static void RunAll()
		{
			FileHelper.GetFiles(@".\", "Sample_CoreMin_CUI*.txt").ForEach(FileHelper.DeleteFile);

			ConsoleHelperWriteAndWriteLineExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 1

			ConsoleHelperWriteNameValueExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 2

			ConsoleHelperWriteCollectionExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 3

			ConsoleHelperWriteColorExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 4

			ConsoleHelperSpecialWriteExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 5

			ConsoleHelperReadLineExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 6

			ConsoleHelperReadKeyExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 7

			ConsoleHelperReadPasswordExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine(); // 8
		}

		[Description("Write and WriteLine Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperWriteAndWriteLineExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.WriteAndWriteLine example [1/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_WriteAndWriteLine.txt";

				#region Sample_CoreMin_CUI_M_ConsoleHelper_WriteAndWriteLine

				ConsoleHelper.Write("The Answer to Life, the Universe, and Everything is ", ConsoleColor.White);
				ConsoleHelper.Write("{0}\n", ConsoleColor.Magenta, 42);
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteLine("The Answer to Life, the Universe, and Everything is 42", ConsoleColor.White);
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteLine("The Answer to Life, the Universe, and Everything is {0}", ConsoleColor.Magenta, 42);
				ConsoleHelper.NewLine();

				var e = new ArgumentNullException("paramName");
				ConsoleHelper.WriteLine(e);

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("WriteLine<Color> Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperWriteColorExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.Write{color} example [2/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_WriteColor.txt";

				#region Sample_CoreMin_CUI_M_ConsoleHelper_WriteColor

				ConsoleHelper.WriteLineGreen("Console text in green.");

				ConsoleHelper.WriteLineWhite("Console text in white.", Paragraph.Default);

				ConsoleHelper.WriteLineRed("Console text in red.", Paragraph.AddAfter);

				ConsoleHelper.WriteLineMagenta("Console text in magenta", Paragraph.AddBefore);

				ConsoleHelper.WriteLineYellow("Console text in yellow", Paragraph.AddBeforeAndAfter);

				ConsoleHelper.WriteLineGray("Console text in gray", Paragraph.AddNoParagraph);

				ConsoleHelper.WriteLineBlue("Console text in blue", Paragraph.AddBeforeAndAfter);

				ConsoleHelper.WriteLineCyan("Console text in cyan");

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("WriteNameValue Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperWriteNameValueExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.WriteNameValue example [3/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_WriteNameValue.txt";

				#region Sample_CoreMin_CUI_M_ConsoleHelper_WriteNameValue

				ConsoleHelper.WriteNameValue("intValue", 42);
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("intValue", 0, ConsoleColor.Magenta, ConsoleColor.White);
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("intValue", -1, ConsoleColor.Cyan, ConsoleColor.White, ": ", ConsoleColor.Yellow);
				ConsoleHelper.NewLine();

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("WriteCollection Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperWriteCollectionExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.WriteCollection example [4/8]");

			#region Sample_CoreMin_CUI_F_ConsoleHelper_OutputFileName
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_WriteCollection.txt";

				#region Sample_CoreMin_CUI_M_ConsoleHelper_WriteCollection

				var intArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

				ConsoleHelper.WriteCollection("intArray", intArray, " | ", ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow);

				ConsoleHelper.WriteNameValue("intArray", intArray, ConsoleColor.Green, ConsoleColor.White, ": ", ConsoleColor.Yellow);

				ConsoleHelper.WriteCollection("intArray", intArray, " | ");
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteCollection(intArray, ConsoleColor.Green, ", ");
				ConsoleHelper.NewLine();

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
			#endregion
		}

		[Description("Special Write Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperSpecialWriteExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.SpecialWrite example [5/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_SpecialWrite.txt";
				#region Sample_CoreMin_CUI_M_ConsoleHelper_SpecialWrite

				ConsoleHelper.HR();
				ConsoleHelper.HR(ConsoleColor.Magenta);
				ConsoleHelper.HR(42, ConsoleColor.White);
				ConsoleHelper.HR(42, '*', ConsoleColor.Green);
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteHeader("Test of WriteHeader");
				ConsoleHelper.NewLine();

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("ReadLine Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperReadLineExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.ReadLine example [6/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_ReadLine.txt";
				#region Sample_CoreMin_CUI_M_ConsoleHelper_ReadLine

				string readString1 = ConsoleHelper.ReadLine("Type some text and hit <Enter>!");
				ConsoleHelper.WriteLineGreen("You entered: " + readString1);
				string readString2 = ConsoleHelper.ReadLine("Type some text and hit <Enter>!", ConsoleColor.White);
				ConsoleHelper.WriteLineGreen("You entered: " + readString2);

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}

		}

		[Description("ReadKey Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperReadKeyExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.ReadKey example [7/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_ReadKey.txt";
				#region Sample_CoreMin_CUI_M_ConsoleHelper_ReadKey

				// Get user input. Shows input message (in yellow) and user input.
				ConsoleKeyInfo cki1 = ConsoleHelper.ReadKey("Hit <ANY KEY> to proceed!", true);
				// User input can be checked via cki1
				ConsoleHelper.NewLine();

				// Get user input. Shows input message (in yellow) but hides user input.
				ConsoleKeyInfo cki2 = ConsoleHelper.ReadKey("Hit <ANY KEY> to proceed!", false);
				ConsoleHelper.NewLine();
				ConsoleHelper.WriteLine("You hit the key: {0} {1}", ConsoleColor.White, cki2.Modifiers, cki2.Key);

				// Get user input. Shows input message (in magenta) but hides user input.
				ConsoleKeyInfo cki3 = ConsoleHelper.ReadKey("Hit <ANY KEY> to proceed!", false, ConsoleColor.Magenta);
				// User input can be checked via cki3
				ConsoleHelper.NewLine();

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}

		}

		[Description("ReadPassword Helpers (ConsoleHelper example)")]
		public static void ConsoleHelperReadPasswordExample()
		{
			ConsoleHelper.WriteHeader("ConsoleHelper.ReadPassword example [8/8]");
			try
			{
				ConsoleHelper.OutputFileName = "Sample_CoreMin_CUI_M_ConsoleHelper_ReadPassword.txt";
				#region Sample_CoreMin_CUI_M_ConsoleHelper_ReadPassword

				var secureString = new SecureString();
				ConsoleKey lastKey1 = ConsoleHelper.ReadPasswordSecure(ref secureString, "Enter password the secure way: ", '#', ConsoleColor.Red);
				ConsoleHelper.NewLine();
				ConsoleHelper.WriteLine("LastKey: 0x{0:X0000}, {1}", ConsoleColor.White, (int)lastKey1, lastKey1);

				string stdString = string.Empty;
				ConsoleKey lastKey2 = ConsoleHelper.ReadPassword(ref stdString, "Enter password: ", '*', ConsoleColor.Red);
				ConsoleHelper.NewLine();
				ConsoleHelper.WriteLine("LastKey: 0x{0:X0000}, {1}", ConsoleColor.White, (int)lastKey2, lastKey2);

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}

		}

// ReSharper restore InconsistentNaming

	}
}
