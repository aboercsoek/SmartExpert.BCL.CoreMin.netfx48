//--------------------------------------------------------------------------
// File:    Program.cs
// Content:	Console Application
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;

using SmartExpert.CUI;
using SmartExpert.Linq;

#endregion

namespace SmartExpert.Examples
{
	/// <summary>Console Application Class</summary>
	partial class Program
	{
		#region Console Application Startup

		/// <summary>
		/// Console Application Entrypoint
		/// </summary>
		/// <param name="args">Commandline arguments</param>
		static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

			new AppMenuController("SmartExpert.BCL.CoreMin Examples - Part 1",
				MenuItem1Handler,
				MenuItem2Handler,
				ExamplesCoreMinConsoleHelper.RunAll,
				ExamplesCoreMinCoreObjectEx.RunAll,
				ExamplesCoreMinDiagnosticsScopeTracer.RunAll,
				ExamplesCoreMinDiagnosticsMethodTracer.RunAll,
				ExamplesCoreMinErrorExceptionHelper.RunAll,
				ExamplesCoreMinSystemRuntimeGacManager.RunAll,
                ExamplesCoreMinSecurityLsaAccountManager.RunAll).Run();
		}

		#endregion
	}
}
