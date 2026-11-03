using System.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public interface IStiCellIndicatorStyle
{
	Color CellIndicatorPositive { get; set; }

	Color CellIndicatorNegative { get; set; }

	Color CellIndicatorNeutral { get; set; }
}
