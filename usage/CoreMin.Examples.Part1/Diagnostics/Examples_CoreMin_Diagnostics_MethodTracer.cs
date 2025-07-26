//--------------------------------------------------------------------------
// File:    Examples_CoreMin_Diagnostics_MethodTracer.cs
// Content:	Implementation of class ExamplesCoreMinDiagnosticsMethodTracer
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using SmartExpert.CUI;
using SmartExpert.Diagnostics;

#endregion

namespace SmartExpert.Examples
{

	///<summary>MethodTracer Examples</summary>
	public static class ExamplesCoreMinDiagnosticsMethodTracer
	{
		[Description("MethodTracer examples")]
		public static void RunAll()
		{
			RunMethodTracerExamples();
		}
		
		public static void RunMethodTracerExamples()
		{
			ConsoleHelper.WriteHeader("MethodTracer Examples");
			
			#region MethodTracer_Default

			int calc = 1;

			using (new MethodTracer(MethodBase.GetCurrentMethod()))
			{
				for ( var i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteNameValue("calc", calc);
			}
			// DebugOutputString Output:
			// 15:26:36.906 MethodTracer: -->
			// 15:26:36.953 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.104 seconds (104 ms))
			// 15:26:37.062 MethodTracer: <--

			#endregion

			ConsoleHelper.NewLine();

			#region MethodTracer_Tracer

			calc = 1;
			using (new MethodTracer(MethodBase.GetCurrentMethod(), new ConsoleTracer()))
			{
				for ( int i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteLineWhite(calc);
			}
			// Console Output:
			// 15:26:36.906 MethodTracer: -->
			// 15:26:36.953 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.104 seconds (104 ms))
			// 15:26:37.062 MethodTracer: <--
			
			#endregion
			
			ConsoleHelper.NewLine();

			MethodTracerOuterScope();
		}

		#region MethodTracer_OuterInnerScope

		public static void MethodTracerOuterScope()
		{
			using (new MethodTracer(MethodBase.GetCurrentMethod()))
			{
				MethodTracerInnerScope();
				MethodTracerInnerScope();
			}
			// DebugOutputString Output:
			// 15:26:36.906 MethodTracer: -->
			// 15:26:36.953 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.RunScopeTracerExamples
			// 15:26:37.062 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.104 seconds (104 ms))
			// 15:26:37.062 MethodTracer: <--
			// 15:26:37.187 MethodTracer: -->
			// 15:26:37.187 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.MethodTracerOuterScope
			// 15:26:37.187 MethodTracer:    -->
			// 15:26:37.187 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.MethodTracerInnerScope
			// 15:26:37.234 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.MethodTracerInnerScope
			// 15:26:37.234 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.050 seconds (50 ms))
			// 15:26:37.234 MethodTracer:    <--
			// 15:26:37.234 MethodTracer:    -->
			// 15:26:37.234 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Entering ExamplesMethodTracer.MethodTracerInnerScope
			// 15:26:37.296 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.MethodTracerInnerScope
			// 15:26:37.296 MethodTracer:    [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.051 seconds (50 ms))
			// 15:26:37.296 MethodTracer:    <--
			// 15:26:37.296 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>]Exiting ExamplesMethodTracer.MethodTracerOuterScope
			// 15:26:37.296 MethodTracer: [Process: ID=5116|Name=Usage.Diagnostics] [Thread: ID=1|Name=<null>](Elapsed time: 00.113 seconds (113 ms))
			// 15:26:37.296 MethodTracer: <--

			ConsoleHelper.NewLine();
		}

		private static void MethodTracerInnerScope()
		{
			using (new MethodTracer(MethodBase.GetCurrentMethod()))
			{
				Thread.Sleep(50);
			}
			ConsoleHelper.NewLine();
		}

		#endregion		
		
	}
}
