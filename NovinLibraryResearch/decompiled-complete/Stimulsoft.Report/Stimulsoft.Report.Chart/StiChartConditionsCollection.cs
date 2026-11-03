using System;
using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiChartConditionsCollectionConverter))]
public class StiChartConditionsCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiChartCondition this[int index]
	{
		get
		{
			return (StiChartCondition)base.List[index];
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
		foreach (StiChartCondition item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		base.List.Clear();
		foreach (JProperty item in jObject.Properties())
		{
			StiChartCondition stiChartCondition = new StiChartCondition();
			stiChartCondition.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiChartCondition);
		}
	}

	public object Clone()
	{
		StiChartConditionsCollection stiChartConditionsCollection = new StiChartConditionsCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiChartCondition stiChartCondition = (StiChartCondition)enumerator.Current;
				stiChartConditionsCollection.Add(stiChartCondition.Clone() as StiChartCondition);
			}
			return stiChartConditionsCollection;
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
		if (((StiChartCondition)value).Conditions == null)
		{
			((StiChartCondition)value).Conditions = this;
		}
	}

	public void Add(StiChartCondition condition)
	{
		base.List.Add(condition);
	}

	public void AddRange(StiChartConditionsCollection conditions)
	{
		foreach (StiChartCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public void AddRange(StiChartCondition[] conditions)
	{
		foreach (StiChartCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public bool Contains(StiChartCondition condition)
	{
		return base.List.Contains(condition);
	}

	public int IndexOf(StiChartCondition condition)
	{
		return base.List.IndexOf(condition);
	}

	public void Insert(int index, StiChartCondition condition)
	{
		base.List.Insert(index, condition);
	}

	public void Remove(StiChartCondition condition)
	{
		base.List.Remove(condition);
	}
}
