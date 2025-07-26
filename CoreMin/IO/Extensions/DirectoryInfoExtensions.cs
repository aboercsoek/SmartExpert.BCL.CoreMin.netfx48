//--------------------------------------------------------------------------
// File:    DirectoryInfoExtensions.cs
// Content:	Implementation of class DirectoryInfoExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Extension methods for the DirectoryInfo class
	/// </summary>
	public static class DirectoryInfoExtensions
	{

		/// <summary>
		/// Gets all files in the directory matching one of the several (!) supplied patterns (instead of just one in the regular implementation).
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="patterns">The patterns.</param>
		/// <returns>The matching files</returns>
		/// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// </code></example>
		public static IEnumerable<FileInfo> GetFiles( this DirectoryInfo directory, params string[] patterns )
		{
			var files = new List<FileInfo>();
			foreach ( var pattern in patterns )
			{
				files.AddRange(directory.GetFiles(pattern));
			}
			return files;
		}

		/// <summary>
		/// Searches the provided directory recursively and returns the first file matching the provided pattern.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="pattern">The pattern.</param>
		/// <returns>The found file</returns>
		/// <example>
		/// <code>
		/// var directory = new DirectoryInfo(@"c:\");
		/// var file = directory.FindFileRecursive("win.ini");
		/// </code></example>
		public static FileInfo FindFileRecursive( this DirectoryInfo directory, string pattern )
		{
			var files = directory.GetFiles(pattern);
			if ( files.Length > 0 ) return files[0];

			return directory.GetDirectories()
					.Select(subDirectory => subDirectory.FindFileRecursive(pattern))
					.FirstOrDefault(foundFile => foundFile != null);
		}

		/// <summary>
		/// Searches the provided directory recursively and returns the first file matching to the provided predicate.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The found file</returns>
		/// <example>
		/// <code>
		/// var directory = new DirectoryInfo(@"c:\");
		/// var file = directory.FindFileRecursive(f => f.Extension == ".ini");
		/// </code></example>
		public static FileInfo FindFileRecursive( this DirectoryInfo directory, Func<FileInfo, bool> predicate )
		{
			foreach (var file in directory.GetFiles().Where(predicate))
			{
				return file;
			}

			return directory.GetDirectories()
					.Select(subDirectory => subDirectory.FindFileRecursive(predicate))
					.FirstOrDefault(foundFile => foundFile != null);
		}

		/// <summary>
		/// Searches the provided directory recursively and returns the all files matching the provided pattern.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="isRegEx">If set to <see langword="true"/>, <paramref name="pattern"/> is interpreted as a regular expression string.</param>
		/// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Collection extension methods.</remarks>
		/// <returns>The found files</returns>
		/// <example>
		/// <code>
		/// var directory = new DirectoryInfo(@"c:\");
		/// var files = directory.FindFilesRecursive("*.ini");
		/// </code></example>
		public static IEnumerable<FileInfo> FindFilesRecursive( this DirectoryInfo directory, string pattern, bool isRegEx )
		{
			var foundFiles = new List<FileInfo>();
			if ( isRegEx )
			{
				Regex regEx = new Regex(pattern);
				FindFilesRecursive(directory, regEx, foundFiles);
			}
			else
			{
				FindFilesRecursive(directory, pattern, foundFiles);
			}
			return foundFiles;
		}

		private static void FindFilesRecursive( DirectoryInfo directory, Regex regexPattern, ICollection<FileInfo> foundFiles )
		{

			foundFiles.AddRange(directory.GetFiles().Where(f => regexPattern.IsMatch(f.Name)));
			directory.GetDirectories().Foreach<DirectoryInfo>(d => FindFilesRecursive(d, regexPattern, foundFiles));
		}

		/// <summary>
		/// Searches the provided directory recursively and returns the all files matching the provided pattern.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="pattern">The pattern.</param>
		/// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
		/// <returns>The found files</returns>
		/// <example>
		/// <code>
		/// var directory = new DirectoryInfo(@"c:\");
		/// var files = directory.FindFilesRecursive("*.ini");
		/// </code></example>
		public static IEnumerable<FileInfo> FindFilesRecursive( this DirectoryInfo directory, string pattern )
		{
			var foundFiles = new List<FileInfo>();
			FindFilesRecursive(directory, pattern, foundFiles);
			return foundFiles;
		}

		private static void FindFilesRecursive( DirectoryInfo directory, string pattern, ICollection<FileInfo> foundFiles )
		{
			foundFiles.AddRange(directory.GetFiles(pattern));
			directory.GetDirectories().Foreach<DirectoryInfo>(d => FindFilesRecursive(d, pattern, foundFiles));
		}

		/// <summary>
		/// Searches the provided directory recursively and returns the all files matching to the provided predicate.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="predicate">The predicate.</param>
		/// <returns>The found files</returns>
		/// <remarks>This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.</remarks>
		/// <example>
		/// <code>
		/// var directory = new DirectoryInfo(@"c:\");
		/// var files = directory.FindFilesRecursive(f => f.Extension == ".ini");
		/// </code></example>
		public static IEnumerable<FileInfo> FindFilesRecursive( this DirectoryInfo directory, Func<FileInfo, bool> predicate )
		{
			var foundFiles = new List<FileInfo>();
			FindFilesRecursive(directory, predicate, foundFiles);
			return foundFiles;
		}

		private static void FindFilesRecursive( DirectoryInfo directory, Func<FileInfo, bool> predicate, ICollection<FileInfo> foundFiles )
		{
			foundFiles.AddRange(directory.GetFiles().Where(predicate));
			directory.GetDirectories().Foreach<DirectoryInfo>(d => FindFilesRecursive(d, predicate, foundFiles));
		}
	}
}
