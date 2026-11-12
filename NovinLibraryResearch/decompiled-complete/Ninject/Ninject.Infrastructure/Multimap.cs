using System.Collections;
using System.Collections.Generic;

namespace Ninject.Infrastructure;

public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>, IEnumerable
{
	private readonly Dictionary<TKey, ICollection<TValue>> items = new Dictionary<TKey, ICollection<TValue>>();

	public ICollection<TKey> Keys => items.Keys;

	public ICollection<ICollection<TValue>> Values => items.Values;

	public ICollection<TValue> this[TKey key]
	{
		get
		{
			Ensure.ArgumentNotNull(key, "key");
			if (!items.ContainsKey(key))
			{
				items[key] = new List<TValue>();
			}
			return items[key];
		}
	}

	public void Add(TKey key, TValue value)
	{
		Ensure.ArgumentNotNull(key, "key");
		Ensure.ArgumentNotNull(value, "value");
		this[key].Add(value);
	}

	public bool Remove(TKey key, TValue value)
	{
		Ensure.ArgumentNotNull(key, "key");
		Ensure.ArgumentNotNull(value, "value");
		if (!items.ContainsKey(key))
		{
			return false;
		}
		return items[key].Remove(value);
	}

	public bool RemoveAll(TKey key)
	{
		Ensure.ArgumentNotNull(key, "key");
		return items.Remove(key);
	}

	public void Clear()
	{
		items.Clear();
	}

	public bool ContainsKey(TKey key)
	{
		Ensure.ArgumentNotNull(key, "key");
		return items.ContainsKey(key);
	}

	public bool ContainsValue(TKey key, TValue value)
	{
		Ensure.ArgumentNotNull(key, "key");
		Ensure.ArgumentNotNull(value, "value");
		if (items.ContainsKey(key))
		{
			return items[key].Contains(value);
		}
		return false;
	}

	public IEnumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}

	IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>.GetEnumerator()
	{
		return items.GetEnumerator();
	}
}
