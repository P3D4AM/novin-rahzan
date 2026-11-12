using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class Bars : CollectionBase, IDisposable
{
	private DotNetBarManager m_Owner;

	internal DotNetBarManager Owner => m_Owner;

	public Bar this[int Index] => base.List[Index] as Bar;

	public Bar this[string Name]
	{
		get
		{
			foreach (Bar item in base.List)
			{
				if (item.Name == Name)
				{
					return item;
				}
			}
			return null;
		}
	}

	internal Bars(DotNetBarManager Owner)
	{
		m_Owner = Owner;
	}

	public void Dispose()
	{
		m_Owner = null;
		base.List.Clear();
	}

	public void Add(Bar bar)
	{
		if (base.List.IndexOf(bar) < 0)
		{
			base.List.Add(bar);
			return;
		}
		throw new InvalidOperationException("Bar is already in collection.");
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		Bar bar = (Bar)value;
		bar.Owner = m_Owner;
		bar.ThemeAware = m_Owner.ThemeAware;
		bar.Style = m_Owner.Style;
		((IOwnerBarSupport)m_Owner)?.AddShortcutsFromBar(bar);
		bar.OnAddedToBars();
	}

	public virtual void Remove(Bar bar)
	{
		base.List.Remove(bar);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		Bar bar = (Bar)value;
		((IOwnerBarSupport)m_Owner)?.RemoveShortcutsFromBar(bar);
		if (bar.Visible)
		{
			bar.HideBar();
		}
		if (((Control)bar).Parent != null)
		{
			if (((Control)bar).Parent is DockSite && ((DockSite)(object)((Control)bar).Parent).IsDocumentDock)
			{
				((DockSite)(object)((Control)bar).Parent).GetDocumentUIManager().UnDock(bar);
			}
			if (((Control)bar).Parent != null)
			{
				((Control)bar).Parent.Controls.Remove((Control)(object)bar);
			}
		}
		base.OnRemoveComplete(index, value);
	}

	protected override void OnClear()
	{
		if (m_Owner != null && m_Owner.GetDesignMode())
		{
			return;
		}
		foreach (Bar item in base.List)
		{
			if (!item.GetDesignMode())
			{
				if (item.AutoHide)
				{
					item.AutoHide = false;
				}
				if (item.Visible)
				{
					item.HideBar();
				}
				if (item.DockSide == eDockSide.Document && ((Control)item).Parent is DockSite && ((DockSite)(object)((Control)item).Parent).GetDocumentUIManager() != null)
				{
					((DockSite)(object)((Control)item).Parent).GetDocumentUIManager().UnDock(item);
				}
				if (item != null && !((Control)item).IsDisposed)
				{
					((IOwnerBarSupport)m_Owner).RemoveShortcutsFromBar(item);
				}
				if (((Control)item).Parent != null)
				{
					((Control)item).Parent.Controls.Remove((Control)(object)item);
				}
				if (!item.GetDesignMode())
				{
					((Component)(object)item).Dispose();
				}
			}
		}
	}

	public bool Contains(Bar bar)
	{
		return base.List.Contains(bar);
	}

	public bool Contains(string name)
	{
		foreach (Bar item in base.List)
		{
			if (item.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(Bar bar)
	{
		return base.List.IndexOf(bar);
	}

	public void CopyTo(Array array)
	{
		base.List.CopyTo(array, 0);
	}

	internal void ClearNonDocumentBars()
	{
		ArrayList arrayList = new ArrayList(base.Count);
		foreach (Bar item in base.List)
		{
			if (item.DockSide != eDockSide.Document)
			{
				arrayList.Add(item);
			}
		}
		foreach (Bar item2 in arrayList)
		{
			Remove(item2);
		}
	}
}
