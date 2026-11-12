using System;
using System.Collections;

namespace Stimulsoft.Controls;

public class StiStringCollection : CollectionBase, ICloneable
{
	public string this[int index]
	{
		get
		{
			lock (base.List)
			{
				return (string)base.List[index];
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

	public string this[string name]
	{
		get
		{
			lock (base.List)
			{
				foreach (string item in base.List)
				{
					if (item == name)
					{
						return item;
					}
				}
				return null;
			}
		}
	}

	public string[] Items
	{
		get
		{
			lock (base.InnerList)
			{
				return (string[])base.InnerList.ToArray(typeof(string));
			}
		}
	}

	public void Add(string str)
	{
		lock (base.List)
		{
			base.List.Add(str);
		}
	}

	public void AddRange(string[] strs)
	{
		lock (base.InnerList)
		{
			base.InnerList.AddRange(strs);
		}
	}

	public void AddRange(StiStringCollection strs)
	{
		foreach (string str in strs)
		{
			Add(str);
		}
	}

	public bool Contains(string str)
	{
		lock (base.List)
		{
			return base.List.Contains(str);
		}
	}

	public int IndexOf(string str)
	{
		lock (base.List)
		{
			return base.List.IndexOf(str);
		}
	}

	public void Insert(int index, string str)
	{
		lock (base.List)
		{
			base.List.Insert(index, str);
		}
	}

	public void Remove(string str)
	{
		lock (base.List)
		{
			base.List.Remove(str);
		}
	}

	public object Clone()
	{
		lock (base.List)
		{
			StiStringCollection stiStringCollection = new StiStringCollection();
			foreach (string item in base.List)
			{
				stiStringCollection.Add((string)item.Clone());
			}
			return stiStringCollection;
		}
	}
}
