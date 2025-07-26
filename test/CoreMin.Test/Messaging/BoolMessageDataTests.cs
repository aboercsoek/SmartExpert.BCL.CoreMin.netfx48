//<file>
//	<name>BoolMessageDataTests.cs</file>
//	<content>Implementation of class BoolMessageDataTests</content>
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

#endregion

namespace SmartExpert.Test.Messaging
{
	///<summary>BoolMessageData Tests</summary>
	public class BoolMessageDataTests : IDisposable
	{
		
		#region Ctor

		#endregion

		#region Test Methods
		// ReSharper disable InconsistentNaming

		[Fact]
		public void TestCase001_BoolMessageData_Data_IsNotNullAfterNew()
		{
			var bmr = new BoolMessageData(true, "Test message");
			bmr.Data.Should().Not.Be.Null();
		}

		[Fact]
		public void TestCase002_BoolMessageData_Data_ContainsOneItemAfterNew()
		{
			var bmr = new BoolMessageData(true, "Test message");
			bmr.Data.Should().Not.Be.Null();
			bmr.Data.Count().Should().Be.EqualTo(1);
		}

		[Fact]
		public void TestCase003_BoolMessageData_Data_ContainItemNullObject()
		{
			var bmr = new BoolMessageData(true, "Test message");
			bmr.Data.Should().Not.Be.Null();
			bmr.Data.GetPropertyValue("$(item)").Should().Be.Null();
			bmr.Item.Should().Be.Null();
		}

		[Fact]
		public void TestCase004_BoolMessageData_Data_ContainItemNotNullObject()
		{
			var bmr = new BoolMessageData(true, "Test message", new Object());
			bmr.Data.Should().Not.Be.Null();
			bmr.Data.GetPropertyValue("$(item)").Should().Not.Be.Null();
			bmr.Item.Should().Not.Be.Null();
		}

		[Fact]
		public void TestCase005_BoolMessageData_Success_ShouldBeEqualToConstructorParameter()
		{
			var bmr = new BoolMessageData(true, "Test message");
			bmr.Success.Should().Be.True();

			bmr = new BoolMessageData(false, "Test message");
			bmr.Success.Should().Be.False();

			BoolMessageData.True.Success.Should().Be.True();
			BoolMessageData.False.Success.Should().Be.False();
		}

		[Fact]
		public void TestCase006_BoolMessageData_Message_ShouldBeEqualToConstructorParameter()
		{
			var bmr = new BoolMessageData(true, "Test message");
			bmr.Message.Should().Be.EqualTo("Test message");

			bmr = new BoolMessageData(false, "Test message");
			bmr.Message.Should().Be.EqualTo("Test message");

			bmr = new BoolMessageData(true, string.Empty);
			bmr.Message.Should().Be.EqualTo(string.Empty);

			bmr = new BoolMessageData(false, string.Empty);
			bmr.Message.Should().Be.EqualTo(string.Empty);

			bmr = new BoolMessageData(true, null);
			bmr.Message.Should().Be.Null();

			bmr = new BoolMessageData(false, null);
			bmr.Message.Should().Be.Null();

			BoolMessageData.True.Message.Should().Be.EqualTo(string.Empty);
			BoolMessageData.False.Message.Should().Be.EqualTo(string.Empty);
		}

		[Fact]
		public void TestCase007_BoolMessageData_DataAndItem_IsEqualWithDataItemKeyValue()
		{
			var bmr = new BoolMessageData(true, "Test message", "Item value");
			
			((string)bmr.Data.GetPropertyValue("$(item)")).Should().Be.EqualTo((string)bmr.Item);
			((string) bmr.Item).Should().Be.EqualTo("Item value");

			bmr.Item = "New item value";
			((string)bmr.Data.GetPropertyValue("$(item)")).Should().Be.EqualTo((string)bmr.Item);
			((string)bmr.Item).Should().Be.EqualTo("New item value");

			bmr.Data.SetProperty("$(item)", "New item value 2");
			((string)bmr.Data.GetPropertyValue("$(item)")).Should().Be.EqualTo((string)bmr.Item);
			((string)bmr.Item).Should().Be.EqualTo("New item value 2");
		}

		[Fact]
		public void TestCase008_BoolMessageData_DataAndItem_ItemIsNullAfterClearProperties()
		{
			var bmr = new BoolMessageData(true, "Test message", "Item value");
			
			bmr.Data.TryGetPropertyValue("$(item)").Should().Not.Be.Null();
			bmr.Item.Should().Not.Be.Null();

			bmr.Data.ClearContext();
			bmr.Data.TryGetPropertyValue("$(item)").Should().Be.Null();
			bmr.Item.Should().Be.Null();

		}

		[Fact]
		public void TestCase009_BoolMessageData_DataAndItem_ItemIsNullAfterRemoveItemKeyProperty()
		{
			var bmr = new BoolMessageData(true, "Test message", "Item value");

			bmr.Data.TryGetPropertyValue("$(item)").Should().Not.Be.Null();
			bmr.Item.Should().Not.Be.Null();

			bmr.Data.RemoveProperty("$(item)");
			bmr.Data.TryGetPropertyValue("$(item)").Should().Be.Null();
			bmr.Item.Should().Be.Null();

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
