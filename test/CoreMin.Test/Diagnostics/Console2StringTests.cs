//<file>
//	<name>Console2StringTests.cs</file>
//	<content>Implementation of test class for Console2String tests</content>
//	<owner name="Andreas Börcsök" email="support@smartexpert.boercsoek.de" />
//	<website>http://www.smartexpert.de</website>
//	<copyright>Copyright 2010 Andreas Börcsök</copyright>
//</file>

#region Using directives
using System;
using System.Linq;
using System.Text;
using SharpTestsEx;
using SmartExpert;
using SmartExpert.CUI;
using SmartExpert.Diagnostics;
using SmartExpert.Error;
using SmartExpert.Linq;
using Xunit;

#endregion

namespace SmartExpert.Test.Diagnostics
{
	///<summary>Console2String Tests</summary>
	public class Console2StringTests : IDisposable
	{
		#region Ctor

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		[Fact]
		public void TestCase001_Console2String_ConsoleWrite_IsRedirectedToStringBuilder()
		{
			var sb = new StringBuilder();
			using (new Console2String(sb))
			{
				Console.Write(@"Test öäü$");
			}
			sb.ToString().Should().Be(@"Test öäü$");
			sb.Clear();

			using (new Console2String(sb))
			{
				Console.Write(@"Test öäü$");
				Console.Write(@"Test öäü$");
			}
			sb.ToString().Should().Be(@"Test öäü$Test öäü$");
			sb.Clear();
		}

		[Fact]
		public void TestCase002_Console2String_ConsoleWriteLine_IsRedirectedToStringBuilder()
		{
			var sb = new StringBuilder();
			using (new Console2String(sb))
			{
				Console.WriteLine(@"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();

			using (new Console2String(sb))
			{
				Console.WriteLine(@"Test öäü$");
				Console.WriteLine(@"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\nTest öäü$\r\n");
			sb.Clear();
		}

		[Fact]
		public void TestCase003_Console2String_ConsoleHelperWrite_IsRedirectedToStringBuilder()
		{
			var sb = new StringBuilder();
			using (new Console2String(sb))
			{
				ConsoleHelper.Write(@"Test öäü$", ConsoleColor.White);
			}
			sb.ToString().Should().Be(@"Test öäü$");
			sb.Clear();

			using (new Console2String(sb))
			{
				ConsoleHelper.Write(@"Test öäü$", ConsoleColor.White);
				ConsoleHelper.Write(@"Test öäü$", ConsoleColor.Green);
			}
			sb.ToString().Should().Be(@"Test öäü$Test öäü$");
			sb.Clear();
		}

		[Fact]
		public void TestCase004_Console2String_ConsoleHelperWriteLine_IsRedirectedToStringBuilder()
		{
			var sb = new StringBuilder();
			using (new Console2String(sb))
			{
				ConsoleHelper.WriteLine(@"Test öäü$", ConsoleColor.White);
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();

			using (new Console2String(sb))
			{
				ConsoleHelper.WriteLine(@"Test öäü$", ConsoleColor.White);
				ConsoleHelper.WriteLine(@"Test öäü$", ConsoleColor.White);
			}
			sb.ToString().Should().Be("Test öäü$\r\nTest öäü$\r\n");
			sb.Clear();

			using (new Console2String(sb))
			{
				ConsoleHelper.WriteLineWhite(@"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();

			using (new Console2String(sb))
			{
				ConsoleHelper.WriteLineWhite(@"Test öäü$", Paragraph.AddBeforeAndAfter);
			}
			sb.ToString().Should().Be("\r\nTest öäü$\r\n\r\n");
			sb.Clear();
		}

		//[Fact]
		//public void TestCase005_Console2String_LogServiceDebugInfoWarnError_IsRedirectedToStringBuilder()
		//{
			/*ILogService logService = AppContext.GetHostingAwareInstance().LogService;

			var sb = new StringBuilder();
			using (new Console2String(sb))
			{
				if (logService != null)
					logService.Info(LogContext.TEST, @"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();
			
			using (new Console2String(sb))
			{
				if (logService != null)
					logService.Debug(LogContext.TEST, @"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();
			
			using (new Console2String(sb))
			{
				if (logService != null)
					logService.Warn(LogContext.TEST, @"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();
			
			using (new Console2String(sb))
			{
				if (logService != null)
					logService.Error(LogContext.TEST, @"Test öäü$");
			}
			sb.ToString().Should().Be("Test öäü$\r\n");
			sb.Clear();*/
		//}

		// ReSharper restore InconsistentNaming
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			//AppContext.Shutdown();
		}

		#endregion
	}
}
