//--------------------------------------------------------------------------
// File:    Console2String.cs
// Content:	Implementation of class Console2File
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{
	///<summary>Console Out to string redirection class.</summary>
	/// <example>
	/// <code>
	/// StringBuilder sb = new StringBuilder();
	/// using (new Console2String(sb))
	/// {
	///     Console.Write(@"Test öäü$");
	/// }
	/// string consoleWrittenText = sb.ToString(); // consoleWrittenText value = "Test öäü$"
	/// </code>
	/// </example>
	public class Console2String : IDisposable
	{
		private TextWriter m_TmpTextWriter;
		private StringWriter m_ConsoleRedirect;

		/// <summary>
		/// Initializes a new instance of the <see cref="Console2String"/> class.
		/// </summary>
		/// <remarks>Is protected to prevent called by user of this class.</remarks>
		protected Console2String()
		{
			m_TmpTextWriter = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Console2String"/> class.
		/// </summary>
		/// <param name="coutStringBuilder">StringBuilder where the console output should be directed.</param>
		/// <example>
		/// <code>
		/// StringBuilder sb = new StringBuilder();
		/// using (new Console2String(sb))
		/// {
		///     Console.Write(@"Test öäü$");
		/// }
		/// string consoleWrittenText = sb.ToString(); // consoleWrittenText value = "Test öäü$"
		/// </code>
		/// </example>
		public Console2String(StringBuilder coutStringBuilder)
		{
			m_TmpTextWriter = Console.Out;
			m_ConsoleRedirect = new StringWriter(coutStringBuilder);
			Console.SetOut(m_ConsoleRedirect);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Console2String"/> is reclaimed by garbage collection.
		/// </summary>
		~Console2String()
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
		/// <example>
		/// <code>
		/// StringBuilder sb = new StringBuilder();
		/// using (new Console2String(sb))
		/// {
		///     Console.Write(@"Test öäü$");
		/// }
		/// string consoleWrittenText = sb.ToString(); // consoleWrittenText value = "Test öäü$"
		/// </code>
		/// </example>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
