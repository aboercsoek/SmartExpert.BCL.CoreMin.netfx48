using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;

namespace SmartExpert.Test.Repository
{
	internal enum MyEnum
	{
		[DisplayName("None Value", "KEY_NONE")]
		None,
		[DisplayName("First Value", "KEY_FIRST")]
		First,
		[DisplayName("Second Value", "KEY_SECOND")]
		Second,
	}

	[Flags]
	internal enum MyEnum2
	{
		[DisplayName("None Flag", "KEY_NONE")]
		None = 0,
		[DisplayName("First Flag", "KEY_FIRST")]
		First = 1,
		[DisplayName("Second Flag", "KEY_FIRST")]
		Second = 2,
	}

	[Serializable]
	public class TestClass : DependencyObject, IDisposable
	{
// ReSharper disable FieldCanBeMadeReadOnly.Local
		private List<int> m_GenericIntList = new List<int>();
// ReSharper restore FieldCanBeMadeReadOnly.Local
		[UsedImplicitly] private List<object> m_GenericObjectList = new List<object>();

#pragma warning disable 67
		public event EventHandler<EventArgs> PublicEvent;
#pragma warning restore 67

		public const int PublicConstant = 42;
		public readonly string ReadOnlyString;

		[UsedImplicitly] private static int m_PrivateStaticInt;
		internal static int InternalStaticIntField = 42;

		private double m_TestDouble;

		protected int ProtectedIntField;
		internal int InternalIntField;

		[UsedImplicitly] private MyEnum m_MyEnum;
		[UsedImplicitly] private MyEnum2 m_MyEnum2;

		public string MyProperty
		{
			get { return (string)GetValue(MyPropertyProperty); }
			set { SetValue(MyPropertyProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MyPropertyProperty =
			DependencyProperty.Register("MyProperty", typeof(string), typeof(TestClass), null);


		static TestClass()
		{
			m_PrivateStaticInt = 42;
		}

		public TestClass()
		{
			m_PrivateStaticInt++;
			ReadOnlyString = "Test of readonly string";
			TestString = string.Empty;
			m_TestDouble = 3.1415926;
			m_MyEnum = MyEnum.First;
			m_MyEnum2 = MyEnum2.First | MyEnum2.Second;
			InternalIntField = 23;
		}

		public void SetTestDouble(double testDouble)
		{
			m_TestDouble = testDouble;
		}

		public string ConvertTestDouble()
		{
			return m_TestDouble.ToString(CultureInfo.InvariantCulture);
		}

		internal int MyEnumValue
		{
			get { return (int)m_MyEnum; }
		}

		internal int MyEnum2Value
		{
			get { return (int)m_MyEnum2; }
		}

		public string TestString { get; set; }

		//private string m_TestString;
		/*public string TestString
		{
			get { return m_TestString; }
			set
			{
				if (value != null)
				{
					m_TestString = value;
				}
			}
		}*/

		internal void MethodInternal(int value)
		{
			ProtectedIntField = value;
		}

		protected void ThrowException()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		[UsedImplicitly]
		private Stream GetStream()
		{
			Stream stream = new FileStream("C:\test.txt", FileMode.Open);
			return stream;
		}

		#region IDisposable Members

		public void Dispose()
		{
			m_GenericIntList.Clear();
		}

		#endregion
	}
}
