using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.HeatmapGradient.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.OnlineMap;

[JsonObject]
[TypeConverter(typeof(StiHeatmapGradientRangeConverter))]
public class StiHeatmapGradientRange : IStiJsonReportObject, IStiHeatmapGradientRange, ICloneable
{
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public Color Color { get; set; } = Color.Red;


	[DefaultValue(0.0)]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public double Start
	{
		get
		{
			return StiValueHelper.TryToDouble(StartExpression);
		}
		set
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				StartExpression = value.ToString();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
	}

	[DefaultValue("0")]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public string StartExpression { get; set; } = "0";


	public object Clone()
	{
		return new StiHeatmapGradientRange
		{
			Start = Start,
			StartExpression = StartExpression,
			Color = Color.FromArgb(Color.ToArgb())
		};
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyColor("Color", Color, Color.Red);
		jObject.AddPropertyStringNullOrEmpty("StartExpresson", StartExpression);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "Start":
				Start = item.DeserializeDouble();
				break;
			case "StartExpression":
				StartExpression = item.DeserializeString();
				break;
			}
		}
	}

	public int GetUniqueCode()
	{
		int num = (int)Start;
		num = (num * 397) ^ ((StartExpression == null) ? 1 : StartExpression.GetHashCode());
		int num2 = num * 397;
		_ = Color;
		return num2 ^ Color.GetHashCode();
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Red;
	}

	public static StiHeatmapGradientRange LoadFromJson(string json)
	{
		StiHeatmapGradientRange stiHeatmapGradientRange = new StiHeatmapGradientRange();
		stiHeatmapGradientRange.LoadFromJsonObject(JObject.Parse(json));
		return stiHeatmapGradientRange;
	}

	public static StiHeatmapGradientRange LoadFromJson(JObject json)
	{
		StiHeatmapGradientRange stiHeatmapGradientRange = new StiHeatmapGradientRange();
		stiHeatmapGradientRange.LoadFromJsonObject(json);
		return stiHeatmapGradientRange;
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public string GetStringRepresentation()
	{
		return StartExpression + "%";
	}

	public StiHeatmapGradientRange()
	{
	}

	public StiHeatmapGradientRange(double start, Color color)
	{
		Color = color;
		Start = start;
	}

	public StiHeatmapGradientRange(Color color, string startExpression)
	{
		Color = color;
		StartExpression = startExpression;
	}
}
