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
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards;

public class StiSparklinesCardsColumn : StiMeasureCardsColumn, IStiSparklinesColumn
{
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public bool AllowCustomColors { get; set; }

	[DefaultValue(StiSparklinesType.Area)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public StiSparklinesType Type { get; set; } = StiSparklinesType.Area;


	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public bool ShowHighLowPoints { get; set; }

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Browsable(false)]
	public bool ShowFirstLastPoints { get; set; } = true;


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


	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue(0)]
	public int Height { get; set; }

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && Type == StiSparklinesType.Area && !ShowHighLowPoints && ShowFirstLastPoints && !AllowCustomColors && !ShouldSerializePositiveColor())
			{
				return !ShouldSerializeNegativeColor();
			}
			return false;
		}
	}

	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SparklinesCardsColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "Sparklines");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("Type", Type, StiSparklinesType.Area);
		jObject.AddPropertyBool("ShowHighLowPoints", ShowHighLowPoints);
		jObject.AddPropertyBool("ShowFirstLastPoints", ShowFirstLastPoints, defaultValue: true);
		jObject.AddPropertyBool("AllowCustomColors", AllowCustomColors);
		jObject.AddPropertyInt("Height", Height);
		jObject.AddPropertyStringNullOrEmpty("PositiveColor", StiJsonReportObjectHelper.Serialize.JColor(PositiveColor, StiColor.Get("537eb6")));
		jObject.AddPropertyStringNullOrEmpty("NegativeColor", StiJsonReportObjectHelper.Serialize.JColor(NegativeColor, StiColor.Get("ff0000")));
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Type":
				Type = item.DeserializeEnum<StiSparklinesType>();
				break;
			case "ShowHighLowPoints":
				ShowHighLowPoints = item.DeserializeBool();
				break;
			case "ShowFirstLastPoints":
				ShowFirstLastPoints = item.DeserializeBool();
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
			case "Height":
				Height = item.DeserializeInt();
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

	private bool ShouldSerializeHeight()
	{
		return Height != 0;
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		uniqueCode = (uniqueCode * 397) ^ Type.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ ShowHighLowPoints.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ ShowFirstLastPoints.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ AllowCustomColors.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ PositiveColor.GetHashCode();
		return (uniqueCode * 397) ^ NegativeColor.GetHashCode();
	}

	public StiSparklinesCardsColumn()
	{
	}

	public StiSparklinesCardsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiCardsColumnVisibility visibility, string visibilityExpression, Color foreColor, bool allowCustomColors, Color positiveColor, Color negativeColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
		AllowCustomColors = allowCustomColors;
		PositiveColor = positiveColor;
		NegativeColor = negativeColor;
	}
}
