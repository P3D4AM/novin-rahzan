using System.Collections;

namespace Stimulsoft.Base.Serializing;

public class StiPropertyInfoCollection : CollectionBase
{
	private StiPropertyInfo parent;

	public StiPropertyInfo this[int index]
	{
		get
		{
			return (StiPropertyInfo)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiPropertyInfo[] Items => (StiPropertyInfo[])base.InnerList.ToArray(typeof(StiPropertyInfo));

	public void Add(StiPropertyInfo propertyInfo)
	{
		propertyInfo.Parent = parent;
		base.List.Add(propertyInfo);
	}

	public void AddRange(StiPropertyInfo[] propertyInfos)
	{
		foreach (StiPropertyInfo propertyInfo in propertyInfos)
		{
			Add(propertyInfo);
		}
	}

	public void AddRange(StiPropertyInfoCollection propertyInfos)
	{
		foreach (StiPropertyInfo propertyInfo in propertyInfos)
		{
			Add(propertyInfo);
		}
	}

	public bool Contains(StiPropertyInfo propertyInfo)
	{
		return base.List.Contains(propertyInfo);
	}

	public int IndexOf(StiPropertyInfo propertyInfo)
	{
		return base.List.IndexOf(propertyInfo);
	}

	public void Insert(int index, StiPropertyInfo propertyInfo)
	{
		base.List.Insert(index, propertyInfo);
	}

	public void Remove(StiPropertyInfo propertyInfo)
	{
		base.List.Remove(propertyInfo);
	}

	public StiPropertyInfoCollection()
		: this(null)
	{
	}

	public StiPropertyInfoCollection(StiPropertyInfo parentPropertyInfo)
	{
		parent = parentPropertyInfo;
	}
}
