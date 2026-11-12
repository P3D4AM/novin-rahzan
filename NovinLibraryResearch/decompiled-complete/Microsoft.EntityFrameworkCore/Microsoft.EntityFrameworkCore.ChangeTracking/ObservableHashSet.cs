using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class ObservableHashSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyPropertyChanging
{
	private HashSet<T> _set;

	public virtual int Count => _set.Count;

	public virtual bool IsReadOnly => ((ICollection<T>)_set).IsReadOnly;

	public virtual IEqualityComparer<T> Comparer => _set.Comparer;

	public virtual event PropertyChangedEventHandler PropertyChanged;

	public virtual event PropertyChangingEventHandler PropertyChanging;

	public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

	public ObservableHashSet()
		: this((IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public ObservableHashSet([JetBrains.Annotations.NotNull] IEqualityComparer<T> comparer)
	{
		_set = new HashSet<T>(comparer);
	}

	public ObservableHashSet([JetBrains.Annotations.NotNull] IEnumerable<T> collection)
		: this(collection, (IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public ObservableHashSet([JetBrains.Annotations.NotNull] IEnumerable<T> collection, [JetBrains.Annotations.NotNull] IEqualityComparer<T> comparer)
	{
		_set = new HashSet<T>(collection, comparer);
	}

	void ICollection<T>.Add(T item)
	{
		Add(item);
	}

	public virtual void Clear()
	{
		if (_set.Count != 0)
		{
			OnCountPropertyChanging();
			List<T> oldItems = this.ToList();
			_set.Clear();
			OnCollectionChanged(ObservableHashSetSingletons._noItems, oldItems);
			OnCountPropertyChanged();
		}
	}

	public virtual bool Contains(T item)
	{
		return _set.Contains(item);
	}

	public virtual void CopyTo(T[] array, int arrayIndex)
	{
		_set.CopyTo(array, arrayIndex);
	}

	public virtual bool Remove(T item)
	{
		if (!_set.Contains(item))
		{
			return false;
		}
		OnCountPropertyChanging();
		_set.Remove(item);
		OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
		OnCountPropertyChanged();
		return true;
	}

	public virtual HashSet<T>.Enumerator GetEnumerator()
	{
		return _set.GetEnumerator();
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual bool Add(T item)
	{
		if (_set.Contains(item))
		{
			return false;
		}
		OnCountPropertyChanging();
		_set.Add(item);
		OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
		OnCountPropertyChanged();
		return true;
	}

	public virtual void UnionWith(IEnumerable<T> other)
	{
		HashSet<T> hashSet = new HashSet<T>(_set, _set.Comparer);
		hashSet.UnionWith(other);
		if (hashSet.Count != _set.Count)
		{
			List<T> newItems = hashSet.Where((T i) => !_set.Contains(i)).ToList();
			OnCountPropertyChanging();
			_set = hashSet;
			OnCollectionChanged(newItems, ObservableHashSetSingletons._noItems);
			OnCountPropertyChanged();
		}
	}

	public virtual void IntersectWith(IEnumerable<T> other)
	{
		HashSet<T> copy = new HashSet<T>(_set, _set.Comparer);
		copy.IntersectWith(other);
		if (copy.Count != _set.Count)
		{
			List<T> oldItems = _set.Where((T i) => !copy.Contains(i)).ToList();
			OnCountPropertyChanging();
			_set = copy;
			OnCollectionChanged(ObservableHashSetSingletons._noItems, oldItems);
			OnCountPropertyChanged();
		}
	}

	public virtual void ExceptWith(IEnumerable<T> other)
	{
		HashSet<T> copy = new HashSet<T>(_set, _set.Comparer);
		copy.ExceptWith(other);
		if (copy.Count != _set.Count)
		{
			List<T> oldItems = _set.Where((T i) => !copy.Contains(i)).ToList();
			OnCountPropertyChanging();
			_set = copy;
			OnCollectionChanged(ObservableHashSetSingletons._noItems, oldItems);
			OnCountPropertyChanged();
		}
	}

	public virtual void SymmetricExceptWith(IEnumerable<T> other)
	{
		HashSet<T> copy = new HashSet<T>(_set, _set.Comparer);
		copy.SymmetricExceptWith(other);
		List<T> list = _set.Where((T i) => !copy.Contains(i)).ToList();
		List<T> list2 = copy.Where((T i) => !_set.Contains(i)).ToList();
		if (list.Count != 0 || list2.Count != 0)
		{
			OnCountPropertyChanging();
			_set = copy;
			OnCollectionChanged(list2, list);
			OnCountPropertyChanged();
		}
	}

	public virtual bool IsSubsetOf(IEnumerable<T> other)
	{
		return _set.IsSubsetOf(other);
	}

	public virtual bool IsProperSubsetOf(IEnumerable<T> other)
	{
		return _set.IsProperSubsetOf(other);
	}

	public virtual bool IsSupersetOf(IEnumerable<T> other)
	{
		return _set.IsSupersetOf(other);
	}

	public virtual bool IsProperSupersetOf(IEnumerable<T> other)
	{
		return _set.IsProperSupersetOf(other);
	}

	public virtual bool Overlaps(IEnumerable<T> other)
	{
		return _set.Overlaps(other);
	}

	public virtual bool SetEquals(IEnumerable<T> other)
	{
		return _set.SetEquals(other);
	}

	public virtual void CopyTo([JetBrains.Annotations.NotNull] T[] array)
	{
		_set.CopyTo(array);
	}

	public virtual void CopyTo([JetBrains.Annotations.NotNull] T[] array, int arrayIndex, int count)
	{
		_set.CopyTo(array, arrayIndex, count);
	}

	public virtual int RemoveWhere([JetBrains.Annotations.NotNull] Predicate<T> match)
	{
		HashSet<T> copy = new HashSet<T>(_set, _set.Comparer);
		int num = copy.RemoveWhere(match);
		if (num == 0)
		{
			return 0;
		}
		List<T> oldItems = _set.Where((T i) => !copy.Contains(i)).ToList();
		OnCountPropertyChanging();
		_set = copy;
		OnCollectionChanged(ObservableHashSetSingletons._noItems, oldItems);
		OnCountPropertyChanged();
		return num;
	}

	public virtual void TrimExcess()
	{
		_set.TrimExcess();
	}

	protected virtual void OnPropertyChanged([JetBrains.Annotations.NotNull] PropertyChangedEventArgs e)
	{
		this.PropertyChanged?.Invoke(this, e);
	}

	protected virtual void OnPropertyChanging([JetBrains.Annotations.NotNull] PropertyChangingEventArgs e)
	{
		this.PropertyChanging?.Invoke(this, e);
	}

	private void OnCountPropertyChanged()
	{
		OnPropertyChanged(ObservableHashSetSingletons._countPropertyChanged);
	}

	private void OnCountPropertyChanging()
	{
		OnPropertyChanging(ObservableHashSetSingletons._countPropertyChanging);
	}

	private void OnCollectionChanged(NotifyCollectionChangedAction action, object item)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item));
	}

	private void OnCollectionChanged(IList newItems, IList oldItems)
	{
		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems));
	}

	protected virtual void OnCollectionChanged([JetBrains.Annotations.NotNull] NotifyCollectionChangedEventArgs e)
	{
		this.CollectionChanged?.Invoke(this, e);
	}
}
