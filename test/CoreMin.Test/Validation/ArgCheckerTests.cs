//<file>
//	<name>ArgCheckerTests.cs</file>
//	<content>Implementation of class ArgCheckerTests</content>
//	<owner name="Andreas Börcsök" email="support@smartexpert.boercsoek.de" />
//	<website>http://www.smartexpert.de</website>
//	<copyright>Copyright 2010 Andreas Börcsök</copyright>
//</file>

#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharpTestsEx;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.RegularExpression;

using Xunit;

#endregion

namespace SmartExpert.Test.Validation
{
	///<summary>ArgChecker Tests</summary>
	public class ArgCheckerTests : IDisposable
	{
		#region Test Methods
		// ReSharper disable InconsistentNaming
		// ReSharper disable AccessToModifiedClosure
        // ReSharper disable NotResolvedInText
		[Fact]
		public void TestCase001_ArgChecker_ShouldNotBeNull_ThrowArgNullException_IfArgumentIsNull_OtherwiseThrowsNot()
		{
			string testString = null;


			Executing.This(() => ArgChecker.ShouldNotBeNull(testString, "testString")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			testString = string.Empty;
			Executing.This(() => ArgChecker.ShouldNotBeNull(testString, "testString")).Should().NotThrow();
			testString = "Test";
			Executing.This(() => ArgChecker.ShouldNotBeNull(testString, "testString")).Should().NotThrow();

			object testObject = null;
			Executing.This(() => ArgChecker.ShouldNotBeNull(testObject, "testObject")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testObject = new object();
			Executing.This(() => ArgChecker.ShouldNotBeNull(testObject, "testObject")).Should().NotThrow();

			int? testNullable = null;
			Executing.This(() => ArgChecker.ShouldNotBeNull(testNullable, "testNullable")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testNullable = 42;
			Executing.This(() => ArgChecker.ShouldNotBeNull(testNullable, "testNullable")).Should().NotThrow();

			IEnumerable<string> testSequence = null;
			Executing.This(() => ArgChecker.ShouldNotBeNull(testSequence, "testSequence")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testSequence = Enumerable.Empty<string>();
			Executing.This(() => ArgChecker.ShouldNotBeNull(testSequence, "testSequence")).Should().NotThrow();
			testSequence = new List<string> {"First", "Second"};
			Executing.This(() => ArgChecker.ShouldNotBeNull(testSequence, "testSequence")).Should().NotThrow();
		}

		[Fact]
		public void TestCase002_ArgChecker_ShouldNotBeNullOrEmpty_ThrowException_IfArgumentIsNullOrEmpty_OtherwiseThrowsNot()
		{
			string testString = null;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testString, "testString")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testString = string.Empty;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testString, "testString")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testString = "";
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testString, "testString")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testString = "Test";
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testString, "testString")).Should().NotThrow();

			Guid? testNullableGuid = null;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testNullableGuid, "testNullableGuid")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testNullableGuid = Guid.Empty;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testNullableGuid, "testNullableGuid")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testNullableGuid = Guid.NewGuid();
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testNullableGuid, "testNullableGuid")).Should().NotThrow();

			StringBuilder testBuilder = null;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testBuilder, "testBuilder")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testBuilder = new StringBuilder();
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testBuilder, "testBuilder")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testBuilder.Append("Test");
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testBuilder, "testBuilder")).Should().NotThrow();
			testBuilder.Clear();
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testBuilder, "testBuilder")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);

			IEnumerable<string> testSequence = null;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testSequence, "testSequence")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			testSequence = EmptyArray<string>.Instance;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testSequence, "testSequence")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testSequence = new List<string> { "First", "Second" };
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testSequence, "testSequence")).Should().NotThrow();
		}

		[Fact]
		public void TestCase003_ArgChecker_ShouldNotBeEmpty_ThrowException_IfArgumentIsEmptyGuid_OtherwiseThrowsNot()
		{
			Guid testGuid = Guid.Empty;
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testGuid, "testGuid")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);
			testGuid = Guid.NewGuid();
			Executing.This(() => ArgChecker.ShouldNotBeNullOrEmpty(testGuid, "testGuid")).Should().NotThrow();
		}

		[Fact]
		public void TestCase004_ArgChecker_ShouldBeTrue_ThrowException_IfArgumentIsFalse_OtherwiseThrowsNot()
		{
			bool testBool = false;
			Executing.This(() => ArgChecker.ShouldBeTrue(testBool, "testBool", "testBool may not be false.")).Should().Throw<ArgException<bool>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);
			testBool = true;
			Executing.This(() => ArgChecker.ShouldBeTrue(testBool, "testBool", "testBool may not be false.")).Should().NotThrow();
		}

		[Fact]
		public void TestCase005_ArgChecker_ShouldBeFalse_ThrowException_IfArgumentIsTrue_OtherwiseThrowsNot()
		{
			bool testBool = true;
			Executing.This(() => ArgChecker.ShouldBeFalse(testBool, "testBool", "testBool may not be true.")).Should().Throw<ArgException<bool>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);
			testBool = false;
			Executing.This(() => ArgChecker.ShouldBeFalse(testBool, "testBool", "testBool may not be true.")).Should().NotThrow();
		}

		[Fact]
		public void TestCase006_ArgChecker_ShouldBeInRange_ThrowException_IfIntArgumentIsNotInRange_OtherwiseThrowsNot()
		{
			int testInt32 = 0;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", 0, 100)).Should().NotThrow();
			testInt32 = 100;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", 0, 100)).Should().NotThrow();
			testInt32 = -1;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", 0, 100)).Should().Throw<ArgOutOfRangeException<int>>()
				.And.ValueOf.ErrorCode.Should().Be(1005);
			testInt32 = 101;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", 0, 100)).Should().Throw<ArgOutOfRangeException<int>>()
				.And.ValueOf.ErrorCode.Should().Be(1005);
			testInt32 = 42;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", 42, 42)).Should().NotThrow();
			testInt32 = -42;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", -42, -42)).Should().NotThrow();
			testInt32 = -100;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", -100, 0)).Should().NotThrow();
			testInt32 = 0;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", -100, 0)).Should().NotThrow();
			testInt32 = 1;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", -100, 0)).Should().Throw<ArgOutOfRangeException<int>>()
				.And.ValueOf.ErrorCode.Should().Be(1005);
			testInt32 = -101;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", -100, 0)).Should().Throw<ArgOutOfRangeException<int>>()
				.And.ValueOf.ErrorCode.Should().Be(1005);
			testInt32 = Int32.MinValue;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", Int32.MinValue, Int32.MaxValue)).Should().NotThrow();
			testInt32 = Int32.MaxValue;
			Executing.This(() => ArgChecker.ShouldBeInRange(testInt32, "testInt32", Int32.MinValue, Int32.MaxValue)).Should().NotThrow();
		}

		[Fact]
		public void TestCase007_ArgChecker_ShouldBeExistingFile_ThrowException_IfFileArgumentDoesNotExist_OtherwiseThrowsNot()
		{
			string testFile = null;
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);
			
			testFile = string.Empty;
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);

			testFile = Path.Combine(Directory.GetCurrentDirectory(), "SmartExpert.BCL.CoreMin.Test.xunit");
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().NotThrow();
			
			testFile = Path.Combine(Directory.GetCurrentDirectory(), "NoTest.config");
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().Throw<ArgFilePathException>()
				.And.ValueOf.ErrorCode.Should().Be(1006);

			// <summary>Maximum file name length (259).</summary>
			string fillString = @"123456789\";
			string path = @"c:\";
			for (int i = 0; i < 25; i++)
			{
				path += fillString;
			}
			testFile = path + "1.txt";
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().Throw<ArgFilePathException>()
				.And.ValueOf.ErrorCode.Should().Be(1006);

			testFile = path + "123456789.txt";
			Executing.This(() => ArgChecker.ShouldBeExistingFile(testFile, "testFile")).Should().Throw<FilePathTooLongException>()
				.And.ValueOf.ErrorCode.Should().Be(1022);
		}
		
		[Fact]
		public void TestCase008_ArgChecker_ShouldBeExistingDirectory_ThrowException_IfDirectoryArgumentDoesNotExist_OtherwiseThrowsNot()
		{
			string testDirectory = null;
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			testDirectory = string.Empty;
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().Throw<ArgEmptyException>()
				.And.ValueOf.ErrorCode.Should().Be(1003);

			testDirectory = Directory.GetCurrentDirectory();
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().NotThrow();

			testDirectory = Path.Combine(Directory.GetCurrentDirectory(), "NoDirectory");
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().Throw<ArgDirectoryPathException>()
				.And.ValueOf.ErrorCode.Should().Be(1007);

			// <summary>Maximum folder name length  (247).</summary>
			string fillString = @"123456789\";
			string path = @"c:\";
			for (int i = 0; i < 24; i++)
			{
				path += fillString;
			}
			testDirectory = path;
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().Throw<ArgDirectoryPathException>()
				.And.ValueOf.ErrorCode.Should().Be(1007);
			
			path += fillString;
			testDirectory = path;
			Executing.This(() => ArgChecker.ShouldBeExistingDirectory(testDirectory, "testDirectory")).Should().Throw<DirectoryPathTooLongException>()
				.And.ValueOf.ErrorCode.Should().Be(1021);
		}

		[Fact]
		public void TestCase009_ArgChecker_ShouldMatch_ThrowException_IfArgumentDoesNotMatchRegexExpression_OtherwiseThrowsNot()
		{
			string testString = null;
			string testRegexPattern = null;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			testString = string.Empty;
// ReSharper disable RedundantAssignment
			testRegexPattern = null;
// ReSharper restore RedundantAssignment
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			testString = null;
			testRegexPattern = string.Empty;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			testString = string.Empty;
			testRegexPattern = string.Empty;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().NotThrow();

			testString = "ABCDefgh";
			testRegexPattern = RegexExpressionStrings.AlphaExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().NotThrow();

			testString = "ABCD efgh";
			testRegexPattern = RegexExpressionStrings.AlphaExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw<ArgException<string>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);

			testString = "ABCDefgh";
			testRegexPattern = RegexExpressionStrings.AlphaUpperCaseExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw < ArgException<string>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);
			
			testString = "ABCD";
			testRegexPattern = RegexExpressionStrings.AlphaUpperCaseExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().NotThrow();

			testString = "ABCDefgh";
			testRegexPattern = RegexExpressionStrings.AlphaLowerCaseExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().Throw<ArgException<string>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);

			testString = "efgh";
			testRegexPattern = RegexExpressionStrings.AlphaLowerCaseExpression;
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegexPattern)).Should().NotThrow();

			testString = "ABCDefgh";
			Regex testRegex = new Regex(RegexExpressionStrings.AlphaLowerCaseExpression);
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegex)).Should().Throw<ArgException<string>>()
				.And.ValueOf.ErrorCode.Should().Be(1001);

			testString = "efgh";
			Executing.This(() => ArgChecker.ShouldMatch(testString, "testString", testRegex)).Should().NotThrow();
		}

		[Fact]
		public void TestCase010_ArgChecker_ShouldBeAssignableFrom_ThrowException_IfArgumentTypeIsNotAssignableFromType_OtherwiseThrowsNot()
		{
			Type sourceType = GetType();
			Type targetType = Type.GetType("System.IDisposable");

			Executing.This(() => ArgChecker.ShouldBeAssignableFrom(sourceType, targetType, "this")).Should().NotThrow();
			Executing.This(() => ArgChecker.ShouldBeAssignableFrom<ArgCheckerTests, IDisposable>("this")).Should().NotThrow();

			Executing.This(() => ArgChecker.ShouldBeAssignableFrom(sourceType, null, "this")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			Executing.This(() => ArgChecker.ShouldBeAssignableFrom(null, targetType, "this")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			Executing.This(() => ArgChecker.ShouldBeAssignableFrom<ArgCheckerTests, IEnumerable>("this")).Should().Throw<InvalidTypeCastException>()
				.And.ValueOf.ErrorCode.Should().Be(1018);

		}

		[Fact]
		public void TestCase011_ArgChecker_ShouldBeInstanceOfType_ThrowException_IfArgumentIsNotInstanceOfType_OtherwiseThrowsNot()
		{
			Type targetType = Type.GetType("System.IDisposable");

			Executing.This(() => ArgChecker.ShouldBeInstanceOfType(targetType, this, "this")).Should().NotThrow();
			Executing.This(() => ArgChecker.ShouldBeInstanceOfType<IDisposable>(this, "this")).Should().NotThrow();

			Executing.This(() => ArgChecker.ShouldBeInstanceOfType(null, this, "this")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			Executing.This(() => ArgChecker.ShouldBeInstanceOfType(targetType, null, "this")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			Executing.This(() => ArgChecker.ShouldBeInstanceOfType<IDisposable>(null, "this")).Should().Throw<ArgNullException>()
				.And.ValueOf.ErrorCode.Should().Be(1002);

			Executing.This(() => ArgChecker.ShouldBeInstanceOfType<IEnumerable>(this, "this")).Should().Throw<InvalidTypeCastException>()
				.And.ValueOf.ErrorCode.Should().Be(1018);
		}

        // ReSharper restore NotResolvedInText
		// ReSharper restore AccessToModifiedClosure
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
