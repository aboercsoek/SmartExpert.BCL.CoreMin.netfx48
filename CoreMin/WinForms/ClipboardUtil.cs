//--------------------------------------------------------------------------
// File:    ClipboardUtil.cs
// Content:	Implementation of class ClipboardUtil
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartExpert;
using SmartExpert.IO;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.WinForms
{
	/// <summary>
	/// Clipboard helper class
	/// </summary>
	public static class ClipboardUtil
	{
		/// <summary>
		/// Gets the unicode text value.
		/// </summary>
		/// <returns>
		/// Returns the unicode text value.
		/// </returns>
		public static string GetUnicodeText()
		{
			var unicodetextFormat = DataFormats.UnicodeText;

			if (!Clipboard.ContainsData(unicodetextFormat))
			{
				return null;
			}

			var dataobject = Clipboard.GetDataObject();

			if (dataobject == null)
			{
				return null;
			}

			if (!dataobject.GetDataPresent(unicodetextFormat))
			{
				return null;
			}

			string text = Clipboard.GetText();
			return text;
		}
		
		/// <summary>
		/// Returns CSV data from the clipboard. If there is no CSV data, null is returned
		/// </summary>
		/// <returns>CSV data as a string</returns>
		public static string GetCsv()
		{
			var csvFormat = DataFormats.CommaSeparatedValue;

			if (!Clipboard.ContainsData(csvFormat))
			{
				return null;
			}

			var dataobject = Clipboard.GetDataObject();

			if (dataobject == null)
			{
				return null;
			}

			if (!dataobject.GetDataPresent(csvFormat))
			{
				return null;
			}

			var stream = (Stream) dataobject.GetData(csvFormat);
			var encoding = Encoding.Default;
			var reader = new StreamReader(stream, encoding);
			string csvdata = reader.ReadToEnd();

			reader.Close();
			return csvdata;
		}

		/// <summary>
		/// Returns Bitmap data from the clipboard. If there is no CSV data, null is returned
		/// </summary>
		/// <returns>The Bitmap object</returns>
		public static System.Drawing.Bitmap GetBitmap()
		{
			var bitmapFormat = DataFormats.Bitmap;

			if (!Clipboard.ContainsData(bitmapFormat))
			{
				return null;
			}

			var dataobject = Clipboard.GetDataObject();

			if (dataobject == null)
			{
				return null;
			}

			if (!dataobject.GetDataPresent(bitmapFormat))
			{
				return null;
			}

			var bmp = (System.Drawing.Bitmap)dataobject.GetData(bitmapFormat);
			return bmp;
		}

		/// <summary>
		/// Returns HTML from the clipboard. If there is no HTML data, null is returned
		/// </summary>
		/// <returns>the HTML as a string</returns>
		public static string GetHtml()
		{
			const TextDataFormat htmlFormat = TextDataFormat.Html;

			if (!Clipboard.ContainsText(htmlFormat))
				return null;

			var fragment = HtmlCLipboardData.FromClipboard();

			if (fragment.HtmlFragment == null)
				return null;

			// The string returned from HTML Fragment contains UTF-8 text
			// convert it to plain unicode string and return it
			var unicodeString = fragment.HtmlFragment.ToUnicodeString(Encoding.UTF8);
			return unicodeString;
		}

		/// <summary>
		/// Sets the HTML.
		/// </summary>
		/// <param name="htmlFragment">The HTML fragment.</param>
		public static void SetHtml(string htmlFragment)
		{
			if (htmlFragment == null)
			{
				throw new ArgumentNullException("htmlFragment");
			}

			string cfHtml = HtmlCLipboardData.GetCfHtmlString(htmlFragment, null, null);

			Clipboard.SetText(cfHtml, TextDataFormat.Html);
		}

		/// <summary>
		/// Sets the CSV data from a data table.
		/// </summary>
		/// <param name="dataobject">The dataobject.</param>
		/// <param name="datatable">The datatable.</param>
		public static void SetDataCsvFromTable(IDataObject dataobject, DataTable datatable)
		{
			var defaultEncoding = Encoding.Default;
			var outMemstream = new MemoryStream();
			var streamwriter = new StreamWriter(outMemstream, defaultEncoding);
			var csvWriter = new CsvWriter(streamwriter);

			ExportToCsv(datatable, csvWriter);
			
			var bytes = outMemstream.ToArray();
			var inMemstream = new MemoryStream(bytes);
			dataobject.SetData(DataFormats.CommaSeparatedValue, inMemstream);
		}

		private static void ExportToCsv(DataTable datatable, CsvWriter csvwriter)
		{
			foreach (DataRow row in datatable.Rows)
			{
				csvwriter.WriteItems(row.ItemArray);
			}

			csvwriter.Close();
		}
	}
}