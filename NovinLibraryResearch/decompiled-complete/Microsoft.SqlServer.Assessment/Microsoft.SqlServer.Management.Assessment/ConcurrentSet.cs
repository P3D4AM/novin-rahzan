using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

public class ConcurrentSet<TItem>
{
	private readonly ConcurrentDictionary<TItem, bool> rep;

	public ConcurrentSet(IEqualityComparer<TItem> comparer)
	{
		rep = new ConcurrentDictionary<TItem, bool>(comparer);
	}

	public bool Add(TItem item)
	{
		return rep.TryAdd(item, value: true);
	}

	public bool Contains(TItem item)
	{
		return rep.ContainsKey(item);
	}

	public void Remove(TItem item)
	{
		rep.TryRemove(item, out var _);
	}
}
