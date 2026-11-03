using System;
using System.Collections;
using System.Threading;

namespace DevComponents.DotNetBar;

public class RibbonTabItemGroupCollection : CollectionBase
{
	private EventHandler ᦧ;

	private EventHandler ᦨ;

	private RibbonStrip _0AC3;

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

	internal RibbonStrip _၉
	{
		get
		{
			return _0AC3;
		}
		set
		{
			_0AC3 = value;
		}
	}

	public event EventHandler GroupRemoved
	{
		add
		{
			EventHandler eventHandler = ᦧ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᦧ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦧ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler GroupAdded
	{
		add
		{
			EventHandler eventHandler = ᦨ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦨ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᦨ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᦨ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		if (ᦧ != null)
		{
			ᦧ(ribbonTabItemGroup, new EventArgs());
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		RibbonTabItemGroup ribbonTabItemGroup = value as RibbonTabItemGroup;
		if (_0AC3 != null)
		{
			ribbonTabItemGroup.ParentRibbonStrip = _0AC3;
		}
		if (ᦨ != null)
		{
			ᦨ(ribbonTabItemGroup, new EventArgs());
		}
	}

	public void CopyTo(RibbonTabItemGroup[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(RibbonTabItemGroup[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		base.OnClear();
	}
}
