//--------------------------------------------------------------------------
// File:    FileInfoExtensions.cs
// Content:	Implementation of class FileInfoExtensions
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Contains extension methods for the <see cref="FileInfo"/> type.
	/// </summary>
	public static class FileInfoExtensions
	{
		/// <summary>
		/// Renames a file.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="newName">The new name.</param>
		/// <returns>The file info result.</returns>
		/// <example>
		/// <code>
		/// var file = new FileInfo(@"c:\test.txt");
		/// file.Rename("test2.txt");
		/// </code></example>
		public static FileInfo Rename( this FileInfo file, string newName )
		{
			ArgChecker.ShouldNotBeNull(file, "file");
			ArgChecker.ShouldNotBeNull(file.FullName, "file");
			ArgChecker.ShouldBeExistingFile(file.FullName, "file");
			ArgChecker.ShouldNotBeNullOrEmpty(newName, "newName");

			string sourceFileDirectory = Path.GetDirectoryName(file.FullName);
			ArgChecker.ShouldBeExistingDirectory(sourceFileDirectory, "file");

			var filePath = Path.Combine(sourceFileDirectory, newName);
			file.MoveTo(filePath);

			return new FileInfo(filePath);
		}

		/// <summary>
		/// Renames a without changing its extension.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="newName">The new name.</param>
		/// <returns>The file info result.</returns>
		/// <example>
		/// <code>
		/// var file = new FileInfo(@"c:\test.txt");
		/// file.RenameFileWithoutExtension("test3");
		/// </code></example>
		public static FileInfo RenameFileWithoutExtension( this FileInfo file, string newName )
		{
			ArgChecker.ShouldNotBeNull(file, "file");
			ArgChecker.ShouldNotBeNullOrEmpty(newName, "newName");

			var fileName = string.Concat(newName, file.Extension);

			return file.Rename(fileName);
		}

		/// <summary>
		/// Changes the files extension.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="newExtension">The new extension.</param>
		/// <returns>The file info result.</returns>
		/// <example>
		/// <code>
		/// var file = new FileInfo(@"c:\test.txt");
		/// file.ChangeExtension("xml");
		/// </code></example>
		public static FileInfo ChangeExtension( this FileInfo file, string newExtension )
		{
			ArgChecker.ShouldNotBeNull(file, "file");
			ArgChecker.ShouldNotBeNullOrEmpty(newExtension, "newExtension");

			if (newExtension.StartsWith(".") == false)
				newExtension = "." + newExtension;

			var fileName = string.Concat(Path.GetFileNameWithoutExtension(file.FullName), newExtension);

			return file.Rename(fileName);
		}

		/// <summary>
		/// Changes the extensions of several files at once.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="newExtension">The new extension.</param>
		/// <returns>The file info collection.</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.ChangeExtensions("tmp");
		/// </code></example>
		public static IEnumerable<FileInfo> ChangeExtensions( this IEnumerable<FileInfo> files, string newExtension )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			ArgChecker.ShouldNotBeNullOrEmpty(newExtension, "newExtension");

			var changedFiles = new List<FileInfo>();

			files.Foreach(f => changedFiles.Add(f.ChangeExtension(newExtension)));

			return changedFiles;
		}

		/// <summary>
		/// Deletes several files at once and consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.Delete()
		/// </code></example>
		public static void Delete( this IEnumerable<FileInfo> files )
		{
			ArgChecker.ShouldNotBeNull(files, "files");

			files.Delete(true);
		}

		/// <summary>
		/// Deletes several files at once and optionally consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="consolidateExceptions">if set to <see langword="true"/> exceptions are consolidated and the processing is not interrupted.</param>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.Delete()
		/// </code></example>
		public static void Delete( this IEnumerable<FileInfo> files, bool consolidateExceptions )
		{
			ArgChecker.ShouldNotBeNull(files, "files");

			List<Exception> exceptions = null;

			foreach ( var file in files )
			{
				try
				{
					file.Delete();
				}
				catch ( Exception e )
				{
					if (!consolidateExceptions)
						throw;

					if (exceptions == null)
						exceptions = new List<Exception>();
					exceptions.Add(e);
				}
			}

			if ( ( exceptions != null ) && ( exceptions.Count > 0 ) )
			{
				throw new CombinedException(
					"Error while deleting one or several files, see InnerExceptions array for details.",
					exceptions.ToArray()
				);
			}
		}

		/// <summary>
		/// Copies several files to a new folder at once and consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="targetPath">The target path.</param>
		/// <returns>The newly created file copies</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// var copiedFiles = files.CopyTo(@"c:\temp\");
		/// </code></example>
		public static IEnumerable<FileInfo> CopyTo( this IEnumerable<FileInfo> files, string targetPath )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			ArgChecker.ShouldNotBeNullOrEmpty(targetPath, "targetPath");

			return files.CopyTo(targetPath, true);
		}

		/// <summary>
		/// Copies several files to a new folder at once and optionally consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="consolidateExceptions">if set to <see langword="true"/> exceptions are consolidated and the processing is not interrupted.</param>
		/// <returns>The newly created file copies</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// var copiedFiles = files.CopyTo(@"c:\temp\");
		/// </code></example>
		public static IEnumerable<FileInfo> CopyTo( this IEnumerable<FileInfo> files, string targetPath, bool consolidateExceptions )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			ArgChecker.ShouldNotBeNullOrEmpty(targetPath, "targetPath");

			var copiedfiles = new List<FileInfo>();
			List<Exception> exceptions = null;

			foreach ( var file in files )
			{
				try
				{
					var fileName = Path.Combine(targetPath, file.Name);
					copiedfiles.Add(file.CopyTo(fileName));
				}
				catch ( Exception e )
				{
					if (!consolidateExceptions)
						throw;
					
					if (exceptions == null)
						exceptions = new List<Exception>();
					exceptions.Add(e);
				}
			}

			if ( ( exceptions != null ) && ( exceptions.Count > 0 ) )
			{
				throw new CombinedException("Error while copying one or several files, see InnerExceptions array for details.", exceptions);
			}

			return copiedfiles;
		}

		/// <summary>
		/// Moves several files to a new folder at once and optionally consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="targetPath">The target path.</param>
		/// <returns>The moved files</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.MoveTo(@"c:\temp\");
		/// </code></example>
		public static IEnumerable<FileInfo> MoveTo( this IEnumerable<FileInfo> files, string targetPath )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			ArgChecker.ShouldNotBeNullOrEmpty(targetPath, "targetPath");

			return files.MoveTo(targetPath, true);
		}

		/// <summary>
		/// Movies several files to a new folder at once and optionally consolidates any exceptions.
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="targetPath">The target path.</param>
		/// <param name="consolidateExceptions">if set to <see langword="true"/> exceptions are consolidated and the processing is not interrupted.</param>
		/// <returns>The moved files</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.MoveTo(@"c:\temp\");
		/// </code></example>
		public static IEnumerable<FileInfo> MoveTo( this IEnumerable<FileInfo> files, string targetPath, bool consolidateExceptions )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			ArgChecker.ShouldNotBeNullOrEmpty(targetPath, "targetPath");

			var movedfiles = new List<FileInfo>();
			List<Exception> exceptions = null;

			FileHelper.CreateDirectory(targetPath);

			foreach ( var file in files )
			{
				try
				{
					var fileName = Path.Combine(targetPath, file.Name);
					file.MoveTo(fileName);
					movedfiles.Add(new FileInfo(fileName));
				}
				catch ( Exception e )
				{
					if ( consolidateExceptions )
					{
						if ( exceptions == null ) exceptions = new List<Exception>();
						exceptions.Add(e);
					}
					else
					{
						throw;
					}
				}
			}

			if ( ( exceptions != null ) && ( exceptions.Count > 0 ) )
			{
				throw new CombinedException("Error while moving one or several files, see InnerExceptions array for details.", exceptions);
			}

			return movedfiles;
		}

		/// <summary>
		/// Sets file attributes for several files at once
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="attributes">The attributes to be set.</param>
		/// <returns>The changed files</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.SetAttributes(FileAttributes.Archive);
		/// </code></example>
		public static IEnumerable<FileInfo> SetAttributes( this IEnumerable<FileInfo> files, FileAttributes attributes )
		{
			ArgChecker.ShouldNotBeNull(files, "files");

			var changedFiles = new List<FileInfo>();

			foreach ( var file in files )
			{
				file.Attributes = attributes;
				changedFiles.Add(new FileInfo(file.FullName));
			}
			return changedFiles;
		}

		/// <summary>
		/// Appends file attributes for several files at once (additive to any existing attributes)
		/// </summary>
		/// <param name="files">The files.</param>
		/// <param name="attributes">The attributes to be set.</param>
		/// <returns>The changed files</returns>
		/// <example>
		/// <code>
		/// var files = directory.GetFiles("*.txt", "*.xml");
		/// files.SetAttributesAdditive(FileAttributes.Archive);
		/// </code></example>
		public static IEnumerable<FileInfo> SetAttributesAdditive( this IEnumerable<FileInfo> files, FileAttributes attributes )
		{
			ArgChecker.ShouldNotBeNull(files, "files");
			
			var changedFiles = new List<FileInfo>();

			foreach ( var file in files )
			{
				file.Attributes = ( file.Attributes | attributes );
				changedFiles.Add(new FileInfo(file.FullName));
			}
			return changedFiles;
		}

		/// <summary>
		/// Reads the lines of the file referenced by the FileInfo object.
		/// </summary>
		/// <param name="file">The file info.</param>
		/// <returns>The text lines of the file referenced by the FileInfo object.</returns>
		public static IEnumerable<string> ReadLines( this FileInfo file)
		{
			if (file == null)
				yield break;

			using (StreamReader sr = file.OpenText())
			{
				sr.EnumLines();
			}
		}


		/// <summary>
		/// Reads the lines of the file referenced by the FileInfo object. Provides line number information for each line.
		/// </summary>
		/// <param name="file">The file info.</param>
		/// <returns>The text lines + line number info of the file referenced by the FileInfo object.</returns>
		public static IEnumerable<IndexValuePair<string>> ReadLinesWithLineNumbers( this FileInfo file )
		{
			if ( file == null )
				yield break;

			using ( StreamReader streamReader = file.OpenText() )
			{
				int lineNumber = 0;
				while (!streamReader.EndOfStream)
				{
					var line = streamReader.ReadLine();
					lineNumber++;
					yield return new IndexValuePair<string>(line, lineNumber);
				}
			}
		}
	}
}
