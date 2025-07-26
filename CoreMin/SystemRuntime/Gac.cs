//--------------------------------------------------------------------------
// File:    Gac.cs
// Content:	Implementation of class Gac
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SmartExpert.AppUtils;
using SmartExpert.Error;
using SmartExpert.Linq;
using SmartExpert.SystemRuntime.Interop;

#endregion

namespace SmartExpert.SystemRuntime
{

	/// <summary>
	/// Contains helper routines to query the GAC for the presence and locations of assemblies.
	/// </summary>
	internal static class Gac
	{
		private static bool m_FusionLoaded;

		/// <summary>
		/// Determines whether the GAC contains the specified code base URI.
		/// </summary>
		/// <param name="codeBaseUri">The code base URI.</param>
		public static bool Contains( Uri codeBaseUri )
		{
			if ( codeBaseUri == null )
				return false;

			lock ( Globals.LockingObject )
			{
				if (LoadFusion() == false)
					return false;

				IAssemblyEnum assemblyEnum;
				int rc = CreateAssemblyEnum(out assemblyEnum, null, null, AsmCacheLocationFlags.GAC, 0);
				if ( rc < 0 || assemblyEnum == null ) return false;

				IApplicationContext applicationContext;
				IAssemblyName currentName;

				while ( assemblyEnum.GetNextAssembly(out applicationContext, out currentName, 0) == 0 )
				{
					if (currentName == null)
						continue;

					AssemblyNameWrapper assemblyName = new AssemblyNameWrapper(currentName);
					string scheme = codeBaseUri.Scheme;
					if ( assemblyName.CodeBase.StartsWith(scheme) )
					{
						try
						{
							Uri foundUri = new Uri(assemblyName.CodeBase);
							if ( codeBaseUri.Equals(foundUri) ) return true;
						}
						catch ( ArgumentNullException )
						{
						}
						catch ( UriFormatException )
						{
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Determines whether the GAC contains the specified strong name assembly.
		/// </summary>
		/// <param name="strongName">strong name of assembly.</param>
		public static bool Contains( string strongName )
		{
			if ( string.IsNullOrEmpty(strongName) ) return false;

			lock ( Globals.LockingObject )
			{
				if (LoadFusion() == false)
					return false;

				IAssemblyEnum assemblyEnum;
				int rc = CreateAssemblyEnum(out assemblyEnum, null, null, AsmCacheLocationFlags.GAC, 0);
				if ( rc < 0 || assemblyEnum == null ) return false;

				IApplicationContext applicationContext;
				IAssemblyName currentName;

				while ( assemblyEnum.GetNextAssembly(out applicationContext, out currentName, 0) == 0 )
				{
					//^ assume currentName != null;
					AssemblyNameWrapper assemblyName = new AssemblyNameWrapper(currentName);
					int nameDiff = assemblyName.StrongName.Length - strongName.Length;

					if (assemblyName.StrongName == strongName)
						return true;

					if ( nameDiff > 0 && assemblyName.StrongName.StartsWith(strongName) && 
						strongName.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 4)
						return true;

					if ( nameDiff < 0 && strongName.StartsWith(assemblyName.StrongName) &&
						assemblyName.StrongName.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Length == 4)
						return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Returns the original location of the corresponding assembly if available, otherwise returns the location of the shadow copy.
		/// If the corresponding assembly is not in the GAC, null is returned.
		/// </summary>
		public static string GetLocation( AssemblyNameWrapper assemblyName )
		{
			lock ( Globals.LockingObject )
			{
				if (LoadFusion() == false)
					return null;

				IAssemblyEnum assemblyEnum;
				CreateAssemblyEnum(out assemblyEnum, null, null, AsmCacheLocationFlags.GAC, 0);
				if ( assemblyEnum == null ) return null;

				IApplicationContext applicationContext;
				IAssemblyName currentName;

				while ( assemblyEnum.GetNextAssembly(out applicationContext, out currentName, 0) == 0 )
				{
					//^ assume currentName != null;
					AssemblyNameWrapper currentAssemblyName = new AssemblyNameWrapper(currentName);

					//string location = currentAssemblyName.GetLocation() ?? string.Empty;

					if ( CheckEquals(currentAssemblyName, assemblyName) )
					{
						string codeBase = currentAssemblyName.CodeBase;
						
						if ( codeBase != null && codeBase.StartsWith("file:///") )
							return codeBase.Substring(8);

						return currentAssemblyName.GetLocation();
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets all assemblies form the GAC.
		/// </summary>
		/// <returns>GAC assembly list.</returns>
		public static IEnumerable<AssemblyNameWrapper> GetAssemblies()
		{
			List<AssemblyNameWrapper> list = new List<AssemblyNameWrapper>();

			lock ( Globals.LockingObject )
			{
				IAssemblyName currentName;
				IAssemblyEnum assemblyEnum;
				IApplicationContext applicationContext;

				if (LoadFusion() == false)
					return list;

				CreateAssemblyEnum(out assemblyEnum, null, null, AsmCacheLocationFlags.GAC, 0);

				while ( assemblyEnum.GetNextAssembly(out applicationContext, out currentName, 0) == 0 )
				{
					AssemblyNameWrapper assemblyName = new AssemblyNameWrapper(currentName);

					list.Add(assemblyName);
				}
			}
			return list;
		}

		/// <summary>
		/// Gets a specific assembly name form the GAC.
		/// </summary>
		/// <returns>The assembly name if fond in the GAC, otherwise <see langword="null"/>.</returns>
		public static AssemblyNameWrapper GetAssemblyName(string strongName)
		{
			lock ( Globals.LockingObject )
			{
				IAssemblyName currentName;
				IAssemblyEnum assemblyEnum;
				IApplicationContext applicationContext;

				if (LoadFusion() == false)
					return null;

				string[] strongNameSplit = strongName.Split(new [] {','}, StringSplitOptions.None);

				if (strongNameSplit.Length < 4)
					return null;
				
				// Minimal strong name infos:
				// SmartExpert.SmartLibrary.Factory.Unity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed05f29d1c947ca0
				string tmpName			 = strongNameSplit[0].Trim();
				string tmpVersion		 = strongNameSplit[1].Split(new [] {'='}, StringSplitOptions.None)[1].Trim();
				string tmpCulture		 = strongNameSplit[2].Split(new [] { '=' }, StringSplitOptions.None)[1].Trim();
				
				string tmpPublicKeyToken = strongNameSplit[3].Split(new [] { '=' }, StringSplitOptions.None)[1].Trim();
				byte[] publicKeyToken = new byte[tmpPublicKeyToken.Length / 2];
				for ( int i = 0; i < publicKeyToken.Length; i++ )
				{
					publicKeyToken[i] = byte.Parse(tmpPublicKeyToken.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
				}
				
				CreateAssemblyEnum(out assemblyEnum, null, null, AsmCacheLocationFlags.GAC, 0);

				while ( assemblyEnum.GetNextAssembly(out applicationContext, out currentName, 0) == 0 )
				{
					AssemblyNameWrapper assemblyName = new AssemblyNameWrapper(currentName);
					if ( assemblyName.Name == tmpName )
					{
						
						if (assemblyName.Name == tmpName &&
						    assemblyName.Version.ToString() == tmpVersion &&
						    (assemblyName.Culture == tmpCulture || assemblyName.Culture == string.Empty )&&
						    IteratorHelper.EnumerablesAreEqual(assemblyName.PublicKeyToken, publicKeyToken))
						{
							return assemblyName;
						}
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Adds the assembly to the GAC.
		/// </summary>
		/// <param name="assemblyFilePath">The assembly file path.</param>
		/// <returns><see langword="true"/> = Adding the assembly to the GAC was successfull, otherwise <see langword="false"/>.</returns>
		internal static bool AddAssembly( string assemblyFilePath )
		{
			ArgChecker.ShouldNotBeNullOrEmpty(assemblyFilePath, "assemblyFilePath");
			if ( !File.Exists(assemblyFilePath) )
			{
				throw new ArgException<string>(assemblyFilePath, "assemblyFilePath", "Argument {0} error: Assembly file does not exist! (value = {1}");
			}

			IAssemblyCache assemblyCache;
			CreateAssemblyCache(out assemblyCache, 0);
			if ( assemblyCache == null ) return false;

			return ( assemblyCache.InstallAssembly(0, assemblyFilePath, IntPtr.Zero) == 0 );
		}

		/// <summary>
		/// Removes the assembly from the GAC.
		/// </summary>
		/// <param name="assemblyName">Name of assembly.</param>
		/// <returns><see langword="true"/> = Removing the assembly from the GAC was successfull, otherwise <see langword="false"/>.</returns>
		internal static bool RemoveAssembly( string assemblyName )
		{
			uint pulDisposition;
			ArgChecker.ShouldNotBeNullOrEmpty(assemblyName, "assemblyName");

			IAssemblyCache assemblyCache;
			CreateAssemblyCache(out assemblyCache, 0);
			if ( assemblyCache == null ) return false;

			if ( assemblyCache.UninstallAssembly(0, assemblyName, IntPtr.Zero, out pulDisposition) != 0 )
			{
				int count = (from asmName in GetAssemblies()
								where asmName.Name == assemblyName
								select asmName).Count();
							
				return (count == 0);
			}

			return true;
		}

		internal static bool CheckEquals( AssemblyNameWrapper currentName, AssemblyNameWrapper asmName )
		{
			if ( currentName.Name != asmName.Name ) return false;

			if ( currentName.Version != asmName.Version ) return false;

			if ( string.Compare(currentName.Culture, asmName.Culture, StringComparison.OrdinalIgnoreCase) != 0 ) return false;

			if ( IteratorHelper.EnumerableIsNotEmpty(currentName.PublicKeyToken) )
				return IteratorHelper.EnumerablesAreEqual(currentName.PublicKeyToken, asmName.PublicKeyToken);

			if ( currentName.GetLocation().Length == 0 || asmName.GetLocation().Length == 0 ) return true;

			return string.Compare(currentName.GetLocation(), asmName.GetLocation(), StringComparison.OrdinalIgnoreCase) == 0;

		}

		private static bool LoadFusion()
		{
			if (!m_FusionLoaded)
			{
				System.Reflection.Assembly systemAssembly = typeof(object).Assembly;
				//^ assume systemAssembly != null;
				string systemAssemblyLocation = systemAssembly.Location;
				//^ assume systemAssemblyLocation != null;
				string dir = Path.GetDirectoryName(systemAssemblyLocation);
				if (dir == null)
					return false;
				LoadLibrary(Path.Combine(dir, "fusion.dll"));
				m_FusionLoaded = true;
			}

			return true;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern IntPtr LoadLibrary( string lpFileName );
		[DllImport("fusion.dll", CharSet = CharSet.Auto)]
		internal static extern int CreateAssemblyEnum( out IAssemblyEnum ppEnum, IApplicationContext pAppCtx, IAssemblyName pName, uint dwFlags, int pvReserved );
		[DllImport("fusion.dll", CharSet = CharSet.Auto)]
		internal static extern int CreateAssemblyCache( out IAssemblyCache ppAsmCache, uint dwReserved );

		

	}

}
