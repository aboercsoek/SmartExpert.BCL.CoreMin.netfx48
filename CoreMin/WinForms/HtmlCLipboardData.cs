//--------------------------------------------------------------------------
// File:    HtmlCLipboardData.cs
// Content:	Implementation of class HtmlCLipboardData
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.WinForms
{

	/// <summary>
	/// Almost entirely based on this code: http://blogs.msdn.com/jmstall/archive/2007/01/21/html-clipboard.aspx for details.
	/// </summary>
	public class HtmlCLipboardData
	{
		/// <summary>
		/// Gets or sets the HTML fragment.
		/// </summary>
		/// <value>The HTML fragment.</value>
		public string HtmlFragment { get; private set; }
		/// <summary>
		/// Gets or sets the source URL.
		/// </summary>
		/// <value>The source URL.</value>
		public Uri SourceUrl { get; private set; }
		/// <summary>
		/// Gets or sets the start selection.
		/// </summary>
		/// <value>The start selection.</value>
		public int StartSelection { get; private set; }
		/// <summary>
		/// Gets or sets the end selection.
		/// </summary>
		/// <value>The end selection.</value>
		public int EndSelection { get; private set; }
		/// <summary>
		/// Gets the start HTML index.
		/// </summary>
		/// <value>The start index.</value>
		public int StartHtml { get; private set; }
		/// <summary>
		/// Gets the end HTML index.
		/// </summary>
		/// <value>The end HTML index.</value>
		public int EndHtml { get; private set; }
		/// <summary>
		/// Gets or sets the start fragment index.
		/// </summary>
		/// <value>The start fragment index.</value>
		public int StartFragment { get; private set; }
		/// <summary>
		/// Gets or sets the end fragment index.
		/// </summary>
		/// <value>The end fragment index.</value>
		public int EndFragment { get; private set; }

		// ReSharper disable UnaccessedField.Local
		private string m_Version;
		// ReSharper restore UnaccessedField.Local
		private string m_Fulltext;

		/// <summary>
		/// Get a HTML fragment from the clipboard.
		/// </summary>    
		public static HtmlCLipboardData FromClipboard()
		{
			string cfHtmlText = Clipboard.GetText(TextDataFormat.Html);
			var htmlData = new HtmlCLipboardData(cfHtmlText);
			return htmlData;
		}

		/// <summary>
		/// Create an HTML fragment decoder around raw HTML text from the clipboard. 
		/// This text should have the header.
		/// </summary>
		/// <param name="cfHtmlText">raw html text, with header.</param>
		public HtmlCLipboardData(string cfHtmlText)
		{
			// This decodes CF_HTML, which is an entirely text format using UTF-8.
			// Format of this header is described at:
			// http://msdn.microsoft.com/en-us/library/aa767917(VS.85).aspx

			// Info: The counters are byte counts in the original string, which may be ANSI. So byte counts
			// may be the same as character counts (since sizeof(char) == 1).
			// But System.String is unicode, and so byte couns are no longer the same as character counts,
			// (since sizeof(wchar) == 2). 
			var r = new Regex("([a-zA-Z]+):(.+?)[\r\n]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Match m;

			for (m = r.Match(cfHtmlText); m.Success; m = m.NextMatch())
			{
				string key = m.Groups[1].Value.ToLower(System.Globalization.CultureInfo.InvariantCulture);
				string val = m.Groups[2].Value;

				switch (key)
				{
					case "version":
						// Version number of the clipboard. Starting version is 0.9. 
						m_Version = val;
						break;

					case "starthtml":
						// Byte count from the beginning of the clipboard to the start of the context, or -1 if no context
						if (StartHtml != 0)
							throw new FormatException("StartHTML is already declared");

						StartHtml = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
						break;

					case "endhtml":
						// Byte count from the beginning of the clipboard to the end of the context, or -1 if no context.
						if (StartHtml == 0)
							throw new FormatException("StartHTML must be declared before endHTML");

						EndHtml = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);

						m_Fulltext = cfHtmlText.Substring(StartHtml, EndHtml - StartHtml);
						break;

					case "startfragment":
						//  Byte count from the beginning of the clipboard to the start of the fragment.
						if (StartFragment != 0)
							throw new FormatException("StartFragment is already declared");

						StartFragment = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
						break;

					case "endfragment":
						// Byte count from the beginning of the clipboard to the end of the fragment.
						if (StartFragment == 0)
							throw new FormatException("StartFragment must be declared before EndFragment");

						EndFragment = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
						HtmlFragment = cfHtmlText.Substring(StartFragment, EndFragment - StartFragment);
						break;

					case "sourceurl":
						// Optional Source URL, used for resolving relative links.
						SourceUrl = new Uri(val);
						break;

					case "startselection":
						StartSelection = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
						break;

					case "endselection":
						EndSelection = int.Parse(val, System.Globalization.CultureInfo.InvariantCulture);

						break;

					default:
						break;
				}
			}

			if (m_Fulltext == null && HtmlFragment == null)
				throw new FormatException("No data specified");

		}

		// Helper to convert an integer into an 8 digit string.
		// String must be 8 characters, because it will be used to replace an 8 character string within a larger string.    
		private static string To8DigitString(int x)
		{
			return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0,8}", x);
		}

		/// <summary>
		/// Converts a HTML fragment into CF_HTML
		/// </summary>
		/// <param name="htmlFragment">a html fragment</param>
		/// <param name="title">optional title of the HTML document (can be null)</param>
		/// <param name="sourceUrl">optional Source URL of the HTML document, for resolving relative links (can be null)</param>
		/// <returns></returns>
		/// <remarks>CF_HTML format specification: http://msdn.microsoft.com/library/default.asp?url=/workshop/networking/clipboard/htmlclipboard.asp</remarks>
		public static string GetCfHtmlString(string htmlFragment, string title, Uri sourceUrl)
		{
			htmlFragment = Encoding.GetEncoding(0).GetString(Encoding.UTF8.GetBytes(htmlFragment));

			if (title == null)
			{
				title = "From Clipboard";
			}

			var sb = new StringBuilder();

			// Builds the CF_HTML header. See format specification here:
			// http://msdn.microsoft.com/library/default.asp?url=/workshop/networking/clipboard/htmlclipboard.asp

			// The string contains index references to other spots in the string, so we need placeholders so we can compute the offsets. 
			// The <<<<<<<_ strings are just placeholders. We'll backpatch them actual values afterwards.
			// The string layout (<<<) also ensures that it can't appear in the body of the html because the <
			// character must be escaped.
			const string header = @"Format:HTML Format
Version:1.0
StartHTML:<<<<<<<1
EndHTML:<<<<<<<2
StartFragment:<<<<<<<3
EndFragment:<<<<<<<4
StartSelection:<<<<<<<3
EndSelection:<<<<<<<3
";

			string pre =
				@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN""><HTML><HEAD><TITLE>"
				+ title
				+ @"</TITLE></HEAD><BODY><!--StartFragment-->";

			const string post = @"<!--EndFragment--></BODY></HTML>";

			sb.Append(header);
			if (sourceUrl != null)
			{
				sb.AppendFormat("SourceURL:{0}", sourceUrl);
			}

			int startHtml = sb.Length;

			sb.Append(pre);
			int fragmentStart = sb.Length;

			sb.Append(htmlFragment);
			int fragmentEnd = sb.Length;

			sb.Append(post);
			int endHtml = sb.Length;

			// Backpatch offsets
			sb.Replace("<<<<<<<1", To8DigitString(startHtml));
			sb.Replace("<<<<<<<2", To8DigitString(endHtml));
			sb.Replace("<<<<<<<3", To8DigitString(fragmentStart));
			sb.Replace("<<<<<<<4", To8DigitString(fragmentEnd));

			string cfHtml = sb.ToString();
			return cfHtml;
		}
	}
}