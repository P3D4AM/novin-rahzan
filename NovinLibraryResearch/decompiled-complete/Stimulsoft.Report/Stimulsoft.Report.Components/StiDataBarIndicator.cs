using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiGdiIndicatorTypePainter(typeof(StiDataBarGdiIndicatorTypePainter))]
[StiWpfIndicatorTypePainter("Stimulsoft.Report.Painters.StiDataBarWpfIndicatorTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDataBarIndicator : StiIndicator, IStiDataBarIndicator
{
	[StiSerializable]
	[DefaultValue(StiBrushType.Gradient)]
	public StiBrushType BrushType { get; set; } = StiBrushType.Gradient;


	[StiSerializable]
	public Color PositiveColor { get; set; } = Color.Green;


	[StiSerializable]
	public Color NegativeColor { get; set; } = Color.Red;


	[StiSerializable]
	public Color PositiveBorderColor { get; set; } = Color.DarkGreen;


	[StiSerializable]
	public Color NegativeBorderColor { get; set; } = Color.DarkRed;


	[StiSerializable]
	[DefaultValue(false)]
	public bool ShowBorder { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	public float Value { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	public float Minimum { get; set; }

	[StiSerializable]
	[DefaultValue(100f)]
	public float Maximum { get; set; } = 100f;


	[StiSerializable]
	[DefaultValue(StiDataBarDirection.Default)]
	public StiDataBarDirection Direction { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "BrushType", (Enum)BrushType, (Enum)StiBrushType.Gradient);
		JsonExtensions.AddPropertyColor(val, "PositiveColor", PositiveColor, Color.Green);
		JsonExtensions.AddPropertyColor(val, "NegativeColor", NegativeColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "PositiveBorderColor", PositiveBorderColor, Color.DarkGreen);
		JsonExtensions.AddPropertyColor(val, "NegativeBorderColor", NegativeBorderColor, Color.DarkRed);
		JsonExtensions.AddPropertyBool(val, "ShowBorder", ShowBorder, false, false);
		JsonExtensions.AddPropertyFloat(val, "Value", Value, 0f);
		JsonExtensions.AddPropertyFloat(val, "Minimum", Minimum, 0f);
		JsonExtensions.AddPropertyFloat(val, "Maximum", Maximum, 100f);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiDataBarDirection.Default);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BrushType":
				BrushType = StiJsonReportObjectHelper.DeserializeEnum<StiBrushType>(item);
				break;
			case "PositiveColor":
				PositiveColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeColor":
				NegativeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "PositiveBorderColor":
				PositiveBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeBorderColor":
				NegativeBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ShowBorder":
				ShowBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Value":
				Value = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Minimum":
				Minimum = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Maximum":
				Maximum = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Direction":
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiDataBarDirection>(item);
				break;
			}
		}
	}

	public StiDataBarIndicator()
	{
	}

	public StiDataBarIndicator(StiBrushType brushType, Color positiveColor, Color negativeColor, bool showBorder, Color positiveBorderColor, Color negativeBorderColor, StiDataBarDirection direction, float value, float minimum, float maximum)
	{
		BrushType = brushType;
		PositiveColor = positiveColor;
		NegativeColor = negativeColor;
		ShowBorder = showBorder;
		PositiveBorderColor = positiveBorderColor;
		NegativeBorderColor = negativeBorderColor;
		Direction = direction;
		Value = value;
		Minimum = minimum;
		Maximum = maximum;
	}
}
