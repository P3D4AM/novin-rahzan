using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiHeatmapStyleData : IStiJsonReportObject, ICloneable
{
	public enum Order
	{
		Color = 1,
		ZeroColor,
		Mode
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiColorConverter))]
	[StiOrder(1)]
	public Color Color { get; set; } = ColorTranslator.FromHtml("#70ad47");


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiColorConverter))]
	[StiOrder(2)]
	public Color ZeroColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[DefaultValue(StiHeatmapFillMode.Lightness)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiOrder(3)]
	public StiHeatmapFillMode Mode { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyColor(val, "Color", Color, "#70ad47");
		JsonExtensions.AddPropertyColor(val, "ZeroColor", ZeroColor, Color.Transparent);
		JsonExtensions.AddPropertyEnum(val, "Mode", (Enum)Mode, (Enum)StiHeatmapFillMode.Lightness);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ZeroColor":
				ZeroColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Mode":
				Mode = StiJsonReportObjectHelper.DeserializeEnum<StiHeatmapFillMode>(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	private bool ShouldSerializeColor()
	{
		return Color != ColorTranslator.FromHtml("#70ad47");
	}

	private bool ShouldSerializeZeroColor()
	{
		return ZeroColor != Color.Transparent;
	}

	private bool ShouldSerializeMode()
	{
		return Mode != StiHeatmapFillMode.Lightness;
	}

	public virtual bool IsDefault()
	{
		if (Color == ColorTranslator.FromHtml("#70ad47") && ZeroColor == Color.Transparent)
		{
			return Mode == StiHeatmapFillMode.Lightness;
		}
		return false;
	}

	public StiHeatmapStyleData()
	{
	}

	[StiUniversalConstructor("Heatmap")]
	public StiHeatmapStyleData(Color color, Color zeroColor, StiHeatmapFillMode mode)
	{
		Color = color;
		ZeroColor = zeroColor;
		Mode = mode;
	}
}
