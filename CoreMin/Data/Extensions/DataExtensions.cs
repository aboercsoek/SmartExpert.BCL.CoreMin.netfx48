//--------------------------------------------------------------------------
// File:    DataExtensions.cs
// Content:	Implementation of class DataExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

// Uses the same namespace as DataColumnCollection and DataRowCollection therewith the extension methods are available by using the System.Data namespace.
namespace SmartExpert.Data
{
	///<summary>Represents extension methods for <see cref="DataColumnCollection"/> and  <see cref="DataRowCollection"/> type.</summary>
	public static class DataExtensions
	{

		/// <summary>
		/// Converts a DataColumnCollection into a IEnumerable-Collection.
		/// </summary>
		/// <param name="dataColumns">The data columns.</param>
		/// <returns>IEnumerable-Collection of DataColumns</returns>
		/// <exception cref="ArgNullException"><paramref name="dataColumns"/> is <see langword="null"/>.</exception>
		public static IEnumerable<DataColumn> AsEnumerable( this DataColumnCollection dataColumns )
		{
			ArgChecker.ShouldNotBeNull(dataColumns, "dataColumns");

			return Enumerable.Cast<DataColumn>(dataColumns);
		}


		/// <summary>
		/// Converts a DataRowCollection into a IEnumerable-Collection.
		/// </summary>
		/// <param name="dataRows">The data rows.</param>
		/// <returns>IEnumerable-Collection of DataRows</returns>
		/// <exception cref="ArgNullException"><paramref name="dataRows"/> is <see langword="null"/>.</exception>
		public static IEnumerable<DataRow> AsEnumerable( this DataRowCollection dataRows )
		{
			ArgChecker.ShouldNotBeNull(dataRows, "dataRows");

			return Enumerable.Cast<DataRow>(dataRows);
		}
	}
}
