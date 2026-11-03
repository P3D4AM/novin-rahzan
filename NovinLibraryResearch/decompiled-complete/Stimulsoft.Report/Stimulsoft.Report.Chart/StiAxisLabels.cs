using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisLabels : IStiAxisLabels, ICloneable, IStiJsonReportObject, IStiDefault, IStiFont
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Invalid comparison between Unknown and I4
			if (AllowApplyStyle && Format != null && Format.Length == 0 && Angle == 0f && Width == 0f && TextAfter != null && TextAfter.Length == 0 && TextBefore != null && TextBefore.Length == 0 && !ShouldSerializeFont() && Antialiasing && Placement == StiLabelsPlacement.OneLine && (int)TextAlignment == 3 && Step == 0f)
			{
				return !WordWrap;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiAxisLabelsCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle { get; set; } = true;


	[DefaultValue("")]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets os sets format string which is used for formating argument values.")]
	public string Format { get; set; } = "";


	[DefaultValue(0f)]
	[StiSerializable]
	[Description("Gets or sets angle of label rotation.")]
	public float Angle { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[Description("Gets or sets fixed width of axis labels.")]
	public float Width { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output before argument string representation.")]
	public string TextBefore { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output after argument string representation.")]
	public string TextAfter { get; set; } = "";


	[StiSerializable]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets font which will be used for axis label drawing.")]
	public Font Font { get; set; } = new Font("Tahoma", 8f);


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode.")]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiLabelsPlacement.OneLine)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or set mode of labels placement on axis.")]
	public StiLabelsPlacement Placement { get; set; } = StiLabelsPlacement.OneLine;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of labels drawing.")]
	public Color Color { get; set; } = Color.Black;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets label text alignment.")]
	public virtual StiHorAlignment TextAlignment { get; set; } = (StiHorAlignment)3;


	[StiSerializable]
	[DefaultValue(0f)]
	[Description("Gets or sets value which indicates with what steps do labels be shown on axis.")]
	public virtual float Step { get; set; }

	[Browsable(false)]
	public float CalculatedStep { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	public bool WordWrap { get; set; }

	[Browsable(false)]
	public StiFormatService FormatService { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyFloat(val, "Angle", Angle, 0f);
		JsonExtensions.AddPropertyFloat(val, "Width", Width, 0f);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBefore", TextBefore);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextAfter", TextAfter);
		JsonExtensions.AddPropertyFontTahoma8(val, "Font", Font);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyEnum(val, "Placement", (Enum)Placement, (Enum)StiLabelsPlacement.OneLine);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "TextAlignment", (Enum)(object)TextAlignment, (Enum)(object)(StiHorAlignment)3);
		JsonExtensions.AddPropertyFloat(val, "Step", Step, 0f);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Angle":
				Angle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Width":
				Width = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "TextBefore":
				TextBefore = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TextAfter":
				TextAfter = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Placement":
				Placement = StiJsonReportObjectHelper.DeserializeEnum<StiLabelsPlacement>(item);
				break;
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TextAlignment":
				TextAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiHorAlignment>(item);
				break;
			case "Step":
				Step = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		IStiAxisLabels stiAxisLabels = MemberwiseClone() as IStiAxisLabels;
		stiAxisLabels.Placement = Placement;
		object obj = Font.Clone();
		stiAxisLabels.Font = (Font)((obj is Font) ? obj : null);
		if (Core != null)
		{
			stiAxisLabels.Core = Core.Clone() as StiAxisLabelsCoreXF;
			stiAxisLabels.Core.Labels = stiAxisLabels;
		}
		return stiAxisLabels;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Tahoma" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Black;
	}

	public StiAxisLabels()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		Core = new StiAxisLabelsCoreXF(this);
	}

	public StiAxisLabels(string format, string textBefore, string textAfter, float angle, Font font, bool antialiasing, StiLabelsPlacement placement, Color color, float width, StiHorAlignment textAlignment, float step, bool allowApplyStyle)
		: this(format, textBefore, textAfter, angle, font, antialiasing, placement, color, width, textAlignment, step, allowApplyStyle, wordWrap: false)
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)


	[StiUniversalConstructor("Labels")]
	public StiAxisLabels(string format, string textBefore, string textAfter, float angle, Font font, bool antialiasing, StiLabelsPlacement placement, Color color, float width, StiHorAlignment textAlignment, float step, bool allowApplyStyle, bool wordWrap)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		Format = format;
		TextBefore = textBefore;
		TextAfter = textAfter;
		Angle = angle;
		Font = font;
		Antialiasing = antialiasing;
		Placement = placement;
		Color = color;
		Width = width;
		TextAlignment = textAlignment;
		Step = step;
		AllowApplyStyle = allowApplyStyle;
		WordWrap = wordWrap;
		Core = new StiAxisLabelsCoreXF(this);
	}
}
