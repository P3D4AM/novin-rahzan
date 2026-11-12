using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemBindingData
{
	private WeakReference _DataItem;

	private int _BindingIndex;

	private object _Tag;

	public object DataItem
	{
		get
		{
			if (_DataItem == null)
			{
				return null;
			}
			if (_DataItem.IsAlive)
			{
				return _DataItem.Target;
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				_DataItem = new WeakReference(value);
			}
			else
			{
				_DataItem = null;
			}
		}
	}

	public int BindingIndex
	{
		get
		{
			return _BindingIndex;
		}
		set
		{
			_BindingIndex = value;
		}
	}

	public object Tag
	{
		get
		{
			return _Tag;
		}
		set
		{
			_Tag = value;
		}
	}

	public ItemBindingData(object dataItem, int bindingIndex)
	{
		DataItem = dataItem;
		_BindingIndex = bindingIndex;
	}
}
