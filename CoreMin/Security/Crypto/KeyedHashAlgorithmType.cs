//--------------------------------------------------------------------------
// File:    KeyedHashAlgorithmType.cs
// Content:	Implementation of a KeyedHashAlgorithmType enum.
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
	/// Cryptography Keyed Hash Algorithm Type
	/// </summary>
	[Serializable]
	public enum KeyedHashAlgorithmType
	{
		/// <summary>
		/// HMACSHA1 keyed cryptography provider.
		/// Combines the HMAC standard with the SHA-1 hashing algorithm.
		/// </summary>
		[DisplayName("HMACSHA1")]
		MacSha1,
		/// <summary>
		/// MACTripleDES keyed cryptography provider.
		/// Uses the Triple-DES encryption algorithm to create a block cipher hash code.
		/// </summary>
		[DisplayName("MACTripleDES")]
		MacTripleDes,
	}
}