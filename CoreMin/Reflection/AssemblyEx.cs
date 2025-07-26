//--------------------------------------------------------------------------
// File:    AssemblyEx.cs
// Content:	Implementation of class AssemblyEx
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace SmartExpert.Reflection
{
	///<summary>Container class for <see cref="Assembly"/> extension methods.</summary>
	public static class AssemblyEx
	{
		/// <summary>
		/// Finds a manifest resource stream by name.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="name">The resource name.</param>
		/// <returns>The manifest resource stream for the given name, or <see langword="null"/> if a resource with that name does not exist.</returns>
		public static Stream FindManifestResourceStream(this Assembly assembly, string name)
		{
			var names = assembly.GetManifestResourceNames();
			
			if (name != null && names.Length > 0)
			{
				var resourceName = names.OrderByDescending(n => n.Length).FirstOrDefault(n => n.EndsWith(name));

				if (!string.IsNullOrEmpty(resourceName))
					return assembly.GetManifestResourceStream(resourceName);
			}

			return null;
		}

		/// <summary>
		/// Finds specific types in a collection of assemblies.
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		/// <param name="predicate">The search filter.</param>
		/// <returns>The types found by the search filter, or an empty collection if no types where found.</returns>
		public static IEnumerable<Type> FindTypes(this IEnumerable<Assembly> assemblies, Predicate<Type> predicate)
		{
			var types = new List<Type>();

			foreach (var assembly in assemblies)
			{
				types.AddRange(assembly.GetTypes().Where(t => predicate(t)));
			}
			return types;
		}

		/// <summary>
		/// Finds a specific type in a collection of assemblies.
		/// </summary>
		/// <param name="assemblies">The assemblies.</param>
		/// <param name="predicate">TThe search filter.</param>
		/// <returns>The specific type or <see langword="null"/> if the type was not found in the assembly collection.</returns>
		public static Type FindType(this IEnumerable<Assembly> assemblies, Predicate<Type> predicate)
		{
			foreach (var assembly in assemblies)
			{
				var type = assembly.GetTypes().FirstOrDefault(t => predicate(t));
				if (type != null) 
					return type;
			}
			return null;
		}
	}
}
