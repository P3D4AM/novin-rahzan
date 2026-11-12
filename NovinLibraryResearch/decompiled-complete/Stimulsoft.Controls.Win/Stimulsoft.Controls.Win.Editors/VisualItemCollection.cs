using System.Collections;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualItemCollection : CollectionBase
{
	private VisualGroup _Parent;

	internal VisualGroup Parent
	{
		get
		{
			return _Parent;
		}
		set
		{
			_Parent = value;
		}
	}

	public VisualItem this[int index]
	{
		get
		{
			return (VisualItem)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public VisualItemCollection()
	{
	}

	public VisualItemCollection(VisualGroup parent)
	{
		_Parent = parent;
	}

	public int Add(VisualItem item)
	{
		return base.List.Add(item);
	}

	public void AddRange(VisualItem[] items)
	{
		foreach (VisualItem item in items)
		{
			Add(item);
		}
	}

	public void AddRange(IList items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			Add((VisualItem)items[i]);
		}
	}

	public void Insert(int index, VisualItem value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(VisualItem value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(VisualItem value)
	{
		return base.List.Contains(value);
	}

	public void Remove(VisualItem value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemove(int index, object value)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, new VisualItem[1] { (VisualItem)value }, eCollectionChangeType.Removing));
		}
		base.OnRemove(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, new VisualItem[1] { (VisualItem)value }, eCollectionChangeType.Removed));
		}
		base.OnRemoveComplete(index, value);
	}

	protected override void OnInsert(int index, object value)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[1] { (VisualItem)value }, null, eCollectionChangeType.Adding));
		}
		base.OnInsert(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[1] { (VisualItem)value }, null, eCollectionChangeType.Added));
		}
		base.OnInsertComplete(index, value);
	}

	protected override void OnClear()
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, null, eCollectionChangeType.Clearing));
		}
		base.OnClear();
	}

	protected override void OnClearComplete()
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(null, null, eCollectionChangeType.Cleared));
		}
		base.OnClearComplete();
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[1] { (VisualItem)oldValue }, new VisualItem[1] { (VisualItem)newValue }, eCollectionChangeType.Removing));
		}
		base.OnSet(index, oldValue, newValue);
	}

	protected override void OnSetComplete(int index, object oldValue, object newValue)
	{
		if (_Parent != null)
		{
			_Parent.ProcessItemsCollectionChanged(new CollectionChangedInfo(new VisualItem[1] { (VisualItem)oldValue }, new VisualItem[1] { (VisualItem)newValue }, eCollectionChangeType.Removed));
		}
		base.OnSetComplete(index, oldValue, newValue);
	}

	public void CopyTo(VisualItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(VisualItem[] array)
	{
		base.List.CopyTo(array, 0);
	}
}
