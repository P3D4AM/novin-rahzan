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
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiSparklinesColumnConverter))]
public class StiSparklinesColumn : StiMeasureColumn, IStiSparklinesColumn
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
	public override StiMeterIdent Ident { get; } = StiMeterIdent.SparklinesColumn;


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
		jObject.AddPropertyColor("PositiveColor", PositiveColor, "#537eb6");
		jObject.AddPropertyColor("NegativeColor", NegativeColor, "#ff0000");
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
		uniqueCode = (uniqueCode * 397) ^ Type.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ ShowHighLowPoints.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ ShowFirstLastPoints.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ AllowCustomColors.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ PositiveColor.GetHashCode();
		return (uniqueCode * 397) ^ NegativeColor.GetHashCode();
	}

	public StiSparklinesColumn()
	{
	}

	public StiSparklinesColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints)
		: base(expression, label, horAlignment, textFormat)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
	}

	public StiSparklinesColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
	}

	public StiSparklinesColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
	}

	public StiSparklinesColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
	}

	public StiSparklinesColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
	}

	public StiSparklinesColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiSparklinesType type, bool showHighLowPoints, bool showFirstLastPoints, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, bool allowCustomColors, Color positiveColor, Color negativeColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		Type = type;
		ShowHighLowPoints = showHighLowPoints;
		ShowFirstLastPoints = showFirstLastPoints;
		AllowCustomColors = allowCustomColors;
		PositiveColor = positiveColor;
		NegativeColor = negativeColor;
	}
}
