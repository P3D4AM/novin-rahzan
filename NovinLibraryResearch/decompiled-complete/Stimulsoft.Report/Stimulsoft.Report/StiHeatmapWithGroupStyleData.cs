using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiHeatmapWithGroupStyleData : IStiJsonReportObject, ICloneable
{
	public enum Order
	{
		Colors = 1,
		ZeroColor,
		Mode
	}

	private Color[] DefaultColors = new Color[2]
	{
		ColorTranslator.FromHtml("#70ad47"),
		ColorTranslator.FromHtml("#ffc000")
	};

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiStyleColorsConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiColorsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(1)]
	public Color[] Colors { get; set; }

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
		JsonExtensions.AddPropertyArrayColor(val, "Colors", Colors);
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
			case "Colors":
				Colors = StiJsonReportObjectHelper.DeserializeArrayColor(item);
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
		if (Colors != null && Colors.Length == DefaultColors.Length)
		{
			return !Colors.ToList().SequenceEqual(DefaultColors);
		}
		return true;
	}

	private bool ShouldSerializeZeroColor()
	{
		return ZeroColor != Color.Transparent;
	}

	private bool ShouldSerializeMode()
	{
		return Mode != StiHeatmapFillMode.Lightness;
	}

	public bool IsDefault()
	{
		if (!ShouldSerializeColor() && !ShouldSerializeZeroColor())
		{
			return !ShouldSerializeMode();
		}
		return false;
	}

	public StiHeatmapWithGroupStyleData()
	{
		Colors = DefaultColors.Clone() as Color[];
	}

	[StiUniversalConstructor("HeatmapWithGroup")]
	public StiHeatmapWithGroupStyleData(Color[] colors, Color zeroColor, StiHeatmapFillMode mode)
	{
		Colors = colors;
		ZeroColor = zeroColor;
		Mode = mode;
	}
}
