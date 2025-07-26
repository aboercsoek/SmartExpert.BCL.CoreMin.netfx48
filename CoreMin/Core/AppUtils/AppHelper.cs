//--------------------------------------------------------------------------
// File:    AppHelper.cs
// Content:	Implementation of class AppHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System.Reflection;
using SmartExpert.SystemProcesses;

#endregion

namespace SmartExpert.AppUtils
{
	///<summary>Provides easy access to common application properties.</summary>
	public static class AppHelper
	{
		#region Private static Fields

		static string m_ApplicationName;
		//static string m_ApplicationTitle;
		static string m_ApplicationVesion;

		static string m_ApplicationEventLogName;
		static string m_ApplicationEventLogSource;

		//static bool? m_DiagnosticLogging;

		#endregion

		#region Public static Properties

		/// <summary>
		/// Gets the name of the application.
		/// </summary>
		/// <value>The name of the application.</value>
		public static string ApplicationName
		{
			get
			{
				if ( string.IsNullOrEmpty(m_ApplicationName) )
				{
					Assembly a = Assembly.GetEntryAssembly();

					if (a == null)
						m_ApplicationName = ProcessHelper.GetCurrentProcessName();
					else
					{
						AssemblyName asmName = a.GetName();
						m_ApplicationName = asmName.Name;
					}
				}

				return m_ApplicationName;
			}
		}

		/// <summary>
		/// Gets the application version.
		/// </summary>
		/// <value>The application version.</value>
		public static string ApplicationVersion
		{
			get
			{
				if ( m_ApplicationVesion == null )
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly != null)
					{
						AssemblyName asmName = entryAssembly.GetName(false);
						m_ApplicationVesion = asmName.Version.ToString();
					}
					else
					{
						Assembly callingAssembly = Assembly.GetCallingAssembly();
						AssemblyName asmName = callingAssembly.GetName(false);
						m_ApplicationVesion = asmName.Version.ToString();
					}
				}
				return m_ApplicationVesion;
			}
		}

		//// <summary>
		//// Checks if diagnostic logging is enabled.
		//// </summary>
		//// <value><c>trus</c> if diagnostic logging is enabled; otherwise <see langword="false"/>.</value>
//        public static bool DiagnosticLogging
//        {
//            get
//            {
//#if CoreMin
//                m_DiagnosticLogging = false;
//

//
//                if (m_DiagnosticLogging.HasValue == false)
//                {
//                    var configServiceSection = ConfigService.Instance.GetConfigServiceSection();
//                    m_DiagnosticLogging = configServiceSection.DiagnosticLogging != null && configServiceSection.DiagnosticLogging.Value;
//                }
//
//                return m_DiagnosticLogging.Value;
//            }
//        }

		/// <summary>
		/// Gets or sets the name of the application eventlog.
		/// </summary>
		/// <value>The name of the application eventlog.</value>
		public static string ApplicationEventLogName
		{
			get
			{

				if (string.IsNullOrEmpty(m_ApplicationEventLogName))
				{
					m_ApplicationEventLogName = "Application";
				}

				return m_ApplicationEventLogName;
			}
			set
			{
				m_ApplicationEventLogName = value;
			}
		}

		/// <summary>
		/// Gets or sets the source of the application eventlog, that should be used during eventlog access of the application.
		/// </summary>
		/// <value>The source of the application eventlog.</value>
		public static string ApplicationEventLogSource
		{
			get
			{

				if (string.IsNullOrEmpty(m_ApplicationEventLogSource))
				{
					m_ApplicationEventLogSource = "SmartExpert";
				}

				return m_ApplicationEventLogSource;
			}
			set
			{
				m_ApplicationEventLogSource = value;
			}
		}

		#endregion

		#region Public static Methods

		/// <summary>
		/// Gets the application name and version value.
		/// </summary>
		/// <returns>
		/// Returns the application name and version value.
		/// </returns>
		public static string GetApplicationNameAndVersion()
		{
			return ApplicationName + ( ( ApplicationVersion != string.Empty ) ? " v" + ApplicationVersion : string.Empty );
		}

		#endregion
	}
}
