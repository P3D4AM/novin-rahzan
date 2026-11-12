using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class TriggerEventTypeSet : IMutableTriggerEventTypeSet, ITriggerEventTypeSet, IEnumerable<string>, IEnumerable
{
	private readonly HashSet<string> events;

	public TriggerEventTypeSet()
	{
		events = new HashSet<string>(CollationInfo.OrdinalIgnoreCase.EqualityComparer);
	}

	public void Add(string item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		events.Add(item);
	}

	public bool Remove(string item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return events.Remove(item);
	}

	public void Clear()
	{
		events.Clear();
	}

	public bool Contains(string item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return events.Contains(item);
	}

	public IEnumerator<string> GetEnumerator()
	{
		return events.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<string>)this).GetEnumerator();
	}
}
