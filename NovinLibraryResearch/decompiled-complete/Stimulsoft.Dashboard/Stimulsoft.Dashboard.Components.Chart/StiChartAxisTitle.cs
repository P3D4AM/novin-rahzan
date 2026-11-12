using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiChartAxisTitle : IStiDefault
{
	public enum Order
	{
		Alignment = 1,
		Color,
		Direction,
		Font,
		Placement,
		Position,
		Text,
		Visible
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of axis.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(8)]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets title text alignment.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	public StringAlignment Alignment { get; set; } = (StringAlignment)1;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for title drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(2)]
	public Color Color { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or set font which will be used for axis title drawing.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(4)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 12f, (FontStyle)1);


	[StiSerializable]
	[DefaultValue(StiTitlePosition.Outside)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets title text position.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(6)]
	public StiTitlePosition Position { get; set; } = StiTitlePosition.Outside;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets title text.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(7)]
	public string Text { get; set; } = "";


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			if (Font != null && !ShouldSerializeFont() && !ShouldSerializeColor() && Text != null && Text.Length == 0 && Position == StiTitlePosition.Outside && (int)Alignment == 1)
			{
				return Visible;
			}
			return false;
		}
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		JObject jObject = new JObject();
		jObject.AddPropertyFontArial12Bold("Font", Font);
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		jObject.AddPropertyEnum("Alignment", (Enum)(object)Alignment, (Enum)(object)(StringAlignment)1);
		jObject.AddPropertyEnum("Position", Position, StiTitlePosition.Outside);
		jObject.AddPropertyBool("Visible", Visible, defaultValue: true);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
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
			case "Alignment":
				Alignment = item.DeserializeEnum<StringAlignment>();
				break;
			case "Position":
				Position = item.DeserializeEnum<StiTitlePosition>();
				break;
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartAxisTitle stiChartAxisTitle = MemberwiseClone() as StiChartAxisTitle;
		object obj = Font.Clone();
		stiChartAxisTitle.Font = (Font)((obj is Font) ? obj : null);
		return stiChartAxisTitle;
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

	public StiChartAxisTitle()
	{
	}//IL_0009: Unknown result type (might be due to invalid IL or missing references)
	//IL_0025: Unknown result type (might be due to invalid IL or missing references)
	//IL_002f: Expected O, but got Unknown


	[StiUniversalConstructor("Title")]
	public StiChartAxisTitle(Font font, string text, Color color, StringAlignment alignment, StiTitlePosition position, bool visible)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Font = font;
		Text = text;
		Color = color;
		Alignment = alignment;
		Position = position;
		Visible = visible;
	}
}
