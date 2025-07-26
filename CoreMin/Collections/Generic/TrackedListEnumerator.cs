using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartExpert;
using SmartExpert.Linq;


namespace SmartExpert.Collections.Generic
{
	internal class TrackedListEnumerator<T> : IEnumerator<T>
	{
		#region Private Members

		private List<TrackedList<T>.ValueEl> m_CombinedValues;
		private DateTime m_CreationTime;
		private T m_Current;
		private bool m_Disposed;
		private bool m_EndReached;
		private IEnumerator m_Enumerator;
		private string m_OuterClassName;
		private TrackedList<T> m_TrackedCollection;

		#endregion

		internal TrackedListEnumerator(string outerClassName, TrackedList<T> trackedCollection, List<TrackedList<T>.ValueEl> combinedValues)
		{
			m_CreationTime = DateTime.UtcNow;
			m_OuterClassName = outerClassName;
			m_TrackedCollection = trackedCollection;
			m_CombinedValues = combinedValues;
		}

		private void CheckChanged()
		{
			if (m_TrackedCollection.LastChanged > m_CreationTime)
			{
				throw new InvalidOperationException(StringResources.TrackedCollectionEnumHasChanged);
			}
		}

		private void CheckDisposed()
		{
			if (m_Disposed)
			{
				throw new ObjectDisposedException(m_OuterClassName);
			}
		}

		public void Dispose()
		{
			m_Disposed = true;
		}

		public bool MoveNext()
		{
			CheckDisposed();
			CheckChanged();

			if (m_EndReached)
				return false;

			if (m_Enumerator == null)
				m_Enumerator = m_CombinedValues.GetEnumerator();
			
			if (m_Enumerator.MoveNext() == false)
			{
				m_EndReached = true;
				return false;
			}

			var current = (TrackedList<T>.ValueEl)m_Enumerator.Current;
			if (current == null)
				return true;

			m_Current = current.IsInserted ? current.InsertedValue : current.OriginalValue.Second;
			
			return true;
		}

		public void Reset()
		{
			CheckDisposed();
			CheckChanged();
			m_EndReached = false;
			m_Enumerator = null;
		}

		bool IEnumerator.MoveNext()
		{
			return MoveNext();
		}

		void IEnumerator.Reset()
		{
			Reset();
		}

		// Properties
		public T Current
		{
			get
			{
				CheckDisposed();
				if (m_EndReached || (m_Enumerator == null))
				{
					throw new InvalidOperationException(StringResources.TrackedCollectionEnumInvalidPos);
				}
				return m_Current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}
	}
}