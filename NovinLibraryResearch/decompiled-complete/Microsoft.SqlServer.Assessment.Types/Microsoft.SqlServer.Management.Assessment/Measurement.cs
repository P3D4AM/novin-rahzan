using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Assessment;

public class Measurement : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, IDataRow
{
	private readonly Dictionary<string, object> rep;

	public int Count => rep.Count;

	public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)rep).IsReadOnly;

	public ICollection<string> Keys => ((IDictionary<string, object>)rep).Keys;

	public ICollection<object> Values => ((IDictionary<string, object>)rep).Values;

	public object this[string key]
	{
		get
		{
			return rep[key];
		}
		set
		{
			rep[key] = CastValue(value);
		}
	}

	public Measurement()
	{
		rep = new Dictionary<string, object>();
	}

	public Measurement(int capacity)
	{
		rep = new Dictionary<string, object>(capacity);
	}

	public Measurement(IDictionary<string, object> dictionary)
	{
		rep = new Dictionary<string, object>(dictionary);
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		return rep.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)rep).GetEnumerator();
	}

	public void Add(KeyValuePair<string, object> item)
	{
		((ICollection<KeyValuePair<string, object>>)rep).Add(new KeyValuePair<string, object>(item.Key, CastValue(item.Value)));
	}

	public void Add(string key, object value)
	{
		rep.Add(key, CastValue(value));
	}

	public void Clear()
	{
		rep.Clear();
	}

	public bool Contains(KeyValuePair<string, object> item)
	{
		return ((ICollection<KeyValuePair<string, object>>)rep).Contains(new KeyValuePair<string, object>(item.Key, CastValue(item.Value)));
	}

	public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<string, object>>)rep).CopyTo(array, arrayIndex);
	}

	public bool Remove(KeyValuePair<string, object> item)
	{
		return ((ICollection<KeyValuePair<string, object>>)rep).Remove(new KeyValuePair<string, object>(item.Key, CastValue(item.Value)));
	}

	public bool ContainsKey(string key)
	{
		return rep.ContainsKey(key);
	}

	public bool Remove(string key)
	{
		return rep.Remove(key);
	}

	public bool TryGetValue(string key, out object value)
	{
		return rep.TryGetValue(key, out value);
	}

	public bool TryGetData(string column, out object value)
	{
		return TryGetValue(column, out value);
	}

	private static object CastValue(object value)
	{
		if (Convert.IsDBNull(value))
		{
			return null;
		}
		if (value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong || value is float || value is double)
		{
			return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
		}
		return value;
	}
}
