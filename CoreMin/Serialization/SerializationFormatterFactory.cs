//--------------------------------------------------------------------------
// File:    SerializationFormatterFactory.cs
// Content:	Implementation of class SerializationFormatterFactory
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Serialization
{
	/// <summary>
	/// Factory used to create the appropriate
	/// serialization formatter object based
	/// on the application configuration.
	/// </summary>
	public static class SerializationFormatterFactory
	{
		// <summary>
		// Creates a serialization formatter object.
		// </summary>
		//public static ISerializationFormatter GetFormatterFromApplicationContext()
		//{
		//    return GetFormatter(ApplicationContext.SerializationFormatter);
		//}

		/// <summary>
		/// Creates a serialization formatter object.
		/// </summary>
		public static ISerializationFormatter GetFormatterFromTypeDetection<T>()
		{
			return GetFormatter(DetectSerializer(typeof(T)));
		}

		/// <summary>
		/// Creates a serialization formatter object.
		/// </summary>
		/// <param name="formatter">The formatter that should be returned.</param>
		public static ISerializationFormatter GetFormatter(string formatter )
		{
			return GetFormatter((SerializationFormatters)Enum.Parse(typeof(SerializationFormatters), formatter));
		}

		/// <summary>
		/// Creates a serialization formatter object.
		/// </summary>
		/// <param name="formatter">The formatter that should be returned.</param>
		public static ISerializationFormatter GetFormatter(SerializationFormatters formatter )
		{
			ISerializationFormatter result = null;

			switch (formatter)
			{
				case SerializationFormatters.XmlSerializer:
					result = new XmlSerializerWrapper();
					break;
				case SerializationFormatters.BinaryFormatter:
					result = new BinaryFormatterWrapper();
					break;
				case SerializationFormatters.DataContractSerializer:
					result = new DataContractSerializerWrapper();
					break;
				case SerializationFormatters.NetDataContractSerializer:
					result = new NetDataContractSerializerWrapper();
					break;
				default:
					throw new ArgumentOutOfRangeException("formatter");
			}

			return result;
		}

		/// <summary>
		/// Informs the caller about the Serializer that should be
		/// used to serialize a specific type or object
		/// </summary>
		/// <param name="type">Type that should be serialized</param>
		/// <returns><see cref="SerializationFormatters"/> object.</returns>
		public static SerializationFormatters DetectSerializer(Type type)
		{
			#region PreConditions

			if (type == null)
				throw new ArgumentNullException("type");

			#endregion

			SerializationFormatters result = SerializationFormatters.BinaryFormatter;

			object[] customAttributes = type.GetCustomAttributes(false);
			foreach (object customAttribute in customAttributes)
			{
				Type caType = customAttribute.GetType();
				if ((caType.Name == "DataContractAttribute") ||
				    (caType.Name == "SerializableAttribute"))
				{
					result = SerializationFormatters.DataContractSerializer;
					break;
				}

				if ( caType.Name == "XmlRootAttribute" )
				{
					result = SerializationFormatters.XmlSerializer;
					break;
				}
			}
			return result;
		}
	}
}
