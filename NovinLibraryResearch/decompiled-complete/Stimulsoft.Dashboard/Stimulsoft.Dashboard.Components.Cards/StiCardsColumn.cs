using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Cards.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards;

[TypeConverter(typeof(StiCardsColumnConverter))]
public abstract class StiCardsColumn : StiMeter, IStiCardsColumn, IStiHorAlignment, IStiForeColor, IStiTextFormat, IStiFont
{
	[DefaultValue(StiHorAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public virtual StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Center;


	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[TypeConverter(typeof(StiColorConverter))]
	[Description("The foreground color of this component, which is used to display text.")]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ForeColor { get; set; } = Color.Transparent;


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class, StiSerializeTypes.SerializeToAllExceptCode)]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 10f);


	[DefaultValue(StiCardsColumnVisibility.True)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public virtual StiCardsColumnVisibility Visibility { get; set; }

	[DefaultValue(null)]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public virtual string VisibilityExpression { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyEnum("Visibility", Visibility, StiTableColumnVisibility.True);
		jObject.AddPropertyFontArial10("Font", Font);
		if (VisibilityExpression != null)
		{
			jObject.AddPropertyStringNullOrEmpty("VisibilityExpression", VisibilityExpression);
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Visibility":
				Visibility = item.DeserializeEnum<StiCardsColumnVisibility>();
				break;
			case "VisibilityExpression":
				VisibilityExpression = item.DeserializeString();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			}
		}
	}

	public override object Clone()
	{
		StiCardsColumn stiCardsColumn = ((IStiCardsColumn)base.Clone()) as StiCardsColumn;
		stiCardsColumn.TextFormat = TextFormat.Clone() as StiFormatService;
		return stiCardsColumn;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 10f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		uniqueCode = (uniqueCode * 397) ^ (int)HorAlignment;
		uniqueCode = (uniqueCode * 397) ^ ((TextFormat != null) ? TextFormat.GetHashCode() : 0);
		uniqueCode = (uniqueCode * 397) ^ Visibility.GetHashCode();
		if (VisibilityExpression != null)
		{
			uniqueCode = (uniqueCode * 397) ^ VisibilityExpression.GetHashCode();
		}
		return uniqueCode;
	}

	public StiCardsColumn()
	{
	}//IL_0028: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Expected O, but got Unknown


	public StiCardsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiCardsColumnVisibility visibility, string visibilityExpression, Color foreColor)
		: base(key, expression, label)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		HorAlignment = horAlignment;
		TextFormat = textFormat;
		Visibility = visibility;
		VisibilityExpression = visibilityExpression;
		ForeColor = foreColor;
	}
}
