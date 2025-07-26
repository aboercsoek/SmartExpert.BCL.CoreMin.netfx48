//--------------------------------------------------------------------------
// File:    Console2File.cs
// Content:	Implementation of class Console2File
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{
	///<summary>Console Out File Redirection Class</summary>
	public class Console2File : IDisposable
	{
		private TextWriter m_TmpTextWriter;
		private StreamWriter m_ConsoleRedirect;

		/// <summary>
		/// Initializes a new instance of the <see cref="Console2File"/> class.
		/// </summary>
		/// <remarks>Is protected to prevent called by user of this class.</remarks>
		protected Console2File()
		{
			m_TmpTextWriter = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Console2File"/> class.
		/// </summary>
		/// <param name="coutFileName">Name of the console redirect output file.</param>
		public Console2File(string coutFileName)
		{
			m_TmpTextWriter = Console.Out;
			m_ConsoleRedirect = new StreamWriter(coutFileName, true);
			Console.SetOut(m_ConsoleRedirect);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Console2File"/> is reclaimed by garbage collection.
		/// </summary>
		~Console2File()
		{
			Dispose(false);
		}

		#region IDisposable Members

		private void Dispose(bool isDispose)
		{
			if (isDispose)
				GC.SuppressFinalize(this);

			if (m_TmpTextWriter != null)
			{
				Console.SetOut(m_TmpTextWriter);
			}

			if ( m_ConsoleRedirect != null )
			{
				m_ConsoleRedirect.Flush();
				m_ConsoleRedirect.Close();
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
