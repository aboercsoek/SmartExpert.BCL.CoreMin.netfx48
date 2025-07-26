//--------------------------------------------------------------------------
// File:    NetDataContractSerializerWrapper.cs
// Content:	Implementation of class NetDataContractSerializerWrapper
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
	/// Wraps the <see cref="NetDataContractSerializer"/> in the <see cref="ISerializationFormatter"/>
	/// interface so it can be used in a standardized manner.
	internal class NetDataContractSerializerWrapper : ISerializationFormatter
	{
		private NetDataContractSerializer m_Formatter = new NetDataContractSerializer();

		#region ISerializationFormatter Members

		/// <summary>
		/// Gets a value indicating whether the serialization formatter support deserialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if deserialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		public bool SupportDeserializationWithoutTypeInformation
		{
			get { return true; }
		}

		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <returns>A deserialized object graph.</returns>
		public object Deserialize( System.IO.Stream serializationStream )
		{
			return m_Formatter.Deserialize(serializationStream);
		}

		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <typeparam name="T">Type that should be deserialized form stream.</typeparam>
		/// <returns>A deserialized object graph.</returns>
		public T Deserialize<T>( System.IO.Stream serializationStream )
		{
			return (T)m_Formatter.Deserialize(serializationStream);
		}

		/// <summary>
		/// Gets a value indicating whether the serialization formatter support serialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if serialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		public bool SupportSerializationWithoutTypeInformation
		{
			get { return true; }
		}

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		public void Serialize( System.IO.Stream serializationStream, object graph )
		{
			m_Formatter.Serialize(serializationStream, graph);
		}

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		public void Serialize<T>( System.IO.Stream serializationStream, T graph )
		{
			m_Formatter.Serialize(serializationStream, graph);
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
			m_Formatter.Serialize(serializationStream, graph);
			if ( closeStream == true )
				serializationStream.Close();
		}

		#endregion
		
	}
}
