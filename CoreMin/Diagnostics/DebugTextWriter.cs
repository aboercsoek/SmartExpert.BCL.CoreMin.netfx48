//--------------------------------------------------------------------------
// File:    DebugTextWriter.cs
// Content:	Implementation of class DebugTextWriter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{
	/// <summary>
	/// TextWriter that writes into the <see cref="System.Diagnostics.Debug"/> trace listeners.
	/// </summary>
	public class DebugTextWriter : TextWriter
	{
		/// <summary>
		/// Returns the System.Text.Encoding in which the output is written.
		/// </summary>
		/// <returns>The Encoding in which the output is written.</returns>
		public override Encoding Encoding
		{
			get { return Encoding.Unicode; }
		}

		/// <summary>
		/// Writes a character to the debug trace listeners.
		/// </summary>
		/// <param name="value">The character to write to the debug stream.</param>
		public override void Write(char value)
		{
			Debug.Write(value.ToString());
		}


		/// <summary>
		/// Writes a subarray of characters to the debug trace listeners.
		/// </summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">Starting index in the buffer.</param>
		/// <param name="count">The number of characters to write.</param>
		public override void Write(char[] buffer, int index, int count)
		{
			Debug.Write(new string(buffer, index, count));
		}

		/// <summary>
		/// Writes a text to the debug trace listeners.
		/// </summary>
		/// <param name="value">A text to write.</param>
		public override void Write(string value)
		{
			Debug.Write(value);
		}

		/// <summary>
		/// Writes a line terminator to the debug trace listeners.
		/// </summary>
		public override void WriteLine()
		{
			Debug.WriteLine(string.Empty);
		}

		/// <summary>
		/// Writes a string followed by a line terminator to the debug trace listeners.
		/// </summary>
		/// <param name="value">The text to write. If value is null, only the line termination characters are written.</param>
		public override void WriteLine(string value)
		{
			Debug.WriteLine(value);
		}
	}
}