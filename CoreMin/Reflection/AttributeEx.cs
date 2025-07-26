//--------------------------------------------------------------------------
// File:    AttributeEx.cs
// Content:	Implementation of class AttributeEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Reflection
{
	///<summary>Provides various Attribute reflection helper methods.</summary>
	public static class AttributeEx
	{

		#region Has Attribute extensions

		/// <summary>
		/// Determines whether the specified reflection type has the custom attribute of type T.
		/// </summary>
		/// <typeparam name="T">Type of the the custom attribute</typeparam>
		/// <param name="self">The custom attribute provider.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified reflection type has the custom attribute of type T; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasAttribute<T>(this ICustomAttributeProvider self)
		{
			return self.HasAttribute(typeof(T), true);
		}

		/// <summary>
		/// Determines whether the specified reflection type has the custom attribute of type T.
		/// </summary>
		/// <typeparam name="T">Type of the the custom attribute</typeparam>
		/// <param name="self">The custom attribute provider.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified reflection type has the custom attribute of type T; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasAttribute<T>(this ICustomAttributeProvider self, bool inherit)
		{
			return self.HasAttribute(typeof(T), inherit);
		}

		/// <summary>
		/// Determines whether the specified reflection type has the custom attribute of type T.
		/// </summary>
		/// <param name="self">The custom attribute provider.</param>
		/// <param name="attributeType">Type of the the custom attribute.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified reflection type has the custom attribute of type attributeType; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasAttribute(this ICustomAttributeProvider self, Type attributeType)
		{
			return self.HasAttribute(attributeType, true);
		}

		/// <summary>
		/// Determines whether the specified reflection type has the custom attribute of type T.
		/// </summary>
		/// <param name="self">The custom attribute provider.</param>
		/// <param name="attributeType">Type of the the custom attribute.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified reflection type has the custom attribute of type attributeType; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasAttribute(this ICustomAttributeProvider self, Type attributeType, bool inherit)
		{
			return (self == null) ? false : self.GetCustomAttributes(attributeType, inherit).Length > 0;
		}

		#endregion

		#region HasXml...Attribute Methods

		/// <summary>
		/// Determines whether the specified type has a XmlRoot attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type  has a XmlRoot attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlRootAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlRootAttribute>(false);
		}

		/// <summary>
		/// Determines whether the specified type has a XmlElement attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type has a XmlElement attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlElementAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlElementAttribute>(false);
		}

		/// <summary>
		/// Determines whether the specified type has a XmlEnum attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type has a XmlEnum attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlEnumAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlEnumAttribute>(false);
		}

		/// <summary>
		/// Determines whether the specified type has a XmlArray attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type has a XmlArray attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlArrayAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlArrayAttribute>(false);
		}

		/// <summary>
		/// Determines whether the specified type has a XmlArrayItem attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type has a XmlArrayItem attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlArrayItemAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlArrayItemAttribute>(false);
		}

		/// <summary>
		/// Determines whether the specified type has a XmlAttribute attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type has a XmlAttribute attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasXmlAttributeAttribute(this Type type)
		{
			return (type == null) ? false : type.HasAttribute<XmlAttributeAttribute>(false);
		}

		#endregion

		#region Get Attribute Methods

		/// <summary>
		/// Get the attribute instance declared on <paramref name="target"/> with the specified <paramref name="attributeFullName"/>.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="attributeFullName">Full name of the attribute.</param>
		/// <returns>The attribute instance or <see langword="null"/> if no attribute with the given <paramref name="attributeFullName"/> was found.</returns>
		public static Attribute GetAttribute(this ICustomAttributeProvider target, string attributeFullName)
		{
			ArgChecker.ShouldNotBeNull(target, "target");
			ArgChecker.ShouldNotBeNullOrEmpty(attributeFullName, "attributeFullName");

			return target.GetCustomAttributes(true)
				.FirstOrDefault(attribute => attribute.GetType().FullName == attributeFullName).As<Attribute>();
		}

		/// <summary>
		/// Returns the custom attribute of type T defined on this member.
		/// </summary>
		/// <typeparam name="T">Type of the the custom attribute</typeparam>
		/// <param name="self">The custom attribute provider.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>
		/// 	The first custom attribute of type T defined on this member or <see langword="null" /> if no custom attribute of type T is defined on this member.
		/// </returns>
		/// <seealso cref="TryGetAttribute{T}"/>
		/// <seealso cref="GetAttributes{T}"/>
		public static T GetAttribute<T>(this ICustomAttributeProvider self, bool inherit)
		{
			return (self == null) ? default(T) : (T)self.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
		}

		/// <summary>
		/// Tries to get the attribute of type <typeparamref name="T"/> defined on <paramref name="self"/>.
		/// </summary>
		/// <typeparam name="T">The attribute type to search for.</typeparam>
		/// <param name="self">The type or member to get the attribute from.</param>
		/// <param name="attribute">[REF] The reference to the found attribute (<see langword="null"/> if not found).</param>
		/// <returns><see langword="true"/> if the attribute of type <typeparamref name="T"/> was found; otherwise <see langword="false"/>.</returns>
		/// <seealso cref="GetAttribute{T}"/>
		/// <seealso cref="GetAttributes{T}"/>
		public static bool TryGetAttribute<T>(this ICustomAttributeProvider self, out T attribute) where T : Attribute
		{
			attribute = self.GetCustomAttributes(typeof(T), true).FirstOrDefault().As<T>();
			return (attribute != null);
		}

		/// <summary>
		/// Returns the custom attributes of type T defined on this member.
		/// </summary>
		/// <typeparam name="T">Type of the the custom attribute</typeparam>
		/// <param name="self">The custom attribute provider.</param>
		/// <param name="inherit">When true, look up the hierarchy chain for the inherited custom attribute.</param>
		/// <returns>
		/// 	The custom attributes of type T defined on this member.
		/// </returns>
		/// <seealso cref="GetAttribute{T}"/>
		/// <seealso cref="TryGetAttribute{T}"/>
		public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider self, bool inherit)
		{

			return (self == null) ? Enumerable.Empty<T>() : self.GetCustomAttributes(typeof(T), inherit).Select(attr => (T)attr);
		}

		/// <summary>
		/// Get all the attribute instances declared on <paramref name="target"/> that match the given <paramref name="attributeFullName"/>.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="attributeFullName">Full name of the attribute.</param>
		/// <returns>The attribute instances or <see langword="null"/> if no attributes with the given <paramref name="attributeFullName"/> were found.</returns>
		public static IEnumerable<Attribute> GetAttributes(this ICustomAttributeProvider target, string attributeFullName)
		{
			ArgChecker.ShouldNotBeNull(target, "target");
			ArgChecker.ShouldNotBeNullOrEmpty(attributeFullName, "attributeFullName");

			return target.GetCustomAttributes(true)
				.Where(attr => attr.GetType().FullName == attributeFullName)
				.Select(attr => attr.As<Attribute>());
		}

		#endregion

		#region IsAttributeDefined Methods

		/// <summary>
		/// Determines whether a attribute is declared on the specified target.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="attributeFullName">Full name of the attribute.</param>
		/// <returns>
		///   <see langword="true"/> if the attribute is defined on the specified target; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsAttributeDefined(this ICustomAttributeProvider target, string attributeFullName)
		{
			ArgChecker.ShouldNotBeNull(target, "target");
			ArgChecker.ShouldNotBeNullOrEmpty(attributeFullName, "attributeFullName");

			Type attributeType = Type.GetType(attributeFullName, false, false);

			ArgChecker.ShouldNotBeNull(attributeType, "attributeFullName");

			return (GetAttribute(target, attributeFullName) != null);
		}

		/// <summary>
		/// Determines whether a attribute is declared on the specified member.
		/// </summary>
		/// <typeparam name="TAttribute">The type of the attribute.</typeparam>
		/// <param name="member">The member to check.</param>
		/// <param name="reflectionOnlyContext">if set to <see langword="true"/> check attribute in reflection only context.</param>
		/// <returns>
		///  <see langword="true"/> if the attribute is defined on the specified member; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsAttributeDefined<TAttribute>(this MemberInfo member, bool reflectionOnlyContext)
		{
			if (reflectionOnlyContext)
			{
				return CustomAttributeData.GetCustomAttributes(member).Any(data => data.Constructor.DeclaringType == typeof(TAttribute));
			}

			return IsAttributeDefined(member, typeof(TAttribute).FullName);
		}

		#endregion

		#region ServiceContract & DataContract related Methods

		/// <summary>
		/// Determines whether the specified type has a service contract attribute.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		///   <see langword="true"/> if the specified type or one of his implemented interfaces has a service contract attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasServiceContractAttribute(this Type type)
		{
			if (type.IsDefined(typeof (ServiceContractAttribute), false))
				return true;

			return type.GetInterfaces().Any(type2 => type2.IsDefined(typeof (ServiceContractAttribute), false));
		}

		/// <summary>
		/// Determines whether the specified type is a data contract.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified type is a data contract; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasDataContractAttribute(this Type type)
		{
			return (type != null) && type.HasAttribute<DataContractAttribute>(false);
		}

		/// <summary>
		/// Gets the name of the service contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The name of the service contract.</returns>
		public static string GetServiceContractName(this Type type)
		{
			ServiceContractAttribute serviceContract;

			if (type.TryGetAttribute(out serviceContract) && !String.IsNullOrEmpty(serviceContract.Name))
				return serviceContract.Name;

			return type.Name;
		}

		/// <summary>
		/// Gets the XML namespace of the service contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The XML namespace of the service contract.</returns>
		public static string GetServiceContractNamespace(this Type type)
		{
			ServiceContractAttribute serviceContract;

			if (type.TryGetAttribute(out serviceContract) && serviceContract.Namespace != null)
				return serviceContract.Namespace;

			return "http://tempuri.org/";
		}

		/// <summary>
		/// Gets the name of the data contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The name of the data contract.</returns>
		public static string GetDataContractName(this Type type)
		{
			return DataContractTypeInfo.Generate(type).Name;
		}

		/// <summary>
		/// Gets the XML namespace of the data contract.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The XML namespace of the data contract.</returns>
		public static string GetDataContractNamespace(this Type type)
		{
			return DataContractTypeInfo.Generate(type).TypeNamespace;
		}

		/// <summary>
		/// Determines whether the specified type is a WCF service class..
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		///   <see langword="true"/> if the specified type is a WCF service class; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsWcfServiceClass(this Type type)
		{
			if (!type.IsClass)
				return false;

			return (type.HasServiceContractAttribute() && !type.IsDerivedFrom(typeof(ClientBase<>)));
		}


		#endregion

	}
}
