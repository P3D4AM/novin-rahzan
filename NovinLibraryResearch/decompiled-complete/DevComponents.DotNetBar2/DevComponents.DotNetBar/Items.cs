using System;
using System.Collections;

namespace DevComponents.DotNetBar;

public class Items : IDisposable, IEnumerable
{
	private SortedList ณ;

	private DotNetBarManager _0AC3;

	public BaseItem this[string sItemName]
	{
		get
		{
			if (ณ.ContainsKey(sItemName))
			{
				return ณ[sItemName] as BaseItem;
			}
			foreach (DictionaryEntry item in ณ)
			{
				BaseItem baseItem = item.Value as BaseItem;
				if (baseItem.Name == sItemName)
				{
					return baseItem;
				}
				if (baseItem.SubItems.Count > 0)
				{
					baseItem = _27B5(sItemName, baseItem);
					if (baseItem != null)
					{
						return baseItem;
					}
				}
			}
			throw new InvalidOperationException("Item not found in collection");
		}
	}

	public BaseItem this[int iIndex] => ณ.GetByIndex(iIndex) as BaseItem;

	public int Count => ณ.Count;

	internal Items(DotNetBarManager objOwner)
	{
		ณ = new SortedList();
		_0AC3 = objOwner;
	}

	public void Dispose()
	{
		Clear();
		ณ = null;
		_0AC3 = null;
	}

	public void Clear()
	{
		if (_0AC3 != null)
		{
			foreach (BaseItem value in ณ.Values)
			{
				((IOwner)_0AC3).RemoveShortcutsFromItem(value);
			}
		}
		ณ.Clear();
	}

	public void Add(BaseItem objItem)
	{
		if (objItem == null)
		{
			throw new ArgumentNullException("Item must be valid value");
		}
		if (objItem.Name == null || objItem.Name == "")
		{
			objItem.Name = "item_" + objItem.Id;
		}
		if (ณ.ContainsKey(objItem.Name))
		{
			throw new InvalidOperationException("Item with this name already exists");
		}
		if (objItem.Parent != null)
		{
			throw new InvalidOperationException("Item already has a Parent. Remove item from Parent first.");
		}
		objItem.SetOwner(_0AC3);
		objItem.GlobalItem = true;
		ณ.Add(objItem.Name, objItem);
	}

	public void AddCopy(BaseItem objItem)
	{
		if (objItem == null)
		{
			throw new ArgumentNullException("Item must be valid value");
		}
		if (objItem.Name == null || objItem.Name == "")
		{
			objItem.Name = "item_" + objItem.Id;
		}
		if (ณ.ContainsKey(objItem.Name))
		{
			throw new InvalidOperationException("Item with this name already exists");
		}
		BaseItem baseItem = objItem.Copy();
		baseItem.SetOwner(_0AC3);
		baseItem.GlobalItem = true;
		ณ.Add(baseItem.Name, baseItem);
	}

	public void Remove(BaseItem objItemToRemove)
	{
		if (ณ.ContainsKey(objItemToRemove.Name))
		{
			ณ.Remove(objItemToRemove.Name);
			objItemToRemove.SetOwner(null);
			((IOwner)_0AC3).RemoveShortcutsFromItem(objItemToRemove);
			return;
		}
		string name = objItemToRemove.Name;
		foreach (DictionaryEntry item in ณ)
		{
			BaseItem baseItem = item.Value as BaseItem;
			if (baseItem == objItemToRemove)
			{
				ณ.RemoveAt(ณ.IndexOfValue(baseItem));
				objItemToRemove.SetOwner(null);
				((IOwner)_0AC3).RemoveShortcutsFromItem(objItemToRemove);
				return;
			}
			if (baseItem.SubItems.Count > 0 && _27B7(name, baseItem))
			{
				return;
			}
		}
		ณ.Remove(objItemToRemove.Name);
		objItemToRemove.SetOwner(null);
	}

	public void Remove(string sItemName)
	{
		Remove(ณ[sItemName] as BaseItem);
	}

	private BaseItem _27B5(string _27B6, BaseItem _෮)
	{
		BaseItem baseItem = null;
		foreach (BaseItem subItem in _෮.SubItems)
		{
			if (subItem.Name == _27B6)
			{
				return subItem;
			}
			if (subItem.SubItems.Count > 0)
			{
				baseItem = _27B5(_27B6, subItem);
				if (baseItem != null)
				{
					return baseItem;
				}
			}
		}
		return null;
	}

	private bool _27B7(string _27B6, BaseItem _෮)
	{
		foreach (BaseItem subItem in _෮.SubItems)
		{
			if (subItem.Name == _27B6)
			{
				_෮.SubItems.Remove(subItem);
				subItem.SetOwner(null);
				((IOwner)_0AC3).RemoveShortcutsFromItem(subItem);
				return true;
			}
			if (subItem.SubItems.Count > 0 && _27B7(_27B6, subItem))
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerator GetEnumerator()
	{
		return ณ.GetEnumerator();
	}

	public bool Contains(BaseItem objItem)
	{
		return ณ.ContainsValue(objItem);
	}

	public bool Contains(string sName)
	{
		return ณ.ContainsKey(sName);
	}
}
