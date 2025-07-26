//--------------------------------------------------------------------------
// File:    WcfHelper.cs
// Content:	Implementation of class WcfHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Web
{
	///<summary>WCF utility class</summary>
	public static class WcfHelper
	{
		/// <summary>
		/// Combines two URIs.
		/// </summary>
		/// <param name="baseAddress">The base address.</param>
		/// <param name="relativeAddress">The relative address.</param>
		/// <returns>The combined URI</returns>
		public static Uri CombineUri(Uri baseAddress, Uri relativeAddress)
		{
			if (relativeAddress == null)
				return baseAddress;

			if (relativeAddress.IsAbsoluteUri)
				return relativeAddress;

			if (baseAddress.AbsoluteUri.EndsWith("/", StringComparison.Ordinal).IsFalse())
				baseAddress = new Uri(baseAddress.AbsoluteUri + "/");

			return new Uri(baseAddress, relativeAddress);
		}

		/// <summary>
		/// Finds the HTTP base address of a WCF service.
		/// </summary>
		/// <param name="serviceHostBaseAddresses">The WCF service addresses.</param>
		/// <returns>The base address of the WCF service or <see langword="null"/> if the list contains no base address</returns>
		public static Uri FindHttpBaseAddress(IEnumerable<Uri> serviceHostBaseAddresses)
		{
			return serviceHostBaseAddresses.FirstOrDefault(uri => uri.Scheme == Uri.UriSchemeHttp);
		}

		/// <summary>
		/// Gets the MEX endpoint of the service.
		/// </summary>
		/// <param name="host">The service host.</param>
		/// <returns>Gets the metadata exchange endpoint address or an empty string if no MEX endpoint was found.</returns>
		public static string GetMexEndpoint(ServiceHostBase host)
		{
			if (host == null) 
				return string.Empty;

			foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
			{
				if (endpoint.Contract.ContractType == typeof(IMetadataExchange))
					return endpoint.Address.ToString();
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the WSDL endpoint from the service host.
		/// </summary>
		/// <param name="host">The service host.</param>
		/// <returns>Gets the WSDL endpoint address or an empty string if service does not provide an WSDL endpoint.</returns>
		public static string GetWsdlEndpoint(ServiceHostBase host)
		{
			if (host == null) 
				return string.Empty;

			foreach (IServiceBehavior behavior in host.Description.Behaviors)
			{
				var metaBehavior = behavior as ServiceMetadataBehavior;
				if ((metaBehavior == null) || metaBehavior.HttpGetEnabled.IsFalse()) 
					continue;

				Uri baseAddress = FindHttpBaseAddress(host.BaseAddresses);
					
				return baseAddress != null ? CombineUri(baseAddress, metaBehavior.HttpGetUrl) + "?wsdl" : string.Empty;
			}
			return string.Empty;
		}

	}
}
