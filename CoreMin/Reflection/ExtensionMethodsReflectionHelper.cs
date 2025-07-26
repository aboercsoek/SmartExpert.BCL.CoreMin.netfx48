//--------------------------------------------------------------------------
// File:    ExtensionMethodsHelper.cs
// Content:	Implementation of class ExtensionMethodsHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Reflection
{
	///<summary>Extension Methods Reflection Helper</summary>
	public static class ExtensionMethodsReflectionHelper
	{
		/// <summary>
		/// The extension method record class
		/// </summary>
		public class ExtensionMethodRecord
		{
			/// <summary>
			/// Type with Extension method
			/// </summary>
			public readonly Type ExtendingType;

			/// <summary>
			/// Method info of Extension method
			/// </summary>
			public readonly MethodInfo Method;

			/// <summary>
			/// The type that is extended by the Extension method
			/// </summary>
			public readonly Type ExtendedType;

			/// <summary>
			/// Initializes a new instance of the <see cref="ExtensionMethodRecord"/> class.
			/// </summary>
			/// <param name="extendingType">Type with Extension method.</param>
			/// <param name="extendedType">The type that is extended by the Extension method.</param>
			/// <param name="method">The Extension method.</param>
			public ExtensionMethodRecord(Type extendingType, Type extendedType, MethodInfo method)
			{
				ExtendingType = extendingType;
				ExtendedType = extendedType;
				Method = method;

			}
		}


		/// <summary>
		/// Finds all Extension methods defined in an assembly
		/// </summary>
		/// <param name="assembly">Assembly where the search for extension methods should take place.</param>
		/// <returns>Collection of found extension methods.</returns>
		public static IEnumerable<ExtensionMethodRecord> EnumExtensionMethods(Assembly assembly)
		{
			Type[] exportedTypes = assembly.GetExportedTypes();

			return exportedTypes.SelectMany<Type, ExtensionMethodRecord>(EnumExtensionMethods);
		}


		/// <summary>
		///  Finds all Extension methods defined by a type
		/// </summary>
		/// <param name="extendingType">The type to search for extension methods.</param>
		/// <returns>Collection of found extension methods.</returns>
		public static IEnumerable<ExtensionMethodRecord> EnumExtensionMethods(Type extendingType)
		{
			var extMethods = Enumerable.Where(extendingType.GetMethods(), IsExtensionMethod);

			return from extMethod in extMethods
				   let firstParam = extMethod.GetParameters()[0]
				   let extendedType = firstParam.ParameterType
				   select new ExtensionMethodRecord(extendingType, extendedType, extMethod);
		}

		/// <summary>
		/// Returns <see langword="true"/> if the <paramref name="method"/> is an extension method
		/// </summary>
		/// <param name="method">The method that should be checked</param>
		/// <returns>
		/// Returns <see langword="true"/> if the <paramref name="method"/> is an extension method; otherwise <see langword="false"/>.
		/// </returns>
		public static bool IsExtensionMethod(MethodInfo method)
		{
			if ((!method.IsPublic) || (!method.IsStatic))
			{
				// By definition an extension method must be public and static
				return false;
			}

			Type extAttrType = typeof(System.Runtime.CompilerServices.ExtensionAttribute);
			var extAttrs = (System.Runtime.CompilerServices.ExtensionAttribute[])method.GetCustomAttributes(extAttrType, false);

			return (extAttrs.Length > 0);
		}
	}
}
