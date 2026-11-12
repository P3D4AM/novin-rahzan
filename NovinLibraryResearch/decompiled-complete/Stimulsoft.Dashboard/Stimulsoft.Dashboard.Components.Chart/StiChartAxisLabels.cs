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
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartAxisLabels : IStiChartAxisLabels, ICloneable, IStiFont, IStiJsonReportObject
{
	public enum Order
	{
		Angle = 1,
		Color,
		Font,
		Placement,
		Step,
		TextAfter,
		TextAlignment,
		TextBefore
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[Description("Gets or sets angle of label rotation.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public float Angle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color of labels drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public Color Color { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or sets font which will be used for axis label drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(StiLabelsPlacement.AutoRotation)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or set mode of labels placement on axis.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(4)]
	public StiLabelsPlacement Placement { get; set; } = StiLabelsPlacement.AutoRotation;


	[StiSerializable]
	[DefaultValue(StiHorAlignment.Right)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets label text alignment.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(7)]
	public virtual StiHorAlignment TextAlignment { get; set; } = StiHorAlignment.Right;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output after argument string representation.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(6)]
	public string TextAfter { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output before argument string representation.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(8)]
	public string TextBefore { get; set; } = "";


	[StiSerializable]
	[DefaultValue(0f)]
	[Description("Gets or sets value which indicates with what steps do labels be shown on axis.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(5)]
	public float Step { get; set; }

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeFont() && !ShouldSerializeColor() && Placement == StiLabelsPlacement.AutoRotation && TextAlignment == StiHorAlignment.Right && TextAfter == "" && TextBefore == "" && Angle == 0f)
			{
				return Step == 0f;
			}
			return false;
		}
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFloat("Angle", Angle);
		jObject.AddPropertyStringNullOrEmpty("TextBefore", TextBefore);
		jObject.AddPropertyStringNullOrEmpty("TextAfter", TextAfter);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyEnum("Placement", Placement, StiLabelsPlacement.AutoRotation);
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		jObject.AddPropertyEnum("TextAlignment", TextAlignment, StiHorAlignment.Right);
		jObject.AddPropertyFloat("Step", Step);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Angle":
				Angle = item.DeserializeFloat();
				break;
			case "TextBefore":
				TextBefore = item.DeserializeString();
				break;
			case "TextAfter":
				TextAfter = item.DeserializeString();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "Placement":
				Placement = item.DeserializeEnum<StiLabelsPlacement>();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			case "TextAlignment":
				TextAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "Step":
				Step = item.DeserializeFloat();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartAxisLabels stiChartAxisLabels = MemberwiseClone() as StiChartAxisLabels;
		object obj = Font.Clone();
		stiChartAxisLabels.Font = (Font)((obj is Font) ? obj : null);
		return stiChartAxisLabels;
	}

	protected bool ShouldSerializeColor()
	{
		return Color != Color.Transparent;
	}

	protected bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	public StiChartAxisLabels()
	{
	}//IL_0016: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Expected O, but got Unknown


	[StiUniversalConstructor("Labels")]
	public StiChartAxisLabels(string textBefore, string textAfter, float angle, Font font, StiLabelsPlacement placement, Color color, StiHorAlignment textAlignment)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		TextBefore = textBefore;
		TextAfter = textAfter;
		Angle = angle;
		Font = font;
		Placement = placement;
		Color = color;
		TextAlignment = textAlignment;
	}
}
