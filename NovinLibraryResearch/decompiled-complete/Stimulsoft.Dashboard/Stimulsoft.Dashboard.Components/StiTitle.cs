using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components;

[JsonObject]
[TypeConverter(typeof(StiTitleConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiTitle : ICloneable, IStiJsonReportObject, IStiTitle, IStiFont, IStiHorAlignment, IStiForeColor, IStiBackColor
{
	[StiSerializable]
	[DefaultValue(StiHorAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets title text alignment.")]
	[StiCategory("Title", StiCategorySource.PropertyCategory)]
	[StiOrder(250)]
	public StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Left;


	[StiSerializable]
	[Description("Gets or set font which will be used for title drawing.")]
	[StiCategory("Title", StiCategorySource.PropertyCategory)]
	[StiOrder(180)]
	public Font Font { get; set; } = new Font("Arial", 11f);


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a back color.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(110)]
	[StiPropertyLevel(StiLevel.Basic)]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The foreground color of this component, which is used to display text.")]
	[StiOrder(210)]
	public Color ForeColor { get; set; } = Color.Transparent;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets title text.")]
	[StiCategory("Title", StiCategorySource.PropertyCategory)]
	[StiOrder(290)]
	public string Text { get; set; } = "";


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory("Title", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of title.")]
	[StiOrder(340)]
	public bool Visible { get; set; } = true;


	[Description("Specifies how the text output in the component area depends on the text size.")]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[StiOrder(165)]
	public StiTextSizeMode SizeMode { get; set; } = StiTextSizeMode.Fit;


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyBool("Visible", Visible, defaultValue: true);
		jObject.AddPropertyFontArial11("Font", Font);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Left);
		jObject.AddPropertyEnum("SizeMode", SizeMode, StiTextSizeMode.Fit);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Text":
				Text = item.DeserializeString();
				break;
			case "Visible":
				Visible = item.DeserializeBool();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "SizeMode":
				SizeMode = item.DeserializeEnum<StiTextSizeMode>();
				break;
			}
		}
	}

	internal static StiTitle CreateFromJsonObject(JObject jObject)
	{
		StiTitle stiTitle = new StiTitle();
		stiTitle.LoadFromJsonObject(jObject);
		return stiTitle;
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 11f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.Transparent;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	public override bool Equals(object obj)
	{
		StiTitle stiTitle = (StiTitle)obj;
		if (stiTitle != null && stiTitle.BackColor == BackColor && ((object)stiTitle.Font).GetHashCode() == ((object)Font).GetHashCode() && stiTitle.ForeColor == ForeColor && stiTitle.HorAlignment == HorAlignment && stiTitle.Text == Text)
		{
			return stiTitle.Visible == Visible;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public StiTitle()
	{
	}//IL_0012: Unknown result type (might be due to invalid IL or missing references)
	//IL_001c: Expected O, but got Unknown


	public StiTitle(string text, Color foreColor, Color backColor, Font font, StiHorAlignment alignment, bool visible, StiTextSizeMode sizeMode)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Text = text;
		ForeColor = foreColor;
		BackColor = backColor;
		Font = font;
		HorAlignment = alignment;
		Visible = visible;
		SizeMode = sizeMode;
	}
}
