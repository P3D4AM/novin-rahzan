using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class LocalView<TEntity> : ICollection<TEntity>, IEnumerable<TEntity>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged, INotifyPropertyChanging, IListSource where TEntity : class
{
	private ObservableBackedBindingList<TEntity> _bindingList;

	private ObservableCollection<TEntity> _observable;

	private readonly DbContext _context;

	private int _countChanges;

	private int? _count;

	private bool _triggeringStateManagerChange;

	private bool _triggeringObservableChange;

	private bool _triggeringLocalViewChange;

	public virtual int Count
	{
		get
		{
			if (!_count.HasValue)
			{
				IStateManager stateManager = _context.GetDependencies().StateManager;
				int num = 0;
				foreach (TEntity nonDeletedEntity in stateManager.GetNonDeletedEntities<TEntity>())
				{
					_ = nonDeletedEntity;
					num++;
				}
				_count = num;
				_countChanges = 0;
			}
			return _count.Value + _countChanges;
		}
	}

	public virtual bool IsReadOnly => false;

	bool IListSource.ContainsListCollection => false;

	public virtual event PropertyChangedEventHandler PropertyChanged;

	public virtual event PropertyChangingEventHandler PropertyChanging;

	public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

	[EntityFrameworkInternal]
	public LocalView([JetBrains.Annotations.NotNull] DbSet<TEntity> set)
	{
		_context = set.GetService<ICurrentDbContext>().Context;
		set.GetService<ILocalViewListener>().RegisterView(StateManagerChangedHandler);
	}

	public virtual ObservableCollection<TEntity> ToObservableCollection()
	{
		if (_observable == null)
		{
			_observable = new ObservableCollection<TEntity>(this);
			_observable.CollectionChanged += ObservableCollectionChanged;
			CollectionChanged += LocalViewCollectionChanged;
		}
		return _observable;
	}

	private void LocalViewCollectionChanged(object _, NotifyCollectionChangedEventArgs args)
	{
		if (_triggeringLocalViewChange)
		{
			return;
		}
		try
		{
			_triggeringObservableChange = true;
			if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				_observable.Remove((TEntity)args.OldItems[0]);
			}
			else
			{
				_observable.Add((TEntity)args.NewItems[0]);
			}
		}
		finally
		{
			_triggeringObservableChange = false;
		}
	}

	private void ObservableCollectionChanged(object _, NotifyCollectionChangedEventArgs args)
	{
		if (_triggeringObservableChange)
		{
			return;
		}
		try
		{
			_triggeringLocalViewChange = true;
			if (args.Action == NotifyCollectionChangedAction.Reset)
			{
				Clear();
				return;
			}
			if (args.Action == NotifyCollectionChangedAction.Remove || args.Action == NotifyCollectionChangedAction.Replace)
			{
				foreach (TEntity oldItem in args.OldItems)
				{
					Remove(oldItem);
				}
			}
			if (args.Action != 0 && args.Action != NotifyCollectionChangedAction.Replace)
			{
				return;
			}
			foreach (TEntity newItem in args.NewItems)
			{
				Add(newItem);
			}
		}
		finally
		{
			_triggeringLocalViewChange = false;
		}
	}

	public virtual IEnumerator<TEntity> GetEnumerator()
	{
		return _context.GetDependencies().StateManager.GetNonDeletedEntities<TEntity>().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual void Add(TEntity item)
	{
		InternalEntityEntry orCreateEntry = _context.GetDependencies().StateManager.GetOrCreateEntry(item);
		if (orCreateEntry.EntityState == EntityState.Deleted || orCreateEntry.EntityState == EntityState.Detached)
		{
			try
			{
				_triggeringStateManagerChange = true;
				OnCountPropertyChanging();
				orCreateEntry.SetEntityState(EntityState.Added);
				_countChanges++;
				OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
				OnCountPropertyChanged();
			}
			finally
			{
				_triggeringStateManagerChange = false;
			}
		}
	}

	public virtual void Clear()
	{
		foreach (TEntity item in _context.GetDependencies().StateManager.GetNonDeletedEntities<TEntity>().ToList())
		{
			Remove(item);
		}
	}

	public virtual bool Contains(TEntity item)
	{
		InternalEntityEntry internalEntityEntry = _context.GetDependencies().StateManager.TryGetEntry(item);
		if (internalEntityEntry != null)
		{
			return internalEntityEntry.EntityState != EntityState.Deleted;
		}
		return false;
	}

	public virtual void CopyTo(TEntity[] array, int arrayIndex)
	{
		foreach (TEntity nonDeletedEntity in _context.GetDependencies().StateManager.GetNonDeletedEntities<TEntity>())
		{
			array[arrayIndex++] = nonDeletedEntity;
		}
	}

	public virtual bool Remove(TEntity item)
	{
		InternalEntityEntry internalEntityEntry = _context.GetDependencies().StateManager.TryGetEntry(item);
		if (internalEntityEntry != null && internalEntityEntry.EntityState != EntityState.Deleted)
		{
			try
			{
				_triggeringStateManagerChange = true;
				OnCountPropertyChanging();
				internalEntityEntry.SetEntityState((internalEntityEntry.EntityState != EntityState.Added) ? EntityState.Deleted : EntityState.Detached);
				_countChanges--;
				OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
				OnCountPropertyChanged();
			}
			finally
			{
				_triggeringStateManagerChange = false;
			}
			return true;
		}
		return false;
	}

	private void StateManagerChangedHandler(InternalEntityEntry entry, EntityState previousState)
	{
		if (_triggeringStateManagerChange || !(entry.Entity is TEntity item))
		{
			return;
		}
		bool num = previousState != 0 && previousState != EntityState.Deleted;
		bool flag = entry.EntityState != 0 && entry.EntityState != EntityState.Deleted;
		if (num != flag)
		{
			OnCountPropertyChanging();
			if (flag)
			{
				_countChanges++;
				OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
			}
			else
			{
				_countChanges--;
				OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
			}
			OnCountPropertyChanged();
		}
	}

	protected virtual void OnPropertyChanged([JetBrains.Annotations.NotNull] PropertyChangedEventArgs e)
	{
		this.PropertyChanged?.Invoke(this, e);
	}

	protected virtual void OnPropertyChanging([JetBrains.Annotations.NotNull] PropertyChangingEventArgs e)
	{
		this.PropertyChanging?.Invoke(this, e);
	}

	protected virtual void OnCollectionChanged([JetBrains.Annotations.NotNull] NotifyCollectionChangedEventArgs e)
	{
		this.CollectionChanged?.Invoke(this, e);
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

	public virtual BindingList<TEntity> ToBindingList()
	{
		return _bindingList ?? (_bindingList = new ObservableBackedBindingList<TEntity>(ToObservableCollection()));
	}

	IList IListSource.GetList()
	{
		throw new NotSupportedException(CoreStrings.DataBindingToLocalWithIListSource);
	}
}
