using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class ProbeCollection : ICollection<ProbeFamily>, IEnumerable<ProbeFamily>, IEnumerable, IDictionary<string, ProbeFamily>, ICollection<KeyValuePair<string, ProbeFamily>>, IEnumerable<KeyValuePair<string, ProbeFamily>>
{
	private readonly ConcurrentDictionary<string, ProbeFamily> rep = new ConcurrentDictionary<string, ProbeFamily>(StringComparer.OrdinalIgnoreCase);

	public int Count => rep.Count;

	public bool IsReadOnly => false;

	public ICollection<string> Keys => rep.Keys;

	public ICollection<ProbeFamily> Values => rep.Values;

	public ProbeFamily this[string key]
	{
		get
		{
			return rep[key];
		}
		set
		{
			rep[key] = value;
		}
	}

	public bool TryGetImplementation(string name, ISqlObjectLocator target, out IProbe value)
	{
		value = null;
		if (rep.TryGetValue(name, out var value2))
		{
			return value2.TryGetValue(target, out value);
		}
		return false;
	}

	void ICollection<ProbeFamily>.Add(ProbeFamily newItem)
	{
		if (newItem == null)
		{
			throw new ArgumentNullException("newItem");
		}
		foreach (IProbe item in newItem)
		{
			Add(newItem.Name, item);
		}
	}

	public void Add(KeyValuePair<string, ProbeFamily> item)
	{
		rep.TryAdd(item.Key, item.Value);
	}

	public void Clear()
	{
		rep.Clear();
	}

	public bool Contains(KeyValuePair<string, ProbeFamily> item)
	{
		return rep.Contains(item);
	}

	public void CopyTo(KeyValuePair<string, ProbeFamily>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public bool Remove(KeyValuePair<string, ProbeFamily> item)
	{
		throw new NotImplementedException();
	}

	public bool Contains(ProbeFamily item)
	{
		return rep.Values.Contains(item);
	}

	public void CopyTo(ProbeFamily[] array, int arrayIndex)
	{
		rep.Values.CopyTo(array, arrayIndex);
	}

	public bool Remove(ProbeFamily item)
	{
		throw new NotImplementedException();
	}

	public void Add(string name, IProbe newItem)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (newItem == null)
		{
			throw new ArgumentNullException("newItem");
		}
		ProbeFamily orAdd = rep.GetOrAdd(name, (string nm) => new ProbeFamily(nm));
		orAdd.Add(newItem);
	}

	IEnumerator<KeyValuePair<string, ProbeFamily>> IEnumerable<KeyValuePair<string, ProbeFamily>>.GetEnumerator()
	{
		return rep.GetEnumerator();
	}

	public IEnumerator<ProbeFamily> GetEnumerator()
	{
		return rep.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<ProbeFamily>)this).GetEnumerator();
	}

	public void Add(string key, ProbeFamily value)
	{
		rep.TryAdd(key, value);
	}

	public bool ContainsKey(string key)
	{
		return rep.ContainsKey(key);
	}

	public bool Remove(string key)
	{
		throw new NotImplementedException();
	}

	public bool TryGetValue(string key, out ProbeFamily value)
	{
		return rep.TryGetValue(key, out value);
	}
}
