//--------------------------------------------------------------------------
// File:    ExcelXmlWriter.cs
// Content:	Implementation of disposable class ExcelXmlWriter
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using System.Xml;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	///<summary>Excel XML writer class implementation.</summary>
	public class ExcelXmlWriter : IDisposable
	{
		#region Nested Types

		/// <summary>
		/// Excel XML cell data types
		/// </summary>
		public enum CellDataType
        {
			/// <summary>String cell data type</summary>
			[DisplayName("String")]
            String,
			/// <summary>Number cell data type</summary>
			[DisplayName("Number")]
			Number,
			/// <summary>DateTime cell data type</summary>
			[DisplayName("DateTime")]
			DateTime
        }
		
		#endregion

		#region Private Fields

        private XmlWriter m_XmlWriter;

        private const string NS_SPREADSHEET_ID = "s";
        private const string NS_SPREADSHEET = "urn:schemas-microsoft-com:office:spreadsheet";

        private const string NS_EXCEL_ID = "x";
        private const string NS_EXCEL = "urn:schemas-microsoft-com:office:excel";

        private const string NS_OFFICE_ID = "o";
        private const string NS_OFFICE = "urn:schemas-microsoft-com:office:office";		
		
		#endregion

		#region Ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelXmlWriter"/> class.
		/// </summary>
		/// <param name="filename">The filename.</param>
		public ExcelXmlWriter(string filename)
        {
            var settings = new XmlWriterSettings {Indent = true};
			m_XmlWriter = XmlWriter.Create(filename, settings);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelXmlWriter"/> class.
		/// </summary>
		/// <param name="xmlwriter">The xmlwriter.</param>
        public ExcelXmlWriter(XmlWriter xmlwriter)
        {
            m_XmlWriter = xmlwriter;
        }

		#endregion Ctor

		#region Dispose Pattern

		private bool m_Disposed;

		/// <summary>
		/// Gets a value indicating whether this <see cref="ExcelXmlWriter"/> is disposed.
		/// </summary>
		/// <value><see langword="true"/> if disposed; otherwise, <see langword="false"/>.</value>
		protected bool Disposed
		{
			get
			{
				lock (this)
				{
					return m_Disposed;
				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			lock (this)
			{
				if (m_Disposed == false)
				{
					Dispose(true);
				}
			}
		}

		private void Dispose(bool isDispose)
		{
			// Place release and cleanup operations here
			m_XmlWriter.Close();

			m_Disposed = true;
			if (isDispose)
				GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="ExcelXmlWriter"/> is reclaimed by garbage collection.
		/// </summary>
		~ExcelXmlWriter()
		{
			Dispose(false);
		}

		#endregion

		#region Public ExcelXmlWriter Members
		
		/// <summary>
		/// Write XML declaration and Excel XML processing instructions.
		/// </summary>
		public void StartDocument()
        {
            m_XmlWriter.WriteStartDocument();
            m_XmlWriter.WriteProcessingInstruction("mso-application", "progid='Excel.Sheet'");

        }

		/// <summary>
		/// Close all open elements.
		/// </summary>
        public void EndDocument()
        {
            m_XmlWriter.WriteEndDocument();
        }

		/// <summary>
		/// Write the Excel XML work book start element.
		/// </summary>
        public void StartWorkBook()
        {
            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Workbook", NS_SPREADSHEET);
            m_XmlWriter.WriteAttributeString(NS_EXCEL_ID, "xmlns", NS_EXCEL);
            m_XmlWriter.WriteAttributeString(NS_OFFICE_ID, "xmlns", NS_OFFICE);
            m_XmlWriter.WriteAttributeString(NS_SPREADSHEET_ID, "xmlns", NS_SPREADSHEET);

        }

		/// <summary>
		/// Close the Excel XML work book element.
		/// </summary>
        public void EndWorkBook()
        {
            m_XmlWriter.WriteEndElement();
        }

		/// <summary>
		/// Write the Excel XML work sheet start element.
		/// </summary>
		/// <param name="name">The name of the work sheet.</param>
		/// <param name="numColumns">The number of columns.</param>
        public void StartWorkSheet(string name, int numColumns)
        {
            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Worksheet", NS_SPREADSHEET);
            m_XmlWriter.WriteAttributeString(NS_SPREADSHEET_ID, "Name", NS_SPREADSHEET, name);
            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Table", NS_SPREADSHEET);

            for (int i = 0; i < numColumns;i++ )
            {
                m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Column", NS_SPREADSHEET);
                m_XmlWriter.WriteEndElement(); // s:Column

            }

        }

		/// <summary>
		/// Close the Excel XML work sheet element.
		/// </summary>
        public void EndWorkSheet()
        {
            m_XmlWriter.WriteEndElement(); // s:Table
            m_XmlWriter.WriteEndElement(); //s:Worksheet
        }

		/// <summary>
		/// Write Excel XML row start element.
		/// </summary>
        public void StartRow()
        {
            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Row", NS_SPREADSHEET);

        }

		/// <summary>
		/// Close Excel XML row element.
		/// </summary>
        public void EndRow()
        {
            m_XmlWriter.WriteEndElement(); // s:Row

        }

		/// <summary>
		/// Writes the Excel XML cell element.
		/// </summary>
		/// <param name="cellData">The cell data.</param>
		/// <param name="cellDataType">The cell data type.</param>
        public void Cell(string cellData, CellDataType cellDataType)
        {
            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Cell", NS_SPREADSHEET);

            m_XmlWriter.WriteStartElement(NS_SPREADSHEET_ID, "Data", NS_SPREADSHEET);
            
			m_XmlWriter.WriteAttributeString(NS_SPREADSHEET_ID, "Type", NS_SPREADSHEET, cellDataType.GetDisplayName());

            m_XmlWriter.WriteString(cellData);
            m_XmlWriter.WriteEndElement(); // s:Data
            m_XmlWriter.WriteEndElement(); // s:Cell

        }

		/// <summary>
		/// Closes this Excel XML writer instance by calling <see cref="Dispose()"/>
		/// </summary>
        public void Close()
        {
        	Dispose();
        }
	
		#endregion
	}
}
