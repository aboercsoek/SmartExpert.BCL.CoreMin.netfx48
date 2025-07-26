//--------------------------------------------------------------------------
// File:    DisplayNameAttribute.cs
// Content:	Implementation of class DisplayNameAttribute
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	///<summary>DisplayName field and property attribute class</summary>
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class DisplayNameAttribute : Attribute
	{
		private string m_DisplayName;
		private readonly string m_DisplayNameKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayNameAttribute"/> class.
		/// </summary>
		/// <param name="displayName">The display name.</param>
		public DisplayNameAttribute(string displayName)
		{
			m_DisplayName = displayName ?? string.Empty;
			m_DisplayNameKey = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayNameAttribute"/> class.
		/// </summary>
		/// <param name="displayName">The display name.</param>
		/// <param name="displayNameKey">The display name key.</param>
		public DisplayNameAttribute(string displayName, string displayNameKey)
		{
			m_DisplayName = displayName ?? string.Empty;
			m_DisplayNameKey = displayNameKey ?? string.Empty;
		}

		/// <summary>Gets the display text (display name).</summary>
		/// <value>The display name value.</value>
		public string Text
		{
			get { return m_DisplayName; }
			set { m_DisplayName = value ?? string.Empty; }
		}

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
		public string DisplayName
		{
			get { return m_DisplayName; }
		}

		/// <summary>
		/// Gets the display name key.
		/// </summary>
		/// <value>The display name key.</value>
		public string DisplayNameKey
		{
			get { return m_DisplayNameKey; }
		}

	}

}
