using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiReportResourceCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiReportResource this[int index]
	{
		get
		{
			return (StiReportResource)base.List[index];
		}
		set
		{
			base.List[index] = value;
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
		foreach (StiReportResource item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiReportResource stiReportResource = new StiReportResource(null, null, null);
			stiReportResource.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiReportResource);
		}
	}

	public void Add(StiReportResource resource)
	{
		base.List.Add(resource);
	}

	public void AddRange(StiReportResource[] resources)
	{
		foreach (StiReportResource resource in resources)
		{
			Add(resource);
		}
	}

	public void AddRange(StiReportResourceCollection resources)
	{
		foreach (StiReportResource resource in resources)
		{
			Add(resource);
		}
	}

	public bool Contains(StiReportResource resource)
	{
		return base.List.Contains(resource);
	}

	public int IndexOf(StiReportResource resource)
	{
		return base.List.IndexOf(resource);
	}

	public void Insert(int index, StiReportResource resource)
	{
		base.List.Insert(index, resource);
	}

	public void Remove(StiReportResource resource)
	{
		base.List.Remove(resource);
	}

	public object Clone()
	{
		StiReportResourceCollection stiReportResourceCollection = new StiReportResourceCollection();
		foreach (StiReportResource item in base.List)
		{
			stiReportResourceCollection.Add((StiReportResource)item.Clone());
		}
		return stiReportResourceCollection;
	}
}
