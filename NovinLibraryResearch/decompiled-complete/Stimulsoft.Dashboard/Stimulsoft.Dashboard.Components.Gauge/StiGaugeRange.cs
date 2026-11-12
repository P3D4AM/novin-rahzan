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
using Stimulsoft.Dashboard.Components.Gauge.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Gauge;

[JsonObject]
[TypeConverter(typeof(StiGaugeRangeConverter))]
public class StiGaugeRange : IStiJsonReportObject, IStiGaugeRange, ICloneable
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

	[DefaultValue(100.0)]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public double End
	{
		get
		{
			return StiValueHelper.TryToDouble(EndExpression);
		}
		set
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				EndExpression = value.ToString();
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


	[DefaultValue("100")]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public string EndExpression { get; set; } = "100";


	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyColor("Color", Color, Color.Red);
		jObject.AddPropertyStringNullOrEmpty("StartExpression", StartExpression);
		jObject.AddPropertyStringNullOrEmpty("EndExpression", EndExpression);
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
			case "End":
				End = item.DeserializeDouble();
				break;
			case "StartExpression":
				StartExpression = item.DeserializeString();
				break;
			case "EndExpression":
				EndExpression = item.DeserializeString();
				break;
			}
		}
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Red;
	}

	public static StiGaugeRange LoadFromJson(string json)
	{
		StiGaugeRange stiGaugeRange = new StiGaugeRange();
		stiGaugeRange.LoadFromJsonObject(JObject.Parse(json));
		return stiGaugeRange;
	}

	public static StiGaugeRange LoadFromJson(JObject json)
	{
		StiGaugeRange stiGaugeRange = new StiGaugeRange();
		stiGaugeRange.LoadFromJsonObject(json);
		return stiGaugeRange;
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public string GetStringRepresentation()
	{
		return StartExpression + " - " + EndExpression;
	}

	public StiGaugeRange()
	{
	}

	public StiGaugeRange(Color color, double start, double end)
	{
		Color = color;
		Start = start;
		End = end;
	}

	public StiGaugeRange(Color color, string startExpression, string endExpression)
	{
		Color = color;
		StartExpression = startExpression;
		EndExpression = endExpression;
	}
}
