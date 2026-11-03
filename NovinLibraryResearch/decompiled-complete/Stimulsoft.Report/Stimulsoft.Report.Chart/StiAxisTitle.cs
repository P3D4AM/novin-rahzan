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
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiAxisTitle : IStiAxisTitle, ICloneable, IStiJsonReportObject, IStiDefault, IStiFont
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Invalid comparison between Unknown and I4
			if (AllowApplyStyle && !ShouldSerializeFont() && Text != null && Text.Length == 0 && Antialiasing && (int)Alignment == 1 && Position == StiTitlePosition.Outside)
			{
				return Direction == StiDirection.LeftToRight;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiAxisTitleCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle { get; set; } = true;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or set font which will be used for axis title drawing.")]
	public Font Font { get; set; } = new Font("Tahoma", 12f, (FontStyle)1);


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets title text.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for title drawing.")]
	public Color Color { get; set; } = Color.Black;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode.")]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets title text alignment.")]
	public StringAlignment Alignment { get; set; } = (StringAlignment)1;


	[StiSerializable]
	[DefaultValue(StiTitlePosition.Outside)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets title text position.")]
	public StiTitlePosition Position { get; set; } = StiTitlePosition.Outside;


	[StiSerializable]
	[DefaultValue(StiDirection.LeftToRight)]
	[Description("Gets or set text direction for axis title drawing.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiDirection Direction { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyFontTahoma12Bold(val, "Font", Font);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Black);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyEnum(val, "Alignment", (Enum)(object)Alignment, (Enum)(object)(StringAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "Position", (Enum)Position, (Enum)StiTitlePosition.Outside);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiDirection.LeftToRight);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Alignment":
				Alignment = StiJsonReportObjectHelper.DeserializeEnum<StringAlignment>(item);
				break;
			case "Position":
				Position = StiJsonReportObjectHelper.DeserializeEnum<StiTitlePosition>(item);
				break;
			case "Direction":
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiDirection>(item);
				break;
			}
		}
	}

	public object Clone()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		IStiAxisTitle stiAxisTitle = MemberwiseClone() as IStiAxisTitle;
		stiAxisTitle.Alignment = Alignment;
		stiAxisTitle.Direction = Direction;
		object obj = Font.Clone();
		stiAxisTitle.Font = (Font)((obj is Font) ? obj : null);
		if (Core != null)
		{
			stiAxisTitle.Core = Core.Clone() as StiAxisTitleCoreXF;
			stiAxisTitle.Core.Title = stiAxisTitle;
		}
		return stiAxisTitle;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Tahoma" && Font.SizeInPoints == 12f)
		{
			return (int)Font.Style != 1;
		}
		return true;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Black;
	}

	public StiAxisTitle()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Core = new StiAxisTitleCoreXF(this);
	}

	public StiAxisTitle(Font font, string text, Color color, bool antialiasing, StringAlignment alignment, StiDirection direction, bool allowApplyStyle)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Font = font;
		Text = text;
		Color = color;
		Antialiasing = antialiasing;
		Alignment = alignment;
		Direction = direction;
		AllowApplyStyle = allowApplyStyle;
		Core = new StiAxisTitleCoreXF(this);
	}

	[StiUniversalConstructor("Title")]
	public StiAxisTitle(Font font, string text, Color color, bool antialiasing, StringAlignment alignment, StiDirection direction, bool allowApplyStyle, StiTitlePosition position)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Font = font;
		Text = text;
		Color = color;
		Antialiasing = antialiasing;
		Alignment = alignment;
		Direction = direction;
		AllowApplyStyle = allowApplyStyle;
		Position = position;
		Core = new StiAxisTitleCoreXF(this);
	}
}
