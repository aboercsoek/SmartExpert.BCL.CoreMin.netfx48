//--------------------------------------------------------------------------
// File:    GacManager.cs
// Content:	Implementation of class GacManager
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartExpert.AppUtils;
using SmartExpert.Error;
using SmartExpert.IO;
using SmartExpert.Linq;
using SmartExpert.SystemRuntime.Interop;

#endregion

namespace SmartExpert.SystemRuntime
{
	///<summary>GAC assembly manager</summary>
	/// <example>
	/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
	/// <para />
	/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
	/// <para />
	/// </example>
	/// <seealso cref="Gac"/>
	public class GacManager : IDisposable
	{
		private Dictionary<string, AssemblyNameWrapper> m_Gac;

		/// <summary>
		/// Initializes a new instance of the <see cref="GacManager"/> class, 
		/// without initializing the internal <see cref="GacManager"/> GAC cache.
		/// </summary>
		/// <remarks>You can call <see cref="InitGacCache"/> manually to initialize the cache. 
		/// If you call any other method that requires the cache, the method ifself calls <see cref="InitGacCache"/> to ensure that the cache is available.</remarks>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
		/// <para/>
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacManager_Output.txt" />
		/// <para/>		
		/// </example>
		public GacManager()
			: this(false)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GacManager"/> class.
		/// </summary>
		/// <param name="initGacCache">if set to <see langword="true"/>, <see cref="InitGacCache"/> is called within the constructor.</param>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
		/// <para/>
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacManager_Output.txt" />
		/// <para/>		
		/// </example>
		/// <seealso cref="InitGacCache"/>
		public GacManager(bool initGacCache)
		{
			m_Disposed = false;
			m_Gac = new Dictionary<string, AssemblyNameWrapper>();
			if (initGacCache)
				InitGacCache();
		}

		/// <summary>
		/// Inits the internal GAC cache of <see cref="GacManager"/> .
		/// </summary>
		/// <returns>The number of assemblies in the GAC.</returns>
		/// <remarks>The first call of this mehtod reads all assembly entries form the GAC.
		/// Subsequent calls only return the number of the internal GacManager cache.</remarks>
		/// <exception cref="ObjectDisposedException">Is thrown if the <see cref="Dispose"/> method was called before.</exception>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
		/// <para/>
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacManager_Output.txt" />
		/// <para/>		
		/// </example>
		public int InitGacCache()
		{
			if (Disposed) throw new ObjectDisposedException("GacManager");

			lock (m_SyncObject)
			{
				if (m_Gac.Keys.Count > 0)
				{
					return m_Gac.Keys.Count;
				}

				Gac.GetAssemblies().Foreach(a => m_Gac.Add(a.StrongName, a));

				if (m_Gac.Keys.Count == 0)
					throw new ApplicationException("GAC cache creation failed!");

			}

			return m_Gac.Keys.Count;
		}

		/// <summary>
		/// Forces the initialisation of the internal <see cref="GacManager"/> GAC cache.
		/// </summary>
		/// <remarks>By contrast with <see cref="InitGacCache"/>, <see cref="ForceInitGacCache"/> builds the internal cache every time it is called.</remarks>
		/// <returns>The number of assemblies in the GAC.</returns>
		/// <exception cref="ObjectDisposedException">Is thrown if the <see cref="Dispose"/> method was called before.</exception>
		/// <seealso cref="InitGacCache"/>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// </example>
		public int ForceInitGacCache()
		{
			if (Disposed) throw new ObjectDisposedException("GacManager");

			lock (m_SyncObject)
			{
				if (m_Gac.Keys.Count > 0)
				{
					m_Gac.Clear();
				}

				Gac.GetAssemblies().Foreach(a => m_Gac.Add(a.StrongName, a));

				if (m_Gac.Keys.Count == 0)
					throw new ApplicationException("GAC cache creation failed!");

			}

			return m_Gac.Keys.Count;
		}

		/// <summary>
		/// Returns the enumerable collection of the internal GAC cache.
		/// </summary>
		/// <returns>The enumerable collection of the internal GAC cache</returns>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// </example>
		public IEnumerable<AssemblyNameWrapper> EnumGac()
		{
			if (Disposed) throw new ObjectDisposedException("GacManager");

			InitGacCache();

			return m_Gac.Values;
		}
		/// <summary>
		/// Searches for assembly names in the internal GAC cache.
		/// </summary>
		/// <param name="asmNameSearchFilter">The assembly name search filter.</param>
		/// <param name="versionSearchFilter">The assembly version search filter.</param>
		/// <returns>The collection of <see cref="AssemblyNameWrapper"/> that meet the search criterias.</returns>
		/// <remarks>
		/// The assembly name search filter <paramref name="asmNameSearchFilter"/> uses the 'Contains'-Rule.
		/// The assembly version search filter <paramref name="versionSearchFilter"/> is not applied if the value is null or empty.
		/// If <paramref name="versionSearchFilter"/> is not null or empty, the 'StartsWith'-Rule is applied.
		/// </remarks>
		/// <seealso cref="Search(System.Func{SmartExpert.SystemRuntime.AssemblyNameWrapper,bool})"/>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public IEnumerable<AssemblyNameWrapper> Search(string asmNameSearchFilter, string versionSearchFilter)
		{
			if (Disposed) throw new ObjectDisposedException("GacManager");

			ArgChecker.ShouldNotBeNullOrEmpty(asmNameSearchFilter, "asmNameSearchFilter");

			InitGacCache();

			IEnumerable<AssemblyNameWrapper> matchingAssemblies;

			if (string.IsNullOrEmpty(versionSearchFilter))
			{
				matchingAssemblies = from asm in m_Gac.Values
									 where asm.Name.Contains(asmNameSearchFilter)
									 select asm;
			}
			else
			{
				matchingAssemblies = from asm in m_Gac.Values
									 where asm.Name.Contains(asmNameSearchFilter)
									 where asm.Version.ToString().StartsWith(versionSearchFilter)
									 select asm;
			}

			return matchingAssemblies;
		}

		/// <summary>
		/// Tries to query the assembly info.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly.</param>
		/// <param name="approximate">if set to <see langword="true"/> [approximate].</param>
		/// <param name="location">The location.</param>
		/// <returns>true is the assembly location could be retrieved; otherwise false.</returns>
		public static bool TryQueryAssemblyInfo(string assemblyName, bool approximate, out string location)
		{
			lock (Globals.LockingObject)
			{
				IAssemblyCache cache;
				AssemblyInfo info;
				
				Gac.CreateAssemblyCache(out cache, 0);
				if (cache == null)
				{
					location = null;
					return false;
				}

				if (approximate)
				{
					int length = assemblyName.LastIndexOf(',');
					if (length >= 0)
					{
						assemblyName = assemblyName.Substring(0, length);
					}
				}

				info = new AssemblyInfo();
				info.cchBuf = 0x400;
				info.pszCurrentAssemblyPathBuf = new string('\0', (int)info.cchBuf);

				location = cache.QueryAssemblyInfo(0, assemblyName, ref info) >= 0 ? info.pszCurrentAssemblyPathBuf : null;
			}

			return (location != null);
		}

		/// <summary>
		/// Searches for assembly names in the internal GAC cache.
		/// </summary>
		/// <param name="searchFilterPredicate">The search filter predicate</param>
		/// <returns>The collection of <see cref="AssemblyNameWrapper"/> items for witch <paramref name="searchFilterPredicate"/> returns <see langword="true"/>.</returns>
		/// <seealso cref="Search(string,string)"/>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public IEnumerable<AssemblyNameWrapper> Search(Func<AssemblyNameWrapper, bool> searchFilterPredicate)
		{
			if (Disposed) throw new ObjectDisposedException("GacManager");

			ArgChecker.ShouldNotBeNull(searchFilterPredicate, "searchFilterPredicate");

			InitGacCache();

			var matchingAssemblies = from asm in m_Gac.Values
									 where searchFilterPredicate(asm)
									 select asm;

			return matchingAssemblies;
		}

		/// <summary>
		/// Adds the assembly to the GAC.
		/// </summary>
		/// <param name="assemblyFilePath">The assembly file path.</param>
		/// <returns><see langword="true"/> = Adding the assembly to the GAC was successfull, otherwise <see langword="false"/>.</returns>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public bool AddToGac(string assemblyFilePath)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(assemblyFilePath, "assemblyFilePath");
			return Gac.AddAssembly(assemblyFilePath);
		}

		/// <summary>
		/// Removes the assembly from the GAC.
		/// </summary>
		/// <param name="assembly">The assembly to remove.</param>
		/// <returns>Returns <see langword="true"/> if the assembly was successfully removed from GAC, otherwise <see langword="false"/>.</returns>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public bool RemoveFromGac(AssemblyNameWrapper assembly)
		{
			ArgChecker.ShouldNotBeNull(assembly, "assembly");
			return Gac.RemoveAssembly(assembly.StrongName);
		}

		/// <summary>
		/// Removes the assembly from the GAC.
		/// </summary>
		/// <param name="assemblyStrongName">StrongName of the assembly.</param>
		/// <returns>Returns <see langword="true"/> if the assembly was successfully removed from GAC, otherwise <see langword="false"/>.</returns>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public bool RemoveFromGac(string assemblyStrongName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(assemblyStrongName, "assemblyStrongName");
			return Gac.RemoveAssembly(assemblyStrongName);
		}

		/// <summary>
		/// Checkes if the assembly is registered in the GAC.
		/// </summary>
		/// <param name="assemblyStrongName">StrongName of the assembly.</param>
		/// <returns>Returns <see langword="true"/> if the assembly is registered in the GAC, otherwise <see langword="false"/>.</returns>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacAddRemoveAssembly" />
		/// <para />
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacAndGacManager_Output.txt" />
		/// <para />
		/// </example>
		public bool IsInGac(string assemblyStrongName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(assemblyStrongName, "assemblyStrongName");
			return Gac.Contains(assemblyStrongName);
		}

		/// <summary>
		/// Saves all GAC assembly names in a file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <exception cref="ObjectDisposedException">Is thrown if the <see cref="Dispose"/> method was called before.</exception>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
		/// <para/>
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacManager_Output.txt" />
		/// <para />
		/// </example>
		public void SaveGacAssemblyNames(string fileName)
		{
			InitGacCache();

			var sortedAssemblies = from asmKey in m_Gac.Keys
								   orderby asmKey.Split(new[] { ',' })[0], asmKey.Split(new[] { ',' })[1]
								   select asmKey;

			using (StreamWriter sw = new StreamWriter(fileName))
			{

				sortedAssemblies.Foreach(sw.WriteLine);

				sw.Flush();
				sw.Close();
			}
		}

		/// <summary>
		/// Copies GAC assemblies into a folder.
		/// </summary>
		/// <remarks>
		/// The assembly name search filter <paramref name="asmNameSearchFilter"/> uses the 'Contains'-Rule.
		/// The assembly version search filter <paramref name="versionSearchFilter"/> is not applied if the value is null or empty.
		/// If <paramref name="versionSearchFilter"/> is not null or empty, the 'StartsWith'-Rule is used.
		/// </remarks>
		/// <param name="asmNameSearchFilter">The assembly name search filter.</param>
		/// <param name="versionSearchFilter">The assembly version search filter.</param>
		/// <param name="destPath">The destination path.</param>
		/// <returns>The number of copied files.</returns>
		/// <exception cref="ObjectDisposedException">Is thrown if the <see cref="Dispose"/> method was called before.</exception>
		/// <example>
		/// <code lang="cs" title="GacManager example" numberLines="true" outlining="true" source=".\examples\Sample_CoreMin_RuntimeServices_GacManager.cs" region="Sample_CoreMin_RuntimeServices_C_GacManager" />
		/// <para/>
		/// <code lang="cs" title="Output Console:" source=".\examples\Sample_CoreMin_RuntimeServices_C_GacManager_Output.txt" />
		/// <para />
		/// </example>
		public int CopyGacAssembliesToFolder(string asmNameSearchFilter, string versionSearchFilter, string destPath)
		{
			int fileCopyCount = 0;

			if (Disposed) throw new ObjectDisposedException("GacManager");

			ArgChecker.ShouldNotBeNullOrEmpty(asmNameSearchFilter, "asmNameSearchFilter");
			ArgChecker.ShouldNotBeNullOrEmpty(destPath, "destPath");

			FileHelper.CreateDirectory(destPath);

			if (Directory.Exists(destPath) == false)
			{
				throw new ArgException<string>(destPath, "destPath", "Argument {0} error: Directory was not found and could not be created! (value = {1}");
			}

			InitGacCache();

			IEnumerable<AssemblyNameWrapper> matchingAssemblies = Search(asmNameSearchFilter, versionSearchFilter);

			foreach (var asm in matchingAssemblies)
			{
				string gacAsmPath = Gac.GetLocation(asm);
				string fileName = Path.GetFileName(gacAsmPath);
				if (fileName.IsNotNull())
				{
					string version = asm.Version.ToString();
					FileHelper.CreateDirectory(Path.Combine(destPath, version));
					string destFilePath = Path.Combine(Path.Combine(destPath, version), fileName);
					File.Copy(gacAsmPath, destFilePath, true);
					fileCopyCount++;
				}
			}

			return fileCopyCount;
		}

		#region IDisposable Members

		private object m_SyncObject = new object();
		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="GacManager"/> instance is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		public bool Disposed
		{
			get
			{
				lock (m_SyncObject)
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>After the call of Dispose all <see cref="GacManager"/> method calls 
		/// will fail with a <see cref="ObjectDisposedException"/> exception.</remarks>
		public void Dispose()
		{
			lock (m_SyncObject)
			{
				if (m_Disposed == false)
				{
					Cleanup();
					m_Disposed = true;
					GC.SuppressFinalize(this);
				}
			}
		}

		/// <summary>
		/// Cleanups this instance.
		/// </summary>
		protected virtual void Cleanup()
		{
			m_Gac.Clear();
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="GacManager"/> is reclaimed by garbage collection.
		/// </summary>
		~GacManager()
		{
			Cleanup();
		}

		#endregion
	}
}
