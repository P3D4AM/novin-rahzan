using System;
using System.Collections;
using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SubItemsCollection : CollectionBase
{
	private BaseItem m_ParentItem;

	protected bool m_IgnoreEvents;

	private bool m_AllowParentRemove = true;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowParentRemove
	{
		get
		{
			return m_AllowParentRemove;
		}
		set
		{
			m_AllowParentRemove = value;
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

	internal bool IgnoreEvents
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
		m_ParentItem = parent;
	}

	internal int _Add(BaseItem item)
	{
		m_IgnoreEvents = true;
		m_AllowParentRemove = false;
		int num = 0;
		try
		{
			return base.List.Add(item);
		}
		finally
		{
			m_IgnoreEvents = false;
			m_AllowParentRemove = true;
		}
	}

	internal void _Add(BaseItem item, int Position)
	{
		m_IgnoreEvents = true;
		m_AllowParentRemove = false;
		try
		{
			if (Position >= 0)
			{
				base.List.Insert(Position, item);
			}
			else
			{
				base.List.Add(item);
			}
		}
		finally
		{
			m_IgnoreEvents = false;
			m_AllowParentRemove = true;
		}
	}

	internal void _Clear()
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
			if (m_AllowParentRemove && baseItem.Parent != null && baseItem.Parent != m_ParentItem && baseItem.Parent.SubItems.Contains(baseItem))
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
			baseItem.Style = m_ParentItem.Style;
			baseItem.Orientation = m_ParentItem.Orientation;
			baseItem.ThemeAware = m_ParentItem.ThemeAware;
			baseItem.SetParent(m_ParentItem);
			if (m_ParentItem.GetOwner() is IOwner owner)
			{
				owner.AddShortcutsFromItem(baseItem);
			}
			if (m_ParentItem != null && m_ParentItem is PopupItem && m_ParentItem.Expanded && ((PopupItem)m_ParentItem).PopupType != ePopupType.Container)
			{
				baseItem.ContainerControl = ((PopupItem)m_ParentItem).PopupControl;
				baseItem.OnContainerChanged(containerControl);
			}
			else
			{
				baseItem.ContainerControl = null;
				baseItem.OnContainerChanged(containerControl);
			}
			containerControl = null;
			baseItem.SetDesignMode(m_ParentItem.DesignMode);
			m_ParentItem.NeedRecalcSize = true;
			m_ParentItem.OnItemAdded(baseItem);
		}
		base.OnInsertComplete(index, value);
	}

	public virtual void Insert(int index, BaseItem item)
	{
		Add(item, index);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void InsertItem(int index, BaseItem item)
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
			m_ParentItem.OnBeforeItemRemoved(baseItem);
			if (m_ParentItem.GetOwner() is IOwner owner)
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
			baseItem.OnAfterItemRemoved(null);
			baseItem.ContainerControl = null;
			m_ParentItem.OnAfterItemRemoved(baseItem);
			m_ParentItem.NeedRecalcSize = true;
			m_ParentItem.Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public new void RemoveAt(int index)
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
	public void RemoveItem(BaseItem item)
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
		IOwner owner = m_ParentItem.GetOwner() as IOwner;
		if (m_ParentItem != null)
		{
			m_ParentItem.SuspendLayout = true;
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
			if (m_ParentItem != null)
			{
				m_ParentItem.SuspendLayout = false;
			}
		}
		owner = null;
		if (m_ParentItem != null)
		{
			m_ParentItem.OnSubItemsClear();
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

	internal void CopyToFromIndex(BaseItem[] array, int fromCollectionIndex)
	{
		for (int i = fromCollectionIndex; i < base.List.Count; i++)
		{
			array[i - fromCollectionIndex] = this[i];
		}
	}

	public void Sort()
	{
		base.InnerList.Sort();
	}

	private void Sort(IComparer comparer)
	{
		base.InnerList.Sort(comparer);
	}
}
