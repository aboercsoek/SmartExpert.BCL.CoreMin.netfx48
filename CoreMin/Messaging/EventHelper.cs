//--------------------------------------------------------------------------
// File:    EventHelper.cs
// Content:	Implementation of static class EventHelper
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2007 Andreas Börcsök
//--------------------------------------------------------------------------

#region Using directives

using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert.Messaging
{
	/// <summary>
	/// Hilfsklasse, für das threadsichere Auslösen von Events.
	/// </summary>
	public static class EventHelper
	{
		/// <summary>
		/// Fires savely an generic EventHandler event and takes care for exceptions inside event handlers
		/// </summary>
		/// <param name="eventHandler">EventHandler event that should be fired</param>
		/// <param name="sender">The sender object</param>
		/// <param name="eventArgs">Event arguments class instance.</param>
		/// <typeparam name="TEventArgs">EventArgs type or a type derived form EventArgs</typeparam>
		public static void FireEvent<TEventArgs>(EventHandler<TEventArgs> eventHandler, object sender, TEventArgs eventArgs)
			where TEventArgs : EventArgs
		{
			FireDelegate(eventHandler, sender, eventArgs);
		}

		/// <summary>
		/// Fires savely an delegate and takes care for exceptions inside delegate handlers
		/// </summary>
		/// <param name="del">Delegate that should be fired</param>
		/// <param name="args">Parameters that should be passed to the delegate</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static void FireDelegate(Delegate del, params object[] args)
		{
			// Sind Subscriber vorhanden?
			if (del == null)
			{
				// Nein, dann raus!
				return;
			}
			// Alles Subscriber in Array speichern
			Delegate[] delegates = del.GetInvocationList();
			foreach (Delegate sink in delegates)
			{
				// Jeden Subscriber einzeln benachrichtigen
				try
				{
					sink.DynamicInvoke(args);
				}
					// Und eventuell geworfene Exceptions 
					// des Subscribers abfangen.
				catch (Exception ex)
				{
					if (ex.IsFatal())
						throw;
				}
			}
		}

		/// <summary>
		/// Fires asynchronous an generic EventHandler event and takes care for exceptions inside event handlers
		/// </summary>
		/// <param name="eventHandler">EventHandler event that should be fired</param>
		/// <param name="sender">The sender object</param>
		/// <param name="eventArgs">Event arguments class instance.</param>
		/// <typeparam name="TEventArgs">EventArgs type or a type derived form EventArgs</typeparam>
		public static void FireEventAsync<TEventArgs>(EventHandler<TEventArgs> eventHandler, object sender,
		                                              TEventArgs eventArgs) where TEventArgs : EventArgs
		{
			FireDelegateAsync(eventHandler, sender, eventArgs);
		}

		/// <summary>
		/// Fires asynchronous an delegate and takes care for exceptions inside delegate handlers
		/// </summary>
		/// <param name="del">Delegate that should be fired</param>
		/// <param name="args">Parameters that should be passed to the delegate</param>
		public static void FireDelegateAsync(Delegate del, params object[] args)
		{
			
			if (del == null)
			{
				return;
			}
			Delegate[] delegates = del.GetInvocationList();
			AsyncFire asyncFire;
			foreach (Delegate sink in delegates)
			{
				asyncFire = InvokeDelegate;
				asyncFire.BeginInvoke(sink, args,
					delegate( IAsyncResult asyncResult ) // AsyncCallback is mandatory to avoid memory leaks
					{
						AsyncResult resultObj = (AsyncResult)asyncResult;
						AsyncFire opp = (AsyncFire)resultObj.AsyncDelegate;
						opp.EndInvoke(asyncResult); 
					}, null);
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private static void InvokeDelegate(Delegate del, object[] args)
		{
			try
			{
				del.DynamicInvoke(args);
			}
			catch (Exception ex)
			{
				if (ex.IsFatal())
					throw;
			}
		}

		#region Nested type: AsyncFire

		private delegate void AsyncFire(Delegate del, object[] args);

		#endregion
	}
}