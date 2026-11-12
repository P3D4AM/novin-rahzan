using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class Items : IEnumerable, IDisposable
{
	private SortedList m_Items;

	private DotNetBarManager m_Owner;

	public BaseItem this[string sItemName]
	{
		get
		{
			if (m_Items.ContainsKey(sItemName))
			{
				return m_Items[sItemName] as BaseItem;
			}
			foreach (DictionaryEntry item in m_Items)
			{
				BaseItem baseItem = item.Value as BaseItem;
				if (baseItem.Name == sItemName)
				{
					return baseItem;
				}
				if (baseItem.SubItems.Count > 0)
				{
					baseItem = GetItem(sItemName, baseItem);
					if (baseItem != null)
					{
						return baseItem;
					}
				}
			}
			throw new InvalidOperationException("Item not found in collection");
		}
	}

	public BaseItem this[int iIndex] => m_Items.GetByIndex(iIndex) as BaseItem;

	public int Count => m_Items.Count;

	internal Items(DotNetBarManager objOwner)
	{
		m_Items = new SortedList();
		m_Owner = objOwner;
	}

	public void Dispose()
	{
		Clear();
		m_Items = null;
		m_Owner = null;
	}

	public void Clear()
	{
		if (m_Owner != null)
		{
			foreach (BaseItem value in m_Items.Values)
			{
				((IOwner)m_Owner).RemoveShortcutsFromItem(value);
			}
		}
		m_Items.Clear();
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
		if (m_Items.ContainsKey(objItem.Name))
		{
			throw new InvalidOperationException("Item with this name already exists");
		}
		if (objItem.Parent != null)
		{
			throw new InvalidOperationException("Item already has a Parent. Remove item from Parent first.");
		}
		objItem.SetOwner(m_Owner);
		objItem.GlobalItem = true;
		m_Items.Add(objItem.Name, objItem);
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
		if (m_Items.ContainsKey(objItem.Name))
		{
			throw new InvalidOperationException("Item with this name already exists");
		}
		BaseItem baseItem = objItem.Copy();
		baseItem.SetOwner(m_Owner);
		baseItem.GlobalItem = true;
		m_Items.Add(baseItem.Name, baseItem);
	}

	public void Remove(BaseItem objItemToRemove)
	{
		if (m_Items.ContainsKey(objItemToRemove.Name))
		{
			m_Items.Remove(objItemToRemove.Name);
			objItemToRemove.SetOwner(null);
			((IOwner)m_Owner).RemoveShortcutsFromItem(objItemToRemove);
			return;
		}
		string name = objItemToRemove.Name;
		foreach (DictionaryEntry item in m_Items)
		{
			BaseItem baseItem = item.Value as BaseItem;
			if (baseItem == objItemToRemove)
			{
				m_Items.RemoveAt(m_Items.IndexOfValue(baseItem));
				objItemToRemove.SetOwner(null);
				((IOwner)m_Owner).RemoveShortcutsFromItem(objItemToRemove);
				return;
			}
			if (baseItem.SubItems.Count > 0 && RemoveItem(name, baseItem))
			{
				return;
			}
		}
		m_Items.Remove(objItemToRemove.Name);
		objItemToRemove.SetOwner(null);
	}

	public void Remove(string sItemName)
	{
		Remove(m_Items[sItemName] as BaseItem);
	}

	private BaseItem GetItem(string sName, BaseItem objParent)
	{
		BaseItem baseItem = null;
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if (subItem.Name == sName)
			{
				return subItem;
			}
			if (subItem.SubItems.Count > 0)
			{
				baseItem = GetItem(sName, subItem);
				if (baseItem != null)
				{
					return baseItem;
				}
			}
		}
		return null;
	}

	private bool RemoveItem(string sName, BaseItem objParent)
	{
		foreach (BaseItem subItem in objParent.SubItems)
		{
			if (subItem.Name == sName)
			{
				objParent.SubItems.Remove(subItem);
				subItem.SetOwner(null);
				((IOwner)m_Owner).RemoveShortcutsFromItem(subItem);
				return true;
			}
			if (subItem.SubItems.Count > 0 && RemoveItem(sName, subItem))
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerator GetEnumerator()
	{
		return m_Items.GetEnumerator();
	}

	public bool Contains(BaseItem objItem)
	{
		return m_Items.ContainsValue(objItem);
	}

	public bool Contains(string sName)
	{
		return m_Items.ContainsKey(sName);
	}
}
