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

public class StiIndicatorColumn : StiMeasureColumn, IStiIndicatorColumn
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.IndicatorColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "Dimension");

	public StiIndicatorColumn()
	{
	}

	public StiIndicatorColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label, horAlignment, textFormat)
	{
	}

	public StiIndicatorColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibleExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibleExpression)
	{
	}

	public StiIndicatorColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiIndicatorColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiIndicatorColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiIndicatorColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, StiTableColumnDashboardInteraction interaction)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment, interaction)
	{
	}
}
