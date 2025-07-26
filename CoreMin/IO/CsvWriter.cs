//--------------------------------------------------------------------------
// File:    CsvWriter.cs
// Content:	Implementation of class CsvWriter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	///<summary>CSV writer class implementation.</summary>
	public class CsvWriter
	{
		#region Public Readonly Fields
		
		/// <summary>The underlying stream writer instance used by <see cref="CsvWriter"/>.</summary>
		public readonly StreamWriter BaseStreamWriter;

		#endregion

		#region Private Fields

		private static readonly char[] m_CharsToQuote = "\",\x0A\x0D".ToCharArray();
		const string SEP = ",";
		const string NEW_LINE = "\n";

		private bool m_ForceQuoting;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvWriter"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public CsvWriter(string filename)
			: this(filename, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvWriter"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="forceQuoting">if set to <see langword="true"/> force string quoting.</param>
		public CsvWriter(string filename, bool forceQuoting)
		{
			m_ForceQuoting = forceQuoting;
			BaseStreamWriter = File.CreateText(filename);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvWriter"/> class.
		/// </summary>
		/// <param name="streamWriter">The stream writer.</param>
		public CsvWriter(StreamWriter streamWriter)
			: this(streamWriter, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvWriter"/> class.
		/// </summary>
		/// <param name="streamWriter">The stream writer.</param>
		/// <param name="forceQuoting">if set to <see langword="true"/> force string quoting.</param>
		public CsvWriter(StreamWriter streamWriter, bool forceQuoting)
		{
			m_ForceQuoting = forceQuoting;
			BaseStreamWriter = streamWriter;
		}

		#endregion

		#region Dispose Pattern

		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="CsvWriter"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock (this)
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock (this)
			{
				if (m_Disposed == false)
				{
					Dispose(true);
				}
			}
		}

		private void Dispose(bool isDispose)
		{
			// Place release and cleanup operations here
			BaseStreamWriter.Close();

			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="CsvWriter"/> is reclaimed by garbage collection.
		/// </summary>
		~CsvWriter()
		{
			Dispose(false);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Writes items to CSV stream.
		/// </summary>
		/// <param name="strings">The strings.</param>
		public void WriteItems(IEnumerable<string> strings)
		{
			int strIndex = 0;
			foreach (var str in strings)
			{
				if (strIndex > 0)
				{
					BaseStreamWriter.Write(SEP);
				}
				WriteItem(str);
				strIndex++;
			}
			BaseStreamWriter.Write(NEW_LINE);
		}

		/// <summary>
		/// Writes the items to CSV stream.
		/// </summary>
		/// <param name="items">The items.</param>
		public void WriteItems(IEnumerable<object> items)
		{
			var a = items.Select(i => i.ToString()).ToList();
			WriteItems(a);
		}

		/// <summary>
		/// Closes this CSV writer instance.
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		#endregion

		#region Private Methods

		private void WriteItem(string s)
		{
			s = s ?? string.Empty;
			const string singleQuote = "\"";
			const string doubleQuote = "\"\"";

			if (m_ForceQuoting || s.IndexOfAny(m_CharsToQuote) > -1)
			{
				BaseStreamWriter.Write("\"{0}\"", s.Replace(singleQuote, doubleQuote));
			}
			else
			{
				BaseStreamWriter.Write(s);
			}
		}

		#endregion

	}
}
