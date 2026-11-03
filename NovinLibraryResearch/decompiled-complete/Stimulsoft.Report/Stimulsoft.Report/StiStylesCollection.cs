using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report;

public class StiStylesCollection : CollectionBase, IStiJsonReportObject
{
	private StiReport report;

	private Hashtable hash = new Hashtable();

	private bool needUpdateHash = true;

	private int lastCount = -1;

	public StiBaseStyle this[int index]
	{
		get
		{
			return (StiBaseStyle)base.List[index];
		}
		set
		{
			base.List[index] = value;
			needUpdateHash = true;
		}
	}

	public StiBaseStyle this[string name]
	{
		get
		{
			UpdateHash();
			if (hash[name.ToLowerInvariant()] != null)
			{
				return hash[name.ToLowerInvariant()] as StiBaseStyle;
			}
			foreach (StiBaseStyle item in base.List)
			{
				if (item.Name.ToLowerInvariant() == name.ToLowerInvariant())
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
				if (((StiBaseStyle)base.List[i]).Name.ToLowerInvariant() == name.ToLowerInvariant())
				{
					base.List[i] = value;
					needUpdateHash = true;
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
		foreach (StiBaseStyle item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public static StiBaseStyle LoadStyleFromJObject(JObject styleJObject)
	{
		string text = styleJObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiBaseStyle stiBaseStyle = null;
		switch (text)
		{
		case "StiChartStyle":
			stiBaseStyle = new StiChartStyle();
			break;
		case "StiGaugeStyle":
			stiBaseStyle = new StiGaugeStyle();
			break;
		case "StiIndicatorStyle":
			stiBaseStyle = new StiIndicatorStyle();
			break;
		case "StiProgressStyle":
			stiBaseStyle = new StiProgressStyle();
			break;
		case "StiCrossTabStyle":
			stiBaseStyle = new StiCrossTabStyle();
			break;
		case "StiDialogStyle":
			stiBaseStyle = new StiDialogStyle();
			break;
		case "StiMapStyle":
			stiBaseStyle = new StiMapStyle();
			break;
		case "StiTableStyle":
			stiBaseStyle = new StiTableStyle();
			break;
		case "StiStyle":
			stiBaseStyle = new StiStyle();
			break;
		}
		if (stiBaseStyle == null)
		{
			stiBaseStyle = StiOptions.Designer.StyleDesigner.InvokeLoadCustomStyle(text);
			if (stiBaseStyle == null)
			{
				throw new Exception($"Type '{text}' is not supported!");
			}
		}
		stiBaseStyle.LoadFromJsonObject(styleJObject);
		return stiBaseStyle;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			JObject styleJObject = (JObject)item.Value;
			StiBaseStyle value = LoadStyleFromJObject(styleJObject);
			base.List.Add(value);
		}
	}

	public void Add(StiBaseStyle style)
	{
		base.List.Add(style);
		hash[style.Name.ToLowerInvariant()] = style;
		style.Report = report;
	}

	protected override void OnClear()
	{
		base.OnClear();
		hash.Clear();
		needUpdateHash = true;
	}

	public void AddRange(StiStylesCollection styles)
	{
		foreach (StiBaseStyle style in styles)
		{
			Add(style);
		}
	}

	public void AddRange(StiBaseStyle[] styles)
	{
		foreach (StiBaseStyle style in styles)
		{
			Add(style);
		}
	}

	public void AddRange(List<StiBaseStyle> styles)
	{
		foreach (StiBaseStyle style in styles)
		{
			Add(style);
		}
	}

	public bool Contains(StiBaseStyle style)
	{
		UpdateHash();
		return hash[style.Name.ToLowerInvariant()] != null;
	}

	public bool Contains(string styleName)
	{
		UpdateHash();
		return hash[styleName.ToLowerInvariant()] != null;
	}

	public int IndexOf(StiBaseStyle style)
	{
		return base.List.IndexOf(style);
	}

	public void Insert(int index, StiBaseStyle style)
	{
		base.List.Insert(index, style);
		hash[style.Name.ToLowerInvariant()] = style;
	}

	public void Remove(StiBaseStyle style)
	{
		base.List.Remove(style);
		hash.Remove(style.Name.ToLowerInvariant());
	}

	public void Load(Stream stream)
	{
		StiStylesSheet stiStylesSheet = new StiStylesSheet(this);
		stiStylesSheet.Load(stream);
		needUpdateHash = true;
		report?.ApplyStyles();
	}

	public void Load(string file)
	{
		StiStylesSheet stiStylesSheet = new StiStylesSheet(this);
		stiStylesSheet.Load(file);
		needUpdateHash = true;
		report?.ApplyStyles();
	}

	public void Save(Stream stream)
	{
		StiStylesSheet stiStylesSheet = new StiStylesSheet(this);
		stiStylesSheet.Save(stream);
	}

	public void Save(string file)
	{
		StiStylesSheet stiStylesSheet = new StiStylesSheet(this);
		stiStylesSheet.Save(file);
	}

	private void UpdateHash()
	{
		if (lastCount != base.Count)
		{
			lastCount = base.Count;
			needUpdateHash = true;
		}
		if (!needUpdateHash)
		{
			return;
		}
		hash.Clear();
		foreach (StiBaseStyle item in base.List)
		{
			hash[item.Name.ToLower(CultureInfo.InvariantCulture)] = item;
		}
		needUpdateHash = false;
	}

	public List<StiBaseStyle> ToList()
	{
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		foreach (StiBaseStyle item in base.List)
		{
			list.Add(item);
		}
		return list;
	}

	public StiCustomStyle GetCustomChartStyle(string customStyleName)
	{
		StiBaseStyle stiBaseStyle = ((!string.IsNullOrWhiteSpace(customStyleName)) ? this[customStyleName] : null);
		if (stiBaseStyle == null)
		{
			return null;
		}
		return new StiCustomStyle(customStyleName);
	}

	public StiCustomGaugeStyle GetCustomGaugeStyle(string customStyleName)
	{
		StiGaugeStyle stiGaugeStyle = ((!string.IsNullOrWhiteSpace(customStyleName)) ? (ToList().FirstOrDefault((StiBaseStyle x) => x.Name == customStyleName) as StiGaugeStyle) : null);
		if (stiGaugeStyle == null)
		{
			return null;
		}
		return new StiCustomGaugeStyle(stiGaugeStyle);
	}

	public StiStylesCollection()
		: this(null)
	{
		needUpdateHash = true;
	}

	public StiStylesCollection(StiReport report)
	{
		this.report = report;
		needUpdateHash = true;
	}
}
