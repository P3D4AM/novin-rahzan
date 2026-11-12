using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Data.Common;

[Serializable]
internal sealed class ReadOnlyCollection<T> : ICollection, IEnumerable, ICollection<T>, IEnumerable<T>
{
	[Serializable]
	internal struct Enumerator<K> : IEnumerator<K>, IDisposable, IEnumerator
	{
		private K[] _items;

		private int _index;

		public K Current => _items[_index];

		object IEnumerator.Current => _items[_index];

		internal Enumerator(K[] items)
		{
			_items = items;
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return ++_index < _items.Length;
		}

		void IEnumerator.Reset()
		{
			_index = -1;
		}
	}

	private T[] _items;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => _items;

	bool ICollection<T>.IsReadOnly => true;

	public int Count => _items.Length;

	internal ReadOnlyCollection(T[] items)
	{
		_items = items;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(_items, 0, array, arrayIndex, _items.Length);
	}

	void ICollection.CopyTo(Array array, int arrayIndex)
	{
		Array.Copy(_items, 0, array, arrayIndex, _items.Length);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator<T>(_items);
	}

	public IEnumerator GetEnumerator()
	{
		return new Enumerator<T>(_items);
	}

	void ICollection<T>.Add(T value)
	{
		throw new NotSupportedException();
	}

	void ICollection<T>.Clear()
	{
		throw new NotSupportedException();
	}

	bool ICollection<T>.Contains(T value)
	{
		return Array.IndexOf(_items, value) >= 0;
	}

	bool ICollection<T>.Remove(T value)
	{
		throw new NotSupportedException();
	}
}
