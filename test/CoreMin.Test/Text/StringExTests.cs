using System;
using System.Collections.Generic;
using System.Linq;

using SharpTestsEx;
using Xunit;
using Xunit.Extensions;

using SmartExpert;



namespace SmartExpert.Test.Text
{
	///<summary>StringEx Tests</summary>
	public class StringExTests : IDisposable
	{
		#region Ctor

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		[Theory]
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData("{X}", false)]
		[InlineData("{ 0}", false)]
		[InlineData("{1}", false)]
		[InlineData("{0}", true)]
		[InlineData("{0} {1}", true)]
		[InlineData("{1} {0}", true)]
		[InlineData("{0:##.##}", true)]
		[InlineData("Dummystring {0:##.##}", true)]
		[InlineData("{{0:##.##}}", true)]
		public void TestCase001_IsFormatString_Theory(string value, bool expectedResult)
		{
			//Console.WriteLine(".");
			value.IsFormatString().Should().Be(expectedResult);
		}

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
