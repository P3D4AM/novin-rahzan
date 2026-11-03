using System;
using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiSerialConverter))]
public class StiStripsCollection : CollectionBase, IStiApplyStyle, IStiJsonReportObject
{
	public IStiStrips this[int index]
	{
		get
		{
			return (IStiStrips)base.InnerList[index];
		}
		set
		{
			base.InnerList[index] = value;
		}
	}

	[Browsable(false)]
	public StiChart Chart { get; set; }

	public event EventHandler StripsAdded;

	public event EventHandler StripsRemoved;

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
		foreach (IStiStrips item in base.List)
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
			StiStrips stiStrips = new StiStrips();
			stiStrips.Chart = Chart;
			Add(stiStrips);
			stiStrips.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public void ApplyStyle(IStiChartStyle style)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IStiStrips stiStrips = (IStiStrips)enumerator.Current;
				stiStrips.Core.ApplyStyle(style);
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

	private string GetStripsTitle()
	{
		string text = StiLocalization.Get("Chart", "Strip");
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
						IStiStrips stiStrips = (IStiStrips)enumerator.Current;
						if (stiStrips.Text == text2)
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

	private void AddCore(IStiStrips value)
	{
		if (Chart != null)
		{
			if (string.IsNullOrEmpty(value.Text))
			{
				value.Text = GetStripsTitle();
			}
			value.Chart = Chart;
		}
		base.List.Add(value);
	}

	public void Add(IStiStrips value)
	{
		AddCore(value);
		InvokeStripsAdded(value, base.List.Count - 1);
	}

	public void AddRange(IStiStrips[] values)
	{
		foreach (IStiStrips stiStrips in values)
		{
			AddCore(stiStrips);
			InvokeStripsAdded(stiStrips, base.List.Count - 1);
		}
	}

	public void AddRange(StiStripsCollection values)
	{
		foreach (IStiStrips value in values)
		{
			AddCore(value);
			InvokeStripsAdded(value, base.List.Count - 1);
		}
	}

	public bool Contains(IStiStrips value)
	{
		return base.List.Contains(value);
	}

	public int IndexOf(IStiStrips value)
	{
		return base.List.IndexOf(value);
	}

	public void Insert(int index, IStiStrips value)
	{
		base.List.Insert(index, value);
		InvokeStripsAdded(value, index);
	}

	protected override void OnClear()
	{
		while (base.List.Count > 0)
		{
			InvokeStripsRemoved(base.List[0], 0);
			base.List.RemoveAt(0);
		}
	}

	public void Remove(IStiStrips value)
	{
		InvokeStripsRemoved(value, base.List.IndexOf(value));
		base.List.Remove(value);
	}

	protected virtual void OnStripsAdded(EventArgs e)
	{
	}

	private void InvokeStripsAdded(object sender, int index)
	{
		this.StripsAdded?.Invoke(sender, EventArgs.Empty);
	}

	protected virtual void OnStripsRemoved(EventArgs e)
	{
	}

	private void InvokeStripsRemoved(object sender, int index)
	{
		this.StripsRemoved?.Invoke(sender, EventArgs.Empty);
	}

	[StiUniversalConstructor("Strips")]
	public StiStripsCollection()
	{
	}
}
