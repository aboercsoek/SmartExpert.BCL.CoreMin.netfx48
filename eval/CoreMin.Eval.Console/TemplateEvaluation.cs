//--------------------------------------------------------------------------
// File:    {Template}Evaluation.cs
// Content:	Implementation of class {Template}Evaluation
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.{Template};

using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Linq;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Eval
{
	///<summary>{Template} Evaluation</summary>
	public static class {Template}Evaluation
	{

		[Description("Evaluation-1 {Template}")]
		public static void Evaluation1()
		{
		try
			{
				#region Evaluation_Code

				// Evaluation code

				#endregion

			}
			catch(Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}

		[Description("Evaluation-2 {Template}")]
		public static void Evaluation2()
		{
			try
			{

				#region Evaluation_Code

				#endregion

			}
			catch(Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}

		[Description("Evaluation-3 {Template}")]
		public static void Evaluation3()
		{
			try
			{
				#region Evaluation_Code

				// Evaluation code

				#endregion

			}
			catch(Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}

		[Description("Evaluation-4 {Template}")]
		public static void Evaluation4()
		{
			try
			{
				#region Evaluation_Code

				// Evaluation code

				#endregion

			}
			catch(Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}
		}

	}

}
