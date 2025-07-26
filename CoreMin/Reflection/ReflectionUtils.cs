//--------------------------------------------------------------------------
// File:    ReflectionUtils.cs
// Content:	Implementation of class ReflectionUtils
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.SystemRuntime;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>Reflection Utillity Class</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static class ReflectionUtils
	{
		#region Public static Members

		public static readonly object[] NoObjects = EmptyArray<object>.Instance;

		#endregion

		#region Private static Members

		private static readonly string[] AssemblyExtensions = new string[] { ".dll", ".exe" };
		private static readonly Dictionary<MethodInfo, PropertyInfo> GetterOf;
		private static readonly Dictionary<Type, FieldInfo[]> OrderedDeclaredInstanceFields;
		private static readonly Dictionary<Type, FieldInfo[]> OrderedDeclaredStaticFields;
		private static readonly string[] ReflectionOnlyAssemblyExtensions = new [] { ".dll", ".exe" };
		private static readonly Dictionary<MethodInfo, PropertyInfo> SetterOf;

		//public static readonly Type GenericByRefHolderType = typeof(__ByRefHolder<>);

		#endregion

		#region Static Ctor

		static ReflectionUtils()
		{
			OrderedDeclaredInstanceFields = new Dictionary<Type, FieldInfo[]>();
			OrderedDeclaredStaticFields = new Dictionary<Type, FieldInfo[]>();
			SetterOf = new Dictionary<MethodInfo, PropertyInfo>();
			GetterOf = new Dictionary<MethodInfo, PropertyInfo>();
		}

		#endregion

		#region MethodInfo and MethodBase related Methods

		public static MethodInfo GetBaseDefinition(MethodInfo methodInfo)
		{
			MethodInfo baseDefinition;
			try
			{
				baseDefinition = methodInfo.GetBaseDefinition();
			}
			catch (ArgumentException)
			{
				baseDefinition = GetBaseDefinition(RebuildMethodInfo(methodInfo));
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;

				baseDefinition = GetBaseDefinition(RebuildMethodInfo(methodInfo));
			}
			return baseDefinition;
		}

		public static MethodInfo GetBaseDefinition(MethodInfo methodInfo, out bool wasBaseDefinitionAlready)
		{
			ArgChecker.ShouldNotBeNull(methodInfo, "methodInfo");

			if ((methodInfo.DeclaringType != null) && (methodInfo.DeclaringType.IsValueType))
			{
				methodInfo = RebuildMethodInfo(methodInfo);
			}

			MethodInfo baseDefinition = GetBaseDefinition(methodInfo);

			if (((baseDefinition != null) && (GetMetadataTokenOrZero(baseDefinition) == GetMetadataTokenOrZero(methodInfo))) &&
				(baseDefinition.Module == methodInfo.Module))
			{
				wasBaseDefinitionAlready = true;
				return baseDefinition;
			}

			wasBaseDefinitionAlready = false;
			return baseDefinition;
		}

		public static MethodInfo GetMethod(Type declaringType, string name, Type[] parameterTypes)
		{
			ArgChecker.ShouldNotBeNull(declaringType, "declaringType");
			ArgChecker.ShouldNotBeNull(name, "name");
			
			if (ContainsGenericParameters(declaringType))
			{
				throw new ArgumentException("declaring type must not contain type parameters", "declaringType");
			}
			
			ThrowIfNotClassOrValueType(declaringType, "declaringType");
			
			MethodInfo methodInfoResult = null;
			
			if (parameterTypes == null)
			{
				const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | 
												  BindingFlags.Static | BindingFlags.Instance;

				foreach (MethodInfo methodInfo in declaringType.GetMethods(bindingFlags))
				{
					if (methodInfo.Name == name)
					{
						if (methodInfoResult != null)
						{
							throw new AmbiguousMatchException("there is more than one matching method with the name " + name);
						}
						methodInfoResult = methodInfo;
					}
				}
			}
			else
			{
				methodInfoResult = declaringType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, parameterTypes, null);
			}

			if (methodInfoResult == null)
			{
				throw new ArgumentException(String.Format("method {0}({1}) not found in {2}", name, 
					StringHelper.Join(",", parameterTypes, (sb, t) => sb.Append(t.Name)), 
					declaringType));
			}

			return methodInfoResult;
		}

		public static string GetMethodFullName(MethodBase method)
		{
			if (method == null)
				return string.Empty;

			if (method.DeclaringType != null)
			{
				return String.Format("{0}.{1}({2})", method.DeclaringType.FullName, method.Name, GetMethodParameters(method.GetParameters(), ", "));
			}
			return String.Format("{0}({1})", method.Name, GetMethodParameters(method.GetParameters(), ", "));
		}



		public static string GetMethodFullNameWithParameterNames(MethodBase method)
		{
			if (method == null)
				return string.Empty;

			if (method.DeclaringType != null)
			{
				return String.Format("{0}.{1}({2})", method.DeclaringType.FullName, method.Name, GetMethodParametersWithNames(method.GetParameters(), ", "));
			}
			return String.Format("{0}({1})", method.Name, GetMethodParametersWithNames(method.GetParameters(), ", "));
		}


		public static string GetMethodParameters(ParameterInfo[] parameters, string separator)
		{
			return StringHelper.Join<ParameterInfo>(separator, parameters, (sb, p) => sb.Append(p.ParameterType.Name));
		}

		public static string GetMethodParametersWithNames(ParameterInfo[] parameters, string separator)
		{
			if (parameters.IsNullOrEmpty())
				return string.Empty;

			return StringHelper.Join<ParameterInfo>(separator, parameters, (sb, p) => sb.AppendFormat("{0} {1}", p.ParameterType.Name, p.Name));
		}

		public static MethodInfo GetRootDefinition(MethodInfo methodInfo)
		{
			ArgChecker.ShouldNotBeNull(methodInfo, "methodInfo");

			bool wasBaseDefinitionAlready;
			do
			{
				methodInfo = GetBaseDefinition(methodInfo, out wasBaseDefinitionAlready);
			}
			while (wasBaseDefinitionAlready.IsFalse());

			return methodInfo;
		}

		public static MethodInfo RebuildMethodInfo(MethodInfo methodInfo)
		{
			ArgChecker.ShouldNotBeNull(methodInfo, "methodInfo");

			int metadataTokenOrZero = GetMetadataTokenOrZero(methodInfo);
			if (metadataTokenOrZero != 0)
			{
				Type declaringType = methodInfo.DeclaringType;

				methodInfo = (MethodInfo)methodInfo.Module.ResolveMethod(
					metadataTokenOrZero,
					(declaringType == null || IsGenericType(declaringType).IsFalse()) ? Type.EmptyTypes : methodInfo.GetGenericArguments(),
					(methodInfo.IsGenericMethod.IsFalse()) ? Type.EmptyTypes : methodInfo.GetGenericArguments()
					);
			}

			return methodInfo;
		}

		#endregion

		#region GetOrderedDeclaredInstance... methods

		public static FieldInfo[] GetOrderedDeclaredInstanceFields(this Type type)
		{
			return OrderedDeclaredInstanceFields.GetOrCreateValue(type, ComputeOrderedDeclaredInstanceFields);
		}

		public static FieldInfo[] GetOrderedDeclaredStaticFields(this Type type)
		{
			return OrderedDeclaredStaticFields.GetOrCreateValue(type, ComputeOrderedDeclaredStaticFields);
		}

		private static FieldInfo[] ComputeOrderedDeclaredInstanceFields(Type type)
		{
			const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public |
											  BindingFlags.Instance | BindingFlags.DeclaredOnly;

			return Enumerable.Where(type.GetFields(bindingFlags), fieldInfo => (fieldInfo.DeclaringType == type) && !fieldInfo.IsStatic).ToArray();
		}

		private static FieldInfo[] ComputeOrderedDeclaredStaticFields(Type type)
		{
			const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public |
											  BindingFlags.Static | BindingFlags.DeclaredOnly;

			return Enumerable.Where(type.GetFields(bindingFlags), fieldInfo => (fieldInfo.DeclaringType == type) && fieldInfo.IsStatic).ToArray();
		}
		
		#endregion

		#region Type related Methods

		public static Type GetType(string assemblyName, string typeFullName)
		{
			Assembly assembly;
			if (String.IsNullOrEmpty(assemblyName))
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (String.IsNullOrEmpty(typeFullName))
			{
				throw new ArgumentNullException("typeFullName");
			}
			if (!TryLoadAssembly(assemblyName, out assembly))
			{
				throw new ArgumentException("could not load assembly", "assemblyName");
			}
			return assembly.GetType(typeFullName, true);
		}

		public static Type GetType(Type typeOfAssembly, string fullTypeName)
		{
			if (typeOfAssembly == null)
			{
				throw new ArgumentNullException("typeOfAssembly");
			}
			if (String.IsNullOrEmpty(fullTypeName))
			{
				throw new ArgumentException("cannot be empty", "fullTypeName");
			}
			Type type = typeOfAssembly.Assembly.GetType(fullTypeName, false);
			if (type == null)
			{
				throw new ArgumentException(String.Format("assembly {0} does not contain {1}", typeOfAssembly.Assembly.GetName().Name, fullTypeName));
			}
			return type;
		}

		#endregion

		#region Is... Methods

		public static bool IsAbstract(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsAbstract(type, out flag) && flag);
		}

		public static bool IsArray(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsArray(type, out flag) && flag);
		}

		public static bool IsByRef(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsByRef(type, out flag) && flag);
		}

		public static bool IsClass(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsClass(type, out flag) && flag);
		}

		public static bool IsCOMObject(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsCOMObject(type, out flag) && flag);
		}

		public static bool IsEnum(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsEnum(type, out flag) && flag);
		}

		public static bool IsGenericParameter(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsGenericParameter(type, out flag) && flag);
		}

		public static bool IsGenericType(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsGenericType(type, out flag) && flag);
		}

		public static bool IsGenericTypeDefinition(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsGenericTypeDefinition(type, out flag) && flag);
		}

		public static bool IsInterface(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsInterface(type, out flag) && flag);
		}

		public static bool IsNotRuntimeType(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			if (type.HasElementType)
			{
				return IsNotRuntimeType(type.GetElementType());
			}
			return (type != Metadata.RuntimeTypeType);
		}

		public static bool IsPointer(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsPointer(type, out flag) && flag);
		}

		public static bool IsPubliclyConstructible(Type type, params Type[] parameterTypes)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			
			if (!type.IsPublic && !type.IsNestedPublic)
			{
				return false;
			}
			if (IsAbstract(type) || ContainsGenericParameters(type))
			{
				return false;
			}
			ConstructorInfo constructor = type.GetConstructor(parameterTypes);
			return ((constructor != null) && constructor.IsPublic);
		}

		public static bool IsPubliclyConstructibleSomehow(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			if (!type.IsPublic && !type.IsNestedPublic)
			{
				return false;
			}
			return ((!IsAbstract(type) && !ContainsGenericParameters(type)) && (type.GetConstructors().Length > 0));
		}

		public static bool IsPubliclyDefaultConstructible(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			return IsPubliclyConstructible(type, Type.EmptyTypes);
		}

		public static bool IsSealed(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsSealed(type, out flag) && flag);
		}

		public static bool IsValueType(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetIsValueType(type, out flag) && flag);
		}

		#endregion

		#region TryGetIs... Methods

		public static bool TryGetIsAbstract(Type type, out bool result)
		{
			try
			{
				result = type.IsAbstract;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsArray(Type type, out bool result)
		{
			try
			{
				result = type.IsArray;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsByRef(Type type, out bool result)
		{
			try
			{
				result = type.IsByRef;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsClass(Type type, out bool result)
		{
			try
			{
				result = type.IsClass;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsCOMObject(Type type, out bool result)
		{
			try
			{
				result = type.IsCOMObject;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsEnum(Type type, out bool result)
		{
			try
			{
				result = type.IsEnum;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsGenericParameter(Type type, out bool result)
		{
			try
			{
				result = type.IsGenericParameter;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsGenericType(Type type, out bool result)
		{
			try
			{
				result = type.IsGenericType;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsGenericTypeDefinition(Type type, out bool result)
		{
			try
			{
				result = type.IsGenericTypeDefinition;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsInterface(Type type, out bool result)
		{
			try
			{
				result = type.IsInterface;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsPointer(Type type, out bool result)
		{
			try
			{
				result = type.IsPointer;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsSealed(Type type, out bool result)
		{
			try
			{
				result = type.IsSealed;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		public static bool TryGetIsValueType(Type type, out bool result)
		{
			try
			{
				result = type.IsValueType;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		#endregion

		#region TryGetPropertyOfGetter and Setter

		public static bool TryGetPropertyOfGetter(MethodInfo getter, out PropertyInfo property)
		{
			property = GetterOf.GetOrCreateValue(getter, ComputePropertyOfGetter);
			return (property != null);
		}

		public static bool TryGetPropertyOfSetter(MethodInfo setter, out PropertyInfo property)
		{
			property = SetterOf.GetOrCreateValue(setter, ComputePropertyOfSetter);
			return (property != null);
		}

		private static PropertyInfo ComputePropertyOfGetter(MethodInfo getter)
		{
			ArgChecker.ShouldNotBeNull(getter, "getter");

			if (getter.DeclaringType == null)
				return null;

			const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public |
											  BindingFlags.Static | BindingFlags.Instance;

			return getter.DeclaringType.GetProperties(bindingFlags).FirstOrDefault(propertyInfo => propertyInfo.GetGetMethod(true) == getter);
		}

		private static PropertyInfo ComputePropertyOfSetter(MethodInfo setter)
		{
			ArgChecker.ShouldNotBeNull(setter, "setter");

			if (setter.DeclaringType == null)
				return null;

			const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public |
											  BindingFlags.Static | BindingFlags.Instance;

			return setter.DeclaringType.GetProperties(bindingFlags).FirstOrDefault(propertyInfo => propertyInfo.GetSetMethod(true) == setter);
		}

		#endregion

		#region ThrowIfNot... Methods

		public static void ThrowIfAbstract(Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			if (IsAbstract(type))
			{
				throw new ArgumentException(String.Format("{0} must not be abstract", type), name);
			}
		}

		public static void ThrowIfNotClass(Type type, string name)
		{
			bool flag;
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			if (!TryGetIsClass(type, out flag))
			{
				throw new ArgumentException(String.Format("cannot determine if {0} is class", type));
			}
			if (flag.IsFalse())
			{
				bool flag2;
				throw new ArgumentException(String.Format("{0} must be class{1}", type, (TryGetIsInterface(type, out flag2) && flag2) ? " (interface not allowed)" : ""), name);
			}
		}

		public static void ThrowIfNotClassOrInterfaceOrValueType(Type type, string name)
		{
			bool flag;
			bool flag2;
			bool flag3;
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			if ((!TryGetIsClass(type, out flag2) || !TryGetIsInterface(type, out flag3)) || !TryGetIsValueType(type, out flag))
			{
				throw new ArgumentException(String.Format("cannot determine if {0} is class, interface or value type", type));
			}
			if ((!flag && !flag2) && !flag3)
			{
				throw new ArgumentException(String.Format("{0} must be class, interface or struct", type), name);
			}
		}

		public static void ThrowIfNotClassOrValueType(Type type, string name)
		{
			bool flag;
			bool flag2;
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			if (!TryGetIsClass(type, out flag2) || !TryGetIsValueType(type, out flag))
			{
				throw new ArgumentException(String.Format("cannot determine if {0} is class or value type", type));
			}
			if (!flag && !flag2)
			{
				throw new ArgumentException(String.Format("{0} must be class or struct", type), name);
			}
		}

		public static void ThrowIfNotClosedTypeReference(Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			if (ContainsGenericParameters(type))
			{
				throw new ArgumentException(String.Format("{0} must not contain generic parameters", type), name);
			}
		}

		public static void ThrowIfNotDelegate(Type type, string name)
		{
			if (type == null)
			{
				throw new ArgumentNullException(name);
			}
			Type baseType = type.BaseType;
			if ((baseType != typeof(Delegate)) && (baseType != typeof(MulticastDelegate)))
			{
				throw new ArgumentException(String.Format("{0} must be delegate", type), name);
			}
		}

		public static void ThrowIfNotTypeDefinition(Type type, string name)
		{
			ThrowIfNotClassOrInterfaceOrValueType(type, name);
			if (!IsGenericTypeDefinition(type) && ContainsGenericParameters(type))
			{
				throw new ArgumentException(String.Format("{0} must be type definition", type), name);
			}
		}

		#endregion

		#region Generic Type and Parameter related Methods

		public static Type MakeGenericType(Type genericType, params Type[] typeArguments)
		{
			ArgChecker.ShouldNotBeNull(genericType, "genericType");

			Type type;
			try
			{
				type = genericType.MakeGenericType(typeArguments);
			}
			catch (ArgumentException exception)
			{
				throw new ArgumentException(FormatGenericTypeMessage(genericType, typeArguments), exception);
			}
			catch (TypeLoadException exception2)
			{
				throw new TypeLoadException(FormatGenericTypeMessage(genericType, typeArguments), exception2);
			}
			return type;
		}

		public static bool ContainsGenericParameters(this Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			bool flag;
			return (TryGetContainsGenericParameters(type, out flag) && flag);
		}

		public static bool TryGetContainsGenericParameters(Type type, out bool result)
		{
			ArgChecker.ShouldNotBeNull(type, "type");
			try
			{
				result = type.ContainsGenericParameters;
				return true;
			}
			catch (NotSupportedException)
			{
				result = false;
				return false;
			}
		}

		private static string FormatGenericTypeMessage(Type genericType, IEnumerable<Type> typeArguments)
		{
			ArgChecker.ShouldNotBeNull(genericType, "genericType");

			var builder = new StringBuilder();
			builder.AppendFormat("The generic type '{0}' cannot be instantiated with [", genericType.FullName);

			bool flag = true;
			foreach (Type type in typeArguments)
			{
				if (flag)
					flag = false;
				else
					builder.Append(',');

				builder.AppendFormat("'{0}'", type.FullName);
			}

			builder.Append(']');

			return builder.ToString();
		}

		#endregion

		#region LoadAssemby Methods

#if NET2_0 || NET3_0 || NET3_5		
		[SecurityCritical(SecurityCriticalScope.Explicit)]
#endif
		public static bool TryLoadAssembly(string assemblyName, out Assembly assembly)
		{
			if (assemblyName.IsNullOrEmptyWithTrim())
			{
				assembly = null;
				return false;	
			}

			bool skipLoadAssemblyUsingAssemblyNameType = false;

			assembly = null;
			if (assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) || assemblyName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					assembly = Assembly.LoadFrom(assemblyName);
					return true;
				}
				catch (FileLoadException exception)
				{
					Exception innerException = exception.InnerException;
					if ((innerException != null) && (innerException.GetType() == typeof(SecurityException)))
					{
						assembly = null;
						return false;
					}
				}
				catch (FileNotFoundException)
				{ }

				try
				{
					assembly = Assembly.LoadFrom(Path.GetFullPath(assemblyName));
					return true;
				}
				catch (FileLoadException fileLoadException)
				{
					Exception innerException = fileLoadException.InnerException;
					if ((innerException != null) && (innerException.GetType() == typeof(SecurityException)))
					{
						assembly = null;
						return false;
					}
					skipLoadAssemblyUsingAssemblyNameType = true;
				}
				catch (FileNotFoundException)
				{
					skipLoadAssemblyUsingAssemblyNameType = true;
				}
			}

			if (skipLoadAssemblyUsingAssemblyNameType)
			{
				try
				{
					var assemblyRef = new AssemblyName(assemblyName);
					assembly = Assembly.Load(assemblyRef);
					//assembly = Assembly.Load(assemblyName);
					return true;
				}
				catch (FileLoadException)
				{ }
				catch (FileNotFoundException)
				{ }
			}

			try
			{
#pragma warning disable 612,618
				assembly = Assembly.LoadWithPartialName(assemblyName);
#pragma warning restore 612,618
			}
			catch (FileLoadException)
			{
			}
			
			return (assembly != null);
		}

		public static bool TryLoadAssemblyFrom(string fileName, out Assembly assembly)
		{
			if (File.Exists(fileName))
			{
				try
				{
					assembly = Assembly.LoadFrom(fileName);
					return true;
				}
				catch (FileLoadException)
				{
				}
				catch (FileNotFoundException)
				{
				}
			}
			assembly = null;
			return false;
		}

#if NET2_0 || NET3_0 || NET3_5		
		[SecurityCritical(SecurityCriticalScope.Explicit)]
#endif		
		public static bool TryLoadAssemblyFrom(string path, string assemblyName, out Assembly assembly)
		{
			if (Directory.Exists(path).IsFalse() || assemblyName.IsNullOrEmptyWithTrim())
			{
				assembly = null;
				return false;
			}

			foreach (string str in AssemblyExtensions)
			{
				if (TryLoadAssemblyFrom(Path.GetFullPath(Path.Combine(path, assemblyName + str)), out assembly))
				{
					return true;
				}
			}

			assembly = null;
			return false;
		}

		public static bool TryReflectionOnlyLoadAssembly(string assemblyName, out Assembly assembly)
		{
			if (assemblyName.IsNullOrEmptyWithTrim())
			{
				assembly = null;
				return false;
			}

			try
			{
				string str;
				var name = new AssemblyName(assemblyName);
				if ((name.FullName == assemblyName) && GacManager.TryQueryAssemblyInfo(assemblyName, true, out str))
				{
					assembly = Assembly.ReflectionOnlyLoadFrom(str);
				}
				else
				{
					assembly = Assembly.ReflectionOnlyLoad(assemblyName);
				}
				if (assembly != null)
				{
					return true;
				}
			}
			catch (FileLoadException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			return TryReflectionOnlyLoadAssemblyFrom(Path.GetFullPath(assemblyName), out assembly);
		}


		public static bool TryReflectionOnlyLoadAssemblyFrom(string assemblyFile, out Assembly assembly)
		{
			if (File.Exists(assemblyFile).IsFalse())
			{
				assembly = null;
				return false;
			}

			try
			{
				assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
				return true;
			}
			catch (BadImageFormatException)
			{
			}
			catch (FileLoadException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			assembly = null;
			return false;
		}

		public static bool TryReflectionOnlyLoadAssemblyFrom(string path, string assemblyName, out Assembly assembly)
		{
			if (Directory.Exists(path).IsFalse() || assemblyName.IsNullOrEmptyWithTrim())
			{
				assembly = null;
				return false;
			}

			foreach (string asmFileExt in ReflectionOnlyAssemblyExtensions)
			{
				if (!TryReflectionOnlyLoadAssemblyFrom(Path.GetFullPath(Path.Combine(path, assemblyName + asmFileExt)), out assembly))
					continue;

				return true;
			}

			assembly = null;
			return false;
		}

		#endregion

		#region EscapeForMetadataName Methods

		public static string EscapeForMetadataName(this MethodInfo method)
		{
			if (method.IsGenericMethod.IsFalse())
				return EscapeForMetadataName(method.Name);

			var builder = new StringBuilder();
			builder.Append(EscapeForMetadataName(method.Name));

			foreach (Type type in method.GetGenericArguments())
			{
				builder.Append(EscapeForMetadataName(type));
			}

			return builder.ToString();
		}

		public static string EscapeForMetadataName(string value)
		{

			bool foundInvalidMetadataNameChar = false;

			if (ValidFirstMetadataNameChar(value[0]))
			{
				for (var j = 1; j < value.Length; j++)
				{
					if (ValidMetadataNameChar(value[j])) continue;

					foundInvalidMetadataNameChar = true;
					break;
				}

				if (foundInvalidMetadataNameChar.IsFalse())
					return value;
			}

			var builder = new StringBuilder(value.Length);

			builder.Append(ValidFirstMetadataNameChar(value[0]) ? value[0] : '_');

			for (var i = 1; i < value.Length; i++)
			{
				if (ValidMetadataNameChar(value[i]))
					builder.Append(value[i]);
			}

			return builder.Length == 0 ? "_" : builder.ToString();
		}

		public static string EscapeForMetadataName(Type type)
		{
			if (IsGenericType(type).IsFalse())
			{
				return EscapeForMetadataName(type.Name);
			}

			var builder = new StringBuilder();
			builder.Append(EscapeForMetadataName(type.Name));

			foreach (Type type2 in type.GetGenericArguments())
			{
				builder.Append(EscapeForMetadataName(type2));
			}

			return builder.ToString();
		}

		public static void EscapeForMetadataPart(StringBuilder sb, string value)
		{
			foreach (var validChar in value.Where(ValidMetadataNameChar))
			{
				sb.Append(validChar);
			}
		}

		#endregion

		#region Metadata related Methods

		public static int GetMetadataToken(MemberInfo memberInfo)
		{
			int metadataTokenOrZero = GetMetadataTokenOrZero(memberInfo);
			if (metadataTokenOrZero == 0)
			{
				throw new OperationExecutionFailedException("could not get metadataToken of {0}".SafeFormatWith(memberInfo));
			}
			return metadataTokenOrZero;
		}

		public static int GetMetadataTokenOrZero(MemberInfo memberInfo)
		{
			if (memberInfo == null)
				return 0;

			try
			{
				return memberInfo.MetadataToken;
			}
			catch (InvalidOperationException)
			{
				return 0;
			}
		}

		private static class Metadata
		{
			public static readonly Type RuntimeTypeType = TypeOf.Object;
		}

		#endregion

		#region Private Metadata Character Validation Methods

		private static bool ValidFirstMetadataNameChar(char c)
		{
			if (((c < 'a') || (c > 'z')) && ((c < 'A') || (c > 'Z')))
			{
				return (c == '_');
			}
			return true;
		}

		private static bool ValidMetadataNameChar(char c)
		{
			if ((((c < 'a') || (c > 'z')) && ((c < 'A') || (c > 'Z'))) && ((c < '0') || (c > '9')))
			{
				return (c == '_');
			}
			return true;
		}

		#endregion

		#region NOT USED Methods

		//public static void InvokeFinalizer(object instance)
		//{
		//    InvokeFinalizer(instance.GetType(), instance);
		//}

		//public static void InvokeFinalizer(Type superType, object instance)
		//{
		//    superType.FinalizeRuntimeInstance(instance);
		//}

		#endregion
	}


}
