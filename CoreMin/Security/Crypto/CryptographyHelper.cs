//--------------------------------------------------------------------------
// File:    CryptographyHelper.cs
// Content:	Implementation of a Cryptography helper class
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2008 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Security.Crypto
{
	/// <summary>
	/// Cryptography helper class. Provides cryptography hash helper methods.
	/// </summary>
	public static class CryptographyHelper
	{
		#region Compute Hash Helper Methods

		/// <summary>
		/// Computes the hash value for the specified byte array.
		/// </summary>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="data">The data to hash.</param>
		/// <returns>The hash value.</returns>
		public static byte[] ComputeHash(HashAlgorithmType hashAlgorithm, byte[] data)
		{
			HashAlgorithm hashProvider = HashAlgorithm.Create(hashAlgorithm.GetDisplayName());
			try
			{
				return hashProvider.ComputeHash(data);

			}
			finally
			{
				hashProvider.Clear();
			}
		}

		/// <summary>
		/// Computes the hash value for the specified string.
		/// </summary>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="text">The text to hash.</param>
		/// <returns>The hash value.</returns>
		public static byte[] ComputeHash(HashAlgorithmType hashAlgorithm, string text)
		{
			byte[] data = text.ToByteArray();
			return ComputeHash(hashAlgorithm, data);
		}

		/// <summary>
		/// Computes the hash string for the specified string.
		/// </summary>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="text">The text to hash.</param>
		/// <returns>The hash string.</returns>
		public static string ComputeHashString(HashAlgorithmType hashAlgorithm, string text)
		{
			byte[] data = ComputeHash(hashAlgorithm, text);
			var sb = new StringBuilder();
			foreach (byte b in data)
			{
				sb.Append(b.ToString("X2").ToLower());
			}
			return sb.ToString();
		}

		#endregion

		#region Compute Keyed Hash Helper Methods

		/// <summary>
		/// Computes the keyed hash value for the specified key and data byte array.
		/// </summary>
		/// <param name="keyedHashAlgorithm">The keyed hash algorithm.</param>
		/// <param name="key">The key to use.</param>
		/// <param name="data">The data to hash.</param>
		/// <returns>The hash value.</returns>
		public static byte[] ComputeKeyedHash(KeyedHashAlgorithmType keyedHashAlgorithm, byte[] key, byte[] data)
		{
			KeyedHashAlgorithm keyedHashProvider = KeyedHashAlgorithm.Create(keyedHashAlgorithm.GetDisplayName());

			try
			{
				keyedHashProvider.Key = key;
				return keyedHashProvider.ComputeHash(data);

			}
			finally
			{
				keyedHashProvider.Clear();
			}
		}


		/// <summary>
		/// Computes the keyed hash value for the specified key and data string.
		/// </summary>
		/// <param name="keyedHashAlgorithm">The keyed hash algorithm.</param>
		/// <param name="keyString">The key to use.</param>
		/// <param name="dataString">The data to hash.</param>
		/// <returns>The hash value.</returns>
		public static byte[] ComputeKeyedHash(KeyedHashAlgorithmType keyedHashAlgorithm, string keyString, string dataString)
		{
			byte[] key = keyString.ToByteArray();
			byte[] data = dataString.ToByteArray();
			return ComputeKeyedHash(keyedHashAlgorithm, key, data);
		}


		/// <summary>
		/// Computes the keyed hash value for the specified key and data string.
		/// </summary>
		/// <param name="keyedHashAlgorithm">The keyed hash algorithm.</param>
		/// <param name="keyString">The key to use.</param>
		/// <param name="dataString">The data to hash.</param>
		/// <returns>The hash string.</returns>
		public static string ComputeKeyedHashString(KeyedHashAlgorithmType keyedHashAlgorithm, string keyString, string dataString)
		{
			byte[] data = ComputeKeyedHash(keyedHashAlgorithm, keyString, dataString);
			return HexConverter.ToHexString(data);
		}

		/// <summary>
		/// Generates a random key.
		/// </summary>
		/// <param name="keySize">Size of the key.</param>
		/// <returns>The generated key.</returns>
		public static byte[] GenerateRandomKey(int keySize)
		{
			var data = new byte[keySize];
			StaticRandomNumberGenerator.GetBytes(data);
			return data;
		}

		/// <summary>Returns an RFC 2898 hash value for the specified password.</summary>
		/// <returns>The hash value for <paramref name="password" /> as a base-64-encoded string.</returns>
		/// <param name="password">The password to generate a hash value for.</param>
		/// <exception cref="ArgNullException"><paramref name="password" /> is null.</exception>
		public static string HashPassword(string password)
		{
			ArgChecker.ShouldNotBeNull(password, "password");

			var bytes = new Rfc2898DeriveBytes(password, 16, 1000);
			byte[] salt = bytes.Salt;
			byte[] randomKey = bytes.GetBytes(32);
			bytes.DisposeIfNecessary();

			var resultHash = new byte[49];
			Buffer.BlockCopy(salt, 0, resultHash, 1, 16);
			Buffer.BlockCopy(randomKey, 0, resultHash, 17, 32);

			return Convert.ToBase64String(resultHash);
		}

		/// <summary>Determines whether the specified RFC 2898 hash and password are a cryptographic match.</summary>
		/// <returns>true if the hash value is a cryptographic match for the password; otherwise, false.</returns>
		/// <param name="hashedPassword">The previously-computed RFC 2898 hash value as a base-64-encoded string.</param>
		/// <param name="password">The plaintext password to cryptographically compare with <paramref name="hashedPassword" />.</param>
		/// <exception cref="ArgNullException"><paramref name="hashedPassword" /> or <paramref name="password" /> is null.</exception>
		public static bool VerifyHashedPassword(string hashedPassword, string password)
		{
			ArgChecker.ShouldNotBeNull(hashedPassword, "hashedPassword");
			ArgChecker.ShouldNotBeNull(password, "password");
			
			byte[] src = Convert.FromBase64String(hashedPassword);

			if ((src.Length != 49) || (src[0] != 0))
				return false;

			var oldSalt = new byte[16];
			Buffer.BlockCopy(src, 1, oldSalt, 0, 16);
			var oldKey = new byte[32];
			Buffer.BlockCopy(src, 17, oldKey, 0, 32);

			var bytes = new Rfc2898DeriveBytes(password, oldSalt, 1000);
			byte[] calcKey = bytes.GetBytes(32);
			bytes.DisposeIfNecessary();

			return ByteArraysEqual(oldKey, calcKey);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			if (ReferenceEquals(a, b))
			{
				return true;
			}
			if (((a == null) || (b == null)) || (a.Length != b.Length))
			{
				return false;
			}
			bool flag = true;
			for (int i = 0; i < a.Length; i++)
			{
				flag &= a[i] == b[i];
			}
			return flag;
		}




		#endregion

		private static RNGCryptoServiceProvider m_Rng;
 

		internal static RNGCryptoServiceProvider StaticRandomNumberGenerator
		{
			get { return m_Rng ?? (m_Rng = new RNGCryptoServiceProvider()); }
		}

	}
}