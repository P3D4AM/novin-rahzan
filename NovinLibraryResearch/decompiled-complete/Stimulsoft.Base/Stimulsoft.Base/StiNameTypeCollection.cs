using System;
using System.Collections;

namespace Stimulsoft.Base;

public class StiNameTypeCollection : CollectionBase
{
	public StiNameTypeEntry this[int index]
	{
		get
		{
			return (StiNameTypeEntry)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiNameTypeEntry entry)
	{
		base.List.Add(entry);
	}

	public void Add(string name, Type type)
	{
		Add(new StiNameTypeEntry(name, type));
	}

	public void Insert(int index, StiNameTypeEntry entry)
	{
		base.List.Insert(index, entry);
	}

	public void Insert(int index, string name, Type type)
	{
		base.List.Insert(index, new StiNameTypeEntry(name, type));
	}

	public void Remove(string name)
	{
		int num = 0;
		while (num < base.List.Count)
		{
			if (this[num].Name == name)
			{
				base.List.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}
}
