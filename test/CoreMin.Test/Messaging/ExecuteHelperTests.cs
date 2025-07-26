//<file>
//	<name>ExecuteHelperTests.cs</file>
//	<content>Implementation of class ExecuteHelperTests</content>
//	<owner name="Andreas Börcsök" email="support@smartexpert.boercsoek.de" />
//	<website>http://www.smartexpert.de</website>
//	<copyright>Copyright 2010 Andreas Börcsök</copyright>
//</file>
#region Using directives
using System;
using System.Linq;
using SharpTestsEx;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.Messaging;
using Xunit;
using SmartExpert.Logging;

#endregion

namespace SmartExpert.Test.Messaging
{
	///<summary>ExecuteHelper Tests</summary>
	public class ExecuteHelperTests : IDisposable
	{

		public ExecuteHelperTests()
		{
		}

		#region Test Methods
		// ReSharper disable InconsistentNaming
        // ReSharper disable UnusedVariable

		[Fact]
		public void TestCase001_ExecuteHelper_TryCatch_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatch(() => { int i = 0; int test = 100/i;} )).Should().NotThrow();
		}

		[Fact]
		public void TestCase002_ExecuteHelper_TryCatchSwallowIfNotFatal_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchSwallowIfNotFatal(() => { int i = 0; int test = 100 / i; })).Should().NotThrow();
		}

		[Fact]
		public void TestCase003_ExecuteHelper_TryCatchSwallowIfNotFatalActionT_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchSwallowIfNotFatal<int>(i => { int test = 100/i; }, 0)).Should().NotThrow();
		}

		[Fact]
		public void TestCase004_ExecuteHelper_TryCatchSwallowIfNotFatalActionT1T2_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchSwallowIfNotFatal<int,int>((i,t) => { int test = t/i; }, 0, 100)).Should().NotThrow();
		}

		[Fact]
		public void TestCase005_ExecuteHelper_TryCatchSwallowIfNotFatalActionT1T2T3_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchSwallowIfNotFatal<int, int, int>((t, i, z) => { int test = t/(i -z); }, 100, 5, 5)).Should().NotThrow();
		}

		[Fact]
		public void TestCase006_ExecuteHelper_TryCatchSwallowIfNotFatalFuncT_SwallowExceptionAndReturnDefaultValue()
		{
			ExecuteHelper.TryCatchSwallowIfNotFatal<int>(() => { int i = 0; return 100/i; }).Should().Be(0);
		}

		[Fact]
		public void TestCase007_ExecuteHelper_TryCatchSwallowIfNotFatalFuncT1T2_SwallowExceptionAndReturnDefaultValue()
		{
			ExecuteHelper.TryCatchSwallowIfNotFatal<int, int>(i => 100 / i, 5).Should().Be(20);
			ExecuteHelper.TryCatchSwallowIfNotFatal<int, int>(i => 100 / i, 0).Should().Be(0);
		}

		[Fact]
		public void TestCase008_ExecuteHelper_TryCatchHandle_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchHandle(() => { int i = 0; int test = 100 / i; })).Should().NotThrow();
		}

		[Fact]
		public void TestCase009_ExecuteHelper_TryCatchHandle_SwallowDivideByZeroException()
		{
			Executing.This(() => ExecuteHelper.TryCatchHandle(
									() => {int i=0; int test=100/i; }, 
									ex => ex.GetType().Should().Be(typeof(DivideByZeroException)))
							).Should().NotThrow();
		}

		[Fact]
		public void TestCase010_ExecuteHelper_TryCatchLog_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; })).Should().NotThrow();
		}

		[Fact]
		public void TestCase011_ExecuteHelper_TryCatchLog_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, "Test Error message.")).Should().NotThrow();
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, string.Empty)).Should().NotThrow();
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, null)).Should().NotThrow();
		}

		[Fact]
		public void TestCase012_ExecuteHelper_TryCatchLog_SwallowException()
		{
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, "Test Error message.", LogContext.FAULT)).Should().NotThrow();
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, string.Empty, LogContext.FAULT)).Should().NotThrow();
			Executing.This(() => ExecuteHelper.TryCatchLog(() => { int i = 0; int test = 100 / i; }, null, LogContext.DIAGNOSTICS)).Should().NotThrow();
		}

        // ReSharper restore UnusedVariable
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
