using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiMarkerBase : StiIndicatorBase, IStiGaugeMarker
{
	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets offset of the scale object from the scale bar.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Offset { get; set; }

	[DefaultValue(0.05f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets relative width of the indicator. It is given as part of the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float RelativeWidth { get; set; } = 0.05f;


	[DefaultValue(0.05f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets relative height of the indicator. It is given as part of the scale dimension.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float RelativeHeight { get; set; } = 0.05f;


	[DefaultValue(StiMarkerSkin.Diamond)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiMarkerSkin Skin { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	[StiSerializable]
	public StiGaugeElementSkin CustomSkin { get; set; }

	[DefaultValue("{0:F0}")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the format string for the ShowValue property.")]
	public string Format { get; set; } = "{0:F0}";


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value indicating whether to display a current value of the indicator.")]
	public bool ShowValue { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("The brush, which is used to display text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.DimGray);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font Font { get; set; } = new Font("Arial", 7f);


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Offset", Offset, 0f);
		JsonExtensions.AddPropertyFloat(val, "RelativeWidth", RelativeWidth, 0.05f);
		JsonExtensions.AddPropertyFloat(val, "RelativeHeight", RelativeHeight, 0.05f);
		JsonExtensions.AddPropertyEnum(val, "Skin", (Enum)Skin);
		JsonExtensions.AddPropertyString(val, "Format", Format);
		JsonExtensions.AddPropertyBool(val, "ShowValue", ShowValue, false, false);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Offset":
				Offset = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeWidth":
				RelativeWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeHeight":
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Skin":
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiMarkerSkin>(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ShowValue":
				ShowValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			}
		}
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		StiMarkerBase stiMarkerBase = (StiMarkerBase)base.Clone();
		stiMarkerBase.TextBrush = (StiBrush)TextBrush.Clone();
		return stiMarkerBase;
	}

	protected StiGaugeElementSkin GetActualSkin()
	{
		if (CustomSkin == null)
		{
			return StiGaugeSkinHelper.GetMarkerSkin(Skin);
		}
		return CustomSkin;
	}
}
