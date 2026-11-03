using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Components;

public class StiFiltersCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiFilter this[int index]
	{
		get
		{
			return (StiFilter)base.List[index];
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
		foreach (StiFilter item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiFilter stiFilter = null;
			JObject val = (JObject)item.Value;
			switch (val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>())
			{
			case "StiFilter":
				stiFilter = new StiFilter();
				break;
			case "StiBaseCondition":
				stiFilter = new StiBaseCondition();
				break;
			case "StiConditionHelper":
				stiFilter = new StiConditionHelper();
				break;
			}
			stiFilter.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiFilter);
		}
	}

	public object Clone()
	{
		StiFiltersCollection stiFiltersCollection = new StiFiltersCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiFilter stiFilter = (StiFilter)enumerator.Current;
				stiFiltersCollection.Add(stiFilter.Clone() as StiFilter);
			}
			return stiFiltersCollection;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public List<StiFilter> ToList()
	{
		return base.List.Cast<StiFilter>().ToList();
	}

	public void Add(StiFilter filter)
	{
		base.List.Add(filter);
	}

	public void AddRange(StiFiltersCollection filters)
	{
		foreach (StiFilter filter in filters)
		{
			Add(filter);
		}
	}

	public void AddRange(StiFilter[] filters)
	{
		foreach (StiFilter filter in filters)
		{
			Add(filter);
		}
	}

	public bool Contains(StiFilter filter)
	{
		return base.List.Contains(filter);
	}

	public int IndexOf(StiFilter filter)
	{
		return base.List.IndexOf(filter);
	}

	public void Insert(int index, StiFilter filter)
	{
		base.List.Insert(index, filter);
	}

	public void Remove(StiFilter filter)
	{
		base.List.Remove(filter);
	}
}
