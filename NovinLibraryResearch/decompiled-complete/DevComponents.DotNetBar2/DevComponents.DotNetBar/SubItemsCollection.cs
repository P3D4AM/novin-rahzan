using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class SubItemsCollection : CollectionBase
{
	private BaseItem _0AF1;

	protected bool m_IgnoreEvents;

	private bool _0AF2 = true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool AllowParentRemove
	{
		get
		{
			return _0AF2;
		}
		set
		{
			_0AF2 = value;
		}
	}

	public virtual BaseItem this[int index]
	{
		get
		{
			return (BaseItem)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public virtual BaseItem this[string name]
	{
		get
		{
			return (BaseItem)base.List[IndexOf(name)];
		}
		set
		{
			base.List[IndexOf(name)] = value;
		}
	}

	internal bool _0AF0
	{
		get
		{
			return m_IgnoreEvents;
		}
		set
		{
			m_IgnoreEvents = value;
		}
	}

	internal SubItemsCollection(BaseItem parent)
	{
		_0AF1 = parent;
	}

	internal int __0AF3(BaseItem _06EC)
	{
		m_IgnoreEvents = true;
		_0AF2 = false;
		int num = 0;
		try
		{
			return base.List.Add(_06EC);
		}
		finally
		{
			m_IgnoreEvents = false;
			_0AF2 = true;
		}
	}

	internal void __0AF3(BaseItem _06EC, int _0AF4)
	{
		m_IgnoreEvents = true;
		_0AF2 = false;
		try
		{
			if (_0AF4 >= 0)
			{
				base.List.Insert(_0AF4, _06EC);
			}
			else
			{
				base.List.Add(_06EC);
			}
		}
		finally
		{
			m_IgnoreEvents = false;
			_0AF2 = true;
		}
	}

	internal void __0AF5()
	{
		m_IgnoreEvents = true;
		try
		{
			base.List.Clear();
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	public virtual int Add(BaseItem item)
	{
		return Add(item, -1);
	}

	public virtual int Add(BaseItem item, int Position)
	{
		int result = Position;
		if (Contains(item))
		{
			return IndexOf(item);
		}
		if (Position >= 0)
		{
			base.List.Insert(Position, item);
		}
		else
		{
			result = base.List.Add(item);
		}
		return result;
	}

	protected override void OnInsert(int index, object value)
	{
		if (value is BaseItem)
		{
			BaseItem baseItem = value as BaseItem;
			if (_0AF2 && baseItem.Parent != null && baseItem.Parent != _0AF1 && baseItem.Parent.SubItems.Contains(baseItem))
			{
				baseItem.Parent.SubItems.Remove(baseItem);
			}
		}
		base.OnInsert(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (!m_IgnoreEvents)
		{
			BaseItem baseItem = value as BaseItem;
			object containerControl = baseItem.ContainerControl;
			baseItem.Style = _0AF1.Style;
			baseItem.Orientation = _0AF1.Orientation;
			baseItem.ThemeAware = _0AF1.ThemeAware;
			baseItem.SetParent(_0AF1);
			if (_0AF1.GetOwner() is IOwner owner)
			{
				owner.AddShortcutsFromItem(baseItem);
			}
			if (_0AF1 != null && _0AF1 is PopupItem && _0AF1.Expanded && ((PopupItem)_0AF1).PopupType != ePopupType.Container)
			{
				baseItem.ContainerControl = ((PopupItem)_0AF1).PopupControl;
				baseItem.OnContainerChanged(containerControl);
			}
			else
			{
				baseItem.ContainerControl = null;
				baseItem.OnContainerChanged(containerControl);
			}
			containerControl = null;
			baseItem.SetDesignMode(_0AF1.DesignMode);
			_0AF1.NeedRecalcSize = true;
			_0AF1.OnItemAdded(baseItem);
		}
		base.OnInsertComplete(index, value);
	}

	public virtual void Insert(int index, BaseItem item)
	{
		Add(item, index);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void _Insert(int index, BaseItem item)
	{
		m_IgnoreEvents = true;
		try
		{
			base.List.Insert(index, item);
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	public virtual int IndexOf(BaseItem value)
	{
		return base.List.IndexOf(value);
	}

	public virtual int IndexOf(string name)
	{
		int num = -1;
		foreach (BaseItem item in base.List)
		{
			num++;
			if (item.Name == name)
			{
				return num;
			}
		}
		return -1;
	}

	public virtual bool Contains(BaseItem value)
	{
		return base.List.Contains(value);
	}

	public virtual bool Contains(string name)
	{
		foreach (BaseItem item in base.List)
		{
			if (item.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void Remove(BaseItem item)
	{
		base.List.Remove(item);
	}

	public virtual void RemoveRange(BaseItem[] items)
	{
		foreach (BaseItem item in items)
		{
			Remove(item);
		}
	}

	protected override void OnRemove(int index, object value)
	{
		RemoveInternal(index, value);
		base.OnRemove(index, value);
	}

	protected virtual void RemoveInternal(int index, object value)
	{
		if (!m_IgnoreEvents)
		{
			BaseItem baseItem = value as BaseItem;
			baseItem.OnBeforeItemRemoved(null);
			_0AF1.OnBeforeItemRemoved(baseItem);
			if (_0AF1.GetOwner() is IOwner owner)
			{
				owner.RemoveShortcutsFromItem(baseItem);
			}
		}
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		RemoveCompleteInternal(index, value);
		base.OnRemoveComplete(index, value);
	}

	protected virtual void RemoveCompleteInternal(int index, object value)
	{
		if (!m_IgnoreEvents)
		{
			BaseItem baseItem = value as BaseItem;
			baseItem.SetParent(null);
			baseItem.OnAfterItemRemoved(null, index);
			baseItem.ContainerControl = null;
			_0AF1.OnAfterItemRemoved(baseItem, index);
			_0AF1.NeedRecalcSize = true;
			_0AF1.Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void _RemoveAt(int index)
	{
		m_IgnoreEvents = true;
		try
		{
			base.List.RemoveAt(index);
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void _Remove(BaseItem item)
	{
		m_IgnoreEvents = true;
		try
		{
			base.List.Remove(item);
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	public void Remove(int index)
	{
		Remove((BaseItem)base.List[index]);
	}

	public virtual void Remove(string name)
	{
		Remove(this[name]);
	}

	protected override void OnClear()
	{
		if (m_IgnoreEvents)
		{
			return;
		}
		_0AF1.GetOwner();
		if (_0AF1 != null)
		{
			_0AF1.SuspendLayout = true;
		}
		try
		{
			if (base.List.Count > 0)
			{
				for (int i = 0; i < base.Count; i++)
				{
					BaseItem value = this[i];
					RemoveInternal(i, value);
					RemoveCompleteInternal(i, value);
				}
			}
		}
		finally
		{
			if (_0AF1 != null)
			{
				_0AF1.SuspendLayout = false;
			}
		}
		if (_0AF1 != null)
		{
			_0AF1.OnSubItemsClear();
		}
	}

	public virtual void AddRange(BaseItem[] items)
	{
		foreach (BaseItem item in items)
		{
			Add(item);
		}
	}

	public virtual void InsertRange(int startPosition, BaseItem[] items)
	{
		foreach (BaseItem value in items)
		{
			base.List.Insert(startPosition, value);
			startPosition++;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CopyTo(ArrayList list)
	{
		if (list == null)
		{
			return;
		}
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BaseItem value = (BaseItem)enumerator.Current;
				list.Add(value);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public virtual void CopyTo(BaseItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0AF6(BaseItem[] ঀ, int _0AF7)
	{
		for (int i = _0AF7; i < base.List.Count; i++)
		{
			ঀ[i - _0AF7] = this[i];
		}
	}

	public void Sort()
	{
		base.InnerList.Sort();
	}

	public void Sort(IComparer comparer)
	{
		base.InnerList.Sort(comparer);
	}
}
