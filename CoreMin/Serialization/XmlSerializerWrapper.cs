//--------------------------------------------------------------------------
// File:    XmlSerializerWrapper.cs
// Content:	Implementation of class XmlSerializerWrapper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Serialization
{
	/// <summary>
	/// Wraps the <see cref="XmlSerializer"/> in the <see cref="ISerializationFormatter"/>
	/// interface so it can be used in a standardized manner.
	/// </summary>
	internal class XmlSerializerWrapper : ISerializationFormatter
	{

		#region ISerializationFormatter Members

		/// <summary>
		/// Gets a value indicating whether the serialization formatter support deserialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if deserialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		public bool SupportDeserializationWithoutTypeInformation
		{
			get { return false; }
		}

		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <returns>
		/// XmlSerializerWrapper does not support this method. <see cref="Deserialize"/> throws a NotSupportedException. Use <see cref="Deserialize{T}"/> instead.
		/// </returns>
		/// <exception cref="NotSupportedException">Is always thrown if method is called.</exception>
		public object Deserialize( System.IO.Stream serializationStream )
		{
			throw new NotSupportedException("XmlSerializerWrapper does not support deserialization without Type information");
		}

		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <typeparam name="T">Type that should be deserialized form stream.</typeparam>
		/// <returns>A deserialized object graph.</returns>
		public T Deserialize<T>( System.IO.Stream serializationStream )
		{
			T result = default(T);

			using ( XmlDictionaryReader xr = XmlDictionaryReader.CreateTextReader(serializationStream, XmlDictionaryReaderQuotas.Max) )
			{
				XmlSerializer dcSer = new XmlSerializer(typeof(T));
				result = (T)dcSer.Deserialize(xr);
			}

			return result;
		}

		/// <summary>
		/// Gets a value indicating whether the serialization formatter support serialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if serialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		public bool SupportSerializationWithoutTypeInformation
		{
			get { return false; }
		}

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		/// <remarks>XmlSerializerWrapper does not support this method. <see cref="Serialize"/> throws a NotSupportedException. Use <see cref="Serialize{T}(System.IO.Stream, T)"/> instead</remarks>
		/// <exception cref="NotSupportedException">Is always thrown if method is called.</exception>
		public void Serialize( System.IO.Stream serializationStream, object graph )
		{
			throw new NotSupportedException("XmlSerializerWrapper does not support serialization without Type information");
		}

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		/// <remarks><see cref="Serialize{T}(System.IO.Stream, T)"/> calls under the hood <see cref="Serialize{T}(System.IO.Stream, T, bool)"/> with closeStream equals false.</remarks>
		public void Serialize<T>( System.IO.Stream serializationStream, T graph )
		{
			Serialize<T>(serializationStream, graph, false);
		}

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		/// <param name="closeStream">
		/// true - toStream is closed after serialization.
		/// false - toStream is still open after serialization
		/// </param>
		public void Serialize<T>( System.IO.Stream serializationStream, T graph, bool closeStream )
		{
			using ( XmlDictionaryWriter xw = XmlDictionaryWriter.CreateTextWriter(serializationStream, Encoding.UTF8, closeStream) )
			{
				XmlSerializer dcSer = new XmlSerializer(typeof(T));
				dcSer.Serialize(xw, graph);
			}
		}

		#endregion

	}
}
