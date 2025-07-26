//--------------------------------------------------------------------------
// File:    DbDataHelper.cs
// Content:	Implementation of class DbDataHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SmartExpert;
using SmartExpert.Linq;
using SmartExpert.Reflection;

#endregion

namespace SmartExpert.Data
{
	/// <summary>
	/// DataRow and DataTable Extensions.
	/// </summary>
	public static class DbDataHelper
	{

		#region Public Static Methods (4)

		/// <summary>
		/// Gets the value of a data row fields at a given column index.
		/// If field contains DBNull value, the method returns the default value of T.
		/// </summary>
		/// <typeparam name="T">Type in witch the row value should be converted.</typeparam>
		/// <param name="row">Data row object</param>
		/// <param name="colIndex">Column index</param>
		/// <returns>Value of the data row field</returns>
		public static T GetValue<T>( this DataRow row, int colIndex )
		{
			ArgChecker.ShouldNotBeNull(row, "row");
			if ( colIndex < 0 )
				throw new ArgumentOutOfRangeException("colIndex", colIndex, @"colIndex is negative!");

			if ( colIndex >= row.ItemArray.Length )
				return default(T);

			if ( row[colIndex] != DBNull.Value )
				return (T)row[colIndex];
			

			return default(T);
		}

		/// <summary>
		/// Gets the value of a data row fields for a given column name.
		/// If field contains DBNull value, the method returns the default value of T.
		/// </summary>
		/// <typeparam name="T">Type in witch the row value should be converted.</typeparam>
		/// <param name="row">Data row object</param>
		/// <param name="colName">Column name</param>
		/// <returns>Value of the data row field</returns>
		public static T GetValue<T>( this DataRow row, string colName )
		{
			ArgChecker.ShouldNotBeNull(row, "row");
			ArgChecker.ShouldNotBeNullOrEmpty(colName, "colName");

			try
			{
				if ( row[colName] != DBNull.Value )
					return (T)row[colName];
				
				return default(T);
			}
			catch ( Exception )
			{
				return default(T);
			}
		}

		/// <summary>
		/// Convert DataTable into an array of type TTarget.
		/// </summary>
		/// <typeparam name="TRow">DataRow type</typeparam>
		/// <typeparam name="TTarget">Array item type (must have [DataContract] or [Serializable] attribute)</typeparam>
		/// <param name="table">The data table.</param>
		/// <param name="converter">The row converter.</param>
		/// <returns>Array of type TTarget witch contains the elements of the source data row.</returns>
		public static TTarget[] ToArray<TRow, TTarget>( this DataTable table, Func<TRow, TTarget> converter ) where TRow : DataRow
		{
			if ( table.Rows.Count == 0 )
				return new TTarget[] { };

			//Verify [DataContract] or [Serializable] on T
			ArgChecker.ShouldBeTrue((typeof(TTarget)).HasDataContractAttribute() || typeof(TTarget).IsSerializable, "TTarget", "Target array item type must have [DataContract] or [Serializable] attribute.");
			//Verify table contains correct rows 
			ArgChecker.ShouldBeTrue(MatchingTableRow<TRow>(table), "table", "Table row type must match with type TRow.");

			return table.Rows.AsSequence<TRow>().Select(converter).ToArray();
		}

		/// <summary>
		/// Gets the DB-Table content as XML.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>
		/// Returns the DB-Table content as XML.
		/// </returns>
		/// <remarks>Method should only be used for tables where record count is less than 1000.</remarks>
		public static XDocument GetDbTableAsXml(string connectionString, string tableName)
		{
			XDocument result;
			DataSet dataSet = new DataSet("tableName");

			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					using (SqlCommand cmd = new SqlCommand("SELECT * FROM {0}".SafeFormatWith(tableName), conn))
					{
						using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
						{
							adapter.Fill(dataSet);
						}
						result = new XDocument();
						using (XmlWriter xw = result.CreateWriter())
						{
							dataSet.WriteXml(xw);
						}
					}
				}
			}
			finally
			{
				dataSet.Clear();
			}

			return result;
		}
		
		#endregion Public Static Methods

		#region Private Static Methods (1)

		static bool MatchingTableRow<TRow>(DataTable table)
		{
			if ( table.Rows.Count == 0 )
				return true;

			return table.Rows[0] is TRow;
		}

		#endregion Private Static Methods

	}
}
