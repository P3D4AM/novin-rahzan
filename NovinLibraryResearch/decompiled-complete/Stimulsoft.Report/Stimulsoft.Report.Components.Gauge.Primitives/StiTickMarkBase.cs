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

public abstract class StiTickMarkBase : StiTickBase
{
	[StiSerializable]
	[DefaultValue(0.1f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the height factor relative to the height of the basic component.")]
	public float RelativeHeight { get; set; } = 0.1f;


	[DefaultValue(0.1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the width ratio relative to the height of the basic component.")]
	public float RelativeWidth { get; set; } = 0.1f;


	[DefaultValue(StiTickMarkSkin.Rectangle)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the skin of the component rendering.")]
	public StiTickMarkSkin Skin { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	[StiSerializable]
	public StiGaugeElementSkin CustomSkin { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the component.")]
	public StiBrush BorderBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(89, 87, 87));


	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border thickness of the component.")]
	public float BorderWidth { get; set; } = 1f;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "RelativeHeight", RelativeHeight, 0.1f);
		JsonExtensions.AddPropertyFloat(val, "RelativeWidth", RelativeWidth, 0.1f);
		JsonExtensions.AddPropertyEnum(val, "Skin", (Enum)Skin);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "BorderBrush", BorderBrush);
		JsonExtensions.AddPropertyFloat(val, "BorderWidth", BorderWidth, 1f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "RelativeHeight":
				RelativeHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "RelativeWidth":
				RelativeWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Skin":
				Skin = StiJsonReportObjectHelper.DeserializeEnum<StiTickMarkSkin>(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderBrush":
				BorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderWidth":
				BorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiTickMarkBase stiTickMarkBase = (StiTickMarkBase)base.Clone();
		stiTickMarkBase.Brush = (StiBrush)Brush.Clone();
		stiTickMarkBase.BorderBrush = (StiBrush)BorderBrush.Clone();
		return stiTickMarkBase;
	}

	protected StiGaugeElementSkin GetActualSkin()
	{
		if (CustomSkin == null)
		{
			return StiGaugeSkinHelper.GetTickMarkSkin(Skin);
		}
		return CustomSkin;
	}

	protected float GetRelativeWidth(float? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return RelativeWidth;
	}

	protected float GetRelativeHeight(float? value)
	{
		if (value.HasValue)
		{
			return value.Value;
		}
		return RelativeHeight;
	}
}
