//--------------------------------------------------------------------------
// File:    HashAlgorithmType.cs
// Content:	Implementation of a HashAlgorithmType enum.
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Security.Crypto
{
	/// <summary>
	/// Cryptography Hash Algorithm Type
	/// </summary>
	[Serializable]
	public enum HashAlgorithmType
	{
		/// <summary>
		/// MD5 cryptography provider.
		/// Input block size: 512; Message limit (bits): 2^64; Hash code size (bits): 128
		/// </summary>
		[DisplayName("MD5")]
		Md5,
		/// <summary>
		/// SHA1 cryptography provider
		/// Input block size: 512; Message limit (bits): 2^64; Hash code size (bits): 160
		/// </summary>
		[DisplayName("SHA1")]
		Sha1,
		/// <summary>
		/// SHA256 cryptography provider
		/// Input block size: 512; Message limit (bits): 2^64; Hash code size (bits): 256
		/// </summary>
		[DisplayName("SHA256")]
		Sha256,
		/// <summary>
		/// SHA384 cryptography provider
		/// Input block size: 1024; Message limit (bits): 2^128; Hash code size (bits): 384
		/// </summary>
		[DisplayName("SHA384")]
		Sha384,
		/// <summary>
		/// SHA512 cryptography provider
		/// Input block size: 1024; Message limit (bits): 2^128; Hash code size (bits): 512
		/// </summary>
		[DisplayName("SHA512")]
		Sha512,
	}
}