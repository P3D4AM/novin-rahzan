using System;
using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiChartFiltersCollectionConverter))]
public class StiChartFiltersCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiChartFilter this[int index]
	{
		get
		{
			return (StiChartFilter)base.List[index];
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
		foreach (StiChartFilter item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiChartFilter stiChartFilter = new StiChartFilter();
			Add(stiChartFilter);
			stiChartFilter.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiChartFiltersCollection stiChartFiltersCollection = new StiChartFiltersCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiChartFilter stiChartFilter = (StiChartFilter)enumerator.Current;
				stiChartFiltersCollection.Add(stiChartFilter.Clone() as StiChartFilter);
			}
			return stiChartFiltersCollection;
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

	protected override void OnInsert(int index, object value)
	{
		if (((StiChartFilter)value).Filters == null)
		{
			((StiChartFilter)value).Filters = this;
		}
	}

	public void Add(StiChartFilter filter)
	{
		base.List.Add(filter);
	}

	public void AddRange(StiChartFiltersCollection filters)
	{
		foreach (StiChartFilter filter in filters)
		{
			Add(filter);
		}
	}

	public void AddRange(StiChartFilter[] filters)
	{
		foreach (StiChartFilter filter in filters)
		{
			Add(filter);
		}
	}

	public bool Contains(StiChartFilter filter)
	{
		return base.List.Contains(filter);
	}

	public int IndexOf(StiChartFilter filter)
	{
		return base.List.IndexOf(filter);
	}

	public void Insert(int index, StiChartFilter filter)
	{
		base.List.Insert(index, filter);
	}

	public void Remove(StiChartFilter filter)
	{
		base.List.Remove(filter);
	}
}
