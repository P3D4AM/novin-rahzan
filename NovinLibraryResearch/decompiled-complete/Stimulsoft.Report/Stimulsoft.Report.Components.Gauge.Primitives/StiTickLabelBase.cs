using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiTickLabelBase : StiTickBase
{
	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the format used to convert value to a string.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string TextFormat { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	internal StiFormatService FormatService { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("The brush, which is used to display text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Black);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font Font { get; set; } = new Font("Arial", 10f);


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextFormat", TextFormat);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyFontArial10(val, "Font", Font);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TextFormat":
				TextFormat = StiJsonReportObjectHelper.DeserializeString(item);
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
		StiTickLabelBase stiTickLabelBase = (StiTickLabelBase)base.Clone();
		stiTickLabelBase.TextBrush = (StiBrush)TextBrush.Clone();
		return stiTickLabelBase;
	}

	protected string GetTextForRender(double value, string format)
	{
		if (!string.IsNullOrEmpty(format))
		{
			return string.Format(format, value);
		}
		return Math.Round(value, 2).ToString();
	}

	protected string GetTextForRender(string value)
	{
		return GetTextForRender(value, TextFormat);
	}

	protected string GetTextForRender(string value, string format)
	{
		if (FormatService != null)
		{
			return FormatService.Format(value);
		}
		if (!string.IsNullOrEmpty(format))
		{
			return string.Format(format, value);
		}
		return value;
	}
}
