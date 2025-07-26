//--------------------------------------------------------------------------
// File:    TimestampedValue.cs
// Content:	Implementation of class TimestampedValue
// Author:	Andreas Börcsök
// Website:	http://www.smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Linq;
using SmartExpert;
using SmartExpert.Error;
using SmartExpert.Linq;


#endregion

namespace SmartExpert
{
	///<summary>Generic Timestamped-Value implementation. 
	/// Provides generic value cashing through evaluator function and update interval.</summary>
	/// <typeparam name="T">Type of the Timestamped-Value.</typeparam>
	public class TimestampedValue<T>
	{
		private readonly Func<T> m_Evaluator;
		private readonly TimeSpan m_UpdatePeriod;
		private DateTime m_LastEvaluated;
		private T m_Value;
		private int m_EvaluationFailureCount;
		private readonly int m_MaxEvaluationFailureCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimestampedValue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="valueEvaluator">The value evaluator function.</param>
		/// <param name="updatePeriod">The update period.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="valueEvaluator"/> is <see langword="null"/>.</exception>
		public TimestampedValue(Func<T> valueEvaluator, TimeSpan updatePeriod)
			: this(valueEvaluator, updatePeriod, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimestampedValue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="valueEvaluator">The value evaluator function.</param>
		/// <param name="updatePeriod">The update period.</param>
		/// <param name="maxEvaluationFailureCount">Max valueEvaluator failure calls retry count.</param>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="valueEvaluator"/> is <see langword="null"/>.</exception>
		public TimestampedValue(Func<T> valueEvaluator, TimeSpan updatePeriod, int maxEvaluationFailureCount)
		{
			ArgChecker.ShouldNotBeNull(valueEvaluator, "valueEvaluator");

			m_MaxEvaluationFailureCount = maxEvaluationFailureCount <= 0 ? 3 : maxEvaluationFailureCount;
			m_EvaluationFailureCount = 0;

			m_Evaluator = valueEvaluator;
			m_UpdatePeriod = updatePeriod;

			m_Value = default(T);
			m_LastEvaluated = DateTime.MinValue;
		}

		/// <summary>
		/// Creates a new timestamped value instance.
		/// </summary>
		/// <param name="valueEvaluator">The value evaluator function.</param>
		/// <param name="updatePeriod">The update period.</param>
		/// <returns>A new timestamped value instance.</returns>
		/// <exception cref="ArgNullException">Is thrown if <paramref name="valueEvaluator"/> is <see langword="null"/>.</exception>
		public static TimestampedValue<T> Create(Func<T> valueEvaluator, TimeSpan updatePeriod)
		{
			ArgChecker.ShouldNotBeNull(valueEvaluator, "valueEvaluator");

			return new TimestampedValue<T>(valueEvaluator, updatePeriod);
		}


		/// <summary>
		/// Gets the Timestamped-Value. 
		/// If the time difference from the last evaluated time to now is less than 
		/// the specified update period the cashed value will be returned; 
		/// otherwise the value will be updated by the value evaluator function
		/// and than returned to the caller. The value timestamp will be set
		/// to DateTime.Now so that a new cashing period (or update period) starts.
		/// </summary>
		public T Value
		{
			get
			{
				if ((DateTime.Now - m_LastEvaluated) >= m_UpdatePeriod)
				{
					try
					{
						// Update value through evaluator function.
						m_Value = m_Evaluator();

						// Last-Evaluated-Timestamp will only be updated
						// if evaluator throws no exception.
						m_LastEvaluated = DateTime.Now;
						// Reset evaluator failure count.
						m_EvaluationFailureCount = 0;
					}
					catch (Exception ex)
					{
						m_EvaluationFailureCount++;

						// If the exception is fatal or the evaluator call failed 
						// three or more times in serie => Rethrow exception!
						if (ex.IsFatal() || m_EvaluationFailureCount >= m_MaxEvaluationFailureCount)
							throw;
					}
					
				}

				return m_Value;
			}
		}

	}
}
