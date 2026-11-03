using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DevComponents.DotNetBar.Primitives;

[Serializable]
[DebuggerDisplay("Count = {Count}")]
[ComVisible(false)]
public class CustomCollection<T> : INotifyCollectionChanged, IList, IList<T>, ICollection<T>, IEnumerable<T>, INotifyPropertyChanged, ICollection, IEnumerable
{
	[Serializable]
	private class SimpleMonitor : IDisposable
	{
		private int _BusyCount;

		public bool Busy => _BusyCount > 0;

		public void Dispose()
		{
			_BusyCount--;
		}

		public void Enter()
		{
			_BusyCount++;
		}
	}

	private SimpleMonitor _monitor;

	private object _SyncRoot;

	private readonly IList<T> _Items;

	private bool _FloatLastItem;

	public int Count
	{
		get
		{
			OnCollectionReadAccess();
			return _Items.Count;
		}
	}

	private string CountString => "Count";

	internal bool FloatLastItem
	{
		get
		{
			return _FloatLastItem;
		}
		set
		{
			_FloatLastItem = value;
		}
	}

	public T this[int index]
	{
		get
		{
			OnCollectionReadAccess();
			return _Items[index];
		}
		set
		{
			if (_Items.IsReadOnly)
			{
				throw new NotSupportedException("collection is read-only");
			}
			if (index < 0 || index >= _Items.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			SetItem(index, value);
		}
	}

	private string ItemString => "Item[]";

	object IList.this[int index]
	{
		get
		{
			OnCollectionReadAccess();
			return _Items[index];
		}
		set
		{
			VerifyValueType(value);
			this[index] = (T)value;
		}
	}

	bool IList.IsFixedSize
	{
		get
		{
			if (_Items is IList list)
			{
				return list.IsFixedSize;
			}
			return false;
		}
	}

	bool IList.IsReadOnly => _Items.IsReadOnly;

	protected IList<T> Items
	{
		get
		{
			OnCollectionReadAccess();
			return _Items;
		}
	}

	bool ICollection.IsSynchronized => false;

	bool ICollection<T>.IsReadOnly => _Items.IsReadOnly;

	object ICollection.SyncRoot
	{
		get
		{
			if (_SyncRoot == null)
			{
				if (_Items is ICollection collection)
				{
					_SyncRoot = collection.SyncRoot;
				}
				else
				{
					Interlocked.CompareExchange(ref _SyncRoot, new object(), null);
				}
			}
			return _SyncRoot;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event NotifyCollectionChangedEventHandler CollectionChanged;

	public CustomCollection()
	{
		_monitor = new SimpleMonitor();
		_Items = new List<T>();
	}

	public void Dispose()
	{
		_monitor.Dispose();
	}

	public CustomCollection(int capacity)
	{
		_monitor = new SimpleMonitor();
		_Items = new List<T>(capacity);
	}

	public CustomCollection(IList<T> list)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		_monitor = new SimpleMonitor();
		_Items = list;
	}

	public void Add(T item)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		InsertItem(_Items.Count, item);
	}

	public void Sort()
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (!(_Items is List<T>))
		{
			throw new NotSupportedException("collection must be default List collection");
		}
		((List<T>)_Items).Sort();
		OnCollectionChanged(NotifyCollectionChangedAction.Reset, null, -1);
	}

	public void SortReverse()
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (!(_Items is List<T>))
		{
			throw new NotSupportedException("collection must be default List collection");
		}
		((List<T>)_Items).Sort();
		((List<T>)_Items).Reverse();
		OnCollectionChanged(NotifyCollectionChangedAction.Reset, null, -1);
	}

	public void Sort(Comparison<T> comparison)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (!(_Items is List<T>))
		{
			throw new NotSupportedException("collection must be default List collection");
		}
		((List<T>)_Items).Sort(comparison);
		OnCollectionChanged(NotifyCollectionChangedAction.Reset, null, -1);
	}

	public void Sort(IComparer<T> comparer)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (!(_Items is List<T>))
		{
			throw new NotSupportedException("collection must be default List collection");
		}
		((List<T>)_Items).Sort(comparer);
		OnCollectionChanged(NotifyCollectionChangedAction.Reset, null, -1);
	}

	public void Sort(int index, int count, IComparer<T> comparer)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (!(_Items is List<T>))
		{
			throw new NotSupportedException("collection must be default List collection");
		}
		((List<T>)_Items).Sort(index, count, comparer);
		OnCollectionChanged(NotifyCollectionChangedAction.Reset, null, -1);
	}

	public void Clear()
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		ClearItems();
	}

	protected virtual void ClearItems()
	{
		CheckReentrancy();
		T item = default(T);
		if (_FloatLastItem && _Items.Count > 0)
		{
			item = _Items[_Items.Count - 1];
		}
		_Items.Clear();
		OnPropertyChanged(new PropertyChangedEventArgs(CountString));
		OnPropertyChanged(new PropertyChangedEventArgs(ItemString));
		OnCollectionReset();
		if (_FloatLastItem)
		{
			_Items.Add(item);
		}
	}

	public bool Contains(T item)
	{
		OnCollectionReadAccess();
		return _Items.Contains(item);
	}

	public void CopyTo(T[] array, int index)
	{
		OnCollectionReadAccess();
		_Items.CopyTo(array, index);
	}

	public void CopyTo(T[] array)
	{
		CopyTo(array, 0);
	}

	public IEnumerator<T> GetEnumerator()
	{
		OnCollectionReadAccess();
		return _Items.GetEnumerator();
	}

	public int IndexOf(T item)
	{
		OnCollectionReadAccess();
		return _Items.IndexOf(item);
	}

	public void Insert(int index, T item)
	{
		if (index < 0 || index > _Items.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		InsertItem(index, item);
	}

	protected virtual void InsertItem(int index, T item)
	{
		CheckReentrancy();
		if (_FloatLastItem && _Items.Count > 0 && index == _Items.Count)
		{
			index--;
		}
		_Items.Insert(index, item);
		OnPropertyChanged(new PropertyChangedEventArgs(CountString));
		OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
	}

	private static bool IsCompatibleObject(object value)
	{
		if (!(value is T))
		{
			if (value == null)
			{
				return !typeof(T).IsValueType;
			}
			return false;
		}
		return true;
	}

	public bool Remove(T item)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		int num = _Items.IndexOf(item);
		if (num < 0)
		{
			return false;
		}
		RemoveItem(num);
		return true;
	}

	public void RemoveAt(int index)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (index < 0 || index >= _Items.Count)
		{
			throw new ArgumentOutOfRangeException();
		}
		RemoveItem(index);
	}

	protected virtual void RemoveItem(int index)
	{
		CheckReentrancy();
		object item = _Items[index];
		_Items.RemoveAt(index);
		OnPropertyChanged(new PropertyChangedEventArgs(CountString));
		OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
	}

	protected virtual void SetItem(int index, T item)
	{
		CheckReentrancy();
		T val = _Items[index];
		_Items[index] = item;
		OnPropertyChanged(new PropertyChangedEventArgs(CountString));
		OnPropertyChanged(new PropertyChangedEventArgs(ItemString));
		OnCollectionChanged(NotifyCollectionChangedAction.Replace, val, item, index);
	}

	int IList.Add(object value)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		VerifyValueType(value);
		Add((T)value);
		return Count - 1;
	}

	bool IList.Contains(object value)
	{
		if (IsCompatibleObject(value))
		{
			return Contains((T)value);
		}
		return false;
	}

	protected IList<T> GetItemsDirect()
	{
		return _Items;
	}

	int IList.IndexOf(object value)
	{
		OnCollectionReadAccess();
		if (!IsCompatibleObject(value))
		{
			return -1;
		}
		return IndexOf((T)value);
	}

	void IList.Insert(int index, object value)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		VerifyValueType(value);
		Insert(index, (T)value);
	}

	void IList.Remove(object value)
	{
		if (_Items.IsReadOnly)
		{
			throw new NotSupportedException("collection is read-only");
		}
		if (IsCompatibleObject(value))
		{
			Remove((T)value);
		}
	}

	private static void VerifyValueType(object value)
	{
		if (!IsCompatibleObject(value))
		{
			throw new ArgumentException("value is of wrong type");
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		OnCollectionReadAccess();
		return _Items.GetEnumerator();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		CheckReentrancy();
		OnCollectionReadAccess();
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (array.Rank != 1)
		{
			throw new ArgumentException("Argument array.Rank multi-dimensional not supported");
		}
		if (array.GetLowerBound(0) != 0)
		{
			throw new ArgumentException("Argument array non zero lower bound not supported");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index", "index must be non-negative number");
		}
		if (array.Length - index < Count)
		{
			throw new ArgumentException("array too small");
		}
		if (array is T[] array2)
		{
			_Items.CopyTo(array2, index);
			return;
		}
		Type elementType = array.GetType().GetElementType();
		Type typeFromHandle = typeof(T);
		if ((object)elementType == null || (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType)))
		{
			throw new ArgumentException("Argument array of invalid type");
		}
		if (!(array is object[] array3))
		{
			throw new ArgumentException("Argument array invalid type");
		}
		int count = _Items.Count;
		try
		{
			for (int i = 0; i < count; i++)
			{
				array3[index++] = _Items[i];
			}
		}
		catch (ArrayTypeMismatchException)
		{
			throw new ArgumentException("Argument array invalid type");
		}
	}

	protected virtual void OnCollectionReadAccess()
	{
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, e);
		}
	}

	protected IDisposable BlockReentrancy()
	{
		_monitor.Enter();
		return _monitor;
	}

	protected void CheckReentrancy()
	{
		if (_monitor.Busy && this.CollectionChanged != null && this.CollectionChanged.GetInvocationList().Length > 1)
		{
			throw new InvalidOperationException("CustomCollectionReentrancyNotAllowed");
		}
	}

	private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
	}

	private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
	}

	private void OnCollectionReset()
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		if (this.CollectionChanged != null)
		{
			using (BlockReentrancy())
			{
				this.CollectionChanged(this, e);
			}
		}
	}
}
