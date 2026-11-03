using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class Bars : CollectionBase, IDisposable
{
	private DotNetBarManager _0AC3;

	internal DotNetBarManager _၉ => _0AC3;

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
		_0AC3 = Owner;
	}

	public void Dispose()
	{
		_0AC3 = null;
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
		bar.Owner = _0AC3;
		bar.ThemeAware = _0AC3.ThemeAware;
		bar.Style = _0AC3.Style;
		((IOwnerBarSupport)_0AC3)?.AddShortcutsFromBar(bar);
		bar._0BF0();
	}

	public virtual void Remove(Bar bar)
	{
		base.List.Remove(bar);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		Bar bar = (Bar)value;
		((IOwnerBarSupport)_0AC3)?.RemoveShortcutsFromBar(bar);
		if (bar.Visible)
		{
			bar.స();
		}
		if (((Control)bar).Parent != null)
		{
			if (((Control)bar).Parent is DockSite && ((DockSite)(object)((Control)bar).Parent).ၓ)
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
		if (_0AC3 != null && _0AC3.GetDesignMode())
		{
			return;
		}
		foreach (Bar item in base.List)
		{
			if (!item.ఛ())
			{
				if (item.AutoHide)
				{
					item.AutoHide = false;
				}
				if (item.Visible)
				{
					item.స();
				}
				if (item.DockSide == eDockSide.Document && ((Control)item).Parent is DockSite && ((DockSite)(object)((Control)item).Parent).GetDocumentUIManager() != null)
				{
					((DockSite)(object)((Control)item).Parent).GetDocumentUIManager().UnDock(item);
				}
				if (item != null && !((Control)item).IsDisposed)
				{
					((IOwnerBarSupport)_0AC3).RemoveShortcutsFromBar(item);
				}
				if (((Control)item).Parent != null)
				{
					((Control)item).Parent.Controls.Remove((Control)(object)item);
				}
				if (!item.ఛ())
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

	internal void _2241()
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
