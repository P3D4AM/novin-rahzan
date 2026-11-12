using System.Collections;
using System.Reflection;

namespace Stimulsoft.Base.Serializing;

public class StiReferenceCollection : CollectionBase
{
	public StiReference this[int index]
	{
		get
		{
			return (StiReference)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiReference[] Items => (StiReference[])base.InnerList.ToArray(typeof(StiReference));

	public void Add(StiPropertyInfo propInfo)
	{
		base.List.Add(new StiReference(propInfo));
	}

	public void Add(StiPropertyInfo propInfo, object value, PropertyInfo propertyInfo)
	{
		base.List.Add(new StiReference(propInfo, value, propertyInfo));
	}

	public void Add(StiReference reference)
	{
		base.List.Add(reference);
	}

	public void AddRange(StiReference[] reference)
	{
		base.InnerList.AddRange(reference);
	}

	public void AddRange(StiReferenceCollection references)
	{
		base.InnerList.AddRange(references.Items);
	}

	public bool Contains(StiReference reference)
	{
		return base.List.Contains(reference);
	}

	public int IndexOf(StiReference reference)
	{
		return base.List.IndexOf(reference);
	}

	public void Insert(int index, StiReference reference)
	{
		base.List.Insert(index, reference);
	}

	public void Remove(StiReference reference)
	{
		base.List.Remove(reference);
	}
}
