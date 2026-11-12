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
public class StiChartConstantLines : IStiJsonReportObject, ICloneable, IStiChartConstantLines
{
	[StiSerializable]
	[DefaultValue(StiTextPosition.LeftTop)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets text position at constant line.")]
	public StiTextPosition Position { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets constant line text.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[DefaultValue(StiPenStyle.Solid)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets constant line style.")]
	public StiPenStyle LineStyle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for drawing constant line.")]
	public virtual Color LineColor { get; set; } = Color.Black;


	[DefaultValue("1")]
	[StiSerializable]
	[Description("Gets or sets Y axis value through what the constant line is drawn.")]
	public virtual string AxisValue { get; set; } = "1";


	[DefaultValue(1f)]
	[StiSerializable]
	[Description("Gets or sets constant line width.")]
	public float LineWidth { get; set; } = 1f;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyEnum("LineStyle", LineStyle, StiPenStyle.Solid);
		jObject.AddPropertyColor("LineColor", LineColor, Color.Black);
		jObject.AddPropertyFloat("LineWidth", LineWidth, 1f);
		jObject.AddPropertyString("AxisValue", AxisValue, "1");
		jObject.AddPropertyEnum("Position", Position, StiTextPosition.LeftTop);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Position":
				Position = item.DeserializeEnum<StiTextPosition>();
				break;
			case "Text":
				Text = item.DeserializeString();
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
			case "AxisValue":
				AxisValue = item.DeserializeString();
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	private bool ShouldSerializeLineColor()
	{
		return LineColor != Color.Black;
	}

	public static StiChartConstantLines LoadFromJson(JObject json)
	{
		StiChartConstantLines stiChartConstantLines = new StiChartConstantLines();
		stiChartConstantLines.LoadFromJsonObject(json);
		return stiChartConstantLines;
	}

	public StiChartConstantLines()
	{
	}

	public StiChartConstantLines(string text, string axisValue, Color lineColor, StiPenStyle lineStyle, float lineWidth)
	{
		Text = text;
		AxisValue = axisValue;
		LineColor = lineColor;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
	}

	[StiUniversalConstructor("ConstantLines")]
	public StiChartConstantLines(string text, string axisValue, Color lineColor, StiPenStyle lineStyle, float lineWidth, StiTextPosition position)
	{
		Text = text;
		AxisValue = axisValue;
		LineColor = lineColor;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
		Position = position;
	}
}
