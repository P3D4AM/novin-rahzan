using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class RibbonTabItemGroupCollection : CollectionBase
{
	private RibbonStrip m_Owner;

	public RibbonTabItemGroup this[int index]
	{
		get
		{
			return (RibbonTabItemGroup)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public RibbonTabItemGroup this[string name]
	{
		get
		{
			foreach (RibbonTabItemGroup item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
	}

	internal RibbonStrip Owner
	{
		get
		{
			return m_Owner;
		}
		set
		{
			m_Owner = value;
		}
	}

	public event EventHandler GroupRemoved;

	public event EventHandler GroupAdded;

	public int Add(RibbonTabItemGroup tab)
	{
		return base.List.Add(tab);
	}

	public void AddRange(RibbonTabItemGroup[] groups)
	{
		foreach (RibbonTabItemGroup value in groups)
		{
			base.List.Add(value);
		}
	}

	public void Insert(int index, RibbonTabItemGroup value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(RibbonTabItemGroup value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(RibbonTabItemGroup value)
	{
		return base.List.Contains(value);
	}

	public void Remove(RibbonTabItemGroup value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		RibbonTabItemGroup ribbonTabItemGroup = value as RibbonTabItemGroup;
		ribbonTabItemGroup.ParentRibbonStrip = null;
		if (this.GroupRemoved != null)
		{
			this.GroupRemoved(ribbonTabItemGroup, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		RibbonTabItemGroup ribbonTabItemGroup = value as RibbonTabItemGroup;
		if (m_Owner != null)
		{
			ribbonTabItemGroup.ParentRibbonStrip = m_Owner;
		}
		if (this.GroupAdded != null)
		{
			this.GroupAdded(ribbonTabItemGroup, new EventArgs());
		}
	}

	public void CopyTo(RibbonTabItemGroup[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(RibbonTabItemGroup[] array)
	{
		base.List.CopyTo(array, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
