using System;
using System.Collections;

namespace Stimulsoft.Base;

public class StiRepositoryItems : ICloneable
{
	private class IntStorage
	{
		public int value;

		public IntStorage(int value)
		{
			this.value = value;
		}
	}

	private class FloatStorage
	{
		public float value;

		public FloatStorage(float value)
		{
			this.value = value;
		}
	}

	private Hashtable items;

	private static object ValueBoolFalse = new object();

	private static object ValueBoolTrue = new object();

	public object Clone()
	{
		StiRepositoryItems stiRepositoryItems = new StiRepositoryItems();
		if (items != null)
		{
			stiRepositoryItems.items = items.Clone() as Hashtable;
		}
		return stiRepositoryItems;
	}

	public bool Equals(StiRepositoryItems obj)
	{
		if (obj == null)
		{
			return false;
		}
		bool flag = items == null || items.Count == 0;
		bool flag2 = obj.items == null || obj.items.Count == 0;
		if (flag && flag2)
		{
			return true;
		}
		if ((flag && !flag2) || (!flag && flag2))
		{
			return false;
		}
		if (items.Count != obj.items.Count)
		{
			return false;
		}
		foreach (DictionaryEntry item in items)
		{
			if (!obj.items.ContainsKey(item.Key))
			{
				return false;
			}
			object obj2 = obj.items[item.Key];
			if (item.Value == null)
			{
				if (obj2 == null)
				{
					return true;
				}
				return false;
			}
			if (!item.Value.Equals(obj2))
			{
				return false;
			}
		}
		return true;
	}

	public void SetInt(object key, int value, int defaultValue)
	{
		if (value == defaultValue)
		{
			if (items != null && items[key] != null)
			{
				items.Remove(key);
			}
			return;
		}
		if (items == null)
		{
			items = new Hashtable();
		}
		items[key] = new IntStorage(value);
	}

	public int GetInt(object key, int defaultValue)
	{
		if (items == null)
		{
			return defaultValue;
		}
		if (!(items[key] is IntStorage intStorage))
		{
			return defaultValue;
		}
		return intStorage.value;
	}

	public void SetFloat(object key, float value, float defaultValue)
	{
		if (value == defaultValue)
		{
			if (items != null && items[key] != null)
			{
				items.Remove(key);
			}
			return;
		}
		if (items == null)
		{
			items = new Hashtable();
		}
		items[key] = new FloatStorage(value);
	}

	public float GetFloat(object key, float defaultValue)
	{
		if (items == null)
		{
			return defaultValue;
		}
		if (!(items[key] is FloatStorage floatStorage))
		{
			return defaultValue;
		}
		return floatStorage.value;
	}

	public void SetBool(object key, bool value, bool defaultValue)
	{
		if (value == defaultValue)
		{
			if (items != null && items[key] != null)
			{
				items.Remove(key);
			}
			return;
		}
		if (items == null)
		{
			items = new Hashtable();
		}
		if (value)
		{
			items[key] = ValueBoolTrue;
		}
		else
		{
			items[key] = ValueBoolFalse;
		}
	}

	public bool GetBool(object key, bool defaultValue)
	{
		if (items == null)
		{
			return defaultValue;
		}
		object obj = items[key];
		if (obj == ValueBoolFalse)
		{
			return false;
		}
		if (obj == ValueBoolTrue)
		{
			return true;
		}
		return defaultValue;
	}

	public void Set(object key, object value, object defaultValue)
	{
		if (items == null)
		{
			items = new Hashtable();
		}
		if (value == null || value == defaultValue)
		{
			if (items[key] != null)
			{
				items.Remove(key);
			}
		}
		else
		{
			items[key] = value;
		}
	}

	public object Get(object key, object defaultValue)
	{
		if (items == null)
		{
			return defaultValue;
		}
		object obj = items[key];
		if (obj == null)
		{
			return defaultValue;
		}
		return obj;
	}

	public bool IsPresent(object key)
	{
		if (items == null)
		{
			return false;
		}
		return items.ContainsKey(key);
	}
}
