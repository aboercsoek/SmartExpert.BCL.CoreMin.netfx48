//--------------------------------------------------------------------------
// File:    DictionaryXmlSerializerHelper.cs
// Content:	Implementation of class DictionaryXmlSerializerHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Serialization
{

	/// <summary>
	/// The dictionary XML serializer helper class
	/// </summary>
	internal static class DictionaryXmlSerializerHelper
	{
		/// <summary>
		/// Deserializes an XML document into a Dictionary object using the specified System.Xml.XmlReader
		/// </summary>
		/// <param name="reader">The System.xml.XmlReader used to read the XML document.</param>
		/// <param name="dic">The Dictionary instance that should be deserialized</param>
		public static void Deserialize<TKey,TValue>( System.Xml.XmlReader reader, IDictionary<TKey,TValue> dic )
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();

			if ( wasEmpty )
				return;

			while ( reader.NodeType != System.Xml.XmlNodeType.EndElement )
			{
				reader.ReadStartElement("item");

				reader.ReadStartElement("key");
				TKey key = (TKey)keySerializer.Deserialize(reader);
				reader.ReadEndElement();

				reader.ReadStartElement("value");
				TValue value = (TValue)valueSerializer.Deserialize(reader);
				reader.ReadEndElement();

				dic.Add(key, value);

				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		/// <summary>
		/// Serializes a Dictionary object into a XML document using the specified System.Xml.XmlWriter
		/// </summary>
		/// <param name="writer">The System.xml.XmlWriter used to write the XML document.</param>
		/// <param name="dic">The Dictionary instance that should be serialized</param>
		public static void Serialize<TKey, TValue>( System.Xml.XmlWriter writer, IDictionary<TKey, TValue> dic )
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			foreach ( TKey key in dic.Keys )
			{
				writer.WriteStartElement("item");

				writer.WriteStartElement("key");
				keySerializer.Serialize(writer, key);
				writer.WriteEndElement();

				writer.WriteStartElement("value");
				TValue value = dic[key];
				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();

				writer.WriteEndElement();
			}
		}

	}
}
