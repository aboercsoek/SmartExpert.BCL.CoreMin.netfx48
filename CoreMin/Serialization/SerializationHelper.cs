//--------------------------------------------------------------------------
// File:    SerializationHelper.cs
// Content:	Implementation of class SerializationHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Runtime.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Serialization
{
	///<summary>Provides Serialization helper methods</summary>
	public static class SerializationHelper
	{
		/// <summary>
		/// Determines whether <paramref name="type"/> has a <see cref="System.Runtime.Serialization.DataContractAttribute">DataContract</see> attribute.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<see langword="true"/> if the type has a DataContract attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasDataContractAttribute(Type type)
		{
			#region PreConditions

			if ( type == null )
				throw new ArgumentNullException("type");

			#endregion

			object[] customAttributes = type.GetCustomAttributes(false);
			foreach ( object customAttribute in customAttributes )
			{
				Type caType = customAttribute.GetType();
				if ( caType.Name == "DataContractAttribute" )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the XML namespace specified in the 
		/// <see cref="System.Runtime.Serialization.DataContractAttribute">DataContract</see> attribute of the given type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// The XML namespace if the type has a <see cref="System.Runtime.Serialization.DataContractAttribute">DataContract</see> attribute; 
		/// otherwise an empty string.
		/// </returns>
		public static string GetXmlNamespaceFromDataContractAttribute( Type type )
		{
			#region PreConditions

			if ( type == null )
				throw new ArgumentNullException("type");

			#endregion

			object[] customAttributes = type.GetCustomAttributes(false);
			foreach ( object customAttribute in customAttributes )
			{
				Type caType = customAttribute.GetType();
				if ( caType.Name == "DataContractAttribute" )
				{
					DataContractAttribute dca = customAttribute as DataContractAttribute;
					if ( dca == null ) return string.Empty;
					return dca.Namespace;
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Determines whether <paramref name="type"/> has a <see cref="System.SerializableAttribute">Serializable</see> attribute.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<see langword="true"/> if the type has a Serializable attribute; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool HasSerializableAttribute( Type type )
		{
			#region PreConditions

			if ( type == null )
				throw new ArgumentNullException("type");

			#endregion

			object[] customAttributes = type.GetCustomAttributes(false);
			foreach ( object customAttribute in customAttributes )
			{
				Type caType = customAttribute.GetType();
				if ( caType.Name == "SerializableAttribute" )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether <paramref name="type"/> has a <see cref="System.Xml.Serialization.XmlRootAttribute">XmlRoot</see> attribute.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<see langword="true"/> if the type has a XmlRoot attribute; otherwise <see langword="false"/>.
		/// </returns>
		public static bool HasXmlRootAttribute( Type type )
		{
			#region PreConditions

			if ( type == null )
				throw new ArgumentNullException("type");

			#endregion

			object[] customAttributes = type.GetCustomAttributes(false);
			foreach ( object customAttribute in customAttributes )
			{
				Type caType = customAttribute.GetType();
				if ( caType.Name == "XmlRootAttribute" )
				{
					return true;
				}
			}
			return false;
		}
	}
}
