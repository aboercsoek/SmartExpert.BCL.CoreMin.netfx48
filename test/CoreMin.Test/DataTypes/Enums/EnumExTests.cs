//--------------------------------------------------------------------------
// File:    EnumExTests.cs
// Content:	Implementation of class EnumExTests
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using SmartExpert.Error;
using SmartExpert.Test.Repository;
using Xunit;
using Xunit.Extensions;
using FluentAssertions;
using SmartExpert;

#endregion

namespace SmartExpert.Test.DataTypes.Enums
{
	///<summary>EnumEx Tests</summary>
	public class EnumExTests
	{
		#region ctor

		public EnumExTests()
		{

		}

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		#region GetDisplayName test methods

		#region TestCase001Data
		public static IEnumerable<object[]> TestCase001Data
		{
			get
			{
				yield return new object[] { TestEnumNoFlags.Zero, "Zero" };
				yield return new object[] { TestEnumNoFlags.One, "OneName" };
				yield return new object[] { TestEnumNoFlags.Two, "TwoName" };
				yield return new object[] { TestEnumNoFlags.Three, "ThreeName" };
				yield return new object[] { TestEnumNoFlags.Four, "Four" };
				yield return new object[] { TestEnumNoFlags.Five, "Five" };
				yield return new object[] { TestEnumNoFlags.Six, "SixName" };
				yield return new object[] { TestEnumNoFlags.Seven, "Seven" };
				yield return new object[] { TestEnumNoFlags.Eight, "Eight" };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase001Data")]
		public void TestCase001_EnumExtensions_GetDisplayName_WithTestEnumNoFlagsTheory(TestEnumNoFlags value, string expectedResult)
		{
			value.GetDisplayName().Should().Be(expectedResult);
		}

		#region TestCase002Data
		public static IEnumerable<object[]> TestCase002Data
		{
			get
			{
				yield return new object[] { (TestEnumWithFlags)0x00, "Zero" };
				yield return new object[] { (TestEnumWithFlags)0x01, "OneName" };
				yield return new object[] { (TestEnumWithFlags)0x02, "TwoName" };
				yield return new object[] { (TestEnumWithFlags)0x04, "ThreeName" };
				yield return new object[] { (TestEnumWithFlags)0x08, "Four" };
				yield return new object[] { (TestEnumWithFlags)0x10, "Five" };
				yield return new object[] { (TestEnumWithFlags)0x20, "SixName" };
				yield return new object[] { (TestEnumWithFlags)0x40, "Seven" };
				yield return new object[] { (TestEnumWithFlags)0x80, "Eight" };
				yield return new object[] { (TestEnumWithFlags)0x0F, "OneName, TwoName, ThreeName, Four" };
				yield return new object[] { (TestEnumWithFlags)0xF0, "Five, SixName, Seven, Eight" };
				yield return new object[] { (TestEnumWithFlags)0xFF, "OneName, TwoName, ThreeName, Four, Five, SixName, Seven, Eight" };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase002Data")]
		public void TestCase002_EnumExtensions_GetDisplayName_WithTestEnumWithFlagsTheory(TestEnumWithFlags value, string expectedResult)
		{
			value.GetDisplayName().Should().Be(expectedResult);
		}

		#endregion

		#region GetDisplayNameKey test methods

		#region TestCase003Data
		public static IEnumerable<object[]> TestCase003Data
		{
			get
			{
				yield return new object[] { TestEnumNoFlags.Zero, "Zero" };
				yield return new object[] { TestEnumNoFlags.One, "One" };
				yield return new object[] { TestEnumNoFlags.Two, "TwoKey" };
				yield return new object[] { TestEnumNoFlags.Three, "Three" };
				yield return new object[] { TestEnumNoFlags.Four, "FourKey" };
				yield return new object[] { TestEnumNoFlags.Five, "Five" };
				yield return new object[] { TestEnumNoFlags.Six, "Six" };
				yield return new object[] { TestEnumNoFlags.Seven, "SevenKey" };
				yield return new object[] { TestEnumNoFlags.Eight, "Eight" };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase003Data")]
		public void TestCase003_EnumExtensions_GetDisplayNameKey_WithTestEnumNoFlagsTheory(TestEnumNoFlags value, string expectedResult)
		{
			value.GetDisplayNameKey().Should().Be(expectedResult);
		}

		#region TestCase004Data
		public static IEnumerable<object[]> TestCase004Data
		{
			get
			{
				yield return new object[] { (TestEnumWithFlags)0x00, "Zero" };
				yield return new object[] { (TestEnumWithFlags)0x01, "One" };
				yield return new object[] { (TestEnumWithFlags)0x02, "TwoKey" };
				yield return new object[] { (TestEnumWithFlags)0x04, "Three" };
				yield return new object[] { (TestEnumWithFlags)0x08, "FourKey" };
				yield return new object[] { (TestEnumWithFlags)0x10, "Five" };
				yield return new object[] { (TestEnumWithFlags)0x20, "Six" };
				yield return new object[] { (TestEnumWithFlags)0x40, "SevenKey" };
				yield return new object[] { (TestEnumWithFlags)0x80, "Eight" };
				yield return new object[] { (TestEnumWithFlags)0x0F, "One, TwoKey, Three, FourKey" };
				yield return new object[] { (TestEnumWithFlags)0xF0, "Five, Six, SevenKey, Eight" };
				yield return new object[] { (TestEnumWithFlags)0xFF, "One, TwoKey, Three, FourKey, Five, Six, SevenKey, Eight" };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase004Data")]
		public void TestCase004_EnumExtensions_GetDisplayNameKey_WithTestEnumWithFlagsTheory(TestEnumWithFlags value, string expectedResult)
		{
			value.GetDisplayNameKey().Should().Be(expectedResult);
		}

		#endregion

		#region EnumParse test methods

		#region TestCase005Data
		public static IEnumerable<object[]> TestCase005Data
		{
			get
			{
				yield return new object[] { "Zero",  TestEnumNoFlags.Zero  };
				yield return new object[] { "One",   TestEnumNoFlags.One   };
				yield return new object[] { "Two",   TestEnumNoFlags.Two   };
				yield return new object[] { "Three", TestEnumNoFlags.Three };
				yield return new object[] { "Four",	 TestEnumNoFlags.Four  };
				yield return new object[] { "Five",  TestEnumNoFlags.Five  };
				yield return new object[] { "Six",   TestEnumNoFlags.Six   };
				yield return new object[] { "Seven", TestEnumNoFlags.Seven };
				yield return new object[] { "Eight", TestEnumNoFlags.Eight };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase005Data")]
		public void TestCase005_EnumExtensions_EnumParse_WithTestEnumNoFlagsTheory(string value, TestEnumNoFlags expectedResult)
		{
			value.EnumParse<TestEnumNoFlags>().Should().Be(expectedResult);
		}

		#region TestCase006Data
		public static IEnumerable<object[]> TestCase006Data
		{
			get
			{
				yield return new object[] { "ZERO", TestEnumNoFlags.Zero };
				yield return new object[] { "ONE", TestEnumNoFlags.One };
				yield return new object[] { "TWO", TestEnumNoFlags.Two };
				yield return new object[] { "THREE", TestEnumNoFlags.Three };
				yield return new object[] { "FOUR", TestEnumNoFlags.Four };
				yield return new object[] { "five", TestEnumNoFlags.Five };
				yield return new object[] { "six", TestEnumNoFlags.Six };
				yield return new object[] { "sEvEn", TestEnumNoFlags.Seven };
				yield return new object[] { "EiGhT", TestEnumNoFlags.Eight };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase006Data")]
		public void TestCase006_EnumExtensions_EnumParse_WithTestEnumNoFlagsIgnoreCaseTheory(string value, TestEnumNoFlags expectedResult)
		{
			value.EnumParse<TestEnumNoFlags>(true).Should().Be(expectedResult);
		}

		#region TestCase007Data
		public static IEnumerable<object[]> TestCase007Data
		{
			get
			{
				yield return new object[] { "Zero", TestEnumNoFlags.Zero };
				yield return new object[] { "One", TestEnumNoFlags.One };
				yield return new object[] { "Two", TestEnumNoFlags.Two };
				yield return new object[] { "Three", TestEnumNoFlags.Three };
				yield return new object[] { "Four", TestEnumNoFlags.Four };
				yield return new object[] { "Five", TestEnumNoFlags.Five };
				yield return new object[] { "Six", TestEnumNoFlags.Six };
				yield return new object[] { "Seven", TestEnumNoFlags.Seven };
				yield return new object[] { "Eight", TestEnumNoFlags.Eight };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase007Data")]
		public void TestCase007_EnumExtensions_EnumParse_WithTestEnumNoFlagsCaseSensitiveTheory(string value, TestEnumNoFlags expectedResult)
		{
			value.EnumParse<TestEnumNoFlags>(false).Should().Be(expectedResult);
		}

		[Fact]
		public void TestCase008_EnumExtensions_EnumParse_WithTestEnumNoFlags_ReturnsNull_IfArgumentIsNotDefined()
		{
			string value = "NotDefined";
			TestEnumNoFlags? result = value.EnumParse<TestEnumNoFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
			

			result = value.EnumParse<TestEnumNoFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
			
			value = "";
			result = value.EnumParse<TestEnumNoFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParse<TestEnumNoFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
		}

		[Fact]
		public void TestCase009_EnumExtensions_EnumParse_WithTestEnumWithFlags_ReturnsNull_IfArgumentIsNotDefined()
		{
			string value = "NotDefined";
			TestEnumWithFlags? result = value.EnumParse<TestEnumWithFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParse<TestEnumWithFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			value = "";
			result = value.EnumParse<TestEnumWithFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParse<TestEnumWithFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
		}

		[Fact]
		public void TestCase010_EnumExtensions_EnumParse_WithNoEnumType_ThrowsArgException()
		{
			const string value = "One";
			Action act = () => value.EnumParse<TestStruct>(false);
			act.ShouldThrow<ArgException<string>>();

			act = () => value.EnumParse<TestStruct>(true);
			act.ShouldThrow<ArgException<string>>();

			//value.Invoking(v => v.EnumParse<TestStruct>(false)).ShouldThrow<ArgException<string>>();
		}

		#region TestCase011Data
		public static IEnumerable<object[]> TestCase011Data
		{
			get
			{
				yield return new object[] { "Zero",  (TestEnumWithFlags)0x00 };
				yield return new object[] { "One",   (TestEnumWithFlags)0x01 };
				yield return new object[] { "Two",   (TestEnumWithFlags)0x02 };
				yield return new object[] { "Three", (TestEnumWithFlags)0x04 };
				yield return new object[] { "Four",  (TestEnumWithFlags)0x08 };
				yield return new object[] { "Five",  (TestEnumWithFlags)0x10 };
				yield return new object[] { "Six",   (TestEnumWithFlags)0x20 };
				yield return new object[] { "Seven", (TestEnumWithFlags)0x40 };
				yield return new object[] { "Eight", (TestEnumWithFlags)0x80 };
				yield return new object[] { "One, Two, Three, Four",   (TestEnumWithFlags)0x0F };
				yield return new object[] { "Five, Six, Seven, Eight", (TestEnumWithFlags)0xF0 };
				yield return new object[] { "One, Two, Three, Four, Five, Six, Seven, Eight", (TestEnumWithFlags)0xFF };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase011Data")]
		public void TestCase011_EnumExtensions_EnumParse_WithTestEnumWithFlagsTheory(string value, TestEnumWithFlags expectedResult)
		{
			value.EnumParse<TestEnumWithFlags>().Should().Be(expectedResult);
		}

		#endregion

		#region EnumParseDisplayName test methods

		#region TestCase012Data
		public static IEnumerable<object[]> TestCase012Data
		{
			get
			{
				yield return new object[] { "Zero", TestEnumNoFlags.Zero };
				yield return new object[] { "OneName", TestEnumNoFlags.One };
				yield return new object[] { "TwoName", TestEnumNoFlags.Two };
				yield return new object[] { "ThreeName", TestEnumNoFlags.Three };
				yield return new object[] { "Four", TestEnumNoFlags.Four };
				yield return new object[] { "Five", TestEnumNoFlags.Five };
				yield return new object[] { "SixName", TestEnumNoFlags.Six };
				yield return new object[] { "Seven", TestEnumNoFlags.Seven };
				yield return new object[] { "Eight", TestEnumNoFlags.Eight };
			}
		}
		#endregion

		[Theory, PropertyData("TestCase012Data")]
		public void TestCase012_EnumExtensions_EnumParseDisplayName_WithTestEnumNoFlagsTheory(string value, TestEnumNoFlags expectedResult)
		{
			value.EnumParseDisplayName<TestEnumNoFlags>(false).Should().Be(expectedResult);
		}

		[Fact]
		public void TestCase013_EnumExtensions_EnumParseDisplayName_WithTestEnumNoFlags_ReturnsNull_IfArgumentIsNotDefined()
		{
			string value = "NotDefined";
			TestEnumNoFlags? result = value.EnumParseDisplayName<TestEnumNoFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParseDisplayName<TestEnumNoFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			value = "";
			result = value.EnumParseDisplayName<TestEnumNoFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParseDisplayName<TestEnumNoFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
		}

		[Fact]
		public void TestCase014_EnumExtensions_EnumParseDisplayName_WithTestEnumWithFlags_ReturnsNull_IfArgumentIsNotDefined()
		{
			string value = "NotDefined";
			TestEnumWithFlags? result = value.EnumParseDisplayName<TestEnumWithFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParseDisplayName<TestEnumWithFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			value = "";
			result = value.EnumParseDisplayName<TestEnumWithFlags>(false);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();

			result = value.EnumParseDisplayName<TestEnumWithFlags>(true);
			//result.Should().Not.Have.Value();
			(result == null).Should().BeTrue();
		}

		[Fact]
		public void TestCase015_EnumExtensions_EnumParseDisplayName_WithNoEnumType_ThrowsArgException()
		{
			const string value = "Zero";
			Action act = () => value.EnumParseDisplayName<TestStruct>(false);
			act.ShouldThrow<ArgException<string>>();
			act = () => value.EnumParseDisplayName<TestStruct>(true);
			act.ShouldThrow<ArgException<string>>();
		}
		#endregion

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
