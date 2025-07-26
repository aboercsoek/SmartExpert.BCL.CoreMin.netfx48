//--------------------------------------------------------------------------
// File:    ISerializationFormatter.cs
// Content:	Definition of interface ISerializationFormatter
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
	/// Defines an object that can serialize and deserialize object graphs.
	/// </summary>
	public interface ISerializationFormatter
	{
		/// <summary>
		/// Gets a value indicating whether the serialization formatter support deserialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if deserialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		bool SupportDeserializationWithoutTypeInformation { get; }

		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <returns>A deserialized object graph.</returns>
		object Deserialize( System.IO.Stream serializationStream );
		
		/// <summary>
		/// Converts a serialization stream into an object graph.
		/// </summary>
		/// <param name="serializationStream">Byte stream containing the serialized data.</param>
		/// <typeparam name="T">Type that should be deserialized form stream.</typeparam>
		/// <returns>A deserialized object graph.</returns>
		T Deserialize<T>( System.IO.Stream serializationStream );

		/// <summary>
		/// Gets a value indicating whether the serialization formatter support serialization without type information.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if serialization without type information is supported; otherwise, <see langword="false"/>.
		/// </value>
		bool SupportSerializationWithoutTypeInformation { get; }

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		void Serialize( System.IO.Stream serializationStream, object graph );

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		void Serialize<T>(System.IO.Stream serializationStream, T graph);

		/// <summary>
		/// Converts an object graph into a byte stream.
		/// </summary>
		/// <param name="serializationStream">Stream that will contain the the serialized data.</param>
		/// <param name="graph">Object graph to be serialized.</param>
		/// <param name="closeStream">
		/// true - toStream is closed after serialization.
		/// false - toStream is still open after serialization
		/// </param>
		void Serialize<T>(System.IO.Stream serializationStream, T graph, bool closeStream);
	}
}
