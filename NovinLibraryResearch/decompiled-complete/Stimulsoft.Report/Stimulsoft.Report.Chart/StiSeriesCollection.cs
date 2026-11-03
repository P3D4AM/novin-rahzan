using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiSerialConverter))]
public class StiSeriesCollection : CollectionBase, IStiApplyStyle, IStiJsonReportObject
{
	public IStiSeries this[int index]
	{
		get
		{
			return (IStiSeries)base.InnerList[index];
		}
		set
		{
			base.InnerList[index] = value;
		}
	}

	public IStiSeries this[string name]
	{
		get
		{
			foreach (IStiSeries item in base.List)
			{
				if (item.CoreTitle == name)
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
				IStiSeries stiSeries = base.List[i] as IStiSeries;
				if (stiSeries.CoreTitle == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	[Browsable(false)]
	public StiChart Chart { get; set; }

	public event EventHandler SeriesAdded;

	public event EventHandler SeriesRemoved;

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
		foreach (IStiSeries item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), ((IStiJsonReportObject)item).SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		Clear();
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiSeries stiSeries = StiOptions.Services.ChartSeries.FirstOrDefault((StiSeries x) => ((object)x).GetType().Name == ident);
			if (stiSeries == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiSeries stiSeries2 = stiSeries.CreateNew();
			stiSeries2.Chart = Chart;
			Add(stiSeries2);
			stiSeries2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (style == null)
		{
			return;
		}
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IStiSeries stiSeries = (IStiSeries)enumerator.Current;
				Color[] colors = style.Core.GetColors(base.Count, stiSeries?.Core?.SeriesColors);
				stiSeries.Core?.ApplyStyle(style, colors[num++]);
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
	}

	public StiSeries FirstOrDefault()
	{
		return ToList().FirstOrDefault();
	}

	public List<StiSeries> ToList()
	{
		return this.Cast<StiSeries>().ToList();
	}

	private string GetSeriesTitle()
	{
		string text = StiLocalization.Get("Chart", "Series");
		string text2 = text;
		int num = 1;
		bool flag = true;
		while (flag)
		{
			text2 = text + " " + num;
			flag = false;
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IStiSeries stiSeries = (IStiSeries)enumerator.Current;
						if (stiSeries.CoreTitle == text2)
						{
							flag = true;
							break;
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
			}
			num++;
		}
		return text2;
	}

	private void AddCore(IStiSeries value)
	{
		if (Chart != null)
		{
			if (string.IsNullOrEmpty(value.CoreTitle))
			{
				value.CoreTitle = GetSeriesTitle();
			}
			value.Chart = Chart;
		}
		base.List.Add(value);
	}

	public void Add(IStiSeries value)
	{
		AddCore(value);
		InvokeSeriesAdded(value);
	}

	public void AddRange(IStiSeries[] values)
	{
		foreach (IStiSeries stiSeries in values)
		{
			AddCore(stiSeries);
			InvokeSeriesAdded(stiSeries);
		}
	}

	public void AddRange(StiSeriesCollection values)
	{
		foreach (IStiSeries value in values)
		{
			AddCore(value);
			InvokeSeriesAdded(value);
		}
	}

	public bool Contains(IStiSeries value)
	{
		return base.List.Contains(value);
	}

	public int IndexOf(IStiSeries value)
	{
		return base.List.IndexOf(value);
	}

	public void Insert(int index, IStiSeries value)
	{
		if (Chart != null)
		{
			if (string.IsNullOrEmpty(value.CoreTitle))
			{
				value.CoreTitle = GetSeriesTitle();
			}
			value.Chart = Chart;
		}
		base.List.Insert(index, value);
		InvokeSeriesAdded(value);
	}

	protected override void OnClear()
	{
		while (base.List.Count > 0)
		{
			base.List.RemoveAt(0);
		}
	}

	public void Remove(IStiSeries value)
	{
		base.List.Remove(value);
	}

	protected virtual void OnSeriesAdded(EventArgs e)
	{
	}

	private void InvokeSeriesAdded(object sender)
	{
		this.SeriesAdded?.Invoke(sender, EventArgs.Empty);
	}

	protected override void OnRemove(int index, object value)
	{
		base.OnRemove(index, value);
		InvokeSeriesRemoved(value);
	}

	protected virtual void OnSeriesRemoved(EventArgs e)
	{
	}

	private void InvokeSeriesRemoved(object sender)
	{
		this.SeriesRemoved?.Invoke(sender, EventArgs.Empty);
	}

	[StiUniversalConstructor("Series")]
	public StiSeriesCollection()
	{
	}
}
