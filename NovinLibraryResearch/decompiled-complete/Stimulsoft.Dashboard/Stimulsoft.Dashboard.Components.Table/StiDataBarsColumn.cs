using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiDataBarsColumnConverter))]
public class StiDataBarsColumn : StiMeasureColumn, IStiDataBarsColumn
{
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
	public override StiMeterIdent Ident { get; } = StiMeterIdent.DataBarsColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "DataBars");

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault)
			{
				return HorAlignment == StiHorAlignment.Center;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "HorAlignment")
			{
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
			}
		}
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		return (uniqueCode * 397) ^ (int)HorAlignment;
	}

	public StiDataBarsColumn()
	{
		HorAlignment = StiHorAlignment.Center;
	}

	public StiDataBarsColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label, horAlignment, textFormat)
	{
	}

	public StiDataBarsColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression)
	{
	}

	public StiDataBarsColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiDataBarsColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiDataBarsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}
}
