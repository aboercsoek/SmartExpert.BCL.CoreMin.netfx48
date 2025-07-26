//--------------------------------------------------------------------------
// File:    _TypeName_Tests.cs
// Content:	Unit test class to test _TypeName_
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2013 Andreas Börcsök
//--------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using SmartExpert;
using SmartExpert.Error;
//using SmartExpert.Test.Repository;
using Xunit;
using Xunit.Extensions;
using FluentAssertions;

// :::::::::::::: Replace _TypeName_ with real type name ::::::::::::::

namespace SmartExpert.Test.SubNamespace
{
	///<summary>_TypeName_ Unit tests</summary>
	public class _TypeName_Tests
	{
		#region ctor

		//public _TypeName_Tests()
		//{
        // 	// Init unit test resources here
		//}

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		#region {MethodName1} test cases

		//public static IEnumerable<object[]> TheoryDataName
		//{
		//	get
		//	{
		//		yield return new object[] { null, false };
		//		yield return new object[] { string.Empty, true };
		//	}
		//}
        //
		//[Theory, PropertyData("TheoryDataName")]
		//public void TestCase000__TypeName__{MethodName1}_With{TestData}Theory(string valueToTest, bool expectedResult)
		//{
		//	valueToTest.{MethodName1}().Should().Be(expectedResult);
		//}

		// ::: More MethodName1 test cases ...
		
		#endregion

		#region {MethodName2} test cases

		//[Fact]
		//public void TestCase000__TypeName__{MethodName2}_With{TestValue}_Throws{ExceptionName}_If{Condition}()
		//{
		//	_TypeName_ valueToTest = new _TypeName_();
		//	Action testAction = () => valueToTest.{MethodName2}(null);
		//	testAction.ShouldThrow<ArgNullException>();
		//	
		//	//valueToTest.Invoking(v => v.{MethodName2}(null)).ShouldThrow<ArgNullException>();
		//}
		
		// ::: More MethodName2 test cases ...
		
		#endregion
		
		#region {MethodName3} test cases
		
		//[Fact]
		//public void TestCase000__TypeName__{MethodName3}_With{TestValue}_Returns{Wath}_If{TestCondition}()
		//{
		//	_TypeName_ valueToTest = new _TypeName_();
		//	valueToTest.{MethodName3}().Should().BeTrue();
		//}
		
		// ::: More MethodName3 test cases ...
		
		#endregion

		#region Sample test cases

		public static IEnumerable<object[]> TheoryDataProperty
        {
            get { yield return new object[] { 42 }; }
        }
		
		[Theory(Skip = "Skip sample test method")]
		//[Theory]
		[PropertyData("TheoryDataProperty")]
		public void TestCase000__TypeName__TestViaProperty(int x) 
		{ 
			x.Should().Be(42);
		}
		
		// ---
		
		public static IEnumerable<object[]> GenericData
        {
            get
            {
                yield return new object[] { 42 };
                yield return new object[] { "Hello, world!" };
                yield return new object[] { new int[] { 1, 2, 3 } };
                yield return new object[] { new List<string> { "a", "b", "c" } };
            }
        }
		
		[Theory(Skip = "Skip sample test method")]
		//[Theory]
		[PropertyData("GenericData")]
		public void TestCase000__TypeName__GenericTest<T>(T value) 
		{
			value.Should().BeOfType<T>();
		}
		
		// ---

		[Theory(Skip = "Skip sample test method")]
		//[Theory]
		[InlineData(42)]
		[InlineData(42L)]
		[InlineData(21.12)]
		[InlineData("Hello world")]
		public void TestCase000__TypeName__OneGenericParameter<T>(T value)
		{
			value.Should().BeOfType<T>();
		}

		// ---

		[Theory(Skip = "Skip sample test method")]
		//[Theory]
		[InlineData(42, 2112)]
		public void TestCase000__TypeName__TwoCompatibleGenericParametersOfOneType<T>(T value1, T value2)
		{
			value1.Should().Be(42);
			value2.Should().Be(2112);
			value1.Should().BeOfType<int>();
		}

		// ---

		[Theory(Skip = "Skip sample test method")]
		//[Theory]
		[InlineData(42, "Hello world")]
		public void TestCase000__TypeName__TwoGenericParametersOfTwoTypes<T1, T2>(T1 value1, T2 value2)
		{
			value1.Should().Be(42);
			value2.Should().Be("Hello world");
			value1.Should().BeOfType<int>();
			value2.Should().BeOfType<string>();
		}
		
		#endregion
		
		
		// ReSharper restore InconsistentNaming
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			// Release resources for unit test here
		}

		#endregion

	}
}
