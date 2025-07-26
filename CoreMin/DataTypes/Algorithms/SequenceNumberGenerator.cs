//--------------------------------------------------------------------------
// File:    SequenceNumberGenerator.cs
// Content:	Implementation of class SequenceNumberGenerator
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{

	/// <summary>
	/// A sequence number generator
	/// </summary>
	public class SequenceNumberGenerator
	{
		private int n;

		/// <summary>
		/// Initializes a new instance of the <see cref="SequenceNumberGenerator"/> class. The sequence starts with 0.
		/// </summary>
		public SequenceNumberGenerator()
		{
			this.n = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SequenceNumberGenerator"/> class.
		/// </summary>
		/// <param name="start">The start of the sequence.</param>
		public SequenceNumberGenerator( int start )
		{
			this.n = start;
		}

		/// <summary>
		/// Gets the next sequence number.
		/// </summary>
		/// <returns>
		/// Returns the next sequence number.
		/// </returns>
		public int GetNextNumber()
		{
			int cur_seq_num = n;
			n++;
			return cur_seq_num;
		}

		/// <summary>
		/// Peeks the next sequence value (value will not be changed be Peek).
		/// </summary>
		/// <returns>Returns the next sequence value</returns>
		public int Peek()
		{
			return n;
		}
	}
}
