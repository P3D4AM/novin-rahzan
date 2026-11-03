using System;
using System.Collections;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class CrumbBarItemsCollection : CollectionBase
{
	private CrumbBar _ᗚ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CrumbBar Parent => _ᗚ;

	public CrumbBarItem this[int index]
	{
		get
		{
			return (CrumbBarItem)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public CrumbBarItem this[string name]
	{
		get
		{
			return _21E1(name);
		}
		set
		{
			int num = _21E2(name);
			if (num == -1)
			{
				throw new ArgumentException("name cannot be found in this collection");
			}
			base.List[num] = value;
		}
	}

	public CrumbBarItemsCollection(CrumbBar parent)
	{
		_ᗚ = parent;
	}

	internal void _0CC5(CrumbBar ԟ)
	{
		_ᗚ = ԟ;
	}

	public int Add(CrumbBarItem ch)
	{
		return base.List.Add(ch);
	}

	public void Insert(int index, CrumbBarItem value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(CrumbBarItem value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(CrumbBarItem value)
	{
		return base.List.Contains(value);
	}

	public void Remove(CrumbBarItem value)
	{
		base.List.Remove(value);
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		CrumbBarItem crumbBarItem = (CrumbBarItem)oldValue;
		crumbBarItem.SetOwner(null);
		crumbBarItem = (CrumbBarItem)newValue;
		crumbBarItem.SetOwner(_ᗚ);
		base.OnSet(index, oldValue, newValue);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		CrumbBarItem crumbBarItem = (CrumbBarItem)value;
		crumbBarItem.SetOwner(null);
		if (crumbBarItem.IsSelected)
		{
			_ᗚ.SetSelectedItem(null, eEventSource.Code);
		}
		base.OnRemoveComplete(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		CrumbBarItem crumbBarItem = (CrumbBarItem)value;
		crumbBarItem.SetOwner(_ᗚ);
		crumbBarItem.ContainerControl = _ᗚ;
		crumbBarItem.Style = eDotNetBarStyle.Office2007;
		base.OnInsertComplete(index, value);
	}

	public void CopyTo(CrumbBarItem[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(CrumbBarItem[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		foreach (CrumbBarItem item in base.List)
		{
			item.SetOwner(null);
		}
		base.OnClear();
	}

	protected override void OnClearComplete()
	{
		if (_ᗚ != null)
		{
			_ᗚ._21D6();
		}
		base.OnClearComplete();
	}

	private CrumbBarItem _21E1(string _0956)
	{
		foreach (CrumbBarItem item in base.List)
		{
			if (item.Name == _0956)
			{
				return item;
			}
		}
		return null;
	}

	private int _21E2(string _0956)
	{
		for (int i = 0; i < base.List.Count; i++)
		{
			CrumbBarItem crumbBarItem = this[i];
			if (crumbBarItem.Name == _0956)
			{
				return i;
			}
		}
		return -1;
	}
}
