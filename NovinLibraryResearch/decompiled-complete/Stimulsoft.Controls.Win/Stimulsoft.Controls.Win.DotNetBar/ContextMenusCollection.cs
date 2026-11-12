using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ContextMenusCollection : CollectionBase
{
	private DotNetBarManager m_Owner;

	public BaseItem this[int index]
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

	public BaseItem this[string name]
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

	public ContextMenusCollection(DotNetBarManager owner)
	{
		m_Owner = owner;
	}

	public int Add(BaseItem item)
	{
		item.SetOwner(m_Owner);
		item.Visible = false;
		item.Displayed = false;
		return base.List.Add(item);
	}

	public void Insert(int index, BaseItem value)
	{
		value.Visible = false;
		value.Displayed = false;
		base.List.Insert(index, value);
	}

	public int IndexOf(BaseItem value)
	{
		return base.List.IndexOf(value);
	}

	public int IndexOf(string name)
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

	public bool Contains(BaseItem value)
	{
		return base.List.Contains(value);
	}

	public bool Contains(string name)
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

	public void Remove(BaseItem value)
	{
		base.List.Remove(value);
	}

	public void CopyTo(BaseItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override void OnClear()
	{
		IOwner owner = m_Owner;
		if (base.List.Count > 0 && owner != null)
		{
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						BaseItem objItem = (BaseItem)enumerator.Current;
						owner?.RemoveShortcutsFromItem(objItem);
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
		}
		base.OnClear();
	}

	internal void SetOwner(DotNetBarManager owner)
	{
		m_Owner = owner;
	}
}
