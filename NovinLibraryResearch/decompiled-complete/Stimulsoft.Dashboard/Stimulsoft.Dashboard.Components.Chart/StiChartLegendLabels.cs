using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartLegendLabels : ICloneable, IStiChartLegendLabels, IStiFont, IStiJsonReportObject
{
	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of legend labels drawing.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public Color Color { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or sets font which will be used for legend label drawing.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiChartSeriesLabelsValueTypeConverter))]
	[Description("Gets or sets which type of information will be shown in legend.")]
	[StiOrder(3)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiChartSeriesLabelsValueTypeEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiSeriesLabelsValueType? ValueType { get; set; }

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!ShouldSerializeFont() && !ShouldSerializeColor())
			{
				return !ValueType.HasValue;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		if (ValueType.HasValue)
		{
			jObject.AddPropertyEnum("ValueType", ValueType);
		}
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
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "ValueType":
				ValueType = item.DeserializeEnum<StiSeriesLabelsValueType>();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartLegendLabels stiChartLegendLabels = MemberwiseClone() as StiChartLegendLabels;
		object obj = Font.Clone();
		stiChartLegendLabels.Font = (Font)((obj is Font) ? obj : null);
		return stiChartLegendLabels;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Transparent;
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

	public StiChartLegendLabels()
	{
	}//IL_0016: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Expected O, but got Unknown


	public StiChartLegendLabels(Font font, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Font = font;
		Color = color;
	}

	[StiUniversalConstructor("Labels")]
	public StiChartLegendLabels(Font font, Color color, StiSeriesLabelsValueType? valueType)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Font = font;
		Color = color;
		ValueType = valueType;
	}
}
