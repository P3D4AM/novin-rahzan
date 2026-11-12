using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartTrendLine : IStiJsonReportObject, ICloneable, IStiChartTrendLine
{
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[Description("Gets or sets type trend line.")]
	public StiChartTrendLineType Type { get; set; }

	[StiSerializable]
	[DefaultValue(StiPenStyle.Solid)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets trend line style.")]
	public StiPenStyle LineStyle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets color which will be used for drawing trend line.")]
	public virtual Color LineColor { get; set; } = Color.Black;


	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets trend line width.")]
	public float LineWidth { get; set; } = 1f;


	[DefaultValue("")]
	[StiSerializable]
	public string KeyValueMeter { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("KeyValueMeter", KeyValueMeter);
		jObject.AddPropertyEnum("Type", Type, StiChartTrendLineType.None);
		jObject.AddPropertyEnum("LineStyle", LineStyle, StiPenStyle.Solid);
		jObject.AddPropertyColor("LineColor", LineColor, Color.Black);
		jObject.AddPropertyFloat("LineWidth", LineWidth, 1f);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeyValueMeter":
				KeyValueMeter = item.DeserializeString();
				break;
			case "Type":
				Type = item.DeserializeEnum<StiChartTrendLineType>();
				break;
			case "LineStyle":
				LineStyle = item.DeserializeEnum<StiPenStyle>();
				break;
			case "LineColor":
				LineColor = item.DeserializeColor();
				break;
			case "LineWidth":
				LineWidth = item.DeserializeFloat();
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public static StiChartTrendLine LoadFromJson(JObject json)
	{
		StiChartTrendLine stiChartTrendLine = new StiChartTrendLine();
		stiChartTrendLine.LoadFromJsonObject(json);
		return stiChartTrendLine;
	}

	public StiChartTrendLine()
	{
	}

	[StiUniversalConstructor("TrendLine")]
	public StiChartTrendLine(string keyValueMeter, StiChartTrendLineType type, Color lineColor, StiPenStyle lineStyle, float lineWidth)
	{
		KeyValueMeter = keyValueMeter;
		Type = type;
		LineColor = lineColor;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
	}
}
