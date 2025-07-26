//--------------------------------------------------------------------------
// File:    Examples_CoreMin_Diagnostics_ScopeTracer.cs
// Content:	Implementation of class ExamplesCoreMinDiagnosticsScopeTracer
// Author:	Andreas Börcsök
// Website:	http://smartexpert.boercsoek.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using SmartExpert.CUI;
using SmartExpert.Diagnostics;

#endregion

namespace SmartExpert.Examples
{
	///<summary>ScopeTracer Examples</summary>
	public static class ExamplesCoreMinDiagnosticsScopeTracer
	{
		[Description("ScopeTracer examples")]
		public static void RunAll()
		{
			RunScopeTracerExamples();
		}

		public static void RunScopeTracerExamples()
		{
			ConsoleHelper.WriteHeader("ScopeTracer Examples");

			#region ScopeTracer_Default

			int calc = 1;

			using (new ScopeTracer())
			{
				for ( int i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteLineWhite(calc);
			}
			// Debug Output:
			// 17:25:47.546 (4164,9) ScopeTracer: Enter[#1]: 
			// 17:25:47.734 (4164,9) ScopeTracer: Leave[#1|time:190 ms]: 

			#endregion

			ConsoleHelper.NewLine();

			#region ScopeTracer_Message

			calc = 1;
			using ( new ScopeTracer("my scope message") )
			{
				for ( int i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteLineWhite(calc);
			}
			// Debug Output:
			// 17:25:47.734 (4164,9) ScopeTracer: Enter[#1]: my scope message
			// 17:25:47.843 (4164,9) ScopeTracer: Leave[#1|time:100 ms]: my scope message

			#endregion

			ConsoleHelper.NewLine();

			#region ScopeTracer_Tracer

			calc = 1;
			using ( new ScopeTracer(new ConsoleTracer()) )
			{
				for ( int i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteLineWhite(calc);
			}
			// Console Output:
			// 17:25:47.843 (4164,9) ScopeTracer: Enter[#1]: 
			// 17:25:47.943 (4164,9) ScopeTracer: Leave[#1|time:100 ms]: 
			
			#endregion
			
			ConsoleHelper.NewLine();

			#region ScopeTracer_TracerPlusMessage

			calc = 1;
			using ( new ScopeTracer(new ConsoleTracer(), "my scope message") )
			{
				for ( int i = 1; i < 20; i++ )
				{ calc *= i; }
				Thread.Sleep(100);
				ConsoleHelper.WriteLineWhite(calc);
			}

			// Console Output:
			// 17:25:47.943 (4164,9) ScopeTracer: Enter[#1]: my scope message
			// 17:25:48.043 (4164,9) ScopeTracer: Leave[#1|time:100 ms]: my scope message

			#endregion

			ConsoleHelper.NewLine();

			ScopeTracerOuterScope();
		}

		#region ScopeTracer_OuterInnerScope

		public static void ScopeTracerOuterScope()
		{
			using ( new ScopeTracer("outer scope") )
			{
				ScopeTracerInnerScope();
				ScopeTracerInnerScope();
			}
			// Debug Output:
			// 18:00:27.015 (1308,11) ScopeTracer: Enter[#1]: outer scope
			// 18:00:27.015 (1308,11) ScopeTracer: Enter[#2]: inner scope
			// 18:00:27.078 (1308,11) ScopeTracer: Leave[#2|time:50 ms]: inner scope
			// 18:00:27.078 (1308,11) ScopeTracer: Enter[#2]: inner scope
			// 18:00:27.125 (1308,11) ScopeTracer: Leave[#2|time:50 ms]: inner scope
			// 18:00:27.125 (1308,11) ScopeTracer: Leave[#1|time:105 ms]: outer scope
			ConsoleHelper.NewLine();
		}

		private static void ScopeTracerInnerScope()
		{
			using ( new ScopeTracer("inner scope") )
			{
				Thread.Sleep(50);
			}
			ConsoleHelper.NewLine();
		}

		#endregion
	}
}
