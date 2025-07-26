using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpTestsEx;
using SmartExpert.Collections;
using SmartExpert.Error;
using Xunit;

namespace SmartExpert.Test.Collections
{
		///<summary>PropertyBag Tests</summary>
	public class PropertyBagTests : IDisposable
	{
		#region Ctor

		public PropertyBagTests()
		{

		}

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		[Fact]
		public void TestCase001_PropertyBag_IsNotNullAfterNew()
		{
			var propBag = new PropertyBag();
			propBag.Should().Not.Be.Null();
		}

		[Fact]
		public void TestCase002_PropertyBag_IsAssignableTo_IPropertyBag()
		{
			object propBag = new PropertyBag();
			propBag.Should().Be.AssignableTo<IPropertyBag>();
		}

		[Fact]
		public void TestCase003_PropertyBag_ContainsOneItemAfterSetProperty()
		{
			var propBag = new PropertyBag();
			propBag.Count().Should().Be.EqualTo(0);
			propBag.SetProperty("Data");
			propBag.Count().Should().Be.EqualTo(1);
		}

		[Fact]
		public void TestCase004_PropertyBag_ContainNullValue_AfterSetProperty_UsingNullValue()
		{
			var propBag = new PropertyBag();
			
			propBag.SetProperty("Key", null);
			
			propBag.GetPropertyValue("Key").Should().Be.Null();
		}

		[Fact]
		public void TestCase005_PropertyBag_ContainNotNullValue_AfterSetProperty_UsingNotNullValue()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key", "Data");

			propBag.GetPropertyValue("Key").Should().Not.Be.Null();
		}

		[Fact]
		public void TestCase006_PropertyBag_ContainsOneItem_AfterCalling_SetProperty_WithSameKeyTwice()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key", null);
			propBag.GetPropertyValue("Key").Should().Be.Null();
			propBag.Count().Should().Be.EqualTo(1);

			propBag.SetProperty("Key", "Data");
			propBag.Count().Should().Be.EqualTo(1);
			propBag.GetPropertyValue("Key").Should().Not.Be.Null();
			propBag.GetPropertyValue("Key").Should().Be("Data");
		}

		[Fact]
		public void TestCase007_PropertyBag_SetProperty_ThrowArgNullException_IfKeyIsNull()
		{
			var propBag = new PropertyBag();
			Executing.This(() => propBag.SetProperty(null, "Data")).Should().Throw<ArgNullException>();
		}

		[Fact]
		public void TestCase008_PropertyBag_SetProperty_ThrowArgEmptyException_IfKeyIsEmptyString()
		{
			var propBag = new PropertyBag();
			Executing.This(() => propBag.SetProperty(string.Empty, "Data")).Should().Throw<ArgEmptyException>();
		}

		[Fact]
		public void TestCase009_PropertyBag_RemoveProperty_RemovesTheProperty_FromTheBag()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key", "Data");
			propBag.Count().Should().Be.EqualTo(1);
			
			propBag.RemoveProperty("Key");
			propBag.Count().Should().Be.EqualTo(0);
		}

		[Fact]
		public void TestCase010_PropertyBag_ContainsProperty_RetrunsTrue_IfKeyIsInBag()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key", "Data");
			propBag.ContainsProperty("Key").Should().Be.True();
		}

		[Fact]
		public void TestCase011_PropertyBag_ContainsProperty_RetrunsFalse_IfKeyIsNotInBag()
		{
			var propBag = new PropertyBag();
			propBag.ContainsProperty("Key").Should().Be.False();

			propBag.SetProperty("Key", "Data");
			propBag.ContainsProperty("Key").Should().Be.True();

			propBag.RemoveProperty("Key");
			propBag.ContainsProperty("Key").Should().Be.False();
		}

		[Fact]
		public void TestCase012_PropertyBag_TryGetProperty_NotThrowsException()
		{
			var propBag = new PropertyBag();

			Executing.This(() => propBag.TryGetPropertyValue("Key")).Should().NotThrow();
			propBag.SetProperty("Key", "Data");
			Executing.This(() => propBag.TryGetPropertyValue("Key")).Should().NotThrow();
		}

		[Fact]
		public void TestCase013_PropertyBag_ClearContext_RemovesAllProperties()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key1", "Data1");
			propBag.SetProperty("Key2", "Data2");
			propBag.Count.Should().Be.EqualTo(2);

			propBag.ClearContext();
			propBag.Count.Should().Be.EqualTo(0);
		}

		[Fact]
		public void TestCase014_PropertyBag_GetEnumerator_LoopingThroughIEnumerable()
		{
			var propBag = new PropertyBag();

			propBag.SetProperty("Key1", "Data1");
			propBag.SetProperty("Key2", "Data2");
			
			int loopCount = 1;
			foreach(var property in propBag)
			{
				property.Key.Should().Be.EqualTo("Key" + loopCount);
				property.Value.As<string>().Should().Be.EqualTo("Data" + loopCount);
				loopCount++;
			}
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
