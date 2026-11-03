using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report;

public class StiGlobalizationItemCollection : CollectionBase, IComparer
{
	public StiGlobalizationItem this[int index]
	{
		get
		{
			return (StiGlobalizationItem)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiGlobalizationItem stiGlobalizationItem = x as StiGlobalizationItem;
		StiGlobalizationItem stiGlobalizationItem2 = y as StiGlobalizationItem;
		return string.Compare(stiGlobalizationItem.PropertyName, stiGlobalizationItem2.PropertyName, StringComparison.Ordinal);
	}

	public void Add(StiGlobalizationItem data)
	{
		base.List.Add(data);
	}

	public void AddRange(StiGlobalizationItem[] data)
	{
		base.InnerList.AddRange(data);
	}

	public bool Contains(StiGlobalizationItem data)
	{
		return base.List.Contains(data);
	}

	public int IndexOf(StiGlobalizationItem data)
	{
		return base.List.IndexOf(data);
	}

	public void Insert(int index, StiGlobalizationItem data)
	{
		lock (this)
		{
			base.List.Insert(index, data);
		}
	}

	public void Remove(StiGlobalizationItem data)
	{
		lock (this)
		{
			base.List.Remove(data);
		}
	}

	public List<StiGlobalizationItem> ToList()
	{
		return base.InnerList.ToArray().Cast<StiGlobalizationItem>().ToList();
	}

	public void Sort()
	{
		base.InnerList.Sort(this);
	}
}
