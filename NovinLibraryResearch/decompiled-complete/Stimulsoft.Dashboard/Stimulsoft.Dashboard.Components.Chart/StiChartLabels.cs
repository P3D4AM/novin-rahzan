using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartLabels : IStiJsonReportObject, IStiChartLabels
{
	public enum Order
	{
		AutoRotate = 1,
		BackColor,
		Font,
		ForeColor,
		Position,
		Style,
		TextAfter,
		TextBefore
	}

	public StiChartElement element;

	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiChartLabelsPositionConverter))]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartLabelsPositionEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a position of the labels.")]
	[StiOrder(5)]
	[RefreshProperties(RefreshProperties.All)]
	public virtual StiChartLabelsPosition Position
	{
		get
		{
			if (!element.IsStackedChart && AxisPosition == StiChartLabelsPosition.Total)
			{
				AxisPosition = StiChartLabelsPosition.None;
				return StiChartLabelsPosition.None;
			}
			if (element.IsPieChart)
			{
				return PiePosition;
			}
			if (element.IsPie3dChart)
			{
				return Pie3dPosition;
			}
			if (element.IsDoughnutChart || element.IsSunburstChart)
			{
				return DoughnutPosition;
			}
			if (element.IsFunnelChart)
			{
				return FunnelPosition;
			}
			if (element.IsTreemapChart)
			{
				return TreemapPosition;
			}
			if (element.IsRadarChart)
			{
				return RadarPosition;
			}
			return AxisPosition;
		}
		set
		{
			if (element.IsPieChart)
			{
				PiePosition = value;
			}
			else if (element.IsPie3dChart)
			{
				Pie3dPosition = value;
			}
			else if (element.IsDoughnutChart || element.IsSunburstChart)
			{
				DoughnutPosition = value;
			}
			else if (element.IsFunnelChart)
			{
				FunnelPosition = value;
			}
			else if (element.IsTreemapChart)
			{
				TreemapPosition = value;
			}
			else if (element.IsRadarChart)
			{
				RadarPosition = value;
			}
			else
			{
				AxisPosition = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.None)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition AxisPosition { get; set; }

	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.Center)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition PiePosition { get; set; } = StiChartLabelsPosition.Center;


	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.Center)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition Pie3dPosition { get; set; } = StiChartLabelsPosition.Center;


	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.Center)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition DoughnutPosition { get; set; } = StiChartLabelsPosition.Center;


	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.Center)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition FunnelPosition { get; set; } = StiChartLabelsPosition.Center;


	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.Center)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition TreemapPosition { get; set; } = StiChartLabelsPosition.Center;


	[StiSerializable]
	[DefaultValue(StiChartLabelsPosition.None)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public virtual StiChartLabelsPosition RadarPosition { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which enables or disables auto rotate mode drawing of the labels.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public virtual bool AutoRotate { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a color which will be used for the labels drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(4)]
	public Color ForeColor { get; set; } = Color.Transparent;


	[Browsable(false)]
	[Obsolete("The 'Labels.Color' property is obsolete. Please use the 'Labels.ForeColor' property instead of.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Color Color
	{
		get
		{
			return ForeColor;
		}
		set
		{
			ForeColor = value;
		}
	}

	[StiSerializable]
	[Description("Gets or set a font which will be used for the labels drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(StiChartLabelsStyle.CategoryValue)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a style of the labels.")]
	[StiOrder(6)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartLabelsStyleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiChartLabelsStyle Style { get; set; } = StiChartLabelsStyle.CategoryValue;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output after the labels string representation.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(7)]
	public string TextAfter { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output before the labels string representation.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(8)]
	public string TextBefore { get; set; } = "";


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (ForeColor == Color.Transparent && !ShouldSerializeFont() && !AutoRotate && AxisPosition == StiChartLabelsPosition.None && Pie3dPosition == StiChartLabelsPosition.Center && PiePosition == StiChartLabelsPosition.Center && DoughnutPosition == StiChartLabelsPosition.Center && FunnelPosition == StiChartLabelsPosition.Center && TreemapPosition == StiChartLabelsPosition.Center && RadarPosition == StiChartLabelsPosition.None && Style == StiChartLabelsStyle.CategoryValue && TextAfter != null && TextAfter.Length == 0 && TextBefore != null)
			{
				return TextBefore.Length == 0;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyStringNullOrEmpty("TextAfter", TextAfter);
		jObject.AddPropertyStringNullOrEmpty("TextBefore", TextBefore);
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyEnum("Position", Position);
		jObject.AddPropertyEnum("Style", Style, StiChartLabelsStyle.CategoryValue);
		jObject.AddPropertyBool("AutoRotate", AutoRotate);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "TextAfter":
				TextAfter = item.DeserializeString();
				break;
			case "TextBefore":
				TextBefore = item.DeserializeString();
				break;
			case "Color":
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Position":
				Position = item.DeserializeEnum<StiChartLabelsPosition>();
				break;
			case "Style":
				Style = item.DeserializeEnum<StiChartLabelsStyle>();
				break;
			case "AutoRotate":
				AutoRotate = item.DeserializeBool();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartLabels stiChartLabels = MemberwiseClone() as StiChartLabels;
		object obj = Font.Clone();
		stiChartLabels.Font = (Font)((obj is Font) ? obj : null);
		return stiChartLabels;
	}

	private bool ShouldSerializePosition()
	{
		if ((!element.IsPieChart || PiePosition == StiChartLabelsPosition.Center) && (!element.IsPie3dChart || Pie3dPosition == StiChartLabelsPosition.Center) && (!element.IsDoughnutChart || DoughnutPosition == StiChartLabelsPosition.Center) && (!element.IsFunnelChart || FunnelPosition == StiChartLabelsPosition.Center) && (!element.IsTreemapChart || TreemapPosition == StiChartLabelsPosition.Center) && (!element.IsRadarChart || RadarPosition == StiChartLabelsPosition.None))
		{
			if (element.IsAxisAreaChart)
			{
				return AxisPosition != StiChartLabelsPosition.None;
			}
			return false;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	public StiChartLabels()
	{
	}//IL_0039: Unknown result type (might be due to invalid IL or missing references)
	//IL_0043: Expected O, but got Unknown


	public StiChartLabels(StiChartLabelsPosition position, StiChartLabelsStyle style, Font font, Color foreColor, string textBefore, string textAfter, bool autoRotate)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		Position = position;
		Style = style;
		Font = font;
		ForeColor = foreColor;
		TextBefore = textBefore;
		TextAfter = textAfter;
		AutoRotate = autoRotate;
	}

	[StiUniversalConstructor("Labels")]
	public StiChartLabels(StiChartLabelsPosition axisPosition, StiChartLabelsPosition piePosition, StiChartLabelsPosition pie3dPosition, StiChartLabelsPosition doughnutPosition, StiChartLabelsPosition funnelPosition, StiChartLabelsPosition treemapPosition, StiChartLabelsPosition radarPosition, StiChartLabelsStyle style, Font font, Color foreColor, string textBefore, string textAfter, bool autoRotate)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		AxisPosition = axisPosition;
		PiePosition = piePosition;
		Pie3dPosition = pie3dPosition;
		DoughnutPosition = doughnutPosition;
		FunnelPosition = funnelPosition;
		TreemapPosition = treemapPosition;
		RadarPosition = radarPosition;
		Style = style;
		Font = font;
		ForeColor = foreColor;
		TextBefore = textBefore;
		TextAfter = textAfter;
		AutoRotate = autoRotate;
	}
}
