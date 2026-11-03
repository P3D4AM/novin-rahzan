using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiSerialConverter))]
public class StiConstantLinesCollection : CollectionBase, IStiApplyStyle, IStiJsonReportObject
{
	public IStiConstantLines this[int index]
	{
		get
		{
			return (IStiConstantLines)base.InnerList[index];
		}
		set
		{
			base.InnerList[index] = value;
		}
	}

	[Browsable(false)]
	public StiChart Chart { get; set; }

	public event EventHandler ConstantLinesAdded;

	public event EventHandler ConstantLinesRemoved;

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
		foreach (IStiConstantLines item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), ((IStiJsonReportObject)item).SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiConstantLines stiConstantLines = new StiConstantLines();
			stiConstantLines.Chart = Chart;
			Add(stiConstantLines);
			stiConstantLines.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public void ApplyStyle(IStiChartStyle style)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IStiConstantLines stiConstantLines = (IStiConstantLines)enumerator.Current;
				stiConstantLines.Core.ApplyStyle(style);
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

	public List<StiConstantLines> ToList()
	{
		return this.Cast<StiConstantLines>().ToList();
	}

	private string GetConstantLineTitle()
	{
		string text = StiLocalization.Get("Chart", "ConstantLine");
		string text2 = text;
		int num = 1;
		bool flag = true;
		while (flag)
		{
			text2 = $"{text} {num}";
			flag = false;
			{
				IEnumerator enumerator = GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IStiConstantLines stiConstantLines = (IStiConstantLines)enumerator.Current;
						if (stiConstantLines.Text == text2)
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

	private void AddCore(IStiConstantLines value)
	{
		if (Chart != null)
		{
			if (string.IsNullOrEmpty(value.Text))
			{
				value.Text = GetConstantLineTitle();
			}
			value.Chart = Chart;
		}
		base.List.Add(value);
	}

	public void Add(IStiConstantLines value)
	{
		AddCore(value);
		InvokeConstantLinesAdded(value, base.List.Count - 1);
	}

	public void AddRange(IStiConstantLines[] values)
	{
		foreach (IStiConstantLines stiConstantLines in values)
		{
			AddCore(stiConstantLines);
			InvokeConstantLinesAdded(stiConstantLines, base.List.Count - 1);
		}
	}

	public void AddRange(StiConstantLinesCollection values)
	{
		foreach (IStiConstantLines value in values)
		{
			AddCore(value);
			InvokeConstantLinesAdded(value, base.List.Count - 1);
		}
	}

	public bool Contains(IStiConstantLines value)
	{
		return base.List.Contains(value);
	}

	public int IndexOf(IStiConstantLines value)
	{
		return base.List.IndexOf(value);
	}

	public void Insert(int index, IStiConstantLines value)
	{
		base.List.Insert(index, value);
		InvokeConstantLinesAdded(value, index);
	}

	protected override void OnClear()
	{
		while (base.List.Count > 0)
		{
			InvokeConstantLinesRemoved(base.List[0], 0);
			base.List.RemoveAt(0);
		}
	}

	public void Remove(IStiConstantLines value)
	{
		InvokeConstantLinesRemoved(value, base.List.IndexOf(value));
		base.List.Remove(value);
	}

	protected virtual void OnConstantLinesAdded(EventArgs e)
	{
	}

	private void InvokeConstantLinesAdded(object sender, int index)
	{
		this.ConstantLinesAdded?.Invoke(sender, EventArgs.Empty);
	}

	protected virtual void OnConstantLinesRemoved(EventArgs e)
	{
	}

	private void InvokeConstantLinesRemoved(object sender, int index)
	{
		this.ConstantLinesRemoved?.Invoke(sender, EventArgs.Empty);
	}

	[StiUniversalConstructor("ConstantLines")]
	public StiConstantLinesCollection()
	{
	}

	public StiConstantLinesCollection(List<StiConstantLines> lines)
	{
		IStiConstantLines[] values = lines.ToArray();
		AddRange(values);
	}
}
