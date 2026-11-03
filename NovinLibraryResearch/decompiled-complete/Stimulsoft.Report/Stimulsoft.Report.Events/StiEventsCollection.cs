using System.Collections;

namespace Stimulsoft.Report.Events;

public class StiEventsCollection : CollectionBase
{
	public StiEvent this[int index]
	{
		get
		{
			return (StiEvent)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiEvent ev)
	{
		base.List.Add(ev);
	}

	public void AddRange(StiEventsCollection evs)
	{
		foreach (StiEvent ev in evs)
		{
			Add(ev);
		}
	}

	public void AddRange(StiEvent[] evs)
	{
		foreach (StiEvent ev in evs)
		{
			Add(ev);
		}
	}

	public bool Contains(StiEvent ev)
	{
		return base.List.Contains(ev);
	}

	public int IndexOf(StiEvent ev)
	{
		return base.List.IndexOf(ev);
	}

	public void Insert(int index, StiEvent ev)
	{
		base.List.Insert(index, ev);
	}

	public void Remove(StiEventsCollection evs)
	{
		foreach (StiEvent ev in evs)
		{
			Remove(ev);
		}
	}

	public void Remove(StiEvent ev)
	{
		base.List.Remove(ev);
	}
}
