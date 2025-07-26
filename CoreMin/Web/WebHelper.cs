//--------------------------------------------------------------------------
// File:    WebHelper.cs
// Content:	Implementation of class WebHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Web
{
	///<summary>Web utility class</summary>
	public static class WebHelper
	{
		private const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";
		private const string PublicFolder = "public";
		private static readonly string[] DecodedCharValues;

		static WebHelper()
		{
			DecodedCharValues = new string[256];

			for (int i = 0; i < 256; i++)
			{
				string hexString = "%" + NumberFormatter.Int32ToHexString(i, 2);
				DecodedCharValues[i] = hexString;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this application domain is web hosted.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if this application domain is web hosted; otherwise, <see langword="false"/>.
		/// </value>
		public static bool IsWebHostingEnvironment
		{
			get { return System.Web.Hosting.HostingEnvironment.IsHosted; }
		}

		/// <summary>
		/// Encodes the URL string.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>The encoded URL.</returns>
		public static string EncodeUrlString(string url)
		{
			if (string.IsNullOrEmpty(url))
				return url;

			string result = url;

			result = result.Replace("%", "%25");
			result = result.Replace(" ", "%20");
			result = result.Replace("\"", "%22");
			result = result.Replace("<", "%3C");
			result = result.Replace(">", "%3E");
			result = result.Replace("#", "%23");
			result = result.Replace("{", "%7B");
			result = result.Replace("}", "%7D");
			result = result.Replace("|", "%7C");
			result = result.Replace("\\", "%5C");
			result = result.Replace("^", "%5E");
			result = result.Replace("~", "%7E");
			result = result.Replace("[", "%5B");
			result = result.Replace("]", "%5D");
			result = result.Replace("`", "%60");

			return result;
		}

		/// <summary>
		/// Normalizes the string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The normalized string</returns>
		public static string NormalizeString(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			string result = text;

			result = result.Replace("://", "_");
			result = result.Replace("/", "_");
			result = result.Replace("\\", "_");
			result = result.Replace("?", "_");
			result = result.Replace("#", "_");
			result = result.Replace(":", "_");
			result = result.Replace(" ", "_");

			return result;
		}

		/// <summary>
		/// XML text encoding.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>XML encoded string.</returns>
		public static string XmlEncode(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			string element = new XElement("t", text).ToString(SaveOptions.DisableFormatting);
			element = element.Substring(3, element.Length - 7);

			return element;
		}

		/// <summary>
		/// XML text decoding.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>XML decoded string.</returns>
		public static string XmlDecode(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			string result = text;

			result = result.Replace("&amp;", "&");
			result = result.Replace("&lt;", "<");
			result = result.Replace("&gt;", ">");
			result = result.Replace("&apos;", "\'");
			result = result.Replace("&quot;", "\"");

			return result;
		}


		/// <summary>
		/// URL encoding with uppercase percent20.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The URL encoded value.</returns>
		public static string UrlEncodeWithUppercasePercent20(string value)
		{
			var result = new StringBuilder(value.Length);

			foreach (char symbol in value)
			{
				if (UnreservedChars.IndexOf(symbol) != -1)
				{
					result.Append(symbol);
				}
				else
				{
					result.Append('%');
					result.Append(((int)symbol).ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			return result.ToString();
		}

		/// <summary>
		/// Creates the public folder URL.
		/// </summary>
		/// <param name="exchangeServerUrl">The exchange server URL.</param>
		/// <param name="publicFolderName">Name of the public folder.</param>
		/// <returns>The public folder URL</returns>
		public static Uri CreatePublicFolderUrl(string exchangeServerUrl, string publicFolderName)
		{
			string rootUrl = exchangeServerUrl.EnsureEndsWith("/") + PublicFolder + "/" + UrlEncodeWithUppercasePercent20(publicFolderName) + "/";
			return new Uri(rootUrl);
		}

		/// <summary>
		/// Creates the credentials.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>The network credential object</returns>
		public static NetworkCredential CreateCredentials(string userName, string password)
		{
			string[] domainUser = userName.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

			if (domainUser.Length > 1)
				return CreateCredentials(domainUser[1], password, domainUser[0]);

			return new NetworkCredential(userName, password);
		}

		/// <summary>
		/// Creates the credentials.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="domain">The domain.</param>
		/// <returns>The network credential object</returns>
		public static NetworkCredential CreateCredentials(string userName, string password, string domain)
		{
			return new NetworkCredential(userName, password, domain);
		}


	}
}
