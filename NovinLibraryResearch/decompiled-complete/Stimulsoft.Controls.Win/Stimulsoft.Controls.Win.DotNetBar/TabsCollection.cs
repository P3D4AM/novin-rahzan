using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class TabsCollection : CollectionBase
{
	private TabStrip m_Owner;

	private bool m_IgnoreEvents;

	public TabItem this[int index]
	{
		get
		{
			return (TabItem)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public TabItem this[string name]
	{
		get
		{
			if (name == null)
			{
				return null;
			}
			int indexOf = GetIndexOf(name);
			if (indexOf >= 0)
			{
				return this[indexOf];
			}
			return null;
		}
		set
		{
			int indexOf = GetIndexOf(name);
			if (indexOf >= 0)
			{
				base.List[indexOf] = value;
			}
		}
	}

	public TabsCollection(TabStrip owner)
	{
		m_Owner = owner;
	}

	public int Add(TabItem item)
	{
		m_Owner.NeedRecalcSize = true;
		return base.List.Add(item);
	}

	private int GetIndexOf(string name)
	{
		name = name.ToLower();
		for (int i = 0; i < base.Count; i++)
		{
			if (this[i].Name.ToLower() == name)
			{
				return i;
			}
		}
		return -1;
	}

	public void Insert(int index, TabItem value)
	{
		value.Visible = true;
		m_Owner.NeedRecalcSize = true;
		base.List.Insert(index, value);
	}

	internal void _Insert(int index, TabItem value)
	{
		m_IgnoreEvents = true;
		try
		{
			value.Visible = true;
			m_Owner.NeedRecalcSize = true;
			base.List.Insert(index, value);
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	public int IndexOf(TabItem value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(TabItem value)
	{
		return base.List.Contains(value);
	}

	public void Remove(TabItem value)
	{
		base.List.Remove(value);
	}

	internal void _Remove(TabItem value)
	{
		m_IgnoreEvents = true;
		try
		{
			base.List.Remove(value);
		}
		finally
		{
			m_IgnoreEvents = false;
		}
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		if (!m_IgnoreEvents)
		{
			TabItem tabItem = value as TabItem;
			m_Owner.InvokeTabRemoved(tabItem);
			tabItem.SetParent(null);
			tabItem.Visible = true;
			if (m_Owner.SelectedTab == tabItem)
			{
				m_Owner.OnSelectedTabRemoved(index, tabItem);
			}
			m_Owner.NeedRecalcSize = true;
			((Control)m_Owner).Refresh();
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		if (!m_IgnoreEvents)
		{
			TabItem tabItem = value as TabItem;
			tabItem.SetParent(m_Owner);
			m_Owner.OnTabAdded(tabItem);
			m_Owner.NeedRecalcSize = true;
			if (m_Owner.IsDesignMode)
			{
				((Control)m_Owner).Refresh();
			}
		}
	}

	public void CopyTo(TabItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override void OnClear()
	{
		m_Owner.InternalTabsCleared();
		base.OnClear();
	}
}
