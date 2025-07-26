//--------------------------------------------------------------------------
// File:    ProcessRunner.cs
// Content:	Implementation of class ProcessRunner
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2009 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using SmartExpert.Error;

#endregion

namespace SmartExpert.SystemProcesses
{
	/// <summary>
	/// Runs a process that sends output to standard output and to
	/// standard error.
	/// </summary>
	public class ProcessRunner : IDisposable
	{
		Process m_Process;
		StringBuilder m_StandardOutput = new StringBuilder();
		StringBuilder m_StandardError = new StringBuilder();
		ManualResetEvent m_EndOfOutput = new ManualResetEvent(false);
		int m_OutputStreamsFinished;

		/// <summary>
		/// Triggered when the process has exited.
		/// </summary>
		public event EventHandler ProcessExited;

		/// <summary>
		/// Triggered when a line of text is read from the standard output.
		/// </summary>
		public event EventHandler<LineReceivedEventArgs> OutputLineReceived;

		/// <summary>
		/// Triggered when a line of text is read from the standard error.
		/// </summary>
		public event EventHandler<LineReceivedEventArgs> ErrorLineReceived;

		/// <summary>
		/// Creates a new instance of the <see cref="ProcessRunner"/>.
		/// </summary>
		public ProcessRunner()
		{
			LogStandardOutputAndError = true;
		}

		/// <summary>
		/// Gets or sets the process's working directory.
		/// </summary>
		public string WorkingDirectory { get; set; }

		/// <summary>
		/// Gets or sets whether standard output is logged to the "StandardOutput" and "StandardError"
		/// properties. When this property is false, output is still redirected to the
		/// OutputLineReceived and ErrorLineReceived events, but the ProcessRunner uses less memory.
		/// The default value is true.
		/// </summary>
		public bool LogStandardOutputAndError { get; set; }

		/// <summary>
		/// Gets the standard output returned from the process.
		/// </summary>
		public string StandardOutput
		{
			get
			{
				lock ( m_StandardOutput )
					return m_StandardOutput.ToString();
			}
		}

		/// <summary>
		/// Gets the standard error output returned from the process.
		/// </summary>
		public string StandardError
		{
			get
			{
				lock ( m_StandardError )
					return m_StandardError.ToString();
			}
		}

		/// <summary>
		/// Releases resources held by the <see cref="ProcessRunner"/>
		/// </summary>
		public void Dispose()
		{
			m_Process.Dispose();
			m_EndOfOutput.Close();
		}

		/// <summary>
		/// Gets the process exit code.
		/// </summary>
		public int ExitCode
		{
			get
			{
				int exitCode = 0;
				if ( m_Process != null )
				{
					exitCode = m_Process.ExitCode;
				}
				return exitCode;
			}
		}

		/// <summary>
		/// Waits for the process to exit.
		/// </summary>
		public void WaitForExit()
		{
			WaitForExit(Int32.MaxValue);
		}

		/// <summary>
		/// Waits for the process to exit.
		/// </summary>
		/// <param name="timeout">A timeout in milliseconds.</param>
		/// <returns><see langword="true"/> if the associated process has
		/// exited; otherwise, <see langword="false"/></returns>
		public bool WaitForExit( int timeout )
		{
			if ( m_Process == null )
			{
				throw new ProcessRunnerException("No Process is running.");
			}

			bool exited = m_Process.WaitForExit(timeout);

			if ( exited )
			{
				m_EndOfOutput.WaitOne(timeout == int.MaxValue ? Timeout.Infinite : timeout, false);
			}

			return exited;
		}

		/// <summary>
		/// Gets a value indicating whether the process is running.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if the process is running; otherwise, <see langword="false"/>.
		/// </value>
		public bool IsRunning
		{
			get
			{
				bool isRunning = false;

				if ( m_Process != null )
				{
					isRunning = !m_Process.HasExited;
				}

				return isRunning;
			}
		}

		/// <summary>
		/// Starts the process.
		/// </summary>
		/// <param name="fileName">The process filename.</param>
		/// <param name="arguments">The command line arguments to
		/// pass to the command.</param>
		public void Start( string fileName, string arguments )
		{
			Encoding encoding = OemEncoding;

			m_Process = new Process();
			m_Process.StartInfo.CreateNoWindow = true;
			m_Process.StartInfo.FileName = fileName;
			m_Process.StartInfo.WorkingDirectory = WorkingDirectory;
			m_Process.StartInfo.RedirectStandardOutput = true;
			m_Process.StartInfo.StandardOutputEncoding = encoding;
			m_Process.OutputDataReceived += OnOutputLineReceived;
			m_Process.StartInfo.RedirectStandardError = true;
			m_Process.StartInfo.StandardErrorEncoding = encoding;
			m_Process.ErrorDataReceived += OnErrorLineReceived;
			m_Process.StartInfo.UseShellExecute = false;
			m_Process.StartInfo.Arguments = arguments;

			if ( ProcessExited != null )
			{
				m_Process.EnableRaisingEvents = true;
				m_Process.Exited += OnProcessExited;
			}

			bool started = false;
			try
			{
				m_Process.Start();
				started = true;
			}
			finally
			{
				if ( !started )
				{
					m_Process.Exited -= OnProcessExited;
					m_Process = null;
				}
			}

			m_Process.BeginOutputReadLine();
			m_Process.BeginErrorReadLine();
		}

		/// <summary>
		/// Starts the process.
		/// </summary>
		/// <param name="fileName">The process filename.</param>
		/// <param name="arguments">The command line arguments to pass to the command.</param>
		/// <param name="userName">The user name.</param>
		/// <param name="password">The password.</param>
		/// <param name="domain">The domain name.</param>
		public void Start(string fileName, string arguments, string userName, ref SecureString password, string domain)
		{
			Encoding encoding = OemEncoding;

			m_Process = new Process();
			m_Process.StartInfo.CreateNoWindow = true;
			m_Process.StartInfo.FileName = fileName;
			m_Process.StartInfo.WorkingDirectory = WorkingDirectory;
			m_Process.StartInfo.RedirectStandardOutput = true;
			m_Process.StartInfo.StandardOutputEncoding = encoding;
			m_Process.OutputDataReceived += OnOutputLineReceived;
			m_Process.StartInfo.RedirectStandardError = true;
			m_Process.StartInfo.StandardErrorEncoding = encoding;
			m_Process.ErrorDataReceived += OnErrorLineReceived;
			m_Process.StartInfo.UseShellExecute = false;
			m_Process.StartInfo.Arguments = arguments;
			m_Process.StartInfo.UserName = userName;
			m_Process.StartInfo.Password = password;
			m_Process.StartInfo.Domain = domain;

			if (ProcessExited != null)
			{
				m_Process.EnableRaisingEvents = true;
				m_Process.Exited += OnProcessExited;
			}

			bool started = false;
			try
			{
				m_Process.Start();
				started = true;
			}
			finally
			{
				if (!started)
				{
					m_Process.Exited -= OnProcessExited;
					m_Process = null;
				}
			}

			m_Process.BeginOutputReadLine();
			m_Process.BeginErrorReadLine();
		}

		/// <summary>
		/// Starts the process.
		/// </summary>
		/// <param name="command">The process filename.</param>
		public void Start( string command )
		{
			Start(command, String.Empty);
		}

		/// <summary>
		/// Kills the running process.
		/// </summary>
		public void Kill()
		{
			if ( m_Process != null )
			{
				if ( !m_Process.HasExited )
				{
					m_Process.Kill();
					m_Process.Close();
					m_Process.Dispose();
					m_Process = null;
					m_EndOfOutput.WaitOne();
				}
				else
				{
					m_Process = null;
				}
			}
		}

		/// <summary>
		/// Gets the OEM encoding.
		/// </summary>
		/// <value>The OEM encoding.</value>
		public static Encoding OemEncoding
		{
			get
			{
				try
				{
					return Encoding.GetEncoding(GetOEMCP());
				}
				catch ( ArgumentException )
				{
					return Encoding.Default;
				}
				catch ( NotSupportedException )
				{
					return Encoding.Default;
				}
			}
		}

		[DllImport("kernel32.dll")]
		static extern int GetOEMCP();

		/// <summary>
		/// Raises the <see cref="ProcessExited"/> event.
		/// </summary>
		protected void OnProcessExited( object sender, EventArgs e )
		{
			if ( ProcessExited != null )
			{
				if ( m_EndOfOutput != null )
				{
					m_EndOfOutput.WaitOne();
				}

				ProcessExited(this, e);
			}
		}

		/// <summary>
		/// Raises the <see cref="OutputLineReceived"/> event.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The line received event arguments.</param>
		protected void OnOutputLineReceived( object sender, DataReceivedEventArgs e )
		{
			if ( e.Data == null )
			{
				if ( Interlocked.Increment(ref m_OutputStreamsFinished) == 2 )
					m_EndOfOutput.Set();
				return;
			}
			if ( LogStandardOutputAndError )
			{
				lock ( m_StandardOutput )
				{
					m_StandardOutput.AppendLine(e.Data);
				}
			}
			if ( OutputLineReceived != null )
			{
				OutputLineReceived(this, new LineReceivedEventArgs(e.Data));
			}
		}

		/// <summary>
		/// Raises the <see cref="ErrorLineReceived"/> event.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The line received event arguments.</param>
		protected void OnErrorLineReceived( object sender, DataReceivedEventArgs e )
		{
			if ( e.Data == null )
			{
				if ( Interlocked.Increment(ref m_OutputStreamsFinished) == 2 )
					m_EndOfOutput.Set();
				return;
			}
			if ( LogStandardOutputAndError )
			{
				lock ( m_StandardError )
				{
					m_StandardError.AppendLine(e.Data);
				}
			}
			if ( ErrorLineReceived != null )
			{
				ErrorLineReceived(this, new LineReceivedEventArgs(e.Data));
			}
		}
	}
}
