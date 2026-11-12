using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Meters;

public interface IStiTableColumn
{
	bool Visible { get; set; }

	bool ShowTotalSummary { get; set; }

	StiSummaryColumnType SummaryType { get; set; }

	StiTableColumnVisibility Visibility { get; set; }

	string VisibilityExpression { get; set; }

	StiHorAlignment SummaryAlignment { get; set; }
}
