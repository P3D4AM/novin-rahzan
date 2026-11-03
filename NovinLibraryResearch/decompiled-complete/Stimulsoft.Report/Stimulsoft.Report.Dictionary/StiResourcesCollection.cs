using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiResourcesCollection : CollectionBase, ICloneable, IComparer
{
	private int directionFactor = 1;

	public StiResource this[int index]
	{
		get
		{
			return (StiResource)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiResource this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			foreach (StiResource item in base.List)
			{
				if (item.Name.ToLowerInvariant() == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				StiResource stiResource = base.List[i] as StiResource;
				if (stiResource.Name.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public object Clone()
	{
		StiResourcesCollection stiResourcesCollection = new StiResourcesCollection();
		(from c in ToList()
			select c.Clone() as StiResource).ToList().ForEach(stiResourcesCollection.Add);
		return stiResourcesCollection;
	}

	public List<StiResource> CloneAsList()
	{
		return (Clone() as StiResourcesCollection).ToList();
	}

	public JObject SaveToJsonObjectEx(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiResource item in base.List)
		{
			if ((int)mode == 0 || ((int)mode == 1 && (item.AvailableInTheViewer || item.Type == StiResourceType.FontEot || item.Type == StiResourceType.FontOtf || item.Type == StiResourceType.FontTtc || item.Type == StiResourceType.FontTtf || item.Type == StiResourceType.FontWoff || item.Type == StiResourceType.Map)))
			{
				JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject((StiJsonSaveMode)0));
				num++;
			}
		}
		return val;
	}

	public void LoadFromJsonObjectEx(JObject jObject, StiReport report)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiResource stiResource = new StiResource();
			stiResource.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiResource);
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiResource stiResource = x as StiResource;
		StiResource stiResource2 = y as StiResource;
		if (StiOptions.Designer.SortDictionaryByAliases)
		{
			return string.Compare(stiResource.Alias, stiResource2.Alias, StringComparison.Ordinal) * directionFactor;
		}
		return string.Compare(stiResource.Name, stiResource2.Name, StringComparison.Ordinal) * directionFactor;
	}

	public void Sort()
	{
		Sort(StiSortOrder.Asc);
	}

	public void Sort(StiSortOrder order)
	{
		if (order == StiSortOrder.Asc)
		{
			directionFactor = 1;
		}
		else
		{
			directionFactor = -1;
		}
		base.InnerList.Sort(this);
	}

	public List<StiResource> ToList()
	{
		return this.Cast<StiResource>().ToList();
	}

	public void Add(string name, StiResourceType type, byte[] content)
	{
		base.List.Add(new StiResource(name, type, content));
	}

	public void Add(StiResource resource)
	{
		base.List.Add(resource);
	}

	public void AddRange(StiResource[] resources)
	{
		foreach (StiResource value in resources)
		{
			base.List.Add(value);
		}
	}

	public void AddRange(StiResourcesCollection resources)
	{
		foreach (StiResource resource in resources)
		{
			Add(resource);
		}
	}

	public bool Contains(StiResource resource)
	{
		return base.List.Contains(resource);
	}

	public bool Contains(string name)
	{
		name = name.ToLower(CultureInfo.InvariantCulture);
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiResource stiResource = (StiResource)enumerator.Current;
				if (stiResource.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					return true;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}

	public int IndexOf(StiResource resource)
	{
		return base.List.IndexOf(resource);
	}

	public int IndexOf(string name)
	{
		name = name.ToLower(CultureInfo.InvariantCulture);
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiResource stiResource = (StiResource)enumerator.Current;
				if (stiResource.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					return num;
				}
				num++;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return -1;
	}

	public void Insert(int index, StiResource resource)
	{
		base.List.Insert(index, resource);
	}

	public void Remove(StiResource resource)
	{
		base.List.Remove(resource);
	}

	public void Remove(string name)
	{
		name = name.ToLower(CultureInfo.InvariantCulture);
		int num = 0;
		while (num < base.List.Count)
		{
			StiResource stiResource = base.List[num] as StiResource;
			if (stiResource.Name.ToLower(CultureInfo.InvariantCulture) == name)
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
