//--------------------------------------------------------------------------
// File:    PathHelper.cs
// Content:	Implementation of class PathHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Path helpers
	/// </summary>
	public static class PathHelper
	{
		private readonly static char[] ms_Separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

		/// <summary>
		/// The famous MAX_PATH constant
		/// </summary>
		public const int MAX_PATH = 259;

		/// <summary>
		/// Gets the current physical path.
		/// </summary>
		/// <value>The physical path.</value>
		/// <remarks>this method considers the hosting environment in order
		/// to determine how to evaluate the physical path</remarks>
		public static string CurrentPhysicalPath
		{
			get
			{
				// if hosted in IIS
				string physicalPath = HostingEnvironment.ApplicationPhysicalPath;
				if (String.IsNullOrEmpty(physicalPath))
				{
					// for hosting outside of IIS
					physicalPath = Directory.GetCurrentDirectory();
				}
				return physicalPath;
			}
		}

		/// <summary>
		/// Escapes the path.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Escaped path.</returns>
		public static string EscapeFileName(string fileName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(fileName, "fileName");

			char[] chArray = fileName.ToCharArray();
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			for (int i = 0; i < chArray.Length; i++)
			{
				char ch = chArray[i];
				if (invalidFileNameChars.Any(t => ch == t))
				{
					chArray[i] = '_';
				}
			}
			return new string(chArray);
		}

		/// <summary>
		/// Escapes, and ensures uniqueness
		/// </summary>
		/// <param name="fileName">Suggested file name.</param>
		/// <returns>Escapes and unique file name.</returns>
		public static string GetRobustFileName(string fileName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(fileName, "fileName");
			fileName = EscapeFileName(fileName);
			fileName = GetUniqueFileName(fileName);
			return fileName;
		}

		/// <summary>
		/// Gets a UNC path.
		/// </summary>
		/// <param name="serverName">Name of the server.</param>
		/// <param name="shareName">Name of the share.</param>
		/// <param name="path">The path.</param>
		/// <returns>A UNC path</returns>
		public static string GetUnc(string serverName, string shareName, string path)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(serverName, "serverName");
			ArgChecker.ShouldNotBeNullOrEmpty(shareName, "shareName");

			string str = @"\\{0}\{1}".SafeFormatWith(serverName, shareName);
			if (!String.IsNullOrEmpty(path))
			{
				str = Path.Combine(str, path);
			}
			return str;
		}

		/// <summary>
		/// Gets a unique directory name
		/// </summary>
		/// <param name="directoryName"></param>
		/// <returns></returns>
		public static string GetUniqueDirectoryName(string directoryName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(directoryName, "directoryName");

			string directory = Path.GetDirectoryName(directoryName);
			string fileName = Path.GetFileName(directoryName);
			string extension = string.Empty;

			return GetUniqueIoName(directory, fileName, extension, Directory.Exists);
		}

		/// <summary>
		/// Gets the name of the unique file.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>A unique filename</returns>
		public static string GetUniqueFileName(string fileName)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(fileName, "fileName");

			string directoryName = Path.GetDirectoryName(fileName);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);

			return GetUniqueFileName(directoryName, fileNameWithoutExtension, extension);
		}

		/// <summary>
		/// Gets the name of the unique file.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="name">The name.</param>
		/// <param name="extension">The extension.</param>
		/// <returns>A unique filename.</returns>
		public static string GetUniqueFileName(string directory, string name, string extension)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(directory, "directory");
			ArgChecker.ShouldNotBeNull(name, "name");
			ArgChecker.ShouldNotBeNull(extension, "extension");

			return GetUniqueIoName(directory, name, extension, File.Exists);
		}

		/// <summary>
		/// Ensure that path ends with a slash "\".
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>path that ends with a slash "\" char.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="path"/> is <see langword="null"/>.</exception>
		public static string EnsureEndsWithSlash(string path)
		{
			ArgChecker.ShouldNotBeNull(path, "path");

			if (path.EndsWith(@"\"))
				return path;

			return path + @"\";
		}

		/// <summary>
		/// Ensure that path starts with a slash "\".
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>path that starts with a slash "\" char.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="path"/> is <see langword="null"/>.</exception>
		public static string EnsureStartsWithSlash(string path)
		{
			ArgChecker.ShouldNotBeNull(path, "path");

			if (path.StartsWith(@"\")) { return path; }

			return @"\" + path;
		}

		/// <summary>
		/// Determines whether the specified path is an URL.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>
		/// 	<see langword="true"/> if the specified path is an URL; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool IsUrl(string path)
		{
			return path.IsNullOrEmptyWithTrim().IsFalse() && path.IsMatchingTo(RegularExpression.RegexExpressionStrings.UrlExpression);
		}

		/// <summary>
		/// Combines multi path segments to a single path.
		/// </summary>
		/// <param name="basePath">The base path.</param>
		/// <param name="paths">The path segments.</param>
		/// <returns>A string containing the combined paths.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="basePath"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgException{TValue}">
		/// Is thrown if <paramref name="basePath"/> or any item of <paramref name="paths"/> contains invalid path characters.
		/// </exception>
		public static string CombinePath(string basePath, params string[] paths)
		{
			#region PreConditions
			ArgChecker.ShouldNotBeNull(basePath, "basePath");

			if (basePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
				throw new ArgException<string>(basePath, "basePath", "Argument {0} error: {0} contains invalid path characters (value = {1}");
			#endregion

			paths.Foreach(path =>
			{
				ArgChecker.ShouldNotBeNull(path, "paths");
				if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
				{
					throw new ArgException<string>(path, "paths",
												   "Argument {0} error: {0} contains invalid path characters (value = {1}");
				}
			});

			return paths.Aggregate(basePath, Path.Combine);
		}

		/// <summary>
		/// Converts a given absolute path and a given base path to a path that leads
		/// from the base path to the absoulte path. (as a relative path)
		/// </summary>
		/// <param name="baseDirectoryPath">The base directory</param>
		/// <param name="absPath">The absolute path.</param>
		/// <returns>The path of absPath relative to baseDirectoryPath.</returns>
		/// <exception cref="ArgNullException">
		/// Is thrown if <paramref name="baseDirectoryPath"/> or <paramref name="absPath"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgEmptyException">
		/// Is thrown if <paramref name="baseDirectoryPath"/> or <paramref name="absPath"/> is empty.
		/// </exception>
		public static string GetRelativePath(string baseDirectoryPath, string absPath)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(baseDirectoryPath, "baseDirectoryPath");
			ArgChecker.ShouldNotBeNullOrEmpty(absPath, "absPath");

			if (IsUrl(absPath) || IsUrl(baseDirectoryPath))
			{
				return absPath;
			}

			baseDirectoryPath = NormalizePath(baseDirectoryPath);
			absPath = NormalizePath(absPath);

			string[] bPath = baseDirectoryPath.Split(ms_Separators);
			string[] aPath = absPath.Split(ms_Separators);
			int indx = 0;

			for (; indx < Math.Min(bPath.Length, aPath.Length); ++indx)
			{
				if (!bPath[indx].Equals(aPath[indx], StringComparison.OrdinalIgnoreCase))
					break;
			}

			if (indx == 0)
			{
				return absPath;
			}

			var result = new StringBuilder();

			if (indx != bPath.Length)
			{
				for (int i = indx; i < bPath.Length; ++i)
				{
					result.Append("..");
					result.Append(Path.DirectorySeparatorChar);
				}
			}
			result.Append(String.Join(Path.DirectorySeparatorChar.ToString(), aPath, indx, aPath.Length - indx));
			return result.ToString();
		}

		/// <summary>
		/// Combines baseDirectoryPath with relPath and normalizes the resulting path.
		/// </summary>
		/// <param name="baseDirectoryPath">The base directory</param>
		/// <param name="relPath">The relative path.</param>
		/// <returns>The absolute path of relPath.</returns>
		/// <exception cref="ArgNullException">
		/// Is thrown if <paramref name="baseDirectoryPath"/> or <paramref name="relPath"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgEmptyException">
		/// Is thrown if <paramref name="baseDirectoryPath"/> is empty.
		/// </exception>
		public static string GetAbsolutePath(string baseDirectoryPath, string relPath)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(baseDirectoryPath, "baseDirectoryPath");
			ArgChecker.ShouldNotBeNull(relPath, "relPath");

			return NormalizePath(Path.Combine(baseDirectoryPath, relPath));
		}

		/// <summary>
		/// Gets the app.config directory or the current directory if app.config directory is null.
		/// </summary>
		/// <returns>
		/// Returns the app.config directory.
		/// </returns>
		public static string GetAppConfigDirectory()
		{
			return Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) ?? Directory.GetCurrentDirectory();
		}

		/// <summary>
		/// Gets the normalized version of <paramref name="fileName"/>.
		/// Slashes are replaced with backslashes, backreferences "." and ".." are 'evaluated'.
		/// </summary>
		/// <param name="fileName">The filename that should be normalized.</param>
		/// <returns>The normalized version of <paramref name="fileName"/>.</returns>
		public static string NormalizePath(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) return fileName;

			int i;

			bool isWeb = false;

			for (i = 0; i < fileName.Length; i++)
			{
				if (fileName[i] == '/' || fileName[i] == '\\')
					break;

				if (fileName[i] != ':') continue;

				if (i > 1)
					isWeb = true;
				break;
			}

			char outputSeparator = isWeb ? '/' : Path.DirectorySeparatorChar;

			var result = new StringBuilder();
			if (isWeb == false && fileName.StartsWith(@"\\") || fileName.StartsWith("//"))
			{
				i = 2;
				result.Append(outputSeparator);
			}
			else
			{
				i = 0;
			}

			int segmentStartPos = i;
			for (; i <= fileName.Length; i++)
			{
				if ((i != fileName.Length && fileName[i] != '/') && fileName[i] != '\\') continue;

				int segmentLength = i - segmentStartPos;
				switch (segmentLength)
				{
					case 0:
						// ignore empty segment (if not in web mode)
						// On unix, don't ignore empty segment if i==0
						if (isWeb || (i == 0 && Environment.OSVersion.Platform == PlatformID.Unix))
						{
							result.Append(outputSeparator);
						}
						break;
					case 1:
						// ignore /./ segment, but append other one-letter segments
						if (fileName[segmentStartPos] != '.')
						{
							if (result.Length > 0)
								result.Append(outputSeparator);

							result.Append(fileName[segmentStartPos]);
						}
						break;
					case 2:
						if (fileName[segmentStartPos] == '.' && fileName[segmentStartPos + 1] == '.')
						{
							// remove previous segment
							int j;

							for (j = result.Length - 1; j >= 0 && result[j] != outputSeparator; j--) { }

							if (j > 0)
							{
								result.Length = j;
							}
							break;
						}

						// append normal segment
						goto default;

					default:
						if (result.Length > 0)
							result.Append(outputSeparator);

						result.Append(fileName, segmentStartPos, segmentLength);
						break;
				}
				segmentStartPos = i + 1; // remember start position for next segment
			}

			if (isWeb == false)
			{
				if (result.Length > 0 && result[result.Length - 1] == outputSeparator)
				{
					result.Length -= 1;
				}
				if (result.Length == 2 && result[1] == ':')
				{
					result.Append(outputSeparator);
				}
			}
			return result.ToString();
		}

		private static string GetUniqueIoName(string directory, string name, string extension, Converter<string, bool> exists)
		{
			ArgChecker.ShouldNotBeNullOrEmpty(directory, "directory");
			
			if (!Directory.Exists(directory))
				throw new ArgException<string>(directory, "directory");

			string str = name.TrimEnd(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
			int num = 1;
			string fileName = name + extension;
			if (!String.IsNullOrEmpty(directory))
			{
				fileName = Path.Combine(directory, fileName);
			}

			while (exists(fileName))
			{
				fileName = Path.Combine(directory, "{0}{1:00}{2}".SafeFormatWith(str, num++, extension));
			}

			return fileName;
		}
	}


}
