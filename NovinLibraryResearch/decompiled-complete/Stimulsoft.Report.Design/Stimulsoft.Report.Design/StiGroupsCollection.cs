using System.Collections;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Design;

public class StiGroupsCollection : CollectionBase
{
	public StiGroup this[int index]
	{
		get
		{
			return (StiGroup)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiGroup this[string name]
	{
		get
		{
			foreach (StiGroup item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				StiGroup stiGroup = base.List[i] as StiGroup;
				if (stiGroup.Name == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	[StiSerializable(StiSerializationVisibility.List)]
	public StiGroupsCollection Groups => this;

	public void Add(StiGroup group)
	{
		base.List.Add(group);
	}

	public void AddRange(StiGroup[] group)
	{
		base.InnerList.AddRange(group);
	}

	public void AddRange(StiGroupsCollection group)
	{
		base.InnerList.AddRange(group.InnerList);
	}

	public bool Contains(StiGroup group)
	{
		return base.List.Contains(group);
	}

	public int IndexOf(StiGroup group)
	{
		return base.List.IndexOf(group);
	}

	public void Insert(int index, StiGroup group)
	{
		base.List.Insert(index, group);
	}

	public void Remove(StiGroup group)
	{
		base.List.Remove(group);
	}
}
