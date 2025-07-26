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

namespace SmartExpert.Eval
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

			new AppMenuController("SmartExpert CoreMin Evaluation",
				Evaluation1,
				Evaluation2,
				ThreadingEvaluation.Evaluation1,
				ThreadingEvaluation.Evaluation2).Run();
		}

		#endregion
	}
}
