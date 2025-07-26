//--------------------------------------------------------------------------
// File:    ObjectExTests.cs
// Content:	Implementation of class ObjectExTests
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpTestsEx;
using Xunit;
using Xunit.Extensions;

using SmartExpert;
#endregion


namespace SmartExpert.Test.Core
{
	///<summary>ObjectEx Tests</summary>
	public class ObjectExTests : IDisposable
	{
		#region Ctor

		public ObjectExTests()
		{

		}

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		#region IsT test methods

		#region TestCase001Data
		public static IEnumerable<object[]> TestCase001Data
		{
			get
			{
				yield return new object[] { new object(), true };
				yield return new object[] { "Teststring", true };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase001Data")]
		public void TestCase001_ObjectEx_IsT_WithOjectTypeTheory(object value, bool expectedResult)
		{
			value.Is<Object>().Should().Be(expectedResult);
		}


		#region TestCase002Data
		public static IEnumerable<object[]> TestCase002Data
		{
			get {
				yield return new object[] { string.Empty, true };
				yield return new object[] { "Teststring", true };
				yield return new object[] { new object(), false };
				yield return new object[] { 0, false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase002Data")]
		public void TestCase002_ObjectEx_IsT_WithStringTypeTheory(object value, bool expectedResult)
		{
			value.Is<string>().Should().Be(expectedResult);
		}


		#region TestCase003Data
		public static IEnumerable<object[]> TestCase003Data
		{
			get
			{
				yield return new object[] { true, true };
				yield return new object[] { 0, true };
				yield return new object[] { 42.0, true };
				yield return new object[] { 1f, true };
				yield return new object[] { 1d, true };
				yield return new object[] { Guid.NewGuid(), true };
				yield return new object[] { new object(), false };
				yield return new object[] { "Teststring", false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase003Data")]
		public void TestCase003_ObjectEx_IsT_WithValueTypeTheory(object value, bool expectedResult)
		{
			value.Is<ValueType>().Should().Be(expectedResult);
		}


		#region TestCase004Data
		public static IEnumerable<object[]> TestCase004Data
		{
			get
			{
				yield return new object[] { new Repository.TestClass(), true };
				yield return new object[] { new object(), false };
				yield return new object[] { "Teststring", false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory] //[AssumeIdentity("Administrators")]
		[PropertyData("TestCase004Data")]
		public void TestCase004_ObjectEx_IsT_WithInterfaceTypeTheory(object value, bool expectedResult)
		{
			value.Is<IDisposable>().Should().Be(expectedResult);
			//Console.WriteLine("Username of Thread:" + Thread.CurrentPrincipal.Identity.Name);
			//Thread.CurrentPrincipal.IsInRole("Administrators").Should().Be(true);
		}

		#endregion

		#region AsT test methods

		#region TestCase005Data
		public static IEnumerable<object[]> TestCase005Data
		{
			get
			{
				yield return new object[] { new object(), true };
				yield return new object[] { "Teststring", true };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase005Data")]
		public void TestCase005_ObjectEx_AsT_WithOjectTypeTheory(object value, bool resultNotNull)
		{
			if (resultNotNull)
				value.As<Object>().Should().Not.Be.Null();
			else
				value.As<Object>().Should().Be.Null();
		}


		#region TestCase006Data
		public static IEnumerable<object[]> TestCase006Data
		{
			get
			{
				yield return new object[] { string.Empty, true };
				yield return new object[] { "Teststring", true };
				yield return new object[] { new object(), false };
				yield return new object[] { 0, false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase006Data")]
		public void TestCase006_ObjectEx_AsT_WithStringTypeTheory(object value, bool resultNotNull)
		{
			if (resultNotNull)
				value.As<string>().Should().Not.Be.Null();
			else
				value.As<string>().Should().Be.Null();
		}


		#region TestCase007Data
		public static IEnumerable<object[]> TestCase007Data
		{
			get
			{
				yield return new object[] { true, true };
				yield return new object[] { 0, true };
				yield return new object[] { 42.0, true };
				yield return new object[] { 1f, true };
				yield return new object[] { 1d, true };
				yield return new object[] { Guid.NewGuid(), true };
				yield return new object[] { new object(), false };
				yield return new object[] { "Teststring", false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase007Data")]
		public void TestCase007_ObjectEx_AsT_WithValueTypeTheory(object value, bool resultNotNull)
		{
			if (resultNotNull)
				value.As<ValueType>().Should().Not.Be.Null();
			else
				value.As<ValueType>().Should().Be.Null();
		}


		#region TestCase008Data
		public static IEnumerable<object[]> TestCase008Data
		{
			get
			{
				yield return new object[] { new Repository.TestClass(), true };
				yield return new object[] { new object(), false };
				yield return new object[] { "Teststring", false };
				yield return new object[] { null, false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase008Data")]
		public void TestCase008_ObjectEx_AsT_WithInterfaceTypeTheory(object value, bool resultNotNull)
		{
			if (resultNotNull)
				value.As<IDisposable>().Should().Not.Be.Null();
			else
				value.As<IDisposable>().Should().Be.Null();
		}

		#endregion

		#region IsTypeOf test methods

		#region TestCase009Data
		public static IEnumerable<object[]> TestCase009Data
		{
			get
			{
				yield return new object[] { new object(), typeof(Object), true };
				yield return new object[] { "Teststring", typeof(Object), true };
				yield return new object[] { null,		  typeof(Object), false };

				yield return new object[] { string.Empty, typeof(String), true };
				yield return new object[] { "Teststring", typeof(String), true };
				yield return new object[] { new object(), typeof(String), false };
				yield return new object[] { 0,			  typeof(String), false };

				yield return new object[] { true,			typeof(Boolean), true };
				yield return new object[] { 0,				typeof(Int32), true };
				yield return new object[] { 42.0,			typeof(Double), true };
				yield return new object[] { 1f,				typeof(Single), true };
				yield return new object[] { (Decimal)1,		typeof(Decimal), true };
				yield return new object[] { Guid.NewGuid(), typeof(Guid), true };
				yield return new object[] { new object(),	typeof(Int32), false };
				yield return new object[] { "Teststring",	typeof(Int32), false };

				yield return new object[] { new Repository.TestClass(), typeof(IDisposable), true };
				yield return new object[] { new object(),	typeof(IDisposable), false };
				yield return new object[] { "Teststring",	typeof(IDisposable), false };
			}
		}
		#endregion

		[Theory]
		[PropertyData("TestCase009Data")]
		public void TestCase009_ObjectEx_IsTypeOf_Theory(object value, Type type, bool expectedResult)
		{
			value.IsTypeOf(type).Should().Be(expectedResult);
		}

		#endregion

		#region AsSequence test methods

		[Fact]
		public void TestCase010_ObjectEx_AsSequenceT_FromObjectSeqToStringSeq()
		{
			var objList = new List<object> {"1", "2", "3", "4", "5"};

			IEnumerable<string> stringSequence = objList.AsSequence<object, string>();

			int targetCount = 0;
			foreach (var item in stringSequence)
			{
				targetCount++;
				item.Should().Be(targetCount.ToInvariantString());
			}

			targetCount.Should().Be(objList.Count);
		}

		[Fact]
		public void TestCase011_ObjectEx_AsSequenceT_FromObjectSeqToDoubleSeq()
		{
			var source = new List<object> { 1d, 2d, 3d, 4d, 5d };

			IEnumerable<double> target = source.AsSequence<object, double>();

			int targetCount = 0;
			foreach (var item in target)
			{
				targetCount++;
				item.Should().Be((double)targetCount);
			}

			targetCount.Should().Be(source.Count);
		}

		#endregion

		#region CastSequence test methods

		[Fact]
		public void TestCase012_ObjectEx_CastSequenceT_FromIntSeqToLongSeq()
		{
			var source = new List<int> { 1, 2, 3, 4, 5 };

			IEnumerable<long> target = source.CastSequence<int, long>();

			int targetCount = 0;
			foreach (var item in target)
			{
				targetCount++;
				item.Should().Be((long)targetCount);
			}

			targetCount.Should().Be(source.Count);
		}

		[Fact]
		public void TestCase013_ObjectEx_CastSequenceT_FromIntSeqToDoubleSeq()
		{
			var source = new List<int> { 1, 2, 3, 4, 5 };

			IEnumerable<double> target = source.CastSequence<int, double>();

			int targetCount = 0;
			foreach (var item in target)
			{
				targetCount++;
				item.Should().Be((double)targetCount);
			}

			targetCount.Should().Be(source.Count);
		}

		[Fact]
		public void TestCase014_ObjectEx_CastSequenceT_FromStringSeqToDoubleSeq()
		{
			var source = new List<string> { "1", "2", "3", "4", "5" };

			IEnumerable<double> target = source.CastSequence<string, double>();

			int targetCount = 0;
			foreach (var item in target)
			{
				targetCount++;
				item.Should().Be((double)targetCount);
			}

			targetCount.Should().Be(source.Count);

			#region TypeConverter Eval
			//Color value = Colors.Green;
			//TypeConverter converter = TypeDescriptor.GetConverter(value);
			//bool canConvertFromString = converter.CanConvertFrom(typeof (string));
			//bool canConvertToString = converter.CanConvertTo(typeof(string));

			//string colorString = converter.ConvertToString(null, CultureInfo.CurrentCulture, value);
			//Color newColor = (Color)converter.ConvertFromString(null, CultureInfo.CurrentCulture, colorString);

			//MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
			//if (method != null)
			//{
			//    Type type = typeof (XElement);
			//    var instanceDescriptor = new InstanceDescriptor(method, new object[] { type.AssemblyQualifiedName });
			//    object result = instanceDescriptor.Invoke();
			//}

			//var xmlSource = new XElement("ElementName", "ElementValue");
			//string sValue = xmlSource.ToInvariantString();
			//var xmlTarget = sValue.FromInvariantString<XElement>();

			#endregion

		}

		#endregion

		#region Cast<T1,T2> test methods

		[Fact]
		public void TestCase015_ObjectEx_CastT1T2_FromAndToTypes()
		{
			string sampleText = "42";
			int sampleNumber = 42;
			float sampleFloat = 42.0f;
			double sampleDouble = 42.0;
			object sampleStream = new MemoryStream();
			string dateString = "2012-01-01";

			sampleStream.Cast<Object, IDisposable>().Should().Not.Be.Null();
			sampleText.Cast<string, int>().Should().Be(42);
			sampleNumber.Cast<int, string>().Should().Be("42");
			sampleNumber.Cast<int, long>().Should().Be(42L);
			sampleFloat.Cast<float, double>().Should().Be(42.0);
			sampleDouble.Cast<double, float>().Should().Be(42.0f);
			dateString.Cast<string, DateTime>().Should().Be(new DateTime(2012, 01, 01));

			sampleStream.DisposeIfNecessary();
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
