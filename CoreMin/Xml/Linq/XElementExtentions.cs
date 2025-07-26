//--------------------------------------------------------------------------
// File:    XLinqHelper.cs
// Content:	Implementation of class XLinqHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Collections.Generic;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as XElement therewith the extension methods are available by using the System.Xml.Linq namespace.
namespace SmartExpert.Xml.Linq
{
	///<summary>Represents extension methods for <see cref="XElement"/> type.</summary>
	public static class XElementExtensions
	{
		/// <summary>
		/// Checks if the attribute value from a <see cref="XElement"/> node is set to true.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns true is the atttribute value is set to true. 
		/// If the attribute does not exists or the value is not a bool value false will be returned.
		/// </returns>
		public static bool IsAttrTrue(this XElement element, XName attrName)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr != null && StringParser.ParseBool(attr.Value, false);
		}

		/// <summary>
		/// Checks if the attribute value from a <see cref="XElement"/> node is set to false.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns true is the atttribute value is set to false. If the attribute does not exists or the value is not a bool value false will be returned.
		/// </returns>
		public static bool IsAttrFalse(this XElement element, XName attrName)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr != null && StringParser.ParseBool(attr.Value, true).IsFalse();
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as bool value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as bool. If <paramref name="element"/> has no attribute with the given name, false is returned.
		/// </returns>
		public static bool AttrAsBool(this XElement element, XName attrName)
		{
			return element.AttrAsBool(attrName, false);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as bool value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as bool. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static bool AttrAsBool(this XElement element, XName attrName, bool defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseBool(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Int32 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as Int32. If <paramref name="element"/> has no attribute with the given name, 0 is returned.
		/// </returns>
		public static int AttrAsInt32(this XElement element, XName attrName)
		{
			return element.AttrAsInt32(attrName, 0);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Int32 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as Int32. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static int AttrAsInt32(this XElement element, XName attrName, int defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseInt32(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Int64 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as Int64. If <paramref name="element"/> has no attribute with the given name, 0 is returned.
		/// </returns>
		public static long AttrAsInt64(this XElement element, XName attrName)
		{
			return element.AttrAsInt64(attrName, 0L);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Int64 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as Int64. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static long AttrAsInt64(this XElement element, XName attrName, long defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseInt64(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as UInt64 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as UInt64. If <paramref name="element"/> has no attribute with the given name, 0 is returned.
		/// </returns>
		public static ulong AttrAsUInt64(this XElement element, XName attrName)
		{
			return element.AttrAsUInt64(attrName, 0UL);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as UInt64 value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as UInt64. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static ulong AttrAsUInt64(this XElement element, XName attrName, ulong defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseUInt64(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Float value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as Float. If <paramref name="element"/> has no attribute with the given name, 0 is returned.
		/// </returns>
		public static float AttrAsFloat(this XElement element, XName attrName)
		{
			return element.AttrAsFloat(attrName, 0f);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Float value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as Float. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static float AttrAsFloat(this XElement element, XName attrName, float defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseFloat(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Double value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as Double. If <paramref name="element"/> has no attribute with the given name, 0 is returned.
		/// </returns>
		public static double AttrAsDouble(this XElement element, XName attrName)
		{
			return element.AttrAsDouble(attrName, 0.0);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Double value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as Double. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static double AttrAsDouble(this XElement element, XName attrName, double defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseDouble(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Guid value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as Guid. If <paramref name="element"/> has no attribute with the given name, Guid.Empty is returned.
		/// </returns>
		public static Guid AttrAsGuid(this XElement element, XName attrName)
		{
			return element.AttrAsGuid(attrName, Guid.Empty);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as Guid value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as Guid. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static Guid AttrAsGuid(this XElement element, XName attrName, Guid defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseGuid(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as DateTime value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as DateTime. If <paramref name="element"/> has no attribute with the given name, DateTime.MinValue is returned.
		/// </returns>
		public static DateTime AttrAsDateTime(this XElement element, XName attrName)
		{
			return element.AttrAsDateTime(attrName, DateTime.MinValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as DateTime value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="dateTimeOption">The DateTime serialization mode.</param>
		/// <returns>
		/// Returns the attribute value as DateTime. If <paramref name="element"/> has no attribute with the given name, DateTime.MinValue is returned.
		/// </returns>
		public static DateTime AttrAsDateTime(this XElement element, XName attrName, XmlDateTimeSerializationMode dateTimeOption)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? DateTime.MinValue : StringParser.ParseDateTime(attr.Value, dateTimeOption);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as DateTime value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as DateTime. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static DateTime AttrAsDateTime(this XElement element, XName attrName, DateTime defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			return attr == null ? defaultValue : StringParser.ParseDateTime(attr.Value, defaultValue);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as value of type T.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as type T. 
		/// If <paramref name="element"/> has no attribute with the given name or the attribute value can not be converted to type T, 
		/// the default value is returned. 
		/// The method uses <see cref="System.Convert.ChangeType(object,Type,IFormatProvider)"/> to convert the attribute string into type T.
		/// </returns>
		public static T AttrAsT<T>(this XElement element, XName attrName, T defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = element.Attribute(attrName);
			T local = defaultValue;
			
			if (attr != null)
			{
				try
				{
					local = (T)Convert.ChangeType(attr.Value, typeof(T), NumberFormatInfo.InvariantInfo);
				}
				catch (ArgumentNullException)
				{
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}

			return local;
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as string value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as string. If <paramref name="element"/> has no attribute with the given name, string.Empty is returned.
		/// </returns>
		public static string AttrAsSafeStr(this XElement element, XName attrName)
		{
			return element.AttrAsStr(attrName, string.Empty);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as string value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns the attribute value as string. If <paramref name="element"/> has no attribute with the given name, null is returned.
		/// </returns>
		public static string AttrAsStr(this XElement element, XName attrName)
		{
			return element.AttrAsStr(attrName, null);
		}

		/// <summary>
		/// Gets the attribute value from a <see cref="XElement"/> node as string value.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="defaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value as string. If <paramref name="element"/> has no attribute with the given name, the default value is returned.
		/// </returns>
		public static string AttrAsStr(this XElement element, XName attrName, string defaultValue)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			return element.GetAttributeValue(attrName, defaultValue);
		}

		
		/// <summary>
		/// Gets the attribute value form a <see cref="XElement"/> node.
		/// </summary>
		/// <param name="xElement">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="attrDefaultValue">The attribute default value.</param>
		/// <returns>
		/// Returns the attribute value. <paramref name="xElement"/> has no attribute with the given name, the method returns the default value.
		/// </returns>
		public static string GetAttributeValue(this XElement xElement, XName attrName, string attrDefaultValue)
		{
			ArgChecker.ShouldNotBeNull(xElement, "xElement");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			var attr = xElement.Attribute(attrName);
			return attr == null ? attrDefaultValue : attr.Value;
		}

		/// <summary>
		/// Gets the attribute value form a <see cref="XElement"/> node.
		/// </summary>
		/// <typeparam name="T">The attribute target type.</typeparam>
		/// <param name="xElement">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="converter">The attribute type converter.</param>
		/// <returns>
		/// Returns the attribute value.
		/// </returns>
		/// <remarks>Uses the <paramref name="converter"/> to convert the attribute string value into the attribute target type.</remarks>
		public static T GetAttributeValue<T>(this XElement xElement, XName attrName, Func<string, T> converter)
		{
			ArgChecker.ShouldNotBeNull(xElement, "xElement");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");
			ArgChecker.ShouldNotBeNull(converter, "converter");

			var attr = xElement.Attribute(attrName);
			if (attr == null)
			{
				throw new ArgException<XName>(attrName, "attrName", "XML Attribute was not found in XML element \"{0}\"".SafeFormatWith(xElement.Name));
			}
			string v = attr.Value;
			return converter(v);
		}

		/// <summary>
		/// Gets the attribute value form a <see cref="XElement"/> node.
		/// </summary>
		/// <typeparam name="T">The attribute target type.</typeparam>
		/// <param name="xElement">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <param name="attrDefaultValue">The attribute default value.</param>
		/// <param name="converter">The attribute type converter.</param>
		/// <returns>
		/// Returns the attribute value or the default value, if the attribute does not exist.
		/// </returns>
		/// <remarks>Uses the <paramref name="converter"/> to convert the attribute string value into the attribute target type.</remarks>
		public static T GetAttributeValue<T>(this XElement xElement, XName attrName, T attrDefaultValue, Func<string, T> converter)
		{
			ArgChecker.ShouldNotBeNull(xElement, "xElement");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");
			ArgChecker.ShouldNotBeNull(converter, "converter");

			var attr = xElement.Attribute(attrName);
			if (attr == null)
			{
				return attrDefaultValue;
			}
			string v = attr.Value;
			return converter(v);
		}

		/// <summary>
		/// Checks if  the <see cref="XElement"/> node has a attribute with the specified name.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <param name="attrName">The Name of the XML attribute.</param>
		/// <returns>
		/// Returns <see langword="true"/> if <para>element</para> has a attribute with the given name, otherwise <see langword="false"/>.
		/// </returns>
		public static bool HasAttribute(this XElement element, XName attrName)
		{
			ArgChecker.ShouldNotBeNull(element, "element");
			ArgChecker.ShouldNotBeNull(attrName, "attrName");

			if (element.HasAttributes == false)
				return false;

			return (element.Attribute(attrName) != null);
		}

		/// <summary>
		/// Checks if the attribute with the specified name has the same value in the source and target element.
		/// </summary>
		/// <param name="source">The source element.</param>
		/// <param name="target">The target element.</param>
		/// <param name="attrName">The attribute name to check.</param>
		/// <returns>true if the attribute exists in both elements and has the same value, otherwise false.</returns>
		public static bool AttributeEqual(this XElement source, XElement target, XName attrName)
		{
			if ((source == null) || (target == null) || (attrName == null))
				return false;

			if (source.HasAttributes == false)
				return false;

			if (target.HasAttributes == false)
				return false;

			var sourceAttr = source.Attribute(attrName);
			if (sourceAttr == null)
				return false;

			var targetAttr = target.Attribute(attrName);
			if (targetAttr == null)
				return false;

			return (sourceAttr.Value == targetAttr.Value);
		}

		/// <summary>
		/// Checks if all attribute names and values of the source and target element are equal.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="target">The target.</param>
		/// <returns>true if all attributes names and values are equal, otherwise false.</returns>
		public static bool AttributesEqual(this XElement source, XElement target)
		{
			if ((source == null) || (target == null))
				return false;
			
			if ((source.HasAttributes == false) && (target.HasAttributes == false))
				return true;

			if (source.HasAttributes == false)
				return false;

			if (target.HasAttributes == false)
				return false;

			if (source.Attributes().Count() != target.Attributes().Count())
				return false;

			bool allAttributesMatches = true;
			XAttribute sourceAttr = source.FirstAttribute;
			while (sourceAttr != null)
			{
				var targetAttr = target.Attribute(sourceAttr.Name);
				if (targetAttr == null)
				{
					allAttributesMatches = false;
					break;
				}
				if (targetAttr.Value != sourceAttr.Value)
				{
					allAttributesMatches = false;
					break;
				}
				sourceAttr = sourceAttr.NextAttribute;
			}

			return allAttributesMatches;
		}

		/// <summary>
		/// Determines whether the source and target element are equal (contents, attribute names and values).
		/// </summary>
		/// <param name="source">The source element.</param>
		/// <param name="target">The target element.</param>
		/// <returns>
		///   <see langword="true"/> if the source and target element are equal; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsDeepEquals(this XElement source, XElement target)
		{
			if (source == null && target == null)
				return true;
			if (source == null)
				return false;
			if (target == null)
				return false;

			return XNode.DeepEquals(source, target);
		}

		/// <summary>
		/// Gets the deep hash code of the xml element.
		/// </summary>
		/// <param name="element">The xml element.</param>
		/// <returns>
		/// Returns the deep hash code value, or 0 if the element is null.
		/// </returns>
		public static int GetDeepHashCode(this XElement element)
		{
			if (element == null)
				return 0;
			
			int num = element.Name.GetHashCode() ^ element.Value.GetHashCode();

			if (element.HasAttributes)
			{
				foreach (var attr in element.Attributes())
					num ^= attr.GetDeepHashCode();
			}

			return num;
		}

		

		/// <summary>
		/// Safe Element Value.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>Returns the Value of the XElement if the XElement is not null, otherwise string.Empty.</returns>
		public static string SafeValue(this XElement element)
		{
			return (element == null) ? string.Empty : element.Value;
		}

		/// <summary>
		/// Checks if the element value is true.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <returns>
		/// Returns true is the element value is true. If the element is null or the value is not a bool value false will be returned.
		/// </returns>
		public static bool IsValueTrue(this XElement element)
		{
			if (element == null)
				return false;

			return StringParser.ParseBool(element.Value, false);
		}

		/// <summary>
		/// Checks if the element value is false.
		/// </summary>
		/// <param name="element">The XML element node.</param>
		/// <returns>
		/// Returns true is the element value is false. If the element is null or the value is not a bool value false will be returned.
		/// </returns>
		public static bool IsValueFalse(this XElement element)
		{
			if (element == null)
				return false;

			return !StringParser.ParseBool(element.Value, true);
		}

		/// <summary>
		/// Walks through the specified XML element.
		/// </summary>
		/// <param name="xmlElement">The XML element.</param>
		/// <returns>A IEnumerable-Collection of XElement walk events.</returns>
		public static IEnumerable<TreeWalker.WalkEvent<XElement>> Walk(this XElement xmlElement)
		{
			return TreeWalker.Walk(xmlElement, n => n.Elements());
		}
	}
}
