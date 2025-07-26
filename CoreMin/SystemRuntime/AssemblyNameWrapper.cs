//--------------------------------------------------------------------------
// File:    AssemblyNameWrapper.cs
// Content:	Implementation of class AssemblyNameWrapper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SmartExpert.SystemRuntime.Interop;

#endregion

namespace SmartExpert.SystemRuntime
{
	///<summary>Wrapper around the IAssemblyName COM-Interface</summary>
	public class AssemblyNameWrapper
	{
		#region Private Members
		/////////////////////////////////////////////////////////////////////////
		
		private IAssemblyName m_AssemblyName;
		
		#endregion

		#region ctors
		//*----------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyNameWrapper"/> class.
		/// </summary>
		/// <param name="assemblyName">AssemblyName COM-Interface object.</param>
		internal AssemblyNameWrapper( IAssemblyName assemblyName )
		{
			m_AssemblyName = assemblyName;
		}

		#endregion

		#region Public Properties and Methods


		//*----------------------------------------------------------------------------
		/// <summary>
		/// Gets the assembly name.
		/// </summary>
		/// <value>The assembly name.</value>
		public string Name
		{
			//set {WriteString(ASM_NAME.NAME, value);}
			get { return ReadString(AsmNamePropertyFlags.NAME); }
		}

		//*----------------------------------------------------------------------------
		/// <summary>
		/// Gets the assembly version.
		/// </summary>
		/// <value>The assembly version.</value>
		public Version Version
		{
			get
			{
				int major = ReadUInt16(AsmNamePropertyFlags.MAJOR_VERSION);
				int minor = ReadUInt16(AsmNamePropertyFlags.MINOR_VERSION);
				int build = ReadUInt16(AsmNamePropertyFlags.BUILD_NUMBER);
				int revision = ReadUInt16(AsmNamePropertyFlags.REVISION_NUMBER);
				return new Version(major, minor, build, revision);
			}
		}

		/// <summary>
		/// Gets the assembly culture.
		/// </summary>
		/// <value>The assembly culture.</value>
		public string Culture
		{
			get { return ReadString(AsmNamePropertyFlags.CULTURE); }
		}

		/// <summary>
		/// Gets the public key token of the assembly.
		/// </summary>
		/// <value>The public key token.</value>
		public byte[] PublicKeyToken
		{
			get { return ReadBytes(AsmNamePropertyFlags.PUBLIC_KEY_TOKEN); }
		}


		/*public byte[] PublicKey
		{
			get { return ReadBytes(AsmNamePropertyFlags.PUBLIC_KEY); }
		}*/

		private string m_StrongName;
		/// <summary>
		/// Gets the assembly strong name.
		/// </summary>
		/// <value>The assembly strong name.</value>
		public string StrongName
		{
			get
			{
				if (m_StrongName == null)
				{
					uint usize = 0;
					m_AssemblyName.GetDisplayName(null, ref usize, (uint)AssemblyNameDisplayFlags.ALL);
					int size = (int)usize;

					if (size <= 0) return string.Empty;

					StringBuilder strongName = new StringBuilder(size);
					m_AssemblyName.GetDisplayName(strongName, ref usize, (uint)AssemblyNameDisplayFlags.ALL);
					m_StrongName = strongName.ToString();
				}
				return m_StrongName;
			}
		}

		/// <summary>
		/// Gets the assembly code base.
		/// </summary>
		/// <value>The assembly code base.</value>
		public string CodeBase
		{
			get { return ReadString(AsmNamePropertyFlags.CODEBASE_URL); }
		}

		/// <summary>
		/// Converts AssemblyNameWrapper into a AssemblyName object.
		/// </summary>
		/// <returns>AssemblyName object with the same base content as the AssemblyNameWrapper instance.</returns>
		public AssemblyName ConvertToAssemblyName()
		{
			return new AssemblyName(StrongName);
		}

		/// <summary>
		/// Gets the assembly file location.
		/// </summary>
		/// <returns>
		/// Returns the assembly file location value.
		/// </returns>
		public string GetAssemblyFileLocation()
		{
			return Gac.GetLocation(this);
		}

		/// <summary>
		/// Returns the strong name of the assembly.
		/// </summary>
		/// <returns>The assembly strong name.</returns>
		public override string ToString()
		{
			return StrongName;
		}

		#endregion

		#region Internal Methods
		/////////////////////////////////////////////////////////////////////////
		
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private extern byte[] nGetPublicKeyToken();

		internal string GetLocation()
		{
			IAssemblyCache assemblyCache;

			Gac.CreateAssemblyCache(out assemblyCache, 0);
			if ( assemblyCache == null ) return null;

			AssemblyInfo assemblyInfo = new AssemblyInfo();
			assemblyInfo.cbAssemblyInfo = (uint)Marshal.SizeOf(typeof(AssemblyInfo));
			assemblyCache.QueryAssemblyInfo(AssemblyInfoFlags.VALIDATE | AssemblyInfoFlags.GETSIZE, StrongName, ref assemblyInfo);

			if ( assemblyInfo.cbAssemblyInfo == 0 ) return null;

			assemblyInfo.pszCurrentAssemblyPathBuf = new string(new char[assemblyInfo.cchBuf]);
			assemblyCache.QueryAssemblyInfo(AssemblyInfoFlags.VALIDATE | AssemblyInfoFlags.GETSIZE, StrongName, ref assemblyInfo);

			string value = assemblyInfo.pszCurrentAssemblyPathBuf;
			return value;
		} 

		#endregion
		
		#region Private Methods
		/////////////////////////////////////////////////////////////////////////
		
		private string ReadString( uint assemblyNameProperty )
		{
			uint size = 0;
			IntPtr ptr = IntPtr.Zero;
			string str;
			
			m_AssemblyName.GetProperty(assemblyNameProperty, IntPtr.Zero, ref size);
			
			if ( size == 0 || size > Int16.MaxValue ) return String.Empty;

			try
			{
				ptr = Marshal.AllocHGlobal((int)size);
				m_AssemblyName.GetProperty(assemblyNameProperty, ptr, ref size);
				str = Marshal.PtrToStringUni(ptr);
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeHGlobal(ptr);
			}
			
			return str;
		}

		private ushort ReadUInt16( uint assemblyNameProperty )
		{
			uint size = 0;
			
			m_AssemblyName.GetProperty(assemblyNameProperty, IntPtr.Zero, ref size);
			
			IntPtr ptr = Marshal.AllocHGlobal((int)size);
			
			m_AssemblyName.GetProperty(assemblyNameProperty, ptr, ref size);
			
			ushort value = (ushort)Marshal.ReadInt16(ptr);
			Marshal.FreeHGlobal(ptr);
			
			return value;
		}

		private byte[] ReadBytes( uint assemblyNameProperty )
		{
			uint size = 0;
			
			m_AssemblyName.GetProperty(assemblyNameProperty, IntPtr.Zero, ref size);
			
			IntPtr ptr = Marshal.AllocHGlobal((int)size);
			
			m_AssemblyName.GetProperty(assemblyNameProperty, ptr, ref size);
			
			byte[] value = new byte[(int)size];
			Marshal.Copy(ptr, value, 0, (int)size);
			Marshal.FreeHGlobal(ptr);
			
			return value;
		}

		#endregion

	}
	
}
