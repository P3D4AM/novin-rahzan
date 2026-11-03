using System;

namespace DevComponents.DotNetBar;

public class ItemBindingData
{
	private WeakReference _ᬐ;

	private int _ԇ;

	private object __1714;

	public object DataItem
	{
		get
		{
			if (_ᬐ == null)
			{
				return null;
			}
			if (_ᬐ.IsAlive)
			{
				return _ᬐ.Target;
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				_ᬐ = new WeakReference(value);
			}
			else
			{
				_ᬐ = null;
			}
		}
	}

	public int BindingIndex
	{
		get
		{
			return _ԇ;
		}
		set
		{
			_ԇ = value;
		}
	}

	public object Tag
	{
		get
		{
			return __1714;
		}
		set
		{
			__1714 = value;
		}
	}

	public ItemBindingData(object dataItem, int bindingIndex)
	{
		DataItem = dataItem;
		_ԇ = bindingIndex;
	}
}
