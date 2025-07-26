//--------------------------------------------------------------------------
// File:    MultiFileWatcher.cs
// Content:	Implementation of class MultiFileWatcher
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
using SmartExpert.Linq;
using SmartExpert.Messaging;

#endregion

namespace SmartExpert.IO
{
	/// <summary>
	/// Watches multiple files at the same time and raises an event whenever 
	/// a single change is detected in any of those files.
	/// </summary>
	internal class MultiFileWatcher : IDisposable
	{
		private object m_SyncLock = new object();
		private List<FileSystemWatcher> m_Watchers = new List<FileSystemWatcher>();

		/// <summary>
		/// Occurs when a change is detected in one of the monitored files.
		/// </summary>
		public event EventHandler<FileSystemEventArgs> OnChange;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			StopWatching();
		}

		private void OnWatcherChanged(object source, FileSystemEventArgs e)
		{
			lock (m_SyncLock)
			{
				EventHelper.FireEvent(OnChange, source, e);
			}
		}

		/// <summary>
		/// Stops the watching.
		/// </summary>
		public void StopWatching()
		{
			lock (m_SyncLock)
			{
				foreach (FileSystemWatcher watcher in m_Watchers)
				{
					//QuickLogger.Log("Stopping file watching for path '{0}' filter '{1}'".SafeFormatWith(watcher.Path, watcher.Filter));
					watcher.EnableRaisingEvents = false;
					watcher.Dispose();
				}
				m_Watchers.Clear();
			}
		}

		/// <summary>
		/// Watches the specified files for changes.
		/// </summary>
		/// <param name="fileNames">The file names.</param>
		public void Watch(IEnumerable<string> fileNames)
		{
			if (fileNames == null) return;

			foreach (var str in fileNames)
			{
				if (str.IsNullOrEmptyWithTrim())
					continue;

				//if (File.Exists(str) == false)
				//{
				//    QuickLogger.Log("Watching path '{0}' is not possible. File does not exist.".SafeFormatWith(str));
				//    continue;
				//}

				Watch(str);
			}
		}

		internal void Watch(string fileName)
		{
			FileSystemWatcher item = new FileSystemWatcher
			{
				Path = Path.GetDirectoryName(fileName),
				Filter = Path.GetFileName(fileName),
				NotifyFilter = NotifyFilters.Security | NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Attributes
			};

			item.Created += OnWatcherChanged;
			item.Changed += OnWatcherChanged;
			item.Deleted += OnWatcherChanged;
			item.EnableRaisingEvents = true;
			
			//QuickLogger.Log("Watching path '{0}' filter '{1}' for changes.".SafeFormatWith(item.Path, item.Filter));
			
			lock (m_SyncLock)
			{
				m_Watchers.Add(item);
			}
		}
	}


}
