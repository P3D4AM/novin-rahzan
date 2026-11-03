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

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiWaterfallConnectorLine : IStiWaterfallConnectorLine, ICloneable, IStiJsonReportObject, IStiSerializeToCodeAsClass
{
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color LineColor { get; set; } = Color.LightGray;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StiPenStyle LineStyle { get; set; }

	[DefaultValue(1f)]
	[StiSerializable]
	public float LineWidth { get; set; } = 1f;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Visible { get; set; } = true;


	public object Clone()
	{
		return MemberwiseClone();
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.LightGray);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 1f);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "ShowShadow", Visible, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public StiWaterfallConnectorLine()
	{
	}

	[StiUniversalConstructor("Line")]
	public StiWaterfallConnectorLine(Color lineColor, StiPenStyle lineStyle, float lineWidth, bool visible)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		LineColor = lineColor;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
		Visible = visible;
	}
}
