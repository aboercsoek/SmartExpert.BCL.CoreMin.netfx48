//--------------------------------------------------------------------------
// File:    DataContractTypeInfo.cs
// Content:	Implementation of class DataContractTypeInfo
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>DataContract information provider.</summary>
	public class DataContractTypeInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataContractTypeInfo"/> class.
		/// </summary>
		public DataContractTypeInfo()
		{
			Name = string.Empty;
			TypeNamespace = string.Empty;
		}

		private DataContractTypeInfo(string name, string typeNamespace)
		{
			Name = name;
			TypeNamespace = typeNamespace;
		}

		/// <summary>
		/// Gets the data contract name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the data contract namespace.
		/// </summary>
		public string TypeNamespace { get; private set; }

		/// <summary>
		/// Generates the data contract info for the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The data contract info for the specified type.</returns>
		public static DataContractTypeInfo Generate(Type type)
		{
			ArgChecker.ShouldNotBeNull(type, "type");

			try
			{
				if (type.FullName.SafeString().StartsWith("System.")) 
					return new DataContractTypeInfo();

				if (type.ContainsGenericParameters) 
					return new DataContractTypeInfo();
				
				var serializer = new DataContractSerializer(type);
				var objectStream = new MemoryStream();

				object instance = type.IsArray ? Activator.CreateInstance(type, new object[] { 0 }) : Activator.CreateInstance(type);

				serializer.WriteObject(objectStream, instance);
				objectStream.Position = 0;
				var xDocument = XDocument.Load(new XmlTextReader(objectStream));

				return xDocument.Root == null
				       	? new DataContractTypeInfo()
				       	: new DataContractTypeInfo(xDocument.Root.Name.LocalName, xDocument.Root.Name.NamespaceName);
			}
			catch (Exception exception)
			{
				if (exception.IsFatal())
					throw;

				throw new OperationExecutionFailedException(
					string.Format("Unable to determine Data Contract information for type '{0}': {1}", type.FullName, exception.Message),
					exception);
			}
		}
	}
}
