using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiTrendLinesCollectionConverter))]
public class StiTrendLinesCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiTrendLine this[int index]
	{
		get
		{
			return (StiTrendLine)base.List[index];
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
		foreach (StiTrendLine item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		Clear();
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiTrendLine stiTrendLine = StiOptions.Services.ChartTrendLines.FirstOrDefault((StiTrendLine x) => ((object)x).GetType().Name == ident);
			if (stiTrendLine == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiTrendLine stiTrendLine2 = stiTrendLine.CreateNew();
			stiTrendLine2.LoadFromJsonObject((JObject)item.Value);
			Add(stiTrendLine2);
		}
	}

	public object Clone()
	{
		StiTrendLinesCollection stiTrendLinesCollection = new StiTrendLinesCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiTrendLine stiTrendLine = (StiTrendLine)enumerator.Current;
				stiTrendLinesCollection.Add(((StiService)stiTrendLine).Clone() as StiTrendLine);
			}
			return stiTrendLinesCollection;
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

	public void Add(StiTrendLine filter)
	{
		base.List.Add(filter);
	}

	public void AddRange(StiTrendLinesCollection lines)
	{
		foreach (StiTrendLine line in lines)
		{
			Add(line);
		}
	}

	public void AddRange(StiTrendLine[] lines)
	{
		foreach (StiTrendLine filter in lines)
		{
			Add(filter);
		}
	}

	public void AddRange(List<StiTrendLine> lines)
	{
		foreach (StiTrendLine line in lines)
		{
			Add(line);
		}
	}

	public bool Contains(StiTrendLine line)
	{
		return base.List.Contains(line);
	}

	public int IndexOf(StiTrendLine line)
	{
		return base.List.IndexOf(line);
	}

	public void Insert(int index, StiTrendLine line)
	{
		base.List.Insert(index, line);
	}

	public void Remove(StiTrendLine line)
	{
		base.List.Remove(line);
	}

	public List<StiTrendLine> ToList()
	{
		List<StiTrendLine> list = new List<StiTrendLine>();
		foreach (StiTrendLine item in base.List)
		{
			list.Add(item);
		}
		return list;
	}
}
