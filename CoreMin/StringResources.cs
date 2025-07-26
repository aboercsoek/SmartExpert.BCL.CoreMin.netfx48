//--------------------------------------------------------------------------
// File:    StringResources.cs
// Content:	Implementation of class StringResources
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

#endregion

#pragma warning disable 0649

namespace SmartExpert
{

	///<summary>String Resources Proxy Type</summary>
	internal class StringResources
	{
		static StringResources()
		{
			Type resourceSource = typeof(StringResources);
			var manager = new ResourceManager(resourceSource);
			foreach (MemberInfo info in resourceSource.GetMembers(BindingFlags.Public | BindingFlags.Static))
			{
				resourceSource.InvokeMember(info.Name, BindingFlags.SetField, null, null,
				                            new object[] { manager.GetString(info.Name, CultureInfo.CurrentUICulture) },
				                            CultureInfo.CurrentCulture);
			}
		}

		public static string ConsoleMenuMoreThan15MenuItemsAreNotSupported;
		
		public static string ErrorArgumentOutOfRangeValidationWithRangeTemplate4Args;
		public static string ErrorArgDirectoryPathToLongTemplate2Args;
		public static string ErrorArgFilePathToLongTemplate2Args;
		public static string ErrorArgumentDirectoryPathExceptionTemplate2Args;
		public static string ErrorArgumentFilePathExceptionTemplate2Args;
		public static string ErrorArgumentNotEmptyValidationTemplate1Arg;
		public static string ErrorArgumentOutOfRangeValidationTemplate4Args;
		public static string ErrorArgumentTemplate1Arg;
		public static string ErrorArgumentValidationFailedTemplate2Args;
		public static string ErrorDirectoryPathToLong;
		public static string ErrorFilePathToLong;
		public static string ErrorInvalidOperationRequestTemplate1Arg;
		public static string ErrorOperationExecutionFailedTemplate1Arg;
		public static string ErrorShouldNotBeNullOrEmptyValidationTemplate2Args;
		public static string ErrorShouldNotBeNullValidationTemplate1Arg;
		public static string ErrorTypesAreNotAssignableTemplate2Args;
		public static string ErrorWin32ExecutionFailedTemplate2Args;
		public static string ErrorInfrastructureOrSystem;
		public static string ErrorAccessViolation;

		public static string ErrorBusinessRuleViolation;

		public static string TrackedCollectionArrayTooSmall;
		public static string TrackedCollectionEnumHasChanged;
		public static string TrackedCollectionEnumInvalidPos;
		public static string TrackedCollectionIndexNotInArray;
		public static string TrackedCollectionNotOneDimensional;
	}
}
