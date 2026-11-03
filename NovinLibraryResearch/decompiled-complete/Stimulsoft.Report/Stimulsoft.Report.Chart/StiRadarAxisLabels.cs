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
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiRadarAxisLabels : IStiRadarAxisLabels, ICloneable, IStiJsonReportObject, IStiFont
{
	[Browsable(false)]
	public StiRadarAxisLabelsCoreXF Core { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that Axis Labels will be rotated.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool RotationLabels { get; set; } = true;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output before argument string representation.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string TextBefore { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output after argument string representation.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string TextAfter { get; set; } = "";


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that label border will be shown.")]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool DrawBorder { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets os sets format string which is used for formating argument values.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string Format { get; set; } = "";


	[StiSerializable]
	[Description("Gets or sets font which will be used for axis label drawing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Tahoma", 8f);


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of labels drawing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color Color { get; set; } = Color.Black;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of labels drawing.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color BorderColor { get; set; } = Color.Black;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill label area.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Gainsboro);


	[DefaultValue(0f)]
	[StiSerializable]
	[Description("Gets or sets fixed width of axis labels.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public float Width { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool WordWrap { get; set; }

	[StiSerializable]
	[StiOrder(240)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool PreventIntersection { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "RotationLabels", RotationLabels, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBefore", TextBefore);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextAfter", TextAfter);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "DrawBorder", DrawBorder, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyFontTahoma8(val, "Font", Font);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Black);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Black);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyFloat(val, "Width", Width, 0f);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		JsonExtensions.AddPropertyBool(val, "PreventIntersection", PreventIntersection, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "PreventIntersection":
				PreventIntersection = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RotationLabels":
				RotationLabels = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TextBefore":
				TextBefore = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TextAfter":
				TextAfter = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DrawBorder":
				DrawBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Width":
				Width = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		IStiRadarAxisLabels stiRadarAxisLabels = MemberwiseClone() as IStiRadarAxisLabels;
		object obj = Font.Clone();
		stiRadarAxisLabels.Font = (Font)((obj is Font) ? obj : null);
		object obj2 = Brush.Clone();
		stiRadarAxisLabels.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		if (Core != null)
		{
			stiRadarAxisLabels.Core = Core.Clone() as StiRadarAxisLabelsCoreXF;
			stiRadarAxisLabels.Core.Labels = stiRadarAxisLabels;
		}
		return stiRadarAxisLabels;
	}

	public StiRadarAxisLabels()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Core = new StiRadarAxisLabelsCoreXF(this);
	}

	public StiRadarAxisLabels(string format, Font font, bool antialiasing, bool drawBorder, Color color, Color borderColor, StiBrush brush, bool allowApplyStyle)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Format = format;
		Font = font;
		Antialiasing = antialiasing;
		DrawBorder = drawBorder;
		Color = color;
		BorderColor = borderColor;
		AllowApplyStyle = allowApplyStyle;
		Brush = brush;
		Width = 0f;
		WordWrap = false;
		Core = new StiRadarAxisLabelsCoreXF(this);
	}

	public StiRadarAxisLabels(string format, Font font, bool antialiasing, bool drawBorder, Color color, Color borderColor, StiBrush brush, bool allowApplyStyle, bool rotationLabels)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Format = format;
		Font = font;
		Antialiasing = antialiasing;
		DrawBorder = drawBorder;
		Color = color;
		BorderColor = borderColor;
		AllowApplyStyle = allowApplyStyle;
		Brush = brush;
		RotationLabels = rotationLabels;
		Width = 0f;
		WordWrap = false;
		Core = new StiRadarAxisLabelsCoreXF(this);
	}

	public StiRadarAxisLabels(string format, Font font, bool antialiasing, bool drawBorder, Color color, Color borderColor, StiBrush brush, bool allowApplyStyle, bool rotationLabels, float width, bool wordWrap)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Format = format;
		Font = font;
		Antialiasing = antialiasing;
		DrawBorder = drawBorder;
		Color = color;
		BorderColor = borderColor;
		AllowApplyStyle = allowApplyStyle;
		Brush = brush;
		RotationLabels = rotationLabels;
		Width = width;
		WordWrap = wordWrap;
		Core = new StiRadarAxisLabelsCoreXF(this);
	}

	[StiUniversalConstructor("Labels")]
	public StiRadarAxisLabels(string format, Font font, bool antialiasing, bool drawBorder, Color color, Color borderColor, StiBrush brush, bool allowApplyStyle, bool rotationLabels, float width, bool wordWrap, bool preventIntersection)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Format = format;
		Font = font;
		Antialiasing = antialiasing;
		DrawBorder = drawBorder;
		Color = color;
		BorderColor = borderColor;
		AllowApplyStyle = allowApplyStyle;
		Brush = brush;
		RotationLabels = rotationLabels;
		Width = width;
		WordWrap = wordWrap;
		PreventIntersection = preventIntersection;
		Core = new StiRadarAxisLabelsCoreXF(this);
	}
}
