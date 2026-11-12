using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards;

public class StiBubbleCardsColumn : StiMeasureCardsColumn, IStiBubbleColumn
{
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public bool AllowCustomColors { get; set; }

	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[TypeConverter(typeof(StiColorConverter))]
	[Browsable(false)]
	[DefaultValue(null)]
	public Color PositiveColor { get; set; } = StiColor.Get("537eb6");


	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[TypeConverter(typeof(StiColorConverter))]
	[Browsable(false)]
	[DefaultValue(null)]
	public Color NegativeColor { get; set; } = StiColor.Get("ff0000");


	[DefaultValue(StiHorAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public override StiHorAlignment HorAlignment
	{
		get
		{
			return base.HorAlignment;
		}
		set
		{
			base.HorAlignment = value;
		}
	}

	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.BubbleCardsColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Chart", "Bubble");

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && HorAlignment == StiHorAlignment.Center && !AllowCustomColors && !ShouldSerializePositiveColor())
			{
				return !ShouldSerializeNegativeColor();
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		jObject.AddPropertyBool("AllowCustomColors", AllowCustomColors);
		jObject.AddPropertyColor("PositiveColor", PositiveColor, StiColor.Get("537eb6"));
		jObject.AddPropertyColor("NegativeColor", NegativeColor, StiColor.Get("ff0000"));
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
			case "AllowCustomColors":
				AllowCustomColors = item.DeserializeBool();
				break;
			case "PositiveColor":
				PositiveColor = item.DeserializeColor();
				break;
			case "NegativeColor":
				NegativeColor = item.DeserializeColor();
				break;
			}
		}
	}

	private bool ShouldSerializePositiveColor()
	{
		return PositiveColor != StiColor.Get("537eb6");
	}

	private bool ShouldSerializeNegativeColor()
	{
		return NegativeColor != StiColor.Get("ff0000");
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		return (uniqueCode * 397) ^ (int)HorAlignment;
	}

	public StiBubbleCardsColumn()
	{
	}

	public StiBubbleCardsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiCardsColumnVisibility visibility, string visibilityExpression, Color foreColor, bool allowCustomColors, Color positiveColor, Color negativeColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor)
	{
		AllowCustomColors = allowCustomColors;
		PositiveColor = positiveColor;
		NegativeColor = negativeColor;
	}
}
