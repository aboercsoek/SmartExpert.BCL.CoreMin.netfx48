//--------------------------------------------------------------------------
// File:    LogMethod.cs
// Content:	Implementation of enumeration LogMethod
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives
using System;


#endregion

namespace SmartExpert.Logging
{
	/// <summary>
	/// Logging method enumeration
	/// </summary>
	/// <seealso cref="LogFormatHelper"/>
	public enum LogMethod
	{
		/// <summary>
		/// Do not log.
		/// <note>DisplayName: <c>NONE</c>.</note>
		/// </summary>
		[DisplayName("NONE")]
		None = 0,
		/// <summary>
		/// Debug log method.
		/// <note>DisplayName: <c>DEBUG</c>.</note>
		/// </summary>
		[DisplayName("DEBUG")]
		Debug,
		/// <summary>
		/// Action log method.
		/// <note>DisplayName: <c>ACTION</c>.<para>Action is mapped to trace level <c>Debug</c>.</para></note>
		/// </summary>
		[DisplayName("ACTION")]
		Action,
		/// <summary>
		/// Info log method.
		/// <note>DisplayName: <c>INFO</c>.</note>
		/// </summary>
		[DisplayName("INFO")]
		Info,
		/// <summary>
		/// Audit log method. 
		/// <note>DisplayName: <c>AUDIT</c>. <para>Audit is mapped to trace level <c>Info</c>.</para></note>
		/// </summary>
		[DisplayName("AUDIT")]
		Audit,
		/// <summary>
		/// Warn log method.
		/// <note>DisplayName: <c>WARNING</c>.</note>
		/// </summary>
		[DisplayName("WARNING")]
		Warn,
		/// <summary>
		/// Error log method.
		/// <note>DisplayName: <c>ERROR</c>.</note>
		/// </summary>
		[DisplayName("ERROR")]
		Error
	}
}
