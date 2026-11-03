using System.Collections;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class TabsCollection : CollectionBase
{
	private TabStrip _0AC3;

	private bool _1CE6;

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
			int num = _1CE7(name);
			if (num >= 0)
			{
				return this[num];
			}
			return null;
		}
		set
		{
			int num = _1CE7(name);
			if (num >= 0)
			{
				base.List[num] = value;
			}
		}
	}

	public TabsCollection(TabStrip owner)
	{
		_0AC3 = owner;
	}

	public int Add(TabItem item)
	{
		_0AC3.ᓧ = true;
		return base.List.Add(item);
	}

	private int _1CE7(string _0956)
	{
		_0956 = _0956.ToLower();
		for (int i = 0; i < base.Count; i++)
		{
			if (this[i].Name.ToLower() == _0956)
			{
				return i;
			}
		}
		return -1;
	}

	public void Insert(int index, TabItem value)
	{
		value.Visible = true;
		_0AC3.ᓧ = true;
		base.List.Insert(index, value);
	}

	internal void __1CE8(int ԡ, TabItem ԝ)
	{
		_1CE6 = true;
		try
		{
			ԝ.Visible = true;
			_0AC3.ᓧ = true;
			base.List.Insert(ԡ, ԝ);
		}
		finally
		{
			_1CE6 = false;
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

	internal void _ᳩ(TabItem ԝ)
	{
		_1CE6 = true;
		try
		{
			base.List.Remove(ԝ);
		}
		finally
		{
			_1CE6 = false;
		}
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		if (!_1CE6)
		{
			TabItem tabItem = value as TabItem;
			_0AC3.ᕑ(tabItem);
			tabItem.SetParent(null);
			tabItem.Visible = true;
			if (_0AC3.SelectedTab == tabItem)
			{
				_0AC3.ᕐ(index, tabItem);
			}
			_0AC3.ᓧ = true;
			if (_0AC3.Ꮬ)
			{
				((Control)_0AC3).Refresh();
			}
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		if (!_1CE6)
		{
			TabItem tabItem = value as TabItem;
			tabItem.SetParent(_0AC3);
			_0AC3.ᕥ(tabItem);
			_0AC3.ᓧ = true;
			if (_0AC3.Ꮬ)
			{
				((Control)_0AC3).Refresh();
			}
		}
	}

	public void CopyTo(TabItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override void OnClear()
	{
		_0AC3.ᕗ();
		base.OnClear();
	}
}
