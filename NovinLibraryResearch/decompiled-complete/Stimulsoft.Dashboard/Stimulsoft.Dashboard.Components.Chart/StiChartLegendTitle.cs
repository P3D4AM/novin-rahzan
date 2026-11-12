using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartLegendTitle : IStiExpressionParentComponent
{
	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for title drawing.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public Color Color { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or set font which will be used for axis title drawing.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 12f, (FontStyle)1);


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets title text.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Text { get; set; } = "";


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Font != null && !ShouldSerializeFont() && !ShouldSerializeColor() && Text != null)
			{
				return Text.Length == 0;
			}
			return false;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public StiComponent ParentComponent { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFontArial12Bold("Font", Font);
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "Text":
				Text = item.DeserializeString();
				break;
			case "Color":
				Color = item.DeserializeColor();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartLegendTitle stiChartLegendTitle = MemberwiseClone() as StiChartLegendTitle;
		object obj = Font.Clone();
		stiChartLegendTitle.Font = (Font)((obj is Font) ? obj : null);
		return stiChartLegendTitle;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Transparent;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 12f)
		{
			return (int)Font.Style != 1;
		}
		return true;
	}

	public StiChartLegendTitle()
	{
	}//IL_0017: Unknown result type (might be due to invalid IL or missing references)
	//IL_0021: Expected O, but got Unknown


	[StiUniversalConstructor("Title")]
	public StiChartLegendTitle(Font font, string text, Color color)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Font = font;
		Text = text;
		Color = color;
	}
}
