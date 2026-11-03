using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report;

public class StiMetaTagCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiMetaTag this[int index]
	{
		get
		{
			return (StiMetaTag)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiMetaTag this[string name]
	{
		get
		{
			foreach (StiMetaTag item in base.List)
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
				StiMetaTag stiMetaTag = base.List[i] as StiMetaTag;
				if (stiMetaTag.Name == name)
				{
					base.List[i] = value;
					return;
				}
			}
			base.List.Add(value);
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiMetaTag item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiMetaTag stiMetaTag = new StiMetaTag(null, null);
			stiMetaTag.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiMetaTag);
		}
	}

	public void Add(string name, string tag)
	{
		base.List.Add(new StiMetaTag(name, tag));
	}

	public void Add(StiMetaTag metaTag)
	{
		base.List.Add(metaTag);
	}

	public void AddRange(StiMetaTag[] metaTags)
	{
		foreach (StiMetaTag metaTag in metaTags)
		{
			Add(metaTag);
		}
	}

	public void AddRange(StiMetaTagCollection metaTags)
	{
		foreach (StiMetaTag metaTag in metaTags)
		{
			Add(metaTag);
		}
	}

	public bool Contains(StiMetaTag metaTag)
	{
		return base.List.Contains(metaTag);
	}

	public int IndexOf(StiMetaTag metaTag)
	{
		return base.List.IndexOf(metaTag);
	}

	public void Insert(int index, StiMetaTag metaTag)
	{
		base.List.Insert(index, metaTag);
	}

	public void Remove(StiMetaTag metaTag)
	{
		base.List.Remove(metaTag);
	}

	public object Clone()
	{
		StiMetaTagCollection stiMetaTagCollection = new StiMetaTagCollection();
		foreach (StiMetaTag item in base.List)
		{
			stiMetaTagCollection.Add((StiMetaTag)item.Clone());
		}
		return stiMetaTagCollection;
	}
}
