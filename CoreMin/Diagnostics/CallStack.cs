//--------------------------------------------------------------------------
// File:    CallStack.cs
// Content:	Implementation of class CallStack
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2010 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SmartExpert;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Diagnostics
{

    /// <summary>
    /// The call stack information data helper.
    /// </summary>
    public static class CallStackExecutionInfo
    {
		/// <summary>
		/// Gets the caller information.
		/// </summary>
		/// <returns>
		/// Returns the caller infos value.
		/// </returns>
        public static CallStackDataItem GetCallerInfos()
        {
            int stackFrameNumber = 2;
            StackFrame frame = new StackFrame(stackFrameNumber);
            CallStackDataItem result = new CallStackDataItem();

            // Stackframe number            
            result.StackFrameNumber = stackFrameNumber;
            // Get caller info from created stack frame
            result.TypeName = frame.GetMethod().DeclaringType.Name;
            result.MethodName = frame.GetMethod().Name;
            // Calling Type Namespace
            result.NamespaceInformation = frame.GetMethod().DeclaringType.Namespace;
            // Get caller assembly info
            result.AssemblyInformation = frame.GetMethod().DeclaringType.Assembly.FullName;

            // Get Thread and Process infos
            result.ThreadId = Thread.CurrentThread.ManagedThreadId;
            result.ThreadName = Thread.CurrentThread.Name;
            result.ProcessId = Process.GetCurrentProcess().Id;
            result.ProcessName = Process.GetCurrentProcess().ProcessName;

            return result;
        }

		/// <summary>
		/// Gets the stack frame information.
		/// </summary>
		/// <param name="stackFrameNumber">The stack frame number.</param>
		/// <returns>
		/// Returns the stack frame infos value.
		/// </returns>
        public static CallStackDataItem GetStackFrameInfos(int stackFrameNumber)
        {
            StackFrame frame = new StackFrame(stackFrameNumber);
            CallStackDataItem result = new CallStackDataItem();

            // Stackframe number            
            result.StackFrameNumber = stackFrameNumber;
            // Get caller info from created stack frame
            result.TypeName = frame.GetMethod().DeclaringType.Name;
            result.MethodName = frame.GetMethod().Name;
            // Calling Type Namespace
            result.NamespaceInformation = frame.GetMethod().DeclaringType.Namespace;
            // Get caller assembly info
            result.AssemblyInformation = frame.GetMethod().DeclaringType.Assembly.FullName;

            // Get Thread and Process infos
            result.ThreadId = Thread.CurrentThread.ManagedThreadId;
            result.ThreadName = Thread.CurrentThread.Name;
            result.ProcessId = Process.GetCurrentProcess().Id;
            result.ProcessName = Process.GetCurrentProcess().ProcessName;

            return result;
        }

    }



    /// <summary>
    /// The stack frame data item class
    /// </summary>
    public class CallStackDataItem
    {
        /// <summary>
        /// Gets or sets the stack frame number.
        /// </summary>
        /// <value>The stack frame number.</value>
        public int StackFrameNumber { get; set; }
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; set; }
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; }
        /// <summary>
        /// Gets or sets the assembly information.
        /// </summary>
        /// <value>The assembly information.</value>
        public string AssemblyInformation { get; set; }
        /// <summary>
        /// Gets or sets the namespace information.
        /// </summary>
        /// <value>The namespace information.</value>
        public string NamespaceInformation { get; set; }

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        /// <value>The thread id.</value>
        public int ThreadId { get; set; }
        /// <summary>
        /// Gets or sets the name of the thread.
        /// </summary>
        /// <value>The name of the thread.</value>
        public string ThreadName { get; set; }

        /// <summary>
        /// Gets or sets the process id.
        /// </summary>
        /// <value>The process id.</value>
        public int ProcessId { get; set; }
        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>The name of the process.</value>
        public string ProcessName { get; set; }


    }
}
