//--------------------------------------------------------------------------
// File:    Example1CoreMinError.cs
// Content:	Implementation of class Example1CoreMinError
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SmartExpert.CUI;
using SmartExpert.IO;
using SmartExpert.Linq;
using System.IO;
using System.Collections;
using SmartExpert.Messaging;
using SmartExpert.Reflection;

#endregion

namespace SmartExpert.Examples
{
	///<summary>Examples CoreMin Core.ObjectEx</summary>
	public static class ExamplesCoreMinCoreObjectEx
	{
		[Description("Run all ObjectEx examples")]
		public static void RunAll()
		{
			FileHelper.GetFiles(@".\", "ExampleResult-CoreMin-Core-ObjectEx-*.txt").ForEach(FileHelper.DeleteFile);

			ExampleA1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleB1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleC1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleD1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleE1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleF1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			ExampleG1();
			ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			//ExampleH1();
			//ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			//ExampleI1();
			//ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
			//ExampleJ1();
			//ConsoleHelper.NewLine(); ConsoleHelper.HR(); ConsoleHelper.NewLine();
		}

		[Description("Example-1 ObjectEx.Is<T>")]
		public static void ExampleA1()
		{
			try
			{
				// CoreMin = Assembly Short Name
				// Core = Core last part
				// ObjectEx = The name of the type
				// Member[A..Z] = M of type ObjectEx
				// MemberType: M=Method; EXM=Extension Method; P=Property; T=Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-IsT.txt";

				#region Example_EXM_IsT

				object sampleText = "42";
				object sampleNumber = 42;
				object sampleFloat = 42.0f;
				object sampleDouble = 42.0;
				object sampleStream = new MemoryStream();

				ConsoleHelper.WriteNameValue("sampleText.Is<string>()  ", sampleText.Is<string>());	// = true;
				ConsoleHelper.WriteNameValue("sampleNumber.Is<int>()   ", sampleNumber.Is<int>());	// = true;
				ConsoleHelper.WriteNameValue("sampleFloat.Is<float>()  ", sampleFloat.Is<float>());	// = true;
				ConsoleHelper.WriteNameValue("sampleDouble.Is<double>()", sampleDouble.Is<double>());// = true;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("sampleStream.Is<IDisposable>()", sampleStream.Is<IDisposable>()); // = true;
				ConsoleHelper.WriteNameValue("sampleText.Is<object>() ", sampleText.Is<object>());			// = true;
				ConsoleHelper.WriteNameValue("sampleDouble.Is<ValueType>()", sampleDouble.Is<ValueType>());	// = true;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("sampleText.Is<ValueType>()", sampleText.Is<ValueType>());// = false;
				ConsoleHelper.WriteNameValue("sampleNumber.Is<long>() ", sampleNumber.Is<long>());	// = false;
				ConsoleHelper.WriteNameValue("sampleFloat.Is<double>()", sampleFloat.Is<double>());	// = false;
				ConsoleHelper.WriteNameValue("sampleDouble.Is<float>()", sampleDouble.Is<float>());	// = false;

				#endregion

				sampleStream.DisposeIfNecessary();

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.IsTypeOf")]
		public static void ExampleB1()
		{
			try
			{
				// MemberType: M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-IsTypeOf.txt";

				#region Example_EXM_IsTypeOf

				object sampleText = "42";
				object sampleNumber = 42;
				object sampleFloat = 42.0f;
				object sampleDouble = 42.0;
				object sampleStream = new MemoryStream();

				ConsoleHelper.WriteNameValue(
					"sampleText.IsTypeOf(typeof(string))  ", sampleText.IsTypeOf(typeof(string))); // = true;
				ConsoleHelper.WriteNameValue(
					"sampleNumber.IsTypeOf(typeof(int))   ", sampleNumber.IsTypeOf(typeof(int))); // = true;
				ConsoleHelper.WriteNameValue(
					"sampleFloat.IsTypeOf(typeof(float))  ", sampleFloat.IsTypeOf(typeof(float))); // = true;
				ConsoleHelper.WriteNameValue(
					"sampleDouble.IsTypeOf(typeof(double))", sampleDouble.IsTypeOf(typeof(double))); // = true;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue(
					"sampleStream.IsTypeOf(typeof(IDisposable))", sampleStream.IsTypeOf(typeof(IDisposable))); // = true;
				ConsoleHelper.WriteNameValue(
					"sampleText.IsTypeOf(typeof(object))", sampleText.IsTypeOf(typeof(object))); // = true;
				ConsoleHelper.WriteNameValue(
					"sampleDouble.IsTypeOf(typeof(ValueType))", sampleDouble.IsTypeOf(typeof(ValueType))); // = true;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue(
					"sampleText.IsTypeOf(typeof(ValueType))", sampleText.IsTypeOf(typeof(ValueType))); // = false;
				ConsoleHelper.WriteNameValue(
					"sampleNumber.IsTypeOf(typeof(long))", sampleNumber.IsTypeOf(typeof(long))); // = false;
				ConsoleHelper.WriteNameValue(
					"sampleFloat.IsTypeOf(typeof(double))", sampleFloat.IsTypeOf(typeof(double))); // = flase;
				ConsoleHelper.WriteNameValue(
					"sampleDouble.IsTypeOf(typeof(float))", sampleDouble.IsTypeOf(typeof(float))); // = false;

				#endregion

				sampleStream.DisposeIfNecessary();
			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.AsT")]
		public static void ExampleC1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-AsT.txt";

				#region Example_EXM_AsT

				object sampleText = "42";
				object sampleNumber = 42;
				object sampleFloat = 42.0f;
				object sampleDouble = 42.0;
				object sampleStream = new MemoryStream();

				ConsoleHelper.WriteNameValue("sampleText.As<string>()  ", sampleText.As<string>());	// = "42";
				ConsoleHelper.WriteNameValue("sampleText.As<int>()     ", sampleNumber.As<int>());	// = 42;
				ConsoleHelper.WriteNameValue("sampleFloat.As<float>()  ", sampleFloat.As<float>());	// = 42.0f;
				ConsoleHelper.WriteNameValue("sampleDouble.As<double>()", sampleDouble.As<double>());// = 42.0;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("sampleStream.As<IDisposable>()", sampleStream.As<IDisposable>()); // != null;
				ConsoleHelper.WriteNameValue("sampleText.As<object>() ", sampleText.As<object>());				// != null;
				ConsoleHelper.NewLine();

				ConsoleHelper.WriteNameValue("sampleNumber.As<string>()", sampleNumber.As<string>());	// = null;
				ConsoleHelper.WriteNameValue("sampleNumber.As<long>()  ", sampleNumber.As<long>());		// = 0;
				ConsoleHelper.WriteNameValue("sampleFloat.As<double>() ", sampleFloat.As<double>());	// = 0;
				ConsoleHelper.WriteNameValue("sampleDouble.As<float>() ", sampleDouble.As<float>());	// = 0;

				#endregion

				sampleStream.DisposeIfNecessary();

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.AsSequenceT1")]
		public static void ExampleD1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-AsSequenceT1.txt";

				#region Example_EXM_AsSequenceT1

				var objList = new List<object>() { "1", "2", "3", "4", "5" };

				IEnumerable<string> stringSequence = objList.AsSequence<object, string>();

				ConsoleHelper.WriteCollection("stringSequence", stringSequence, ",");
				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.AsSequenceT2")]
		public static void ExampleE1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-AsSequenceT2.txt";

				#region Example_EXM_AsSequenceT2

				IEnumerable objList = new List<object> { "1", "2", "3", "4", "5" };

				IEnumerable<string> stringSequence = objList.AsSequence<string>();

				ConsoleHelper.WriteCollection("stringSequence", stringSequence, ",");

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.CastSequenceT1T2")]
		public static void ExampleF1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-CastSequenceT1T2.txt";

				#region Example_EXM_CastSequenceT1T2

				var source1 = new List<int> { 1, 2, 3, 4, 5 };
				var source2 = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };
				var source3 = new List<string> { "1", "2", "3", "4", "5" };

				IEnumerable<long> target1 = source1.CastSequence<int, long>();
				ConsoleHelper.WriteCollection("target1", target1, ",");
				
				IEnumerable<string> target21 = source2.CastSequence<double, string>();
				ConsoleHelper.WriteCollection("target21", target21, ",");

				IEnumerable<float> target22 = source2.CastSequence<double, float>();
				ConsoleHelper.WriteCollection("target22", target22, ",");

				IEnumerable<long> target3 = source3.CastSequence<string, long>();
				ConsoleHelper.WriteCollection("target3", target3, ",");
				
				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.CastT1T2")]
		public static void ExampleG1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-CastT1T2.txt";

				#region Example_EXM_CastT1T2

				string sampleText = "42";
				int sampleNumber = 42;
				float sampleFloat = 42.0f;
				double sampleDouble = 42.0;
				object sampleStream = new MemoryStream();

				ConsoleHelper.WriteNameValue("sampleStream.Cast<Object, IDisposable>()", sampleStream.Cast<Object, IDisposable>()); // != null;
				ConsoleHelper.WriteNameValue("sampleText.Cast<string, int>()", sampleText.Cast<string, int>());			// = 42;
				ConsoleHelper.WriteNameValue("sampleNumber.Cast<int, string>()", sampleNumber.Cast<int, string>());		// = "42";
				ConsoleHelper.WriteNameValue("sampleNumber.Cast<int, long>()", sampleNumber.Cast<int, long>());			// = 42L;
				ConsoleHelper.WriteNameValue("sampleFloat.Cast<float, double>()", sampleFloat.Cast<float, double>());	// = 42.0d;
				ConsoleHelper.WriteNameValue("sampleDouble.Cast<double, float>()", sampleDouble.Cast<double, float>()); // = 42.0f;
				#endregion

				sampleStream.DisposeIfNecessary();

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.MemberNameH")]
		public static void ExampleH1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-MemberNameH.txt";

				#region Example_EXM_MemberNameH

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.MemberNameI")]
		public static void ExampleI1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-MemberNameI.txt";

				#region Example_EXM_MemberNameI

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}

		[Description("Example-1 ObjectEx.MemberNameJ")]
		public static void ExampleJ1()
		{
			try
			{
				// MemberType(EXM): M = Method; EXM = Extension Method; P = Property; T = Type
				ConsoleHelper.OutputFileName = "ExampleResult-CoreMin-Core-ObjectEx-MemberNameJ.txt";

				#region Example_EXM_MemberNameJ

				// Example code

				#endregion

			}
			finally
			{
				ConsoleHelper.OutputFileName = null;
			}
		}


	}

}
