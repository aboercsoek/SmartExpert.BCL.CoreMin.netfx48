//--------------------------------------------------------------------------
// File:    ReflectionEx.cs
// Content:	Implementation of class ReflectionEx
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
using SmartExpert;
using SmartExpert.Linq;
using JetBrains.Annotations;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>Reflection Extension Methods</summary>
	public static class ReflectionEx
	{
		#region Internal constants

		/// <summary>
		/// Mscorlib Assembly Ref
		/// </summary>
		[NotNull] internal static readonly Assembly Mscorlib = TypeOf.Object.Assembly;

		internal const BindingFlags StdBindingFlags = BindingFlags.Public | 
													 BindingFlags.NonPublic | 
													 BindingFlags.Static |
													 BindingFlags.Instance;

		#endregion

		#region Set and Get Field Helpers

		/// <summary>
		/// Gets the dynamic field value.
		/// </summary>
		/// <typeparam name="T">The type of the field.</typeparam>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>
		/// Returns the dynamic field value.
		/// </returns>
		public static T GetDynamicField<T>(this object reflectedObject, string fieldName)
		{
			return (T)GetDynamicField(reflectedObject, fieldName);
		}

		/// <summary>
		/// Gets the dynamic field value.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="fieldName">Name of the s field.</param>
		/// <returns>
		/// Returns the dynamic field value.
		/// </returns>
		public static object GetDynamicField(this object reflectedObject, string fieldName)
		{
			ArgChecker.ShouldNotBeNull(reflectedObject, "reflectedObject");
			ArgChecker.ShouldNotBeNullOrEmpty(fieldName, "fieldName");

			Type type = reflectedObject.GetType();
			FieldInfo field = type.GetField(fieldName, StdBindingFlags);
			if (field == null)
			{
				throw new MissingFieldException(type.ToString(), fieldName);
			}
			return field.GetValue(reflectedObject);
		}

		/// <summary>
		/// Sets the dynamic field value.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="newFieldValue">The new field value.</param>
		public static void SetDynamicField<T>(this object reflectedObject, string fieldName, T newFieldValue)
		{
			SetDynamicField(reflectedObject, fieldName, (object)newFieldValue);
		}

		/// <summary>
		/// Sets the dynamic field value.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <param name="newFieldValue">The new field value.</param>
		public static void SetDynamicField(this object reflectedObject, string fieldName, object newFieldValue)
		{
			ArgChecker.ShouldNotBeNull(reflectedObject, "reflectedObject");
			ArgChecker.ShouldNotBeNullOrEmpty(fieldName, "fieldName");

			Type type = reflectedObject.GetType();
			FieldInfo field = type.GetField(fieldName, StdBindingFlags);
			if (field == null)
			{
				throw new MissingFieldException(type.ToString(), fieldName);
			}
			
			field.SetValue(reflectedObject, newFieldValue);
		}

		#endregion

		#region Set and Get Property Helpers

		/// <summary>
		/// Gets the dynamic property value.
		/// </summary>
		/// <typeparam name="T">Type of the property</typeparam>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// Returns the dynamic property value.
		/// </returns>
		public static T GetDynamicProperty<T>(this object reflectedObject, string propertyName)
		{
			return (T)GetDynamicProperty(reflectedObject, propertyName);
		}

		/// <summary>
		/// Gets the dynamic property value.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// Returns the dynamic property value.
		/// </returns>
		public static object GetDynamicProperty(this object reflectedObject, string propertyName)
		{
			ArgChecker.ShouldNotBeNull(reflectedObject, "reflectedObject");
			ArgChecker.ShouldNotBeNullOrEmpty(propertyName, "propertyName");

			Type type = reflectedObject.GetType();
			PropertyInfo property = type.GetProperty(propertyName, StdBindingFlags);
			if (property == null)
			{
				throw new MissingMemberException(type.ToString(), propertyName);
			}

			return property.GetValue(reflectedObject, EmptyArray<object>.Instance);
		}

		/// <summary>
		/// Gets the field or property value.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="fieldOrPropertyName">Name of the field or property.</param>
		/// <returns>
		/// Returns the field or property value value.
		/// </returns>
		public static object GetFieldOrPropertyValue(this object reflectedObject, string fieldOrPropertyName)
		{
			ArgChecker.ShouldNotBeNull(reflectedObject, "reflectedObject");
			ArgChecker.ShouldNotBeNullOrEmpty(fieldOrPropertyName, "fieldOrPropertyName");

			FieldInfo fieldInfo = reflectedObject.GetType().GetField(fieldOrPropertyName, StdBindingFlags);
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(reflectedObject);
			}

			PropertyInfo property = reflectedObject.GetType().GetProperty(fieldOrPropertyName, StdBindingFlags);
			if (property != null)
			{
				return property.GetValue(reflectedObject, EmptyArray<object>.Instance);
			}
			return null;
		}

		/// <summary>
		/// Sets the dynamic property.
		/// </summary>
		/// <param name="reflectedObject">The reflected object.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="value">The value.</param>
		public static void SetDynamicProperty(this object reflectedObject, string propertyName, object value)
		{
			ArgChecker.ShouldNotBeNull(reflectedObject, "reflectedObject");
			ArgChecker.ShouldNotBeNullOrEmpty(propertyName, "propertyName");

			reflectedObject.GetType().InvokeMember(propertyName,
											BindingFlags.SetProperty | StdBindingFlags, 
											null, 
											reflectedObject, 
											new [] { value });
		}

		#endregion

		#region Public Properties Helpers

		/// <summary>
		/// Given an instance of a type, enumerates the name,value pairs for each public property of that anonymous type.
		/// </summary>
		/// <param name="instance">An instance from where all public properties and their values should be retrieved.</param>
		/// <returns>Enumerable collection of all found properties and their values.</returns>
		public static IEnumerable<KeyValuePair<string, object>> EnumPublicProperties(this object instance)
		{
			if (instance == null)
				return Enumerable.Empty<KeyValuePair<string, object>>();
			
			var t = instance.GetType();

			return from prop in t.GetProperties()
				   let v = prop.GetValue(instance, BindingFlags.Public, null, null, CultureInfo.InvariantCulture)
				   select new KeyValuePair<string, object>(prop.Name, v);

		}

		/// <summary>
		/// Given an instance of a type, returns a dictionary of name,value pairs for each public property
		/// </summary>
		/// <param name="instance">An instance from where all public properties and their values should be retrieved.</param>
		/// <returns>A dictionary of name,value pairs for each public property found in the given <paramref name="instance"/>.</returns>
		public static IDictionary<string, object> GetPublicPropertyDictionary(this object instance)
		{
			var dic = new Dictionary<string, object>();

			foreach (var kv in EnumPublicProperties(instance))
			{
				dic[kv.Key] = kv.Value;
			}

			return dic;

		}

		#endregion

		#region Invoke Method Helpers

		/// <summary>
		/// Dynamic method invokation.
		/// </summary>
		/// <param name="invokationTarget">The object instance where the method should be invoked.</param>
		/// <param name="methodName">The name of the method that should be invoked.</param>
		/// <param name="args">The method arguments.</param>
		/// <returns>The result of the invoked method.</returns>
		/// <exception cref="MissingMethodException">Is thrown if no matching method was found in <paramref name="invokationTarget"/>.</exception>
		public static object InvokeDynamicMethod(this object invokationTarget, string methodName, params object[] args)
		{
			ArgChecker.ShouldNotBeNull(invokationTarget, "invokationTarget");
			ArgChecker.ShouldNotBeNullOrEmpty(methodName, "methodName");
			ArgChecker.ShouldNotBeNullOrEmpty(args, "args");

			return invokationTarget.GetType().InvokeMember(methodName,
												BindingFlags.InvokeMethod | StdBindingFlags, 
												null, 
												invokationTarget, 
												args);
		}

		
		/// <summary>
		/// Dynamic method info invokation.
		/// </summary>
		/// <typeparam name="T">Return type of the method.</typeparam>
		/// <param name="invokationTarget">The object instance where the method should be invoked.</param>
		/// <param name="methodName">The name of the method that should be invoked.</param>
		/// <param name="args">The method arguments.</param>
		/// <returns>The result of the invoked method.</returns>
		/// <exception cref="MissingMethodException">Is thrown if no matching method was found in <paramref name="invokationTarget"/>.</exception>
		public static T InvokeDynamicMethodInfo<T>(this object invokationTarget, string methodName, params object[] args)
		{
			return invokationTarget.InvokeDynamicMethodInfo(methodName, args).As<T>();
		}

		/// <summary>
		/// Dynamic method info invokation.
		/// </summary>
		/// <param name="invokationTarget">The object instance where the method should be invoked.</param>
		/// <param name="methodName">The name of the method that should be invoked.</param>
		/// <param name="args">The method arguments.</param>
		/// <returns>The result of the invoked method.</returns>
		/// <exception cref="MissingMethodException">Is thrown if no matching method was found in <paramref name="invokationTarget"/>.</exception>
		public static object InvokeDynamicMethodInfo(this object invokationTarget, string methodName, params object[] args)
		{
			ArgChecker.ShouldNotBeNull(invokationTarget, "invokationTarget");
			ArgChecker.ShouldNotBeNullOrEmpty(methodName, "methodName");
			ArgChecker.ShouldNotBeNullOrEmpty(args, "args");

			Type type = invokationTarget.GetType();
			MethodInfo methodInfo = null;

			List<MethodInfo> source = (from meth in type.GetMethods() select meth).ToList();
			if (source.Count == 1)
			{
				methodInfo = source.Single();
			}

			if (methodInfo == null)
			{
				source = source.Where(meth => meth.GetParameters().Length == args.Length).ToList();
				if (source.Count == 1)
					methodInfo = source.Single();
			}

			if (methodInfo == null)
			{
				Func<MethodInfo, bool> wherePredicate = meth => meth.GetParameters()
				                                                	.Select(par => par.ParameterType)
				                                                	.EqualEnumerables(args.Select(arg => (arg == null) ? TypeOf.Object : arg.GetType()));

				source = source.Where(wherePredicate).ToList();
				if (source.Count == 1)
					methodInfo = source.Single();
			}

			if (methodInfo == null)
			{
				throw new MissingMethodException(type.ToString(), methodName);
			}
			
			return methodInfo.Invoke(invokationTarget, args);
		}

		/// <summary>
		/// Dynamic method info invokation.
		/// </summary>
		/// <param name="invokationTarget">The object instance where the method should be invoked.</param>
		/// <param name="methodName">The name of the method that should be invoked.</param>
		/// <param name="argTypes">The method argument types.</param>
		/// <param name="args">The method arguments.</param>
		/// <returns>The result of the invoked method.</returns>
		/// <exception cref="MissingMethodException">Is thrown if no matching method was found in <paramref name="invokationTarget"/>.</exception>
		public static object InvokeDynamicMethodInfo(this object invokationTarget, string methodName, Type[] argTypes, params object[] args)
		{
			ArgChecker.ShouldNotBeNull(invokationTarget, "invokationTarget");
			ArgChecker.ShouldNotBeNullOrEmpty(methodName, "methodName");
			ArgChecker.ShouldNotBeNullOrEmpty(args, "args");
			ArgChecker.ShouldNotBeNullOrEmpty(argTypes, "argTypes");
			ArgChecker.ShouldBeInRange(args.Length, "args", argTypes.Length, argTypes.Length);

			Type type = invokationTarget.GetType();
			MethodInfo methodInfo = null;

			var targetMethodInfos = (from method in type.GetMethods() select method).ToList();
			if (targetMethodInfos.Count == 1)
			{
				methodInfo = targetMethodInfos.Single();
			}

			if (methodInfo == null)
			{
				targetMethodInfos = targetMethodInfos.Where(meth => meth.GetParameters().Length == args.Length).ToList();
				if (targetMethodInfos.Count == 1)
					methodInfo = targetMethodInfos.Single();
			}

			if (methodInfo == null)
			{
				Func<MethodInfo, bool> wherePredicate = meth => meth.GetParameters()
																	.Select(par => par.ParameterType)
																	.EqualEnumerables(argTypes.Select(argType => argType));

				targetMethodInfos = targetMethodInfos.Where(wherePredicate).ToList();

				if (targetMethodInfos.Count == 1)
					methodInfo = targetMethodInfos.Single();
			}

			if (methodInfo == null)
			{
				throw new MissingMethodException(type.ToString(), methodName);
			}

			return methodInfo.Invoke(invokationTarget, args);
		}

		#endregion

	}
}
