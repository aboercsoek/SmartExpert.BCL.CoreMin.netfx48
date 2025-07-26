//--------------------------------------------------------------------------
// File:    TypeEx.cs
// Content:	Implementation of class TypeEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using SmartExpert;
using SmartExpert.Linq;

#endregion

// ReSharper disable CheckNamespace
namespace SmartExpert
{
// ReSharper restore CheckNamespace

	///<summary>Provides extension methods for <see cref="Type"/>.</summary>
	public static class TypeEx
	{
		#region TypeName extensions

		private static readonly Dictionary<Type, string> TypeNames;
		private static readonly char[] Separators;


		static TypeEx()
		{
			var dictionary = new Dictionary<Type, string>
			                 	{
			                 		{TypeOf.String, "string"},
			                 		{TypeOf.Object, "object"},
			                 		{TypeOf.Byte,	"byte"},
			                 		{TypeOf.Int16,	"short"},
									{TypeOf.Int32,  "int"},
			                 		{TypeOf.Int64,	"long"},
									{TypeOf.UInt16,	"ushort"},
									{TypeOf.UInt32, "uint"},
			                 		{TypeOf.UInt64,	"ulong"},
			                 		{TypeOf.Decimal,"decimal"},
			                 		{TypeOf.Single, "float"},
			                 		{TypeOf.Double, "double"},
			                 		{TypeOf.Boolean, "bool"},
			                 		{TypeOf.Char,	"char"},
			                 		{TypeOf.Void,	"void"}
			                 	};
			TypeNames = dictionary;
			Separators = new char[] { '&', '[', '*' };
		}

		/// <summary>
		/// Gets the name of the object type.
		/// <note>Supports generic type names in a user friendly way without the '-signs and also resolves nested generic type names.</note>
		/// </summary>
		/// <param name="value">The object to get the type name for.</param>
		/// <returns>
		/// Returns the type name.
		/// </returns>
		/// <remarks>Supports generic type names in a user friendly way without the '-signs and also resolves nested generic type names.</remarks>
		public static string GetTypeNameFromObject(object value)
		{
			if (value == null)
				return string.Empty;

			var type = Type.GetTypeFromHandle(Type.GetTypeHandle(value));

			if (type.IsGenericType)
			{
				var argNames = type
					.GetGenericArguments()
					.Select<Type,string>(GetTypeName)
					.ToArray();

				string args = StringEx.Join(",", argNames);

				string typeName = type.Name;
				int index = typeName.IndexOf("`", StringComparison.Ordinal);
				typeName = typeName.Substring(0, index);

				return string.Format("{0}[of {1}]", typeName, args);
			}

			return type.Name;
		}

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The type name in a user friendly way.</returns>
		public static string GetUserfriendlyTypeName(this Type type)
		{
			string str;
			if (TypeNames.TryGetValue(type, out str))
			{
				return str;
			}
			if (type.IsGenericType)
			{
				string genericTypeName = type.GetGenericTypeName();

				IEnumerable<string> values = from argType in type.GetGenericArguments() select argType.GetUserfriendlyTypeName();

				return string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", 
									new object[] { genericTypeName, StringEx.Join(", ", values) });
			}
			if ((!type.IsByRef && !type.IsArray) && !type.IsPointer)
			{
				return type.Name;
			}

			string typeName = type.GetElementType().GetUserfriendlyTypeName();
			int startIndex = type.Name.IndexOfAny(Separators);
			return (typeName + type.Name.Substring(startIndex));
		}

		private static string GetGenericTypeName(this Type type)
		{
			if (IsAnonymousType(type))
			{
				return "AnonymousType";
			}
			string name = type.GetGenericTypeDefinition().Name;
			int index = name.IndexOf('`');
			return name.Substring(0, index);
		}

		/// <summary>
		/// Gets the name of the type.
		/// <note>Supports generic type names in a user friendly way without the '-signs and also resolves nested generic type names.</note>
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// Returns the type name.
		/// </returns>
		/// <remarks>Supports generic type names in a user friendly way without the '-signs and also resolves nested generic type names.</remarks>
		public static string GetTypeName(this Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			if (type.IsGenericType)
			{

				var argNames = type
					.GetGenericArguments()
					.Select<Type,string>(GetTypeName)
					.ToArray();

				string args = string.Join(",", argNames);

				string typeName = type.Name;
				int index = typeName.IndexOf("`", StringComparison.Ordinal);
				typeName = typeName.Substring(0, index);

				return string.Format("{0}[of {1}]", typeName, args);
			}
			return type.Name;
		}

		/// <summary>
		/// Takes the type presentation, surrounds it with quotes if it contains spaces.
		/// </summary>
		public static string GetTypeNameWithQuoteIfNeeded(this Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			return type.AssemblyQualifiedName.QuoteIfNeeded();
		}

		#endregion

		#region Get Field extensions

		/// <summary>
		/// Gets the the first field that meet the fieldName condition.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>
		/// Returns the field info witch first match the fieldname condition.
		/// </returns>
		public static FieldInfo GetAnyField(this Type type, string fieldName)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			ArgChecker.ShouldNotBeNullOrEmpty(fieldName, "fieldName");

			return GetFieldInfo(type, fieldName);
		}

		/// <summary>
		/// Returns the enumerable collection of all instance fields in a type (only field that are declared in that type).
		/// </summary>
		/// <param name="type">The type to get the fields from</param>
		/// <returns>The fields of the specified type.</returns>
		public static IEnumerable<FieldInfo> GetAllFields(this Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			Type currentType = type;
			while (currentType != null)
			{
				FieldInfo[] fields =
					currentType.GetFields(BindingFlags.Public | 
											BindingFlags.NonPublic |
											BindingFlags.Instance |
											BindingFlags.Static |
											BindingFlags.IgnoreCase |
											BindingFlags.DeclaredOnly);
				foreach (FieldInfo fieldInfo in fields)
				{
					yield return fieldInfo;
				}
				currentType = currentType.BaseType;
			}
		}

		/// <summary>
		/// Gets the field info of a field inside Type type that matches the given fieldName.
		/// </summary>
		/// <param name="type">The type to search for the field.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>
		/// Returns the first field info value that matches the fieldname.
		/// </returns>
		/// <remarks>Begins the search in the provided type and walks if not found the inheritance tree up. 
		/// Returns null if the fieldName was not found in type nor in the inheritance tree.</remarks>
		private static FieldInfo GetFieldInfo(this Type type, string fieldName)
		{
			if (type == null)
				return null;

			if (fieldName.IsNullOrEmptyWithTrim())
				return null;

			var allFields = from f in type.GetFields(BindingFlags.Public |
													 BindingFlags.NonPublic |
													 BindingFlags.Instance |
													 BindingFlags.Static |
													 BindingFlags.IgnoreCase |
													 BindingFlags.DeclaredOnly)
							where f.Name.ToLowerInvariant() == fieldName.ToLowerInvariant()
							select f;

			FieldInfo field = allFields.FirstOrDefault();
			if (field != null)
				return field;

			return GetFieldInfo(type.BaseType, fieldName);
		}

		#endregion

		#region Default Value related extensions

		/// <summary>
		/// Gets the default value for this reference or value type.
		/// </summary>
		public static object GetDefaultValue(this Type type)
		{
			return type.IsValueType.IsFalse() ? null : Activator.CreateInstance(type);
		}


		/// <summary>
		/// Gets whether the <paramref name="value" /> is the default value for this reference or value type.
		/// </summary>
		public static bool IsDefaultValue(this Type type, object value)
		{
			if (ReferenceEquals(value, null))
			{
				return true;
			}
			return type.IsValueType && Equals(Activator.CreateInstance(type), value);
		}

		/// <summary>
		/// Gets whether the <paramref name="value" /> is the default value for this reference or value type, or an empty string.
		/// </summary>
		public static bool IsDefaultValueOrEmptyString(this Type type, object value)
		{
			if (ReferenceEquals(value, null))
			{
				return true;
			}

			if (type.IsValueType.IsFalse())
			{
				return ((value as string) == string.Empty);
			}
			return Equals(Activator.CreateInstance(type), value);
		}

		#endregion

		#region Is...Type methods (Nullable, OpenGeneric)

		/// <summary>
		/// Determines whether <paramref name="type"/> is a <see cref="Nullable{type}"/> type.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if <paramref name="type"/> is a <see cref="Nullable{type}"/> type; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsNullableType(this Type type)
		{
			//http://msdn.microsoft.com/en-us/library/ms366789.aspx
			return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));

		}

		/// <summary>
		/// Is this type an open generic type
		/// </summary>
		/// <returns><see langword="true"/> if the specified type is an open generic type, otherwise <see langword="false"/></returns>
		public static bool IsOpenGenericType(this Type type)
		{
			if (type == null)
				return false;

			return (type.IsGenericType && type.ContainsGenericParameters);
		}

		/// <summary>
		/// Determines whether type is an anonymous type.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		///   <see langword="true"/> if the specified type is a anonymous type; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsAnonymousType(this Type type)
		{
			if (((!Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) || 
				!type.IsGenericType) || 
				!type.Name.Contains("AnonymousType")) || 
				(!type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) && !type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase)))
			{
				return false;
			}

			//TypeAttributes attributes = type.Attributes;
			return true;
		}



		/// <summary>
		/// Determines whether <paramref name="type"/> is derived from <paramref name="baseType"/>.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <param name="baseType">The specified base type.</param>
		/// <returns>
		///   <see langword="true"/> if <paramref name="type"/> is derived from <paramref name="baseType"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDerivedFrom(this Type type, Type baseType)
		{
			if (type.BaseType == null)
				return false;

			return ((type.BaseType.GUID == baseType.GUID) || IsDerivedFrom(type.BaseType, baseType));
		}

		/// <summary>
		/// Determines whether the specified type is implements IEnumerable&lt;&gt;.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <see langword="true"/> if the specified type implements IEnumerable&lt;&gt;; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsEnumerable(this Type type)
		{
			return type.GetInterfaces().FirstOrDefault(
				t => t.IsGenericType &&
					t.GetGenericTypeDefinition() == typeof(IEnumerable<>)) != null;
		}

		/// <summary>
		/// Gets the type T of a type that implements IEnumerable{T}.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>The generic argument type T of IEnumerable{T}; or <see langword="null"/> if <paramref name="type"/> 
		/// does not implements IEnumerable{T}.</returns>
		public static Type GetEnumerableType(this Type type)
		{
			var enumerableInterface = type.GetInterfaces().FirstOrDefault(
				t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

			return enumerableInterface != null ? enumerableInterface.GetGenericArguments()[0] : null;
		}

		/// <summary>
		/// Checks if <paramref name="type"/> implements the interface of type <paramref name="interfaceType"/>
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>true if type implements interfaceType; otherwise false.</returns>
		public static bool ImplementsInterface(this Type type, Type interfaceType)
		{
			Func<Type, bool> predicate = interfaceType.IsAssignableFrom;
			if (interfaceType.IsGenericType)
			{
				predicate = t => t.IsGenericType && interfaceType.IsAssignableFrom(t.GetGenericTypeDefinition());
			}
			return predicate(type) || type.GetInterfaces().Any<Type>(predicate);
		}

		/// <summary>
		/// Checks if <paramref name="type"/> implements at least one of the <paramref name="interfaces"/>
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="interfaces">The interfaces type collection.</param>
		/// <returns>true if type implements at least one interface of the interface type collection; otherwise false.</returns>
		public static bool ImplementsInterface(this Type type, IEnumerable<Type> interfaces)
		{
			return interfaces.Any(type.ImplementsInterface);
		}

		#endregion
	}
}

