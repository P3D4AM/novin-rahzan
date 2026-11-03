using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiTypesCollection : CollectionBase
{
	public StiType this[int index]
	{
		get
		{
			lock (base.List)
			{
				return (StiType)base.List[index];
			}
		}
		set
		{
			lock (base.List)
			{
				base.List[index] = value;
			}
		}
	}

	public StiType this[string name]
	{
		get
		{
			lock (base.List)
			{
				foreach (StiType item in base.List)
				{
					if (item.Name == name)
					{
						return item;
					}
				}
				return null;
			}
		}
		set
		{
			lock (base.List)
			{
				for (int i = 0; i < base.List.Count; i++)
				{
					StiType stiType = base.List[i] as StiType;
					if (stiType.Name == name)
					{
						base.List[i] = value;
						return;
					}
				}
				Add(value);
			}
		}
	}

	public StiType this[Type type]
	{
		get
		{
			lock (base.List)
			{
				foreach (StiType item in base.List)
				{
					if (item.Type == type)
					{
						return item;
					}
				}
				return null;
			}
		}
		set
		{
			lock (base.List)
			{
				int num = 0;
				foreach (StiType item in base.List)
				{
					if (item.Type == type)
					{
						base.List[num] = value;
					}
					num++;
				}
			}
		}
	}

	public StiType[] Items
	{
		get
		{
			lock (base.InnerList)
			{
				return (StiType[])base.InnerList.ToArray(typeof(StiType));
			}
		}
	}

	public List<StiType> ToList()
	{
		return this.Cast<StiType>().ToList();
	}

	public void Add(StiType type)
	{
		lock (base.List)
		{
			base.List.Add(type);
		}
	}

	public void AddRange(StiType[] type)
	{
		lock (base.InnerList)
		{
			base.InnerList.AddRange(type);
		}
	}

	public void AddRange(StiTypesCollection types)
	{
		foreach (StiType type in types)
		{
			Add(type);
		}
	}

	public bool Contains(StiType type)
	{
		lock (base.List)
		{
			return base.List.Contains(type);
		}
	}

	public int IndexOf(StiType type)
	{
		lock (base.List)
		{
			return base.List.IndexOf(type);
		}
	}

	public void Insert(int index, StiType type)
	{
		lock (base.List)
		{
			base.List.Insert(index, type);
		}
	}

	public void Remove(StiType type)
	{
		lock (base.List)
		{
			base.List.Remove(type);
		}
	}

	public void RegType(string name, Type type)
	{
		Add(new StiType(name, type));
	}
}
