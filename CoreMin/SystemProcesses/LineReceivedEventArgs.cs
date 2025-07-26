//--------------------------------------------------------------------------
// File:    LineReceivedEventArgs.cs
// Content:	Implementation of class LineReceivedEventArgs
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;

#endregion

namespace SmartExpert.SystemProcesses
{
	//public delegate void LineReceivedEventHandler( object sender, LineReceivedEventArgs e );

	/// <summary>
	/// The arguments for the Line Received Event Handler event.
	/// </summary>
	/// <see cref="ProcessRunner"/>
	public class LineReceivedEventArgs : EventArgs
	{
		string m_Line = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="LineReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="line">The line.</param>
		public LineReceivedEventArgs( string line )
		{
			m_Line = line;
		}

		/// <summary>
		/// Gets the received line.
		/// </summary>
		/// <value>The received line.</value>
		public string Line
		{
			get
			{
				return m_Line;
			}
		}
	}
}
