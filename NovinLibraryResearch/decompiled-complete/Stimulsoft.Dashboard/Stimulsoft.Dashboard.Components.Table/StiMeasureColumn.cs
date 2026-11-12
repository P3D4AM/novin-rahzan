using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table;

public class StiMeasureColumn : StiTableColumn, IStiMeasureMeter, IStiMeter, IStiMeasureColumn
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.MeasureColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "Indicator");

	public StiMeasureColumn()
	{
	}

	public StiMeasureColumn(string expression)
	{
		base.Expression = expression;
	}

	public StiMeasureColumn(string expression, string label)
	{
		base.Expression = expression;
		base.Label = label;
	}

	public StiMeasureColumn(string key, string expression, string label)
		: base(key, expression, label)
	{
	}

	public StiMeasureColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label, horAlignment, textFormat)
	{
	}

	public StiMeasureColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression)
	{
	}

	public StiMeasureColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiMeasureColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiMeasureColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiMeasureColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, StiTableColumnDashboardInteraction interaction)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment, interaction)
	{
	}
}
