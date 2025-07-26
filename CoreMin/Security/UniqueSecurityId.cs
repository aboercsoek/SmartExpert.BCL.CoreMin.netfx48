//--------------------------------------------------------------------------
// File:    UniqueSecurityId.cs
// Content:	Implementation of class UniqueSecurityId
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2011 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

#endregion

namespace SmartExpert.Security
{
	///<summary>Provides Methods to create unique security IDs that can be used as securty token IDs.</summary>
	public class UniqueSecurityId
	{
		#region Private Fields

		private static string m_CommonPrefix = ("uuid-" + Guid.NewGuid() + "-");
		private static long m_NextId;

		private long m_Id;
		private string m_Prefix;
		private string m_Val;

		#endregion

		#region Ctors

		/// <summary>
		/// Prevents a default instance of the <see cref="UniqueSecurityId"/> class from being created.
		/// </summary>
		/// <param name="prefix">The security id prefix.</param>
		/// <param name="id">The id part of the unique security id.</param>
		private UniqueSecurityId(string prefix, long id)
		{
			m_Id = id;
			m_Prefix = prefix ?? m_CommonPrefix;
			m_Val = null;
		}

		#endregion

		#region Static Factory Methods

		/// <summary>
		/// Creates a new unique security id instance.
		/// </summary>
		/// <returns>Returns the new unique security id.</returns>
		public static UniqueSecurityId Create()
		{
			return Create(m_CommonPrefix);
		}

		/// <summary>
		/// Creates a new unique security id instance using the provided prefix.
		/// </summary>
		/// <param name="prefix">The prefix of the unique security id.</param>
		/// <returns>Returns the new unique security id.</returns>
		public static UniqueSecurityId Create(string prefix)
		{
			return new UniqueSecurityId(prefix, Interlocked.Increment(ref m_NextId));
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the unique security id value.
		/// </summary>
		public string Value
		{
			get { return m_Val ?? (m_Val = m_Prefix + m_Id.ToString(CultureInfo.InvariantCulture)); }
		}

		#endregion

	}
}
