//--------------------------------------------------------------------------
// File:    ThreadingEvaluation.cs
// Content:	Implementation of class ThreadingEvaluation
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Linq;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Eval
{
	///<summary>Threading Evaluation</summary>
	public static class ThreadingEvaluation
	{

		[Description("Evaluation-1 Threading")]
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

		[Description("Evaluation-2 Threading")]
		public static void Evaluation2()
		{
			try
			{
				#region Evaluation_Code

				#endregion

			}
			catch (Exception ex)
			{
				ConsoleHelper.WriteLineRed(ex.RenderExceptionDetails());
			}


		}

		[Description("Evaluation-3 Threading")]
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

		[Description("Evaluation-4 Threading")]
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
