//--------------------------------------------------------------------------
// File:    MethodInfoEx.cs
// Content:	Implementation of class MethodInfoEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using SmartExpert.Error;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>Container class for <see cref="MethodInfo"/> extension methods.</summary>
	public static class MethodInfoEx
	{
		/// <summary>
		/// Gets the parameter info from <paramref name="method"/> by <paramref name="name"/>.
		/// </summary>
		/// <param name="method">The method info to get the parameter info from.</param>
		/// <param name="name">The parameter name.</param>
		/// <returns>The found parameter info, or <see langword="null"/> if <paramref name="method"/> has no parameter with the given <paramref name="name"/>.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="method"/> or <paramref name="name"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgEmptyException">Is thrown if <paramref name="name"/> is empty.</exception>
		public static ParameterInfo GetParameter(this MethodInfo method, string name)
		{
			ArgChecker.ShouldNotBeNull(method, "method");
			ArgChecker.ShouldNotBeNull(name, "name");

			return method.GetParameters().FirstOrDefault(p => p.Name == name);
		}

		/// <summary>
		/// Gets the name of the service operation.
		/// <note>This method tries to get the operation name from the <see cref="OperationContractAttribute"/>, 
		/// if one is defined on the <paramref name="method"/>; otherwise the name of the <paramref name="method"/> is returned.</note>
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>The service operation name.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="method"/> is <see langword="null"/>.</exception>
		public static string GetOperationName(this MethodInfo method)
		{
			ArgChecker.ShouldNotBeNull(method, "method");

			OperationContractAttribute operationContract;

			if (method.TryGetAttribute(out operationContract) && !string.IsNullOrEmpty(operationContract.Name)) 
				return operationContract.Name;

			return method.Name;
		}
	}
}
