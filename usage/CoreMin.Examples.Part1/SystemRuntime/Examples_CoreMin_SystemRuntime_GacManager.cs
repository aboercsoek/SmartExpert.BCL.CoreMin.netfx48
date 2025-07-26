//--------------------------------------------------------------------------
// File:    Examples_CoreMin_SystemRuntime_GacManager.cs
// Content:	Implementation of class Sample_CoreMin_SystemRuntime_GacManager
// Author:	Andreas Börcsök
// Website:	http://smartexpert.boercsoek.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.SystemRuntime;

#endregion

namespace SmartExpert.Examples
{
	///<summary>GacManager Examples</summary>
	public static class ExamplesCoreMinSystemRuntimeGacManager
	{
		[Description("GacManager examples")]
		public static void RunAll()
		{
			GacManagerCopyGacAssembliesExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();

			GacAddRemoveAssemblyExample();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
		}

		public static void GacManagerCopyGacAssembliesExample()
		{
			//using (new Console2File("Sample_CoreMin_SystemRuntime_T_GacManager_ReadAccess.txt"))
			{
				#region Sample_CoreMin_SystemRuntime_T_GacManager_ReadAccess

				using ( GacManager gacManager = new GacManager(false) )
				{
					ConsoleHelper.WriteLine("Init internal GAC cache ...", ConsoleColor.White);
					int count = gacManager.InitGacCache();
					ConsoleHelper.WriteLineWhite("GAC contains {0} assemblies.".SafeFormatWith(count), Paragraph.AddAfter);

					ConsoleHelper.WriteLine("Saving all GAC assembly names ...", ConsoleColor.White);
					gacManager.SaveGacAssemblyNames("GacAssemblies.txt");
					ConsoleHelper.WriteLineWhite("The file GacAssemblies.txt contains the names of all GAC assemblies.", Paragraph.AddAfter);
					
					ConsoleHelper.WriteLineWhite("Start copying mscor* GAC assembly files ...");
					count = gacManager.CopyGacAssembliesToFolder("mscor", "", @"F:\Workbench\Res.AsmRef\");
					ConsoleHelper.WriteLineWhite("Copied {0} files from the GAC".SafeFormatWith(count), Paragraph.AddAfter);

					ConsoleHelper.WriteLineWhite("Start copying PowerShell GAC assembly files ...");
					count = gacManager.CopyGacAssembliesToFolder("Microsoft.PowerShell", null, @"F:\Workbench\Res.AsmRef\Microsoft.PowerShell");
					ConsoleHelper.WriteLineWhite("Copied {0} files from the GAC".SafeFormatWith(count), Paragraph.AddAfter);
				}

				#endregion
			}
		}

		public static void GacAddRemoveAssemblyExample()
		{
			//using ( new Console2File("Sample_CoreMin_SystemRuntime_T_GacManager_Management.txt") )
			{
			#region Sample_CoreMin_SystemRuntime_T_GacManager_Management

			// Create Assembly Manager and init internal GAC cache
			using (GacManager gacManager = new GacManager(true))
			{
				const string asmFilePath = @"F:\CC\SmartExpert\Workspace\BCL.Binaries\SmartExpert.BCL.CoreMin.dll";
				const string asmName = "SmartExpert.BCL.CoreMin";

				//------------------------------------------------------------------
				// Search for assembly in GAC
				IEnumerable<AssemblyNameWrapper> matchingAssemblies = gacManager.Search(asmName, "2.0.0.0");
				// Exit if already in GAC
				if ( matchingAssemblies.Count() > 0 ) return;
				
				//------------------------------------------------------------------
				// Add assembly to GAC
				ConsoleHelper.WriteLineMagenta("AddToGac({0})".SafeFormatWith(asmFilePath));
				bool addAssemblyResult = gacManager.AddToGac(asmFilePath);
				// Exit if AddToGac was not successful
				if ( addAssemblyResult == false ) return;
				
				//------------------------------------------------------------------
				// Rebuild internal GAC cache
				gacManager.ForceInitGacCache();
				
				//------------------------------------------------------------------
				// Search again in internal GAC cache.
				var asm = gacManager.Search(a => a.Name == asmName).FirstOrDefault();
				ConsoleHelper.WriteLineWhite("GAC search result: {0}".SafeFormatWith(asm));
				// Exit if assembly is not in GAC
				if (asm == null) return;
				ConsoleHelper.WriteLineGreen("AddToGac was successful.", Paragraph.AddAfter);

				//------------------------------------------------------------------
				// Remove assembly from GAC
				ConsoleHelper.WriteLineMagenta("RemoveFromGac({0})".SafeFormatWith(asm));
				bool removeAssemblyResult = gacManager.RemoveFromGac(asm);
				// bool removeAssemblyResult = asmManager.RemoveFromGac(asm.StrongName); // alternative method call
				// Exit if RemoveFromGac was not successful
				if ( removeAssemblyResult == false ) return;

				//------------------------------------------------------------------
				// Rebuild internal GAC cache
				gacManager.ForceInitGacCache();

				//------------------------------------------------------------------
				// Search again in internal GAC cache.
				matchingAssemblies = gacManager.Search(asmName, "2.0.0.0");
				// Exit if assembly is still in GAC
				if ( matchingAssemblies.Count() != 0 ) return;
				ConsoleHelper.WriteLineGreen("RemoveFromGac was successful.", Paragraph.AddAfter);
			}
			#endregion
			}
		}
	}
}
