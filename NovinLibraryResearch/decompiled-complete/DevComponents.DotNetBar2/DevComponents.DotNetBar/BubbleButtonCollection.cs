using System;
using System.Collections;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

public class BubbleButtonCollection : CollectionBase
{
	private BubbleBarTab ӭ;

	private bool _1CE6;

	internal BubbleBarTab ކ => ӭ;

	public virtual BubbleButton this[int index]
	{
		get
		{
			return (BubbleButton)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public virtual BubbleButton this[string name]
	{
		get
		{
			return (BubbleButton)base.List[IndexOf(name)];
		}
		set
		{
			base.List[IndexOf(name)] = value;
		}
	}

	internal void _0CD0(IBlock[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	internal BubbleButtonCollection(BubbleBarTab parent)
	{
		ӭ = parent;
	}

	internal int __0AF3(BubbleButton _06EC)
	{
		_1CE6 = true;
		int num = 0;
		try
		{
			return base.List.Add(_06EC);
		}
		finally
		{
			_1CE6 = false;
		}
	}

	internal void __0AF3(BubbleButton _06EC, int _0AF4)
	{
		_1CE6 = true;
		try
		{
			base.List.Insert(_0AF4, _06EC);
		}
		finally
		{
			_1CE6 = false;
		}
	}

	internal void __0AF5()
	{
		_1CE6 = true;
		try
		{
			base.List.Clear();
		}
		finally
		{
			_1CE6 = false;
		}
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (newValue == null)
		{
			throw new InvalidOperationException("Setting of null values to BubbleButtonCollection is not allowed.");
		}
		BubbleButton bubbleButton = newValue as BubbleButton;
		if (bubbleButton.Parent != null)
		{
			bubbleButton.Parent.Buttons.Remove(bubbleButton);
		}
		base.OnSet(index, oldValue, newValue);
	}

	protected override void OnSetComplete(int index, object oldValue, object newValue)
	{
		if (!_1CE6)
		{
			BubbleButton bubbleButton = newValue as BubbleButton;
			bubbleButton.SetParentCollection(this);
			ӭ.OnButtonInserted(bubbleButton);
		}
		base.OnSetComplete(index, oldValue, newValue);
	}

	protected override void OnInsert(int index, object value)
	{
		BubbleButton bubbleButton = value as BubbleButton;
		if (bubbleButton.Parent != null && bubbleButton.Parent != ކ)
		{
			bubbleButton.Parent.Buttons.Remove(bubbleButton);
		}
		base.OnInsert(index, value);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (!_1CE6)
		{
			BubbleButton bubbleButton = value as BubbleButton;
			bubbleButton.SetParentCollection(this);
			ӭ.OnButtonInserted(bubbleButton);
		}
		base.OnInsertComplete(index, value);
	}

	protected override void OnRemove(int index, object value)
	{
		base.OnRemove(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		if (!_1CE6)
		{
			BubbleButton bubbleButton = value as BubbleButton;
			bubbleButton.SetParentCollection(null);
			ӭ.OnButtonRemoved(bubbleButton);
		}
		base.OnRemoveComplete(index, value);
	}

	internal void _ᳩ(BubbleButton _06EC)
	{
		_1CE6 = true;
		try
		{
			base.List.Remove(_06EC);
		}
		finally
		{
			_1CE6 = false;
		}
	}

	protected override void OnClear()
	{
		if (!_1CE6 && ӭ != null)
		{
			ӭ.OnButtonsCollectionClear();
		}
		base.OnClear();
	}

	public void CopyTo(ArrayList list)
	{
		if (list == null)
		{
			return;
		}
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BubbleButton value = (BubbleButton)enumerator.Current;
				list.Add(value);
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

	public virtual int Add(BubbleButton item)
	{
		return Add(item, -1);
	}

	public virtual int Add(BubbleButton item, int Position)
	{
		int result = Position;
		if (Position >= 0)
		{
			base.List.Insert(Position, item);
		}
		else
		{
			result = base.List.Add(item);
		}
		return result;
	}

	public virtual void Insert(int index, BubbleButton item)
	{
		Add(item, index);
	}

	public virtual int IndexOf(BubbleButton value)
	{
		return base.List.IndexOf(value);
	}

	public virtual int IndexOf(string name)
	{
		int num = -1;
		foreach (BubbleButton item in base.List)
		{
			num++;
			if (item.Name == name)
			{
				return num;
			}
		}
		return -1;
	}

	public virtual bool Contains(BubbleButton value)
	{
		return base.List.Contains(value);
	}

	public virtual bool Contains(string name)
	{
		foreach (BubbleButton item in base.List)
		{
			if (item.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void Remove(BubbleButton item)
	{
		base.List.Remove(item);
	}

	public void Remove(int index)
	{
		Remove((BubbleButton)base.List[index]);
	}

	public virtual void Remove(string name)
	{
		Remove(this[name]);
	}

	public virtual void AddRange(BubbleButton[] items)
	{
		foreach (BubbleButton item in items)
		{
			Add(item);
		}
	}

	public virtual void CopyTo(BubbleButton[] array, int index)
	{
		base.List.CopyTo(array, index);
	}
}
