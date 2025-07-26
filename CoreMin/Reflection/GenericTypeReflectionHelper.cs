//--------------------------------------------------------------------------
// File:    GenericTypeReflectionHelper.cs
// Content:	Implementation of class GenericTypeReflectionHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;
using JetBrains.Annotations;

#endregion

namespace SmartExpert.Reflection
{
	/// <summary>
	/// A helper class that encapsulate details of the reflection API, particularly around generics.
	/// </summary>
	public class GenericTypeReflectionHelper
	{
		private readonly Type m_TypeToReflect;

		/// <summary>
		/// Create a new <see cref="GenericTypeReflectionHelper" /> instance that
		/// lets you look at information about the given type.
		/// </summary>
		/// <param name="typeToReflect">Type to do reflection on.</param>
		public GenericTypeReflectionHelper(Type typeToReflect)
		{
			ArgChecker.ShouldNotBeNull(typeToReflect, "typeToReflect");

			m_TypeToReflect = typeToReflect;
		}

		/// <summary>
		/// Makes a closed generic type from the internal open generic type.
		/// </summary>
		/// <param name="typeArguments">The generic type arguments.</param>
		/// <returns>The closed generic type.</returns>
		public Type MakeGenericType(params Type[] typeArguments)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(typeArguments, "typeArguments");
			
			try
			{
				return GetClosedParameterType(typeArguments);
			}
			catch (ArgumentException exception)
			{
				throw new ArgumentException(FormatGenericTypeMessage(TypeToReflect, typeArguments), exception);
			}
			catch (TypeLoadException exception2)
			{
				throw new TypeLoadException(FormatGenericTypeMessage(TypeToReflect, typeArguments), exception2);
			}
		}

		/// <summary>
		/// If this type is an open generic, use the given <paramref name="genericArguments" /> array to
		/// determine what the required closed type is and return that.
		/// </summary>
		/// <remarks>If the parameter is not an open type, just return this parameter's type.</remarks>
		/// <param name="genericArguments">Type arguments to substitute in for the open type parameters.</param>
		/// <returns>Corresponding closed type of this parameter.</returns>
		public Type GetClosedParameterType(Type[] genericArguments)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(genericArguments, "genericArguments");

			if (TypeToReflect.IsOpenGenericType())
			{
				Type[] typeArgs = TypeToReflect.GetGenericArguments();
				for (int i = 0; i < typeArgs.Length; i++)
				{
					typeArgs[i] = genericArguments[typeArgs[i].GenericParameterPosition];
				}
				
				return TypeToReflect.GetGenericTypeDefinition().MakeGenericType(typeArgs);
			}

			if (TypeToReflect.IsGenericParameter)
			{
				return genericArguments[TypeToReflect.GenericParameterPosition];
			}

			if (IsArray.IsFalse() || ArrayElementType.IsGenericParameter.IsFalse())
			{
				return TypeToReflect;
			}

			int rank = TypeToReflect.GetArrayRank();
			if (rank == 1)
			{
				return genericArguments[ArrayElementType.GenericParameterPosition].MakeArrayType();
			}
			return genericArguments[ArrayElementType.GenericParameterPosition].MakeArrayType(rank);
		}

		/// <summary>
		/// Given a generic argument name, return the corresponding type for this closed type. 
		/// For example, if the current type is SomeType&lt;User&gt;, and the corresponding definition was SomeType&lt;TSomething&gt;, 
		/// calling this method and passing "TSomething" will return typeof(User).
		/// </summary>
		/// <param name="parameterName">Name of the generic parameter.</param>
		/// <returns>Type of the corresponding generic parameter, or null if there is no matching name.</returns>
		public Type GetNamedGenericParameter(string parameterName)
		{
			Type openType = TypeToReflect.GetGenericTypeDefinition();
			Type result = null;
			int index = -1;
			
			if (openType == null)
				return null;

			foreach (Type genericArgumentType in openType.GetGenericArguments())
			{
				if (genericArgumentType.Name == parameterName)
				{
					index = genericArgumentType.GenericParameterPosition;
					break;
				}
			}

			if (index != -1)
			{
				result = TypeToReflect.GetGenericArguments()[index];
			}
			return result;
		}

		/// <summary>
		/// Test the given <see cref="T:System.Reflection.MethodBase" /> object, looking at the parameters. 
		/// Determine if any of the parameters are open generic types that need type attributes filled in.
		/// </summary>
		/// <param name="method">The method to check.</param>
		/// <returns><see langword="true"/> if any of the parameters are open generics. <see langword="false"/> if not.</returns>
		public static bool MethodHasOpenGenericParameters(MethodBase method)
		{
			ArgChecker.ShouldNotBeNull(method, "method");

			return method.GetParameters().Any(param => param.ParameterType.IsOpenGenericType());
		}

		/// <summary>
		/// The type of the elements in this type (if it's an array).
		/// </summary>
		public Type ArrayElementType
		{
			get
			{
				return IsArray ? m_TypeToReflect.GetElementType() : null;
			}
		}

		/// <summary>
		/// Is this type an array type?
		/// </summary>
		public bool IsArray
		{
			get
			{
				return m_TypeToReflect.IsArray;
			}
		}

		/// <summary>
		/// Is this type an array of generic elements?
		/// </summary>
		public bool IsGenericArray
		{
			get
			{
				return (IsArray && ArrayElementType.IsGenericParameter);
			}
		}

		/// <summary>
		/// Is this type generic?
		/// </summary>
		public bool IsGenericType
		{
			get
			{
				return m_TypeToReflect.IsGenericType;
			}
		}

		/// <summary>
		/// Is this type generic?
		/// </summary>
		public bool IsGenericTypeDefinition
		{
			get
			{
				return m_TypeToReflect.IsGenericTypeDefinition;
			}
		}

		/// <summary>
		/// Is this type an open generic (no type parameter specified)
		/// </summary>
		public bool IsOpenGeneric
		{
			get
			{
				return m_TypeToReflect.IsOpenGenericType();
			}
		}

		/// <summary>
		/// The <see cref="Type" /> object we're reflecting over.
		/// </summary>
		[NotNull]
		public Type TypeToReflect
		{
			get
			{
				return m_TypeToReflect;
			}
		}

		private static string FormatGenericTypeMessage(Type genericType, IEnumerable<Type> typeArguments)
		{
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

	}
}
